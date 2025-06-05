using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
	private static List<SkinnedMeshRenderer> smrList = new List<SkinnedMeshRenderer>();

	private static List<MeshFilter> mfList = new List<MeshFilter>();

	public static Shader replaceShader;

	public static void Render(this GameObject go, Vector3 position, Quaternion rotation)
	{
		go.Render(position, rotation, Vector3.one);
	}

	public static void Render(this GameObject go, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		smrList.Clear();
		go.FindComponentsInChildren(ref smrList);
		foreach (SkinnedMeshRenderer smr in smrList)
		{
			GameObject gameObject = smr.gameObject;
			Mesh sharedMesh = smr.sharedMesh;
			Material[] sharedMaterials = smr.GetComponent<Renderer>().sharedMaterials;
			Quaternion q = rotation * gameObject.transform.rotation;
			Matrix4x4 matrix = Matrix4x4.TRS(position + (gameObject.transform.position - go.transform.position), q, new Vector3(gameObject.transform.lossyScale.x * scale.x, gameObject.transform.lossyScale.y * scale.y, gameObject.transform.lossyScale.z * scale.z));
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i] != null && sharedMesh != null)
				{
					Shader shader = sharedMaterials[i].shader;
					if (replaceShader != null)
					{
						sharedMaterials[i].shader = replaceShader;
					}
					sharedMaterials[i].SetPass(0);
					Graphics.DrawMeshNow(sharedMesh, matrix, i);
					sharedMaterials[i].shader = shader;
				}
			}
		}
		mfList.Clear();
		go.FindComponentsInChildren(ref mfList);
		foreach (MeshFilter mf in mfList)
		{
			GameObject gameObject2 = mf.gameObject;
			Mesh sharedMesh2 = mf.sharedMesh;
			Material[] sharedMaterials2 = mf.GetComponent<Renderer>().sharedMaterials;
			Quaternion q2 = rotation * gameObject2.transform.rotation;
			Matrix4x4 matrix2 = Matrix4x4.TRS(position + (gameObject2.transform.position - go.transform.position), q2, new Vector3(gameObject2.transform.lossyScale.x * scale.x, gameObject2.transform.lossyScale.y * scale.y, gameObject2.transform.lossyScale.z * scale.z));
			for (int j = 0; j < sharedMaterials2.Length; j++)
			{
				if (sharedMaterials2[j] != null && sharedMesh2 != null)
				{
					Shader shader2 = sharedMaterials2[j].shader;
					if (replaceShader != null)
					{
						sharedMaterials2[j].shader = replaceShader;
					}
					sharedMaterials2[j].SetPass(0);
					Graphics.DrawMeshNow(sharedMesh2, matrix2, j);
					sharedMaterials2[j].shader = shader2;
				}
			}
		}
	}

	public static T FindComponentInChildren<T>(this GameObject go) where T : Component
	{
		if (go == null)
		{
			return (T)null;
		}
		T val = go.GetComponent<T>();
		for (int i = 0; i < go.transform.childCount; i++)
		{
			if (!(val == null))
			{
				break;
			}
			GameObject gameObject = go.transform.GetChild(i).gameObject;
			val = gameObject.FindComponentInChildren<T>();
		}
		return val;
	}

	public static void FindComponentsInChildren<T>(this GameObject go, ref List<T> array) where T : Component
	{
		if (!(go == null))
		{
			T component = go.GetComponent<T>();
			if (component != null)
			{
				array.Add(component);
			}
			for (int i = 0; i < go.transform.childCount; i++)
			{
				GameObject gameObject = go.transform.GetChild(i).gameObject;
				gameObject.FindComponentsInChildren(ref array);
			}
		}
	}

	public static T FindComponentInParent<T>(this GameObject go) where T : Component
	{
		T val = (T)null;
		if (go != null)
		{
			Transform transform = go.transform;
			while (val == null && transform != null)
			{
				val = transform.GetComponent<T>();
				transform = transform.parent;
			}
		}
		return val;
	}

	public static void SetLayerRecursively(this GameObject go, int newLayer)
	{
		if (go == null)
		{
			return;
		}
		go.layer = newLayer;
		foreach (Transform item in go.transform)
		{
			if (item != null)
			{
				item.gameObject.SetLayerRecursively(newLayer);
			}
		}
	}

	public static void SetSortingLayerRecursively(this GameObject go, int newLayer)
	{
		if (go == null)
		{
			return;
		}
		MeshRenderer component = go.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.sortingLayerID = newLayer;
		}
		Canvas component2 = go.GetComponent<Canvas>();
		if (component2 != null)
		{
			component2.sortingLayerID = newLayer;
		}
		foreach (Transform item in go.transform)
		{
			if (item != null)
			{
				item.gameObject.SetSortingLayerRecursively(newLayer);
			}
		}
	}

	public static List<T> GetAllObjectsInScene<T>() where T : UnityEngine.Object
	{
		List<T> result = new List<T>();
		T[] array = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
		for (int i = 0; i < array.Length; i++)
		{
			T val = array[i];
			if (val.hideFlags == HideFlags.NotEditable || val.hideFlags != HideFlags.HideAndDontSave)
			{
			}
		}
		return result;
	}

	public static List<UnityEngine.Object> GetAllObjectsInScene(Type type)
	{
		List<UnityEngine.Object> result = new List<UnityEngine.Object>();
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(type);
		foreach (UnityEngine.Object obj in array)
		{
			if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags != HideFlags.HideAndDontSave)
			{
			}
		}
		return result;
	}

	public static T GetComponentOnlyInParent<T>(this GameObject go) where T : Component
	{
		return go.GetComponentOnlyInParent(typeof(T)) as T;
	}

	public static Component GetComponentOnlyInParent(this GameObject go, Type type)
	{
		Transform parent = go.transform.parent;
		if (parent != null)
		{
			while (parent != null)
			{
				if (parent.gameObject.activeInHierarchy)
				{
					Component component = parent.gameObject.GetComponent(type);
					if (component != null)
					{
						return component;
					}
				}
				parent = parent.parent;
			}
		}
		return null;
	}

	public static T GetRequiredComponent<T>(this GameObject go, ref T component) where T : Component
	{
		if (component == null)
		{
			component = go.GetComponent<T>();
			if (component == null)
			{
				component = go.AddComponent<T>();
			}
		}
		return component;
	}
}
