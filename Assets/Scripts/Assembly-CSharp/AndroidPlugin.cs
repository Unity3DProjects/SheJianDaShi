using System.Collections.Generic;
using UnityEngine;

public sealed class AndroidPlugin
{
	private class CallbackListener : AndroidJavaProxy
	{
		private Callback callback;

		public CallbackListener(Callback processor)
			: base(JAVA_CALLBACK_INTERFACE)
		{
			callback = processor;
		}

		public void OnCallback(string result)
		{
			Debug.Log("Universal callback!!!");
			if (callback != null)
			{
				callback(result);
			}
		}
	}

	public delegate void Callback(string message);

	public static string JAVA_CALLBACK_INTERFACE = "com.inventain.android.ICallback";

	public static string REGISTER_CALLBACK_FUNCTION = "RegisterCallback";

	public static string JAVA_INSTANCE = "GetInstance";

	private static Dictionary<string, AndroidJavaObject> objectPool = new Dictionary<string, AndroidJavaObject>();

	private static Dictionary<string, AndroidJavaClass> classPool = new Dictionary<string, AndroidJavaClass>();

	private static readonly AndroidPlugin instance = new AndroidPlugin();

	public static AndroidPlugin Instance
	{
		get
		{
			return instance;
		}
	}

	private AndroidPlugin()
	{
	}

	public void RegisterCallback(string androidClass, Callback processor)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(androidClass);
		androidJavaClass.CallStatic(REGISTER_CALLBACK_FUNCTION, new CallbackListener(processor));
	}

	public void Call(string className, string methodName, params object[] args)
	{
		AndroidJavaObject androidJavaObject = null;
		if (objectPool.ContainsKey(className))
		{
			androidJavaObject = objectPool[className];
		}
		else
		{
			androidJavaObject = CallStatic<AndroidJavaObject>(className, JAVA_INSTANCE, new object[0]);
			objectPool.Add(className, androidJavaObject);
		}
		androidJavaObject.Call(methodName, args);
	}

	public ResultType Call<ResultType>(string className, string methodName, params object[] args)
	{
		AndroidJavaObject androidJavaObject = null;
		if (objectPool.ContainsKey(className))
		{
			androidJavaObject = objectPool[className];
		}
		else
		{
			androidJavaObject = CallStatic<AndroidJavaObject>(className, JAVA_INSTANCE, new object[0]);
			objectPool.Add(className, androidJavaObject);
		}
		return androidJavaObject.Call<ResultType>(methodName, args);
	}

	public void CallStatic(string className, string methodName, params object[] args)
	{
		AndroidJavaClass androidJavaClass = null;
		if (classPool.ContainsKey(className))
		{
			androidJavaClass = classPool[className];
		}
		else
		{
			androidJavaClass = new AndroidJavaClass(className);
			classPool.Add(className, androidJavaClass);
		}
		androidJavaClass.CallStatic(methodName, args);
	}

	public ResultType CallStatic<ResultType>(string className, string methodName, params object[] args)
	{
		AndroidJavaClass androidJavaClass = null;
		if (classPool.ContainsKey(className))
		{
			androidJavaClass = classPool[className];
		}
		else
		{
			androidJavaClass = new AndroidJavaClass(className);
			classPool.Add(className, androidJavaClass);
		}
		return androidJavaClass.CallStatic<ResultType>(methodName, args);
	}
}
