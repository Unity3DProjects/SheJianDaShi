using System;

[Serializable]
public class FloorSettings
{
	public int winsCount;

	public int maxHeight = 1;

	public int minHeight = -1;

	public int minStraightParts = 1;

	public int maxStraightParts = 5;
}
