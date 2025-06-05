using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mit_ServicesUtils;
using UnityEngine;

namespace Mit_Goliath_Native.Implementations
{
	public class UnityLogsGoliathImplementation : MonoBehaviour, GoliathImplementation
	{
		private const bool LOGS_ARE_ENABLED = false;

		private static Action gameStartCallback;

		private static Action gameStopCallback;

		private static Action gameSendSuccessCallback;

		private static Action gameSendErrorCallback;

		private static Action<string, string> gameTransmissionSuccessCallback;

		private static Action<int, string> gameTransmissionFailureCallback;

		private CallbackEvents callbackEvents = new CallbackEvents();

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
			gameSendSuccessCallback = callback;
		}

		public void SetSendErrorCallback(Action callback)
		{
			gameSendErrorCallback = callback;
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
		}

		public string GetAppVersion()
		{
			return string.Empty;
		}

		public void SetAppVersion(string version)
		{
		}

		public string GetUserId()
		{
			return string.Empty;
		}

		public void SetUserId(string userId)
		{
		}

		public string GetSessionId()
		{
			return string.Empty;
		}

		public void SetSessionId(string sessionId)
		{
		}

		public string GetHost()
		{
			return string.Empty;
		}

		public void SetHost(string host)
		{
		}

		public string GetGameId()
		{
			return string.Empty;
		}

		public void SetGameId(string gameId)
		{
		}

		public string GetApiKey()
		{
			return string.Empty;
		}

		public void SetApiKey(string apiKey)
		{
		}

		public string GetHmacKey()
		{
			return string.Empty;
		}

		public void SetHmacKey(string hmacKey)
		{
		}

		public string GetPathForSavedFile()
		{
			return string.Empty;
		}

		public void SetPathForSavedFile(string path)
		{
		}

		public int GetMaxNumberOfEventsPerPackage()
		{
			return -1;
		}

		public void SetMaxNumberOfEventsPerPackage(int eventsNumber)
		{
		}

		public int GetTransmissionInterval()
		{
			return -1;
		}

		public void SetTransmissionInterval(int interval)
		{
		}

		public int GetSerializationInterval()
		{
			return -1;
		}

		public void SetSerializationInterval(int interval)
		{
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
			return -1;
		}

		public int PostClientInitEvent(int pushNotificationsEnabled = -1, string pushToken = "", string facebookID = "", string facebookIDforBusiness = "", string googlePlusID = "", string googleGamerID = "", string miniclipID = "", string dob = "", string gender = "", int sDeviceCompromised = -1, string userID = "", string sessionID = "")
		{
			return -1;
		}

		public int PostConfigurationUpdateEvent(string latestVersion, string previousVersion = "")
		{
			return -1;
		}

		public Dictionary<string, string> GetGlobalParameters()
		{
			return null;
		}

		public void SetGlobalParameters(Dictionary<string, object> globalParameters)
		{
		}

		public void AddGlobalParameter(string key, string value)
		{
		}

		public void GoliathStart()
		{
			StartCallback();
		}

		public void GoliathStop()
		{
			StopCallback();
		}

		private void StartCallback()
		{
			callbackEvents.AddEvent(gameStartCallback);
		}

		private void StopCallback()
		{
			callbackEvents.AddEvent(gameStopCallback);
		}

		private void TransmissionSuccessCallback(IntPtr receivedData, uint sizeOfReceivedData, string requestTransmitted)
		{
			callbackEvents.AddEvent(delegate
			{
				gameTransmissionSuccessCallback("_receivedDataString_", requestTransmitted);
			});
		}

		private void TransmissionFailureCallback(int errorCode, string requestTransmitted)
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
