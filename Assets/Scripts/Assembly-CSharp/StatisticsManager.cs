using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using MiniJSON;
using UnityEngine;
using UnityEngine.Analytics;
using Debug = UnityEngine.Debug;

public class StatisticsManager : SingletonMonoBehaviour<StatisticsManager>
{
	private const string TOTAOL_SESSION_COUNT = "TotalSeesionCount";

	private const string SERVER_URL = "https://api.playgendary.com/v1/info/time?build=";

	private const string TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffZ";

	private const string TIME_KEY = "time";

	private const int userTimeInMinutesOffset = 5;

	public const int DAY_BETWEEN_ROULETTE_CHARACTERS = 1;

	private const string NEW_ADS_ADDED = "new_ads_added";

	private const SystemLanguage DEFAULT_LANGUAGE = SystemLanguage.Chinese;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<int> m_OnCoinsChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<int> m_OnScoreChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<int> m_OnGemsChanged;

	[SerializeField]
	private int minMatchesToShowPartyMastersAnnouncer;

	[SerializeField]
	private int matchesCountStepToShowPartyMastersAnnouncer;

	private int score;

	private DateTime gameStartTime = DateTime.Now;

	private TimeSpan cheatedTime;

	private TimeState currentTimeState;

	private int coinsFromLastGame;

	private int gemsFromLastGame;

	private List<int> openedGiftCharacterNumbers;

	private List<CharacterNameType> defeatedCharacters = new List<CharacterNameType>();

	private ObjectsSpawner objectsSpawner;

	private ShopManager cachedShopManager;

	private List<int> roulettePlayedCharacters;

	private ShopManager CachedShopManager
	{
		get
		{
			if (cachedShopManager == null)
			{
				cachedShopManager = SingletonMonoBehaviour<ShopManager>.Instance;
			}
			return cachedShopManager;
		}
	}

	public int Score
	{
		get
		{
			return score;
		}
		set
		{
			score = value;
			StatisticsManager.m_OnScoreChanged?.Invoke(score);
		}
	}

	public int Coins
	{
		get
		{
			return CustomPlayerPrefs.GetInt("Coins", 0);
		}
		set
		{
			if (Coins != value)
			{
				value = Mathf.Max(value, 0);
				CustomPlayerPrefs.SetInt("Coins", value, true);
				StatisticsManager.m_OnCoinsChanged?.Invoke(value);
				AudioManager.PlayAudioWithKey("coins_changing");
			}
		}
	}

	public int Gems
	{
		get
		{
			return CustomPlayerPrefs.GetInt("Gems", 0);
		}
		set
		{
			if (Gems != value)
			{
				value = Mathf.Max(value, 0);
				CustomPlayerPrefs.SetInt("Gems", value, true);
				StatisticsManager.m_OnGemsChanged?.Invoke(value);
				AudioManager.PlayAudioWithKey("gems_changing");
			}
		}
	}

