using System;
using Mit_Goliath_Native.Utils;

public class DoubleCoinsPopup : UpgradePopup
{
	protected override bool IsForced
	{
		get
		{
			if (SingletonMonoBehaviour<Managers>.InstanceIfExist != null && SingletonMonoBehaviour<ProposalManager>.InstanceIfExist != null)
			{
				return base.CachedManagers.CachedProposalManager.IsShowingForcedDoubleCoins;
			}
			return false;
		}
		set
		{
			if (SingletonMonoBehaviour<Managers>.InstanceIfExist != null && SingletonMonoBehaviour<ProposalManager>.InstanceIfExist != null)
			{
				base.CachedManagers.CachedProposalManager.IsShowingForcedDoubleCoins = value;
			}
		}
	}

	private void Start()
	{
		SingletonMonoBehaviour<ProposalManager>.Instance.LastDoubleCoins = DateTime.Now;
	}

	protected override void InitializeShopItemVariable()
	{
		shopItem = base.CachedManagers.CachedProposalManager.DoubleCoins;
	}

	protected override void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		if (base.CachedManagers.CachedGUIScreenManager.MenuScreen == null)
		{
			StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.BOUGHT, GoliathIds.Context.IN_APP_PURCHASE, GoliathIds.CurrencyName.COINS, cachedStatisticsManager.CoinsFromLastGame / 2, GoliathIds.Placement.POPUP_PACKAGE);
			cachedStatisticsManager.AddCoins(cachedStatisticsManager.CoinsFromLastGame / 2);
		}
		base.ShopItem_OnItemPurchased(shopItem);
	}
}
