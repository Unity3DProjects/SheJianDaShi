using System;
using System.IO;

public sealed class LLNotificationManager
{
	private const string METHOD_SCHEDULE_LOCAL_NOTIFICATION = "LLNotificationManagerScheduleLocalNotification";

	private const string METHOD_CANCEL_ALL_LOCAL_NOTIFICATION_WITH_KEY = "LLNotificationManagerCancelAllLocalNotificationWithKey";

	public static void ScheduleLocalNotification(string notificationKey, string notificationId, string iconFileName, string title, string alertBody, DateTime fireDate)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(iconFileName);
		LLAndroidJavaSingletone<LLNotificationManager>.CallStatic("LLNotificationManagerScheduleLocalNotification", notificationKey, notificationId, fileNameWithoutExtension, title, alertBody, fireDate.ToString("G"));
	}

	public static void CancelAllLocalNotification(string notificationKey)
	{
		LLAndroidJavaSingletone<LLNotificationManager>.CallStatic("LLNotificationManagerCancelAllLocalNotificationWithKey", notificationKey);
	}
}
