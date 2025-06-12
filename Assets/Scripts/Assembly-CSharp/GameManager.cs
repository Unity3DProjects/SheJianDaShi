using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Mit_Goliath_Native.Utils;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	private const string SYSTEM_DOWNLOAD_ERROR_TITLE = "Installation error";

	private const string SYSTEM_DOWNLOAD_ERROR_MESSAGE = "Please reinstall the application";

	private const string SYSTEM_DOWNLOAD_ERROR_BUTTON_TITLE = "OK";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnGameStart;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnGameStop;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnGamePause;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnGameDownload;

	[SerializeField]
	private CameraController cameraController;

	[SerializeField]
	private List<GameObject> managers;

	[SerializeField]
	private List<GameObject> loadedManagers;

	[SerializeField]
	private int defaultUnlockedGameType = 1;

	[SerializeField]
	private int vsHumanUnlockBorder = 15;

	[SerializeField]
	private int birdsHuntUnlockBorder = 7;

	[SerializeField]
	private int appleShootUnlockBorder = 10;

	[SerializeField]
	private int tournamentUnlockBorder = 20;

	[SerializeField]
	private int multiplayerUnlockBorder = 30;

	private Managers cachedManagers;

	private LevelsManager cachedLevelsManager;

	private GameType currentGameType;

	private int coinsEarnedDuringGame;

	private DateTime gameStartTime;

	private static string saveLoadedTextKey = "ui.toast.save.loaded";

	private static string saveLoadedTutorialTextKey = "ui.toast.save.loaded.tutorial";

	private bool isGamePaused = true;

	private bool isGameRunning;

	public CameraController CameraController
	{
		get
		{
			return cameraController;
		}
	}

	public bool IsGamePaused
	{
		get
		{
			return isGamePaused;
		}
		set
		{
			if (value != isGamePaused)
			{
				isGamePaused = value;
				if (isGamePaused)
				{
					Time.timeScale = 0f;
				}
				else
				{
					Time.timeScale = 1f;
				}
				GameManager.m_OnGamePause?.Invoke(isGamePaused);
			}
		}
	}

	public bool IsGameRunning
	{
		get
		{
			return isGameRunning && !isGamePaused;
		}
	}

	public GameType CurrentGameType { get; set; }

	public bool IsVisibleOnGUI { get; set; }

	public int DefaultUnlockedGameType
	{
		get
		{
			return defaultUnlockedGameType;
		}
	}

	public int VSHumanUnlockBorder
	{
		get
		{
			return vsHumanUnlockBorder;
		}
	}

	public int AppleShootUnlockBorder
	{
		get
		{
			return appleShootUnlockBorder;
		}
	}

	public int BirdsHuntUnlockBorder
	{
		get
		{
			return birdsHuntUnlockBorder;
		}
	}

	public int TournamentUnlockBorder
	{
		get
		{
			return tournamentUnlockBorder;
		}
	}

	public int MultiplayerUnlockBorder
	{
		get
		{
			return multiplayerUnlockBorder;
		}
	}

	public bool IsTapToStartScreenExist { get; private set; }

	public Managers CachedManagers
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

	protected LevelsManager CachedLevelsManager
	{
		get
		{
			if (cachedLevelsManager == null)
			{
				cachedLevelsManager = CachedManagers.CachedLevelsManager;
			}
			return cachedLevelsManager;
		}
	}

	public static bool IsIPhoneX
	{
		get
		{
			return false;
		}
	}

	public static event Action OnGameStart
	{
		add
		{
			Action action = GameManager.m_OnGameStart;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGameStart, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GameManager.m_OnGameStart;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGameStart, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnGameStop
	{
		add
		{
			Action action = GameManager.m_OnGameStop;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGameStop, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GameManager.m_OnGameStop;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGameStop, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnGamePause
	{
		add
		{
			Action<bool> action = GameManager.m_OnGamePause;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGamePause, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = GameManager.m_OnGamePause;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGamePause, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnGameDownload
	{
		add
		{
			Action action = GameManager.m_OnGameDownload;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGameDownload, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GameManager.m_OnGameDownload;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GameManager.m_OnGameDownload, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		isGamePaused = true;
		// Application.targetFrameRate = ((DeviceInfo.CurrentClass != DeviceInfo.PerformanceClass.High) ? 30 : 60);
		IsVisibleOnGUI = true;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
		if (IsIPhoneX)
		{
			Screen.autorotateToPortraitUpsideDown = false;
		}
		Screen.orientation = ScreenOrientation.AutoRotation;
		foreach (GameObject manager in managers)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(manager);
			gameObject.name = manager.name;
			gameObject.transform.parent = base.transform;
			loadedManagers.Add(gameObject);
		}
		managers.Clear();
		CachedManagers.CachedStatisticsManager.TotalSeesionCount++;
		CachedManagers.CachedStatisticsManager.CheckUnlockGameModes();
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.GetInstanceID();
		SingletonMonoBehaviour<AdjustManager>.Instance.GetInstanceID();
		SingletonMonoBehaviour<GDPRManager>.Instance.GetInstanceID();
		GameObject miniclipManagers = MiniclipPrefabs.Instance.miniclipManagers;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(miniclipManagers);
		gameObject2.name = miniclipManagers.name;
		gameObject2.transform.parent = base.transform;
		loadedManagers.Add(gameObject2);
		if (CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
		{
			StartMenu();
		}
		IsTapToStartScreenExist = true;
		GameManager.m_OnGameDownload?.Invoke();
		if (CachedManagers.CachedStatisticsManager.OpenedChestsCount >= 1 && !CachedManagers.CachedStatisticsManager.NewAdsAdded)
		{
			CachedManagers.CachedStatisticsManager.NewAdsAdded = true;
			CustomPlayerPrefs.SetDateTime("last_super_gift_date", DateTime.Now - TimeSpan.FromDays(1.0));
			CustomPlayerPrefs.SetDateTime("last_roulette_played_day", DateTime.Now - TimeSpan.FromDays(1.0));
			CustomPlayerPrefs.SetDateTime("last_free_chest_accelerator", DateTime.Now - TimeSpan.FromDays(1.0));
		}
	}

	private void OnEnable()
	{
		TapToStartScreen.OnLoaderRemoved += TapToStartScreen_OnLoaderRemoved;
		CloudSaveManager.OnSaveLoaded += GameCenterManager_OnSaveLoaded;
	}

	private void OnDisable()
	{
		TapToStartScreen.OnLoaderRemoved -= TapToStartScreen_OnLoaderRemoved;
		CloudSaveManager.OnSaveLoaded -= GameCenterManager_OnSaveLoaded;
	}

	private void FixedUpdate()
	{
		if (!isGamePaused)
		{
			float deltaTime = Time.deltaTime;
			CachedLevelsManager.UpdateLevel(deltaTime);
			CameraController.UpdateCameraController(deltaTime);
		}
	}

	private void Update()
	{
		if (!isGamePaused)
		{
			TouchManager.UpdateTouches();
		}
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		TapToStartScreen componentInChildren = SingletonMonoBehaviour<GUIScreenManager>.Instance.gameObject.GetComponentInChildren<TapToStartScreen>();
		if (componentInChildren != null && componentInChildren.isActiveAndEnabled)
		{
			componentInChildren.BackButtonPressed();
			return;
		}
		if (BowmastersNewsfeed.Instance.IsNewsfeedShowing)
		{
			BowmastersNewsfeed.Instance.DismissBoard();
			return;
		}
		GUIPopup activePopup = SingletonMonoBehaviour<GUIPopupManager>.Instance.ActivePopup;
		if (activePopup != null)
		{
			activePopup.BackButtonPressed();
		}
		else
		{
			SingletonMonoBehaviour<GUIScreenManager>.Instance.CurrentActiveScreen.BackButtonPressed();
		}
	}

	private void LateUpdate()
	{
		if (!isGamePaused)
		{
			CachedLevelsManager.UpdateLevelLocation(Time.deltaTime);
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			return;
		}
		CustomPlayerPrefs.Save();
		if (IsGameRunning)
		{
			CachedManagers.CachedGUIPopupManager.HidePopup(PopupType.Timer);
			if (CurrentGameType != GameType.Multiplayer)
			{
				IsGamePaused = true;
				CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Pause, null, false);
			}
			else
			{
				StopGame();
				CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Main);
			}
		}
	}

	private void OnApplicationQuit()
	{
		if (IsGameRunning)
		{
			TrackGameEnd(GoliathIds.MatchEndReason.LEFT);
		}
		CustomPlayerPrefs.Save();
	}

	public void StartMenu()
	{
		CachedManagers.CachedStatisticsManager.ResetStatistics();
		CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Main);
	}

	public void StartGame()
	{
		gameStartTime = DateTime.Now;
		isGameRunning = true;
		CachedManagers.CachedStatisticsManager.ResetStatistics();
		CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Game, delegate
			{
				CachedLevelsManager.CreateLevel(CurrentGameType, cameraController);
				GameManager.m_OnGameStart?.Invoke();
			}, true);
		});
	}

	public void StopGame()
	{
		isGameRunning = false;
		CloudSaveManager.Instance.SaveToCloud();
		CachedLevelsManager.RemoveLevel();
		GameManager.m_OnGameStop?.Invoke();
	}

	public void RestartGame()
	{
		StartGame();
	}

	public virtual void StartTutorialMatch()
	{
		CachedManagers.CachedStatisticsManager.ResetStatistics();
		CurrentGameType = GameType.VSComputer;
		string tutorialCharacterID = ((CachedManagers.CachedStatisticsManager.OpenedChestsCount != 0) ? "ronetienne" : "robinhood");
		ShopItemCharacter shopItemCharacter = CachedManagers.CachedShopManager.AvailableCharacters.Find((ShopItemCharacter character) => character.ShopItemID == tutorialCharacterID);
		CachedLevelsManager.ObjectsSpawner.SetFirstSelectedCharacter(shopItemCharacter.Infos[0].characterNameType);
		ShopItemCharacter shopItemCharacter2;
		do
		{
			shopItemCharacter2 = CachedManagers.CachedShopManager.EasyCharacters.RandomObject();
		}
		while (shopItemCharacter2 == shopItemCharacter);
		CachedLevelsManager.ObjectsSpawner.SetSecondSelectedCharacter(shopItemCharacter2.Infos[0].characterNameType);
		CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Versus);
	}

	public void ForceResourcesUnload(float delay)
	{
		if (delay < 0f)
		{
			Resources.UnloadUnusedAssets();
			return;
		}
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			Resources.UnloadUnusedAssets();
		}, delay);
	}

	public void TrackGameEnd(GoliathIds.MatchEndReason lostEndReason = null)
	{
		Level currentLevel = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel;
		GameType gameType = currentLevel.GameType;
		GoliathIds.GameMode gameMode = GoliathIds.GameMode.GameTypeToGameMode(gameType, SingletonMonoBehaviour<TournamentManager>.Instance.CurrentTournament.settings.difficultType);
		int matchDuration = (int)DateTime.Now.Subtract(gameStartTime).TotalSeconds;
		if (gameType == GameType.VSHuman)
		{
			GameGoliath.Instance.TrackMatchEnd(gameMode, matchDuration);
			return;
		}
		CharacterNameType firstSelectedCharacterNameType = SingletonMonoBehaviour<Managers>.Instance.CachedLevelsManager.ObjectsSpawner.FirstSelectedCharacterNameType;
		ShopItemCharacter shopItemCharacter = SingletonMonoBehaviour<Managers>.Instance.CachedLevelsManager.ObjectsSpawner.GetShopItemCharacter(firstSelectedCharacterNameType);
		string characterID = shopItemCharacter.GetCharacterID();
		GoliathIds.MatchResult result;
		GoliathIds.MatchEndReason endReason;
		if (lostEndReason != null)
		{
			result = GoliathIds.MatchResult.LOST;
			endReason = lostEndReason;
		}
		else
		{
			bool flag = currentLevel.LostCharacter != currentLevel.CharacterFirst;
			result = ((!flag) ? GoliathIds.MatchResult.LOST : GoliathIds.MatchResult.WON);
			endReason = ((!flag) ? GoliathIds.MatchEndReason.LOST : GoliathIds.MatchEndReason.WON);
		}
		GameGoliath.Instance.TrackMatchEnd(gameMode, matchDuration, characterID, result, endReason);
	}

	private void DownloadErrorCallback()
	{
		Application.Quit();
	}

	private void TapToStartScreen_OnLoaderRemoved()
	{
		if (!CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
		{
			StartTutorialMatch();
		}
		IsTapToStartScreenExist = false;
	}

	private void GameCenterManager_OnSaveLoaded()
	{
		if (IsGameRunning && SingletonMonoBehaviour<TutorialManager>.Instance.IsTutorialMatchesEnded)
		{
			SingletonMonoBehaviour<TutorialManager>.Instance.ForceTutorialDone();
			OnApplicationPause(true);
			MiniclipUtils.SendToast(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(saveLoadedTextKey));
			return;
		}
		MiniclipUtils.SendToast(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(saveLoadedTutorialTextKey));
		if (SingletonMonoBehaviour<GUIScreenManager>.Instance.CurrentActiveScreen == null)
		{
			StartMenu();
		}
	}
}