	public int BirdsKilled
	{
		get
		{
			return CustomPlayerPrefs.GetInt("birds_killed", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("birds_killed", value);
		}
	}

	public int HighScoreBirdsKilled
	{
		get
		{
			return CustomPlayerPrefs.GetInt("highscore_birds_killed", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("highscore_birds_killed", value);
			SingletonMonoBehaviour<GameCenterManager>.Instance.ReportBirdsLeaderboardScore(value);
		}
	}

	public int FruitsSmashed
	{
		get
		{
			return CustomPlayerPrefs.GetInt("fruits_smashed", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("fruits_smashed", value);
		}
	}

	public int HighScoreFruitsSmashed
	{
		get
		{
			return CustomPlayerPrefs.GetInt("highscore_fruits_smashed", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("highscore_fruits_smashed", value);
			SingletonMonoBehaviour<GameCenterManager>.Instance.ReportApplesLeaderboardScore(value);
		}
	}

	public float TotalDamageApplied
	{
		get
		{
			return CustomPlayerPrefs.GetFloat("total_damage_applied", 0f);
		}
		set
		{
			CustomPlayerPrefs.SetFloat("total_damage_applied", value);
		}
	}

	public int MatchesCountVSHuman
	{
		get
		{
			return CustomPlayerPrefs.GetInt("matches_count_vs_human", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("matches_count_vs_human", value);
		}
	}

	public int MatchesCountMultiplayer
	{
		get
		{
			return CustomPlayerPrefs.GetInt("matches_count_multiplayer", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("matches_count_multiplayer", value);
		}
	}

	public int MatchesWinsMultiplayer
	{
		get
		{
			return CustomPlayerPrefs.GetInt("matches_wins_multiplayer", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("matches_wins_multiplayer", value);
			CheckMultiplayerCharacterUnlock();
		}
	}

	public int WinsCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("wins_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("wins_count", value);
			SingletonMonoBehaviour<AdjustManager>.Instance.TrackMatchWin(value);
			LastWinDate = DateTime.Now;
			SingletonMonoBehaviour<NotifyManager>.Instance.UpdateNotifications(0f, string.Empty);
			SingletonMonoBehaviour<GameCenterManager>.Instance.ReportWinsLeaderboardScore(value);
		}
	}

	public int LosesCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("loses_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("loses_count", value);
		}
	}

	public int MathesCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("matches_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("matches_count", value);
			CheckUnlockGameModes();
			SingletonMonoBehaviour<ProposalManager>.Instance.IsForcedPackAvailable = true;
			CheckVideoCharacterUnlock();
			SendMatchesEvents(value);
		}
	}

	public int MenuScreensCount { get; set; }

	public int UnlockedGameTypes
	{
		get
		{
			return CustomPlayerPrefs.GetInt("unlocked_game_types", SingletonMonoBehaviour<GameManager>.Instance.DefaultUnlockedGameType);
		}
		set
		{
			CustomPlayerPrefs.SetInt("unlocked_game_types", value);
		}
	}

	public int TotalSeesionCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("TotalSeesionCount", 0);
		}
		set
		{
			if (TotalSeesionCount != value)
			{
				CustomPlayerPrefs.SetInt("TotalSeesionCount", value);
				SendSessionEvents(value);
			}
		}
	}

	public int OpenedChestsCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("opened_chets", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("opened_chets", value);
			SingletonMonoBehaviour<MicroachievementsManager>.Instance.CheckLevelAchievementReaching(MicroachievementType.ComeBack, value);
			StatisticsManager instance = SingletonMonoBehaviour<StatisticsManager>.Instance;
			Analytics.CustomEvent("Chest_Opened", new Dictionary<string, object>
			{
				{ "ChestsCount", value },
				{
					"GameTimeMinutes",
					instance.TotalGameTime.Minutes
				},
				{ "DaysSinceFirstLaunch", instance.DaysSinceFirstLaunch }
			});
		}
	}

	public TimeSpan TotalGameTime
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("total_game_time", DateTime.MinValue) - DateTime.MinValue;
		}
		private set
		{
			CustomPlayerPrefs.SetDateTime("total_game_time", DateTime.MinValue + value);
		}
	}

	public TimeSpan GameTime
	{
		get
		{
			TimeSpan timeSpan = DateTime.Now - gameStartTime;
			if (timeSpan < TimeSpan.Zero)
			{
				gameStartTime = DateTime.Now;
				timeSpan = TimeSpan.Zero;
			}
			return timeSpan;
		}
	}

	public string LastCharacterID
	{
		get
		{
			return CustomPlayerPrefs.GetString("last_character");
		}
		set
		{
			CustomPlayerPrefs.SetString("last_character", value);
		}
	}

	public CharacterNameType LastCharacterNameType
	{
		get
		{
			CharacterNameType characterNameType = (CharacterNameType)CustomPlayerPrefs.GetInt("last_character_name_type", 0);
			if (characterNameType == CharacterNameType.None)
			{
				ShopItemCharacter shopItemCharacter = CachedShopManager.GetShopItemCharacter(LastCharacterID);
				if (shopItemCharacter == null)
				{
					shopItemCharacter = CachedShopManager.AvailableCharacters[0];
				}
				characterNameType = ((shopItemCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? shopItemCharacter.Infos[0].characterNameType : shopItemCharacter.CharacterUpgradeInfo.characterNameType);
			}
			return characterNameType;
		}
		set
		{
			CustomPlayerPrefs.SetInt("last_character_name_type", (int)value);
		}
	}

	public bool DoubleCoins
	{
		get
		{
			return CustomPlayerPrefs.GetBool("double_coins");
		}
		set
		{
			CustomPlayerPrefs.SetBool("double_coins", value);
		}
	}

	public string DeviceName
	{
		get
		{
			return "Androids";
		}
	}

	public DateTime LastDeviceTypeDate
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_device_type_date", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_device_type_date", value);
		}
	}

	public DateTime LastRealUtcDate
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_real_utc_date", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_real_utc_date", value);
		}
	}

	public int CountMatchesInDay
	{
		get
		{
			return CustomPlayerPrefs.GetInt("count_matches_in_day", 0);
		}
		set
		{
			DateTime now = DateTime.Now;
			if (LastMatchesInDay.Day != now.Day)
			{
				CustomPlayerPrefs.SetInt("count_matches_in_day", 1);
			}
			else
			{
				CustomPlayerPrefs.SetInt("count_matches_in_day", value);
			}
			LastMatchesInDay = DateTime.Now;
		}
	}

	public DateTime LastMatchesInDay
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_matches_in_day", DateTime.Now);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_matches_in_day", value);
		}
	}

	public int VideosWatched
	{
		get
		{
			return CustomPlayerPrefs.GetInt("videos_watched", 0);
		}
		private set
		{
			CustomPlayerPrefs.SetInt("videos_watched", value);
			CheckVideoCharacterUnlock();
		}
	}

	public DateTime FirstLaunchTime
	{
		get
		{
			DateTime dateTime = CustomPlayerPrefs.GetDateTime("first_launch_time", DateTime.Now);
			if (dateTime == DateTime.Now)
			{
				CustomPlayerPrefs.SetDateTime("first_launch_time", DateTime.Now);
			}
			return dateTime;
		}
	}

	public int DaysSinceFirstLaunch
	{
		get
		{
			return (DateTime.Now - FirstLaunchTime).Days;
		}
	}

	public int DaysSinceLastFirstWin
	{
		get
		{
			return (DateTime.Now - LastFirstDailyWin).Days;
		}
	}

	public int DaysSinceLastNoAds
	{
		get
		{
			return (DateTime.Now - LastNoAds).Days;
		}
	}

	public int DaysSinceLastFoolishSubscribe
	{
		get
		{
			return (DateTime.Now - LastFoolishSubscribe).Days;
		}
	}

	public int DaysSinceLastPromoFetcher
	{
		get
		{
			return (DateTime.Now - LastPromoFetcher).Days;
		}
	}

	public int SecondsSinceLastMisteryBox
	{
		get
		{
			return (int)(DateTime.Now - LastMisteryBox).TotalSeconds;
		}
	}

	public DateTime LastWinDate
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_win_date", DateTime.Now.AddDays(-2.0));
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_win_date", value);
		}
	}

	public int TournamentEasyWin
	{
		get
		{
			return CustomPlayerPrefs.GetInt("tournament_easy_win", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("tournament_easy_win", value);
		}
	}

	public int TournamentMediumWin
	{
		get
		{
			return CustomPlayerPrefs.GetInt("tournament_medium_win", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("tournament_medium_win", value);
		}
	}

	public int TournamentHardWin
	{
		get
		{
			return CustomPlayerPrefs.GetInt("tournament_hard_win", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("tournament_hard_win", value);
			CheckTournamentCharacterUnlock();
		}
	}

	public int CoinsFromLastGame
	{
		get
		{
			return coinsFromLastGame;
		}
	}

	public int GemsFromLastGame
	{
		get
		{
			return gemsFromLastGame;
		}
	}

	public int CharactersBoughtOnBucks
	{
		get
		{
			return CustomPlayerPrefs.GetInt("characters_bought_on_bucks", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("characters_bought_on_bucks", value);
		}
	}

	public int GiftsOpened
	{
		get
		{
			return CustomPlayerPrefs.GetInt("gifts_opened", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("gifts_opened", value);
		}
	}

	public int SpentBucks
	{
		get
		{
			return CustomPlayerPrefs.GetInt("spent_bucks", 0);
		}
		set
		{
			if (value > 0)
			{
				SingletonMonoBehaviour<ShopManager>.Instance.NoAdsBought = true;
			}
			CustomPlayerPrefs.SetInt("spent_bucks", value);
		}
	}

	public int FreeChestsOpened
	{
		get
		{
			return CustomPlayerPrefs.GetInt("free_chests_opened", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("free_chests_opened", value);
		}
	}

	public List<int> OpenedGiftNumbers
	{
		get
		{
			openedGiftCharacterNumbers = new List<int>();
			for (int i = 0; i < SingletonMonoBehaviour<GiftManager>.Instance.NumberGift; i++)
			{
				if (CustomPlayerPrefs.GetInt("opened_gift_numbers" + i) != 0)
				{
					openedGiftCharacterNumbers.Add(i);
				}
			}
			return openedGiftCharacterNumbers;
		}
	}

	public int CurrentOpenedGiftNumber
	{
		set
		{
			CustomPlayerPrefs.SetInt("opened_gift_numbers" + value, value);
		}
	}

	public List<CharacterNameType> DefeatedCharacters
	{
		get
		{
			string text = CustomPlayerPrefs.GetString("defeated_characters_list", string.Empty);
			if (defeatedCharacters.Count == 0)
			{
				if (string.IsNullOrEmpty(text))
				{
					string text2 = CustomPlayerPrefs.GetString("defeated_characters_64", string.Empty);
					long num = ((!string.IsNullOrEmpty(text2)) ? long.Parse(text2) : CustomPlayerPrefs.GetInt("defeated_characters", 0));
					List<ShopItemCharacter> availableCharacters = SingletonMonoBehaviour<Managers>.Instance.CachedShopManager.AvailableCharacters;
					for (int i = 0; i < availableCharacters.Count; i++)
					{
						if ((num & (1L << i)) == 1)
						{
							defeatedCharacters.Add(availableCharacters[i].Infos[0].characterNameType);
						}
					}
				}
				else
				{
					defeatedCharacters = Json.Deserialize<List<CharacterNameType>>(text);
				}
			}
			return defeatedCharacters;
		}
		set
		{
			defeatedCharacters = value;
			string value2 = Json.Serialize(defeatedCharacters);
			CustomPlayerPrefs.SetString("defeated_characters_list", value2);
		}
	}

	public DateTime LastFirstDailyWin
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_first_daily_win_date", DateTime.Now.AddDays(-2.0));
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_first_daily_win_date", value);
		}
	}

	public DateTime LastNoAds
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_no_ads", DateTime.Now.AddDays(-2.0));
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_no_ads", value);
		}
	}

	public DateTime LastFoolishSubscribe
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_foolish_subscribe", DateTime.Now.AddDays(-2.0));
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_foolish_subscribe", value);
		}
	}

	public DateTime LastMisteryBox
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_mistery_box", DateTime.Now.AddDays(-2.0));
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_mistery_box", value);
		}
	}

	public DateTime LastPromoFetcher
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_promo_fetcher", DateTime.Now.AddDays(-2.0));
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_promo_fetcher", value);
		}
	}

	public ICLoudSaveType ICloudStore
	{
		get
		{
			return (ICLoudSaveType)CustomPlayerPrefs.GetInt("icloud_store", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("icloud_store", (int)value);
		}
	}

	public SystemLanguage CurrentLanguage
	{
		get
		{
			int num = CustomPlayerPrefs.GetInt(GamePrefs.LANGUAGE_TYPE, -1);
			if (num == -1)
			{
				CurrentLanguage = DEFAULT_LANGUAGE;
				return DEFAULT_LANGUAGE;
			}
			SystemLanguage systemLanguage = (SystemLanguage)num;
			if (!IsSupportedLanguage(systemLanguage))
			{
				CurrentLanguage = DEFAULT_LANGUAGE;
				return DEFAULT_LANGUAGE;
			}
			return systemLanguage;
		}
		set
		{
			CustomPlayerPrefs.SetInt(GamePrefs.LANGUAGE_TYPE, (int)value);
		}
	}

	private static bool IsSupportedLanguage(SystemLanguage language)
	{
		return language == SystemLanguage.English || language == SystemLanguage.Chinese || language == SystemLanguage.ChineseSimplified || language == SystemLanguage.ChineseTraditional;
	}

	public TimeSpan CheatedTime
	{
		get
		{
			return cheatedTime;
		}
		set
		{
			cheatedTime = value;
			CurrentTimeState = ((!(cheatedTime.TotalMinutes > 5.0)) ? TimeState.Normal : TimeState.Cheated);
		}
	}

	public int FreeGemsReceived
	{
		get
		{
			return CustomPlayerPrefs.GetInt("free_gems_received", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("free_gems_received", value);
		}
	}

	public TimeState CurrentTimeState { get; private set; }

	private ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner;
			}
			return objectsSpawner;
		}
	}

	public bool AreGemsBought
	{
		get
		{
			return CustomPlayerPrefs.GetBool("gems_bought");
		}
		set
		{
			CustomPlayerPrefs.SetBool("gems_bought", value);
		}
	}

	public DateTime LastRoulettePlayedDay
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_roulette_played_day", DateTime.Now);
		}
		set
		{
			DateTime dateTime = CustomPlayerPrefs.GetDateTime("last_roulette_played_day", DateTime.Now);
			if ((value - dateTime).TotalDays >= 1.0)
			{
				CustomPlayerPrefs.SetDateTime("last_roulette_played_day", value);
				RoulettePlayedCharacters = new List<int>();
			}
		}
	}

	public List<int> RoulettePlayedCharacters
	{
		get
		{
			if (roulettePlayedCharacters == null)
			{
				string value = CustomPlayerPrefs.GetString("roulette_played_characters", string.Empty);
				if (string.IsNullOrEmpty(value))
				{
					roulettePlayedCharacters = new List<int>();
				}
				else
				{
					roulettePlayedCharacters = Json.Deserialize<List<int>>(CustomPlayerPrefs.GetString("roulette_played_characters"));
				}
			}
			return roulettePlayedCharacters;
		}
		set
		{
			roulettePlayedCharacters = value;
			CustomPlayerPrefs.SetString("roulette_played_characters", Json.Serialize(roulettePlayedCharacters));
		}
	}

	public bool NewAdsAdded
	{
		get
		{
			return CustomPlayerPrefs.GetBool("new_ads_added");
		}
		set
		{
			CustomPlayerPrefs.SetBool("new_ads_added", value);
		}
	}

	public bool IsPartyMastersButtonClicked
	{
		get
		{
			return CustomPlayerPrefs.GetBool("party_masters_button_clicked");
		}
		set
		{
			CustomPlayerPrefs.SetBool("party_masters_button_clicked", value);
		}
	}

	public int LastMatchShowedPartyMastersAnnouncer
	{
		get
		{
			return CustomPlayerPrefs.GetInt("last_match_showed_party_masters_announcer", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("last_match_showed_party_masters_announcer", value);
		}
	}

	public bool IsNeedShowPartyMastersAnnouncer
	{
		get
		{
			bool flag = !IsPartyMastersButtonClicked;
			flag &= MathesCount > minMatchesToShowPartyMastersAnnouncer;
			return flag & (MathesCount - LastMatchShowedPartyMastersAnnouncer >= matchesCountStepToShowPartyMastersAnnouncer);
		}
	}

	public static event Action<int> OnCoinsChanged
	{
		add
		{
			Action<int> action = StatisticsManager.m_OnCoinsChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StatisticsManager.m_OnCoinsChanged, (Action<int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int> action = StatisticsManager.m_OnCoinsChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StatisticsManager.m_OnCoinsChanged, (Action<int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<int> OnScoreChanged
	{
		add
		{
			Action<int> action = StatisticsManager.m_OnScoreChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StatisticsManager.m_OnScoreChanged, (Action<int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int> action = StatisticsManager.m_OnScoreChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StatisticsManager.m_OnScoreChanged, (Action<int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<int> OnGemsChanged
	{
		add
		{
			Action<int> action = StatisticsManager.m_OnGemsChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StatisticsManager.m_OnGemsChanged, (Action<int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int> action = StatisticsManager.m_OnGemsChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StatisticsManager.m_OnGemsChanged, (Action<int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		LLFacebookManager.Initialize();
		LLTenjinManager.Initialize();
	}

	private void Start()
	{
		gameStartTime = DateTime.Now;
		// Debug.LogError($"使用语言：{CurrentLanguage}");
		LLTapjoyManager.Initialize();
		SingletonMonoBehaviour<LocalisationManager>.Instance.ChangeLanguage(CurrentLanguage);
		if (DateTime.Now.Subtract(LastDeviceTypeDate).Days >= 1)
		{
			Analytics.CustomEvent("Device_Type", new Dictionary<string, object> { { "DeviceType", DeviceName } });
			LastDeviceTypeDate = DateTime.Now;
		}
		int hour = DateTime.Now.Hour;
		string customEventName = string.Format("{0}{1}-{2}", "Heat_Map_", hour.ToString(), (hour + 1).ToString());
		Analytics.CustomEvent(customEventName);
	}

	private void OnEnable()
	{
		GameCenterManager.OnAuthentication += GameCenterManager_OnAuthentication;
		AdvertisingManager.OnVideoAdFinish += AdvertisingManager_OnVideoAdFinish;
		GameManager.OnGameStart += GameManager_OnGameStart;
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		StartCoroutine(CheckServerTime(null));
	}

	private void OnDisable()
	{
		GameCenterManager.OnAuthentication -= GameCenterManager_OnAuthentication;
		AdvertisingManager.OnVideoAdFinish -= AdvertisingManager_OnVideoAdFinish;
		GameManager.OnGameStart -= GameManager_OnGameStart;
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			TotalGameTime += GameTime;
		}
		else
		{
			gameStartTime = DateTime.Now;
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		StartCoroutine(CheckServerTime(null));
	}

	public void ResetStatistics()
	{
		Score = 0;
	}

	public void AddCoins(int value, bool isNotSavingMoney = true)
	{
		value = value / 5 * 5;
		int num = ((!DoubleCoins || !isNotSavingMoney) ? value : (value * 2));
		coinsFromLastGame += num;
		Coins += num;
	}

	public void DecreaseCoins(int value)
	{
		Coins -= value;
	}

	public void AddGems(int value)
	{
		gemsFromLastGame += value;
		Gems += value;
	}

	public void DecreaseGems(int value)
	{
		Gems -= value;
	}

	public bool IsUnlockedMode(GameType typeMode)
	{
		return ((uint)UnlockedGameTypes & (uint)typeMode) != 0;
	}

	public void CheckUnlockGameModes()
	{
		CheckUnlockMode(GameType.VSHuman);
		CheckUnlockMode(GameType.BirdsHunt);
		CheckUnlockMode(GameType.AppleShoot);
		CheckUnlockMode(GameType.Tournament);
		CheckUnlockMode(GameType.Multiplayer);
	}

	public void SendPurchaseEvent(string productID, string transactionId, float revenue)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("af_currency", "USD");
		dictionary.Add("af_revenue", revenue.ToString());
		dictionary.Add("af_quantity", "1");
		LLTenjinManager.LogPurchase(productID, "USD", 1, revenue.ToString(), transactionId);
	}

	public void SendEvent(string eventName, float? value = null)
	{
		if (!string.IsNullOrEmpty(eventName))
		{
			if (value.HasValue)
			{
				LLFacebookManager.LogEvent(eventName, value.Value);
				LLTenjinManager.LogEvent(eventName, value.ToString());
			}
			else
			{
				LLFacebookManager.LogEvent(eventName);
				LLTenjinManager.LogEvent(eventName, string.Empty);
			}
		}
	}

	private void CheckUnlockMode(GameType mode)
	{
		if (!IsUnlockedMode(mode))
		{
			int num = MathesCount - LosesCount - MatchesCountVSHuman;
			int num2 = 0;
			GameManager instance = SingletonMonoBehaviour<GameManager>.Instance;
			switch (mode)
			{
			case GameType.VSHuman:
				num2 = instance.VSHumanUnlockBorder;
				break;
			case GameType.BirdsHunt:
				num2 = instance.BirdsHuntUnlockBorder;
				break;
			case GameType.AppleShoot:
				num2 = instance.AppleShootUnlockBorder;
				break;
			case GameType.Tournament:
				num = WinsCount + MatchesCountVSHuman;
				num2 = instance.TournamentUnlockBorder;
				break;
			case GameType.Multiplayer:
				num2 = instance.MultiplayerUnlockBorder;
				break;
			}
			//TODO:修改--模式全解锁
			// if (num >= num2)
			// {
			// 	UnlockMode(mode);
			// }
			
				UnlockMode(mode);
			
		}
	}

	private void UnlockMode(GameType typeMode)
	{
		UnlockedGameTypes |= (int)typeMode;
		GUIScreenManager instance = SingletonMonoBehaviour<GUIScreenManager>.Instance;
		switch (typeMode)
		{
		case GameType.VSHuman:
			instance.IsNeedPlayUnlockVSHumanModeEffect = true;
			break;
		case GameType.BirdsHunt:
			instance.IsNeedPlayUnlockBirdHuntModeEffect = true;
			break;
		case GameType.AppleShoot:
			instance.IsNeedPlayUnlockAppleModeEffect = true;
			break;
		case GameType.Tournament:
			instance.IsNeedPlayUnlockTournamentModeEffect = true;
			break;
		case GameType.Multiplayer:
			instance.IsNeedPlayUnlockMultiplayerModeEffect = true;
			break;
		}
	}

	private void CheckVideoCharacterUnlock()
	{
		GiftManager instance = SingletonMonoBehaviour<GiftManager>.Instance;
		int videosWatched = VideosWatched;
		ShopItemCharacter shopItemCharacter = ObjectsSpawner.GetShopItemCharacter(instance.VideoGiftCharacter);
		if (videosWatched >= instance.VideosForGiftCharacter && shopItemCharacter != null && !shopItemCharacter.Availible)
		{
			instance.OpenSpecialGiftCharacter(shopItemCharacter);
		}
	}

	private void CheckMultiplayerCharacterUnlock()
	{
		GiftManager instance = SingletonMonoBehaviour<GiftManager>.Instance;
		int matchesWinsMultiplayer = MatchesWinsMultiplayer;
		ShopItemCharacter shopItemCharacter = ObjectsSpawner.GetShopItemCharacter(instance.MultiplayerGiftCharacter);
		if (matchesWinsMultiplayer >= instance.MultiplayerWinsForGiftCharacter && shopItemCharacter != null && !shopItemCharacter.Availible)
		{
			instance.OpenSpecialGiftCharacter(shopItemCharacter);
		}
	}

	private void CheckTournamentCharacterUnlock()
	{
		GiftManager instance = SingletonMonoBehaviour<GiftManager>.Instance;
		int tournamentHardWin = TournamentHardWin;
		ShopItemCharacter shopItemCharacter = ObjectsSpawner.GetShopItemCharacter(instance.TournamentGiftCharacter);
		if (tournamentHardWin >= instance.LegendaryTournamenWinsForGiftCharacter && shopItemCharacter != null && !shopItemCharacter.Availible)
		{
			instance.OpenSpecialGiftCharacter(shopItemCharacter);
		}
	}

	private void SendMatchesEvents(int matchesCount)
	{
		string eventName = string.Empty;
		switch (matchesCount)
		{
		case 1:
			eventName = "matches_count_1";
			break;
		case 3:
			eventName = "matches_count_3";
			break;
		case 7:
			eventName = "matches_count_7";
			break;
		case 15:
			eventName = "matches_count_15";
			break;
		case 30:
			eventName = "matches_count_30";
			break;
		case 60:
			eventName = "matches_count_60";
			break;
		}
		SendEvent(eventName);
		SendEvent("matches_count", matchesCount);
	}

	private void SendSessionEvents(int sessionCount)
	{
		string eventName = string.Empty;
		switch (sessionCount)
		{
		case 3:
			eventName = "session_count_3";
			break;
		case 10:
			eventName = "session_count_10";
			break;
		case 15:
			eventName = "session_count_15";
			break;
		case 20:
			eventName = "session_count_20";
			break;
		case 30:
			eventName = "session_count_30";
			break;
		}
		SendEvent(eventName);
	}

	private void GameCenterManager_OnAuthentication(bool isAuthenticated)
	{
		if (isAuthenticated)
		{
			SingletonMonoBehaviour<GameCenterManager>.Instance.ReportWinsLeaderboardScore(WinsCount);
			SingletonMonoBehaviour<GameCenterManager>.Instance.ReportApplesLeaderboardScore(HighScoreFruitsSmashed);
			SingletonMonoBehaviour<GameCenterManager>.Instance.ReportBirdsLeaderboardScore(HighScoreBirdsKilled);
		}
	}

	private void AdvertisingManager_OnVideoAdFinish(bool isSuccessful)
	{
		if (isSuccessful)
		{
			VideosWatched++;
		}
	}

	private void GameManager_OnGameStart()
	{
		coinsFromLastGame = 0;
	}

	public IEnumerator CheckServerTime(Action<TimeSpan, TimeState> callback)
	{
		WWW info = new WWW("https://api.playgendary.com/v1/info/time?build=" + Application.version);
		yield return info;
		if (info.error != null)
		{
			CurrentTimeState = TimeState.Undefined;
		}
		else
		{
			string text = JsonConvert.DeserializeObject<string>(info.text);
			if (text != null)
			{
				CheatedTime = DateTime.Now - DateTime.ParseExact(text, "yyyy-MM-ddTHH:mm:ss.fffZ", null);
				LastRealUtcDate = DateTime.UtcNow.Subtract(CheatedTime);
			}
			else
			{
				CurrentTimeState = TimeState.Undefined;
			}
		}
		if (callback != null)
		{
			callback(CheatedTime, CurrentTimeState);
		}
		if (CurrentTimeState == TimeState.Normal)
		{
			SingletonMonoBehaviour<GiftManager>.Instance.CheckLockedCharactersStates();
			MenuScreen menuScreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.MenuScreen;
			if (menuScreen != null)
			{
				menuScreen.UpdateProposals();
			}
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(Screen.width / 2, Screen.height - 50, 50f, 50f), "Unlock"))
		{
			UnlockMode((GameType)62);
			TournamentEasyWin = 10;
			TournamentMediumWin = 10;
			TournamentHardWin = 10;
		}
		if (GUI.Button(new Rect(Screen.width / 2, Screen.height - 100, 50f, 50f), "UnClicked"))
		{
			IsPartyMastersButtonClicked = false;
		}
	}
}
