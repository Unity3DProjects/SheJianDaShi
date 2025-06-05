using UnityEngine;

public static class LLSystemSharingManager
{
	public static void Share(string text, string imagePath, string url)
	{
	}

	public static void Share(string text, Texture2D imageTexture, string url)
	{
		byte[] imageBytes = imageTexture.EncodeToJPG(100);
		int width = imageTexture.width;
		int height = imageTexture.height;
		Share(text, imageBytes, width, height, url);
	}

	public static void Share(string text, byte[] imageBytes, int widthImage, int heightImage, string url)
	{
	}
}
