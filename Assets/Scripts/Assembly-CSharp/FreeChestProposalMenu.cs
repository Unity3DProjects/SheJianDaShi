using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Spine;
using UnityEngine;

public class FreeChestProposalMenu : GUIProposalMenu
{
	private const string BUTTON_ACTIVE_SPRITE = "btn_blue_long_floating";

	private const string BUTTON_INACTIVE_SPRITE = "btn_purp_long_floating";

	public const int DAY_BETWEEN_ACCELERATOR = 1;

	[SerializeField]
	private SkeletonAnimation idleAnimation;

	[SerializeField]
	private tk2dTextMesh timerLabel;

	[SerializeField]
	private GUILayouter timerActiveLayouter;

	[SerializeField]
	private GUILayouter timerInactiveLayouter;

	[SerializeField]
	private tk2dBaseSprite backSprite;

	[SerializeField]
	private float proposalEffectYPosition = 110f;

	[SerializeField]
	private SkeletonAnimation alertAnimation;

	private Spine.Animation jumpAnimation;

	private bool isJumpEnabled;

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

	public Spine.Animation JumpAnimation
	{
		get
		{
			if (jumpAnimation == null)
			{
				jumpAnimation = idleAnimation.State.Data.SkeletonData.FindAnimation("jump");
			}
			return jumpAnimation;
		}
	}

	private bool IsAcceleratorButtonAvailable
	{
		get
		{
			return (DateTime.Now - CachedManagers.CachedProposalManager.LastFreeChestAccelerator).TotalDays >= 1.0 && !CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (tk2dSystem.IsRetina)
		{
			idleAnimation.transform.localPosition *= 2f;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GUITimer freeChestTimer = SingletonMonoBehaviour<ProposalManager>.Instance.FreeChestTimer;
		freeChestTimer.OnTimerChanged += ProposalManager_OnProposalTimerChanged;
		ProposalManager_OnProposalTimerChanged((int)freeChestTimer.TimerInSeconds);
		ChangeTimerActiveState((int)freeChestTimer.TimerInSeconds != 0);
		UpdateMainButton((int)freeChestTimer.TimerInSeconds != 0);
		idleAnimation.Reset();
		StartCoroutine(ChestJump());
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		ProposalManager instanceIfExist = SingletonMonoBehaviour<ProposalManager>.InstanceIfExist;
		if (instanceIfExist != null)
		{
			instanceIfExist.FreeChestTimer.OnTimerChanged -= ProposalManager_OnProposalTimerChanged;
		}
		StopAllCoroutines();
	}

	public override void Activate()
	{
		base.Activate();
		if (idleAnimation != null)
		{
			if (currentEffect != null)
			{
				UnityEngine.Object.Destroy(currentEffect);
				currentEffect = null;
			}
			if (effectType != EffectType.None)
			{
				currentEffect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(effectType));
				currentEffect.transform.parent = idleAnimation.transform;
				currentEffect.transform.localPosition = Vector3.up * proposalEffectYPosition;
				currentEffect.transform.localPosition *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
			}
		}
	}

	public void Deactivate(bool isImmediately = true)
	{
		if (isImmediately)
		{
			base.Deactivate();
			return;
		}
		mainButton.enabled = false;
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				mainButton.enabled = true;
				_003CDeactivate_003E__BaseCallProxy0();
			}
		}, 2f);
	}

	public void ChangeTimerActiveState(bool isTimerActive)
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				timerActiveLayouter.gameObject.SetActive(isTimerActive);
				timerInactiveLayouter.gameObject.SetActive(!isTimerActive);
			}
		}, 0f);
	}

	public void UpdateMainButton(bool isTimerActive)
	{
		mainButton.enabled = !isTimerActive || IsAcceleratorButtonAvailable;
		isJumpEnabled = !isTimerActive;
		alertAnimation.gameObject.SetActive(isTimerActive && IsAcceleratorButtonAvailable);
		string sprite = ((!isTimerActive || IsAcceleratorButtonAvailable) ? "btn_blue_long_floating" : "btn_purp_long_floating");
		backSprite.SetSprite(sprite);
	}

	public void GetAward()
	{
		MainButton_OnClick();
	}

	private void ProposalManager_OnProposalTimerChanged(int time)
	{
		timerLabel.text = string.Format("{0}:{1}:{2}", time / 3600, (time / 60 % 60).ToString("00"), (time % 60).ToString("00"));
		if (time == 0)
		{
			ChangeTimerActiveState(false);
		}
		UpdateMainButton(time > 0);
	}

	protected override void MainButton_OnClick()
	{
		if (IsAcceleratorButtonAvailable && CachedManagers.CachedProposalManager.FreeChestTimer.TimerInSeconds > 0f)
		{
			(CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.FreeChest) as FreeChestPopup).Init(false);
		}
		else
		{
			base.MainButton_OnClick();
		}
	}

	private IEnumerator ChestJump()
	{
		while (base.gameObject.activeInHierarchy)
		{
			if (isJumpEnabled)
			{
				idleAnimation.State.SetAnimation(0, JumpAnimation, false);
			}
			yield return new WaitForSeconds(SingletonMonoBehaviour<ProposalManager>.Instance.FreeChestJumpDelay);
		}
		yield return null;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CDeactivate_003E__BaseCallProxy0()
	{
		base.Deactivate();
	}
}
