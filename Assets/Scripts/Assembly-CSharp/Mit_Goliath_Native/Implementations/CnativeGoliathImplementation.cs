using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AOT;
using Mit_ServicesUtils;
using UnityEngine;

namespace Mit_Goliath_Native.Implementations
{
	public class CnativeGoliathImplementation : MonoBehaviour, GoliathImplementation
	{
		public delegate void TransmissionSuccessDelegate(IntPtr receivedData, uint sizeOfReceivedData, string requestTransmitted);

		public delegate void TransmissionFailureDelegate(int errorCode, string requestTransmitted);

		public delegate void StartStopDelegate();

		private const bool LOGS_ARE_ENABLED = false;

		private static Action gameStartCallback = null;

		private static Action gameStopCallback = null;

		private static Action<string, string> gameTransmissionSuccessCallback = null;

		private static Action<int, string> gameTransmissionFailureCallback = null;

		private static CallbackEvents callbackEvents = new CallbackEvents();

		[DllImport("mcprime")]
		public static extern void mc_connection_load_helper();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperInit(string gameID, string apiKey, string version, string host, string hmacKey, string pathForSaveFile);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperAppVersion();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetAppVersion(string version);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperUserID();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetUserID(string userID);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperSessionID();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetSessionID(string sessionID);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperHost();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetHost(string goliathHost);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperGameID();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetGameID(string gameID);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperAPIKey();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetAPIKey(string apiKey);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperHmacKey();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetHmacKey(string hmacKey);

		[DllImport("mcgoliath")]
		public static extern IntPtr goliathWrapperPathForSaveFile();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetPathForSaveFile(string filepath);

		[DllImport("mcgoliath")]
		public static extern int goliathWrapperMaxNumberOfEventsPerPackage();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetMaxNumberOfEventsPerPackage(int eventsNumber);

		[DllImport("mcgoliath")]
		public static extern int goliathWrapperTransmissionInterval();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetTransmissionInterval(int interval);

		[DllImport("mcgoliath")]
		public static extern int goliathWrapperSerializationInterval();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetSerializationInterval(int interval);

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetTransmissionSuccessCallback(TransmissionSuccessDelegate callback);

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetTransmissionFailureCallback(TransmissionFailureDelegate callback);

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetOnStartCallback(StartStopDelegate callback);

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetOnStopCallback(StartStopDelegate callback);

		[DllImport("mcgoliath")]
		public static extern int goliathWrapperPostEvent(string eventType, string eventContents);

		[DllImport("mcgoliath")]
		public static extern int goliathWrapperPostClientInitEvent(int pushNotificationsEnabled = -1, string pushToken = "", string facebookID = "", string facebookIDforBusiness = "", string googlePlusID = "", string googleGamerID = "", string miniclipID = "", string dob = "", string gender = "", int sDeviceCompromised = -1, string userID = "", string sessionID = "");

		[DllImport("mcgoliath")]
		public static extern int goliathWrapperPostConfigurationUpdateEvent(string latestVersion, string previousVersion = "");

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperAddGlobalParameter(string key, string value);

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperGetGlobalParameters(IntPtr[] keys, IntPtr[] values, ref int length);

		[DllImport("mcgoliath")]
		public static extern int goliathWrapperGetGlobalParametersSize();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperSetGlobalParameters(string[] keys, string[] values, IntPtr length);

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperStart();

		[DllImport("mcgoliath")]
		public static extern void goliathWrapperStop();

		public void SetOnStartCallback(Action callback)
		{
			gameStartCallback = callback;
		}

		public void SetOnStopCallback(Action callback)
		{
			gameStopCallback = callback;
		}

		public void SetSendSuccessCallback(Action callback)
		{
		}

		public void SetSendErrorCallback(Action callback)
		{
		}

		public void SetTransmissionSuccessCallback(Action<string, string> callback)
		{
			gameTransmissionSuccessCallback = callback;
		}

		public void SetTransmissionFailureCallback(Action<int, string> callback)
		{
			gameTransmissionFailureCallback = callback;
		}

		public void Setup(string gameId, string apiKey, string version, string endpointUrl, string hmacKey)
		{
			mc_connection_load_helper();
			string empty = string.Empty;
			goliathWrapperInit(gameId, apiKey, version, endpointUrl, hmacKey, empty);
			goliathWrapperSetOnStartCallback(StartCallback);
			goliathWrapperSetOnStopCallback(StopCallback);
			goliathWrapperSetTransmissionSuccessCallback(TransmissionSuccessCallback);
			goliathWrapperSetTransmissionFailureCallback(TransmissionFailureCallback);
			goliathWrapperSetMaxNumberOfEventsPerPackage(3);
			goliathWrapperSetTransmissionInterval(2000);
			goliathWrapperSetSerializationInterval(4000);
		}

