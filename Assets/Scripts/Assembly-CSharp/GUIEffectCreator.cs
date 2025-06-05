using Spine;
using UnityEngine;

public class GUIEffectCreator : MonoBehaviour
{
	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private Transform bodyTransform;

	private CharacterAnimationCache animationCache;

	private GameObject idleEffect;

	private GameObject idleEffectLeftHand;

	private GameObject idleEffectLeftHandByEvent;

	private GameObject idleEffectLeftHandByEventSecond;

	private GameObject effectBroomIdle;

	private GameObject effectBroomSplash;

	private GameObject effectHammerFirst;

	private GameObject effectHammerSecond;

	private GameObject effectSkull;

	internal EmissionCharacter cachedEmission;

	private EventData cachedEventIdleSpill;

	private EventData cachedEventIdleSpillEnd;

	private EventData cachedEventIdleSpillBeer;

	private EventData cachedEventIdleSpillBeerEnd;

	private EventData cachedEventIdleSpillFries;

	private EventData cachedEventIdleSpillFriesEnd;

	private EventData cachedEventIdleMoney;

	private EventData cachedEventIdleMoneyEnd;

	private EventData cachedEventBroomIdleAppear;

	private EventData cachedEventBroomIdleDisappear;

	private EventData cachedEventBroomSplash;

	private EventData cachedEventHammerFirstActivate;

	private EventData cachedEventHammerFirstDeactivate;

	private EventData cachedEventHammerSecondActivate;

	private EventData cachedEventHammerSecondDeactivate;

	private EventData cachedEventSteam;

	private SkeletonAnimation SkeletonAnimation
	{
		get
		{
			return skeletonAnimation;
		}
	}

	public Transform BodyTransform
	{
		get
		{
			return bodyTransform;
		}
	}

	private CharacterAnimationCache AnimationChache
	{
		get
		{
			return animationCache;
		}
	}

	private EmissionCharacter CachedEmission
	{
		get
		{
			if (cachedEmission == null)
			{
				cachedEmission = GetComponent<EmissionCharacter>();
			}
			return cachedEmission;
		}
	}

