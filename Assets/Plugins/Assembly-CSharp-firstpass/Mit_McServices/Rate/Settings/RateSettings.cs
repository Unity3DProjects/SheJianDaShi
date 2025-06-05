namespace Mit_McServices.Rate.Settings
{
	public class RateSettings
	{
		public bool WillUseNativePopup { get; set; }

		public RateTexts Texts { get; set; }

		public float MinimumActiveTimePerSession { get; set; }

		public int QuestionLimitPerVersion { get; set; }

		public int MinimumSessionStarts { get; set; }

		public float MinimumDaysInterval { get; set; }

		public int MinimumPositiveExperiences { get; set; }

		public bool ShouldUseIOSNativePopup { get; set; }
	}
}
