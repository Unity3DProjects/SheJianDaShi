using UnityEngine;

public class ShopItemUpgradeNew : ShopItemUpgrade
{
	[SerializeField]
	private string shopItemFullPriceID;

	public string ProductFullPriceID
	{
		get
		{
			return BundlePrefix.PrefixInAppPurchaseID() + shopItemFullPriceID;
		}
	}

	public string PriceStringSale
	{
		get
		{
			string text = null;
			if (base.IsDataReceived)
			{
				return LLStoreManager.LocalizedPriceProduct(ProductFullPriceID);
			}
			return string.Empty;
		}
	}
}
