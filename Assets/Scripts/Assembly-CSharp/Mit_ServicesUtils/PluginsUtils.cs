namespace Mit_ServicesUtils
{
	public static class PluginsUtils
	{
		public static bool IsDevelopmentBuild()
		{
			return CustomDebug.Enable;
		}

		public static string GetUserId()
		{
			return "userId";
		}
	}
}
