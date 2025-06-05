using System;
using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class RevievePopup : GUIPopup
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

	private LevelVSComputer currentLevel;

	private bool isOnVideoFinishEventSubscribed;

	private bool isGemsAvailable;

	private int currentPrice;

	private int currentPriceGems;

	private int currentPercent;

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
		if (base.CachedManagers.CachedGameManager.CurrentGameType == GameType.Tournament)
		{
			currentPriceGems = base.CachedManagers.CachedProposalManager.PriceMoreLifeTournamentInGems;
			currentPercent = base.CachedManagers.CachedProposalManager.PercentMoreLifeTournament;
		}
		else
		{
			currentPriceGems = base.CachedManagers.CachedProposalManager.PriceMoreLifeInGems;
			currentPercent = base.CachedManagers.CachedProposalManager.PercentMoreLife;
		}
		priceGemsLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.proposal.game.revieve.coins") + currentPriceGems;
		currentLevel = base.CachedManagers.CachedLevelsManager.CurrentLevel as LevelVSComputer;
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.CachedManagers.CachedProposalManager.IsAvalaibleMoreLife = false;
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void Revive(bool isOnCoins, bool isOnVideo = false)
	{
		currentLevel.CharacterFirst.Revive(currentPercent);
		currentLevel.CharacterSecond.ShotsCountWithIndividualHittingAfterRevieve = base.CachedManagers.CachedProposalManager.ShotsCountAfterMoreLifeWithIndividualHitting;
		string empty = string.Empty;
		if (base.CachedManagers.CachedGameManager.CurrentGameType == GameType.Tournament)
		{
			empty = ((!isOnCoins) ? "Revive_Tournament_Accepted_Gems" : "Revive_Tournament_Accepted_Coins");
			if (isOnVideo)
			{
				empty = "Revive_Tournament_Accepted_Video";
			}
		}
		else
		{
			empty = ((!isOnCoins) ? "Revive_Accepted_Gems" : "Revive_Accepted_Coins");
			if (isOnVideo)
			{
				empty = "Revive_Accepted_Video";
			}
		}
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("MatchesCount", cachedStatisticsManager.MathesCount);
		dictionary.Add("DaysSinceFirstLaunch", cachedStatisticsManager.DaysSinceFirstLaunch);
		Dictionary<string, object> eventData = dictionary;
		Analytics.CustomEvent(empty, eventData);
		empty = ((!isOnCoins) ? "SaveMe_Accepted_Gems" : "SaveMe_Accepted_Coins");
		if (isOnVideo)
		{
			empty = "SaveMe_Accepted_Video";
		}
		Analytics.CustomEvent(empty, eventData);
	}

	private void SpendGemsForRevive()
	{
		base.CachedManagers.CachedStatisticsManager.DecreaseGems(currentPriceGems);
		Revive(false);
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
			GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW, GoliathIds.AdReward.REVIVE);
			Revive(false, true);
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
