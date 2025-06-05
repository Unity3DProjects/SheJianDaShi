using System.Collections.Generic;
using Prime31;
using com.adjust.sdk;

public class AdjustManager : SingletonMonoBehaviour<AdjustManager>
{
	private static class EventTokens
	{
		public const string REVENUE = "m709sy";

		public const string GAME_LOGIN = "cd47yj";

		public const string FIRST_TIME_DEPOSIT = "ngot2p";

		public const string TWO_MATCHES_WON = "bdjeys";

		public const string FIVE_MATCHES_WON = "40e8r7";

		public const string SEVEN_MATCHES_WON = "6lxs8x";

		public const string TEN_MATCHES_WON = "28flso";

		public const string TWELVE_MATCHES_WON = "jc5l42";

		public const string FIFTEEN_MATCHES_WON = "6z9e64";

		public const string TWENTY_MATCHES_WON = "p6bgf6";

		public const string VIDEO_AD_VIEWED = "sqjuw0";

		public const string SUBSCRIPTION_SUCCESS = "io95xg";

		public const string SUBSCRIPTION_CANCEL = "ghusmn";

		public const string SUBSCRIPTION_REVENUE = "qslynd";

		public const string SUBSCRIPTION_INITIATED = "c2334c";

		public static readonly Dictionary<int, string> MATCHES_WIN = new Dictionary<int, string>
		{
			{ 2, "bdjeys" },
			{ 5, "40e8r7" },
			{ 7, "6lxs8x" },
			{ 10, "28flso" },
			{ 12, "jc5l42" },
			{ 15, "6z9e64" },
			{ 20, "p6bgf6" }
		};
	}

	private const string APP_TOKEN = "cx197tdrwkxs";

	private const string REVENUE_CURRENCY = "USD";

	protected override void Awake()
	{
		Adjust adjust = base.gameObject.AddComponent<Adjust>();
		AdjustEnvironment environment = AdjustEnvironment.Production;
		AdjustConfig adjustConfig = new AdjustConfig("cx197tdrwkxs", environment);
		adjustConfig.setLogLevel(AdjustLogLevel.Error);
		Adjust.start(adjustConfig);
	}

	private void OnEnable()
	{
		TapToStartScreen.OnLoaderRemoved += TapToStartScreen_OnLoaderRemoved;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseSuccessful += MCInnAppsmanager_PurchaseSuccesfull;
		AdvertisingManager.OnVideoAdFinish += AdvertisingManager_OnSupersonicVideoFinish;
	}

	private void OnDisable()
	{
		TapToStartScreen.OnLoaderRemoved -= TapToStartScreen_OnLoaderRemoved;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseSuccessful -= MCInnAppsmanager_PurchaseSuccesfull;
		AdvertisingManager.OnVideoAdFinish -= AdvertisingManager_OnSupersonicVideoFinish;
	}

	public void TrackRevenue(float amount)
	{
		AdjustEvent adjustEvent = new AdjustEvent("m709sy");
		adjustEvent.setRevenue(amount, "USD");
		Adjust.trackEvent(adjustEvent);
	}

	public void TrackMatchWin(int wins)
	{
		string value = null;
		if (EventTokens.MATCHES_WIN.TryGetValue(wins, out value))
		{
			TrackEvent(value);
		}
	}

	public void TrackVideoAdViewed()
	{
		TrackEvent("sqjuw0");
	}

	public void TrackSubscriptionInitiated()
	{
		TrackEvent("c2334c");
	}

	public void TrackSubscriptionSuccess()
	{
		TrackEvent("io95xg");
	}

	public void TrackSubscriptionCancel()
	{
		TrackEvent("ghusmn");
	}

	public void TrackSubscriptionRevenue(float amount)
	{
		TrackEvent("qslynd");
	}

	private void TrackEvent(string eventToken)
	{
		AdjustEvent adjustEvent = new AdjustEvent(eventToken);
		Adjust.trackEvent(adjustEvent);
	}

	private void TapToStartScreen_OnLoaderRemoved()
	{
		TrackEvent("cd47yj");
	}

	private void MCInnAppsmanager_PurchaseSuccesfull(GooglePurchase purchase, Receipt receipt)
	{
		if (SingletonMonoBehaviour<MCInnAppsmanager>.Instance.GetNumberPurchases() == 1)
		{
			TrackEvent("ngot2p");
		}
	}

	private void AdvertisingManager_OnSupersonicVideoFinish(bool success)
	{
		if (success)
		{
			TrackVideoAdViewed();
		}
	}
}
