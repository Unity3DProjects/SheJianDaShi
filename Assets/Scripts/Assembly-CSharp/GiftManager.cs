using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;

public class GiftManager : SingletonMonoBehaviour<GiftManager>
{
	public const string LAST_GIFT_DATE = "GiftDate";

	private const string LAST_GIFT_NUMBER = "GiftNumber";

	private const string LAST_GIFT_STRUCT = "GiftStruct";

	private const string MISTERY_BOX_NUMBER = "MisteryBoxNumer";

	public const int DAY_BETWEENGIFT = 1;

	public const int DAY_BETWEENSUPERGIFT = 1;

	private const string DATE_FORMAT = "yyyy-MM-dd";

	private const string DAYS = "ui.menu.days";

	private const string HOURS = "ui.menu.hours";

	private const string MINUTES = "ui.menu.minutes";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<TimeSpan> m_OnTimeForGiftChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnGiftAvailable;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnInviteCharacterOpen;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<ShopItemCharacter> m_OnSpecialGiftCharacterOpen;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnGiftGemsOpen;

	[SerializeField]
	private GiftsList giftsList;

	[SerializeField]
	private CharacterNameType videoGiftCharacter;

	[SerializeField]
	private CharacterNameType multiplayerGiftCharacter;

	[SerializeField]
	private CharacterNameType tournamentGiftCharacter;

	[SerializeField]
	private List<GiftInviteCharacter> inviteGiftCharacters;

	[SerializeField]
	private List<GiftSubscribeCharacter> subscribeGiftCharacters;

	[SerializeField]
	private List<LockedByTimerCharacter> lockedByTimerCharacters;

	[SerializeField]
	private GiftInviteGems giftInviteGems;

	[SerializeField]
	private GiftSubscribeGems giftSubscribeGems;

	[SerializeField]
	private int giftAdsGems = 1;

	[SerializeField]
	private int videosForGiftCharacter = 15;

	[SerializeField]
	private int multiplayerWinsForGiftCharacter = 10;

	[SerializeField]
	private int legendaryTournamenWinsForGiftCharacter = 1;

	[SerializeField]
	private List<MisteryBoxReward> firstRewards;

	[SerializeField]
	[Range(0f, 1f)]
	private float gemsRewardChanse;

	[SerializeField]
	private int gemsReward;

	[SerializeField]
	private int coinsReward;

	private GUITimer timer = new GUITimer();

	private int numberGift;

	private int numberStruct;

	private Managers cachedManagers;

	private ObjectsSpawner cachedObjectsSpawner;

	private StatisticsManager cachedStatisticsManager;

	private LockedByTimerCharacter currentLockedCharacter;

	private bool isCountdownActive;

	private float timeSinceLastSecond;

	private bool isUnlocked;

	protected Managers CachedManagers
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

	private ObjectsSpawner CachedObjectsSpawner
	{
		get
		{
			if (cachedObjectsSpawner == null)
			{
				cachedObjectsSpawner = CachedManagers.CachedLevelsManager.ObjectsSpawner;
			}
			return cachedObjectsSpawner;
		}
	}

	private StatisticsManager CachedStatisticsManager
	{
		get
		{
			if (cachedStatisticsManager == null)
			{
				cachedStatisticsManager = CachedManagers.CachedStatisticsManager;
			}
			return cachedStatisticsManager;
		}
	}

	public GiftInfoArray CurrentGiftInfoArray
	{
		get
		{
			GiftInfoArray result = default(GiftInfoArray);
			if (numberStruct < giftsList.infoList.Count)
			{
				return giftsList.infoList[numberStruct];
			}
			return result;
		}
	}

	public int NumberGift
	{
		get
		{
			return numberGift;
		}
	}

	public bool IsGiftAvailable
	{
		get
		{
			if (CachedStatisticsManager.OpenedChestsCount == 0)
			{
				return false;
			}
			return (DateTime.Now - DateLastGift).TotalDays >= 1.0;
		}
	}

	public TimeSpan TimeNeed
	{
		get
		{
			TimeSpan timeSpan = DateTime.Now - DateLastGift;
			timeSpan = TimeSpan.FromDays(1.0) - timeSpan;
			if (timeSpan < TimeSpan.Zero)
			{
				timeSpan = TimeSpan.Zero;
			}
			return timeSpan;
		}
	}

