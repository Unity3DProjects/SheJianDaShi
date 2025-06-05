using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class Crab : MonoBehaviour
{
	private const string CRAB_DEFAULT = "default";

	private const string CRAB_GRAB = "attack";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnHeadTouched;

	[SerializeField]
	private TweenPosition crabTween;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	private Spine.Animation animationCrabDefault;

	private Spine.Animation animationCrabGrab;

	private CharacterBase targetCharacter;

	public Spine.Animation AnimationCrabDefault
	{
		get
		{
			if (animationCrabDefault == null)
			{
				animationCrabDefault = skeletonAnimation.Skeleton.Data.FindAnimation("default");
			}
			return animationCrabDefault;
		}
	}

	public Spine.Animation AnimationCrabGrab
	{
		get
		{
			if (animationCrabGrab == null)
			{
				animationCrabGrab = skeletonAnimation.Skeleton.Data.FindAnimation("attack");
			}
			return animationCrabGrab;
		}
	}

	public event Action OnHeadTouched
	{
		add
		{
			Action action = this.m_OnHeadTouched;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHeadTouched, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnHeadTouched;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHeadTouched, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		skeletonAnimation.state.SetAnimation(0, AnimationCrabGrab, false);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (this.m_OnHeadTouched != null)
			{
				this.m_OnHeadTouched();
			}
		}, AnimationCrabGrab.Duration);
	}
}
