using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class ShopItemUpgrade : ShopItem
{
	[SerializeField]
	private ShopItemCharacter[] characters;

	[SerializeField]
	private int coinsReward;

	[SerializeField]
	private bool isNoAdsActive;

	public ShopItemCharacter[] Characters
	{
		get
		{
			return characters;
		}
	}

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
			bool flag = transactionState == LLTransactionState.Purchased || transactionState == LLTransactionState.RePurchased;
			Analytics.CustomEvent("Proposal_Purchased", new Dictionary<string, object>
			{
				{ "ProposalName", shopItemID },
				{
					"GameTimeMinutes",
					instance.TotalGameTime.Minutes
				},
				{ "DaysSinceFirstLaunch", instance.DaysSinceFirstLaunch }
			});
			for (int i = 0; i < characters.Length; i++)
			{
				ShopItemCharacter shopItemCharacter = characters[i];
				if (shopItemCharacter != null && !shopItemCharacter.Availible)
				{
					shopItemCharacter.OpenCharacter(flag, false);
				}
			}
			if (isNoAdsActive)
			{
				SingletonMonoBehaviour<ShopManager>.Instance.NoAdsBought = true;
			}
			if (flag)
			{
				GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.BOUGHT, GoliathIds.Context.IN_APP_PURCHASE, GoliathIds.CurrencyName.COINS, CoinsReward, GoliathIds.Placement.END_GAME);
				instance.AddCoins(CoinsReward);
			}
			SingletonMonoBehaviour<ICloudManager>.Instance.SaveProgressToICloud();
		}
		base.PurchaseComplete(transactionState);
	}
}
