using System.Collections.Generic;
using Spine;
using UnityEngine;

[RequireComponent(typeof(MaskSpine))]
public class MaskBase : MonoBehaviour
{
	private const int MAX_DECALS = 3;

	[SerializeField]
	private SkeletonAnimation characterAnimation;

	[Header("Decal without cut")]
	[SerializeField]
	private List<Decal> prefabsDecal;

	[Header("Decal with cut")]
	[SerializeField]
	private List<Decal> prefabsCutDecal;

	[Header("FX start by cut")]
	[SerializeField]
	private GameObject fxMeatDrop;

	[Header("Additional slots will hide by cut decal")]
	[SpineSlot("", "characterAnimation", false)]
	[SerializeField]
	private string[] additionalCutSlots;

	[Header("Additional slots will hide with clear all")]
	[SpineSlot("", "characterAnimation", false)]
	[SerializeField]
	private string[] additionalFadeSlots;

	private MaskSpine cachedMaskSpine;

	private Transform cachedTransform;

	private Collider cachedMaskCollider;

	private SkeletonUtilityBone cachedUtilityBone;

	private Slot cachedSlot;

	private Slot cachedSkeletonSlot;

	private List<Slot> cachedBoneSlots = new List<Slot>();

	private List<Slot> cachedCutSlots = new List<Slot>();

	private List<Slot> cachedFadeSlots = new List<Slot>();

	private List<Decal> cachedPrefabBlood = new List<Decal>();

	private List<Decal> cachedPrefabAcid = new List<Decal>();

	private List<Decal> cachedPrefabScorch = new List<Decal>();

	private List<Decal> cachedPrefabCut = new List<Decal>();

	private MaskManager cachedManager;

	private List<Decal> currentDecals = new List<Decal>();

	private List<Attachment> currentHideAttachments = new List<Attachment>();

	private List<Decal> prefabInstantiate = new List<Decal>();

	private List<Decal> tempList = new List<Decal>();

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

	public MaskSpine CachedMaskSpine
	{
		get
		{
			if (cachedMaskSpine == null)
			{
				cachedMaskSpine = GetComponent<MaskSpine>();
			}
			return cachedMaskSpine;
		}
	}

	public Collider CachedMaskCollider
	{
		get
		{
			if (cachedMaskCollider == null && (bool)CachedMaskSpine && (bool)CachedMaskSpine.CachedUtilityBone && (bool)CachedMaskSpine.CachedUtilityBoneTransform)
			{
				foreach (Transform item in CachedMaskSpine.CachedUtilityBoneTransform)
				{
					cachedMaskCollider = item.GetComponent<Collider>();
					if ((bool)cachedMaskCollider)
					{
						break;
					}
				}
			}
			return cachedMaskCollider;
		}
	}

	public Slot CachedSlot
	{
		get
		{
			if (cachedSlot == null && (bool)CachedMaskSpine)
			{
				cachedSlot = CachedMaskSpine.CachedSlot;
			}
			return cachedSlot;
		}
	}

	public Slot CachedSkeletonSlot
	{
		get
		{
			if (cachedSkeletonSlot == null && (bool)CachedManager && (bool)CachedMaskSpine)
			{
				Bone cachedBone = CachedMaskSpine.CachedBone;
				if (cachedBone != null)
				{
					cachedSkeletonSlot = CachedManager.GetSkeletonSlot(cachedBone);
				}
			}
			return cachedSkeletonSlot;
		}
	}

	public SkeletonUtilityBone CachedUtilityBone
	{
		get
		{
			if (cachedUtilityBone == null && CachedSlot != null && (bool)CachedManager)
			{
				cachedUtilityBone = CachedManager.GetUtilityBone(CachedSlot.Bone.Data.Name);
			}
			return cachedUtilityBone;
		}
	}

	public MaskManager CachedManager
	{
		get
		{
			if (cachedManager == null)
			{
				cachedManager = GetComponentInParent<MaskManager>();
			}
			return cachedManager;
		}
	}

