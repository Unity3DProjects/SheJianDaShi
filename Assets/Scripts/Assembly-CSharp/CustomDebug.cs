using UnityEngine;

public sealed class CustomDebug
{
	public static readonly string DEFINE_DEBUG = "DEBUG_TARGET";

	public static bool Enable
	{
		get
		{
			// return Debug.isDebugBuild;
			return false;
		}
	}

	public static void Log(object message)
	{
		if (Enable)
		{
			Debug.Log(message);
		}
	}

	public static void Log(object message, Object context)
	{
		if (Enable)
		{
			Debug.Log(message, context);
		}
	}

	public static void LogWarning(object message)
	{
		if (Enable)
		{
			Debug.LogWarning(message);
		}
	}

	public static void LogWarning(object message, Object context)
	{
		if (Enable)
		{
			Debug.LogWarning(message, context);
		}
	}

	public static void LogError(object message)
	{
		if (Enable)
		{
			Debug.LogError(message);
		}
	}

	public static void LogError(object message, Object context)
	{
		if (Enable)
		{
			Debug.LogError(message, context);
		}
	}

	public static void LogFormat(string format, params object[] args)
	{
		if (Enable)
		{
			Debug.LogFormat(format, args);
		}
	}
}
