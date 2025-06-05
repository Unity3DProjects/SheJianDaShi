using System.Collections.Generic;
using UnityEngine.Analytics;

public class ShopItemAllIncluded : ShopItem
{
	private bool isRestoredItem;

	public override void PurchaseComplete(LLTransactionState transactionState)
	{
		isRestoredItem = transactionState == LLTransactionState.Restored;
		base.PurchaseComplete(transactionState);
	}

	protected override void OpenShopItem(bool isPlayHappySound = false)
	{
		ShopManager instance = SingletonMonoBehaviour<ShopManager>.Instance;
		StatisticsManager instance2 = SingletonMonoBehaviour<StatisticsManager>.Instance;
		Analytics.CustomEvent("Proposal_Purchased", new Dictionary<string, object>
		{
			{ "ProposalName", shopItemID },
			{
				"GameTimeMinutes",
				instance2.TotalGameTime.Minutes
			},
			{ "DaysSinceFirstLaunch", instance2.DaysSinceFirstLaunch }
		});
		List<ShopItemCharacter> availableCharacters = instance.AvailableCharacters;
		for (int i = 0; i < availableCharacters.Count; i++)
		{
			if (!availableCharacters[i].Bought && (!availableCharacters[i].IsLockedByTimer || (availableCharacters[i].IsLockedByTimer && availableCharacters[i].IsTimeEnded)))
			{
				availableCharacters[i].OpenCharacter(!isRestoredItem, false);
			}
		}
		instance.NoAdsBought = true;
		instance.AllCharactersBought = true;
		SingletonMonoBehaviour<ICloudManager>.Instance.SaveProgressToICloud();
		base.OpenShopItem(true);
	}
}
