using System;
using Spine;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Spine/SkeletonUtilityBone")]
public class SkeletonUtilityBone : MonoBehaviour
{
	public enum Mode
	{
		Follow = 0,
		Override = 1
	}

	[NonSerialized]
	public bool valid;

	[NonSerialized]
	public SkeletonUtility skeletonUtility;

	[NonSerialized]
	public Bone bone;

	public Mode mode;

	public bool zPosition = true;

	public bool position;

	public bool rotation;

	public bool scale;

	public bool flip;

	public bool flipX;

	[Range(0f, 1f)]
	public float overrideAlpha = 1f;

	public string boneName;

	public Transform parentReference;

	[HideInInspector]
	public bool transformLerpComplete;

	protected Transform cachedTransform;

	protected Transform skeletonTransform;

	private bool nonUniformScaleWarning;

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public bool NonUniformScaleWarning
	{
		get
		{
			return nonUniformScaleWarning;
		}
	}

	public void Reset()
	{
		bone = null;
		cachedTransform = base.transform;
		valid = skeletonUtility != null && skeletonUtility.skeletonRenderer != null && skeletonUtility.skeletonRenderer.valid;
		if (valid)
		{
			skeletonTransform = skeletonUtility.transform;
			skeletonUtility.OnReset -= HandleOnReset;
			skeletonUtility.OnReset += HandleOnReset;
			DoUpdate();
		}
	}

	private void OnEnable()
	{
		skeletonUtility = SkeletonUtility.GetInParent<SkeletonUtility>(base.transform);
		if (!(skeletonUtility == null))
		{
			skeletonUtility.RegisterBone(this);
			skeletonUtility.OnReset += HandleOnReset;
		}
	}

	private void HandleOnReset()
	{
		Reset();
	}

	private void OnDisable()
	{
		if (skeletonUtility != null)
		{
			skeletonUtility.OnReset -= HandleOnReset;
			skeletonUtility.UnregisterBone(this);
		}
	}