	public DateTime DateLastGift
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("GiftDate", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("GiftDate", value);
		}
	}

	public DateTime DateLastSuperGift
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_super_gift_date", DateTime.Now);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_super_gift_date", value);
		}
	}

	public DateTime DateNextGift
	{
		get
		{
			return DateLastGift + TimeSpan.FromDays(1.0);
		}
	}

	public CharacterNameType VideoGiftCharacter
	{
		get
		{
			return videoGiftCharacter;
		}
	}

	public CharacterNameType MultiplayerGiftCharacter
	{
		get
		{
			return multiplayerGiftCharacter;
		}
	}

	public CharacterNameType TournamentGiftCharacter
	{
		get
		{
			return tournamentGiftCharacter;
		}
	}

	public List<GiftInviteCharacter> InviteGiftCharacters
	{
		get
		{
			List<GiftInviteCharacter> list = new List<GiftInviteCharacter>();
			for (int i = 0; i < inviteGiftCharacters.Count; i++)
			{
				ShopItemCharacter shopItemCharacter = CachedObjectsSpawner.GetShopItemCharacter(inviteGiftCharacters[i].inviteCharacter);
				if (!shopItemCharacter.IsLockedByTimer)
				{
					list.Add(inviteGiftCharacters[i]);
				}
				else if (shopItemCharacter.IsTimeEnded)
				{
					list.Add(inviteGiftCharacters[i]);
				}
			}
			return list;
		}
	}

	public List<GiftSubscribeCharacter> SubscribeGiftCharacters
	{
		get
		{
			return subscribeGiftCharacters;
		}
	}

	public List<LockedByTimerCharacter> LockedByTimerCharacters
	{
		get
		{
			return lockedByTimerCharacters;
		}
	}

	public GiftInviteGems GiftInviteGems
	{
		get
		{
			return giftInviteGems;
		}
	}

	public GiftSubscribeGems GiftSubscribeGems
	{
		get
		{
			return giftSubscribeGems;
		}
	}

	public int GiftAdsGems
	{
		get
		{
			return giftAdsGems;
		}
	}

	public int VideosForGiftCharacter
	{
		get
		{
			return videosForGiftCharacter;
		}
	}

	public int MultiplayerWinsForGiftCharacter
	{
		get
		{
			return multiplayerWinsForGiftCharacter;
		}
	}

	public int LegendaryTournamenWinsForGiftCharacter
	{
		get
		{
			return legendaryTournamenWinsForGiftCharacter;
		}
	}

	public CharacterNameType LastOpenedSpecialGiftCharacter { get; set; }

	public LockedByTimerCharacter CurrentLockedCharacter
	{
		get
		{
			return currentLockedCharacter;
		}
		set
		{
			if (currentLockedCharacter.lockedCharacter != value.lockedCharacter)
			{
				currentLockedCharacter = value;
				IsUnlocked = false;
				SetTimer();
				SetItemState(CachedStatisticsManager.CheatedTime, CachedStatisticsManager.CurrentTimeState);
			}
		}
	}

	public string TimerInfo
	{
		get
		{
			return string.Format("{0}{3} : {1}{4} : {2}{5}", ((int)(timer.TimerInSeconds / 86400f)/*cast due to .constrained prefix*/).ToString(), (timer.TimerInSeconds / 3600f % 24f).ToString("00"), (timer.TimerInSeconds / 60f % 60f).ToString("00"), CachedManagers.CachedLocalisationManager.GetTextByKey("ui.menu.days"), CachedManagers.CachedLocalisationManager.GetTextByKey("ui.menu.hours"), CachedManagers.CachedLocalisationManager.GetTextByKey("ui.menu.minutes"));
		}
	}

	public DateTime UnlockedDate
	{
		get
		{
			ShopItemCharacter shopItemCharacter = CachedObjectsSpawner.GetShopItemCharacter(CurrentLockedCharacter.lockedCharacter);
			if (CurrentLockedCharacter.dateWhenUnlocked != null && shopItemCharacter.IsLockedByTimer)
			{
				return DateTime.ParseExact(CurrentLockedCharacter.dateWhenUnlocked, "yyyy-MM-dd", null);
			}
			return DateTime.Now;
		}
	}

	public bool IsUnlocked
	{
		get
		{
			return isUnlocked || IsCharacterLockTimeEnded;
		}
		private set
		{
			if (isUnlocked != value)
			{
				isUnlocked = value;
				TryRefreshMenuScreenBuyButtons();
			}
		}
	}

	public bool IsLockedByTimerCharacterAvaliable
	{
		get
		{
			return CachedManagers.CachedStatisticsManager.CurrentTimeState != TimeState.None && CachedManagers.CachedStatisticsManager.CurrentTimeState != TimeState.Undefined && IsUnlocked && IsCharacterLockTimeEnded;
		}
	}

	private bool IsCharacterLockTimeEnded
	{
		get
		{
			if (CurrentLockedCharacter.lockedCharacter != CharacterNameType.None)
			{
				ShopItemCharacter shopItemCharacter = CachedObjectsSpawner.GetShopItemCharacter(CurrentLockedCharacter.lockedCharacter);
				return shopItemCharacter.IsTimeEnded;
			}
			return false;
		}
	}

	private bool IsCountdownActive
	{
		get
		{
			return isCountdownActive;
		}
		set
		{
			if (isCountdownActive != value)
			{
				isCountdownActive = value;
				if (isCountdownActive && GiftManager.m_OnTimeForGiftChanged != null)
				{
					GiftManager.m_OnTimeForGiftChanged(TimeNeed);
				}
			}
		}
	}

	public int MisteryBoxNumber
	{
		get
		{
			return CustomPlayerPrefs.GetInt("mistery_box_number", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("mistery_box_number", value);
		}
	}

	public static event Action<TimeSpan> OnTimeForGiftChanged
	{
		add
		{
			Action<TimeSpan> action = GiftManager.m_OnTimeForGiftChanged;
			Action<TimeSpan> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnTimeForGiftChanged, (Action<TimeSpan>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<TimeSpan> action = GiftManager.m_OnTimeForGiftChanged;
			Action<TimeSpan> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnTimeForGiftChanged, (Action<TimeSpan>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnGiftAvailable
	{
		add
		{
			Action action = GiftManager.m_OnGiftAvailable;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnGiftAvailable, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GiftManager.m_OnGiftAvailable;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnGiftAvailable, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnInviteCharacterOpen
	{
		add
		{
			Action action = GiftManager.m_OnInviteCharacterOpen;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnInviteCharacterOpen, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GiftManager.m_OnInviteCharacterOpen;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnInviteCharacterOpen, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<ShopItemCharacter> OnSpecialGiftCharacterOpen
	{
		add
		{
			Action<ShopItemCharacter> action = GiftManager.m_OnSpecialGiftCharacterOpen;
			Action<ShopItemCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnSpecialGiftCharacterOpen, (Action<ShopItemCharacter>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ShopItemCharacter> action = GiftManager.m_OnSpecialGiftCharacterOpen;
			Action<ShopItemCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnSpecialGiftCharacterOpen, (Action<ShopItemCharacter>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnGiftGemsOpen
	{
		add
		{
			Action action = GiftManager.m_OnGiftGemsOpen;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnGiftGemsOpen, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GiftManager.m_OnGiftGemsOpen;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GiftManager.m_OnGiftGemsOpen, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		numberGift = CustomPlayerPrefs.GetInt("GiftNumber", 0);
		numberStruct = CustomPlayerPrefs.GetInt("GiftStruct", 0);
		IsCountdownActive = true;
		base.Awake();
	}

	private void OnEnable()
	{
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		timer.OnTimerChanged += Timer_OnTimerChanged;
		TapToStartScreen.OnLoaderRemoved += TapToStartScreen_OnLoaderRemoved;
	}

	private void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		timer.OnTimerChanged -= Timer_OnTimerChanged;
		TapToStartScreen.OnLoaderRemoved -= TapToStartScreen_OnLoaderRemoved;
		StopAllCoroutines();
	}

	private void Update()
	{
		if (isCountdownActive)
		{
			timeSinceLastSecond += Time.unscaledDeltaTime;
			if (timeSinceLastSecond >= 1f)
			{
				timeSinceLastSecond -= 1f;
				if (IsGiftAvailable)
				{
					GiftManager.m_OnGiftAvailable?.Invoke();
					isCountdownActive = false;
				}
				else GiftManager.m_OnTimeForGiftChanged?.Invoke(TimeNeed);
			}
		}
		timer.UpdateTimer(Time.unscaledDeltaTime);
	}

	public void GiftOpen(Action callback)
	{
		if (IsGiftAvailable)
		{
			CachedManagers.CachedNotifyManager.UpdateNotifications(0f, string.Empty);
			DateLastGift = DateTime.Now;
			numberGift++;
			numberStruct = (int)Math.Truncate((float)numberGift / (float)giftsList.infoList[numberStruct].giftInfos.Length);
			numberStruct = Mathf.Clamp(numberStruct, 0, giftsList.infoList.Count - 1);
			CustomPlayerPrefs.SetInt("GiftStruct", numberStruct);
			CustomPlayerPrefs.SetInt("GiftNumber", numberGift);
			IsCountdownActive = true;
			CachedManagers.CachedProposalManager.GenerateProposalMenu();
			CachedStatisticsManager.GiftsOpened++;
			Analytics.CustomEvent("Gift_Opened", new Dictionary<string, object>
			{
				{ "GiftsCount", CachedStatisticsManager.GiftsOpened },
				{ "MatchesCount", CachedStatisticsManager.MathesCount },
				{
					"GameTimeMinutes",
					CachedStatisticsManager.TotalGameTime.Minutes
				}
			});
		}
		if (callback != null)
		{
			callback();
		}
	}

	public void OpenSpecialGiftCharacter(ShopItemCharacter character)
	{
		character.OpenCharacter(true, true);
		LastOpenedSpecialGiftCharacter = character.Infos[0].characterNameType;
		if (GiftManager.m_OnSpecialGiftCharacterOpen != null)
		{
			if (character.IsInviteReward && GiftManager.m_OnInviteCharacterOpen != null)
			{
				GiftManager.m_OnInviteCharacterOpen();
			}
			GiftManager.m_OnSpecialGiftCharacterOpen(character);
		}
	}

	public void OpenGiftGems(int gemsCount)
	{
		CachedStatisticsManager.AddGems(gemsCount);
		CachedStatisticsManager.FreeGemsReceived++;
		GiftManager.m_OnGiftGemsOpen?.Invoke();
		Analytics.CustomEvent("Gems_Invite");
	}

	public void CheckLockedCharactersStates()
	{
		for (int i = 0; i < LockedByTimerCharacters.Count; i++)
		{
			ShopItemCharacter shopItemCharacter = CachedObjectsSpawner.GetShopItemCharacter(LockedByTimerCharacters[i].lockedCharacter);
			if (DateTime.ParseExact(LockedByTimerCharacters[i].dateWhenUnlocked, "yyyy-MM-dd", null) <= DateTime.Now && !shopItemCharacter.IsTimeEnded)
			{
				shopItemCharacter.IsTimeEnded = true;
			}
		}
	}

	public void GenerateMisteryBoxAward(bool isOriginalMisteryBox = false)
	{
		int misteryBoxNumber = CachedManagers.CachedGiftManager.MisteryBoxNumber;
		bool flag;
		int reward;
		if (misteryBoxNumber < firstRewards.Count)
		{
			flag = firstRewards[misteryBoxNumber].isCoinsReward;
			reward = firstRewards[misteryBoxNumber].reward;
		}
		else
		{
			flag = UnityEngine.Random.value >= gemsRewardChanse;
			reward = ((!flag) ? gemsReward : coinsReward);
		}
		CachedManagers.CachedGiftManager.MisteryBoxNumber++;
		CachedManagers.CachedGameManager.IsGamePaused = true;
		(CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MisteryBoxReward) as MisteryBoxRewardPopup).Init(flag, reward, isOriginalMisteryBox);
	}

	private void SetTimer()
	{
		if (CachedObjectsSpawner.GetShopItemCharacter(CurrentLockedCharacter.lockedCharacter).IsLockedByTimer)
		{
			if (!IsUnlocked)
			{
				timer.InitializeTimer((float)(UnlockedDate - DateTime.Now).TotalSeconds, true);
			}
			else
			{
				TryRefreshMenuScreenBuyButtons();
			}
		}
	}

	private void SetItemState(TimeSpan cheatedTime, TimeState timeState)
	{
		switch (timeState)
		{
		case TimeState.Normal:
			IsUnlocked = UnlockedDate < DateTime.Now;
			break;
		case TimeState.Cheated:
		{
			float num = (float)(UnlockedDate - DateTime.Now).TotalSeconds + (float)cheatedTime.TotalSeconds;
			if (num > 0f)
			{
				timer.TimerInSeconds = num;
				timer.UpdateTimer(Time.unscaledDeltaTime);
				IsUnlocked = false;
			}
			else
			{
				IsUnlocked = UnlockedDate < DateTime.Now;
			}
			break;
		}
		case TimeState.None:
		case TimeState.Undefined:
			IsUnlocked = UnlockedDate < DateTime.Now;
			break;
		}
	}

	private void TryRefreshMenuScreenBuyButtons()
	{
		MenuScreen menuScreen = CachedManagers.CachedGUIScreenManager.MenuScreen;
		if (menuScreen != null)
		{
			menuScreen.MenuScreenScrollView.UpdateBuyButtons();
		}
	}

	private void Timer_OnTimerChanged(int seconds)
	{
		if (Mathf.CeilToInt(seconds % 60) == 0)
		{
			TryRefreshMenuScreenBuyButtons();
		}
		if (seconds <= 0 && !IsUnlocked && !IsCharacterLockTimeEnded)
		{
			StartCoroutine(CachedStatisticsManager.CheckServerTime(delegate(TimeSpan cheatedTime, TimeState timeState)
			{
				SetItemState(cheatedTime, timeState);
			}));
			TryRefreshMenuScreenBuyButtons();
		}
	}

	private void TapToStartScreen_OnLoaderRemoved()
	{
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 50, 50f, 50f), "+ day"))
		{
			DateLastGift -= TimeSpan.FromDays(1.0);
			GiftManager.m_OnGiftAvailable?.Invoke();
		}
		if (GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height - 50, 50f, 50f), "- day"))
		{
			DateLastGift += TimeSpan.FromDays(1.0);
			GiftManager.m_OnGiftAvailable?.Invoke();
		}
	}
}
