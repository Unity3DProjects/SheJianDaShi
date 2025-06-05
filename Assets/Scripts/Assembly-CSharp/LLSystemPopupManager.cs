using System;
using System.Collections.Generic;
using AOT;

public static class LLSystemPopupManager
{
	private delegate void LLSystemPopupManagerDelegate(string str);

	private static bool initialized = false;

	private static Dictionary<string, Action> sendersCallback = new Dictionary<string, Action>();

	public static void ShowPopUp(string title, string message, string buttonTitle, Action callback = null)
	{
		Initialize();
		title = (string.IsNullOrEmpty(title) ? string.Empty : LocalisationManager.LocalizedStringOrSource(title));
		message = (string.IsNullOrEmpty(message) ? string.Empty : LocalisationManager.LocalizedStringOrSource(message));
		buttonTitle = (string.IsNullOrEmpty(buttonTitle) ? string.Empty : LocalisationManager.LocalizedStringOrSource(buttonTitle));
		string empty = string.Empty;
		if (callback != null)
		{
			empty = Guid.NewGuid().ToString();
			sendersCallback.Add(empty, callback);
		}
	}

	private static void Initialize()
	{
		if (!initialized)
		{
			initialized = true;
		}
	}

	[MonoPInvokeCallback(typeof(LLSystemPopupManagerDelegate))]
	private static void PopUpManagerCallback(string callbackName)
	{
		if (!string.IsNullOrEmpty(callbackName) && sendersCallback.ContainsKey(callbackName))
		{
			Action action = sendersCallback[callbackName];
			if (action != null)
			{
				action();
			}
			sendersCallback.Remove(callbackName);
		}
	}
}
