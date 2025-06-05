using System;
using Spine;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Spine/BoneFollower")]
public class BoneFollower : MonoBehaviour
{
	[NonSerialized]
	public bool valid;

	public SkeletonRenderer skeletonRenderer;

	public Bone bone;

	public bool followZPosition = true;

	public bool followBoneRotation = true;

	[SpineBone("", "skeletonRenderer")]
	public string boneName;

	public bool resetOnAwake = true;

	protected Transform cachedTransform;

	protected Transform skeletonTransform;

	public SkeletonRenderer SkeletonRenderer
	{
		get
		{
			return skeletonRenderer;
		}
		set
		{
			skeletonRenderer = value;
			Reset();
		}
	}

	public void HandleResetRenderer(SkeletonRenderer skeletonRenderer)
	{
		Reset();
	}

	public void Reset()
	{
		bone = null;
		cachedTransform = base.transform;
		valid = skeletonRenderer != null && skeletonRenderer.valid;
		if (valid)
		{
			skeletonTransform = skeletonRenderer.transform;
			SkeletonRenderer obj = skeletonRenderer;
			obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleResetRenderer));
			SkeletonRenderer obj2 = skeletonRenderer;
			obj2.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Combine(obj2.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleResetRenderer));
			if (Application.isEditor)
			{
				DoUpdate();
			}
		}
	}

	private void OnDestroy()
	{
		if (skeletonRenderer != null)
		{
			SkeletonRenderer obj = skeletonRenderer;
			obj.OnReset = (SkeletonRenderer.SkeletonRendererDelegate)Delegate.Remove(obj.OnReset, new SkeletonRenderer.SkeletonRendererDelegate(HandleResetRenderer));
		}
	}

	public void Awake()
	{
		if (resetOnAwake)
		{
			Reset();
		}
	}

	private void LateUpdate()
	{
		DoUpdate();
	}

	public void DoUpdate()
	{
		if (!valid)
		{
			Reset();
			return;
		}
		if (bone == null)
		{
			if (boneName == null || boneName.Length == 0)
			{
				return;
			}
			bone = skeletonRenderer.skeleton.FindBone(boneName);
			if (bone == null)
			{
				Debug.LogError("Bone not found: " + boneName, this);
				return;
			}
		}
		Skeleton skeleton = skeletonRenderer.skeleton;
		float num = ((!(skeleton.flipX ^ skeleton.flipY)) ? 1f : (-1f));
		if (cachedTransform.parent == skeletonTransform)
		{
			cachedTransform.localPosition = new Vector3(bone.worldX, bone.worldY, (!followZPosition) ? cachedTransform.localPosition.z : 0f);
			if (followBoneRotation)
			{
				Vector3 eulerAngles = cachedTransform.localRotation.eulerAngles;
				cachedTransform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, bone.worldRotation * num);
			}
			return;
		}
		Vector3 position = skeletonTransform.TransformPoint(new Vector3(bone.worldX, bone.worldY, 0f));
		if (!followZPosition)
		{
			position.z = cachedTransform.position.z;
		}
		cachedTransform.position = position;
		if (followBoneRotation)
		{
			Vector3 eulerAngles2 = skeletonTransform.rotation.eulerAngles;
			cachedTransform.rotation = Quaternion.Euler(eulerAngles2.x, eulerAngles2.y, skeletonTransform.rotation.eulerAngles.z + bone.worldRotation * num);
		}
	}
}