	public SkeletonAnimation CharacterAnimation
	{
		get
		{
			return characterAnimation;
		}
		set
		{
			characterAnimation = value;
		}
	}

	public static MaskBase CreateMask(GameObject go, SkeletonAnimation animation, string[] additionalCuts, string[] additionalFades)
	{
		MaskBase maskBase = go.GetComponent<MaskBase>();
		if (maskBase == null)
		{
			maskBase = go.AddComponent<MaskBase>();
		}
		maskBase.characterAnimation = animation;
		maskBase.additionalCutSlots = additionalCuts;
		maskBase.additionalFadeSlots = additionalFades;
		return maskBase;
	}

	private void Awake()
	{
		if (prefabsDecal != null)
		{
			for (int i = 0; i < prefabsDecal.Count; i++)
			{
				if (!prefabsDecal[i].MaskedCut && !prefabsDecal[i].MaskedFill)
				{
					continue;
				}
				prefabsDecal[i].InitDecal();
				DecalType decalType = prefabsDecal[i].DecalType;
				if (prefabsDecal[i].IsCut)
				{
					cachedPrefabCut.Add(prefabsDecal[i]);
					continue;
				}
				switch (decalType)
				{
				case DecalType.Blood:
					cachedPrefabBlood.Add(prefabsDecal[i]);
					break;
				case DecalType.Scorch:
					cachedPrefabScorch.Add(prefabsDecal[i]);
					break;
				case DecalType.Acid:
					cachedPrefabAcid.Add(prefabsDecal[i]);
					break;
				}
			}
		}
		if (prefabsCutDecal == null)
		{
			return;
		}
		for (int j = 0; j < prefabsCutDecal.Count; j++)
		{
			if (prefabsCutDecal[j].IsCut)
			{
				cachedPrefabCut.Add(prefabsCutDecal[j]);
			}
		}
	}

	public void InitMask(MaskManager manager)
	{
		cachedManager = manager;
		if (additionalCutSlots != null && cachedCutSlots != null)
		{
			cachedCutSlots.Clear();
			for (int i = 0; i < additionalCutSlots.Length; i++)
			{
				Slot slot = CachedManager.GetSlot(additionalCutSlots[i]);
				if (slot != null)
				{
					cachedCutSlots.Add(slot);
				}
			}
		}
		if (additionalFadeSlots != null && cachedFadeSlots != null)
		{
			cachedFadeSlots.Clear();
			for (int j = 0; j < additionalFadeSlots.Length; j++)
			{
				Slot slot2 = CachedManager.GetSlot(additionalFadeSlots[j]);
				if (slot2 != null)
				{
					cachedFadeSlots.Add(slot2);
				}
			}
		}
		if (CachedSlot != null && cachedBoneSlots != null)
		{
			cachedBoneSlots = CachedManager.GetSlots(CachedSlot.Bone);
		}
	}

	public Decal GetRandomDecal(DecalType type)
	{
		Decal decal = null;
		tempList.Clear();
		switch (type)
		{
		case DecalType.Acid:
			tempList.AddRange(cachedPrefabAcid);
			break;
		case DecalType.Blood:
			tempList.AddRange(cachedPrefabBlood);
			break;
		case DecalType.Scorch:
			tempList.AddRange(cachedPrefabScorch);
			break;
		}
		tempList.RemoveAll((Decal i) => prefabInstantiate.Contains(i));
		decal = tempList.RandomObject();
		if ((bool)decal)
		{
			prefabInstantiate.Add(decal);
		}
		return decal;
	}

	public Decal GetRandomCutDecal()
	{
		Decal decal = null;
		tempList.Clear();
		tempList.AddRange(cachedPrefabCut);
		tempList.RemoveAll((Decal i) => prefabInstantiate.Contains(i));
		decal = tempList.RandomObject();
		if ((bool)decal)
		{
			prefabInstantiate.Add(decal);
		}
		return decal;
	}

	public void AddDecal(DecalType type, Vector2 position)
	{
		if ((bool)CachedMaskSpine)
		{
			Decal randomDecal = GetRandomDecal(type);
			AddDecal(randomDecal, position);
		}
	}

