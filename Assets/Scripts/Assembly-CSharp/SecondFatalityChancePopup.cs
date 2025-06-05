using System;
using UnityEngine;

public class SecondFatalityChancePopup : GUIPopup
{
	private const string COINS_PREFIX_KEY = "ui.proposal.game.revieve.coins";

	[SerializeField]
	private tk2dTextMesh priceGemsLabel;

	[SerializeField]
	private tk2dUIItem buttonFree;

	[SerializeField]
	private tk2dUIItem buttonGems;

	[SerializeField]
	private tk2dUIItem buttonTouch;

	[SerializeField]
	private float timerFullTime;

	[SerializeField]
	private ProgressBar[] progressBars;

	private bool isOnVideoFinishEventSubscribed;

	private int currentPriceGems;

	private bool isGemsAvailable;

	private bool IsOnVideoFinishEventSubscribed
	{
		get
		{
			return isOnVideoFinishEventSubscribed;
		}
		set
		{
			if (isOnVideoFinishEventSubscribed != value)
			{
				isOnVideoFinishEventSubscribed = value;
				if (isOnVideoFinishEventSubscribed)
				{
					MCAdsManager.OnVideoTryLoad += MCAdsManager_OnVideoTryLoad;
					AdvertisingManager.OnVideoAdFinish += AdvertisingManager_OnVideoAdFinish;
				}
				else
				{
					MCAdsManager.OnVideoTryLoad -= MCAdsManager_OnVideoTryLoad;
					AdvertisingManager.OnVideoAdFinish -= AdvertisingManager_OnVideoAdFinish;
				}
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		buttonTouch.OnClick += ButtonTouch_OnClick;
		GUIPopupManager.OnOverlayedPopupHide += GUIPopupManager_OnOverlayedPopupHide;
		MiniBankPopup.OnBankItemPurchsed += MiniBankPopup_OnBankItemPurchsed;
		ProgressBar.OnProgressBarEnded += ProgressBar_OnProgressBarEnded;
		for (int i = 0; i < progressBars.Length; i++)
		{
			progressBars[i].IsIgnoreTimeScale = true;
			progressBars[i].SetFillAmount(Time.unscaledDeltaTime / timerFullTime);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		buttonTouch.OnClick -= ButtonTouch_OnClick;
		IsOnVideoFinishEventSubscribed = false;
		GUIPopupManager.OnOverlayedPopupHide -= GUIPopupManager_OnOverlayedPopupHide;
		MiniBankPopup.OnBankItemPurchsed -= MiniBankPopup_OnBankItemPurchsed;
		ProgressBar.OnProgressBarEnded -= ProgressBar_OnProgressBarEnded;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		isGemsAvailable = base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		buttonGems.gameObject.SetActive(isGemsAvailable);
		buttonFree.gameObject.SetActive(!isGemsAvailable);
		currentPriceGems = base.CachedManagers.CachedProposalManager.PriceForSecondFatalityChanseInGems;
		priceGemsLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.proposal.game.revieve.coins") + currentPriceGems;
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.CachedManagers.CachedProposalManager.IsAvalaibleMoreLife = false;
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void GetSecondFatalityChance()
	{
		base.CachedManagers.CachedProposalManager.IsAvalaibleSecondFatalityChance = false;
		base.CachedManagers.CachedLevelsManager.CurrentLevel.CharacterSecond.RestoreArcher();
	}

	private void SpendGemsForRevive()
	{
		base.CachedManagers.CachedStatisticsManager.DecreaseGems(currentPriceGems);
		GetSecondFatalityChance();
		Hide();
	}

	private void ButtonTouch_OnClick()
	{
		if (isGemsAvailable)
		{
			buttonGems.SimulateClick();
			if (base.CachedManagers.CachedStatisticsManager.Gems >= currentPriceGems)
			{
				SpendGemsForRevive();
				return;
			}
			(base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true) as MiniBankPopup).InitPopup(currentPriceGems);
			isRealTimerEnable = false;
		}
		else
		{
			buttonFree.SimulateClick();
			base.CachedManagers.CachedAdvertisingManager.ShowVideo(VideoCallback);
			IsOnVideoFinishEventSubscribed = true;
			isRealTimerEnable = false;
		}
	}

	private void MCAdsManager_OnVideoTryLoad(bool result)
	{
		if (!result)
		{
			Level currentLevel = base.CachedManagers.CachedLevelsManager.CurrentLevel;
			if (currentLevel != null)
			{
				currentLevel.ActiveCharacter.KillCharacter();
			}
			Hide();
		}
	}

	private void AdvertisingManager_OnVideoAdFinish(bool isVideoAvailable)
	{
		IsOnVideoFinishEventSubscribed = false;
		if (isVideoAvailable)
		{
			GetSecondFatalityChance();
		}
		else
		{
			IsOnVideoFinishEventSubscribed = true;
			GUIPopupManager_OnOverlayedPopupHide();
		}
		Hide();
	}

	private void GUIPopupManager_OnOverlayedPopupHide()
	{
		if (!IsOnVideoFinishEventSubscribed)
		{
			base.CachedManagers.CachedLevelsManager.CurrentLevel.ActiveCharacter.KillCharacter();
			Hide();
		}
		isRealTimerEnable = true;
	}

	private void MiniBankPopup_OnBankItemPurchsed(bool areGemsPurchsed)
	{
		if (areGemsPurchsed)
		{
			SpendGemsForRevive();
			Hide();
		}
	}

	private void ProgressBar_OnProgressBarEnded()
	{
		if (!IsOnVideoFinishEventSubscribed)
		{
			base.CachedManagers.CachedLevelsManager.CurrentLevel.ActiveCharacter.KillCharacter();
			Hide();
		}
	}

	protected override void CloseButton_OnClick()
	{
		base.CachedManagers.CachedLevelsManager.CurrentLevel.ActiveCharacter.KillCharacter();
		Hide();
	}

	private void VideoCallback(bool result)
	{
		if (!result)
		{
			IsOnVideoFinishEventSubscribed = false;
			ProgressBar_OnProgressBarEnded();
		}
	}
}