	private GameObject IdleEffect
	{
		get
		{
			if (idleEffect == null && (bool)CachedEmission)
			{
				idleEffect = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero);
			}
			return idleEffect;
		}
	}

	private GameObject IdleEffectLeftHand
	{
		get
		{
			if (idleEffectLeftHand == null && (bool)CachedEmission)
			{
				idleEffectLeftHand = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.LeftArm);
			}
			return idleEffectLeftHand;
		}
	}

	private GameObject IdleEffectLeftHandByEvent
	{
		get
		{
			if (idleEffectLeftHandByEvent == null && (bool)CachedEmission)
			{
				idleEffectLeftHandByEvent = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.LeftArmByEvent);
				if (idleEffectLeftHandByEvent != null)
				{
					if (GetComponent<CharacterBase>() != null && GetComponent<CharacterBase>().IsFlipped)
					{
						idleEffectLeftHandByEvent.transform.localScale = new Vector3(-1f, 1f, 1f);
						if (CachedEmission.IsUnflippedWithScale)
						{
							Transform[] componentsInChildren = idleEffectLeftHandByEvent.GetComponentsInChildren<Transform>();
							Transform[] array = componentsInChildren;
							foreach (Transform transform in array)
							{
								if ((bool)transform.GetComponent<GUIEffectsScale>())
								{
									transform.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
								}
							}
						}
					}
					idleEffectLeftHandByEvent.SetActive(false);
					idleEffectLeftHandByEvent.StopFX(true);
				}
			}
			return idleEffectLeftHandByEvent;
		}
	}

	private GameObject IdleEffectLeftHandByEventSecond
	{
		get
		{
			if (idleEffectLeftHandByEventSecond == null && (bool)CachedEmission)
			{
				idleEffectLeftHandByEventSecond = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.LeftArmByEventSecond);
				if (idleEffectLeftHandByEventSecond != null)
				{
					if (GetComponent<CharacterBase>() != null && GetComponent<CharacterBase>().IsFlipped)
					{
						idleEffectLeftHandByEventSecond.transform.localScale = new Vector3(-1f, 1f, 1f);
					}
					idleEffectLeftHandByEventSecond.SetActive(false);
					idleEffectLeftHandByEventSecond.StopFX(true);
				}
			}
			return idleEffectLeftHandByEventSecond;
		}
	}

	private GameObject EffectBroomIdle
	{
		get
		{
			if (effectBroomIdle == null && (bool)CachedEmission)
			{
				effectBroomIdle = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.Broom);
			}
			return effectBroomIdle;
		}
	}

	private GameObject EffectBroomSplash
	{
		get
		{
			if (effectBroomSplash == null && (bool)CachedEmission)
			{
				effectBroomSplash = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.BroomSplash);
			}
			return effectBroomSplash;
		}
	}

	private GameObject EffectHammerFirst
	{
		get
		{
			if (effectHammerFirst == null && (bool)CachedEmission)
			{
				effectHammerFirst = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.HammerFirst);
			}
			return effectHammerFirst;
		}
	}

	private GameObject EffectHammerSecond
	{
		get
		{
			if (effectHammerSecond == null && (bool)CachedEmission)
			{
				effectHammerSecond = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.HammerSecond);
			}
			return effectHammerSecond;
		}
	}

	private GameObject EffectSkull
	{
		get
		{
			if (effectSkull == null && (bool)CachedEmission)
			{
				effectSkull = CachedEmission.StartIdleEffect(base.transform, base.transform.position, Vector3.zero, EmitType.Shoot, EmissionCharacter.IdleEffectPositionType.Skull);
			}
			return effectSkull;
		}
	}

	private EventData CachedEventIdleSpill
	{
		get
		{
			if (cachedEventIdleSpill == null)
			{
				cachedEventIdleSpill = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Spill");
			}
			return cachedEventIdleSpill;
		}
	}

	private EventData CachedEventIdleSpillEnd
	{
		get
		{
			if (cachedEventIdleSpillEnd == null)
			{
				cachedEventIdleSpillEnd = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Spill_End");
			}
			return cachedEventIdleSpillEnd;
		}
	}

	private EventData CachedEventIdleSpillFries
	{
		get
		{
			if (cachedEventIdleSpill == null)
			{
				cachedEventIdleSpillFries = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Spill_Fries");
			}
			return cachedEventIdleSpillFries;
		}
	}

	private EventData CachedEventIdleSpillFriesEnd
	{
		get
		{
			if (cachedEventIdleSpillEnd == null)
			{
				cachedEventIdleSpillFriesEnd = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Spill_Fries_End");
			}
			return cachedEventIdleSpillFriesEnd;
		}
	}

	private EventData CachedEventIdleSpillBeer
	{
		get
		{
			if (cachedEventIdleSpill == null)
			{
				cachedEventIdleSpillBeer = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Spill_Beer");
			}
			return cachedEventIdleSpillBeer;
		}
	}

	private EventData CachedEventIdleSpillBeerEnd
	{
		get
		{
			if (cachedEventIdleSpillEnd == null)
			{
				cachedEventIdleSpillBeerEnd = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Spill_Beer_End");
			}
			return cachedEventIdleSpillBeerEnd;
		}
	}

	private EventData CachedEventIdleMoney
	{
		get
		{
			if (cachedEventIdleMoney == null)
			{
				cachedEventIdleMoney = skeletonAnimation.Skeleton.Data.FindEvent("Money_start");
			}
			return cachedEventIdleMoney;
		}
	}

	private EventData CachedEventIdleMoneyEnd
	{
		get
		{
			if (cachedEventIdleMoneyEnd == null)
			{
				cachedEventIdleMoneyEnd = skeletonAnimation.Skeleton.Data.FindEvent("Money_end");
			}
			return cachedEventIdleMoneyEnd;
		}
	}

	private EventData CachedEventBroomIdleAppear
	{
		get
		{
			if (cachedEventBroomIdleAppear == null)
			{
				cachedEventBroomIdleAppear = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Appear");
			}
			return cachedEventBroomIdleAppear;
		}
	}

	private EventData CachedEventBroomIdleDisappear
	{
		get
		{
			if (cachedEventBroomIdleDisappear == null)
			{
				cachedEventBroomIdleDisappear = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Disappear");
			}
			return cachedEventBroomIdleDisappear;
		}
	}

	private EventData CachedEventBroomSplash
	{
		get
		{
			if (cachedEventBroomSplash == null)
			{
				cachedEventBroomSplash = skeletonAnimation.Skeleton.Data.FindEvent("Idle_Effect");
			}
			return cachedEventBroomSplash;
		}
	}

	private EventData CachedEventHammerFirstActivate
	{
		get
		{
			if (cachedEventHammerFirstActivate == null)
			{
				cachedEventHammerFirstActivate = skeletonAnimation.Skeleton.Data.FindEvent("Effect_hammer_on");
			}
			return cachedEventHammerFirstActivate;
		}
	}

	private EventData CachedEventHammerSecondActivate
	{
		get
		{
			if (cachedEventHammerSecondActivate == null)
			{
				cachedEventHammerSecondActivate = skeletonAnimation.Skeleton.Data.FindEvent("Effect_hammer2_on");
			}
			return cachedEventHammerSecondActivate;
		}
	}

	private EventData CachedEventHammerFirstDeactivate
	{
		get
		{
			if (cachedEventHammerFirstDeactivate == null)
			{
				cachedEventHammerFirstDeactivate = skeletonAnimation.Skeleton.Data.FindEvent("Effect_hammer_off");
			}
			return cachedEventHammerFirstDeactivate;
		}
	}

	private EventData CachedEventHammerSecondDeactivate
	{
		get
		{
			if (cachedEventHammerSecondDeactivate == null)
			{
				cachedEventHammerSecondDeactivate = skeletonAnimation.Skeleton.Data.FindEvent("Effect_hammer2_off");
			}
			return cachedEventHammerSecondDeactivate;
		}
	}

	private EventData CachedEventSteam
	{
		get
		{
			if (cachedEventSteam == null)
			{
				cachedEventSteam = skeletonAnimation.Skeleton.Data.FindEvent("Steam");
			}
			return cachedEventSteam;
		}
	}

	private void OnEnable()
	{
		animationCache = new CharacterAnimationCache();
		animationCache.SkeletonAnimation = skeletonAnimation;
		animationCache.AnimationState.Start += AnimationStart_Event;
		animationCache.AnimationState.Complete += AnimationComplete_Event;
		skeletonAnimation.State.Event += SkeletonAnimation_State_Event;
	}

	private void OnDisable()
	{
		animationCache.AnimationState.Start -= AnimationStart_Event;
		animationCache.AnimationState.Complete -= AnimationComplete_Event;
		skeletonAnimation.State.Event -= SkeletonAnimation_State_Event;
	}

	private void Update()
	{
		if (IdleEffectLeftHandByEvent != null)
		{
			IdleEffectLeftHandByEvent.transform.rotation = Quaternion.identity;
		}
		if (IdleEffectLeftHandByEventSecond != null)
		{
			IdleEffectLeftHandByEventSecond.transform.rotation = Quaternion.identity;
		}
	}

	private void AnimationStart_Event(Spine.AnimationState state, int trackIndex)
	{
		if (state.tracks.Items[trackIndex].Animation.Name == animationCache.IdleAnimationName)
		{
			if ((bool)IdleEffect)
			{
				IdleEffect.SetActive(true);
				IdleEffect.PlayFX();
			}
			if ((bool)IdleEffectLeftHand)
			{
				IdleEffectLeftHand.SetActive(true);
				IdleEffectLeftHand.PlayFX();
			}
			return;
		}
		if ((bool)IdleEffect)
		{
			IdleEffect.SetActive(false);
			IdleEffect.StopFX(true);
		}
		if ((bool)IdleEffectLeftHand)
		{
			IdleEffectLeftHand.SetActive(false);
			IdleEffectLeftHand.StopFX(true);
		}
		if ((bool)IdleEffectLeftHandByEvent)
		{
			IdleEffectLeftHandByEvent.SetActive(false);
			IdleEffectLeftHandByEvent.StopFX(true);
		}
		if ((bool)IdleEffectLeftHandByEventSecond)
		{
			IdleEffectLeftHandByEventSecond.SetActive(false);
			IdleEffectLeftHandByEventSecond.StopFX(true);
		}
		if ((bool)EffectBroomIdle)
		{
			EffectBroomIdle.SetActive(false);
			EffectBroomIdle.StopFX(true);
		}
		if ((bool)EffectBroomSplash)
		{
			EffectBroomSplash.SetActive(false);
			EffectBroomSplash.StopFX(true);
		}
		if ((bool)EffectHammerFirst)
		{
			EffectHammerFirst.SetActive(false);
			EffectHammerFirst.StopFX(true);
		}
		if ((bool)EffectHammerSecond)
		{
			EffectHammerSecond.SetActive(false);
			EffectHammerSecond.StopFX(true);
		}
		if ((bool)EffectSkull)
		{
			EffectSkull.SetActive(false);
			EffectSkull.StopFX(true);
		}
	}

	private void AnimationComplete_Event(Spine.AnimationState state, int trackIndex, int loopCount)
	{
		AnimationStart_Event(state, trackIndex);
	}

	private void SkeletonAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventIdleSpill && (bool)IdleEffectLeftHandByEvent)
		{
			IdleEffectLeftHandByEvent.SetActive(true);
			IdleEffectLeftHandByEvent.PlayFX();
		}
		if (e.Data == CachedEventIdleSpillEnd && (bool)IdleEffectLeftHandByEvent)
		{
			IdleEffectLeftHandByEvent.SetActive(false);
			IdleEffectLeftHandByEvent.StopFX(true);
		}
		if (e.Data == CachedEventIdleSpillBeer && (bool)IdleEffectLeftHandByEvent)
		{
			IdleEffectLeftHandByEvent.SetActive(true);
			IdleEffectLeftHandByEvent.PlayFX();
		}
		if (e.Data == CachedEventIdleSpillBeerEnd && (bool)IdleEffectLeftHandByEvent)
		{
			IdleEffectLeftHandByEvent.SetActive(false);
			IdleEffectLeftHandByEvent.StopFX(true);
		}
		if (e.Data == CachedEventIdleSpillFries && (bool)IdleEffectLeftHandByEventSecond)
		{
			IdleEffectLeftHandByEventSecond.SetActive(true);
			IdleEffectLeftHandByEventSecond.PlayFX();
		}
		if (e.Data == CachedEventIdleSpillFriesEnd && (bool)IdleEffectLeftHandByEventSecond)
		{
			IdleEffectLeftHandByEventSecond.SetActive(false);
			IdleEffectLeftHandByEventSecond.StopFX(true);
		}
		if (e.Data == CachedEventIdleMoney && (bool)IdleEffectLeftHandByEvent)
		{
			IdleEffectLeftHandByEvent.SetActive(true);
			IdleEffectLeftHandByEvent.PlayFX();
		}
		if (e.Data == CachedEventIdleMoneyEnd && (bool)IdleEffectLeftHandByEvent)
		{
			IdleEffectLeftHandByEvent.SetActive(false);
			IdleEffectLeftHandByEvent.StopFX(true);
		}
		if (e.Data == CachedEventBroomIdleAppear && (bool)EffectBroomIdle)
		{
			EffectBroomIdle.SetActive(true);
			EffectBroomIdle.PlayFX();
		}
		if (e.Data == CachedEventBroomIdleDisappear && (bool)EffectBroomIdle)
		{
			EffectBroomIdle.SetActive(false);
			EffectBroomIdle.StopFX(true);
		}
		if (e.Data == CachedEventBroomSplash && (bool)EffectBroomSplash)
		{
			EffectBroomSplash.SetActive(true);
			EffectBroomSplash.PlayFX();
		}
		if (e.Data == CachedEventHammerFirstActivate && (bool)EffectHammerFirst)
		{
			EffectHammerFirst.SetActive(true);
			EffectHammerFirst.PlayFX();
		}
		if (e.Data == CachedEventHammerSecondActivate && (bool)EffectHammerSecond)
		{
			EffectHammerSecond.SetActive(true);
			EffectHammerSecond.PlayFX();
		}
		if (e.Data == CachedEventHammerFirstDeactivate && (bool)EffectHammerFirst)
		{
			EffectHammerFirst.SetActive(false);
			EffectHammerFirst.StopFX(true);
		}
		if (e.Data == CachedEventHammerSecondDeactivate && (bool)EffectHammerSecond)
		{
			EffectHammerSecond.SetActive(false);
			EffectHammerSecond.StopFX(true);
		}
		if (e.Data == CachedEventSteam && (bool)EffectSkull)
		{
			EffectSkull.SetActive(true);
			EffectSkull.PlayFX();
		}
	}
}
