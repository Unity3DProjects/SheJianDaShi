using UnityEngine;

public class ShopItemGemForAds : ShopItemGems
{
	[SerializeField]
	private FreeGemsAdsInfo adsInfo;

	public FreeGemsAdsInfo AdsInfo
	{
		get
		{
			return adsInfo;
		}
	}
}
