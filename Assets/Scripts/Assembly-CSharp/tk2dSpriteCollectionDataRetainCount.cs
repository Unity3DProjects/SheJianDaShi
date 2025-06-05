using UnityEngine;

public static class tk2dSpriteCollectionDataRetainCount
{
	public static void AddRetain(this tk2dSpriteCollectionData platfrom, MonoBehaviour sender)
	{
		if ((bool)platfrom)
		{
			platfrom.AddRetainCount(sender);
		}
	}

	public static void RemoveRetain(this tk2dSpriteCollectionData platfrom, MonoBehaviour sender)
	{
		if ((bool)platfrom)
		{
			platfrom.RemoveRetainCount(sender);
		}
	}
}
