using System;
using Mit_Goliath_Native.Utils;
using UnityEngine;

public class SubscriptionManager : SingletonMonoBehaviour<SubscriptionManager>
{
	private const int DAYS_IN_TRIAL = 3;

	[SerializeField]
	private int coinsForDay = 1000;

	[SerializeField]
	private int gemsForDay = 100;

	[SerializeField]
	private string privacyPolicyLink;

	[SerializeField]
	private string privacyPolicyTitle;

	[SerializeField]
	private string termsOfUseLink;

	[SerializeField]
	private string termsOfUseTitle;

	[SerializeField]
	private string subscriptionLink;

	[SerializeField]
	private string subscriptionTitle;

	private bool subscriptionStateEventSent;

	private Managers cachedManagers;

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	private DateTime CheckedDate
	{
		get
		{
			DateTime lastRealUtcDate = CachedManagers.CachedStatisticsManager.LastRealUtcDate;
			DateTime dateTime = DateTime.UtcNow.Subtract(CachedManagers.CachedStatisticsManager.CheatedTime);
			if (lastRealUtcDate == DateTime.MinValue)
			{
				return dateTime;
			}
			int num = DateTime.Compare(dateTime, lastRealUtcDate);
			return (num >= 0) ? dateTime : lastRealUtcDate;
		}
	}

	public int CoinsForDay
	{
		get
		{
			return coinsForDay;
		}
	}

	public int GemsForDay
	{
		get
		{
			return gemsForDay;
		}
	}

	public bool IsSubscriptionActive
	{
		get
		{
			return !string.IsNullOrEmpty(SubscriptionForDate(CheckedDate).productID);
		}
	}

	public string ActiveSubscription
	{
		get
		{
			SubscriptionResult subscriptionResult = SubscriptionForDate(CheckedDate);
			if (CachedManagers.CachedShopManager.SubscriptionWeek.ProductID == subscriptionResult.productID)
			{
				return CachedManagers.CachedShopManager.SubscriptionWeek.ShopItemID;
			}
			if (CachedManagers.CachedShopManager.SubscriptionYear.ProductID == subscriptionResult.productID)
			{
				return CachedManagers.CachedShopManager.SubscriptionYear.ShopItemID;
			}
			if (CachedManagers.CachedShopManager.SubscriptionMonth.ProductID == subscriptionResult.productID)
			{
				return CachedManagers.CachedShopManager.SubscriptionMonth.ShopItemID;
			}
			return subscriptionResult.productID;
		}
	}

	public string LastActiveSubscription
	{
		get
		{
			return CustomPlayerPrefs.GetString("last_active_subscription");
		}
		set
		{
			CustomPlayerPrefs.SetString("last_active_subscription", value);
		}
	}

	public bool LastActiveSubscriptionDataSent
	{
		get
		{
			return CustomPlayerPrefs.GetBool("subscription_was_activated");
		}
		set
		{
			CustomPlayerPrefs.SetBool("subscription_was_activated", value);
		}
	}

