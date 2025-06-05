using UnityEngine;

public sealed class LLAndroidJavaSingletone<T>
{
	private static AndroidJavaClass instance;

	private static AndroidJavaClass Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new AndroidJavaClass("java.lang.Object");
			}
			return instance;
		}
	}

	public static void CallStatic(string methodName, params object[] args)
	{
	}

	public static ReturnType CallStatic<ReturnType>(string methodName, params object[] args)
	{
		return default(ReturnType);
	}
}
