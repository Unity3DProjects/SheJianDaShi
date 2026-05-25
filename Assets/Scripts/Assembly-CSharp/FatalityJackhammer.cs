using System.Collections;
using Spine;
using UnityEngine;

public class FatalityJackhammer : FatalityBase
{
	private const string CHARACTER_GET_UP_EVENT_NAME = "Get_Up";

	private const string BLOOD_EFFECT_EVENT_NAME = "FX_Blood";

	private const string CHARACTER_EXPLODE_EVENT_NAME = "Character_Explode";

	private const string CHARACTER_ANIMATION = "death_by_bur";

	private const string HEAD_BONE = "head";

	[SerializeField]
	private float boerDelay;

	[SerializeField]
	private float boerHeight;

	[SerializeField]
	private float getingUpDuration;

	[SerializeField]
	private float explodeCharacterForce;

	[SerializeField]
	[ResourceLink]
	private AssetLink bloodEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink headBloodEffectPrefab;

	[SerializeField]
	private SkeletonAnimation boerPrefab;

	[SerializeField]
	private float delayBetweenBloodEffects;

	[SerializeField]
	private int bloodEffectCount;

	[SerializeField]
	private SkeletonAnimation headPrefab;

	private SkeletonAnimation boer;

	private EventData cachedCharacterGetUpEvent;

	private EventData cachedBloodEffectEvent;

	private EventData cachedCharacterExplodeEvent;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private Skeleton cachedSkeleton;

	private bool isHeadDeleted;

	private EventData CachedCharacterGetUpEvent
	{
		get
		{
			if (cachedCharacterGetUpEvent == null)
			{
				cachedCharacterGetUpEvent = boer.Skeleton.Data.FindEvent("Get_Up");
			}
			return cachedCharacterGetUpEvent;
		}
	}

	private EventData CachedBloodEffectEvent
	{
		get
		{
			if (cachedBloodEffectEvent == null)
			{
				cachedBloodEffectEvent = boer.Skeleton.Data.FindEvent("FX_Blood");
			}
			return cachedBloodEffectEvent;
		}
	}

	private EventData CachedCharacterExplodeEvent
	{
		get
		{
			if (cachedCharacterExplodeEvent == null)
			{
				cachedCharacterExplodeEvent = boer.Skeleton.Data.FindEvent("Character_Explode");
			}
			return cachedCharacterExplodeEvent;
		}
	}

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

	private Skeleton CachedSkeleton
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
			if (value && !GoreContentFilter.AllowDismemberment)
			{
				return;
			}
			isHeadDeleted = value;
			CachedSkeleton.FindBone("head").ScaleX = ((!isHeadDeleted) ? 1 : 0);
			CachedSkeleton.FindBone("head").ScaleY = ((!isHeadDeleted) ? 1 : 0);
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		base.StartFatality(infoCollision);
		base.CachedManager.CurrentCharacter.SetAnimation(0, "finish_him", true);
		StartCoroutine(StartBoer());
	}

	public override void StopFatality()
	{
		base.StopFatality();
		AudioManager.StopAudioWithKey("fatality_jackhammer_boer");
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		base.CachedManager.CurrentCharacter.RemoveAllDriveIntoWeapon();
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if (!GoreContentFilter.AllowDismemberment)
		{
			SkeletonUtilityBone utilityBoneBodyForSafeRagdoll = base.CachedManager.GetUtilityBoneBody();
			if ((bool)utilityBoneBodyForSafeRagdoll)
			{
				Rigidbody rigidbodyForSafeRagdoll = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBodyForSafeRagdoll.boneName);
				if ((bool)rigidbodyForSafeRagdoll)
				{
					rigidbodyForSafeRagdoll.velocity = Vector3.up * explodeCharacterForce * 0.25f;
					rigidbodyForSafeRagdoll.constraints = (RigidbodyConstraints)56;
				}
			}
			base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
			return;
		}
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		SkeletonUtilityBone utilityBoneHead = base.CachedManager.GetUtilityBoneHead();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Rigidbody rigidbody2 = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneHead.boneName);
			Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if ((bool)rigidbody && rigidbodyArray != null && rigidbodyArray.Length > 0)
			{
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.forward * 30f;
				rigidbody.gameObject.GetComponent<MeshCollider>().enabled = false;
				SphereCollider sphereCollider = rigidbody.gameObject.AddComponent<SphereCollider>();
				sphereCollider.radius = 50f;
				for (int i = 0; i < rigidbodyArray.Length; i++)
				{
					if (rigidbodyArray[i] != rigidbody && rigidbodyArray[i] != null)
					{
						if (rigidbodyArray[i] != rigidbody2)
						{
							Quaternion quaternion = Quaternion.LookRotation(rigidbodyArray[i].transform.position - rigidbody.transform.position, Vector3.forward);
							rigidbodyArray[i].velocity = quaternion * new Vector3(explodeCharacterForce, 0f, 0f);
							rigidbodyArray[i].constraints = (RigidbodyConstraints)56;
						}
						else
						{
							rigidbodyArray[i].constraints = RigidbodyConstraints.FreezeAll;
							Object.Instantiate(headBloodEffectPrefab.GetAsset(), rigidbodyArray[i].transform.position, Quaternion.Euler(0f, 0f, -90f), rigidbodyArray[i].transform);
							Object.Instantiate(headBloodEffectPrefab.GetAsset(), rigidbodyArray[i].transform.position, Quaternion.Euler(0f, 0f, 45f), rigidbodyArray[i].transform);
							Object.Instantiate(headBloodEffectPrefab.GetAsset(), rigidbodyArray[i].transform.position, Quaternion.Euler(0f, 0f, 135f), rigidbodyArray[i].transform);
						}
					}
					BreakAllJoint(rigidbodyArray[i]);
				}
			}
		}
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, Vector3.zero);
		base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, Vector3.zero);
		base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
		IsHeadDeleted = true;
		SkeletonAnimation skeletonAnimation = Object.Instantiate(headPrefab, base.CachedManager.transform.position, Quaternion.identity, base.transform);
		skeletonAnimation.State.SetAnimation(0, "animation", true);
	}

	private IEnumerator StartBoer()
	{
		yield return new WaitForSeconds(boerDelay);
		if (boerPrefab != null)
		{
			boer = Object.Instantiate(boerPrefab, base.CachedManager.CurrentCharacter.transform.position, Quaternion.identity, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
			boer.State.Event += Boer_State_Event;
		}
		StartAnimation(base.CachedManager.AnimationJackhammer);
		AudioManager.PlayAudioWithKey("fatality_jackhammer_boer");
	}

	private IEnumerator StartBloodEffects()
	{
		if (bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
		{
			for (int i = 0; i < bloodEffectCount; i++)
			{
				GameObject bloodEffect = (GameObject)bloodEffectPrefab.GetInstance();
				bloodEffect.transform.position = base.CachedManager.CurrentCharacter.BodyTransform.position;
				bloodEffect.transform.parent = base.transform;
				AudioManager.PlayAudioWithKey("blood_fountain_loop");
				yield return new WaitForSeconds(delayBetweenBloodEffects);
			}
		}
	}

	private void Boer_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedBloodEffectEvent)
		{
			StartCoroutine(StartBloodEffects());
		}
		if (e.Data == CachedCharacterGetUpEvent)
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(20f, 20f);
		}
		if (e.Data == CachedCharacterExplodeEvent)
		{
			ExplodeCharacter();
		}
	}
}
