using System.Collections.Generic;

public sealed class LLAppsflyerManager
{
	private const string METHOD_INIT = "LLInit";

	private const string METHOD_LOG_RICH_EVENT = "LLLogRichEvent";

	private const string METHOD_LOG_PURCHASE = "LLLogPurchase";

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLAppsflyerManager>.CallStatic("LLInit", LLAppsflyerSettings.DevKey(), LLAppsflyerSettings.EnableDebug());
	}

	public static void LogEvent(string eventName, string value = null)
	{
		if (value == null)
		{
			value = string.Empty;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("af_param_1", value);
		LogRichEvent(eventName, dictionary);
	}

	public static void LogRichEvent(string eventName, Dictionary<string, string> parameters = null)
	{
		int num = 0;
		string[] array = null;
		string[] array2 = null;
		if (parameters != null)
		{
			num = parameters.Count;
			array = new string[num];
			parameters.Keys.CopyTo(array, 0);
			array2 = new string[num];
			parameters.Values.CopyTo(array2, 0);
		}
		LLAndroidJavaSingletone<LLAppsflyerManager>.CallStatic("LLLogRichEvent", eventName, num, array, array2);
	}

	public static void LogPurchase(string productName, string currencyCode, string price, string transactionId)
	{
	}

	public static void LogPurchaseAndroid(string publicKey, string signature, string purchaseDataJSON, string currencyCode, string price)
	{
		LLAndroidJavaSingletone<LLAppsflyerManager>.CallStatic("LLLogPurchase", publicKey, signature, purchaseDataJSON, currencyCode, price);
	}
}
