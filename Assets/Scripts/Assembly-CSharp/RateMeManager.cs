using UnityEngine;

public class RateMeManager : SingletonMonoBehaviour<RateMeManager>
{
	private const string GooglePlayStoreLink = "http://play.google.com/store/apps/details?id=";

	private bool hasRatePopupPending = true;

	public static string BowmastersGooglePlayStoreLink
	{
		get
		{
			return "http://play.google.com/store/apps/details?id=" + Application.identifier;
		}
	}

	public bool IsRateMeAvalaible
	{
		get
		{
			StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
			bool flag = cachedStatisticsManager.WinsCount == 3;
			flag |= cachedStatisticsManager.WinsCount > 3 && (cachedStatisticsManager.WinsCount - 3) % 5 == 0;
			flag &= cachedStatisticsManager.TotalSeesionCount > 0;
			bool flag2 = MCServices.CanShowRatePopup();
			bool flag3 = flag2 && flag;
			if ((flag3 && hasRatePopupPending) || (flag2 && hasRatePopupPending))
			{
				hasRatePopupPending = false;
				CustomPlayerPrefs.SetInt("rate_pending", 0);
				return true;
			}
			return flag3;
		}
	}

	public string ITunesLinkRateMe
	{
		get
		{
			string empty = string.Empty;
			return "https://itunes.apple.com/app/id1118431695";
		}
	}

	private void Start()
	{
		InvokeRepeating("CheckIfRateIsAvailable", 5f, 5f);
		hasRatePopupPending = CustomPlayerPrefs.GetInt("rate_pending", 0) == 1;
	}

	private void CheckIfRateIsAvailable()
	{
		if (!hasRatePopupPending)
		{
			hasRatePopupPending = IsRateMeAvalaible;
			CustomPlayerPrefs.SetInt("rate_pending", 1);
		}
	}

	public void RateAppAction()
	{
		Application.OpenURL("http://play.google.com/store/apps/details?id=" + Application.identifier);
	}

	public void RatePopupShown()
	{
		StatisticsManager cachedStatisticsManager = SingletonMonoBehaviour<GameManager>.Instance.CachedManagers.CachedStatisticsManager;
		CustomPlayerPrefs.SetInt("rate_game" + cachedStatisticsManager.WinsCount, 1);
	}
}
