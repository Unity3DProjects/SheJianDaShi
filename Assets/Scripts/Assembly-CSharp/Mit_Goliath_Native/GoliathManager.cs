using System.Collections.Generic;
using System.Diagnostics;
using Mit_Goliath_Native.Implementations;
using Mit_ServicesUtils;
using UnityEngine;

namespace Mit_Goliath_Native
{
	public class GoliathManager : MonoBehaviour
	{
		private delegate string StringGetter();

		private delegate int IntGetter();

		private const bool LOGS_ARE_ENABLED = false;

		private const string SENDER_TYPE = "client";

		public string gameId = "gameId";

		public string gameName = "gameName";

		public string apikeyDevelopment = "apikey";

		public string apikeyProduction = "apikey";

		public string hmacKeyDevelopment = "hmacKey";

		public string hmacKeyProduction = "hmacKey";

		public string version = "version";

		public string endpointUrl = "endpointUrl";

		private string hmacKey = "hmacKey";

		private string environment = "environment";

		public int maxNumberOfEventsPerPackage = 3;

		public int maxTransmissionInterval = 2000;

		public int maxSerializationInterval = 4000;

		private string apikey = string.Empty;

		private GoliathImplementation implementation;

		protected void Awake()
		{
			SetImplementation();
			SetApiKey();
			SetHmacKey();
			SetEnvironment();
			SetSender();
			SetGameName();
			implementation.Setup(gameId, apikey, version, endpointUrl, hmacKey);
			implementation.SetMaxNumberOfEventsPerPackage(maxNumberOfEventsPerPackage);
			implementation.SetTransmissionInterval(maxTransmissionInterval);
			implementation.SetSerializationInterval(maxSerializationInterval);
			SetCallbacks();
		}

		private void SetImplementation()
		{
			implementation = base.gameObject.AddComponent<CnativeGoliathImplementation>();
		}

		private void SetApiKey()
		{
			if (PluginsUtils.IsDevelopmentBuild())
			{
				apikey = apikeyDevelopment;
			}
			else
			{
				apikey = apikeyProduction;
			}
		}

		private void SetHmacKey()
		{
			if (PluginsUtils.IsDevelopmentBuild())
			{
				hmacKey = hmacKeyDevelopment;
			}
			else
			{
				hmacKey = hmacKeyProduction;
			}
		}

		private void SetEnvironment()
		{
			if (PluginsUtils.IsDevelopmentBuild())
			{
				environment = "development";
			}
			else
			{
				environment = "production";
			}
			implementation.SetEnvironment(environment);
		}

		private void SetSender()
		{
			implementation.SetSender("client");
		}

		private void SetGameName()
		{
			implementation.SetGameName(gameName);
		}

		private void SetCallbacks()
		{
			implementation.SetOnStartCallback(delegate
			{
			});
			implementation.SetOnStopCallback(delegate
			{
			});
			implementation.SetSendSuccessCallback(delegate
			{
			});
			implementation.SetSendErrorCallback(delegate
			{
			});
			implementation.SetTransmissionSuccessCallback(delegate
			{
			});
			implementation.SetTransmissionFailureCallback(delegate
			{
			});
		}

		public void SetUserId(string userId)
		{
			implementation.SetUserId(userId);
		}

		public void SetSessionId(string sessionId)
		{
			implementation.SetSessionId(sessionId);
		}

		public Dictionary<string, string> GetGlobalParameters()
		{
			return implementation.GetGlobalParameters();
		}

		public void SetGlobalParameters(Dictionary<string, object> globalParameters)
		{
			implementation.SetGlobalParameters(globalParameters);
		}

		public void AddGlobalParameter(string key, string value)
		{
			implementation.AddGlobalParameter(key, value);
		}

		public void SetMaxNumberOfEventsPerPackage(int eventsNumber)
		{
			implementation.SetMaxNumberOfEventsPerPackage(eventsNumber);
		}

		public void SetTransmissionInterval(int interval)
		{
			implementation.SetTransmissionInterval(interval);
		}

		public void SetSerializationInterval(int interval)
		{
			implementation.SetSerializationInterval(interval);
		}

		public void PostEvent(string eventType, Dictionary<string, object> eventParameters)
		{
			implementation.PostEvent(eventType, eventParameters);
		}

		public void GoliathStart()
		{
			implementation.GoliathStart();
		}

		public void GoliathStop()
		{
			implementation.GoliathStop();
		}

		public string GetPathForSavedFile()
		{
			return implementation.GetPathForSavedFile();
		}

		protected void PrintAllData()
		{
		}

		protected void PostClientInitEvent(string facebookID, string tokenForbusiness, string userID, string sessionID)
		{
			implementation.PostClientInitEvent(-1, string.Empty, (!string.IsNullOrEmpty(facebookID)) ? facebookID : string.Empty, (!string.IsNullOrEmpty(tokenForbusiness)) ? tokenForbusiness : string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, -1, userID, sessionID);
		}

		protected void PostClientInitEvent(int pushNotificationsEnabled, string pushToken, string facebookID, string facebookIDforBusiness, string googlePlusID, string googleGamerID, string miniclipID, string dateOfBirth, string gender, int isDeviceCompromised, string userID, string sessionID)
		{
			implementation.PostClientInitEvent(pushNotificationsEnabled, (!string.IsNullOrEmpty(pushToken)) ? pushToken : string.Empty, (!string.IsNullOrEmpty(facebookID)) ? facebookID : string.Empty, (!string.IsNullOrEmpty(facebookIDforBusiness)) ? facebookIDforBusiness : string.Empty, (!string.IsNullOrEmpty(googlePlusID)) ? googlePlusID : string.Empty, (!string.IsNullOrEmpty(googleGamerID)) ? googleGamerID : string.Empty, (!string.IsNullOrEmpty(miniclipID)) ? miniclipID : string.Empty, (!string.IsNullOrEmpty(dateOfBirth)) ? dateOfBirth : string.Empty, (!string.IsNullOrEmpty(gender)) ? gender : string.Empty, isDeviceCompromised, userID, sessionID);
		}

		private string GetDbgParameter_str(string paramName, StringGetter getter)
		{
			string text = "NO GETTER";
			if (getter != null)
			{
				text = getter();
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "NULL OR EMPTY";
			}
			return paramName + ": " + text;
		}

		private string GetDbgParameter_int(string paramName, IntGetter getter)
		{
			string text = "NO GETTER";
			if (getter != null)
			{
				text = getter().ToString();
			}
			return paramName + ": " + text;
		}

		protected string GetPlatform()
		{
			RuntimePlatform platform = Application.platform;
			string empty = string.Empty;
			switch (platform)
			{
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			case RuntimePlatform.WP8Player:
				return "WP8";
			case RuntimePlatform.WebGLPlayer:
				return "WebGL";
			default:
				return "unknown";
			}
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
