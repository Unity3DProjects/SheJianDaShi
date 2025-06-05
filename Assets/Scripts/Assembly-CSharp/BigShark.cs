using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class BigShark : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnBiten;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnContacted;

	[SerializeField]
	private SkeletonAnimation sharkAnimation;

	[SerializeField]
	private SkeletonAnimation jawAnimation;

	[SerializeField]
	private Transform jawBoneTransform;

	private Spine.Animation cahedSharkAnimation;

	private Spine.Animation cahedSharkJawAnimation;

	private EventData cachedEventDataBite;

	private EventData cachedEventDataContact;

	private Spine.Animation CahedSharkAnimation
	{
		get
		{
			if (cahedSharkAnimation == null)
			{
				cahedSharkAnimation = sharkAnimation.Skeleton.Data.FindAnimation("fatality_small_shark");
			}
			return cahedSharkAnimation;
		}
	}

	private Spine.Animation CahedSharkJawAnimation
	{
		get
		{
			if (cahedSharkJawAnimation == null)
			{
				cahedSharkJawAnimation = jawAnimation.Skeleton.Data.FindAnimation("fatality_small_shark");
			}
			return cahedSharkJawAnimation;
		}
	}

	private EventData CachedEventDataBite
	{
		get
		{
			if (cachedEventDataBite == null)
			{
				cachedEventDataBite = sharkAnimation.Skeleton.Data.FindEvent("Omnomnom");
			}
			return cachedEventDataBite;
		}
	}

	private EventData CachedEventDataContact
	{
		get
		{
			if (cachedEventDataContact == null)
			{
				cachedEventDataContact = sharkAnimation.Skeleton.Data.FindEvent("Contact");
			}
			return cachedEventDataContact;
		}
	}

	public Transform JawBoneTransform
	{
		get
		{
			return jawBoneTransform;
		}
	}

	public event Action OnBiten
	{
		add
		{
			Action action = this.m_OnBiten;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnBiten, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnBiten;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnBiten, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnContacted
	{
		add
		{
			Action action = this.m_OnContacted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnContacted, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnContacted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnContacted, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		sharkAnimation.State.SetAnimation(0, CahedSharkAnimation, false);
		jawAnimation.State.SetAnimation(0, CahedSharkJawAnimation, false);
		AudioManager.PlayAudioWithKey("candy_trident_shark_jump");
	}

	private void OnEnable()
	{
		sharkAnimation.State.Event += SharkAnimation_State_Event;
	}

	private void OnDisable()
	{
		sharkAnimation.State.Event -= SharkAnimation_State_Event;
	}

	private void SharkAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataBite && this.OnBiten != null)
		{
			this.OnBiten();
		}
		if (e.Data == CachedEventDataContact && this.OnContacted != null)
		{
			this.OnContacted();
		}
	}
}
