using System;

namespace GoliathSimple
{
	public class ClientInit : IDisposable
	{
		private IntPtr Instance;

		public ClientInit()
		{
			Instance = Implementation.ClientInitCreate();
		}

		~ClientInit()
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
				Implementation.ClientInitDestroy(Instance);
				Instance = IntPtr.Zero;
			}
		}

		public Result SetDeviceCompromised(bool value)
		{
			return Implementation.ClientInitSetDeviceCompromised(Instance, value);
		}

		public Result SetNotificationsEnabled(bool value)
		{
			return Implementation.ClientInitSetNotificationsEnabled(Instance, value);
		}

		public Result SetPushToken(string value)
		{
			return Implementation.ClientInitSetPushToken(Instance, value);
		}

		public Result SetClientGameVersion(string value)
		{
			return Implementation.ClientInitSetClientGameVersion(Instance, value);
		}

		public Result SetAdvertisingTrackingEnabled(bool value)
		{
			return Implementation.ClientInitSetAdvertisingTrackingEnabled(Instance, value);
		}

		public Result SetFacebookId(string value)
		{
			return Implementation.ClientInitSetFacebookId(Instance, value);
		}

		public Result SetFacebookIdForBusiness(string value)
		{
			return Implementation.ClientInitSetFacebookIdForBusiness(Instance, value);
		}

		public Result SetGooglePlusId(string value)
		{
			return Implementation.ClientInitSetGooglePlusId(Instance, value);
		}

		public Result SetGoogleGamerId(string value)
		{
			return Implementation.ClientInitSetGoogleGamerId(Instance, value);
		}

		public Result SetMiniclipId(string value)
		{
			return Implementation.ClientInitSetMiniclipId(Instance, value);
		}

		public Result SetDateOfBirth(string value)
		{
			return Implementation.ClientInitSetDateOfBirth(Instance, value);
		}

		public Result SetGender(string value)
		{
			return Implementation.ClientInitSetGender(Instance, value);
		}

		public Result SetEmail(string value)
		{
			return Implementation.ClientInitSetEmail(Instance, value);
		}

		public Result SetDisplayName(string value)
		{
			return Implementation.ClientInitSetDisplayName(Instance, value);
		}

		public Result SetNotificationId(string value)
		{
			return Implementation.ClientInitSetNotificationId(Instance, value);
		}

		public Result SetDateOfBirth(int year, int month, int day)
		{
			string dateOfBirth = year.ToString("0000") + "-" + month.ToString("00") + "-" + day.ToString("00");
			return SetDateOfBirth(dateOfBirth);
		}

		public Result SetLong(string key, long value)
		{
			return Implementation.ClientInitSetLong(Instance, key, value);
		}

		public Result SetInt(string key, int value)
		{
			return Implementation.ClientInitSetInt(Instance, key, value);
		}

		public Result SetDouble(string key, double value)
		{
			return Implementation.ClientInitSetDouble(Instance, key, value);
		}

		public Result SetBoolean(string key, bool value)
		{
			return Implementation.ClientInitSetBoolean(Instance, key, value);
		}

		public Result SetString(string key, string value)
		{
			return Implementation.ClientInitSetString(Instance, key, value);
		}

		public Result SetDate(string key, string value)
		{
			return Implementation.ClientInitSetDate(Instance, key, value);
		}

		public Result SetDate(string key, int year, int month, int day)
		{
			return Implementation.ClientInitSetDate(Instance, key, year, month, day);
		}

		public Result SetDecimal(string key, string value)
		{
			return Implementation.ClientInitSetDecimal(Instance, key, value);
		}

		public Result Post()
		{
			return Implementation.ClientInitPost(Instance);
		}
	}
}
