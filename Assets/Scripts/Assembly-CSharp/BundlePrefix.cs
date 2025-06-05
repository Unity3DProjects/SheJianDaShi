using UnityEngine;

public static class BundlePrefix
{
	private const string ANDROID_IAP_PREFIX = "com.playgendary.bowmasters.";

	private const string DOT_SUFFIX = ".";

	private const string IAP_SUFFIX = "iap.";

	private const string LEADERBOARD_SUFFIX = "gc.lb.";

	private const string ACHIEVEMENT_SUFFIX = "gc.ach.";

	private static string cachedBundleCompanyID;

	private static string cachedBundleAppID;

	private static string cachedBundleID;

	private static string cachedPrefixBundleID;

	private static string cachedPrefixLeaderboardID;

	private static string cachedPrefixAchievementID;

	private static string cachedPrefixInAppPurchaseID;

	private static string CachedBundleCompanyID
	{
		get
		{
			if (cachedBundleCompanyID == null)
			{
				cachedBundleCompanyID = string.Empty;
				int num = CachedBundleID.IndexOf(".");
				if (num > 0 && num + 1 < CachedBundleID.Length)
				{
					num = CachedBundleID.IndexOf(".", num + 1);
					if (num > 0)
					{
						cachedBundleCompanyID = CachedBundleID.Substring(0, num);
					}
				}
			}
			return cachedBundleCompanyID;
		}
	}

	private static string CachedBundleAppID
	{
		get
		{
			if (cachedBundleAppID == null)
			{
				cachedBundleAppID = string.Empty;
				int num = CachedBundleID.IndexOf(".");
				if (num > 0 && num + 1 < CachedBundleID.Length)
				{
					num = CachedBundleID.IndexOf(".", num + 1);
					if (num > 0)
					{
						cachedBundleAppID = CachedBundleID.Substring(num + 1);
					}
				}
			}
			return cachedBundleAppID;
		}
	}

	private static string CachedBundleID
	{
		get
		{
			if (cachedBundleID == null)
			{
				cachedBundleID = ((!string.IsNullOrEmpty(Application.identifier)) ? Application.identifier : string.Empty);
			}
			return cachedBundleID;
		}
	}

	private static string CachedPrefixBundleID
	{
		get
		{
			if (cachedPrefixBundleID == null)
			{
				cachedPrefixBundleID = CachedBundleID + ".";
			}
			return cachedPrefixBundleID;
		}
	}

	private static string CachedPrefixLeaderboardID
	{
		get
		{
			if (cachedPrefixLeaderboardID == null)
			{
				cachedPrefixLeaderboardID = CachedPrefixBundleID + "gc.lb.";
			}
			return cachedPrefixLeaderboardID;
		}
	}

	private static string CachedPrefixAchievementID
	{
		get
		{
			if (cachedPrefixAchievementID == null)
			{
				cachedPrefixAchievementID = CachedPrefixBundleID + "gc.ach.";
			}
			return cachedPrefixAchievementID;
		}
	}

	private static string CachedPrefixInAppPurchaseID
	{
		get
		{
			if (cachedPrefixInAppPurchaseID == null)
			{
				cachedPrefixInAppPurchaseID = "com.playgendary.bowmasters.iap.";
			}
			return cachedPrefixInAppPurchaseID;
		}
	}

	public static string BundleCompanyID()
	{
		return CachedBundleCompanyID;
	}

	public static string BundleAppID()
	{
		return CachedBundleAppID;
	}

	public static string BundleID()
	{
		return CachedBundleID;
	}

	public static string PrefixBundleID()
	{
		return CachedPrefixBundleID;
	}

	public static string PrefixLeaderboardID()
	{
		return CachedPrefixLeaderboardID;
	}

	public static string PrefixAchievementID()
	{
		return CachedPrefixAchievementID;
	}

	public static string PrefixInAppPurchaseID()
	{
		return CachedPrefixInAppPurchaseID;
	}
}
