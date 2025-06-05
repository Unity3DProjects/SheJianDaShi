using System;

namespace GoliathSimple
{
	public class Session : IDisposable
	{
		private IntPtr Instance;

		public Session()
		{
			Instance = Implementation.SessionCreate();
		}

		~Session()
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
				Implementation.SessionDestroy(Instance);
				Instance = IntPtr.Zero;
			}
		}

		public Result SetLong(string key, long value)
		{
			return Implementation.SessionSetLong(Instance, key, value);
		}

		public Result SetInt(string key, int value)
		{
			return Implementation.SessionSetInt(Instance, key, value);
		}

		public Result SetDouble(string key, double value)
		{
			return Implementation.SessionSetDouble(Instance, key, value);
		}

		public Result SetBoolean(string key, bool value)
		{
			return Implementation.SessionSetBoolean(Instance, key, value);
		}

		public Result SetString(string key, string value)
		{
			return Implementation.SessionSetString(Instance, key, value);
		}

		public Result SetDate(string key, string value)
		{
			return Implementation.SessionSetDate(Instance, key, value);
		}

		public Result SetDate(string key, int year, int month, int day)
		{
			return Implementation.SessionSetDate(Instance, key, year, month, day);
		}

		public Result SetDecimal(string key, string value)
		{
			return Implementation.SessionSetDecimal(Instance, key, value);
		}

		public Result Post()
		{
			return Implementation.SessionPost(Instance);
		}
	}
}
