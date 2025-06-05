public sealed class LLTapjoyManager
{
	private const string METHOD_INIT = "LLTapjoyManagerInit";

	private const string METHOD_LOG = "LLTapjoyManagerLogEvent";

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLTapjoyManager>.CallStatic("LLTapjoyManagerInit", LLTapjoySettings.AppKey(), LLTapjoySettings.EnableDebug());
	}

	public static void LogEvent(string eventName, string category, string param1, string param2)
	{
		LLAndroidJavaSingletone<LLTapjoyManager>.CallStatic("LLTapjoyManagerLogEvent", eventName, category, param1, param2);
	}

	public static void LogEvent(string eventName, string category, string param1, string param2, int value)
	{
		LLAndroidJavaSingletone<LLTapjoyManager>.CallStatic("LLTapjoyManagerLogEvent", eventName, category, param1, param2, value);
	}
}
