using System;

[Serializable]
public class SupersonicBannerSettings
{
	public enum SupersonicBannerSize
	{
		Default = 0,
		Large = 1,
		Rectangle = 2,
		Smart = 3
	}

	public enum SupersonicBannerPosition
	{
		Top = 0,
		Bottom = 1,
		TopLeft = 2,
		TopRight = 3,
		BottomLeft = 4,
		BottomRight = 5
	}

	public SupersonicBannerSize size;

	public SupersonicBannerPosition position;
}
