using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class NotifyManager : SingletonMonoBehaviour<NotifyManager>
{
	private string[] dailyNotificationStringKeys = new string[5] { "localnotification.dailyNotification0", "localnotification.dailyNotification1", "localnotification.dailyNotification2", "localnotification.dailyNotification3", "localnotification.dailyNotification4" };

	private string[] dailyGiftNotificationsStringKeys = new string[5] { "localnotification.dailyGift0", "localnotification.dailyGift1", "localnotification.dailyGift2", "localnotification.dailyGift3", "localnotification.dailyGift4" };

	private const string firstWintGiftNotificationStringKey = "localnotification.firstWinInDay";

	private const string freeChestNotificationStringKey = "localnotification.freeChest";

	private const string adsPopupNotificationStringKey = "localnotification.adsPopup";

	private const string adsPopupNotificationStringKeySecond = "localnotification.adsPopup.second";

	private const string notificationSoundName = "push_notification_0";

	private const string notificationContentKey = "localnotification.pressForMore";

	private const string notificationFreeChestTitleKey = "localnotification.android.freeChest.title";

	private const string notificationFreeChestContentKey = "localnotification.android.freeChest.checkItOut";

	private const string notificationFirstDailyWinTitleKey = "localnotification.android.firstDailyWin.title";

	private string[] notificationFirstDailyWinContentKey = new string[2] { "localnotification.android.firstDailyWin.newCompetition", "localnotification.android.firstDailyWin.defeatOpponents" };

	private const string notificationDailyGiftNotifTitleKey = "localnotification.android.dailyGiftNotif.title";

	private string[] notificationDailyGiftNotifContentKey = new string[2] { "localnotification.android.dailyGiftNotif.takeYourPrize", "localnotification.android.dailyGiftNotif.specialGift" };

	private const string notificationDailyNotifTitleKey = "localnotification.android.dailyNotif.title";

	private string[] notificationDailyNotifContentKey = new string[2] { "localnotification.android.dailyNotif.weapon", "localnotification.android.dailyNotif.victory" };

	private const string LOCAL_NOTIFICATIONS_ENABLE_KEY = "LocalNotifyEnable";

	private const string REGISTERED_KEY = "IsNotificationsRegistered";

	private const string DAILY_GIFT_NOTIFICATION_ENABLED = "DailyGiftNotificationEnabled";

	private const string ADS_POPUP_NOTIFICATION_ENABLED = "AdsPopupNotificationEnabled";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnLocalNotificationsEnabled;

	[SerializeField]
	private int minTimeToNotificationInMinutes = 5;

	public bool IsLocalNotificationsEnable
	{
		get
		{
			return CustomPlayerPrefs.GetInt("LocalNotifyEnable", 1) != 0;
		}
		set
		{
			if (value != IsLocalNotificationsEnable)
			{
				CustomPlayerPrefs.SetInt("LocalNotifyEnable", value ? 1 : 0);
				if (NotifyManager.OnLocalNotificationsEnabled != null)
				{
					NotifyManager.OnLocalNotificationsEnabled(value);
				}
				LocalNotificationEnable(value, 0f, string.Empty);
			}
		}
	}

	private bool IsNotificationsRegistered
	{
		get
		{
			return CustomPlayerPrefs.GetInt("IsNotificationsRegistered") != 0;
		}
		set
		{
			CustomPlayerPrefs.SetInt("IsNotificationsRegistered", value ? 1 : 0);
		}
	}

	private bool IsFreeChestNotificationEnabled
	{
		get
		{
			StatisticsManager instance = SingletonMonoBehaviour<StatisticsManager>.Instance;
			bool flag = instance.MathesCount >= SingletonMonoBehaviour<ProposalManager>.Instance.MinMatchesForFreeChest;
			return flag & (instance.FreeChestsOpened >= 1);
		}
	}

	public bool IsDailyGiftNotificationEnabled
	{
		get
		{
			return CustomPlayerPrefs.GetBool("DailyGiftNotificationEnabled");
		}
		set
		{
			CustomPlayerPrefs.SetBool("DailyGiftNotificationEnabled", value);
		}
	}

	public bool IsAdsPopupNotificationEnabled
	{
		get
		{
			return CustomPlayerPrefs.GetBool("AdsPopupNotificationEnabled");
		}
		set
		{
			CustomPlayerPrefs.SetBool("AdsPopupNotificationEnabled", value);
		}
	}

	public static event Action<bool> OnLocalNotificationsEnabled
	{
		add
		{
			Action<bool> action = NotifyManager.m_OnLocalNotificationsEnabled;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref NotifyManager.m_OnLocalNotificationsEnabled, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = NotifyManager.m_OnLocalNotificationsEnabled;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref NotifyManager.m_OnLocalNotificationsEnabled, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public void TryToRegisterForLocalNotifications()
	{
		if (!IsNotificationsRegistered)
		{
			RegisterForLocalNotifications();
			IsNotificationsRegistered = true;
		}
	}

	public void UpdateNotifications(float secondsForAdsPopup = 0f, string characterName = "")
	{
		LocalNotificationEnable(IsLocalNotificationsEnable, secondsForAdsPopup, characterName);
	}

	private void LocalNotificationEnable(bool enable, float secondsForAdsPopup = 0f, string characterName = "")
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.LOCAL_NOTIFICATION_BIND);
		androidJavaClass.CallStatic(AndroidPluginNames.LOCAL_NOTIFICATION_BIND_CANCEL_ALL_NOTIFICATIONS);
		if (enable)
		{
			RegisterDailyNotification(9, 45);
			RegisterDailyGiftNotification();
			RegisterFirstDailyWinNotification();
			RegisterFreeChestNotification();
			RegisterAdsPopupNotification(secondsForAdsPopup, characterName);
		}
	}

	private void RegisterDailyNotification(int hours, int minutes)
	{
		DateTime dateTime = DateTime.Today.AddHours(hours).AddMinutes(minutes);
		if ((dateTime - DateTime.Now).TotalMinutes < (double)minTimeToNotificationInMinutes)
		{
			dateTime = dateTime.AddDays(1.0);
		}
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.LOCAL_NOTIFICATION_BIND);
		androidJavaClass.CallStatic(AndroidPluginNames.LOCAL_NOTIFICATION_BIND_SCHEDULE_LOCAL_NOTIFICATION, "localnotification.android.dailyNotif.title", SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("localnotification.android.dailyNotif.title"), SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(notificationDailyNotifContentKey.RandomObject()), "push_notification_0", (int)dateTime.Subtract(DateTime.Now).TotalSeconds);
	}

	private void RegisterDailyGiftNotification()
	{
		if (IsDailyGiftNotificationEnabled)
		{
			DateTime dateTime = Max(SingletonMonoBehaviour<GiftManager>.Instance.DateNextGift, DateTime.Now.AddMinutes(minTimeToNotificationInMinutes));
			AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.LOCAL_NOTIFICATION_BIND);
			androidJavaClass.CallStatic(AndroidPluginNames.LOCAL_NOTIFICATION_BIND_SCHEDULE_LOCAL_NOTIFICATION, "localnotification.android.dailyGiftNotif.title", SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("localnotification.android.dailyGiftNotif.title"), SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(notificationDailyGiftNotifContentKey.RandomObject()), "push_notification_0", (int)dateTime.Subtract(DateTime.Now).TotalSeconds);
		}
	}

	private void RegisterFirstDailyWinNotification()
	{
		DateTime dateTime = Max(SingletonMonoBehaviour<StatisticsManager>.Instance.LastWinDate.AddDays(1.0), DateTime.Now.AddMinutes(minTimeToNotificationInMinutes));
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.LOCAL_NOTIFICATION_BIND);
		androidJavaClass.CallStatic(AndroidPluginNames.LOCAL_NOTIFICATION_BIND_SCHEDULE_LOCAL_NOTIFICATION, "localnotification.android.firstDailyWin.title", SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("localnotification.android.firstDailyWin.title"), SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(notificationFirstDailyWinContentKey.RandomObject()), "push_notification_0", (int)dateTime.Subtract(DateTime.Now).TotalSeconds);
	}

	private void RegisterFreeChestNotification()
	{
		if (IsFreeChestNotificationEnabled)
		{
			DateTime dateTime = Max(DateTime.Now.AddSeconds(SingletonMonoBehaviour<ProposalManager>.Instance.FreeChestTimer.TimerInSeconds), DateTime.Now.AddMinutes(minTimeToNotificationInMinutes));
			AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.LOCAL_NOTIFICATION_BIND);
			androidJavaClass.CallStatic(AndroidPluginNames.LOCAL_NOTIFICATION_BIND_SCHEDULE_LOCAL_NOTIFICATION, "localnotification.freeChest", SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("localnotification.android.freeChest.title"), SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("localnotification.android.freeChest.checkItOut"), "push_notification_0", (int)dateTime.Subtract(DateTime.Now).TotalSeconds);
		}
	}

	private void RegisterAdsPopupNotification(float fireTime, string characterName)
	{
		if (!IsAdsPopupNotificationEnabled)
		{
		}
	}

	private void RegisterForLocalNotifications()
	{
	}

	private DateTime Max(DateTime a, DateTime b)
	{
		return (!(a < b)) ? a : b;
	}
}
