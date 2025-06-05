using System;

namespace GoliathSimple
{
	public class VirtualCurrencyPurchase : IDisposable
	{
		private IntPtr Instance;

		public VirtualCurrencyPurchase()
		{
			Instance = Implementation.VirtualCurrencyPurchaseCreate();
		}

		~VirtualCurrencyPurchase()
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
				Implementation.VirtualCurrencyPurchaseDestroy(Instance);
				Instance = IntPtr.Zero;
			}
		}

		public Result SetEventId(string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetEventId(Instance, value);
		}

		public Result SetCurrencyName(string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetCurrencyName(Instance, value);
		}

		public Result SetCurrencyAmount(string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetCurrencyAmount(Instance, value);
		}

		public Result SetProductId(string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetProductId(Instance, value);
		}

		public Result SetProductFamily(string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetProductFamily(Instance, value);
		}

		public Result SetProductSubFamily(string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetProductSubFamily(Instance, value);
		}

		public Result SetLong(string key, long value)
		{
			return Implementation.VirtualCurrencyPurchaseSetLong(Instance, key, value);
		}

		public Result SetInt(string key, int value)
		{
			return Implementation.VirtualCurrencyPurchaseSetInt(Instance, key, value);
		}

		public Result SetDouble(string key, double value)
		{
			return Implementation.VirtualCurrencyPurchaseSetDouble(Instance, key, value);
		}

		public Result SetBoolean(string key, bool value)
		{
			return Implementation.VirtualCurrencyPurchaseSetBoolean(Instance, key, value);
		}

		public Result SetString(string key, string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetString(Instance, key, value);
		}

		public Result SetDate(string key, string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetDate(Instance, key, value);
		}

		public Result SetDate(string key, int year, int month, int day)
		{
			return Implementation.VirtualCurrencyPurchaseSetDate(Instance, key, year, month, day);
		}

		public Result SetDecimal(string key, string value)
		{
			return Implementation.VirtualCurrencyPurchaseSetDecimal(Instance, key, value);
		}

		public Result Post()
		{
			return Implementation.VirtualCurrencyPurchasePost(Instance);
		}
	}
}
