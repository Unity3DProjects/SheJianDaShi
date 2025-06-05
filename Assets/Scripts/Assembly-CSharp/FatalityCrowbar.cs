using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityCrowbar : FatalityBase
{
	private const string HEAD_BONE = "head";

	[SerializeField]
	private float delayCrabAppear = 2f;

	[SerializeField]
	private float timeBeforeHeadExplode = 2f;

	[SerializeField]
	[ResourceLink]
	private AssetLink holeFatalityEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink explodeEffectPrefab;

	[SerializeField]
	private Vector3 portalOffset;

	[SerializeField]
	private Crab crabPrefab;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	protected List<string> breakBonesNames = new List<string>();

	private Crab crab;

	private GameObject holeEffect;

	private Vector3 eulersAngles = new Vector3(0f, 0f, 90f);

	private Skeleton cachedSkeleton;

	private bool isHeadDeleted;

	protected SkeletonUtilityBone CachedHead
	{
		get
		{
			if (cachedHead == null)
			{
				cachedHead = base.CachedManager.GetUtilityBoneHead();
			}
			return cachedHead;
		}
	}

	protected SkeletonUtilityBone CachedBody
	{
		get
		{
			if (cachedBody == null)
			{
				cachedBody = base.CachedManager.GetUtilityBoneBody();
			}
			return cachedBody;
		}
	}

	protected Skeleton CachedSkeleton
	{
		get
		{
			if (cachedSkeleton == null)
			{
				cachedSkeleton = base.CachedManager.CurrentCharacter.SkeletonAnimation.Skeleton;
			}
			return cachedSkeleton;
		}
	}

	private bool IsHeadDeleted
	{
		set
		{
			isHeadDeleted = value;
			if (isHeadDeleted)
			{
				CachedSkeleton.FindBone("head").ScaleX = 0f;
				CachedSkeleton.FindBone("head").ScaleY = 0f;
			}
			else
			{
				CachedSkeleton.FindBone("head").ScaleX = 1f;
				CachedSkeleton.FindBone("head").ScaleY = 1f;
			}
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			StartAnimation(base.CachedManager.AnimationHeadcrabDeath);
			Vector3 position = base.CachedManager.CurrentCharacter.HeadColliderBody.transform.position + portalOffset;
			if (holeFatalityEffectPrefab != null && !string.IsNullOrEmpty(holeFatalityEffectPrefab.assetGUID))
			{
				holeEffect = (GameObject)holeFatalityEffectPrefab.GetInstance();
				holeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
				holeEffect.transform.position = position;
			}
			AudioManager.PlayAudioWithKey("fire_shuriken_fatality_explode");
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				AudioManager.StopAudioWithKey("fire_shuriken_fatality_explode");
			}, 2f);
			StartCoroutine(CoroutineAnimationFinished());
		}
	}

	public override void StopFatality()
	{
		crab.OnHeadTouched -= StartCoroutineOnTouched;
		base.StopFatality();
		Object.Destroy(holeEffect);
		holeEffect = null;
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_4", delegate(SoundObject obj)
		{
			if (obj != null)
			{
				obj.Volume = 100f;
			}
		});
		base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, CachedHead.transform, CachedHead.transform.position, eulersAngles);
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(1f);
		base.PlayCrowdHappySound();
	}

	protected override void PlayCrowdHappySound()
	{
	}

	private IEnumerator CoroutineAnimationFinished()
	{
		if (base.CachedManager.CurrentCharacter.IsFlipped)
		{
			ParticleSystem[] componentsInChildren = holeEffect.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Vector3 localScale = componentsInChildren[i].transform.localScale;
				localScale.x *= -1f;
				componentsInChildren[i].transform.localScale = localScale;
			}
		}
		yield return new WaitForSeconds(delayCrabAppear);
		if (crabPrefab != null)
		{
			crab = Object.Instantiate(crabPrefab, base.CachedManager.CurrentCharacter.HeadColliderBody.transform.position + portalOffset, Quaternion.identity, base.CachedManager.CurrentCharacter.transform);
			crab.OnHeadTouched += StartCoroutineOnTouched;
		}
	}

	private IEnumerator CoroutineOnTouched()
	{
		AudioManager.PlayAudioWithKey("crowbar_fatality_crab_crawl");
		yield return new WaitForSeconds(timeBeforeHeadExplode);
		if (explodeEffectPrefab != null && !string.IsNullOrEmpty(explodeEffectPrefab.assetGUID))
		{
			Object.Instantiate((GameObject)explodeEffectPrefab.GetAsset(), crab.gameObject.transform.position, Quaternion.identity, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
		}
		IsHeadDeleted = true;
		CrabDelete();
		crab.gameObject.SetActive(false);
		ExplodeCharacter();
	}

	private void CrabDelete()
	{
		Skeleton skeleton = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter.SkeletonAnimation.Skeleton;
		if (skeleton.FindSlot("crab_head") != null)
		{
			skeleton.SetAttachment("crab_head", null);
		}
		if (skeleton.FindSlot("crab_leg1") != null)
		{
			skeleton.SetAttachment("crab_leg1", null);
		}
		if (skeleton.FindSlot("crab_leg2") != null)
		{
			skeleton.SetAttachment("crab_leg2", null);
		}
		if (skeleton.FindSlot("crab_leg3") != null)
		{
			skeleton.SetAttachment("crab_leg3", null);
		}
		if (skeleton.FindSlot("crab_body") != null)
		{
			skeleton.SetAttachment("crab_body", null);
		}
	}

	private void StartCoroutineOnTouched()
	{
		StartCoroutine(CoroutineOnTouched());
	}
}
