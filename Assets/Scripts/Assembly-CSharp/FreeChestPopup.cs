using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class FreeChestPopup : GUIPopup
{
	private const string ANIMATION_OPEN = "open";

	private const string ANIMATION_APPEAR = "appear_one_lock";

	private const string ANIMATION_UNLOCK = "unlock_one_lock";

	private const string ANIMATION_SPAWN_END = "Appear_End";

	private const string ANIMATION_OPEN_END = "Open_End";

	private const string ANIMATION_OPEN_START = "Open_Start";

	private const string ANIMATION_UNLOCK_END = "Center_Unlock_End";

	private const string LABEL_HEADER_FREE_ROULETTE = "ui.popup.free.roulette.header";

	private const string LABEL_BUTTON_FREE_ROULETTE = "ui.popup.free.roulette.button";

	private const string LABEL_HEADER_FREE_PROPOSAL = "ui.popup.free.chest.accelerator.header";

	private const string LABEL_BUTTON_FREE_PROPOSAL = "ui.popup.free.chest.accelerator.button";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnVideoWatched;

	[SerializeField]
	private tk2dUIItem skipButton;

	[SerializeField]
	private SkeletonAnimation chestAnimation;

	[SerializeField]
	private Vector3 chestAnimationPosition;

	[SerializeField]
	private Transform effectIdleTransform;

	[SerializeField]
	private Transform effectOpenTransform;

	[SerializeField]
	private float hideDelay = 0.5f;

	[SerializeField]
	private tk2dTextMesh headerLabel;

	[SerializeField]
	private tk2dTextMesh buttonLabel;

	private GameObject characterEffect;

	private Spine.Animation cachedAnimationSpawn;

	private Spine.Animation cachedAnimationOpen;

	private Spine.Animation cachedAnimationUnlock;

	private EventData cachedEventDataSpawnEnd;

	private EventData cachedEventDataOpenStart;

	private EventData cachedEventDataOpenEnd;

	private EventData cachedEventDataUnlockEnd;

	private bool isProposalPopup;

	private Spine.Animation CachedAnimationSpawn
	{
		get
		{
			if (cachedAnimationSpawn == null)
			{
				cachedAnimationSpawn = chestAnimation.Skeleton.Data.FindAnimation("appear_one_lock");
			}
			return cachedAnimationSpawn;
		}
	}

	private Spine.Animation CachedAnimationOpen
	{
		get
		{
			if (cachedAnimationOpen == null)
			{
				cachedAnimationOpen = chestAnimation.Skeleton.Data.FindAnimation("open");
			}
			return cachedAnimationOpen;
		}
	}

	private Spine.Animation CachedAnimationUnlock
	{
		get
		{
			if (cachedAnimationUnlock == null)
			{
				cachedAnimationUnlock = chestAnimation.Skeleton.Data.FindAnimation("unlock_one_lock");
			}
			return cachedAnimationUnlock;
		}
	}

	private EventData CachedEventDataOpenEnd
	{
		get
		{
			if (cachedEventDataOpenEnd == null)
			{
				cachedEventDataOpenEnd = chestAnimation.Skeleton.Data.FindEvent("Open_End");
			}
			return cachedEventDataOpenEnd;
		}
	}

	private EventData CachedEventDataSpawnEnd
	{
		get
		{
			if (cachedEventDataSpawnEnd == null)
			{
				cachedEventDataSpawnEnd = chestAnimation.Skeleton.Data.FindEvent("Appear_End");
			}
			return cachedEventDataSpawnEnd;
		}
	}

	private EventData CachedEventDataOpenStart
	{
		get
		{
			if (cachedEventDataOpenStart == null)
			{
				cachedEventDataOpenStart = chestAnimation.Skeleton.Data.FindEvent("Open_Start");
			}
			return cachedEventDataOpenStart;
		}
	}

	private EventData CachedEventDataUnlockEnd
	{
		get
		{
			if (cachedEventDataUnlockEnd == null)
			{
				cachedEventDataUnlockEnd = chestAnimation.Skeleton.Data.FindEvent("Center_Unlock_End");
			}
			return cachedEventDataUnlockEnd;
		}
	}

	public static event Action<bool> OnVideoWatched
	{
		add
		{
			Action<bool> action = FreeChestPopup.m_OnVideoWatched;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref FreeChestPopup.m_OnVideoWatched, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = FreeChestPopup.m_OnVideoWatched;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref FreeChestPopup.m_OnVideoWatched, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void OnEnable()
	{
		skipButton.OnClick += SkipButton_OnClick;
		chestAnimation.State.Event += ChestAnimation_State_Event;
		chestAnimation.transform.localPosition = chestAnimationPosition * ((!tk2dSystem.IsRetina) ? 1 : 2);
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		skipButton.OnClick -= SkipButton_OnClick;
		chestAnimation.State.Event -= ChestAnimation_State_Event;
		base.OnDisable();
	}

	public void Init(bool isRoulettePopup)
	{
		isProposalPopup = !isRoulettePopup;
		string key = ((!isProposalPopup) ? "ui.popup.free.roulette.header" : "ui.popup.free.chest.accelerator.header");
		headerLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey(key);
		string key2 = ((!isProposalPopup) ? "ui.popup.free.roulette.button" : "ui.popup.free.chest.accelerator.button");
		buttonLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey(key2);
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			chestAnimation.State.SetAnimation(0, CachedAnimationSpawn, false);
			characterEffect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.GUIPopupChestSpawn));
			characterEffect.transform.parent = effectOpenTransform;
			characterEffect.transform.localPosition = Vector3.zero;
		}, 0f);
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = delegate(GUIControl control)
		{
			if (characterEffect != null)
			{
				UnityEngine.Object.Destroy(characterEffect);
				characterEffect = null;
			}
			if (onFinished != null)
			{
				onFinished(control);
			}
		};
		base.Hide(onFinished2, immediately, delay, callInvokes);
	}

	protected virtual void GetReward()
	{
		if (isProposalPopup)
		{
			base.CachedManagers.CachedProposalManager.LastFreeChestAccelerator = DateTime.Now;
			base.CachedManagers.CachedProposalManager.LastFreeChestTimerStart = base.CachedManagers.CachedProposalManager.LastFreeChestTimerStart.AddSeconds(0f - base.CachedManagers.CachedProposalManager.FreeChestTimer.TimerInSeconds);
			base.CachedManagers.CachedProposalManager.FreeChestTimer.TimerInSeconds = 0f;
		}
		if (FreeChestPopup.OnVideoWatched != null)
		{
			FreeChestPopup.OnVideoWatched(isProposalPopup);
		}
	}

	private void SkipButton_OnClick()
	{
		base.CachedManagers.CachedGameManager.IsGamePaused = true;
		skipButton.GetComponent<BoxCollider>().enabled = false;
		base.CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoShown)
		{
			base.CachedManagers.CachedGameManager.IsGamePaused = false;
			if (isVideoShown)
			{
				chestAnimation.State.SetAnimation(0, CachedAnimationUnlock, false);
				chestAnimation.State.AddAnimation(0, CachedAnimationOpen, false, 0f);
			}
			else
			{
				skipButton.GetComponent<BoxCollider>().enabled = true;
			}
		});
	}

	private void ChestAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataOpenEnd)
		{
			GetReward();
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
			{
				Hide();
			}, hideDelay);
		}
		else if (e.Data == CachedEventDataOpenStart)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				MenuScreen menuScreen = base.CachedManagers.CachedGUIScreenManager.MenuScreen;
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIPopupChestOpen));
				gameObject.transform.parent = ((!(menuScreen == null)) ? menuScreen.CachedTransform : effectOpenTransform);
				gameObject.transform.localPosition = Vector3.zero;
				if (menuScreen != null)
				{
					gameObject.transform.position = effectOpenTransform.position;
				}
			}, 0f);
		}
		else if (e.Data == CachedEventDataUnlockEnd)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIPopupChestOpenLock));
				gameObject.transform.parent = effectOpenTransform;
				gameObject.transform.localPosition = Vector3.zero;
			}, 0f);
			AudioManager.PlayAudioWithKey("chest_open");
		}
		else if (e.Data == CachedEventDataSpawnEnd)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIPopupInviteSecond));
				gameObject.transform.parent = effectIdleTransform;
				gameObject.transform.localPosition = Vector3.zero;
			}, 0f);
		}
	}
}
