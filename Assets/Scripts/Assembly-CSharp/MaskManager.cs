using System.Collections.Generic;
using Spine;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
	public struct MaskSettings
	{
		public string nameSlot;

		public MaskBase prefabMask;

		public List<string> excludesNamesForCut;

		public List<string> excludesNamesForHide;
	}

	private const int CONST_RAY = 10000;

	[SerializeField]
	private string skeletonSlotPrefix;

	[SerializeField]
	private SkeletonAnimation characterAnimation;

	[SerializeField]
	private SkeletonUtility characterUtility;

	[SerializeField]
	private float collisionForceCut;

	[SerializeField]
	private Shader shaderMaskSlot;

	[SerializeField]
	private Masks masks;

	private List<Slot> cachedSlotsSkeleton = new List<Slot>();

	private List<Slot> cachedSlots = new List<Slot>();

	private List<MaskBase> currentMasks = new List<MaskBase>();

	public List<MaskBase> CurrentMasks
	{
		get
		{
			return currentMasks;
		}
	}

	private void Start()
	{
		if ((bool)characterAnimation && !string.IsNullOrEmpty(skeletonSlotPrefix))
		{
			for (int i = 0; i < characterAnimation.Skeleton.Slots.Count; i++)
			{
				Slot slot = characterAnimation.Skeleton.Slots.Items[i];
				if (slot != null && !string.IsNullOrEmpty(slot.Data.Name))
				{
					if (slot.Data.Name.Contains(skeletonSlotPrefix))
					{
						cachedSlotsSkeleton.Add(slot);
					}
					cachedSlots.Add(slot);
				}
			}
		}
		GetComponentsInChildren(currentMasks);
		if (currentMasks != null)
		{
			for (int j = 0; j < currentMasks.Count; j++)
			{
				currentMasks[j].InitMask(this);
			}
		}
	}

	private void OnDestroy()
	{
		if (currentMasks != null)
		{
			for (int i = 0; i < currentMasks.Count; i++)
			{
				currentMasks[i].RemoveDecals();
			}
		}
	}

	public void ApplyDamage(CollisionInfo info)
	{
		if (info == null || currentMasks == null)
		{
			return;
		}
		float num = 0f;
		if ((bool)info.collider)
		{
			WeaponBowlDownCollider[] componentsInChildren = info.collider.GetComponentsInChildren<WeaponBowlDownCollider>();
			if (componentsInChildren != null)
			{
				foreach (WeaponBowlDownCollider weaponBowlDownCollider in componentsInChildren)
				{
					if (weaponBowlDownCollider != null)
					{
						num = Mathf.Max(num, weaponBowlDownCollider.CollisionForce);
					}
				}
			}
		}
		ApplyDamage(info, num > collisionForceCut);
	}

	public void ApplyDamage(CollisionInfo info, bool withCut)
	{
		if (info == null || !info.otherCollider || currentMasks == null)
		{
			return;
		}
		MaskBase maskBase = FindMaskWithCollider(info.otherCollider);
		if (maskBase != null)
		{
			AddDecal(maskBase, info.decalType, GetClossestPont(info));
			if (withCut)
			{
				AddCutDecal(maskBase, info.position, info.direction);
			}
		}
	}

	public void AddDecal(MaskBase onMask, DecalType type, Vector2 position)
	{
		if ((bool)onMask)
		{
			onMask.AddDecal(type, position);
		}
	}

	public void AddCutDecal(MaskBase onMask, Vector2 position, Vector2 direction)
	{
		if ((bool)onMask)
		{
			onMask.AddCutDecal(position, direction);
		}
	}

	public MaskBase FindMaskWithCollider(Collider collider)
	{
		MaskBase result = null;
		if ((bool)collider)
		{
			for (int i = 0; i < currentMasks.Count; i++)
			{
				if (currentMasks[i].CachedMaskCollider == collider)
				{
					return currentMasks[i];
				}
			}
		}
		return result;
	}

	public Vector2 GetClossestPont(CollisionInfo info)
	{
		Ray ray = new Ray(info.position - info.direction * 10000f, info.direction);
		Vector3 vector = info.position;
		if (info.otherCollider != null && ray.direction.magnitude > 0f)
		{
			RaycastHit hitInfo = default(RaycastHit);
			if (info.otherCollider.Raycast(ray, out hitInfo, 20000f))
			{
				vector = ray.origin + ray.direction * hitInfo.distance;
			}
		}
		return vector;
	}

	public Slot GetSkeletonSlot(Bone bone)
	{
		Slot result = null;
		if (cachedSlotsSkeleton != null && bone != null)
		{
			for (int i = 0; i < cachedSlotsSkeleton.Count; i++)
			{
				Slot slot = cachedSlotsSkeleton[i];
				if (slot != null && slot.Bone == bone)
				{
					result = slot;
					break;
				}
			}
		}
		return result;
	}

	public List<Slot> GetSlots(Bone bone)
	{
		List<Slot> list = null;
		if (bone != null && cachedSlots != null)
		{
			list = new List<Slot>();
			for (int i = 0; i < cachedSlots.Count; i++)
			{
				Slot slot = cachedSlots[i];
				if (slot != null && slot.Bone == bone)
				{
					list.Add(slot);
				}
			}
		}
		return list;
	}

	public Slot GetSlot(string slotName)
	{
		Slot result = null;
		if (cachedSlots != null && !string.IsNullOrEmpty(slotName))
		{
			for (int i = 0; i < cachedSlots.Count; i++)
			{
				Slot slot = cachedSlots[i];
				if (slot != null && slot.Data.Name == slotName)
				{
					result = slot;
					break;
				}
			}
		}
		return result;
	}

	public SkeletonUtilityBone GetUtilityBone(string boneName)
	{
		SkeletonUtilityBone result = null;
		if (characterUtility != null && !string.IsNullOrEmpty(boneName))
		{
			for (int i = 0; i < characterUtility.utilityBones.Count; i++)
			{
				SkeletonUtilityBone skeletonUtilityBone = characterUtility.utilityBones[i];
				if (skeletonUtilityBone != null && skeletonUtilityBone.boneName == boneName)
				{
					result = skeletonUtilityBone;
					break;
				}
			}
		}
		return result;
	}

	public MaskBase GetMask(string slotName)
	{
		MaskBase result = null;
		if (currentMasks != null && !string.IsNullOrEmpty(slotName))
		{
			for (int i = 0; i < currentMasks.Count; i++)
			{
				MaskBase maskBase = currentMasks[i];
				if (maskBase != null && maskBase.CachedSlot.Data.Name == slotName)
				{
					result = maskBase;
					break;
				}
			}
		}
		return result;
	}
}
