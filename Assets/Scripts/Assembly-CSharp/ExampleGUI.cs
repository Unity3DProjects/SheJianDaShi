using System;
using UnityEngine;
using com.adjust.sdk;

public class ExampleGUI : MonoBehaviour
{
	private int numberOfButtons = 8;

	private bool isEnabled;

	private bool showPopUp;

	private string txtSetEnabled = "Disable SDK";

	private string txtManualLaunch = "Manual Launch";

	private string txtSetOfflineMode = "Turn Offline Mode ON";

	private void OnGUI()
	{
		if (showPopUp)
		{
			GUI.Window(0, new Rect(Screen.width / 2 - 150, Screen.height / 2 - 65, 300f, 130f), showGUI, "Is SDK enabled?");
		}
		if (GUI.Button(new Rect(0f, Screen.height * 0 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), txtManualLaunch) && !string.Equals(txtManualLaunch, "SDK Launched", StringComparison.OrdinalIgnoreCase))
		{
			AdjustConfig adjustConfig = new AdjustConfig("{YourAppToken}", AdjustEnvironment.Sandbox);
			adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
			adjustConfig.setLogDelegate(delegate(string msg)
			{
				Debug.Log(msg);
			});
			adjustConfig.setSendInBackground(true);
			adjustConfig.setLaunchDeferredDeeplink(true);
			adjustConfig.setEventSuccessDelegate(EventSuccessCallback);
			adjustConfig.setEventFailureDelegate(EventFailureCallback);
			adjustConfig.setSessionSuccessDelegate(SessionSuccessCallback);
			adjustConfig.setSessionFailureDelegate(SessionFailureCallback);
			adjustConfig.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
			adjustConfig.setAttributionChangedDelegate(AttributionChangedCallback);
			Adjust.start(adjustConfig);
			isEnabled = true;
			txtManualLaunch = "SDK Launched";
		}
		if (GUI.Button(new Rect(0f, Screen.height / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Simple Event"))
		{
			AdjustEvent adjustEvent = new AdjustEvent("{YourEventToken}");
			Adjust.trackEvent(adjustEvent);
		}
		if (GUI.Button(new Rect(0f, Screen.height * 2 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Revenue Event"))
		{
			AdjustEvent adjustEvent2 = new AdjustEvent("{YourEventToken}");
			adjustEvent2.setRevenue(0.25, "EUR");
			Adjust.trackEvent(adjustEvent2);
		}
		if (GUI.Button(new Rect(0f, Screen.height * 3 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Callback Event"))
		{
			AdjustEvent adjustEvent3 = new AdjustEvent("{YourEventToken}");
			adjustEvent3.addCallbackParameter("key", "value");
			adjustEvent3.addCallbackParameter("foo", "bar");
			Adjust.trackEvent(adjustEvent3);
		}
		if (GUI.Button(new Rect(0f, Screen.height * 4 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Track Partner Event"))
		{
			AdjustEvent adjustEvent4 = new AdjustEvent("{YourEventToken}");
			adjustEvent4.addPartnerParameter("key", "value");
			adjustEvent4.addPartnerParameter("foo", "bar");
			Adjust.trackEvent(adjustEvent4);
		}
		if (GUI.Button(new Rect(0f, Screen.height * 5 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), txtSetOfflineMode))
		{
			if (string.Equals(txtSetOfflineMode, "Turn Offline Mode ON", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setOfflineMode(true);
				txtSetOfflineMode = "Turn Offline Mode OFF";
			}
			else
			{
				Adjust.setOfflineMode(false);
				txtSetOfflineMode = "Turn Offline Mode ON";
			}
		}
		if (GUI.Button(new Rect(0f, Screen.height * 6 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), txtSetEnabled))
		{
			if (string.Equals(txtSetEnabled, "Disable SDK", StringComparison.OrdinalIgnoreCase))
			{
				Adjust.setEnabled(false);
				txtSetEnabled = "Enable SDK";
			}
			else
			{
				Adjust.setEnabled(true);
				txtSetEnabled = "Disable SDK";
			}
		}
		if (GUI.Button(new Rect(0f, Screen.height * 7 / numberOfButtons, Screen.width, Screen.height / numberOfButtons), "Is SDK Enabled?"))
		{
			isEnabled = Adjust.isEnabled();
			showPopUp = true;
		}
	}

	private void showGUI(int windowID)
	{
		if (isEnabled)
		{
			GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is ENABLED!");
		}
		else
		{
			GUI.Label(new Rect(65f, 40f, 200f, 30f), "Adjust SDK is DISABLED!");
		}
		if (GUI.Button(new Rect(90f, 75f, 120f, 40f), "OK"))
		{
			showPopUp = false;
		}
	}

	public void handleGooglePlayId(string adId)
	{
		Debug.Log("Google Play Ad ID = " + adId);
	}

	public void AttributionChangedCallback(AdjustAttribution attributionData)
	{
		Debug.Log("Attribution changed!");
		if (attributionData.trackerName != null)
		{
			Debug.Log("trackerName " + attributionData.trackerName);
		}
		if (attributionData.trackerToken != null)
		{
			Debug.Log("trackerToken " + attributionData.trackerToken);
		}
		if (attributionData.network != null)
		{
			Debug.Log("network " + attributionData.network);
		}
		if (attributionData.campaign != null)
		{
			Debug.Log("campaign " + attributionData.campaign);
		}
		if (attributionData.adgroup != null)
		{
			Debug.Log("adgroup " + attributionData.adgroup);
		}
		if (attributionData.creative != null)
		{
			Debug.Log("creative " + attributionData.creative);
		}
		if (attributionData.clickLabel != null)
		{
			Debug.Log("clickLabel" + attributionData.clickLabel);
		}
	}

	public void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
	{
		Debug.Log("Event tracked successfully!");
		if (eventSuccessData.Message != null)
		{
			Debug.Log("Message: " + eventSuccessData.Message);
		}
		if (eventSuccessData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
		}
		if (eventSuccessData.Adid != null)
		{
			Debug.Log("Adid: " + eventSuccessData.Adid);
		}
		if (eventSuccessData.EventToken != null)
		{
			Debug.Log("EventToken: " + eventSuccessData.EventToken);
		}
		if (eventSuccessData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
		}
	}

	public void EventFailureCallback(AdjustEventFailure eventFailureData)
	{
		Debug.Log("Event tracking failed!");
		if (eventFailureData.Message != null)
		{
			Debug.Log("Message: " + eventFailureData.Message);
		}
		if (eventFailureData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + eventFailureData.Timestamp);
		}
		if (eventFailureData.Adid != null)
		{
			Debug.Log("Adid: " + eventFailureData.Adid);
		}
		if (eventFailureData.EventToken != null)
		{
			Debug.Log("EventToken: " + eventFailureData.EventToken);
		}
		Debug.Log("WillRetry: " + eventFailureData.WillRetry);
		if (eventFailureData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
		}
	}

	public void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
	{
		Debug.Log("Session tracked successfully!");
		if (sessionSuccessData.Message != null)
		{
			Debug.Log("Message: " + sessionSuccessData.Message);
		}
		if (sessionSuccessData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
		}
		if (sessionSuccessData.Adid != null)
		{
			Debug.Log("Adid: " + sessionSuccessData.Adid);
		}
		if (sessionSuccessData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
		}
	}

	public void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
	{
		Debug.Log("Session tracking failed!");
		if (sessionFailureData.Message != null)
		{
			Debug.Log("Message: " + sessionFailureData.Message);
		}
		if (sessionFailureData.Timestamp != null)
		{
			Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
		}
		if (sessionFailureData.Adid != null)
		{
			Debug.Log("Adid: " + sessionFailureData.Adid);
		}
		Debug.Log("WillRetry: " + sessionFailureData.WillRetry);
		if (sessionFailureData.JsonResponse != null)
		{
			Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
		}
	}

	private void DeferredDeeplinkCallback(string deeplinkURL)
	{
		Debug.Log("Deferred deeplink reported!");
		if (deeplinkURL != null)
		{
			Debug.Log("Deeplink URL: " + deeplinkURL);
		}
		else
		{
			Debug.Log("Deeplink URL is null!");
		}
	}
}
