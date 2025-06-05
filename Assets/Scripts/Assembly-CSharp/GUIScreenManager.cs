using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class GUIScreenManager : SingletonMonoBehaviour<GUIScreenManager>
{
	private const float TRANSITION_PLANE_HIDE_DELAY = 0.5f;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnLanguageFontsChanged;

	[SerializeField]
	private GUIScreen[] screensPrefabs;

	[SerializeField]
	private List<GUIScreenType> screensClearingHistory;

	[SerializeField]
	private tk2dCamera uiCamera;

	[SerializeField]
	private TransitionPlane transitionPlanePrefab;

	[SerializeField]
	private TapToStartScreen tapToStartScreenPrefab;

	private List<GUIScreen> activeScreens = new List<GUIScreen>();

	private GUIScreen screenToShow;

	private TransitionPlane cachedTransitionPlane;

	private float transitionPlaneDelayCounter;

	private bool shouldHideTransitionPlaneAfterDelay;

	private static float guiScale = -1f;

	private bool areAtlasesDownloaded;

	private Managers cachedManagers;

	public static float GuiScale
	{
		get
		{
			if (guiScale < 0f)
			{
				float val = (float)ScreenDimentions.Width / (float)Screen.width;
				float num = (float)ScreenDimentions.Height / (float)Screen.height;
				if (num < 1f)
				{
					guiScale = Math.Min(val, num);
				}
				else
				{
					guiScale = Math.Max(val, num);
				}
			}
			return guiScale;
		}
	}

	public tk2dCamera UICamera
	{
		get
		{
			return uiCamera;
		}
	}

	public GameScreen GameScreen
	{
		get
		{
			GUIScreen screenByType = GetScreenByType(GUIScreenType.Game);
			if (screenByType != null)
			{
				return screenByType as GameScreen;
			}
			return null;
		}
	}

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	public MenuScreen MenuScreen
	{
		get
		{
			GUIScreen screenByType = GetScreenByType(GUIScreenType.Main);
			if (screenByType != null)
			{
				return screenByType as MenuScreen;
			}
			return null;
		}
	}

	public ResultScreen ResultScreen
	{
		get
		{
			GUIScreen screenByType = GetScreenByType(GUIScreenType.Result);
			if (screenByType != null)
			{
				return screenByType as ResultScreen;
			}
			return null;
		}
	}

	public RouletteScreen RouletteScreen
	{
		get
		{
			GUIScreen screenByType = GetScreenByType(GUIScreenType.Roulette);
			if (screenByType != null)
			{
				return screenByType as RouletteScreen;
			}
			return null;
		}
	}

	public GUIScreenType BeforeShopScreenType { get; set; }

	public GUIScreenType ActiveScreen
	{
		get
		{
			if (activeScreens != null && activeScreens.Count > 0)
			{
				return activeScreens[activeScreens.Count - 1].Type;
			}
			return GUIScreenType.None;
		}
	}

	public TransitionPlane CachedTransitionPlane
	{
		get
		{
			if (cachedTransitionPlane == null)
			{
				cachedTransitionPlane = UnityEngine.Object.Instantiate(transitionPlanePrefab);
				cachedTransitionPlane.transform.parent = base.transform;
				cachedTransitionPlane.transform.localPosition = new Vector3(0f, 0f, -200f);
				cachedTransitionPlane.transform.localScale = Vector3.one;
			}
			return cachedTransitionPlane;
		}
	}

	public bool IsNeedPlayUnlockVSHumanModeEffect { get; set; }

	public bool IsNeedPlayUnlockBirdHuntModeEffect { get; set; }

	public bool IsNeedPlayUnlockAppleModeEffect { get; set; }

	public bool IsNeedPlayUnlockTournamentModeEffect { get; set; }

	public bool IsNeedPlayUnlockMultiplayerModeEffect { get; set; }

	public GUIScreen CurrentActiveScreen
	{
		get
		{
			if (activeScreens.Count == 0)
			{
				return null;
			}
			return activeScreens[activeScreens.Count - 1];
		}
	}

	public static event Action OnLanguageFontsChanged
	{
		add
		{
			Action action = GUIScreenManager.m_OnLanguageFontsChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GUIScreenManager.m_OnLanguageFontsChanged, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GUIScreenManager.m_OnLanguageFontsChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GUIScreenManager.m_OnLanguageFontsChanged, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private new void Awake()
	{
		base.Awake();
		SwitchSettingsPrefab();
	}

	private void Start()
	{
		AdjustGUIScale();
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			TapToStartScreen tapToStartScreen = UnityEngine.Object.Instantiate(tapToStartScreenPrefab);
			tapToStartScreen.transform.parent = base.transform;
			tapToStartScreen.transform.localPosition = Vector3.back * 150f;
			tapToStartScreen.transform.localScale = Vector3.one;
			tapToStartScreen.gameObject.SetActive(true);
		}, 0f);
	}

	private void Update()
	{
		if (!shouldHideTransitionPlaneAfterDelay || !areAtlasesDownloaded)
		{
			return;
		}
		transitionPlaneDelayCounter += Time.unscaledDeltaTime;
		if (transitionPlaneDelayCounter > 0.5f)
		{
			shouldHideTransitionPlaneAfterDelay = false;
			transitionPlaneDelayCounter = 0f;
			CachedTransitionPlane.ChangeTransitionScale(false, delegate
			{
				tk2dUIManager.Instance.InputEnabled = true;
			});
		}
	}

	private void OnEnable()
	{
		GameManager.OnGameDownload += GameManager_OnGameDownload;
		LocalisationManager.OnLanguageChanged += LocalisationManager_OnLanguageChanged;
		GameManager.OnGamePause += GameManager_OnGamePause;
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void OnDisable()
	{
		GameManager.OnGameDownload -= GameManager_OnGameDownload;
		LocalisationManager.OnLanguageChanged -= LocalisationManager_OnLanguageChanged;
		GameManager.OnGamePause -= GameManager_OnGamePause;
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void SwitchSettingsPrefab()
	{
		int num = 0;
		GUIScreen[] array = screensPrefabs;
		foreach (GUIScreen gUIScreen in array)
		{
			if (gUIScreen.name == "SettingsScreen")
			{
				GameObject gameObject = Resources.Load("Miniclip/Prefabs/SettingsScreenAndroid") as GameObject;
				GUIScreen component = gameObject.GetComponent<GUIScreen>();
				screensPrefabs[num] = component;
				break;
			}
			num++;
		}
	}

	private GUIScreen ScreenWithType(GUIScreenType type)
	{
		for (int i = 0; i < screensPrefabs.Length; i++)
		{
			GUIScreen gUIScreen = screensPrefabs[i];
			if (gUIScreen.Type == type)
			{
				return gUIScreen;
			}
		}
		return null;
	}

	private GUIScreen ScreenFromStackWithType(GUIScreenType type)
	{
		foreach (GUIScreen activeScreen in activeScreens)
		{
			if (activeScreen.Type == type)
			{
				return activeScreen;
			}
		}
		return null;
	}

	public void HideCurrentAndShow(GUIScreenType t, Action<GUIControl> onFinished = null, bool needShowTransitionPlane = true)
	{
		HideCurrentScreen(false, delegate
		{
			ShowScreen(t);
		}, needShowTransitionPlane);
	}

	public void ShowScreen(GUIScreenType t, Action<GUIControl> onFinished = null, bool needForceTransitionPlane = false)
	{
		tk2dUIManager.Instance.DisableTouchesForScreenTransition();
		tk2dUIManager.Instance.InputEnabled = false;
		TweenCallback tweenCallback = delegate
		{
			StartCoroutine(NextFrameExecution(delegate
			{
				screenToShow = ScreenFromStackWithType(t);
				bool flag = true;
				if (screenToShow == null)
				{
					flag = false;
					screenToShow = UnityEngine.Object.Instantiate(ScreenWithType(t));
				}
				if (screenToShow != null)
				{
					screenToShow.gameObject.SetActive(true);
					if (screensClearingHistory.Contains(t))
					{
						foreach (GUIScreen activeScreen in activeScreens)
						{
							if (activeScreen != null && activeScreen != screenToShow)
							{
								UnityEngine.Object.Destroy(activeScreen.gameObject);
							}
						}
						activeScreens.Clear();
					}
					Action<GUIControl> onFinished2 = delegate(GUIControl control)
					{
						if (onFinished != null)
						{
							onFinished(control);
						}
						tk2dUIManager.Instance.LockedForTransition = false;
					};
					activeScreens.Add(screenToShow);
					if (!flag)
					{
						screenToShow.Show(onFinished2);
						screenToShow.transform.parent = base.transform;
						screenToShow.transform.localScale = Vector3.one;
						screenToShow.transform.localPosition = new Vector3(0f, 0f, 500 - activeScreens.Count * 100);
					}
					else
					{
						tk2dUIManager.Instance.LockedForTransition = false;
					}
					screenToShow = null;
					shouldHideTransitionPlaneAfterDelay = true;
				}
			}));
		};
		if (needForceTransitionPlane)
		{
			CachedTransitionPlane.ChangeTransitionScale(true, tweenCallback);
		}
		else
		{
			tweenCallback(null);
		}
	}

	public void ShowPreviousScreen()
	{
		tk2dUIManager.Instance.DisableTouchesForScreenTransition();
		GUIScreenType t = GUIScreenType.None;
		if (activeScreens.Count > 1)
		{
			t = activeScreens[activeScreens.Count - 2].Type;
		}
		HideCurrentAndShow(t);
	}

	public void HideCurrentScreen(bool needRemoveFromStack, Action<GUIControl> onFinished = null, bool needShowTransitionPlane = true)
	{
		tk2dUIManager.Instance.InputEnabled = false;
		TweenCallback tweenCallback = delegate
		{
			StartCoroutine(NextFrameExecution(delegate
			{
				if (activeScreens.Count > 0)
				{
					GUIScreen currentScreen = activeScreens[activeScreens.Count - 1];
					if (needRemoveFromStack)
					{
						activeScreens.Remove(currentScreen);
					}
					Action<GUIControl> onFinished2 = delegate(GUIControl c)
					{
						if (currentScreen != null)
						{
							if (needRemoveFromStack)
							{
								UnityEngine.Object.Destroy(currentScreen.gameObject);
							}
							else
							{
								currentScreen.gameObject.SetActive(false);
							}
							tk2dUIManager.Instance.InputEnabled = true;
							if (onFinished != null)
							{
								onFinished(c);
							}
						}
					};
					currentScreen.Hide(onFinished2);
				}
				else if (onFinished != null)
				{
					onFinished(null);
				}
			}));
		};
		if (needShowTransitionPlane)
		{
			CachedTransitionPlane.ChangeTransitionScale(true, tweenCallback);
		}
		else
		{
			tweenCallback(null);
		}
	}

	public GUIScreen GetScreenByType(GUIScreenType type)
	{
		for (int i = 0; i < activeScreens.Count; i++)
		{
			GUIScreen gUIScreen = activeScreens[i];
			if (gUIScreen.Type == type)
			{
				return gUIScreen;
			}
		}
		return null;
	}

	public void ForceGUIScaleRecalculation()
	{
		guiScale = -1f;
	}

	private void AdjustGUIScale()
	{
		Vector3 localScale = Vector3.one / GuiScale;
		localScale.z = 1f;
		base.transform.localScale = localScale;
	}

	private void GameManager_OnGameDownload()
	{
		areAtlasesDownloaded = true;
	}

	private void LocalisationManager_OnLanguageChanged()
	{
		LocaleMeshChanger[] componentsInChildren = GetComponentsInChildren<LocaleMeshChanger>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].RefreshMeshFonts();
		}
		LocaleSpriteChanger[] componentsInChildren2 = GetComponentsInChildren<LocaleSpriteChanger>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].RefreshSpriteFonts();
		}
		GUIScreenManager.m_OnLanguageFontsChanged?.Invoke();
	}

	private void GameManager_OnGamePause(bool isPaused)
	{
		if (GameScreen != null)
		{
			GameScreen.PauseBrutalityEffects(isPaused);
		}
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType orientation)
	{
		AdjustGUIScale();
	}

	private IEnumerator NextFrameExecution(Action callback)
	{
		yield return null;
		if (callback != null)
		{
			callback();
		}
	}
}
