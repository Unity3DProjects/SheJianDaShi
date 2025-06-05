using Mit_Goliath_Native.Utils;

public class ShopItemCoins : ShopItemGems
{
	protected override void AddGems(LLTransactionState transactionState)
	{
		if (transactionState == LLTransactionState.Purchased || transactionState == LLTransactionState.RePurchased)
		{
			int amount = base.RewardValue + base.BenefitValue;
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.BOUGHT, GoliathIds.Context.IN_APP_PURCHASE, GoliathIds.CurrencyName.COINS, amount, GoliathIds.Placement.CURRENCY_SHOP, SingletonMonoBehaviour<StatisticsManager>.Instance.Gems);
			SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.AddCoins(base.RewardValue + base.BenefitValue);
		}
	}
}
