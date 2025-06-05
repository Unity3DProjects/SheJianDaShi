using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class ShopItemGems : ShopItem
{
	[SerializeField]
	private int rewardValue;

	[SerializeField]
	private int benefitValue;

	[SerializeField]
	private bool isBestValue;

	[SerializeField]
	private bool isMostPopular;

	[SerializeField]
	private GUIBankItem guiItem;

	public int RewardValue
	{
		get
		{
			return rewardValue;
		}
	}

	public int BenefitValue
	{
		get
		{
			return benefitValue;
		}
	}

	public bool IsBestValue
	{
		get
		{
			return isBestValue;
		}
	}

	public bool IsMostPopular
	{
		get
		{
			return isMostPopular;
		}
	}

	public GUIBankItem GUIItem
	{
		get
		{
			return guiItem;
		}
	}

	public override bool IsConsumable()
	{
		return true;
	}

	public override void PurchaseComplete(LLTransactionState transactionState)
	{
		AddGems(transactionState);
		base.PurchaseComplete(transactionState);
		if (transactionState == LLTransactionState.RePurchased)
		{
			Analytics.CustomEvent(string.Format("{0}_{1}", "Shop_Item_Purchased", shopItemID));
		}
	}

	protected virtual void AddGems(LLTransactionState transactionState)
	{
		if (transactionState == LLTransactionState.Purchased || transactionState == LLTransactionState.RePurchased)
		{
			int amount = RewardValue + benefitValue;
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.BOUGHT, GoliathIds.Context.IN_APP_PURCHASE, GoliathIds.CurrencyName.GEMS, amount, GoliathIds.Placement.CURRENCY_SHOP, SingletonMonoBehaviour<StatisticsManager>.Instance.Gems);
			SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.AddGems(RewardValue + benefitValue);
		}
	}

	protected override void WritePurchaseToPrefs()
	{
	}
}
