using System;
using System.Runtime.InteropServices;

namespace GoliathSimple
{
	internal class Implementation
	{
		private static Result castIntToResult(int value)
		{
			if (Enum.IsDefined(typeof(Result), value))
			{
				return (Result)value;
			}
			return Result.InvalidResult;
		}

		[DllImport("mcgoliath")]
		private static extern IntPtr goliathSimpleInitCreate();

		public static IntPtr InitCreate()
		{
			return goliathSimpleInitCreate();
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetServerVersion(IntPtr ev, string value);

		public static Result InitSetServerVersion(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInitSetServerVersion(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetLoginType(IntPtr ev, string value);

		public static Result InitSetLoginType(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInitSetLoginType(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetLong(IntPtr ev, string key, long value);

		public static Result InitSetLong(IntPtr ev, string key, long value)
		{
			int value2 = goliathSimpleInitSetLong(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetInt(IntPtr ev, string key, int value);

		public static Result InitSetInt(IntPtr ev, string key, int value)
		{
			int value2 = goliathSimpleInitSetInt(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetDouble(IntPtr ev, string key, double value);

		public static Result InitSetDouble(IntPtr ev, string key, double value)
		{
			int value2 = goliathSimpleInitSetDouble(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetBoolean(IntPtr ev, string key, bool value);

		public static Result InitSetBoolean(IntPtr ev, string key, bool value)
		{
			int value2 = goliathSimpleInitSetBoolean(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetString(IntPtr ev, string key, string value);

		public static Result InitSetString(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleInitSetString(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetDate(IntPtr ev, string key, string value);

		public static Result InitSetDate(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleInitSetDate(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetDateComponents(IntPtr ev, string key, int year, int month, int day);

		public static Result InitSetDate(IntPtr ev, string key, int year, int month, int day)
		{
			int value = goliathSimpleInitSetDateComponents(ev, key, year, month, day);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitSetDecimal(IntPtr ev, string key, string value);

		public static Result InitSetDecimal(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleInitSetDecimal(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInitPost(IntPtr ev);

		public static Result InitPost(IntPtr ev)
		{
			int value = goliathSimpleInitPost(ev);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern void goliathSimpleInitDestroy(IntPtr ev);

		public static void InitDestroy(IntPtr ev)
		{
			goliathSimpleInitDestroy(ev);
		}

		[DllImport("mcgoliath")]
		private static extern IntPtr goliathSimpleClientInitCreate();

		public static IntPtr ClientInitCreate()
		{
			return goliathSimpleClientInitCreate();
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetDeviceCompromised(IntPtr ev, bool value);

		public static Result ClientInitSetDeviceCompromised(IntPtr ev, bool value)
		{
			int value2 = goliathSimpleClientInitSetDeviceCompromised(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetNotificationsEnabled(IntPtr ev, bool value);

		public static Result ClientInitSetNotificationsEnabled(IntPtr ev, bool value)
		{
			int value2 = goliathSimpleClientInitSetNotificationsEnabled(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetPushToken(IntPtr ev, string value);

		public static Result ClientInitSetPushToken(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetPushToken(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetClientGameVersion(IntPtr ev, string value);

		public static Result ClientInitSetClientGameVersion(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetClientGameVersion(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetAdvertisingTrackingEnabled(IntPtr ev, bool value);

		public static Result ClientInitSetAdvertisingTrackingEnabled(IntPtr ev, bool value)
		{
			int value2 = goliathSimpleClientInitSetAdvertisingTrackingEnabled(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetFacebookId(IntPtr ev, string value);

		public static Result ClientInitSetFacebookId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetFacebookId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetFacebookIdForBusiness(IntPtr ev, string value);

		public static Result ClientInitSetFacebookIdForBusiness(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetFacebookIdForBusiness(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetGooglePlusId(IntPtr ev, string value);

		public static Result ClientInitSetGooglePlusId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetGooglePlusId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetGoogleGamerId(IntPtr ev, string value);

		public static Result ClientInitSetGoogleGamerId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetGoogleGamerId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetMiniclipId(IntPtr ev, string value);

		public static Result ClientInitSetMiniclipId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetMiniclipId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetDateOfBirth(IntPtr ev, string value);

		public static Result ClientInitSetDateOfBirth(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetDateOfBirth(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetGender(IntPtr ev, string value);

		public static Result ClientInitSetGender(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetGender(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetEmail(IntPtr ev, string value);

		public static Result ClientInitSetEmail(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetEmail(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetDisplayName(IntPtr ev, string value);

		public static Result ClientInitSetDisplayName(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetDisplayName(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetNotificationId(IntPtr ev, string value);

		public static Result ClientInitSetNotificationId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleClientInitSetNotificationId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetLong(IntPtr ev, string key, long value);

		public static Result ClientInitSetLong(IntPtr ev, string key, long value)
		{
			int value2 = goliathSimpleClientInitSetLong(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetInt(IntPtr ev, string key, int value);

		public static Result ClientInitSetInt(IntPtr ev, string key, int value)
		{
			int value2 = goliathSimpleClientInitSetInt(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetDouble(IntPtr ev, string key, double value);

		public static Result ClientInitSetDouble(IntPtr ev, string key, double value)
		{
			int value2 = goliathSimpleClientInitSetDouble(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetBoolean(IntPtr ev, string key, bool value);

		public static Result ClientInitSetBoolean(IntPtr ev, string key, bool value)
		{
			int value2 = goliathSimpleClientInitSetBoolean(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetString(IntPtr ev, string key, string value);

		public static Result ClientInitSetString(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleClientInitSetString(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetDate(IntPtr ev, string key, string value);

		public static Result ClientInitSetDate(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleClientInitSetDate(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetDateComponents(IntPtr ev, string key, int year, int month, int day);

		public static Result ClientInitSetDate(IntPtr ev, string key, int year, int month, int day)
		{
			int value = goliathSimpleClientInitSetDateComponents(ev, key, year, month, day);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitSetDecimal(IntPtr ev, string key, string value);

		public static Result ClientInitSetDecimal(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleClientInitSetDecimal(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleClientInitPost(IntPtr ev);

		public static Result ClientInitPost(IntPtr ev)
		{
			int value = goliathSimpleClientInitPost(ev);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern void goliathSimpleClientInitDestroy(IntPtr ev);

		public static void ClientInitDestroy(IntPtr ev)
		{
			goliathSimpleClientInitDestroy(ev);
		}

		[DllImport("mcgoliath")]
		private static extern IntPtr goliathSimpleSessionCreate();

		public static IntPtr SessionCreate()
		{
			return goliathSimpleSessionCreate();
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetLong(IntPtr ev, string key, long value);

		public static Result SessionSetLong(IntPtr ev, string key, long value)
		{
			int value2 = goliathSimpleSessionSetLong(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetInt(IntPtr ev, string key, int value);

		public static Result SessionSetInt(IntPtr ev, string key, int value)
		{
			int value2 = goliathSimpleSessionSetInt(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetDouble(IntPtr ev, string key, double value);

		public static Result SessionSetDouble(IntPtr ev, string key, double value)
		{
			int value2 = goliathSimpleSessionSetDouble(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetBoolean(IntPtr ev, string key, bool value);

		public static Result SessionSetBoolean(IntPtr ev, string key, bool value)
		{
			int value2 = goliathSimpleSessionSetBoolean(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetString(IntPtr ev, string key, string value);

		public static Result SessionSetString(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleSessionSetString(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetDate(IntPtr ev, string key, string value);

		public static Result SessionSetDate(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleSessionSetDate(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetDateComponents(IntPtr ev, string key, int year, int month, int day);

		public static Result SessionSetDate(IntPtr ev, string key, int year, int month, int day)
		{
			int value = goliathSimpleSessionSetDateComponents(ev, key, year, month, day);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionSetDecimal(IntPtr ev, string key, string value);

		public static Result SessionSetDecimal(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleSessionSetDecimal(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleSessionPost(IntPtr ev);

		public static Result SessionPost(IntPtr ev)
		{
			int value = goliathSimpleSessionPost(ev);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern void goliathSimpleSessionDestroy(IntPtr ev);

		public static void SessionDestroy(IntPtr ev)
		{
			goliathSimpleSessionDestroy(ev);
		}

		[DllImport("mcgoliath")]
		private static extern IntPtr goliathSimpleInAppPurchaseCreate();

		public static IntPtr InAppPurchaseCreate()
		{
			return goliathSimpleInAppPurchaseCreate();
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetEventId(IntPtr ev, string value);

		public static Result InAppPurchaseSetEventId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetEventId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetPurchaseValue(IntPtr ev, string value);

		public static Result InAppPurchaseSetPurchaseValue(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetPurchaseValue(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetProductId(IntPtr ev, string value);

		public static Result InAppPurchaseSetProductId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetProductId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetProductFamily(IntPtr ev, string value);

		public static Result InAppPurchaseSetProductFamily(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetProductFamily(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetProductSubFamily(IntPtr ev, string value);

		public static Result InAppPurchaseSetProductSubFamily(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetProductSubFamily(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetLocalCurrency(IntPtr ev, string value);

		public static Result InAppPurchaseSetLocalCurrency(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetLocalCurrency(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetLocalPrice(IntPtr ev, string value);

		public static Result InAppPurchaseSetLocalPrice(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetLocalPrice(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetReceipt(IntPtr ev, string value);

		public static Result InAppPurchaseSetReceipt(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetReceipt(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetSignedReceipt(IntPtr ev, string value);

		public static Result InAppPurchaseSetSignedReceipt(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetSignedReceipt(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetTransactionId(IntPtr ev, string value);

		public static Result InAppPurchaseSetTransactionId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetTransactionId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetLong(IntPtr ev, string key, long value);

		public static Result InAppPurchaseSetLong(IntPtr ev, string key, long value)
		{
			int value2 = goliathSimpleInAppPurchaseSetLong(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetInt(IntPtr ev, string key, int value);

		public static Result InAppPurchaseSetInt(IntPtr ev, string key, int value)
		{
			int value2 = goliathSimpleInAppPurchaseSetInt(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetDouble(IntPtr ev, string key, double value);

		public static Result InAppPurchaseSetDouble(IntPtr ev, string key, double value)
		{
			int value2 = goliathSimpleInAppPurchaseSetDouble(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetBoolean(IntPtr ev, string key, bool value);

		public static Result InAppPurchaseSetBoolean(IntPtr ev, string key, bool value)
		{
			int value2 = goliathSimpleInAppPurchaseSetBoolean(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetString(IntPtr ev, string key, string value);

		public static Result InAppPurchaseSetString(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetString(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetDate(IntPtr ev, string key, string value);

		public static Result InAppPurchaseSetDate(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetDate(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetDateComponents(IntPtr ev, string key, int year, int month, int day);

		public static Result InAppPurchaseSetDate(IntPtr ev, string key, int year, int month, int day)
		{
			int value = goliathSimpleInAppPurchaseSetDateComponents(ev, key, year, month, day);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchaseSetDecimal(IntPtr ev, string key, string value);

		public static Result InAppPurchaseSetDecimal(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleInAppPurchaseSetDecimal(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleInAppPurchasePost(IntPtr ev);

		public static Result InAppPurchasePost(IntPtr ev)
		{
			int value = goliathSimpleInAppPurchasePost(ev);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern void goliathSimpleInAppPurchaseDestroy(IntPtr ev);

		public static void InAppPurchaseDestroy(IntPtr ev)
		{
			goliathSimpleInAppPurchaseDestroy(ev);
		}

		[DllImport("mcgoliath")]
		private static extern IntPtr goliathSimpleVirtualCurrencyPurchaseCreate();

		public static IntPtr VirtualCurrencyPurchaseCreate()
		{
			return goliathSimpleVirtualCurrencyPurchaseCreate();
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetEventId(IntPtr ev, string value);

		public static Result VirtualCurrencyPurchaseSetEventId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetEventId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetCurrencyName(IntPtr ev, string value);

		public static Result VirtualCurrencyPurchaseSetCurrencyName(IntPtr ev, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetCurrencyName(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetCurrencyAmount(IntPtr ev, string value);

		public static Result VirtualCurrencyPurchaseSetCurrencyAmount(IntPtr ev, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetCurrencyAmount(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetProductId(IntPtr ev, string value);

		public static Result VirtualCurrencyPurchaseSetProductId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetProductId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetProductFamily(IntPtr ev, string value);

		public static Result VirtualCurrencyPurchaseSetProductFamily(IntPtr ev, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetProductFamily(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetProductSubFamily(IntPtr ev, string value);

		public static Result VirtualCurrencyPurchaseSetProductSubFamily(IntPtr ev, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetProductSubFamily(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetLong(IntPtr ev, string key, long value);

		public static Result VirtualCurrencyPurchaseSetLong(IntPtr ev, string key, long value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetLong(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetInt(IntPtr ev, string key, int value);

		public static Result VirtualCurrencyPurchaseSetInt(IntPtr ev, string key, int value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetInt(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetDouble(IntPtr ev, string key, double value);

		public static Result VirtualCurrencyPurchaseSetDouble(IntPtr ev, string key, double value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetDouble(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetBoolean(IntPtr ev, string key, bool value);

		public static Result VirtualCurrencyPurchaseSetBoolean(IntPtr ev, string key, bool value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetBoolean(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetString(IntPtr ev, string key, string value);

		public static Result VirtualCurrencyPurchaseSetString(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetString(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetDate(IntPtr ev, string key, string value);

		public static Result VirtualCurrencyPurchaseSetDate(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetDate(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetDateComponents(IntPtr ev, string key, int year, int month, int day);

		public static Result VirtualCurrencyPurchaseSetDate(IntPtr ev, string key, int year, int month, int day)
		{
			int value = goliathSimpleVirtualCurrencyPurchaseSetDateComponents(ev, key, year, month, day);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchaseSetDecimal(IntPtr ev, string key, string value);

		public static Result VirtualCurrencyPurchaseSetDecimal(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleVirtualCurrencyPurchaseSetDecimal(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleVirtualCurrencyPurchasePost(IntPtr ev);

		public static Result VirtualCurrencyPurchasePost(IntPtr ev)
		{
			int value = goliathSimpleVirtualCurrencyPurchasePost(ev);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern void goliathSimpleVirtualCurrencyPurchaseDestroy(IntPtr ev);

		public static void VirtualCurrencyPurchaseDestroy(IntPtr ev)
		{
			goliathSimpleVirtualCurrencyPurchaseDestroy(ev);
		}

		[DllImport("mcgoliath")]
		private static extern IntPtr goliathSimpleMatchCreate();

		public static IntPtr MatchCreate()
		{
			return goliathSimpleMatchCreate();
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetMatchId(IntPtr ev, string value);

		public static Result MatchSetMatchId(IntPtr ev, string value)
		{
			int value2 = goliathSimpleMatchSetMatchId(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetDuration(IntPtr ev, long value);

		public static Result MatchSetDuration(IntPtr ev, long value)
		{
			int value2 = goliathSimpleMatchSetDuration(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetLong(IntPtr ev, string key, long value);

		public static Result MatchSetLong(IntPtr ev, string key, long value)
		{
			int value2 = goliathSimpleMatchSetLong(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetInt(IntPtr ev, string key, int value);

		public static Result MatchSetInt(IntPtr ev, string key, int value)
		{
			int value2 = goliathSimpleMatchSetInt(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetDouble(IntPtr ev, string key, double value);

		public static Result MatchSetDouble(IntPtr ev, string key, double value)
		{
			int value2 = goliathSimpleMatchSetDouble(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetBoolean(IntPtr ev, string key, bool value);

		public static Result MatchSetBoolean(IntPtr ev, string key, bool value)
		{
			int value2 = goliathSimpleMatchSetBoolean(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetString(IntPtr ev, string key, string value);

		public static Result MatchSetString(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleMatchSetString(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetDate(IntPtr ev, string key, string value);

		public static Result MatchSetDate(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleMatchSetDate(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetDateComponents(IntPtr ev, string key, int year, int month, int day);

		public static Result MatchSetDate(IntPtr ev, string key, int year, int month, int day)
		{
			int value = goliathSimpleMatchSetDateComponents(ev, key, year, month, day);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchSetDecimal(IntPtr ev, string key, string value);

		public static Result MatchSetDecimal(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleMatchSetDecimal(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleMatchPost(IntPtr ev);

		public static Result MatchPost(IntPtr ev)
		{
			int value = goliathSimpleMatchPost(ev);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern void goliathSimpleMatchDestroy(IntPtr ev);

		public static void MatchDestroy(IntPtr ev)
		{
			goliathSimpleMatchDestroy(ev);
		}

		[DllImport("mcgoliath")]
		private static extern IntPtr goliathSimpleConfigUpdateCreate();

		public static IntPtr ConfigUpdateCreate()
		{
			return goliathSimpleConfigUpdateCreate();
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetNewVersion(IntPtr ev, string value);

		public static Result ConfigUpdateSetNewVersion(IntPtr ev, string value)
		{
			int value2 = goliathSimpleConfigUpdateSetNewVersion(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetOldVersion(IntPtr ev, string value);

		public static Result ConfigUpdateSetOldVersion(IntPtr ev, string value)
		{
			int value2 = goliathSimpleConfigUpdateSetOldVersion(ev, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetLong(IntPtr ev, string key, long value);

		public static Result ConfigUpdateSetLong(IntPtr ev, string key, long value)
		{
			int value2 = goliathSimpleConfigUpdateSetLong(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetInt(IntPtr ev, string key, int value);

		public static Result ConfigUpdateSetInt(IntPtr ev, string key, int value)
		{
			int value2 = goliathSimpleConfigUpdateSetInt(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetDouble(IntPtr ev, string key, double value);

		public static Result ConfigUpdateSetDouble(IntPtr ev, string key, double value)
		{
			int value2 = goliathSimpleConfigUpdateSetDouble(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetBoolean(IntPtr ev, string key, bool value);

		public static Result ConfigUpdateSetBoolean(IntPtr ev, string key, bool value)
		{
			int value2 = goliathSimpleConfigUpdateSetBoolean(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetString(IntPtr ev, string key, string value);

		public static Result ConfigUpdateSetString(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleConfigUpdateSetString(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetDate(IntPtr ev, string key, string value);

		public static Result ConfigUpdateSetDate(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleConfigUpdateSetDate(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetDateComponents(IntPtr ev, string key, int year, int month, int day);

		public static Result ConfigUpdateSetDate(IntPtr ev, string key, int year, int month, int day)
		{
			int value = goliathSimpleConfigUpdateSetDateComponents(ev, key, year, month, day);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdateSetDecimal(IntPtr ev, string key, string value);

		public static Result ConfigUpdateSetDecimal(IntPtr ev, string key, string value)
		{
			int value2 = goliathSimpleConfigUpdateSetDecimal(ev, key, value);
			return castIntToResult(value2);
		}

		[DllImport("mcgoliath")]
		private static extern int goliathSimpleConfigUpdatePost(IntPtr ev);

		public static Result ConfigUpdatePost(IntPtr ev)
		{
			int value = goliathSimpleConfigUpdatePost(ev);
			return castIntToResult(value);
		}

		[DllImport("mcgoliath")]
		private static extern void goliathSimpleConfigUpdateDestroy(IntPtr ev);

		public static void ConfigUpdateDestroy(IntPtr ev)
		{
			goliathSimpleConfigUpdateDestroy(ev);
		}
	}
}
