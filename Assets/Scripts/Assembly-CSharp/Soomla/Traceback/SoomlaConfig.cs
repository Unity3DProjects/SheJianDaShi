namespace Soomla.Traceback
{
	public class SoomlaConfig
	{
		private string userId;

		private bool userIdSet;

		private bool testMode;

		private bool collectAdvertisingId;

		private bool validateVersions;

		private bool sendAttributionData;

		public string UserId
		{
			get
			{
				return userId;
			}
			set
			{
				userIdSet = true;
				userId = value;
			}
		}

		internal bool UserIdSet
		{
			get
			{
				return userIdSet;
			}
		}

		public bool TestMode
		{
			get
			{
				return testMode;
			}
			set
			{
				testMode = value;
			}
		}

		public bool CollectAdvertisingId
		{
			get
			{
				return collectAdvertisingId;
			}
			set
			{
				collectAdvertisingId = value;
			}
		}

		public bool ValidateVersions
		{
			get
			{
				return validateVersions;
			}
			set
			{
				validateVersions = value;
			}
		}

		public bool SendAttributionData
		{
			get
			{
				return sendAttributionData;
			}
			set
			{
				sendAttributionData = value;
			}
		}

		public SoomlaConfig()
		{
			userId = null;
			testMode = false;
			collectAdvertisingId = true;
			validateVersions = true;
			userIdSet = false;
			sendAttributionData = true;
		}
	}
}
