using System.Text;
using UnityEngine;

public static class LLSplashManager
{
	public struct SplashData
	{
		public Texture2D texture;

		public bool needRotate;

		public float pixelsPerMeter;

		public Vector2 newResolution;
	}

	public const string formatNameLoader = "loader_{0}x{1}";

	public static SplashData GetSplashData(bool useLandscapeOrientation)
	{
		string empty = string.Empty;
		bool flag = (useLandscapeOrientation ? true : false);
		int num = Mathf.Max(Screen.width, Screen.height);
		int num2 = Mathf.Min(Screen.width, Screen.height);
		float num3 = 1f;
		string empty2 = string.Empty;
		float num4 = (float)Screen.width / (float)Screen.height;
		Vector2 vector = default(Vector2);
		if (num <= 960 && num2 <= 640)
		{
			empty2 = "960x640";
			vector = new Vector2(960f, 640f);
		}
		else if (num <= 1024 && num2 <= 768)
		{
			empty2 = "1024x768";
			vector = new Vector2(1024f, 768f);
		}
		else if (num <= 1136 && num2 <= 640)
		{
			empty2 = "1136x640";
			vector = new Vector2(1136f, 640f);
		}
		else if (num <= 1334 && num2 <= 750)
		{
			empty2 = "1334x750";
			vector = new Vector2(1334f, 750f);
		}
		else if (num4 <= 1.5f)
		{
			empty2 = "2048x1536";
			vector = new Vector2(2048f, 1536f);
		}
		else
		{
			empty2 = "2208x1242";
			vector = new Vector2(2208f, 1242f);
		}
		if (!useLandscapeOrientation)
		{
			string[] array = empty2.Split('x');
			StringBuilder stringBuilder = new StringBuilder();
			empty2 = stringBuilder.Append(array[1]).Append("x").Append(array[0])
				.ToString();
		}
		flag = false;
		empty = string.Format("loader_{0}", empty2);
		string str = PathUtils.GetDataRoot().AppendPathComponent("Textures").AppendPathComponent("Loader")
			.AppendWWWFilePrefix();
		string imagePath = str.AppendPathComponent(empty).AppendPathExtention("png");
		Texture2D texture2D = TextureManager.LoadTexture(imagePath, false);
		num3 = (float)texture2D.width / (float)Screen.width;
		float num5 = (float)texture2D.height / num3;
		if (num5 < (float)Screen.height)
		{
			num3 = num5 / (float)Screen.height * num3;
		}
		return new SplashData
		{
			texture = texture2D,
			needRotate = flag,
			pixelsPerMeter = num3,
			newResolution = 
			{
				x = (float)texture2D.width / num3,
				y = (float)texture2D.height / num3
			}
		};
	}

	public static void UnloadTexture(tk2dSpriteFromTexture sprite)
	{
		if (sprite != null)
		{
			sprite.Clear();
			if (sprite.texture != null)
			{
				TextureManager.UnloadTexture((Texture2D)sprite.texture);
				sprite.texture = null;
			}
		}
	}
}
