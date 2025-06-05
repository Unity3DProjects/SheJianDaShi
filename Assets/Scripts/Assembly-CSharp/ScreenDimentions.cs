using UnityEngine;

public static class ScreenDimentions
{
	public const int iPhone6Height = 1334;

	public const int iPhone6PlusHeight = 1920;

	public const int iPadProHeight = 2732;

	public const int iPhoneXHeight = 2436;

	public const float androidDownScaleRatio = 1.8f;

	public const float androidScaleMultiplier = 1.15f;

	private static tk2dCamera uiCamera;

	private static int uiWidth;

	private static int uiHeight;

	public static int Height
	{
		get
		{
			int height = Screen.height;
			UpdateUICamera();
			return uiHeight;
		}
	}

	public static int Width
	{
		get
		{
			int width = Screen.width;
			UpdateUICamera();
			return uiWidth;
		}
	}

	public static int NativeResolutionWidth { get; private set; }

	public static int NativeResolutionHeight { get; private set; }

	public static void UpdateUICamera()
	{
		if (uiCamera == null)
		{
			uiCamera = GameObject.Find("/GUI/GUICamera").GetComponent<tk2dCamera>();
			NativeResolutionWidth = uiCamera.nativeResolutionWidth;
			NativeResolutionHeight = uiCamera.nativeResolutionHeight;
			tk2dCameraResolutionOverride tk2dCameraResolutionOverride2 = new tk2dCameraResolutionOverride();
			tk2dCameraResolutionOverride2.matchBy = tk2dCameraResolutionOverride.MatchByType.Wildcard;
			tk2dCameraResolutionOverride2.fitMode = tk2dCameraResolutionOverride.FitMode.Constant;
			uiCamera.resolutionOverride = new tk2dCameraResolutionOverride[1] { tk2dCameraResolutionOverride2 };
		}
		if (IsInLandscape())
		{
			uiHeight = 736;
			uiWidth = uiHeight * Screen.width / Screen.height;
		}
		else
		{
			uiHeight = 1308;
			uiWidth = (int)((float)(uiHeight * Screen.width) / (float)Screen.height);
			if (uiWidth > 770)
			{
				uiWidth = 770;
				uiHeight = (int)((float)(uiWidth * Screen.height) / (float)Screen.width);
			}
		}
		uiCamera.forceResolution = new Vector2(uiWidth, uiHeight);
	}

	public static bool IsInLandscape()
	{
		return Screen.width > Screen.height;
	}
}
