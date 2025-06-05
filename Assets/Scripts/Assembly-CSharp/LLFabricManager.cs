public class LLFabricManager
{
	private const string METHOD_INIT = "LLFabricManagerInit";

	public static void Initialize()
	{
		LLAndroidJavaSingletone<LLFabricManager>.CallStatic("LLFabricManagerInit", LLFabricSettings.EnableDebug());
	}
}
