using System.Collections.Generic;
using Spine;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SkeletonRagdoll))]
public class StopBonesClass : MonoBehaviour
{
	private List<string> availableBonesList;

	private List<string> stopBonesList = new List<string>();

	private void Awake()
	{
		string[] collection = new string[12]
		{
			"body", "arm_l", "arm_r", "forearm_l", "forearm_r", "hand_l", "hand_r", "head", "thigh_l", "thigh_r",
			"shin_l", "shin_r"
		};
		availableBonesList = new List<string>(collection);
		SkeletonAnimation component = GetComponent<SkeletonAnimation>();
		if (component != null)
		{
			Bone bone = component.Skeleton.FindBone("body");
			if (bone != null)
			{
				AnimationRecursion(bone);
			}
		}
		SkeletonRagdoll component2 = GetComponent<SkeletonRagdoll>();
		if (component2 != null)
		{
			component2.stopBoneNames = stopBonesList;
		}
	}

	private void AnimationRecursion(Bone bone)
	{
		ExposedList<Bone> children = bone.Children;
		if (children == null)
		{
			return;
		}
		Bone[] array = children.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (availableBonesList.Contains(array[i].ToString()))
			{
				AnimationRecursion(array[i]);
			}
			else
			{
				stopBonesList.Add(array[i].ToString());
			}
		}
	}
}
