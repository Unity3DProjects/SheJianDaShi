using UnityEngine;

public static class DeviceInfo
{
	public enum PerformanceClass
	{
		TooLow = 0,
		iPhone4 = 1,
		iPhone4S = 2,
		iPad3 = 3,
		iPad4 = 4,
		iPhone5 = 5,
		High = 6
	}

	public const int LIMIT_PROCESSOR_MHZ = 1400;

	public const int LIMIT_RAM_MB = 1000;

	public const int LIMIT_PROCESSOR_CORES = 4;

	public static string UUID
	{
		get
		{
			return SystemInfo.deviceUniqueIdentifier;
		}
	}

	public static PerformanceClass CurrentClass
	{
		get
		{
			PerformanceClass result = PerformanceClass.High;
			if (SystemInfo.processorFrequency < 1400 || SystemInfo.systemMemorySize < 1000 || SystemInfo.processorCount < 4)
			{
				result = PerformanceClass.TooLow;
			}
			return result;
		}
	}
}
