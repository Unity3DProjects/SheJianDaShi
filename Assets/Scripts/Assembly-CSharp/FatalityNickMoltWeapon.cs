using Spine;
using UnityEngine;

public class FatalityNickMoltWeapon : FatalitySatelite
{
	private const string LIKE_FATALITY = "like_fatality";

	[SerializeField]
	private SkeletonAnimation likeAnimation;

	[SerializeField]
	[ResourceLink]
	private AssetLink likeEffectPrefab;

	private SkeletonAnimation currentAnimation;

	private EventData cachedEventCrash;

	private EventData CachedEventCrash
	{
		get
		{
			if (cachedEventCrash == null)
			{
				cachedEventCrash = currentAnimation.Skeleton.Data.FindEvent("Crash");
			}
			return cachedEventCrash;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (base.IsActive || !(likeAnimation != null))
		{
			return;
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			Vector3 position = base.CachedManager.CurrentCharacter.HeadCollider.transform.position - base.CachedManager.CurrentCharacter.AppleHeightOffset * Vector3.up;
			GameObject gameObject = Object.Instantiate(likeAnimation.gameObject, position, Quaternion.identity, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
			if (likeEffectPrefab != null && !string.IsNullOrEmpty(likeEffectPrefab.assetGUID))
			{
				Object.Instantiate((GameObject)likeEffectPrefab.GetAsset(), gameObject.transform.position, Quaternion.identity, gameObject.transform);
			}
			currentAnimation = gameObject.GetComponent<SkeletonAnimation>();
			currentAnimation.State.Event += LikeAnimation_State_Event;
			currentAnimation.State.Complete += LikeAnimation_State_Complete;
			currentAnimation.State.SetAnimation(0, "like_fatality", false);
		}, delayBeforeStart);
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		base.ExplodeCharacter();
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration();
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedHead.transform.position, base.CachedHead.transform.eulerAngles);
		base.CachedEffect.EmitBrain(base.transform, base.CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedBody.transform.position, base.CachedBody.transform.eulerAngles);
		base.CachedEffect.EmitGuts(base.transform, base.CachedBody.transform.position, Vector3.up);
	}

	private void LikeAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventCrash)
		{
			ExplodeCharacter();
		}
		currentAnimation.State.Event -= LikeAnimation_State_Event;
	}

	private void LikeAnimation_State_Complete(Spine.AnimationState state, int trackIndex, int loopCount)
	{
		currentAnimation.State.Complete -= LikeAnimation_State_Complete;
		currentAnimation.gameObject.SetActive(false);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			OnFatalityFinish(this);
		}, 2f);
	}
}