	public void AddDecal(Decal decal, Vector2 position)
	{
		if ((bool)CachedMaskSpine && (bool)decal)
		{
			PrepareAddDecal();
			Decal decal2 = Object.Instantiate(decal);
			decal2.CachedTransform.position = position;
			if ((bool)CachedMaskSpine.CachedUtilityBoneTransform)
			{
				decal2.CachedTransform.parent = CachedMaskSpine.CachedUtilityBoneTransform;
			}
			else
			{
				decal2.CachedTransform.parent = CachedTransform;
			}
			if ((bool)decal2.MaskedCut)
			{
				CachedMaskSpine.AddMaskedCut(decal2.MaskedCut);
			}
			if ((bool)decal2.MaskedFill)
			{
				CachedMaskSpine.AddMaskedFill(decal2.MaskedFill);
			}
			currentDecals.Add(decal2);
		}
	}

	public void AddCutDecal(Vector2 position, Vector2 direction)
	{
		if ((bool)CachedMaskSpine)
		{
			Decal randomCutDecal = GetRandomCutDecal();
			AddCutDecal(randomCutDecal, position, direction);
		}
	}

	public void AddCutDecal(Decal decal, Vector2 position, Vector2 direction)
	{
		if (!CachedMaskSpine || !decal)
		{
			return;
		}
		if (CanAddCutDecal())
		{
			PrepareAddDecal();
			Decal decal2 = Object.Instantiate(decal);
			Vector3 positionCutDecal = GetPositionCutDecal();
			decal2.CachedTransform.position = positionCutDecal;
			if ((bool)CachedMaskSpine.CachedUtilityBoneTransform)
			{
				decal2.CachedTransform.rotation = GetRotationCutDecal();
				decal2.CachedTransform.parent = CachedMaskSpine.CachedUtilityBoneTransform;
			}
			else
			{
				decal2.CachedTransform.parent = CachedTransform;
			}
			if ((bool)decal2.MaskedCut)
			{
				CachedMaskSpine.AddMaskedCut(decal2.MaskedCut);
			}
			if ((bool)decal2.MaskedFill)
			{
				CachedMaskSpine.AddMaskedFill(decal2.MaskedFill);
			}
			currentDecals.Add(decal2);
			if ((bool)fxMeatDrop)
			{
				GameObject gameObject = Object.Instantiate(fxMeatDrop);
				gameObject.transform.position = position;
				if (direction.x < 0f)
				{
					gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
				}
				gameObject.transform.parent = CachedTransform;
			}
			if (cachedCutSlots == null)
			{
				return;
			}
			for (int i = 0; i < cachedCutSlots.Count; i++)
			{
				Slot slot = cachedCutSlots[i];
				if (slot != null && slot.Attachment != null)
				{
					SetClearColorSlot(slot.Attachment);
					currentHideAttachments.Add(slot.Attachment);
				}
			}
		}
		else
		{
			FadeOut();
		}
	}

	public void RemoveDecals()
	{
		if (!CachedMaskSpine)
		{
			return;
		}
		for (int i = 0; i < currentDecals.Count; i++)
		{
			MaskedSpine maskedFill = currentDecals[i].MaskedFill;
			if ((bool)maskedFill)
			{
				CachedMaskSpine.RemoveMaskedFill(maskedFill);
			}
			MaskedSpine maskedCut = currentDecals[i].MaskedCut;
			if ((bool)maskedCut)
			{
				CachedMaskSpine.RemoveMaskedCut(maskedCut);
			}
		}
		currentDecals.Clear();
		for (int j = 0; j < currentHideAttachments.Count; j++)
		{
			SetWhiteColorSlot(currentHideAttachments[j]);
		}
		currentHideAttachments.Clear();
	}

	public void FadeIn()
	{
		for (int i = 0; i < currentHideAttachments.Count; i++)
		{
			SetWhiteColorSlot(currentHideAttachments[i]);
		}
		currentHideAttachments.Clear();
	}

