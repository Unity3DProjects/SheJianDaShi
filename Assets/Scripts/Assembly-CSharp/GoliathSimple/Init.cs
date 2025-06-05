using System;

namespace GoliathSimple
{
	public class Init : IDisposable
	{
		private IntPtr Instance;

		public Init()
		{
			Instance = Implementation.InitCreate();
		}

		~Init()
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
				Implementation.InitDestroy(Instance);
				Instance = IntPtr.Zero;
			}
		}

		public Result SetServerVersion(string value)
		{
			return Implementation.InitSetServerVersion(Instance, value);
		}

		public Result SetLoginType(string value)
		{
			return Implementation.InitSetLoginType(Instance, value);
		}

		public Result SetLong(string key, long value)
		{
			return Implementation.InitSetLong(Instance, key, value);
		}

		public Result SetInt(string key, int value)
		{
			return Implementation.InitSetInt(Instance, key, value);
		}

		public Result SetDouble(string key, double value)
		{
			return Implementation.InitSetDouble(Instance, key, value);
		}

		public Result SetBoolean(string key, bool value)
		{
			return Implementation.InitSetBoolean(Instance, key, value);
		}

		public Result SetString(string key, string value)
		{
			return Implementation.InitSetString(Instance, key, value);
		}

		public Result SetDate(string key, string value)
		{
			return Implementation.InitSetDate(Instance, key, value);
		}

		public Result SetDate(string key, int year, int month, int day)
		{
			return Implementation.InitSetDate(Instance, key, year, month, day);
		}

		public Result SetDecimal(string key, string value)
		{
			return Implementation.InitSetDecimal(Instance, key, value);
		}

		public Result Post()
		{
			return Implementation.InitPost(Instance);
		}
	}
}
