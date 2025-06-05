using System.Collections.Generic;
using UnityEngine;

public class tmSkinnedBatchedInstance : tmBatchedInstance
{
	private Transform[] tempBones;

	private Matrix4x4[] tempBindPoses;

	private BoneWeight[] tempBoneWeights;

	private Dictionary<int, int> bonesPerMesh = new Dictionary<int, int>();

	protected override void Awake()
	{
		base.gameObject.AddComponent<SkinnedMeshRenderer>();
	}

	private void Update()
	{
		Bounds localBounds = default(Bounds);
		if (base.BatchedParts.Count > 0)
		{
			int i = 0;
			for (int count = base.BatchedParts.Count; i < count; i++)
			{
				tmBatchObject tmBatchObject2 = base.BatchedParts[i];
				if (!tmBatchObject2.IsNull())
				{
					Bounds bounds = tmBatchObject2.Mesh.bounds;
					bounds.center += tmBatchObject2.CachedTransform.position;
					localBounds.Encapsulate(bounds);
				}
			}
			localBounds.center -= base.CachedTransform.position;
		}
		base.CachedSkinnedRender.localBounds = localBounds;
	}

	protected override void PostRecombine(CombineInstance[] combos)
	{
		int num = 0;
		int i = 0;
		for (int num2 = combos.Length; i < num2; i++)
		{
			CombineInstance combineInstance = combos[i];
			int instanceID = combineInstance.mesh.GetInstanceID();
			int value;
			if (!bonesPerMesh.TryGetValue(instanceID, out value))
			{
				Matrix4x4[] bindposes = combineInstance.mesh.bindposes;
				bool flag = bindposes != null && bindposes.Length > 0;
				value += ((!flag) ? 1 : bindposes.Length);
				bonesPerMesh.Add(instanceID, value);
			}
			num += value;
		}
		tempBones = ArrayExtention.EnsureLength(tempBones, num);
		tempBindPoses = ArrayExtention.EnsureLength(tempBindPoses, num);
		tempBoneWeights = ArrayExtention.EnsureLength(tempBoneWeights, base.Mesh.vertexCount);
		int num3 = 0;
		int num4 = 0;
		int j = 0;
		for (int count = base.BatchedParts.Count; j < count; j++)
		{
			tmBatchObject tmBatchObject2 = base.BatchedParts[j];
			SkinnedMeshRenderer skinnedMeshRender = tmBatchObject2.SkinnedMeshRender;
			if ((bool)skinnedMeshRender)
			{
				Transform[] bones = skinnedMeshRender.bones;
				BoneWeight[] boneWeights = tmBatchObject2.Mesh.boneWeights;
				Matrix4x4[] bindposes2 = tmBatchObject2.Mesh.bindposes;
				Matrix4x4 matrix4x = tmBatchObject2.CachedTransform.worldToLocalMatrix * base.CachedTransform.localToWorldMatrix;
				int num5 = bones.Length;
				for (int k = 0; k < num5; k++)
				{
					tempBones[num3 + k] = bones[k];
					tempBindPoses[num3 + k] = bindposes2[k] * matrix4x;
				}
				int num6 = boneWeights.Length;
				for (int l = 0; l < num6; l++)
				{
					BoneWeight boneWeight = boneWeights[l];
					boneWeight.boneIndex0 += num3;
					boneWeight.boneIndex1 += num3;
					boneWeight.boneIndex2 += num3;
					boneWeight.boneIndex3 += num3;
					tempBoneWeights[num4] = boneWeight;
					num4++;
				}
				num3 += num5;
			}
			else
			{
				Transform transform = tmBatchObject2.CachedTransform;
				tempBones[num3] = transform;
				tempBindPoses[num3] = transform.worldToLocalMatrix * base.CachedTransform.localToWorldMatrix;
				int num7 = tmBatchObject2.Mesh.vertexCount;
				for (int m = 0; m < num7; m++)
				{
					tempBoneWeights[num4].boneIndex0 = num3;
					tempBoneWeights[num4].weight0 = 1f;
					num4++;
				}
				num3++;
			}
		}
		base.Mesh.boneWeights = tempBoneWeights;
		base.Mesh.bindposes = tempBindPoses;
		base.CachedSkinnedRender.quality = SkinQuality.Auto;
		base.CachedSkinnedRender.bones = tempBones;
		base.CachedSkinnedRender.sharedMesh = base.Mesh;
		base.CachedSkinnedRender.localBounds = base.Mesh.bounds;
		base.CachedSkinnedRender.enabled = base.Mesh.vertexCount > 0;
	}

	protected override void ClearMesh()
	{
		base.Mesh.boneWeights = null;
		base.Mesh.bindposes = null;
		base.CachedSkinnedRender.bones = null;
		base.CachedSkinnedRender.sharedMesh = null;
		base.CachedSkinnedRender.enabled = false;
	}
}