	public DateTime LastSubscriptionRewardDate
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("subscribe_reward_date", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("subscribe_reward_date", value);
		}
	}

	public int SubscriptionRewardCount
	{
		get
		{
			DateTime lastSubscriptionRewardDate = LastSubscriptionRewardDate;
			TimeSpan timeSpan = default(TimeSpan);
			if (lastSubscriptionRewardDate != DateTime.MinValue)
			{
				SubscriptionResult subscriptionResult = SubscriptionForDate(lastSubscriptionRewardDate);
				SubscriptionResult subscriptionResult2 = SubscriptionForDate(CheckedDate);
				if (subscriptionResult.subscriptionID == subscriptionResult2.subscriptionID)
				{
					timeSpan = CheckedDate - lastSubscriptionRewardDate;
				}
				else
				{
					TimeSpan timeSpan2 = subscriptionResult.expirationDate - lastSubscriptionRewardDate;
					TimeSpan timeSpan3 = CheckedDate - subscriptionResult2.purchaseDate;
					timeSpan = timeSpan2 + timeSpan3;
				}
			}
			else
			{
				SubscriptionResult subscriptionResult3 = LastSubscription();
				if (!string.IsNullOrEmpty(subscriptionResult3.productID))
				{
					LastSubscriptionRewardDate = subscriptionResult3.purchaseDate;
					timeSpan = CheckedDate - subscriptionResult3.purchaseDate;
					int num = ((!CachedManagers.CachedShopManager.IsPurchasesInSandbox) ? timeSpan.Days : (timeSpan.Minutes / 2));
					return num + 1;
				}
			}
			return (!CachedManagers.CachedShopManager.IsPurchasesInSandbox) ? timeSpan.Days : (timeSpan.Minutes / 2);
		}
	}

	public string PrivacyPolicyLink
	{
		get
		{
			return privacyPolicyLink;
		}
	}

	public string PrivacyPolicyTitle
	{
		get
		{
			return privacyPolicyTitle;
		}
	}

	public string SubscriptionLink
	{
		get
		{
			return subscriptionLink;
		}
	}

	public string SubscriptionTitle
	{
		get
		{
			return subscriptionTitle;
		}
	}

	public string TermsOfUseLink
	{
		get
		{
			return termsOfUseLink;
		}
	}

	public string TermsOfUseTitle
	{
		get
		{
			return termsOfUseTitle;
		}
	}

	private void Start()
	{
	}

	public void OnEnable()
	{
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.OnQueryInventoryCompleted += MCInnAppsmanager_OnQueryInventoryCompleted;
	}

	public void OnDisable()
	{
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.OnQueryInventoryCompleted -= MCInnAppsmanager_OnQueryInventoryCompleted;
	}

	public bool IsSubscriptionInTrial(SubscriptionResult subscriptionResult)
	{
		return CachedManagers.CachedShopManager.SubscriptionWeek.ProductID == subscriptionResult.productID && (DateTime.Now - subscriptionResult.purchaseDate).TotalDays < 3.0;
	}

	public void SendSubscriptionStateEvent()
	{
		if (!(GameGoliath.Instance == null) && IsSubscriptionActive && !subscriptionStateEventSent && SingletonMonoBehaviour<MCInnAppsmanager>.Instance.IsQueryInventoryOK && GameGoliath.Instance.SessionBeginSent)
		{
			SubscriptionResult subscriptionResult = SubscriptionForDate(CheckedDate);
			string productID = subscriptionResult.productID;
			string subscriptionID = subscriptionResult.subscriptionID;
			DateTime expirationDate = subscriptionResult.expirationDate;
			bool isFreeTrial = IsSubscriptionInTrial(subscriptionResult);
			int timeToExpire = (((expirationDate - DateTime.Now).Seconds >= 0) ? (expirationDate - DateTime.Now).Seconds : 0);
			GameGoliath.Instance.TrackSubscriptonState(productID, isFreeTrial, subscriptionID, timeToExpire);
			subscriptionStateEventSent = true;
		}
	}

	public void MCInnAppsmanager_OnQueryInventoryCompleted(bool success)
	{
		if (success)
		{
			if (IsSubscriptionActive)
			{
				LastActiveSubscription = ActiveSubscription;
				LastActiveSubscriptionDataSent = false;
				SendSubscriptionStateEvent();
			}
			else if (!string.IsNullOrEmpty(LastActiveSubscription) && !LastActiveSubscriptionDataSent)
			{
				LastActiveSubscriptionDataSent = true;
				GameGoliath.Instance.TrackSubscriptonAction(LastActiveSubscription, GoliathIds.Action.CANCELLED, string.Empty);
				SingletonMonoBehaviour<AdjustManager>.Instance.TrackSubscriptionCancel();
			}
		}
	}

	public void claimReward()
	{
		SubscriptionResult subscriptionResult = SubscriptionForDate(CheckedDate);
		TimeSpan timeSpan = CheckedDate - LastSubscriptionRewardDate;
		if (!string.IsNullOrEmpty(subscriptionResult.productID))
		{
			if (CachedManagers.CachedShopManager.IsPurchasesInSandbox)
			{
				int num = timeSpan.Minutes / 2;
				LastSubscriptionRewardDate = LastSubscriptionRewardDate.AddMinutes((float)num * 2f);
			}
			else
			{
				int days = timeSpan.Days;
				LastSubscriptionRewardDate = LastSubscriptionRewardDate.AddDays(days);
			}
		}
		else
		{
			LastSubscriptionRewardDate = CheckedDate;
		}
	}

	private SubscriptionResult SubscriptionForDate(DateTime date)
	{
		SubscriptionResult subscriptionResult = default(SubscriptionResult);
		return SingletonMonoBehaviour<MCInnAppsmanager>.Instance.SubscriptionForDate(date);
	}

	private SubscriptionResult SubscriptionForID(string subscriptionID)
	{
		SubscriptionResult subscriptionResult = default(SubscriptionResult);
		return SingletonMonoBehaviour<MCInnAppsmanager>.Instance.SubscriptionForID(subscriptionID);
	}

	private SubscriptionResult LastSubscription()
	{
		SubscriptionResult subscriptionResult = default(SubscriptionResult);
		return SingletonMonoBehaviour<MCInnAppsmanager>.Instance.LastSubscription();
	}
}
