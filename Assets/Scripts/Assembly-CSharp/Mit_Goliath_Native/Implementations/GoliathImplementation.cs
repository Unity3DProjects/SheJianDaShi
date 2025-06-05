using System;
using System.Collections.Generic;

namespace Mit_Goliath_Native.Implementations
{
	public interface GoliathImplementation
	{
		void SetOnStartCallback(Action callback);

		void SetOnStopCallback(Action callback);

		void SetSendSuccessCallback(Action callback);

		void SetSendErrorCallback(Action callback);

		void SetTransmissionSuccessCallback(Action<string, string> callback);

		void SetTransmissionFailureCallback(Action<int, string> callback);

		void Setup(string gameId, string apiKey, string version, string endpointUrl, string hmacKey);

		string GetAppVersion();

		void SetAppVersion(string version);

		string GetUserId();

		void SetUserId(string userId);

		string GetSessionId();

		void SetSessionId(string sessionId);

		string GetHost();

		void SetHost(string host);

		string GetGameId();

		void SetGameId(string gameId);

		string GetApiKey();

		void SetApiKey(string apiKey);

		string GetHmacKey();

		void SetHmacKey(string hmacKey);

		string GetPathForSavedFile();

		void SetPathForSavedFile(string path);

		int GetMaxNumberOfEventsPerPackage();

		void SetMaxNumberOfEventsPerPackage(int eventsNumber);

		int GetTransmissionInterval();

		void SetTransmissionInterval(int interval);

		int GetSerializationInterval();

		void SetSerializationInterval(int interval);

		void SetGameName(string gameName);

		void SetEnvironment(string environment);

		void SetSender(string sender);

		int PostEvent(string eventType, Dictionary<string, object> eventParameters);

		int PostClientInitEvent(int pushNotificationsEnabled = -1, string pushToken = "", string facebookID = "", string facebookIDforBusiness = "", string googlePlusID = "", string googleGamerID = "", string miniclipID = "", string dob = "", string gender = "", int sDeviceCompromised = -1, string userID = "", string sessionID = "");

		int PostConfigurationUpdateEvent(string latestVersion, string previousVersion = "");

		Dictionary<string, string> GetGlobalParameters();

		void SetGlobalParameters(Dictionary<string, object> globalParameters);

		void AddGlobalParameter(string key, string value);

		void GoliathStart();

		void GoliathStop();
	}
}
