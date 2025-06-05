using System;
using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class AmmoPopup : GUIPopup
{
	private const string COINS_PREFIX_KEY = "ui.proposal.game.ammo.coins";

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

	private LevelAppleShoot currentLevel;

	private bool isOnVideoFinishEventSubscribed;

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
		GUIPopupManager.OnOverlayedPopupHide -= GUIPopupManager_OnOverlayedPopupHide;
		IsOnVideoFinishEventSubscribed = false;
		MiniBankPopup.OnBankItemPurchsed -= MiniBankPopup_OnBankItemPurchsed;
		ProgressBar.OnProgressBarEnded -= ProgressBar_OnProgressBarEnded;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		isGemsAvailable = base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		buttonGems.gameObject.SetActive(isGemsAvailable);
		buttonFree.gameObject.SetActive(!isGemsAvailable);
		priceGemsLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.proposal.game.ammo.coins") + base.CachedManagers.CachedProposalManager.PriceMoreAmmoInGems;
		currentLevel = base.CachedManagers.CachedLevelsManager.CurrentLevel as LevelAppleShoot;
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.CachedManagers.CachedProposalManager.IsAvalaibleMoreAmmo = false;
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void RemoveMisses(bool isOnCoins, bool isOnVideo = false)
	{
		int num = ((!isOnCoins) ? base.CachedManagers.CachedProposalManager.MissesRemovedOnVideo : base.CachedManagers.CachedProposalManager.MissesRemovedOnCoins);
		currentLevel.AppleMissesCount -= num;
		string customEventName = ((!isOnCoins) ? "Misses_Accepted_Gems" : "Misses_Accepted_Coins");
		if (isOnVideo)
		{
			customEventName = "Misses_Accepted_Video";
		}
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("MatchesCount", cachedStatisticsManager.MathesCount);
		dictionary.Add("DaysSinceFirstLaunch", cachedStatisticsManager.DaysSinceFirstLaunch);
		Dictionary<string, object> eventData = dictionary;
		Analytics.CustomEvent(customEventName, eventData);
		customEventName = ((!isOnCoins) ? "SaveMe_Accepted_Gems" : "SaveMe_Accepted_Coins");
		if (isOnVideo)
		{
			customEventName = "SaveMe_Accepted_Video";
		}
		Analytics.CustomEvent(customEventName, eventData);
	}

	private void SpendGemsForRevive()
	{
		base.CachedManagers.CachedStatisticsManager.DecreaseGems(base.CachedManagers.CachedProposalManager.PriceMoreAmmoInGems);
		RemoveMisses(false);
		Hide();
	}

	private void ButtonTouch_OnClick()
	{
		if (isGemsAvailable)
		{
			buttonGems.SimulateClick();
			if (base.CachedManagers.CachedStatisticsManager.Gems >= base.CachedManagers.CachedProposalManager.PriceMoreAmmoInGems)
			{
				SpendGemsForRevive();
				return;
			}
			(base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true) as MiniBankPopup).InitPopup(base.CachedManagers.CachedProposalManager.PriceMoreAmmoInGems);
			isRealTimerEnable = false;
		}
		else
		{
			buttonFree.SimulateClick();
			base.CachedManagers.CachedAdvertisingManager.ShowVideo();
			IsOnVideoFinishEventSubscribed = true;
			isRealTimerEnable = false;
		}
	}

	private void MCAdsManager_OnVideoTryLoad(bool result)
	{
		if (!result)
		{
			Hide();
		}
	}

	private void AdvertisingManager_OnVideoAdFinish(bool isVideoAvailable)
	{
		IsOnVideoFinishEventSubscribed = false;
		if (isVideoAvailable)
		{
			GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW, GoliathIds.AdReward.REMOVE_MISSES);
			RemoveMisses(false, true);
		}
		else
		{
			GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW);
		}
		Hide();
	}

	private void GUIPopupManager_OnOverlayedPopupHide()
	{
		isRealTimerEnable = true;
		if (!IsOnVideoFinishEventSubscribed)
		{
			Hide();
		}
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
			Hide();
		}
	}
}
