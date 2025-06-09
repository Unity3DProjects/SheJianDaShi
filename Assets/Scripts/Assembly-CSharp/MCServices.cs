using System.Runtime.InteropServices;

public static class MCServices
{
	// [DllImport("mcservices")]
	// private static extern void mc_displayMoreGamesPage();
	//
	// [DllImport("mcservices")]
	// private static extern void mc_displayCorporateInfo();
	//
	// [DllImport("mcservices")]
	// private static extern void mc_displayCustomerSupportPage();
	//
	// [DllImport("mcservices")]
	// private static extern int mc_rate_canShowRatePopup();
	//
	// [DllImport("mcservices")]
	// private static extern int mc_rate_showRatePopup();

	public static void ShowCustomerSupportWebView()
	{
		// mc_displayCustomerSupportPage();
	}

	public static void ShowMoreGamesWebView()
	{
		// mc_displayMoreGamesPage();
	}

	public static void ShowTermsPopupWebView()
	{
		// mc_displayCorporateInfo();
	}

	public static void ShowRatePopup()
	{
		// mc_rate_showRatePopup();
	}

	public static bool CanShowRatePopup()
	{
		// return mc_rate_canShowRatePopup() == 1;
		return false;
	}
}
