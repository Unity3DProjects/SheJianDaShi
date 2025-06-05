using System;

[Serializable]
public struct LockedByAdsInfo
{
	public bool isLockedByAds;

	public int totalAdsCount;

	public int adsChainLenght;

	public int timeBetweenFirstAndLastChainItem;

	public int waitingTime;
}
