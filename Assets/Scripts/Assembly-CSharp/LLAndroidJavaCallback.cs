using System;
using UnityEngine;

public static class LLAndroidJavaCallback
{
	public sealed class AndroidJavaProxyCallback : AndroidJavaProxy
	{
		private static string LLLIBSET_INTERFACE_CALLBACK = "com.lllibset.LLActivity.ILLLibSetCallback";

		private LLAndroidCallback proxyCallback;

		public AndroidJavaProxyCallback(Delegate callback, float time = 0.01f)
			: base(LLLIBSET_INTERFACE_CALLBACK)
		{
			proxyCallback = ScriptableObject.CreateInstance<LLAndroidCallback>();
			proxyCallback.Setup(callback, time);
		}

		public void OnCallback()
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke();
			}
		}

		public void OnCallback(string message)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(message);
			}
		}

		public void OnCallback(string messageString, bool messageBool)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageString, messageBool);
			}
		}

		public void OnCallback(string messageString, int messageInt)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageString, messageInt);
			}
		}

		public void OnCallback(string messageString1, string messageString2)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageString1, messageString2);
			}
		}

		public void OnCallback(string messageString1, string messageString2, bool messageBool1, bool messageBool2)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageString1, messageString2, messageBool1, messageBool2);
			}
		}

		public void OnCallback(bool messageBool)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageBool);
			}
		}

		public void OnCallback(int messageInt)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageInt);
			}
		}

		public void OnCallback(string messageString1, string messageString2, string messageString3, int messageInt)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageString1, messageString2, messageString3, messageInt);
			}
		}

		public void OnCallback(string messageString1, string messageString2, int messageInt)
		{
			if (proxyCallback != null)
			{
				proxyCallback.DynamicInvoke(messageString1, messageString2, messageInt);
			}
		}

		~AndroidJavaProxyCallback()
		{
			UnityEngine.Object.Destroy(proxyCallback);
		}
	}

	public static AndroidJavaProxy ProxyCallback(Action callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<string> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<string, bool> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<string, int> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<string, string> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<string, string, bool, bool> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<bool> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<int> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<string, string, string, int> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}

	public static AndroidJavaProxy ProxyCallback(Action<string, string, int> callback)
	{
		return new AndroidJavaProxyCallback(callback);
	}
}
