using System;
using UnityEngine;

namespace com.adjust.sdk
{
	public class Adjust : MonoBehaviour
	{
		private const string errorMsgStart = "Adjust: SDK not started. Start it manually using the 'start' method.";

		private const string errorMsgPlatform = "Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

		public bool startManually = true;

		public bool eventBuffering;

		public bool sendInBackground;

		public bool launchDeferredDeeplink = true;

		public string appToken = "{Your App Token}";

		public AdjustLogLevel logLevel = AdjustLogLevel.Info;

		public AdjustEnvironment environment;

		private void Awake()
		{
			if (!Application.isEditor)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
				if (!startManually)
				{
					AdjustConfig adjustConfig = new AdjustConfig(appToken, environment, logLevel == AdjustLogLevel.Suppress);
					adjustConfig.setLogLevel(logLevel);
					adjustConfig.setSendInBackground(sendInBackground);
					adjustConfig.setEventBufferingEnabled(eventBuffering);
					adjustConfig.setLaunchDeferredDeeplink(launchDeferredDeeplink);
					start(adjustConfig);
				}
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (!Application.isEditor)
			{
				if (pauseStatus)
				{
					AdjustAndroid.OnPause();
				}
				else
				{
					AdjustAndroid.OnResume();
				}
			}
		}

		public static void start(AdjustConfig adjustConfig)
		{
			if (!Application.isEditor)
			{
				if (adjustConfig == null)
				{
					Debug.Log("Adjust: Missing config to start.");
				}
				else
				{
					AdjustAndroid.Start(adjustConfig);
				}
			}
		}

		public static void trackEvent(AdjustEvent adjustEvent)
		{
			if (!Application.isEditor)
			{
				if (adjustEvent == null)
				{
					Debug.Log("Adjust: Missing event to track.");
				}
				else
				{
					AdjustAndroid.TrackEvent(adjustEvent);
				}
			}
		}

		public static void setEnabled(bool enabled)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.SetEnabled(enabled);
			}
		}

		public static bool isEnabled()
		{
			if (!Application.isEditor)
			{
				return AdjustAndroid.IsEnabled();
			}
			return false;
		}

		public static void setOfflineMode(bool enabled)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.SetOfflineMode(enabled);
			}
		}

		public static void setDeviceToken(string deviceToken)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.SetDeviceToken(deviceToken);
			}
		}

		public static void appWillOpenUrl(string url)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.AppWillOpenUrl(url);
			}
		}

		public static void sendFirstPackages()
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.SendFirstPackages();
			}
		}

		public static void addSessionPartnerParameter(string key, string value)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.AddSessionPartnerParameter(key, value);
			}
		}

		public static void addSessionCallbackParameter(string key, string value)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.AddSessionCallbackParameter(key, value);
			}
		}

		public static void removeSessionPartnerParameter(string key)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.RemoveSessionPartnerParameter(key);
			}
		}

		public static void removeSessionCallbackParameter(string key)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.RemoveSessionCallbackParameter(key);
			}
		}

		public static void resetSessionPartnerParameters()
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.ResetSessionPartnerParameters();
			}
		}

		public static void resetSessionCallbackParameters()
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.ResetSessionCallbackParameters();
			}
		}

		public static string getAdid()
		{
			if (!Application.isEditor)
			{
				return AdjustAndroid.GetAdid();
			}
			return string.Empty;
		}

		public static AdjustAttribution getAttribution()
		{
			if (!Application.isEditor)
			{
				return AdjustAndroid.GetAttribution();
			}
			return null;
		}

		public static string getWinAdid()
		{
			if (!Application.isEditor)
			{
				Debug.Log("Adjust: Error! Windows Advertising ID is not available on Android platform.");
				return string.Empty;
			}
			return string.Empty;
		}

		public static string getIdfa()
		{
			if (!Application.isEditor)
			{
				Debug.Log("Adjust: Error! IDFA is not available on Android platform.");
				return string.Empty;
			}
			return string.Empty;
		}

		[Obsolete("This method is intended for testing purposes only. Do not use it.")]
		public static void setReferrer(string referrer)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.SetReferrer(referrer);
			}
		}

		public static void getGoogleAdId(Action<string> onDeviceIdsRead)
		{
			if (!Application.isEditor)
			{
				AdjustAndroid.GetGoogleAdId(onDeviceIdsRead);
			}
		}

		public static string getAmazonAdId()
		{
			if (!Application.isEditor)
			{
				return AdjustAndroid.GetAmazonAdId();
			}
			return string.Empty;
		}
	}
}