		public string GetAppVersion()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperAppVersion());
		}

		public void SetAppVersion(string version)
		{
			goliathWrapperSetAppVersion(version);
		}

		public string GetUserId()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperUserID());
		}

		public void SetUserId(string userId)
		{
			goliathWrapperSetUserID(userId);
		}

		public string GetSessionId()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperSessionID());
		}

		public void SetSessionId(string sessionId)
		{
			goliathWrapperSetSessionID(sessionId);
		}

		public string GetHost()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperHost());
		}

		public void SetHost(string host)
		{
			goliathWrapperSetHost(host);
		}

		public string GetGameId()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperGameID());
		}

		public void SetGameId(string gameId)
		{
			goliathWrapperSetGameID(gameId);
		}

		public string GetApiKey()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperAPIKey());
		}

		public void SetApiKey(string apiKey)
		{
			goliathWrapperSetAPIKey(apiKey);
		}

		public string GetHmacKey()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperHmacKey());
		}

		public void SetHmacKey(string hmacKey)
		{
			goliathWrapperSetHmacKey(hmacKey);
		}

		public string GetPathForSavedFile()
		{
			return Marshal.PtrToStringAnsi(goliathWrapperPathForSaveFile());
		}

		public void SetPathForSavedFile(string path)
		{
			goliathWrapperSetPathForSaveFile(path);
		}

		public int GetMaxNumberOfEventsPerPackage()
		{
			return goliathWrapperMaxNumberOfEventsPerPackage();
		}

		public void SetMaxNumberOfEventsPerPackage(int eventsNumber)
		{
			goliathWrapperSetMaxNumberOfEventsPerPackage(eventsNumber);
		}

		public int GetTransmissionInterval()
		{
			return goliathWrapperTransmissionInterval();
		}

		public void SetTransmissionInterval(int interval)
		{
			goliathWrapperSetTransmissionInterval(interval);
		}

		public int GetSerializationInterval()
		{
			return goliathWrapperSerializationInterval();
		}

		public void SetSerializationInterval(int interval)
		{
			goliathWrapperSetSerializationInterval(interval);
		}

		public void SetGameName(string gameName)
		{
		}

		public void SetEnvironment(string environment)
		{
		}

		public void SetSender(string sender)
		{
		}

		public int PostEvent(string eventType, Dictionary<string, object> eventParameters)
		{
			string eventContents = Json.Serialize(eventParameters);
			return goliathWrapperPostEvent(eventType, eventContents);
		}

		public int PostClientInitEvent(int pushNotificationsEnabled = -1, string pushToken = "", string facebookID = "", string facebookIDforBusiness = "", string googlePlusID = "", string googleGamerID = "", string miniclipID = "", string dob = "", string gender = "", int sDeviceCompromised = -1, string userID = "", string sessionID = "")
		{
			return goliathWrapperPostClientInitEvent(pushNotificationsEnabled, pushToken, facebookID, facebookIDforBusiness, googlePlusID, googleGamerID, miniclipID, dob, gender, sDeviceCompromised, userID, sessionID);
		}

		public int PostConfigurationUpdateEvent(string latestVersion, string previousVersion = "")
		{
			return goliathWrapperPostConfigurationUpdateEvent(latestVersion, previousVersion);
		}

		public Dictionary<string, string> GetGlobalParameters()
		{
			int length = goliathWrapperGetGlobalParametersSize();
			if (length <= 0)
			{
				return null;
			}
			IntPtr[] array = new IntPtr[length];
			IntPtr[] array2 = new IntPtr[length];
			goliathWrapperGetGlobalParameters(array, array2, ref length);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int i = 0; i < length; i++)
			{
				dictionary.Add(Marshal.PtrToStringAnsi(array[i]), Marshal.PtrToStringAnsi(array2[i]));
			}
			return dictionary;
		}

		public void SetGlobalParameters(Dictionary<string, object> globalParameters)
		{
			if (globalParameters == null || globalParameters.Count <= 0)
			{
				return;
			}
			int count = globalParameters.Count;
			string[] array = new string[count];
			string[] array2 = new string[count];
			int num = 0;
			foreach (KeyValuePair<string, object> globalParameter in globalParameters)
			{
				array[num] = globalParameter.Key;
				array2[num] = globalParameter.Value.ToString();
				num++;
			}
			goliathWrapperSetGlobalParameters(array, array2, new IntPtr(count));
		}

		public void AddGlobalParameter(string key, string value)
		{
			goliathWrapperAddGlobalParameter(key, value);
		}

		public void GoliathStart()
		{
			goliathWrapperStart();
		}

		public void GoliathStop()
		{
			goliathWrapperStop();
		}

		[MonoPInvokeCallback(typeof(StartStopDelegate))]
		private static void StartCallback()
		{
			callbackEvents.AddEvent(gameStartCallback);
		}

		[MonoPInvokeCallback(typeof(StartStopDelegate))]
		private static void StopCallback()
		{
			callbackEvents.AddEvent(gameStopCallback);
		}

		[MonoPInvokeCallback(typeof(TransmissionSuccessDelegate))]
		private static void TransmissionSuccessCallback(IntPtr receivedData, uint sizeOfReceivedData, string requestTransmitted)
		{
			string receivedDataString = string.Empty;
			if (sizeOfReceivedData != 0)
			{
				receivedDataString = Marshal.PtrToStringAuto(receivedData, (int)sizeOfReceivedData);
			}
			callbackEvents.AddEvent(delegate
			{
				gameTransmissionSuccessCallback(receivedDataString, requestTransmitted);
			});
		}

		[MonoPInvokeCallback(typeof(TransmissionFailureDelegate))]
		private static void TransmissionFailureCallback(int errorCode, string requestTransmitted)
		{
			callbackEvents.AddEvent(delegate
			{
				gameTransmissionFailureCallback(errorCode, requestTransmitted);
			});
		}

		private void Update()
		{
			callbackEvents.TryInvokeEvents();
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
