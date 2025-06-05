using System;
using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class AutoRenewableSubscriptionRewardPopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem claimButton;

	[SerializeField]
	private tk2dTextMesh coinsLabel;

	[SerializeField]
	private tk2dTextMesh gemsLabel;

	private int rewardDay;

	private int coinsReward;

	private int gemsReward;

	protected override void OnEnable()
	{
		if (claimButton != null)
		{
			claimButton.OnClick += ClaimReward;
			rewardDay = Math.Max(base.CachedManagers.CachedSubscriptionManager.SubscriptionRewardCount, 1);
			coinsReward = base.CachedManagers.CachedSubscriptionManager.CoinsForDay * rewardDay;
			gemsReward = base.CachedManagers.CachedSubscriptionManager.GemsForDay * rewardDay;
			coinsLabel.text = "+" + coinsReward;
			gemsLabel.text = "+" + gemsReward;
		}
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		if (claimButton != null)
		{
			claimButton.OnClick -= ClaimReward;
		}
		base.OnDisable();
	}

	private void ClaimReward()
	{
		GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.SUBSCRIPTION, GoliathIds.CurrencyName.COINS, coinsReward, GoliathIds.Placement.MAIN_MENU, SingletonMonoBehaviour<StatisticsManager>.Instance.Coins);
		GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.SUBSCRIPTION, GoliathIds.CurrencyName.GEMS, gemsReward, GoliathIds.Placement.MAIN_MENU, SingletonMonoBehaviour<StatisticsManager>.Instance.Gems);
		base.CachedManagers.CachedStatisticsManager.AddCoins(coinsReward);
		base.CachedManagers.CachedStatisticsManager.AddGems(gemsReward);
		base.CachedManagers.CachedSubscriptionManager.claimReward();
		Hide();
		Analytics.CustomEvent("Subscription_Reward_For_Days", new Dictionary<string, object> { { "Days", rewardDay } });
	}
}
