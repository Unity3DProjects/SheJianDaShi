using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityTurkey : MonoBehaviour
{
	public static Action<Transform> OnTurkeyBite;

	[SerializeField]
	private Rigidbody turkeyRigidbody;

	[SerializeField]
	private List<Collider> turkeyColliders;

	[SerializeField]
	private SkeletonAnimation turkeyAnimation;

	[SerializeField]
	private GameObject beak;

	[SerializeField]
	[Range(0f, 1f)]
	private float biteAnimationProbability;

	[SerializeField]
	[Range(0f, 1f)]
	private float biteFlyAnimationProbability;

	[SerializeField]
	[Range(0f, 1f)]
	private float idleAnimationProbability;

	[SerializeField]
	private TweenPosition turkeyTweenPosition;

	[SerializeField]
	[ResourceLink]
	private AssetLink landingEffect;

	[SerializeField]
	private Transform landingEffectParent;

	[SerializeField]
	[ResourceLink]
	private AssetLink idleEffect;

	[SerializeField]
	private Transform idleEffectParent;

	[SerializeField]
	private TweenPosition flyTween;

	private Spine.Animation cachedFlyAnimation;

	private Spine.Animation cachedFlyEndAnimation;

	private Spine.Animation cachedStartFlyAnimation;

	private Spine.Animation cachedStartFlyEndAnimation;

	private Spine.Animation cachedLandingAnimation;

	private Spine.Animation cachedBiteAnimation;

	private Spine.Animation cachedBiteFlyAnimation;

	private Spine.Animation cachedIdleAnimation;

	private EventData cachedEventDataBite;

	private bool isTurkeyLanded;

	private bool isTurkeyAboveOrBelowCharacter;

	private bool isNeedToFly;

	public List<Collider> TurkeyColliders
	{
		get
		{
			return turkeyColliders;
		}
	}

	public Vector3 TurkeyAnimationLocalScale
	{
		get
		{
			return turkeyAnimation.transform.localScale;
		}
		set
		{
			if (turkeyAnimation.transform.localScale != value)
			{
				turkeyAnimation.transform.localScale = value;
				if (turkeyAnimation.transform.localScale.x < 0f)
				{
					turkeyTweenPosition.endPosition *= -1f;
				}
			}
		}
	}

	private Spine.Animation CachedFlyAnimation
	{
		get
		{
			if (cachedFlyAnimation == null)
			{
				cachedFlyAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("fly");
			}
			return cachedFlyAnimation;
		}
	}

	private Spine.Animation CachedFlyEndAnimation
	{
		get
		{
			if (cachedFlyEndAnimation == null)
			{
				cachedFlyEndAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("fly_end");
			}
			return cachedFlyEndAnimation;
		}
	}

	private Spine.Animation CachedLandingAnimation
	{
		get
		{
			if (cachedLandingAnimation == null)
			{
				cachedLandingAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("landing");
			}
			return cachedLandingAnimation;
		}
	}

	private Spine.Animation CachedBiteAnimation
	{
		get
		{
			if (cachedBiteAnimation == null)
			{
				cachedBiteAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("bite");
			}
			return cachedBiteAnimation;
		}
	}

	private Spine.Animation CachedBiteFlyAnimation
	{
		get
		{
			if (cachedBiteFlyAnimation == null)
			{
				cachedBiteFlyAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("bite_fly");
			}
			return cachedBiteFlyAnimation;
		}
	}

	private Spine.Animation CachedStartFlyAnimation
	{
		get
		{
			if (cachedStartFlyAnimation == null)
			{
				cachedStartFlyAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("start_fly");
			}
			return cachedStartFlyAnimation;
		}
	}

	private Spine.Animation CachedStartFlyEndAnimation
	{
		get
		{
			if (cachedStartFlyEndAnimation == null)
			{
				cachedStartFlyEndAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("start_fly_end");
			}
			return cachedStartFlyEndAnimation;
		}
	}

	private Spine.Animation CachedIdleAnimation
	{
		get
		{
			if (cachedIdleAnimation == null)
			{
				cachedIdleAnimation = turkeyAnimation.Skeleton.Data.FindAnimation("idle");
			}
			return cachedIdleAnimation;
		}
	}

	private EventData CachedEventDataBite
	{
		get
		{
			if (cachedEventDataBite == null)
			{
				cachedEventDataBite = turkeyAnimation.Skeleton.Data.FindEvent("Bite");
			}
			return cachedEventDataBite;
		}
	}

	public bool IsFlyingTurkey
	{
		get
		{
			return isNeedToFly;
		}
	}

	private void Awake()
	{
		turkeyRigidbody.isKinematic = false;
		turkeyAnimation.State.SetAnimation(0, CachedFlyAnimation, true);
		turkeyAnimation.timeScale = UnityEngine.Random.Range(1f, 2f);
	}

	private void OnEnable()
	{
		turkeyAnimation.State.Event += TurkeyAnimation_State_Event;
		turkeyAnimation.State.Complete += TurkeyAnimation_State_Complete;
		FatalityTurkeys.OnBrainEmitted = (Action)Delegate.Combine(FatalityTurkeys.OnBrainEmitted, new Action(FatalityTurkeys_OnBrainEmitted));
	}

	private void OnDisable()
	{
		turkeyAnimation.State.Event -= TurkeyAnimation_State_Event;
		turkeyAnimation.State.Complete -= TurkeyAnimation_State_Complete;
		FatalityTurkeys.OnBrainEmitted = (Action)Delegate.Remove(FatalityTurkeys.OnBrainEmitted, new Action(FatalityTurkeys_OnBrainEmitted));
	}

	private void Update()
	{
		if (isNeedToFly && !SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused)
		{
			base.transform.position += new Vector3((!(turkeyAnimation.transform.localScale.x < 0f)) ? UnityEngine.Random.Range(3f, 7f) : UnityEngine.Random.Range(-3f, -7f), UnityEngine.Random.Range(4f, 10f), 0f);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			turkeyRigidbody.isKinematic = true;
		}, 0f);
		for (int num = 0; num < turkeyColliders.Count; num++)
		{
			turkeyColliders[num].enabled = false;
		}
		if (landingEffect != null && !string.IsNullOrEmpty(landingEffect.assetGUID))
		{
			InstantiatePrefab((GameObject)landingEffect.GetAsset(), landingEffectParent, landingEffectParent.position);
		}
		if (idleEffect != null && !string.IsNullOrEmpty(idleEffect.assetGUID))
		{
			InstantiatePrefab((GameObject)idleEffect.GetAsset(), idleEffectParent, idleEffectParent.position);
		}
		turkeyAnimation.State.SetAnimation(0, CachedLandingAnimation, false);
		if ((base.transform.localRotation.eulerAngles.z < 25f || 360f - base.transform.localRotation.eulerAngles.z < 25f) && Mathf.Abs(base.transform.localPosition.y) < 50f)
		{
			base.transform.localRotation = Quaternion.Euler(new Vector3(base.transform.localRotation.eulerAngles.x, base.transform.localRotation.eulerAngles.y, 0f));
		}
		else
		{
			isTurkeyAboveOrBelowCharacter = true;
			base.transform.localPosition += Vector3.down * 15f;
		}
		isTurkeyLanded = true;
	}

	public void Fly()
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			base.transform.localRotation = Quaternion.Euler(new Vector3(base.transform.localRotation.eulerAngles.x, base.transform.localRotation.eulerAngles.y, 0f));
			turkeyAnimation.State.SetAnimation(0, CachedStartFlyEndAnimation, false);
			turkeyAnimation.State.AddAnimation(0, CachedFlyEndAnimation, true, 0f);
			isNeedToFly = true;
			flyTween.enabled = true;
		}, UnityEngine.Random.Range(0f, 2f));
	}

	private GameObject InstantiatePrefab(GameObject prefab, Transform parent, Vector3 position)
	{
		GameObject gameObject = null;
		if (prefab != null)
		{
			gameObject = UnityEngine.Object.Instantiate(prefab);
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.position = position;
		}
		return gameObject;
	}

	private void TurkeyAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataBite && OnTurkeyBite != null)
		{
			OnTurkeyBite(beak.transform);
		}
	}

	private void TurkeyAnimation_State_Complete(Spine.AnimationState state, int trackIndex, int loopCount)
	{
		if (!isTurkeyLanded || isNeedToFly)
		{
			return;
		}
		float num = 0f;
		num += biteAnimationProbability;
		num += biteFlyAnimationProbability;
		num += idleAnimationProbability;
		float num2 = UnityEngine.Random.Range(0f, num);
		if (num2 < biteAnimationProbability)
		{
			turkeyAnimation.State.SetAnimation(0, CachedBiteAnimation, false);
			return;
		}
		if (num2 > biteAnimationProbability && num2 < biteAnimationProbability + biteFlyAnimationProbability)
		{
			if (!isTurkeyAboveOrBelowCharacter)
			{
				turkeyTweenPosition.duration = turkeyAnimation.State.SetAnimation(0, CachedBiteFlyAnimation, false).Animation.Duration;
				turkeyTweenPosition.SetOnFinishedDelegate(delegate
				{
					if (!isNeedToFly)
					{
						turkeyAnimation.transform.localScale = new Vector3(-1f * turkeyAnimation.transform.localScale.x, turkeyAnimation.transform.localScale.y, turkeyAnimation.transform.localScale.z);
						turkeyTweenPosition.IsReversed = !turkeyTweenPosition.IsReversed;
					}
				});
				turkeyTweenPosition.enabled = true;
			}
			else
			{
				turkeyAnimation.State.SetAnimation(0, CachedIdleAnimation, false);
			}
			return;
		}
		turkeyTweenPosition.duration = turkeyAnimation.State.SetAnimation(0, CachedIdleAnimation, false).Animation.Duration;
		turkeyTweenPosition.SetOnFinishedDelegate(delegate
		{
			if (!isNeedToFly)
			{
				turkeyAnimation.transform.localScale = new Vector3(-1f * turkeyAnimation.transform.localScale.x, turkeyAnimation.transform.localScale.y, turkeyAnimation.transform.localScale.z);
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -1f * base.transform.localPosition.z);
				turkeyTweenPosition.IsReversed = !turkeyTweenPosition.IsReversed;
			}
		});
		if (!isTurkeyAboveOrBelowCharacter)
		{
			turkeyTweenPosition.enabled = true;
		}
	}

	private void FatalityTurkeys_OnBrainEmitted()
	{
		Rigidbody[] componentsInChildren = beak.transform.GetComponentsInChildren<Rigidbody>();
		if (componentsInChildren.Length > 0)
		{
			base.transform.localRotation = Quaternion.Euler(new Vector3(base.transform.localRotation.eulerAngles.x, base.transform.localRotation.eulerAngles.y, 0f));
			base.transform.localPosition += Vector3.back * 50f;
			turkeyAnimation.State.SetAnimation(0, CachedStartFlyEndAnimation, false);
			turkeyAnimation.State.AddAnimation(0, CachedFlyEndAnimation, true, 0f);
			isNeedToFly = true;
			flyTween.enabled = true;
		}
	}
}