	public void DoUpdate()
	{
		if (!valid)
		{
			Reset();
			return;
		}
		Skeleton skeleton = skeletonUtility.skeletonRenderer.skeleton;
		if (bone == null)
		{
			if (boneName == null || boneName.Length == 0)
			{
				return;
			}
			bone = skeleton.FindBone(boneName);
			if (bone == null)
			{
				Debug.LogError("Bone not found: " + boneName, this);
				return;
			}
		}
		float num = ((!(skeleton.flipX ^ skeleton.flipY)) ? 1f : (-1f));
		float num2 = 0f;
		if (flip && (flipX || flipX != bone.flipX) && bone.parent != null)
		{
			num2 = bone.parent.WorldRotation * -2f;
		}
		if (mode == Mode.Follow)
		{
			if (flip)
			{
				flipX = bone.flipX;
			}
			if (position)
			{
				cachedTransform.localPosition = new Vector3(bone.x, bone.y, 0f);
			}
			if (rotation)
			{
				if (bone.Data.InheritRotation)
				{
					if (bone.FlipX)
					{
						cachedTransform.localRotation = Quaternion.Euler(0f, 180f, bone.rotationIK - num2);
					}
					else
					{
						cachedTransform.localRotation = Quaternion.Euler(0f, 0f, bone.rotationIK);
					}
				}
				else
				{
					Vector3 eulerAngles = skeletonTransform.rotation.eulerAngles;
					cachedTransform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, skeletonTransform.rotation.eulerAngles.z + bone.worldRotation * num);
				}
			}
			if (scale)
			{
				cachedTransform.localScale = new Vector3(bone.scaleX, bone.scaleY, (!bone.worldFlipX) ? 1 : (-1));
				nonUniformScaleWarning = bone.scaleX != bone.scaleY;
			}
		}
		else
		{
			if (mode != Mode.Override || transformLerpComplete)
			{
				return;
			}
			if (parentReference == null)
			{
				if (position)
				{
					bone.x = Mathf.Lerp(bone.x, cachedTransform.localPosition.x, overrideAlpha);
					bone.y = Mathf.Lerp(bone.y, cachedTransform.localPosition.y, overrideAlpha);
				}
				if (rotation)
				{
					float num3 = Mathf.LerpAngle(bone.Rotation, cachedTransform.localRotation.eulerAngles.z, overrideAlpha) + num2;
					if (flip)
					{
						if (!flipX && bone.flipX)
						{
							num3 -= num2;
						}
						if (num3 >= 360f)
						{
							num3 -= 360f;
						}
						else if (num3 <= -360f)
						{
							num3 += 360f;
						}
					}
					bone.Rotation = num3;
					bone.RotationIK = num3;
				}
				if (scale)
				{
					bone.scaleX = Mathf.Lerp(bone.scaleX, cachedTransform.localScale.x, overrideAlpha);
					bone.scaleY = Mathf.Lerp(bone.scaleY, cachedTransform.localScale.y, overrideAlpha);
					nonUniformScaleWarning = bone.scaleX != bone.scaleY;
				}
				if (flip)
				{
					bone.flipX = flipX;
				}
			}
			else
			{
				if (transformLerpComplete)
				{
					return;
				}
				if (position)
				{
					Vector3 vector = parentReference.InverseTransformPoint(cachedTransform.position);
					bone.x = Mathf.Lerp(bone.x, vector.x, overrideAlpha);
					bone.y = Mathf.Lerp(bone.y, vector.y, overrideAlpha);
				}
				if (rotation)
				{
					float num4 = Mathf.LerpAngle(bone.Rotation, Quaternion.LookRotation((!flipX) ? Vector3.forward : (Vector3.forward * -1f), parentReference.InverseTransformDirection(cachedTransform.up)).eulerAngles.z, overrideAlpha) + num2;
					if (flip)
					{
						if (!flipX && bone.flipX)
						{
							num4 -= num2;
						}
						if (num4 >= 360f)
						{
							num4 -= 360f;
						}
						else if (num4 <= -360f)
						{
							num4 += 360f;
						}
					}
					bone.Rotation = num4;
					bone.RotationIK = num4;
				}
				if (scale)
				{
					bone.scaleX = Mathf.Lerp(bone.scaleX, cachedTransform.localScale.x, overrideAlpha);
					bone.scaleY = Mathf.Lerp(bone.scaleY, cachedTransform.localScale.y, overrideAlpha);
					nonUniformScaleWarning = bone.scaleX != bone.scaleY;
				}
				if (flip)
				{
					bone.flipX = flipX;
				}
			}
			transformLerpComplete = true;
		}
	}

	public void FlipX(bool state)
	{
		if (state != flipX)
		{
			flipX = state;
			if (flipX && Mathf.Abs(base.transform.localRotation.eulerAngles.y) > 90f)
			{
				skeletonUtility.skeletonAnimation.LateUpdate();
				return;
			}
			if (!flipX && Mathf.Abs(base.transform.localRotation.eulerAngles.y) < 90f)
			{
				skeletonUtility.skeletonAnimation.LateUpdate();
				return;
			}
		}
		bone.FlipX = state;
		base.transform.RotateAround(base.transform.position, skeletonUtility.transform.up, 180f);
		Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.y = (bone.FlipX ? 180 : 0);
		base.transform.localRotation = Quaternion.Euler(eulerAngles);
	}

	public void AddBoundingBox(string skinName, string slotName, string attachmentName)
	{
		SkeletonUtility.AddBoundingBox2D(bone.skeleton, skinName, slotName, attachmentName, base.transform);
	}

	public void AddBoundingBox3D(string skinName, string slotName, string attachmentName, float thickness = 0.125f)
	{
		Skeleton skeleton = bone.skeleton;
		Transform parent = base.transform;
		SkeletonUtility.AddBoundingBox3D(skeleton, skinName, slotName, attachmentName, parent, true, thickness);
	}

	private void OnDrawGizmos()
	{
		if (NonUniformScaleWarning)
		{
			Gizmos.DrawIcon(base.transform.position + new Vector3(0f, 0.128f, 0f), "icon-warning");
		}
	}
}
