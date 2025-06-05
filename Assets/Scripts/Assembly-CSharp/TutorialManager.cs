using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using MiniJSON;
using UnityEngine;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnTutorialMatchesEnded;

	private const string IS_VS_COMP_TUTORIAL_FINISHED = "is_vs_comp_tutorial_finished";

	private const string IS_VS_HUMAN_TUTORIAL_FINISHED = "is_vs_human_tutorial_finished";

	private const string IS_APPLE_GAME_TUTORIAL_FINISHED = "is_apple_game_tutorial_finished";

	private const string IS_BIRDS_GAME_TUTORIAL_FINISHED = "is_birds_game_tutorial_finished";

	private const string WAS_CHARACTER_BOUGHT_ON_COINS = "was_character_bought_on_coins";

	private const string WAS_CHARACTER_UPGRADED_ON_GEMS = "was_character_upgraded_on_gems";

	private const string UNLOCKED_ACTIVATIONS_KEY = "unlocked_activations";

	private const string MATCHES_FOR_COINS_REMINDER_KEY = "matches_for_coins_reminder";

	[SerializeField]
	private int hintDelayByNoActivationShoot = 3;

	[SerializeField]
	private float hintDelayByInactivity = 30f;

	[SerializeField]
	private WeaponType tutorialWeaponDuel = WeaponType.Arrow;

	[SerializeField]
	private WeaponType tutorialWeaponApple = WeaponType.Arrow;

	[SerializeField]
	private WeaponType tutorialWeaponBirds = WeaponType.Arrow;

	[SerializeField]
	private GameObject buyCharacterTutorialPrefab;

	[SerializeField]
	private int winsToDisableEasyMatches;

	[SerializeField]
	private int matchesWithLowHealthEnemies;

	[SerializeField]
	private float lowHealthEnemiesCoeff;

	[SerializeField]
	private int matchesForFirstReminder = 1;

	[SerializeField]
	private int matchesForRepeatReminder = 1;

	[SerializeField]
	private int tutorialMatchesCount = 2;

	[SerializeField]
	private int freeChestsCount = 3;

	private int firstPlayerShotsWithoutActivation;

	private int secondPlayerShotsWithoutActivation;

	private float playerInactivityTime;

	private int turnsForShootingAfterTutorial;

	private bool isVSCompTutorialInProgress;

	private bool isAppleGameTutorialInProgress;

	private bool isBirdsGameTutorialInProgress;

	private List<TutorialType> unlockedActivations;

	private Tutorial cachedTutorial;

	private TutorialType curTutorialType;

	private Managers cachedManagers;

	private LevelsManager cachedLevelsManager;

	private ObjectsSpawner objectsSpawner;

	private bool wasWeaponActivated;

	private bool isAimingTutorialNeed;

	private GameObject[] tutorialCharacterIcon;

	private GameObject[] tutorialCoinsButton;

	private bool isTutorialCharacterSelected;

	private int tutorialCharacterIndex;

	private bool isTutorialMatchesEndedEventSended;

	private ShopItemCharacter characterWithMinUpgradePrice;

	private ShopItemCharacter availablecharacterWithMaxUpgradePrice;

	private bool isGemsTutorial;

	public bool IsVSCompTutorialInProgress
	{
		get
		{
			return isVSCompTutorialInProgress;
		}
		set
		{
			if (isVSCompTutorialInProgress == value || IsVSCompTutorialFinished)
			{
				return;
			}
			isVSCompTutorialInProgress = value;
			if (!isVSCompTutorialInProgress)
			{
				IsVSCompTutorialFinished = true;
				if (SingletonMonoBehaviour<GameManager>.Instance.IsGameRunning && cachedTutorial != null)
				{
					CachedTutorial.ShowCongratulations();
				}
			}
			else
			{
				isAimingTutorialNeed = true;
				turnsForShootingAfterTutorial = 1;
			}
		}
	}

	public bool IsAppleGameTutorialInProgress
	{
		get
		{
			return isAppleGameTutorialInProgress;
		}
		set
		{
			if (isAppleGameTutorialInProgress != value && !IsAppleGameTutorialFinished)
			{
				isAppleGameTutorialInProgress = value;
				if (!isAppleGameTutorialInProgress)
				{
					IsAppleGameTutorialFinished = true;
					CachedTutorial.ShowCongratulations();
				}
				else
				{
					isAimingTutorialNeed = true;
				}
			}
		}
	}

	public bool IsBirdsGameTutorialInProgress
	{
		get
		{
			return isBirdsGameTutorialInProgress;
		}
		set
		{
			if (isBirdsGameTutorialInProgress != value && !IsBirdsGameTutorialFinished)
			{
				isBirdsGameTutorialInProgress = value;
				if (!isBirdsGameTutorialInProgress)
				{
					IsBirdsGameTutorialFinished = true;
					CachedManagers.CachedGameManager.IsGamePaused = true;
					CachedTutorial.ShowCongratulations();
					Tutorial.OnCongratulationsDisappear += Tutorial_OnCongratulationsDisappear;
				}
				else
				{
					isAimingTutorialNeed = true;
				}
			}
		}
	}

	public WeaponType TutorialWeaponDuel
	{
		get
		{
			return tutorialWeaponDuel;
		}
	}

	public WeaponType TutorialWeaponApple
	{
		get
		{
			return tutorialWeaponApple;
		}
	}

	public WeaponType TutorialWeaponBirds
	{
		get
		{
			return tutorialWeaponBirds;
		}
	}

	public bool IsSwitchCharacterEnable
	{
		get
		{
			bool flag = true;
			if (CachedLevelsManager.CurrentLevel.GameType == GameType.VSComputer)
			{
				flag = !IsVSCompTutorialInProgress;
				if (flag && turnsForShootingAfterTutorial > 0)
				{
					flag = false;
					turnsForShootingAfterTutorial--;
				}
			}
			return flag;
		}
	}

	public bool WasCharacterBoughtOnCoins
	{
		get
		{
			return CustomPlayerPrefs.GetBool("was_character_bought_on_coins");
		}
		set
		{
			if (WasCharacterBoughtOnCoins != value)
			{
				CustomPlayerPrefs.SetBool("was_character_bought_on_coins", value);
				if (value)
				{
					StopBuyCharacterOnCoinsTutorial();
				}
			}
		}
	}

	public bool WasCharacterUpgradedOnGems
	{
		get
		{
			return CustomPlayerPrefs.GetBool("was_character_upgraded_on_gems");
		}
		set
		{
			availablecharacterWithMaxUpgradePrice = null;
			characterWithMinUpgradePrice = null;
			if (WasCharacterUpgradedOnGems != value)
			{
				CustomPlayerPrefs.SetBool("was_character_upgraded_on_gems", value);
				if (value)
				{
					StopBuyCharacterOnCoinsTutorial();
				}
			}
		}
	}

	public int CharacterSelectedIndex
	{
		set
		{
			IsTutorialCharacterSelected = tutorialCharacterIndex == value;
		}
	}

	private bool IsTutorialCharacterSelected
	{
		get
		{
			return isTutorialCharacterSelected;
		}
		set
		{
			isTutorialCharacterSelected = value;
			if (((WasCharacterBoughtOnCoins && !ShouldCoinsSpendReminderLoad) || isGemsTutorial) && !isGemsTutorial)
			{
				return;
			}
			if (tutorialCharacterIcon != null)
			{
				GameObject[] array = tutorialCharacterIcon;
				foreach (GameObject gameObject in array)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(!isTutorialCharacterSelected);
					}
				}
			}
			if (tutorialCoinsButton == null)
			{
				return;
			}
			GameObject[] array2 = tutorialCoinsButton;
			foreach (GameObject gameObject2 in array2)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(isTutorialCharacterSelected);
				}
			}
		}
	}

	public bool IsVSCompTutorialFinished
	{
		get
		{
			return CustomPlayerPrefs.GetBool("is_vs_comp_tutorial_finished");
		}
		set
		{
			if (IsVSCompTutorialFinished != value)
			{
				CustomPlayerPrefs.SetBool("is_vs_comp_tutorial_finished", value);
			}
		}
	}

	public bool IsVSHumanTutorialFinished
	{
		get
		{
			return CustomPlayerPrefs.GetBool("is_vs_human_tutorial_finished");
		}
		set
		{
			if (IsVSHumanTutorialFinished != value)
			{
				CustomPlayerPrefs.SetBool("is_vs_human_tutorial_finished", value);
			}
		}
	}

	public bool IsAppleGameTutorialFinished
	{
		get
		{
			return CustomPlayerPrefs.GetBool("is_apple_game_tutorial_finished");
		}
		set
		{
			if (IsAppleGameTutorialFinished != value)
			{
				CustomPlayerPrefs.SetBool("is_apple_game_tutorial_finished", value);
			}
		}
	}

	public bool IsBirdsGameTutorialFinished
	{
		get
		{
			return CustomPlayerPrefs.GetBool("is_birds_game_tutorial_finished");
		}
		set
		{
			if (IsBirdsGameTutorialFinished != value)
			{
				CustomPlayerPrefs.SetBool("is_birds_game_tutorial_finished", value);
			}
		}
	}

	private Tutorial CachedTutorial
	{
		get
		{
			if (cachedTutorial == null)
			{
				GameScreen gameScreen = CachedManagers.CachedGUIScreenManager.GameScreen;
				if (gameScreen != null)
				{
					cachedTutorial = gameScreen.Tutorial;
					cachedTutorial.gameObject.SetActive(true);
				}
			}
			return cachedTutorial;
		}
	}

	public int WinsToDisableEasyMatches
	{
		get
		{
			return winsToDisableEasyMatches;
		}
	}

	public int MatchesWithLowHealthEnemies
	{
		get
		{
			return matchesWithLowHealthEnemies;
		}
	}

	public float LowHealthEnemiesCoeff
	{
		get
		{
			return lowHealthEnemiesCoeff;
		}
	}

	public int MatchesForCoinsReminder
	{
		get
		{
			return CustomPlayerPrefs.GetInt("matches_for_coins_reminder", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("matches_for_coins_reminder", value);
		}
	}

	private bool ShouldCoinsSpendReminderLoad
	{
		get
		{
			bool flag = false;
			int matchesForCoinsReminder = MatchesForCoinsReminder;
			flag |= matchesForCoinsReminder == matchesForFirstReminder;
			return flag | ((matchesForCoinsReminder - matchesForFirstReminder) % matchesForRepeatReminder == 0);
		}
	}

	public bool IsTutorialMatchesEnded
	{
		get
		{
			bool flag = CachedManagers.CachedStatisticsManager.MathesCount >= tutorialMatchesCount;
			if (flag && !isTutorialMatchesEndedEventSended)
			{
				isTutorialMatchesEndedEventSended = true;
				if (TutorialManager.OnTutorialMatchesEnded != null)
				{
					TutorialManager.OnTutorialMatchesEnded();
				}
			}
			return flag;
		}
	}

	public bool IsFreeChestsEnded
	{
		get
		{
			return CachedManagers.CachedStatisticsManager.OpenedChestsCount >= freeChestsCount;
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

	private LevelsManager CachedLevelsManager
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

	private ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = CachedLevelsManager.ObjectsSpawner;
			}
			return objectsSpawner;
		}
	}

	private ShopItemCharacter AvailableCharacterWithMaxUpgradePrice
	{
		get
		{
			if (availablecharacterWithMaxUpgradePrice == null)
			{
				for (int i = 0; i < CachedManagers.CachedShopManager.AvailableCharacters.Count; i++)
				{
					if (CachedManagers.CachedShopManager.AvailableCharacters[i].IsUpgradeAvailable && CachedManagers.CachedShopManager.AvailableCharacters[i].Availible)
					{
						int num = ((!(availablecharacterWithMaxUpgradePrice == null)) ? availablecharacterWithMaxUpgradePrice.NextLevelCharacterUpgradeInfo.priceGems : 0);
						if (CachedManagers.CachedShopManager.AvailableCharacters[i].NextLevelCharacterUpgradeInfo.priceGems > num && CachedManagers.CachedShopManager.AvailableCharacters[i].NextLevelCharacterUpgradeInfo.priceGems <= CachedManagers.CachedStatisticsManager.Gems)
						{
							availablecharacterWithMaxUpgradePrice = CachedManagers.CachedShopManager.AvailableCharacters[i];
							GemsTutorialCharacterIndex = i;
						}
					}
				}
			}
			return availablecharacterWithMaxUpgradePrice;
		}
	}

	private ShopItemCharacter CharacterWithMinUpgradePrice
	{
		get
		{
			if (characterWithMinUpgradePrice == null)
			{
				for (int i = 0; i < CachedManagers.CachedShopManager.AvailableCharacters.Count; i++)
				{
					if (CachedManagers.CachedShopManager.AvailableCharacters[i].IsUpgradeAvailable && CachedManagers.CachedShopManager.AvailableCharacters[i].Availible)
					{
						if (characterWithMinUpgradePrice == null)
						{
							characterWithMinUpgradePrice = CachedManagers.CachedShopManager.AvailableCharacters[i];
						}
						else
						{
							characterWithMinUpgradePrice = ((CachedManagers.CachedShopManager.AvailableCharacters[i].NextLevelCharacterUpgradeInfo.priceGems >= characterWithMinUpgradePrice.NextLevelCharacterUpgradeInfo.priceGems) ? characterWithMinUpgradePrice : CachedManagers.CachedShopManager.AvailableCharacters[i]);
						}
					}
				}
			}
			return characterWithMinUpgradePrice;
		}
	}

	private int GemsTutorialCharacterIndex { get; set; }

	public List<TutorialType> UnlockedActivations
	{
		get
		{
			return unlockedActivations;
		}
	}

	public static event Action OnTutorialMatchesEnded
	{
		add
		{
			Action action = TutorialManager.m_OnTutorialMatchesEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TutorialManager.m_OnTutorialMatchesEnded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = TutorialManager.m_OnTutorialMatchesEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TutorialManager.m_OnTutorialMatchesEnded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		LoadUnlockedActivations();
	}

	private void OnEnable()
	{
		Tutorial.OnTutorialWillClose += Tutorial_OnTutorialWillClose;
		Tutorial.OnTutorialClose += Tutorial_OnTutorialClose;
		GameManager.OnGameStart += GameManager_OnGameStart;
		Level.OnPlayerChanged += Level_OnPlayerChanged;
		TouchManager.TouchBeganEvent += TouchManager_TouchBeganEvent;
		TouchManager.TouchEndedEvent += TouchManager_TouchEndedEvent;
		TouchManager.TouchMoveEvent += TouchManager_TouchMoveEvent;
		TouchManager.TouchHoldEvent += TouchManager_TouchHoldEvent;
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnGemsChanged;
	}

	private void OnDisable()
	{
		Tutorial.OnTutorialWillClose -= Tutorial_OnTutorialWillClose;
		Tutorial.OnTutorialClose -= Tutorial_OnTutorialClose;
		GameManager.OnGameStart -= GameManager_OnGameStart;
		Level.OnPlayerChanged -= Level_OnPlayerChanged;
		TouchManager.TouchBeganEvent -= TouchManager_TouchBeganEvent;
		TouchManager.TouchEndedEvent -= TouchManager_TouchEndedEvent;
		TouchManager.TouchMoveEvent -= TouchManager_TouchMoveEvent;
		TouchManager.TouchHoldEvent -= TouchManager_TouchHoldEvent;
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnGemsChanged;
	}

	public void UpdateTutorialManager(float deltaTime)
	{
		Level currentLevel = CachedLevelsManager.CurrentLevel;
		if (currentLevel.CurrentWeapon != null && currentLevel.CurrentWeapon.TutorialType != TutorialType.Aiming && !currentLevel.ActiveCharacter.IsAI && !currentLevel.ActiveCharacter.IsMultiplayerEnemy && !currentLevel.IsLevelFinished)
		{
			playerInactivityTime += deltaTime;
			if (playerInactivityTime >= hintDelayByInactivity && currentLevel.ActiveCharacter.Health > 0f)
			{
				playerInactivityTime = 0f;
				CachedTutorial.ShowTutorialHint(currentLevel.CurrentWeapon.TutorialType);
			}
		}
	}

	public void WeaponShoot()
	{
		if (cachedTutorial != null)
		{
			CachedTutorial.HideTutorialHint();
		}
	}

	public void ShowFirstTutorialAnimation(TutorialType type)
	{
		GameType gameType = CachedLevelsManager.CurrentLevel.GameType;
		if (gameType != GameType.Multiplayer && (!unlockedActivations.Contains(type) || isAimingTutorialNeed || (gameType == GameType.VSHuman && !IsVSHumanTutorialFinished)))
		{
			if (gameType == GameType.VSHuman)
			{
				IsVSHumanTutorialFinished = true;
			}
			ShowTutorialAnimation(type, false);
			isAimingTutorialNeed = false;
		}
	}

	public void ShowTutorialAnimation(TutorialType type, bool isCloseButtonImmediately = true)
	{
		CachedManagers.CachedGameManager.IsGamePaused = true;
		curTutorialType = type;
		CachedTutorial.ShowTutorialAnimation(type, isCloseButtonImmediately);
	}

	public void WeaponWasActivated(WeaponGeneral weapon)
	{
		if (weapon.TutorialType == TutorialType.Aiming)
		{
			return;
		}
		switch (CachedManagers.CachedGameManager.CurrentGameType)
		{
		case GameType.VSComputer:
			wasWeaponActivated = true;
			firstPlayerShotsWithoutActivation = 0;
			break;
		case GameType.VSHuman:
		{
			wasWeaponActivated = true;
			Level currentLevel = CachedLevelsManager.CurrentLevel;
			if (currentLevel.ActiveCharacter == currentLevel.CharacterFirst)
			{
				firstPlayerShotsWithoutActivation = 0;
			}
			else
			{
				secondPlayerShotsWithoutActivation = 0;
			}
			break;
		}
		case GameType.Multiplayer:
		{
			wasWeaponActivated = true;
			LevelMultiplayer levelMultiplayer = CachedLevelsManager.CurrentLevel as LevelMultiplayer;
			if (levelMultiplayer.ActiveCharacter == levelMultiplayer.LocalPlayerCharacter)
			{
				if (levelMultiplayer.LocalPlayerCharacter == levelMultiplayer.CharacterFirst)
				{
					firstPlayerShotsWithoutActivation = 0;
				}
				else
				{
					secondPlayerShotsWithoutActivation = 0;
				}
			}
			break;
		}
		}
	}

	public void WeaponWasDisabled(WeaponGeneral weapon)
	{
		if (IsVSCompTutorialInProgress || IsAppleGameTutorialInProgress || IsBirdsGameTutorialInProgress || weapon.TutorialType == TutorialType.Aiming || wasWeaponActivated || weapon.IsEmittedWeapon)
		{
			return;
		}
		GameType currentGameType = CachedManagers.CachedGameManager.CurrentGameType;
		Level currentLevel = CachedLevelsManager.CurrentLevel;
		if (currentGameType == GameType.VSComputer && !currentLevel.ActiveCharacter.IsAI)
		{
			firstPlayerShotsWithoutActivation++;
			return;
		}
		switch (currentGameType)
		{
		case GameType.VSHuman:
			if (currentLevel.ActiveCharacter == currentLevel.CharacterFirst)
			{
				firstPlayerShotsWithoutActivation++;
			}
			else
			{
				secondPlayerShotsWithoutActivation++;
			}
			break;
		case GameType.Multiplayer:
		{
			LevelMultiplayer levelMultiplayer = currentLevel as LevelMultiplayer;
			if (levelMultiplayer.ActiveCharacter == levelMultiplayer.LocalPlayerCharacter)
			{
				if (levelMultiplayer.LocalPlayerCharacter == levelMultiplayer.CharacterFirst)
				{
					firstPlayerShotsWithoutActivation++;
				}
				else
				{
					secondPlayerShotsWithoutActivation++;
				}
			}
			break;
		}
		}
	}

	public ShopItemCharacter LoadBuyCharacterOnCoinsTutorial(MenuScreenScrollView screen)
	{
		List<ShopItemCharacter> availableCharacters = CachedManagers.CachedShopManager.AvailableCharacters;
		StatisticsManager cachedStatisticsManager = CachedManagers.CachedStatisticsManager;
		ShopItemCharacter shopItemCharacter = null;
		int characterIndex = 0;
		if (screen != null)
		{
			isGemsTutorial = false;
			if (!WasCharacterUpgradedOnGems || ShouldCoinsSpendReminderLoad)
			{
				shopItemCharacter = AvailableCharacterWithMaxUpgradePrice;
				characterIndex = GemsTutorialCharacterIndex;
				isGemsTutorial = shopItemCharacter != null;
			}
			if (shopItemCharacter == null)
			{
				if (!WasCharacterBoughtOnCoins)
				{
					int i = 0;
					for (int count = availableCharacters.Count; i < count; i++)
					{
						ShopItemCharacter shopItemCharacter2 = availableCharacters[i];
						if (!shopItemCharacter2.IsPremium && cachedStatisticsManager.Coins >= shopItemCharacter2.PriceCoins && !shopItemCharacter2.Availible && !shopItemCharacter2.IsIncludedInSubscription)
						{
							shopItemCharacter = shopItemCharacter2;
							characterIndex = i;
							break;
						}
					}
				}
				else if (ShouldCoinsSpendReminderLoad)
				{
					int j = 0;
					for (int count2 = availableCharacters.Count; j < count2; j++)
					{
						ShopItemCharacter shopItemCharacter3 = availableCharacters[j];
						if (!shopItemCharacter3.IsPremium && cachedStatisticsManager.Coins >= shopItemCharacter3.PriceCoins && !shopItemCharacter3.Availible && !shopItemCharacter3.IsIncludedInSubscription && (shopItemCharacter == null || shopItemCharacter.PriceCoins < shopItemCharacter3.PriceCoins))
						{
							shopItemCharacter = shopItemCharacter3;
							characterIndex = j;
						}
					}
				}
			}
			if (shopItemCharacter != null)
			{
				CharacterNameType characterNameType = shopItemCharacter.Infos[0].characterNameType;
				GUICharacter gUICharacter = ObjectsSpawner.GetGUICharacter(characterNameType);
				if (gUICharacter.ShopIconCheckTransform != null)
				{
					LoadBuyCharacterOnCoinsTutorial(characterIndex, gUICharacter.ShopIconCheckTransform, screen);
				}
				return shopItemCharacter;
			}
			StopBuyCharacterOnCoinsTutorial();
		}
		return null;
	}

	public void StopBuyCharacterOnCoinsTutorial()
	{
		if (tutorialCharacterIcon != null)
		{
			for (int i = 0; i < tutorialCharacterIcon.Length; i++)
			{
				UnityEngine.Object.Destroy(tutorialCharacterIcon[i]);
			}
			tutorialCharacterIcon = null;
		}
		if (tutorialCoinsButton != null)
		{
			for (int j = 0; j < tutorialCoinsButton.Length; j++)
			{
				UnityEngine.Object.Destroy(tutorialCoinsButton[j]);
			}
			tutorialCoinsButton = null;
		}
	}

	public void RefreshTutorial()
	{
		MatchesForCoinsReminder = 0;
		availablecharacterWithMaxUpgradePrice = null;
		characterWithMinUpgradePrice = null;
	}

	public void AddWeaponActivation(TutorialType tutorialType)
	{
		if (!unlockedActivations.Contains(tutorialType))
		{
			unlockedActivations.Add(tutorialType);
			CustomPlayerPrefs.SetString("unlocked_activations", Json.Serialize(unlockedActivations));
		}
	}

	public void ForceTutorialDone()
	{
		isVSCompTutorialInProgress = false;
		IsVSCompTutorialFinished = true;
		isAimingTutorialNeed = false;
		turnsForShootingAfterTutorial = 0;
	}

	private void LoadUnlockedActivations()
	{
		string text = CustomPlayerPrefs.GetString("unlocked_activations");
		if (!string.IsNullOrEmpty(text))
		{
			unlockedActivations = Json.Deserialize<List<TutorialType>>(text);
			return;
		}
		unlockedActivations = new List<TutorialType>();
		unlockedActivations.Add(TutorialType.None);
		CustomPlayerPrefs.SetString("unlocked_activations", Json.Serialize(unlockedActivations));
	}

	private void LoadBuyCharacterOnCoinsTutorial(int characterIndex, Transform[] shopIconCheckTransform, MenuScreenScrollView screen)
	{
		if (((WasCharacterBoughtOnCoins && !ShouldCoinsSpendReminderLoad) || isGemsTutorial) && !isGemsTutorial)
		{
			return;
		}
		if (tutorialCharacterIcon != null)
		{
			for (int i = 0; i < tutorialCharacterIcon.Length; i++)
			{
				UnityEngine.Object.Destroy(tutorialCharacterIcon[i]);
			}
			tutorialCharacterIcon = null;
		}
		if (tutorialCoinsButton != null)
		{
			for (int j = 0; j < tutorialCoinsButton.Length; j++)
			{
				UnityEngine.Object.Destroy(tutorialCoinsButton[j]);
			}
			tutorialCoinsButton = null;
		}
		if (shopIconCheckTransform.Length > 0)
		{
			tutorialCharacterIcon = new GameObject[shopIconCheckTransform.Length];
			for (int k = 0; k < tutorialCharacterIcon.Length; k++)
			{
				tutorialCharacterIcon[k] = CreateBuyCharacterOnCoinsTutorial(shopIconCheckTransform[k].localPosition * 1.4f);
				tutorialCharacterIcon[k].transform.parent = shopIconCheckTransform[k].parent;
				Color color = tutorialCharacterIcon[k].GetComponent<tk2dSprite>().color;
				color.a = shopIconCheckTransform[k].parent.GetComponentInChildren<tk2dSprite>().color.a;
				tutorialCharacterIcon[k].gameObject.GetComponent<tk2dSprite>().color = color;
			}
			tk2dUIItem[] array = ((!isGemsTutorial) ? screen.BuyCharacterOnCoinsButtons : screen.UpgradeButtons);
			tutorialCoinsButton = new GameObject[array.Length];
			for (int l = 0; l < tutorialCoinsButton.Length; l++)
			{
				tutorialCoinsButton[l] = CreateBuyCharacterOnCoinsTutorial(shopIconCheckTransform[l].localPosition * 1.4f);
				tutorialCoinsButton[l].transform.parent = array[l].transform;
			}
			tutorialCharacterIndex = characterIndex;
			IsTutorialCharacterSelected = tutorialCharacterIndex == screen.SelectedIndex;
		}
	}

	private GameObject CreateBuyCharacterOnCoinsTutorial(Vector3 offset)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(buyCharacterTutorialPrefab);
		gameObject.transform.localPosition = Vector3.zero;
		TweenPosition component = gameObject.GetComponent<TweenPosition>();
		if (component != null)
		{
			component.beginPosition += offset;
			component.endPosition += offset;
			component.enabled = true;
		}
		return gameObject;
	}

	private void Tutorial_OnTutorialWillClose()
	{
		if (!unlockedActivations.Contains(curTutorialType))
		{
			unlockedActivations.Add(curTutorialType);
			CustomPlayerPrefs.SetString("unlocked_activations", Json.Serialize(unlockedActivations));
		}
	}

	private void Tutorial_OnTutorialClose()
	{
		CachedManagers.CachedGameManager.IsGamePaused = false;
	}

	private void GameManager_OnGameStart()
	{
		wasWeaponActivated = false;
		firstPlayerShotsWithoutActivation = 0;
		secondPlayerShotsWithoutActivation = 0;
		playerInactivityTime = 0f;
		switch (CachedManagers.CachedGameManager.CurrentGameType)
		{
		case GameType.VSComputer:
			IsVSCompTutorialInProgress = true;
			break;
		case GameType.AppleShoot:
			IsAppleGameTutorialInProgress = true;
			break;
		case GameType.BirdsHunt:
			IsBirdsGameTutorialInProgress = true;
			break;
		}
	}

	private void GameManager_OnGameStop()
	{
		switch (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType)
		{
		case GameType.VSComputer:
			isVSCompTutorialInProgress = false;
			break;
		case GameType.AppleShoot:
			isAppleGameTutorialInProgress = false;
			break;
		case GameType.BirdsHunt:
			isBirdsGameTutorialInProgress = false;
			break;
		}
	}

	private void Level_OnPlayerChanged(int player)
	{
		wasWeaponActivated = false;
		Level currentLevel = CachedLevelsManager.CurrentLevel;
		if (!currentLevel.IsLevelFinished && !currentLevel.InactiveArcher.IsFinishHim)
		{
			if (player == 1)
			{
				if (firstPlayerShotsWithoutActivation >= hintDelayByNoActivationShoot && currentLevel.CharacterFirst.Health > 0f)
				{
					CachedTutorial.ShowTutorialHint(ObjectsSpawner.GetWeapon(currentLevel.ActiveCharacter.AvailableWeapons[0]).TutorialType);
					firstPlayerShotsWithoutActivation = 0;
				}
			}
			else if ((currentLevel.GameType == GameType.VSHuman || currentLevel.GameType == GameType.Multiplayer) && secondPlayerShotsWithoutActivation >= hintDelayByNoActivationShoot && currentLevel.CharacterSecond.Health > 0f)
			{
				CachedTutorial.ShowTutorialHint(ObjectsSpawner.GetWeapon(currentLevel.ActiveCharacter.AvailableWeapons[0]).TutorialType);
				secondPlayerShotsWithoutActivation = 0;
			}
		}
		playerInactivityTime = 0f;
	}

	private void TouchManager_TouchBeganEvent(TouchInfo touch)
	{
		playerInactivityTime = 0f;
	}

	private void TouchManager_TouchEndedEvent(TouchInfo touch)
	{
		playerInactivityTime = 0f;
	}

	private void TouchManager_TouchMoveEvent(TouchInfo touch)
	{
		playerInactivityTime = 0f;
	}

	private void TouchManager_TouchHoldEvent(TouchInfo touch)
	{
		playerInactivityTime = 0f;
	}

	private void StatisticsManager_OnCoinsChanged(int coins)
	{
		if (CachedManagers.CachedStatisticsManager.Coins >= 500)
		{
			MenuScreen menuScreen = CachedManagers.CachedGUIScreenManager.MenuScreen;
			if (menuScreen != null)
			{
				LoadBuyCharacterOnCoinsTutorial(menuScreen.MenuScreenScrollView);
			}
		}
	}

	private void StatisticsManager_OnGemsChanged(int gems)
	{
		if (CharacterWithMinUpgradePrice != null && CachedManagers.CachedStatisticsManager.Gems >= CharacterWithMinUpgradePrice.NextLevelCharacterUpgradeInfo.priceGems)
		{
			MenuScreen menuScreen = CachedManagers.CachedGUIScreenManager.MenuScreen;
			if (menuScreen != null)
			{
				availablecharacterWithMaxUpgradePrice = null;
				LoadBuyCharacterOnCoinsTutorial(menuScreen.MenuScreenScrollView);
			}
		}
	}

	private void Tutorial_OnCongratulationsDisappear()
	{
		CachedManagers.CachedGameManager.IsGamePaused = false;
		Tutorial.OnCongratulationsDisappear -= Tutorial_OnCongratulationsDisappear;
	}
}
