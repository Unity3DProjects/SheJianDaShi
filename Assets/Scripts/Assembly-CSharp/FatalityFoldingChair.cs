using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Spine;
using UnityEngine;

public class FatalityFoldingChair : FatalityBase
{
	private const string DEATH_ADAM_X_FATALITY = "adam_x_fatality";

	private const string KICK_EVENT = "Down_Fight";

	[SerializeField]
	private float delayBeforeStart = 3f;

	[SerializeField]
	private int bloodEffectMultiplier = 5;

	[SerializeField]
	[ResourceLink]
	private AssetLink smokeEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink flashEffectPrefab;

	[SerializeField]
	private Vector3 flashOffset;

	[SerializeField]
	private float height;

	[SerializeField]
	private float adamXfallOffset = -50f;

	[SerializeField]
	private float flashDuration = 2f;

	[SerializeField]
	private int flashFrequency = 5;

	[SerializeField]
	private float standUpDelay;

	[SerializeField]
	private string adamXFallingSound;

	[SerializeField]
	private float bodyPartsAppearDelay = 0.2f;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	protected List<string> breakBonesNames = new List<string>();

	private GameObject rightSmokeEffect;

	private GameObject leftSmokeEffect;

	private GameObject flashEffect;

	private Character adamX;

	private ObserverPhysics observer;

	private Spine.Animation cachedAnimationAdamXFatality;

	private EventData cachedEventIdleSpill;

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

	private SkeletonUtilityBone CachedBody
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

	private EventData CachedEventIdleSpill
	{
		get
		{
			if (cachedEventIdleSpill == null)
			{
				cachedEventIdleSpill = adamX.SkeletonAnimation.Skeleton.Data.FindEvent("Down_Fight");
			}
			return cachedEventIdleSpill;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			StartCoroutine(CoroutineAnimationFinished(1f, delayBeforeStart));
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		if (leftSmokeEffect != null)
		{
			Object.Destroy(leftSmokeEffect);
			leftSmokeEffect = null;
		}
		if (rightSmokeEffect != null)
		{
			Object.Destroy(rightSmokeEffect);
			rightSmokeEffect = null;
		}
		if (flashEffect != null)
		{
			Object.Destroy(flashEffect);
			flashEffect = null;
		}
	}

	protected override void PlayCrowdHappySound()
	{
	}

	private void SmokeCreation()
	{
		if (smokeEffectPrefab != null && !string.IsNullOrEmpty(smokeEffectPrefab.assetGUID))
		{
			Vector3 vector = new Vector3(flashOffset.x, 0f, 0f);
			float y = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(base.CachedManager.CurrentCharacter.transform.position.x).y;
			Vector3 vector2 = new Vector3(adamX.transform.position.x, y, 0f);
			if (smokeEffectPrefab != null)
			{
				rightSmokeEffect = (GameObject)smokeEffectPrefab.GetInstance();
				rightSmokeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
				rightSmokeEffect.transform.position = vector2 + vector;
				leftSmokeEffect = (GameObject)smokeEffectPrefab.GetInstance();
				leftSmokeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
				leftSmokeEffect.transform.position = vector2 - vector;
			}
		}
		AudioManager.PlayAudioWithKey("arrow_firework_main_explosion");
	}

	private IEnumerator AdamXTransposition()
	{
		AudioManager.PlayAudioWithKey(adamXFallingSound);
		if (adamX.IsFlipped)
		{
			adamXfallOffset *= -1f;
		}
		adamX.SkeletonAnimation.state.SetAnimation(0, cachedAnimationAdamXFatality, false);
		yield return new WaitForSeconds(0.01f);
		adamX.transform.position = base.CachedManager.CurrentCharacter.transform.position + Vector3.right * adamXfallOffset;
	}

	private IEnumerator ExplodeCharacter(float delay)
	{
		AudioManager.PlayAudioWithKey("pickaxe_fatality_explode");
		if (!GoreContentFilter.AllowInternalOrgans)
		{
			StartCoroutine(OnCharacterExploded(standUpDelay));
			yield break;
		}
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position + flashOffset, CachedHead.transform.eulerAngles);
		for (int i = 0; i < bloodEffectMultiplier; i++)
		{
			base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
		}
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(1f);
		yield return new WaitForSeconds(bodyPartsAppearDelay);
		base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
		StartCoroutine(OnCharacterExploded(standUpDelay));
	}

	private IEnumerator CoroutineAnimationFinished(float delayAnimation, float delayFatalityEffect)
	{
		yield return new WaitForSeconds(delayFatalityEffect);
		adamX = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter;
		adamX.SkeletonAnimation.State.Event += AdamX_SkeletonAnimation_State_Event;
		cachedAnimationAdamXFatality = adamX.SkeletonAnimation.Skeleton.Data.FindAnimation("adam_x_fatality");
		StartCoroutine(AdamXTransposition());
		yield return new WaitForSeconds(cachedAnimationAdamXFatality.Duration);
		SmokeCreation();
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_4", delegate(SoundObject obj)
		{
			if (obj != null)
			{
				obj.Volume = 0.5f;
			}
		});
		_003CPlayCrowdHappySound_003E__BaseCallProxy0();
	}

	private IEnumerator StartFotograghing(Vector3 beginPosition)
	{
		int count = (int)flashDuration * flashFrequency;
		float timeBeetweenFlashes = 1f / (float)flashFrequency;
		if (flashEffectPrefab == null || string.IsNullOrEmpty(flashEffectPrefab.assetGUID))
		{
			yield break;
		}
		for (int i = 0; i < count; i++)
		{
			if (flashEffect != null)
			{
				flashEffect = (GameObject)flashEffectPrefab.GetInstance();
				flashEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
				Vector3 delta = new Vector3((0.5f - Random.value) * flashOffset.x, Random.value * flashOffset.y, 0f) * 2f;
				flashEffect.transform.position = beginPosition + delta;
				yield return new WaitForSeconds(timeBeetweenFlashes);
			}
		}
	}

	private void AdamX_SkeletonAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventIdleSpill)
		{
			StartCoroutine(ExplodeCharacter(standUpDelay));
			adamX.SkeletonAnimation.State.Event -= AdamX_SkeletonAnimation_State_Event;
		}
	}

	private IEnumerator OnCharacterExploded(float delay)
	{
		yield return new WaitForSeconds(delay);
		adamX.ApplyCharacterRagdoll();
		adamX.RootRigidBody.transform.position = adamX.transform.transform.position;
		adamX.transform.position = adamX.transform.position;
		adamX.transform.rotation = Quaternion.identity;
		adamX.Restore();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CPlayCrowdHappySound_003E__BaseCallProxy0()
	{
		base.PlayCrowdHappySound();
	}
}
