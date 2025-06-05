using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ShopItemNoAds : ShopItem
{
	[SerializeField]
	private int coinsReward;

	public int CoinsReward
	{
		get
		{
			return coinsReward;
		}
	}

	public override void PurchaseComplete(LLTransactionState transactionState)
	{
		if (!base.Bought)
		{
			StatisticsManager instance = SingletonMonoBehaviour<StatisticsManager>.Instance;
			Analytics.CustomEvent("Proposal_Purchased", new Dictionary<string, object>
			{
				{ "ProposalName", shopItemID },
				{
					"GameTimeMinutes",
					instance.TotalGameTime.Minutes
				},
				{ "DaysSinceFirstLaunch", instance.DaysSinceFirstLaunch }
			});
			if (transactionState == LLTransactionState.Purchased || transactionState == LLTransactionState.RePurchased)
			{
				instance.AddCoins(CoinsReward);
			}
			SingletonMonoBehaviour<ShopManager>.Instance.NoAdsBought = true;
			SingletonMonoBehaviour<ICloudManager>.Instance.SaveProgressToICloud();
		}
		base.PurchaseComplete(transactionState);
	}
}
