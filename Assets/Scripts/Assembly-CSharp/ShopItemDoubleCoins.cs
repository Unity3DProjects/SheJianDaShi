using System.Collections.Generic;
using UnityEngine.Analytics;

public class ShopItemDoubleCoins : ShopItem
{
	protected override void OpenShopItem(bool isPlayHappySound = false)
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
		instance.DoubleCoins = true;
		base.OpenShopItem(true);
	}
}
