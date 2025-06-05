using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class MisteryBox : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnOpenEnd;

	[SerializeField]
	private float misteryBoxAppearDelay = 2.7f;

	[SerializeField]
	private Collider misteryBoxCollider;

	[SerializeField]
	private Rigidbody misteryBoxRigidBody;

	[SerializeField]
	private SkeletonAnimation misteryBoxAnimation;

	[SerializeField]
	private Transform effectTransform;

	private Spine.Animation cachedAppearAnimation;

	private Spine.Animation cachedIdleAnimation;

	private Spine.Animation cachedDisappearAnimation;

	private EventData cachedEventDataOpenEnd;

	private Managers cachedManagers;

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	public Collider MisteryBoxCollider
	{
		get
		{
			return misteryBoxCollider;
		}
	}

	private Spine.Animation CachedAppearAnimation
	{
		get
		{
			if (cachedAppearAnimation == null)
			{
				cachedAppearAnimation = misteryBoxAnimation.Skeleton.Data.FindAnimation("appear");
			}
			return cachedAppearAnimation;
		}
	}

	private Spine.Animation CachedIdleAnimation
	{
		get
		{
			if (cachedIdleAnimation == null)
			{
				cachedIdleAnimation = misteryBoxAnimation.Skeleton.Data.FindAnimation("idle");
			}
			return cachedIdleAnimation;
		}
	}

	private Spine.Animation CachedDisappearAnimation
	{
		get
		{
			if (cachedDisappearAnimation == null)
			{
				cachedDisappearAnimation = misteryBoxAnimation.Skeleton.Data.FindAnimation("opening");
			}
			return cachedDisappearAnimation;
		}
	}

	private EventData CachedEventDataOpenEnd
	{
		get
		{
			if (cachedEventDataOpenEnd == null)
			{
				cachedEventDataOpenEnd = misteryBoxAnimation.Skeleton.Data.FindEvent("Opening");
			}
			return cachedEventDataOpenEnd;
		}
	}

	public event Action OnOpenEnd
	{
		add
		{
			Action action = this.m_OnOpenEnd;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnOpenEnd, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnOpenEnd;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnOpenEnd, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		misteryBoxRigidBody.isKinematic = false;
	}

	private void OnEnable()
	{
		misteryBoxAnimation.State.Event += MisteryBoxAnimation_State_Event;
		misteryBoxAnimation.State.End += MisteryBoxAnimation_State_End;
	}

	private void OnDisable()
	{
		misteryBoxAnimation.State.Event -= MisteryBoxAnimation_State_Event;
		misteryBoxAnimation.State.End -= MisteryBoxAnimation_State_End;
	}

	private void OnCollisionEnter(Collision collision)
	{
		misteryBoxRigidBody.isKinematic = true;
		misteryBoxCollider.isTrigger = true;
		base.transform.localRotation = Quaternion.identity;
	}

	public void Appear()
	{
		GameObject effect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.MisteryBoxSpawn));
		effect.transform.parent = effectTransform;
		effect.transform.localPosition = Vector3.zero;
		misteryBoxCollider.gameObject.SetActive(false);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			misteryBoxAnimation.State.SetAnimation(0, CachedAppearAnimation, false);
			misteryBoxAnimation.State.AddAnimation(0, CachedIdleAnimation, true, 0f);
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				effect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.MisteryBoxIdle));
				effect.transform.parent = effectTransform;
				effect.transform.localPosition = Vector3.zero;
			}, 0f);
			misteryBoxCollider.gameObject.SetActive(true);
			AudioManager.PlayAudioWithKey("respawn");
		}, misteryBoxAppearDelay);
	}

	public void GetAward()
	{
		CachedManagers.CachedGameManager.IsGamePaused = true;
		CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoShown)
		{
			CachedManagers.CachedGameManager.IsGamePaused = false;
			if (isVideoShown)
			{
				misteryBoxCollider.gameObject.SetActive(false);
				misteryBoxAnimation.State.SetAnimation(0, CachedDisappearAnimation, false);
			}
		});
	}

	private void MisteryBoxAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataOpenEnd)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				CachedManagers.CachedGiftManager.GenerateMisteryBoxAward(true);
			}, 0f);
		}
	}

	private void MisteryBoxAnimation_State_End(Spine.AnimationState state, int trackIndex)
	{
		if (misteryBoxAnimation.State.GetCurrent(trackIndex).Animation == CachedDisappearAnimation && this.m_OnOpenEnd != null)
		{
			this.m_OnOpenEnd?.Invoke();
		}
	}
}
