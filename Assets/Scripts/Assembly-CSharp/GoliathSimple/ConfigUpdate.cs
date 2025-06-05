using System;

namespace GoliathSimple
{
	public class ConfigUpdate : IDisposable
	{
		private IntPtr Instance;

		public ConfigUpdate()
		{
			Instance = Implementation.ConfigUpdateCreate();
		}

		~ConfigUpdate()
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
				Implementation.ConfigUpdateDestroy(Instance);
				Instance = IntPtr.Zero;
			}
		}

		public Result SetNewVersion(string value)
		{
			return Implementation.ConfigUpdateSetNewVersion(Instance, value);
		}

		public Result SetOldVersion(string value)
		{
			return Implementation.ConfigUpdateSetOldVersion(Instance, value);
		}

		public Result SetLong(string key, long value)
		{
			return Implementation.ConfigUpdateSetLong(Instance, key, value);
		}

		public Result SetInt(string key, int value)
		{
			return Implementation.ConfigUpdateSetInt(Instance, key, value);
		}

		public Result SetDouble(string key, double value)
		{
			return Implementation.ConfigUpdateSetDouble(Instance, key, value);
		}

		public Result SetBoolean(string key, bool value)
		{
			return Implementation.ConfigUpdateSetBoolean(Instance, key, value);
		}

		public Result SetString(string key, string value)
		{
			return Implementation.ConfigUpdateSetString(Instance, key, value);
		}

		public Result SetDate(string key, string value)
		{
			return Implementation.ConfigUpdateSetDate(Instance, key, value);
		}

		public Result SetDate(string key, int year, int month, int day)
		{
			return Implementation.ConfigUpdateSetDate(Instance, key, year, month, day);
		}

		public Result SetDecimal(string key, string value)
		{
			return Implementation.ConfigUpdateSetDecimal(Instance, key, value);
		}

		public Result Post()
		{
			return Implementation.ConfigUpdatePost(Instance);
		}
	}
}
