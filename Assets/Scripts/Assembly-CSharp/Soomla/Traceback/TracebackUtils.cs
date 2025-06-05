using System;
using UnityEngine;

namespace Soomla.Traceback
{
	public static class TracebackUtils
	{
		private static bool isDebugBuild;

		private static bool isDebugBuildSet;

		public static void LogDebug(string tag, string message)
		{
			if (!isDebugBuildSet)
			{
				try
				{
					isDebugBuild = Debug.isDebugBuild;
				}
				catch (Exception ex)
				{
					isDebugBuild = true;
					Debug.Log(string.Format("{0} {1}", tag, ex.Message));
				}
				isDebugBuildSet = true;
			}
			if (isDebugBuild)
			{
				Debug.Log(string.Format("{0} {1}", tag, message));
			}
		}

		public static void LogError(string tag, string message)
		{
			Debug.LogError(string.Format("{0} {1}", tag, message));
		}

		public static void LogWarning(string tag, string message)
		{
			Debug.LogWarning(string.Format("{0} {1}", tag, message));
		}

		public static string GetClassName(object target)
		{
			return target.GetType().Name;
		}
	}
}
