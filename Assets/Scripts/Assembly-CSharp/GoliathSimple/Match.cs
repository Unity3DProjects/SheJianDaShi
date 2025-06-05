using System;

namespace GoliathSimple
{
	public class Match : IDisposable
	{
		private IntPtr Instance;

		public Match()
		{
			Instance = Implementation.MatchCreate();
		}

		~Match()
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
				Implementation.MatchDestroy(Instance);
				Instance = IntPtr.Zero;
			}
		}

		public Result SetMatchId(string value)
		{
			return Implementation.MatchSetMatchId(Instance, value);
		}

		public Result SetDuration(long value)
		{
			return Implementation.MatchSetDuration(Instance, value);
		}

		public Result SetLong(string key, long value)
		{
			return Implementation.MatchSetLong(Instance, key, value);
		}

		public Result SetInt(string key, int value)
		{
			return Implementation.MatchSetInt(Instance, key, value);
		}

		public Result SetDouble(string key, double value)
		{
			return Implementation.MatchSetDouble(Instance, key, value);
		}

		public Result SetBoolean(string key, bool value)
		{
			return Implementation.MatchSetBoolean(Instance, key, value);
		}

		public Result SetString(string key, string value)
		{
			return Implementation.MatchSetString(Instance, key, value);
		}

		public Result SetDate(string key, string value)
		{
			return Implementation.MatchSetDate(Instance, key, value);
		}

		public Result SetDate(string key, int year, int month, int day)
		{
			return Implementation.MatchSetDate(Instance, key, year, month, day);
		}

		public Result SetDecimal(string key, string value)
		{
			return Implementation.MatchSetDecimal(Instance, key, value);
		}

		public Result Post()
		{
			return Implementation.MatchPost(Instance);
		}
	}
}
