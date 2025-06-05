using System;

public static class TimeSpanExtension
{
	public static string ToHHMMSS(this TimeSpan timeSpan)
	{
		return (!(timeSpan.TotalSeconds > 0.0)) ? "00:00:00" : string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	public static string ToHHMM(this TimeSpan timeSpan)
	{
		return (!(timeSpan.TotalSeconds > 0.0)) ? "00:00" : string.Format("{0:D2}:{1:D2}", timeSpan.Hours, timeSpan.Minutes);
	}

	public static string ToMMSS(this TimeSpan timeSpan)
	{
		return (!(timeSpan.TotalSeconds > 0.0)) ? "00:00" : string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
	}
}
