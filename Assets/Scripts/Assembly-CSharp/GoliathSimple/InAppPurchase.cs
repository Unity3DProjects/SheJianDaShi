using System;

namespace GoliathSimple
{
	public class InAppPurchase : IDisposable
	{
		private IntPtr Instance;

		public InAppPurchase()
		{
			Instance = Implementation.InAppPurchaseCreate();
		}

		~InAppPurchase()
		{
			DestroyInstance();
		}

		public void Dispose()
		{
			DestroyInstance();
			GC.SuppressFinalize(this);
		}

		private void DestroyInstance()
		{
			if (Instance != IntPtr.Zero)
			{
				Implementation.InAppPurchaseDestroy(Instance);
				Instance = IntPtr.Zero;
			}
		}

		public Result SetEventId(string value)
		{
			return Implementation.InAppPurchaseSetEventId(Instance, value);
		}

		public Result SetPurchaseValue(string value)
		{
			return Implementation.InAppPurchaseSetPurchaseValue(Instance, value);
		}

		public Result SetProductId(string value)
		{
			return Implementation.InAppPurchaseSetProductId(Instance, value);
		}

		public Result SetProductFamily(string value)
		{
			return Implementation.InAppPurchaseSetProductFamily(Instance, value);
		}

		public Result SetProductSubFamily(string value)
		{
			return Implementation.InAppPurchaseSetProductSubFamily(Instance, value);
		}

		public Result SetLocalCurrency(string value)
		{
			return Implementation.InAppPurchaseSetLocalCurrency(Instance, value);
		}

		public Result SetLocalPrice(string value)
		{
			return Implementation.InAppPurchaseSetLocalPrice(Instance, value);
		}

		public Result SetReceipt(string value)
		{
			return Implementation.InAppPurchaseSetReceipt(Instance, value);
		}

		public Result SetSignedReceipt(string value)
		{
			return Implementation.InAppPurchaseSetSignedReceipt(Instance, value);
		}

		public Result SetTransactionId(string value)
		{
			return Implementation.InAppPurchaseSetTransactionId(Instance, value);
		}

		public Result SetLong(string key, long value)
		{
			return Implementation.InAppPurchaseSetLong(Instance, key, value);
		}

		public Result SetInt(string key, int value)
		{
			return Implementation.InAppPurchaseSetInt(Instance, key, value);
		}

		public Result SetDouble(string key, double value)
		{
			return Implementation.InAppPurchaseSetDouble(Instance, key, value);
		}

		public Result SetBoolean(string key, bool value)
		{
			return Implementation.InAppPurchaseSetBoolean(Instance, key, value);
		}

		public Result SetString(string key, string value)
		{
			return Implementation.InAppPurchaseSetString(Instance, key, value);
		}

		public Result SetDate(string key, string value)
		{
			return Implementation.InAppPurchaseSetDate(Instance, key, value);
		}

		public Result SetDate(string key, int year, int month, int day)
		{
			return Implementation.InAppPurchaseSetDate(Instance, key, year, month, day);
		}

		public Result SetDecimal(string key, string value)
		{
			return Implementation.InAppPurchaseSetDecimal(Instance, key, value);
		}

		public Result Post()
		{
			return Implementation.InAppPurchasePost(Instance);
		}
	}
}
