using System;
using System.Collections.Generic;
using UnityEngine;

public class FatalityMask : MonoBehaviour
{
	[Header("Method for masks")]
	[Button]
	[SerializeField]
	private string maskMethodName;

	[Header("Method for decals")]
	[Button]
	[SerializeField]
	private string decalMethodName;

	[SerializeField]
	private DecalType decalType;

	private FatalityBase cachedFatality;

	private FatalityManager cachedManager;

	private Action<Collider, SkeletonUtilityBone> cachedMaskMethod;

	private Action<Collider, SkeletonUtilityBone> cachedDecalMethod;

	private List<MaskBase> currentMasks = new List<MaskBase>();

	private List<MaskBase> currentDecals = new List<MaskBase>();

	public FatalityBase CachedFatality
	{
		get
		{
			if (cachedFatality == null)
			{
				cachedFatality = GetComponent<FatalityBase>();
			}
			return cachedFatality;
		}
	}

	public FatalityManager CachedManager
	{
		get
		{
			if (cachedManager == null && (bool)CachedFatality)
			{
				cachedManager = CachedFatality.CachedManager;
			}
			return cachedManager;
		}
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(maskMethodName))
		{
			Invoke(maskMethodName, 0f);
		}
		if (!string.IsNullOrEmpty(decalMethodName))
		{
			Invoke(decalMethodName, 0f);
		}
		currentDecals.Clear();
		currentMasks.Clear();
	}

	public void Activate(Collider collider, SkeletonUtilityBone utilityBone)
	{
		if (cachedMaskMethod != null)
		{
			cachedMaskMethod(collider, utilityBone);
		}
		if (cachedDecalMethod != null)
		{
			cachedDecalMethod(collider, utilityBone);
		}
	}

	public void Deactivate()
	{
		for (int i = 0; i < currentMasks.Count; i++)
		{
			currentMasks[i].FadeIn();
		}
		currentMasks.Clear();
		for (int j = 0; j < currentDecals.Count; j++)
		{
			currentDecals[j].RemoveDecals();
		}
		currentDecals.Clear();
	}

	private void MaskRemoveAll()
	{
		cachedMaskMethod = MaskRemoveAll;
	}

	private void MaskRemoveAll(Collider collider, SkeletonUtilityBone utilityBone)
	{
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase)
			{
				maskBase.FadeOut();
				currentMasks.Add(maskBase);
			}
		}
	}

	private void MaskRemoveHeadByHitHead()
	{
		cachedMaskMethod = MaskRemoveHeadByHitHead;
	}

	private void MaskRemoveHeadByHitHead(Collider collider, SkeletonUtilityBone utilityBone)
	{
		if (!(utilityBone == CachedManager.GetUtilityBoneHead()))
		{
			return;
		}
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase && maskBase.CachedUtilityBone == utilityBone)
			{
				maskBase.FadeOut();
				currentMasks.Add(maskBase);
				break;
			}
		}
	}

	private void MaskRemoveBodyByHitBody()
	{
		cachedMaskMethod = MaskRemoveBodyByHitBody;
	}

	private void MaskRemoveBodyByHitBody(Collider collider, SkeletonUtilityBone utilityBone)
	{
		if (!(utilityBone == CachedManager.GetUtilityBoneBody()))
		{
			return;
		}
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase && maskBase.CachedUtilityBone == utilityBone)
			{
				maskBase.FadeOut();
				currentMasks.Add(maskBase);
				break;
			}
		}
	}

	private void MaskRemoveHeadOrBodyByHitHeadOrBody()
	{
		cachedMaskMethod = MaskRemoveHeadOrBodyByHitHeadOrBody;
	}

	private void MaskRemoveHeadOrBodyByHitHeadOrBody(Collider collider, SkeletonUtilityBone utilityBone)
	{
		if (!(utilityBone == CachedManager.GetUtilityBoneBody()) && !(utilityBone == CachedManager.GetUtilityBoneHead()))
		{
			return;
		}
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase && maskBase.CachedUtilityBone == utilityBone)
			{
				maskBase.FadeOut();
				currentMasks.Add(maskBase);
				break;
			}
		}
	}

	private void MaskRemovePartByHitPart()
	{
		cachedMaskMethod = MaskRemovePartByHitPart;
	}

	private void MaskRemovePartByHitPart(Collider collider, SkeletonUtilityBone utilityBone)
	{
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase && maskBase.CachedUtilityBone == utilityBone)
			{
				maskBase.FadeOut();
				currentMasks.Add(maskBase);
				break;
			}
		}
	}

	private void DecalOnAll()
	{
		cachedDecalMethod = DecalOnAll;
	}

	private void DecalOnAll(Collider collider, SkeletonUtilityBone utilityBone)
	{
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase)
			{
				maskBase.AddDecal(decalType, maskBase.CachedUtilityBone.transform.position);
				currentDecals.Add(maskBase);
			}
		}
	}

	private void DecalOnBody()
	{
		cachedDecalMethod = DecalOnBody;
	}

	private void DecalOnBody(Collider collider, SkeletonUtilityBone utilityBone)
	{
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase && maskBase.CachedUtilityBone == CachedManager.GetUtilityBoneBody())
			{
				maskBase.AddDecal(decalType, maskBase.CachedUtilityBone.transform.position);
				currentDecals.Add(maskBase);
				break;
			}
		}
	}

	private void DecalOnBodyX3()
	{
		cachedDecalMethod = DecalOnBodyX3;
	}

	private void DecalOnBodyX3(Collider collider, SkeletonUtilityBone utilityBone)
	{
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase && maskBase.CachedUtilityBone == CachedManager.GetUtilityBoneBody())
			{
				for (int j = 0; j < 3; j++)
				{
					maskBase.AddDecal(decalType, GetRandomPositionDecal(maskBase));
					currentDecals.Add(maskBase);
				}
				break;
			}
		}
	}

	private void DecalOnBodyByHitHead()
	{
		cachedDecalMethod = DecalOnBodyByHitHead;
	}

	private void DecalOnBodyByHitHead(Collider collider, SkeletonUtilityBone utilityBone)
	{
		if (!(utilityBone == CachedManager.GetUtilityBoneHead()))
		{
			return;
		}
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase && maskBase.CachedUtilityBone == CachedManager.GetUtilityBoneBody())
			{
				maskBase.AddDecal(decalType, maskBase.CachedUtilityBone.transform.position);
				currentDecals.Add(maskBase);
				break;
			}
		}
	}

	private void DecalOnBodyOrHeadByHitBodyOrHead()
	{
		cachedDecalMethod = DecalOnBodyOrHeadByHitBodyOrHead;
	}

	private void DecalOnBodyOrHeadByHitBodyOrHead(Collider collider, SkeletonUtilityBone utilityBone)
	{
		if (!(utilityBone == CachedManager.GetUtilityBoneHead()) && !(utilityBone == CachedManager.GetUtilityBoneBody()))
		{
			return;
		}
		List<MaskBase> currentMaks = GetCurrentMaks();
		if (currentMaks == null)
		{
			return;
		}
		for (int i = 0; i < currentMaks.Count; i++)
		{
			MaskBase maskBase = currentMaks[i];
			if ((bool)maskBase)
			{
				if (utilityBone == CachedManager.GetUtilityBoneHead() && maskBase.CachedUtilityBone == utilityBone)
				{
					maskBase.AddDecal(decalType, maskBase.CachedUtilityBone.transform.position);
					currentDecals.Add(maskBase);
					break;
				}
				if (utilityBone == CachedManager.GetUtilityBoneBody() && maskBase.CachedUtilityBone != CachedManager.GetUtilityBoneHead())
				{
					maskBase.AddDecal(decalType, maskBase.CachedUtilityBone.transform.position);
					currentDecals.Add(maskBase);
				}
			}
		}
	}

	private Vector3 GetRandomPositionDecal(MaskBase mask)
	{
		Vector3 zero = Vector3.zero;
		if ((bool)mask)
		{
			Collider cachedMaskCollider = mask.CachedMaskCollider;
			if ((bool)cachedMaskCollider)
			{
				zero.x = UnityEngine.Random.Range(cachedMaskCollider.bounds.min.x, cachedMaskCollider.bounds.max.x);
				zero.y = UnityEngine.Random.Range(cachedMaskCollider.bounds.min.y, cachedMaskCollider.bounds.max.y);
			}
		}
		return zero;
	}

	private List<MaskBase> GetCurrentMaks()
	{
		List<MaskBase> result = null;
		if ((bool)CachedManager)
		{
			List<SkeletonUtilityBone> cachedUtilityBones = CachedManager.CachedUtilityBones;
			MaskManager characterMaskManager = CachedManager.CharacterMaskManager;
			if (cachedUtilityBones != null && (bool)characterMaskManager)
			{
				result = characterMaskManager.CurrentMasks;
			}
		}
		return result;
	}
}
