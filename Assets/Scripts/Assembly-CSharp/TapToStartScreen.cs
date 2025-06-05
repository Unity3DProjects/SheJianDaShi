using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TapToStartScreen : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnLoaderRemoved;

	[SerializeField]
	private tk2dUIItem[] loaderButton;

	[SerializeField]
	private tk2dSpriteFromTexture[] loaderSprite;

	[SerializeField]
	private TweenScale[] labelTweener;

	[SerializeField]
	private float labelTweenerDuration;

	[SerializeField]
	private AnimationCurve labelTweenerCurve;

	[SerializeField]
	private float animationAppearDelay = 0.5f;

	[SerializeField]
	private GameObject[] animationHand;

	private GameObject tapToStart;

	private Texture2D loadedTexture;

	public static event Action OnLoaderRemoved
	{
		add
		{
			Action action = TapToStartScreen.m_OnLoaderRemoved;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TapToStartScreen.m_OnLoaderRemoved, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = TapToStartScreen.m_OnLoaderRemoved;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TapToStartScreen.m_OnLoaderRemoved, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		base.transform.localPosition = new Vector3(ScreenDimentions.Width / 2, ScreenDimentions.Height / 2, base.transform.localPosition.z);
		Screen.sleepTimeout = -1;
		Transform parent = animationHand[1].transform.parent.parent.parent.parent;
		parent.localEulerAngles = new Vector3(0f, 0f, 90f);
		parent.GetComponent<GUILayoutCell>().FixedSizeValue = 250f;
		parent.parent.GetChild(0).GetComponent<GUILayoutCell>().Type = GUILayoutCellType.FixedSize;
	}

	private void OnEnable()
	{
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
		SingletonMonoBehaviour<GDPRManager>.Instance.OnDataReceived += GDPRManager_OnDataReceived;
		SingletonMonoBehaviour<GDPRManager>.Instance.Initialize();
		for (int i = 0; i < loaderButton.Length; i++)
		{
			loaderButton[i].OnClick += LoaderButton_OnClick;
		}
		if (loadedTexture == null)
		{
			LLSplashManager.SplashData splashData = LLSplashManager.GetSplashData(true);
			int num = (int)((float)ScreenDimentions.Width / GUIScreenManager.GuiScale);
			int num2 = (int)((float)ScreenDimentions.Height / GUIScreenManager.GuiScale);
			string loaderPathForWidth = GetLoaderPathForWidth(num, num2, splashData.needRotate);
			TextureLoaded(TextureHelper.Instance.LoadImageToTexture(loaderPathForWidth, false, TextureFormat.RGB24), num2 > num);
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
				androidJavaClass.CallStatic(AndroidPluginNames.BOWMASTERS_ACTIVITY_REMOVE_SPLASH_SCREEN);
			}, animationAppearDelay);
		}
	}

	private void OnDisable()
	{
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
		SingletonMonoBehaviour<GDPRManager>.Instance.OnDataReceived -= GDPRManager_OnDataReceived;
		for (int i = 0; i < loaderButton.Length; i++)
		{
			loaderButton[i].OnClick -= LoaderButton_OnClick;
		}
		if (loadedTexture != null)
		{
			TextureHelper.Instance.UnloadTexture(loadedTexture);
			loadedTexture = null;
		}
	}

	private void Update()
	{
		for (int i = 0; i < labelTweener.Length; i++)
		{
			if (!labelTweener[i].gameObject.activeInHierarchy && labelTweener[i].enabled)
			{
				labelTweener[i].Update();
			}
		}
	}

	private string GetLoaderPathForWidth(int width, int height, bool isRotateNeeded)
	{
		string empty = string.Empty;
		isRotateNeeded = height > width;
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
		empty = androidJavaClass.CallStatic<string>(AndroidPluginNames.BOWMASTERS_ACTIVITY_GET_LOADER_NAME, new object[2] { width, height });
		string str = "Loader";
		return str.AppendPathComponent(empty).AppendPathExtention("png");
	}

	private void TextureLoaded(Texture2D tex, bool isRotateNeeded)
	{
		loadedTexture = tex;
		for (int i = 0; i < loaderSprite.Length; i++)
		{
			loaderSprite[i].texture = tex;
			loaderSprite[i].spriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
			loaderSprite[i].spriteCollectionSize.pixelsPerMeter = 1f;
			loaderSprite[i].anchor = tk2dBaseSprite.Anchor.MiddleCenter;
			loaderSprite[i].ForceBuild();
			loaderSprite[i].transform.localEulerAngles = new Vector3(0f, 0f, (!isRotateNeeded) ? 0f : 90f);
			Vector2 vector = new Vector2((float)ScreenDimentions.Width / (float)tex.width, (float)ScreenDimentions.Height / (float)tex.height);
			float num = Mathf.Max(vector.x, vector.y);
			loaderSprite[i].transform.localScale = new Vector3(num, num, 1f);
		}
	}

	protected void LoaderButton_OnClick()
	{
		TapToStartScreen.m_OnLoaderRemoved?.Invoke();
		UnityEngine.Object.Destroy(base.gameObject);
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
		androidJavaClass.CallStatic(AndroidPluginNames.BOWMASTERS_ACTIVITY_REMOVE_SPLASH_SCREEN);
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType newRotation)
	{
		int num = (int)((float)ScreenDimentions.Width / GUIScreenManager.GuiScale);
		int num2 = (int)((float)ScreenDimentions.Height / GUIScreenManager.GuiScale);
		string loaderPathForWidth = GetLoaderPathForWidth(num, num2, true);
		TextureLoaded(TextureHelper.Instance.LoadImageToTexture(loaderPathForWidth, false, TextureFormat.RGB24), num2 > num);
	}

	private void ShowTapToStart()
	{
		for (int i = 0; i < labelTweener.Length; i++)
		{
			labelTweener[i].duration = labelTweenerDuration;
			labelTweener[i].animationCurve = labelTweenerCurve;
			labelTweener[i].SetOnFinishedDelegate(delegate
			{
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						for (int j = 0; j < animationHand.Length; j++)
						{
							animationHand[j].SetActive(true);
						}
					}
				}, animationAppearDelay);
			});
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				for (int j = 0; j < labelTweener.Length; j++)
				{
					labelTweener[j].enabled = true;
				}
			}
		}, 0f);
	}

	private void GDPRManager_OnDataReceived()
	{
		Debug.Log("GDPRManager_OnDataReceived");
		SingletonMonoBehaviour<GDPRManager>.Instance.ShowPopupIfRequired();
		ShowTapToStart();
	}

	public void BackButtonPressed()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
		androidJavaClass.CallStatic(AndroidPluginNames.BOWMASTERS_ACTIVITY_SHOW_EXIT_POPUP);
	}
}