	public void FadeOut()
	{
		if (cachedBoneSlots != null)
		{
			for (int i = 0; i < cachedBoneSlots.Count; i++)
			{
				Slot slot = cachedBoneSlots[i];
				if (slot != null && slot != CachedSkeletonSlot && slot.Attachment != null)
				{
					SetClearColorSlot(slot.Attachment);
					currentHideAttachments.Add(slot.Attachment);
				}
			}
		}
		if (cachedFadeSlots == null)
		{
			return;
		}
		for (int j = 0; j < cachedFadeSlots.Count; j++)
		{
			Slot slot2 = cachedFadeSlots[j];
			if (slot2 != null && slot2 != CachedSkeletonSlot && slot2.Attachment != null)
			{
				SetClearColorSlot(slot2.Attachment);
				currentHideAttachments.Add(slot2.Attachment);
			}
		}
	}

	private bool CanAddCutDecal()
	{
		bool result = true;
		for (int i = 0; i < currentDecals.Count; i++)
		{
			if ((bool)currentDecals[i].MaskedCut)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private void PrepareAddDecal()
	{
		if (currentDecals.Count < 3)
		{
			return;
		}
		for (int i = 0; i < currentDecals.Count; i++)
		{
			if (!currentDecals[i].MaskedCut)
			{
				Decal decal = currentDecals[i];
				if ((bool)decal.MaskedCut)
				{
					CachedMaskSpine.RemoveMaskedCut(decal.MaskedCut);
				}
				if ((bool)decal.MaskedFill)
				{
					CachedMaskSpine.RemoveMaskedFill(decal.MaskedFill);
				}
				currentDecals.Remove(decal);
				Object.Destroy(decal.gameObject);
				break;
			}
		}
	}

	private Vector3 GetPositionCutDecal()
	{
		Vector3 result = Vector3.zero;
		if (CachedSkeletonSlot != null)
		{
			RegionAttachment regionAttachment = CachedSkeletonSlot.Attachment as RegionAttachment;
			if (regionAttachment != null)
			{
				Vector2 vector = new Vector2(regionAttachment.X, regionAttachment.Y);
				if ((bool)CachedMaskSpine && (bool)CachedMaskSpine.CachedUtilityBoneTransform)
				{
					result = CachedMaskSpine.CachedUtilityBoneTransform.TransformPoint(vector);
				}
			}
		}
		return result;
	}

	private Quaternion GetRotationCutDecal()
	{
		Quaternion result = Quaternion.identity;
		if (CachedSkeletonSlot != null)
		{
			RegionAttachment regionAttachment = CachedSkeletonSlot.Attachment as RegionAttachment;
			if (regionAttachment != null)
			{
				float rotation = regionAttachment.Rotation;
				if ((bool)CachedMaskSpine && (bool)CachedMaskSpine.CachedUtilityBoneTransform)
				{
					result = CachedMaskSpine.CachedUtilityBoneTransform.rotation * Quaternion.Euler(0f, 0f, rotation);
				}
			}
		}
		return result;
	}

	private void SetClearColorSlot(Attachment attachment)
	{
		if (attachment != null)
		{
			if (attachment is RegionAttachment)
			{
				(attachment as RegionAttachment).SetColor(Color.clear);
			}
			else if (attachment is MeshAttachment)
			{
				(attachment as MeshAttachment).SetColor(Color.clear);
			}
			else if (attachment is SkinnedMeshAttachment)
			{
				(attachment as SkinnedMeshAttachment).SetColor(Color.clear);
			}
		}
	}

	private void SetWhiteColorSlot(Attachment attachment)
	{
		if (attachment != null)
		{
			if (attachment is RegionAttachment)
			{
				(attachment as RegionAttachment).SetColor(Color.white);
			}
			else if (attachment is MeshAttachment)
			{
				(attachment as MeshAttachment).SetColor(Color.white);
			}
			else if (attachment is SkinnedMeshAttachment)
			{
				(attachment as SkinnedMeshAttachment).SetColor(Color.white);
			}
		}
	}
}
