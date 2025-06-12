using System;
using System.Collections.Generic;
using Soomla.Traceback.Singletons;
using UnityEngine;

namespace Soomla.Traceback
{
	public class SoomlaTraceback : CodeGeneratedSingleton
	{
		private const string TAG = "SOOMLA Traceback";

		private const bool DEBUG = false;

		protected override bool DontDestroySingleton
		{
			get
			{
				return true;
			}
		}

		protected override void InitAfterRegisteringAsSingleInstance()
		{
			base.InitAfterRegisteringAsSingleInstance();
		}

		public static void Initialize(string appKey)
		{
			Initialize(appKey, false);
		}

		[Obsolete("Initialize(string appKey, bool testMode) is deprecated, please use Initialize(string appKey, SoomlaConfig soomlaConfig) instead.")]
		public static void Initialize(string appKey, bool testMode)
		{
			Initialize(appKey, testMode, true);
		}

		[Obsolete("Initialize(string appKey, bool testMode, bool validateVersions) is deprecated, please use Initialize(string appKey, SoomlaConfig soomlaConfig) instead.")]
		public static void Initialize(string appKey, bool testMode, bool validateVersions)
		{
			Initialize(appKey, null, false, testMode, validateVersions, true, true);
		}

		[Obsolete("InitializeWithUserId(string appKey, string userId) is deprecated, please use InitializeWithUserId(string appKey, SoomlaConfig soomlaConfig) instead.")]
		public static void InitializeWithUserId(string appKey, string userId)
		{
			InitializeWithUserId(appKey, userId, false);
		}

		[Obsolete("InitializeWithUserId(string appKey, string userId, bool testMode) is deprecated, please use InitializeWithUserId(string appKey, SoomlaConfig soomlaConfig) instead.")]
		public static void InitializeWithUserId(string appKey, string userId, bool testMode)
		{
			InitializeWithUserId(appKey, userId, testMode, true);
		}

		[Obsolete("InitializeWithUserId(string appKey, string userId, bool testMode) is deprecated, please use InitializeWithUserId(string appKey, SoomlaConfig soomlaConfig, bool validateVersions) instead.")]
		public static void InitializeWithUserId(string appKey, string userId, bool testMode, bool validateVersions)
		{
			Initialize(appKey, userId, true, testMode, validateVersions, true, true);
		}

		public static void Initialize(string appKey, SoomlaConfig soomlaConfig)
		{
			Initialize(appKey, soomlaConfig.UserId, soomlaConfig.UserIdSet, soomlaConfig.TestMode, soomlaConfig.ValidateVersions, soomlaConfig.CollectAdvertisingId, soomlaConfig.SendAttributionData);
		}

		private static void Initialize(string appKey, string userId, bool userIdSet, bool testMode, bool validateVersions, bool collectAdvertisingId, bool sendAttributionData)
		{
			// UnitySingleton.GetSynchronousCodeGeneratedInstance<SoomlaTraceback>();
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.soomla.traceback.LogUtils"))
			// {
			// 	androidJavaClass.SetStatic("logDebug", false);
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
			// TracebackUtils.LogDebug("SOOMLA Traceback", "SOOMLA/UNITY Initializing Traceback");
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.soomla.traceback.unity.SoomlaTraceback"))
			// {
			// 	AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			// 	AndroidJavaObject androidJavaObject = androidJavaClass3.GetStatic<AndroidJavaObject>("currentActivity");
			// 	androidJavaClass2.CallStatic("initialize", androidJavaObject, appKey, userId, userIdSet, testMode, validateVersions, collectAdvertisingId, sendAttributionData);
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		public static void ChangeUserId(string userId)
		{
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.soomla.traceback.unity.SoomlaTraceback"))
			// {
			// 	androidJavaClass.CallStatic("changeUserId", userId);
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		public static void AddExtraUserId(string userId)
		{
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.soomla.traceback.unity.SoomlaTraceback"))
			// {
			// 	androidJavaClass.CallStatic("addExtraUserId", userId);
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		public static void SetUserConsent(bool userConsent)
		{
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.soomla.traceback.unity.SoomlaTraceback"))
			// {
			// 	androidJavaClass.CallStatic("setUserConsent", userConsent);
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		public static void AddTags(List<string> tags)
		{
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.soomla.traceback.unity.SoomlaTraceback"))
			// {
			// 	androidJavaClass.CallStatic("addTags", jsonStringFromList(tags));
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		public static void RemoveTags(List<string> tags)
		{
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.soomla.traceback.unity.SoomlaTraceback"))
			// {
			// 	androidJavaClass.CallStatic("removeTags", jsonStringFromList(tags));
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		public static void OnInAppPurchaseCompleted(string itemId, double amount, string currency)
		{
			// AndroidJNI.PushLocalFrame(100);
			// using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.soomla.traceback.unity.SoomlaTraceback"))
			// {
			// 	androidJavaClass.CallStatic("onInAppPurchaseCompleted", itemId, amount, currency);
			// }
			// AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		private static string jsonStringFromList(List<string> list)
		{
			// if (list == null || list.Count == 0)
			// {
				return string.Empty;
			// }
			// // JSONObject jSONObject = new JSONObject();
			// foreach (string item in list)
			// {
			// 	jSONObject.Add(item);
			// }
			// return jSONObject.ToString();
		}
	}
}
