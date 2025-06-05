public sealed class LLTenjinManager
{
	private const string METHOD_INIT = "LLTenjinManagerInit";

	private const string METHOD_LOG = "LLTenjinManagerLogEvent";

	private const string METHOD_LOG_WITH_PARAM = "LLTenjinManagerLogEventWithParam";

	private const string METHOD_TRANSACTION = "LLTenjinManagerCompleteTransaction";

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLTenjinManager>.CallStatic("LLTenjinManagerInit", LLTenjinSettings.ApiKey());
	}

	public static void LogEvent(string eventName, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			LLAndroidJavaSingletone<LLTenjinManager>.CallStatic("LLTenjinManagerLogEvent", eventName);
		}
		else
		{
			LLAndroidJavaSingletone<LLTenjinManager>.CallStatic("LLTenjinManagerLogEventWithParam", eventName, value);
		}
	}

	public static void LogPurchase(string productName, string currencyCode, int quantity, string price, string transactionId)
	{
	}

	public static void LogPurchaseAndroid(string productName, string currencyCode, int quantity, string price, string purchaseDataJSON, string signature)
	{
		LLAndroidJavaSingletone<LLTenjinManager>.CallStatic("LLTenjinManagerCompleteTransaction", productName, currencyCode, quantity, price, purchaseDataJSON, signature);
	}

	public static void LogRevenue(string productName, string currencyCode, int quantity, string price)
	{
		LLAndroidJavaSingletone<LLTenjinManager>.CallStatic("LLTenjinManagerCompleteTransaction", productName, currencyCode, quantity, price);
	}
}
