using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class tmBatchingManager : SingletonMonoBehaviour<tmBatchingManager>
{
	public const int DYNAMIC_BATCHING_LIMIT = 1024;

	public const int VERTEX_LIMIT = 65535;

	public const int VERTEX_PER_FRAME_LIMIT = 1;

	private Dictionary<string, List<tmBatchedInstance>> batchTable = new Dictionary<string, List<tmBatchedInstance>>();

	private List<tmBatchedInstance> modifiedBatchInstances = new List<tmBatchedInstance>();

	private List<tmBatchedInstance> forcedInstancesToRemove = new List<tmBatchedInstance>();

	public new static tmBatchingManager Instance
	{
		get
		{
			return SingletonMonoBehaviour<tmBatchingManager>.InstanceIfExist;
		}
	}

	public void RegisterModifiedBatchedInstance(tmBatchedInstance batchedInstance)
	{
		if (!modifiedBatchInstances.Contains(batchedInstance))
		{
			modifiedBatchInstances.Add(batchedInstance);
		}
	}

	public void Flush(int vertexLimit = 1)
	{
		if (modifiedBatchInstances.Count <= 0)
		{
			return;
		}
		int num = 0;
		int i;
		for (i = 0; i < modifiedBatchInstances.Count; i++)
		{
			if (num >= vertexLimit)
			{
				break;
			}
			tmBatchedInstance tmBatchedInstance2 = modifiedBatchInstances[i];
			if (!tmBatchedInstance2.IsNull())
			{
				tmBatchedInstance2.Recombine();
				if (!tmBatchedInstance2.ForcedBatching)
				{
					num += tmBatchedInstance2.VertexCount;
				}
			}
		}
		modifiedBatchInstances.RemoveRange(0, i);
		forcedInstancesToRemove.Clear();
		foreach (tmBatchedInstance modifiedBatchInstance in modifiedBatchInstances)
		{
			if (!modifiedBatchInstance.IsNull() && modifiedBatchInstance.ForcedBatching)
			{
				modifiedBatchInstance.Recombine();
				forcedInstancesToRemove.Add(modifiedBatchInstance);
			}
		}
		modifiedBatchInstances.RemoveAll((tmBatchedInstance x) => forcedInstancesToRemove.Contains(x));
		forcedInstancesToRemove.Clear();
	}

	public void StaticBatch(GameObject root)
	{
		tmTextureRenderBase[] componentsInChildren = root.GetComponentsInChildren<tmTextureRenderBase>();
		tmManager.Instance.Flush(componentsInChildren);
		List<tmBatchedInstance> list = new List<tmBatchedInstance>();
		tmBatchObject[] componentsInChildren2 = root.GetComponentsInChildren<tmBatchObject>();
		tmBatchObject[] array = componentsInChildren2;
		foreach (tmBatchObject tmBatchObject2 in array)
		{
			if (tmBatchObject2.BatchInstance == null && tmBatchObject2.BatchingType == tmBatchingType.Static)
			{
				tmBatchedInstance item = Instance.BatchObject(tmBatchObject2, root.transform, false);
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		Flush(int.MaxValue);
		HashSet<string> hashSet = new HashSet<string>();
		for (int j = 0; j < list.Count; j++)
		{
			string text = root.name + "_" + list[j].Mesh.name;
			text = text.Replace("_Batch", string.Empty);
			text = text.Replace("_batch", string.Empty);
			text = text.Substring(0, Mathf.Min(text.Length, 120));
			if (hashSet.Contains(text))
			{
				text = text + "_" + list[j].Mesh.vertexCount;
			}
			else
			{
				hashSet.Add(text);
			}
			list[j].Mesh.name = text;
		}
		foreach (tmBatchedInstance item2 in list)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(item2);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(item2);
			}
		}
		tmBatchObject[] array2 = componentsInChildren2;
		foreach (tmBatchObject tmBatchObject3 in array2)
		{
			if (!tmBatchObject3.IsNull() && tmBatchObject3.BatchingType == tmBatchingType.Static)
			{
				UnityEngine.Object.Destroy(tmBatchObject3.gameObject);
			}
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public void Unbatch(GameObject root)
	{
		tmBatchedInstance[] componentsInChildren = root.GetComponentsInChildren<tmBatchedInstance>();
		tmBatchedInstance[] array = componentsInChildren;
		foreach (tmBatchedInstance tmBatchedInstance2 in array)
		{
			tmBatchedInstance2.Clear();
		}
	}

	public void BatchObject(tmBatchObject render, bool move = true)
	{
		BatchObject(render, base.transform, move);
	}

	public tmBatchedInstance BatchObject(tmBatchObject bo, Transform root, bool move = true)
	{
		if (bo.BatchInstance != null || bo.Mesh == null || bo.Material == null)
		{
			CustomDebug.LogError("invalid batch object : " + bo.name + "  reason: instance:" + (bo.BatchInstance != null) + ", mesh:" + (bo.Mesh == null) + ", material:" + (bo.Material == null), bo);
			return null;
		}
		move = root == null;
		if (root == null)
		{
			root = base.transform;
		}
		bool flag = bo.BatchingType == tmBatchingType.Skinning;
		string key = string.Empty + root.GetInstanceID() + bo.Material.GetInstanceID() + flag;
		List<tmBatchedInstance> batches = null;
		if (batchTable.ContainsKey(key))
		{
			batches = batchTable[key];
		}
		else
		{
			batches = new List<tmBatchedInstance>();
			batchTable.Add(key, batches);
		}
		tmBatchedInstance availibleBatch = null;
		foreach (tmBatchedInstance item in batches)
		{
			if (item.CanAdd(bo))
			{
				availibleBatch = item;
				break;
			}
		}
		if (availibleBatch == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = root;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = "Batch_" + root.name + "_" + bo.Material.name.Replace(tk2dSpriteCollectionData.internalResourcePrefix, string.Empty) + "_" + flag;
			gameObject.layer = bo.gameObject.layer;
			availibleBatch = ((!flag) ? gameObject.AddComponent<tmBatchedInstance>() : gameObject.AddComponent<tmSkinnedBatchedInstance>());
			availibleBatch.shouldMove = move;
			availibleBatch.SharedMaterial = bo.Material;
			tmTextureRenderBase component = bo.GetComponent<tmTextureRenderBase>();
			if ((bool)component)
			{
				tmBatchRender tmBatchRender2 = gameObject.AddComponent<tmBatchRender>();
				tmBatchRender2.MainTexCollectionGUID = component.MainTexCollectionGUID;
				tmBatchRender2.MainTextureID = component.MainTextureID;
				tmBatchRender2.LightmapCollectionGUID = component.LightmapCollectionGUID;
				tmBatchRender2.LightmapTextureID = component.LightmapTextureID;
				tmBatchRender2.Material = component.Material;
			}
			batches.Add(availibleBatch);
			availibleBatch.OnDestroyEvent += delegate
			{
				batches.Remove(availibleBatch);
			};
		}
		if (availibleBatch != null)
		{
			availibleBatch.Add(bo);
		}
		return availibleBatch;
	}
}
