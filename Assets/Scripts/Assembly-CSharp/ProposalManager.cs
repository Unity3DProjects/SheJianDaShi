using System;
using System.Collections;
using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class ProposalManager : SingletonMonoBehaviour<ProposalManager>
{
	public static Action OnRemoveAdsPurchased;

	public static Action OnRemoveAdsPurchaseFailed;

	[SerializeField]
	private int minMatchesForInterstitial;

	[SerializeField]
	private int minMatchesForSpecialEvents = 5;

	[SerializeField]
	private float delayForForcedCrossPromo = 10f;

	[SerializeField]
	private int minMatchesForRouletteCharacters;

	[Header("-------------------------------")]
	[Header("IN GAME")]
	[SerializeField]
	private int priceMoreAmmo;

	[SerializeField]
	private int priceMoreAmmoInGems;

	[SerializeField]
	private int missesRemovedOnCoins = 2;

	[SerializeField]
	private int missesRemovedOnVideo = 3;

	[SerializeField]
	private WeaponType weaponMoreAmmo;

	[SerializeField]
	private int priceMoreTime;

	[SerializeField]
	private int priceMoreTimeInGems;

	[SerializeField]
	private int secondsMoreTime;

	[SerializeField]
	private int priceMoreLife;

	[SerializeField]
	private int priceMoreLifeInGems;

	[SerializeField]
	private int percentMoreLife;

	[SerializeField]
	private int shotsCountAfterMoreLifeWithIndividualHitting;

	[SerializeField]
	private float minProbabilityAfterMoreLife;

	[SerializeField]
	private float maxProbabilityAfterMoreLife;

	[SerializeField]
	private int priceMoreLifeTournament;

	[SerializeField]
	private int priceMoreLifeTournamentInGems;

	[SerializeField]
	private int percentMoreLifeTournament;

	[SerializeField]
	private int priceForSecondFatalityChanseInGems;

	[Header("RESULT SCREEN")]
	[Header("-------------------------------")]
	[SerializeField]
	private int minMatchesRate;

	[SerializeField]
	private int minMinutesRate;

	[SerializeField]
	private int minutesBetweenRate;

	[SerializeField]
	private int minMatchesNoAds;

	[SerializeField]
	private int minMatchesNoAdsInDay;

	[SerializeField]
	private int minMatchesFoolishSubscrive;

	[SerializeField]
	private int minMatchesFoolishSubscribeInDay;

	[SerializeField]
	private int minutesBetweenFreeVideo;

	[SerializeField]
	private int matchesBetweenInterstitialResult;

	[SerializeField]
	private int minutesBetweenInterstitialResult;

	[SerializeField]
	private int gemsForSaveTournamentPrice;

	[SerializeField]
	private int minMatchesPromoFetcher;

	[SerializeField]
	private int minMatchesPromoFetcherInDay;

	[Header("Interstitials")]
	[Header("-------------------------------")]
	[Header("MAIN MENU")]
	[SerializeField]
	private int countCloseSettingsScreen;

	[SerializeField]
	private int countCloseAchievementsScreen;

	[Header("Try Character")]
	[SerializeField]
	private int minutesBetweenTryCharacter;

	[SerializeField]
	private int matchesBetweenTryCharacter;

	[SerializeField]
	private int minMatchesForTryCharacter;

	[SerializeField]
	private int matchesBetweenGeneratedTryCharacter;

	[SerializeField]
	private int priceForTryCharacter;

	[SerializeField]
	private int priceForTryCharacterGems;

	[Header("Characters Pack")]
	[SerializeField]
	private int minMatchesForPack;

	[SerializeField]
	private int minutesBetweenPack;

	[SerializeField]
	private int timerPackDuration;

	[SerializeField]
	private int minutesBetweenPackAfterPay;

	[SerializeField]
	private int timerPackDurationAfterPay;

	[Header("Vip Pack")]
	[SerializeField]
	private int minMatchesForVipPack;

	[SerializeField]
	private int minSpentBucksForVipPack = 5;

	[SerializeField]
	private int minMinutesInGameForVipPack;

	[SerializeField]
	private int minutesBetweenVipPack;

	[SerializeField]
	private int timerVipPackDuration;

	[SerializeField]
	private int minutesBetweenVipPackAfterPay;

	[SerializeField]
	private int timerVipPackDurationAfterPay;

	[SerializeField]
	private float maxPercentBoughtCharactersForVipPack;

	[Header("Champion Pack")]
	[SerializeField]
	private int minutesBetweenChampionPack;

	[SerializeField]
	private int minMatchesForChampionPack;

	[SerializeField]
	private int timerChampionPackDuration = 10;

	[SerializeField]
	private int minutesBetweenChampionPackAfterPay;

	[SerializeField]
	private int timerChampionPackDurationAfterPay;

	[SerializeField]
	private int minSpentBucksForChampionPack = 1;

	[SerializeField]
	private int maxSpentBucksForChampionPack = 4;

	[SerializeField]
	private int charactersBoughtForChampionUnavailable = 2;

	[SerializeField]
	private int selfCharactersBoughtForChampionUnavailable = 3;

	[Header("Starter Pack")]
	[SerializeField]
	private int minMatchesUpgrade;

	[SerializeField]
	private int minMinutesInGameForUpgrade;

	[SerializeField]
	private int minutesBetweenUpgrade;

	[SerializeField]
	private int[] matchesForForcedUpgrade;

	[SerializeField]
	private int timerUpgradeDuration;

	[SerializeField]
	private int minutesBetweenUpgradeAfterPay;

	[SerializeField]
	private int timerUpgradeDurationAfterPay;

	[SerializeField]
	private int charactersBoughtForUpgradeUnavailable = 2;

	[SerializeField]
	private int selfCharactersBoughtForUpgradeUnavailable = 2;

	[Header("Free Coins")]
	[SerializeField]
	private int minMatchesForFreeCoins;

	[SerializeField]
	private int minutesBetweenFreeCoins;

	[SerializeField]
	private int coinsGetFreeCoins;

	[Header("Double Coins")]
	[SerializeField]
	private int minutesBetweenDoubleCoins;

	[SerializeField]
	private int minMatchesForDoubleCoins;

	[SerializeField]
	private int timerDoubleCoinsDuration;

	[SerializeField]
	private int minCoinsForDoubleCoinsResultPopup = 100;

	[SerializeField]
	private int[] matchesForForcedDoubleCoins;

	[SerializeField]
	private int minutesBetweenDoubleCoinsAfterPay;

	[SerializeField]
	private int timerDoubleCoinsDurationAfterPay;

	[Header("Free Chest")]
	[SerializeField]
	private int minMatchesForFreeChest;

	[SerializeField]
	private int[] freeChestTimerDurations;

	[SerializeField]
	private float freeChestJumpDelay;

	[Header("Daily gift")]
	[SerializeField]
	private int minMatchesForDailyGift;

	[SerializeField]
	private int secondsBeforeFirstGift;

	[Header("Super gift")]
	[SerializeField]
	private int minMatchesForSuperGift;

	[Header("Crosspromo")]
	[SerializeField]
	private int menuScreensBetweenCrosspromo;

	[Header("Gift characters")]
	[SerializeField]
	private ShopItemCharacter giftCharacter;

	[Header("Shared settings")]
	[SerializeField]
	private int minutesBetweenProposalsWithTimer;

	[SerializeField]
	private int minutesBetweenProposalsWithTimerAfterPay;

	[SerializeField]
	private int minSpentBucksForNewTimers;

	[Header("Invite Friends")]
	[SerializeField]
	private float chanseForShowInviteProposal = 0.5f;

	[Header("What's New")]
	[SerializeField]
	private bool isWhatsNewAvailable;

	[SerializeField]
	private List<ProposalMenuType> proposalsMenuPriority;

	private bool isAvalaibleMoreAmmo;

	private bool isAvalaibleMoreTime;

	private bool isAvalaibleMoreLife;

	private bool isAvalaibleSecondFatalityChanse;

	private DateTime lastFreeVideo = DateTime.Now.AddDays(-10.0);

	private DateTime lastInterstitialResult = DateTime.Now.AddDays(-10.0);

	private ShopItemCharacter buyCharacter;

	private int lastMatchesInterstitialResult;

	private ProposalMenuType currentProposalMenu;

	private List<ShopItemCharacter> nonBoughtCharacters = new List<ShopItemCharacter>();

	private ShopItemCharacter tryCharacter;

	private List<ShopItemPack> nonBoughtPack = new List<ShopItemPack>();

	private ShopItemPack buyPack;

	private ShopItemVipPack vipPack;

	private ShopItemUpgrade upgrade;

	private ShopItemChampion champion;

	private ShopItemDoubleCoins doubleCoins;

	private ShopItemNoAds removeAds;

	private ShopItemUpgradeNew upgradeNew;

	private ShopItemChampionNew championNew;

	private ShopItemAllIncluded allincluded;

	private GUITimer proposalTimer = new GUITimer();

	private GUITimer freeChestTimer = new GUITimer();

	private Managers cachedManagers;

	private StatisticsManager statisticsManager;

	private ObjectsSpawner objectsSpawner;

	private bool isOnVideoFinishEventSubscribed;

	private DateTime crossPromoDateTime;

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

	private ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = CachedManagers.CachedLevelsManager.ObjectsSpawner;
			}
			return objectsSpawner;
		}
	}

	public int GemsForSaveTournamentPrice
	{
		get
		{
			return gemsForSaveTournamentPrice;
		}
	}

	public bool IsAvalaibleMoreAmmo
	{
		get
		{
			return isAvalaibleMoreAmmo;
		}
		set
		{
			isAvalaibleMoreAmmo = value;
		}
	}

	public int PriceMoreAmmo
	{
		get
		{
			return priceMoreAmmo;
		}
	}

	public int PriceMoreAmmoInGems
	{
		get
		{
			return priceMoreAmmoInGems;
		}
	}

	public WeaponType WeaponMoreAmmo
	{
		get
		{
			return weaponMoreAmmo;
		}
	}

	public bool IsAvalaibleMoreTime
	{
		get
		{
			return isAvalaibleMoreTime;
		}
		set
		{
			isAvalaibleMoreTime = value;
		}
	}

	public int PriceMoreTime
	{
		get
		{
			return priceMoreTime;
		}
	}

	public int PriceMoreTimeInGems
	{
		get
		{
			return priceMoreTimeInGems;
		}
	}

	public int SecondsMoreTime
	{
		get
		{
			return secondsMoreTime;
		}
	}

	public bool IsAvalaibleMoreLife
	{
		get
		{
			bool flag = isAvalaibleMoreLife;
			return flag & CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded;
		}
		set
		{
			isAvalaibleMoreLife = value;
		}
	}

	public int PriceMoreLife
	{
		get
		{
			return priceMoreLife;
		}
	}

	public int PriceMoreLifeInGems
	{
		get
		{
			return priceMoreLifeInGems;
		}
	}

	public int PercentMoreLife
	{
		get
		{
			return percentMoreLife;
		}
	}

	public int PriceMoreLifeTournament
	{
		get
		{
			return priceMoreLifeTournament;
		}
	}

	public int PriceMoreLifeTournamentInGems
	{
		get
		{
			return priceMoreLifeTournamentInGems;
		}
	}

	public int PercentMoreLifeTournament
	{
		get
		{
			return percentMoreLifeTournament;
		}
	}

	public int PriceForSecondFatalityChanseInGems
	{
		get
		{
			return priceForSecondFatalityChanseInGems;
		}
	}

	public bool IsAvalaibleSecondFatalityChance
	{
		get
		{
			return isAvalaibleSecondFatalityChanse && CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded && CachedManagers.CachedRemoteManager.GetRemoteValueForKey("IsFatalitySecondChanceEnabled", true);
		}
		set
		{
			isAvalaibleSecondFatalityChanse = value;
		}
	}

	public int MissesRemovedOnCoins
	{
		get
		{
			return missesRemovedOnCoins;
		}
	}

	public int MissesRemovedOnVideo
	{
		get
		{
			return missesRemovedOnVideo;
		}
	}

	public int ShotsCountAfterMoreLifeWithIndividualHitting
	{
		get
		{
			return shotsCountAfterMoreLifeWithIndividualHitting;
		}
	}

	public float MinProbabilityAfterMoreLife
	{
		get
		{
			return minProbabilityAfterMoreLife;
		}
	}

	public float MaxProbabilityAfterMoreLife
	{
		get
		{
			return maxProbabilityAfterMoreLife;
		}
	}

	private bool IsOnVideoFinishEventSubscribed
	{
		get
		{
			return isOnVideoFinishEventSubscribed;
		}
		set
		{
			if (isOnVideoFinishEventSubscribed != value)
			{
				isOnVideoFinishEventSubscribed = value;
				if (isOnVideoFinishEventSubscribed)
				{
					AdvertisingManager.OnVideoAdFinish += AdvertisingManager_OnVideoAdFinish;
				}
				else
				{
					AdvertisingManager.OnVideoAdFinish -= AdvertisingManager_OnVideoAdFinish;
				}
			}
		}
	}

	public int MinCoinsForDoubleCoinsResultPopup
	{
		get
		{
			return minCoinsForDoubleCoinsResultPopup;
		}
	}

	public bool IsRateMeAvalaible
	{
		get
		{
			return SingletonMonoBehaviour<RateMeManager>.Instance.IsRateMeAvalaible;
		}
	}

	public bool IsNoAdsAvalaible
	{
		get
		{
			bool flag = !CachedManagers.CachedShopManager.NoAdsBought;
			flag &= statisticsManager.MathesCount >= minMatchesNoAds;
			flag &= statisticsManager.DaysSinceLastNoAds >= 1;
			return flag & (statisticsManager.CountMatchesInDay >= minMatchesNoAdsInDay);
		}
	}

	public bool IsFoolishSubscribeAvalaible
	{
		get
		{
			bool flag = !CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
			flag &= !CachedManagers.CachedShopManager.TrialActivated;
			flag &= statisticsManager.MathesCount >= minMatchesFoolishSubscrive;
			flag &= statisticsManager.DaysSinceLastFoolishSubscribe >= 1;
			flag &= statisticsManager.CountMatchesInDay >= minMatchesFoolishSubscribeInDay;
			flag &= CachedManagers.CachedGUIPopupManager.ActivePopup == null || (CachedManagers.CachedGUIPopupManager.ActivePopup != null && CachedManagers.CachedGUIPopupManager.ActivePopup.TypePopup != PopupType.BuyCharacter);
			return flag & CachedManagers.CachedRemoteManager.IsSubscriptionPopupShowed;
		}
	}

	private TimeSpan LastRateInterval
	{
		get
		{
			return DateTime.Now - LastRate;
		}
	}

	private DateTime LastRate
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_rate", DateTime.Now.AddDays(-10.0));
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_rate", value);
		}
	}

	public bool IsFreeVideoAvalaible
	{
		get
		{
			return LastFreeVideo.TotalMinutes >= (double)minutesBetweenFreeVideo;
		}
	}

	public TimeSpan LastFreeVideo
	{
		get
		{
			return DateTime.Now - lastFreeVideo;
		}
	}

	public bool IsInterstitialResultAvalaible
	{
		get
		{
			bool flag = true;
			flag &= statisticsManager.SpentBucks == 0;
			flag &= statisticsManager.TotalSeesionCount > 0;
			flag &= SingletonMonoBehaviour<SuperSonicManager>.Instance.IsInterstitialAvailable;
			Debug.Log("IsInterstitialResultAvalaible(BUCKS[" + statisticsManager.SpentBucks + "] SEESIONS[" + statisticsManager.TotalSeesionCount + "] AVAILABLE[" + SingletonMonoBehaviour<SuperSonicManager>.Instance.IsInterstitialAvailable + "])");
			return flag;
		}
	}

	public TimeSpan LastInterstitialResult
	{
		get
		{
			return DateTime.Now - lastInterstitialResult;
		}
	}

	public int LastMatchesInterstitialResult
	{
		get
		{
			return statisticsManager.MathesCount - lastMatchesInterstitialResult;
		}
	}

	public bool IsEpicChestResultAvalaible
	{
		get
		{
			bool flag = CachedManagers.CachedRouletteManager.KeysCount >= SingletonMonoBehaviour<RouletteManager>.Instance.LocksCount;
			return flag | !CachedManagers.CachedTutorialManager.IsFreeChestsEnded;
		}
	}

	public bool IsBuyCharacterAvalaible
	{
		get
		{
			bool flag = buyCharacter != null;
			if (flag)
			{
				CharacterNameType characterNameType = ((buyCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? buyCharacter.Infos[0].characterNameType : buyCharacter.CharacterUpgradeInfo.characterNameType);
				flag &= statisticsManager.LastCharacterNameType == characterNameType;
			}
			return flag;
		}
	}

	public ShopItemCharacter BuyCharacter
	{
		get
		{
			return buyCharacter;
		}
	}

	public ProposalResultType CurrentProposalResult { get; set; }

	public bool IsForcedDoubleCoins
	{
		get
		{
			bool flag = false;
			int mathesCount = statisticsManager.MathesCount;
			for (int i = 0; i < matchesForForcedDoubleCoins.Length; i++)
			{
				if (mathesCount == matchesForForcedDoubleCoins[i])
				{
					flag = true;
				}
			}
			return flag & !DoubleCoins.Bought;
		}
	}

	public bool IsPromoFetcherAvalaible
	{
		get
		{
			bool flag = statisticsManager.MathesCount >= minMatchesPromoFetcher;
			flag &= statisticsManager.CountMatchesInDay >= minMatchesPromoFetcherInDay;
			flag &= statisticsManager.DaysSinceLastPromoFetcher >= 1;
			flag &= !string.IsNullOrEmpty(LLPromoFetcher.PromoFetcherActiveUnit().promoURL);
			return flag & CachedManagers.CachedRemoteManager.GetRemoteValueForKey("isPromoFetcherEnabled", true);
		}
	}

	public bool IsShowingForcedDoubleCoins { get; set; }

	public ProposalMenuType CurrentProposalMenu
	{
		get
		{
			return currentProposalMenu;
		}
		private set
		{
			if (currentProposalMenu != value)
			{
				currentProposalMenu = value;
				if (LastProposalWithTimerType == ProposalMenuType.None && IsProposalWithTimer(currentProposalMenu))
				{
					IsNeedShowForcedTimerPopup = true;
					LastProposalWithTimerType = currentProposalMenu;
					LastProposalWithTimerStart = DateTime.Now;
					proposalTimer.TimerInSeconds = TimerDurationByProposalType(currentProposalMenu);
				}
			}
		}
	}

	public bool AreRouletteCharactersAvailable
	{
		get
		{
			return !CachedManagers.CachedSubscriptionManager.IsSubscriptionActive && CachedManagers.CachedStatisticsManager.MathesCount >= minMatchesForRouletteCharacters;
		}
	}

	public bool IsRateUsForcedAvailable
	{
		get
		{
			return isWhatsNewAvailable && CustomPlayerPrefs.GetString("forced_rate_game") != Application.version && CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded;
		}
	}

	public bool IsNeedShowInvitePopup { get; set; }

	public bool IsTryCharacterAvailable
	{
		get
		{
			bool flag = LastTryCharacterInterval.TotalMinutes >= (double)minutesBetweenTryCharacter;
			flag &= tryCharacter != null;
			flag &= statisticsManager.MathesCount >= minMatchesForTryCharacter;
			flag &= statisticsManager.MathesCount - LastTryCharacterMatches > matchesBetweenTryCharacter;
			return flag & (statisticsManager.MathesCount - LastTryCharacterGeneratedMatches > matchesBetweenGeneratedTryCharacter);
		}
	}

	private int LastTryCharacterMatches
	{
		get
		{
			return CustomPlayerPrefs.GetInt("last_try_character_matches", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("last_try_character_matches", value);
		}
	}

	public int LastTryCharacterGeneratedMatches
	{
		get
		{
			return CustomPlayerPrefs.GetInt("last_try_character_gen_matches", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("last_try_character_gen_matches", value);
		}
	}

	private string LastTryCharacterID
	{
		get
		{
			return CustomPlayerPrefs.GetString("last_try_character_id", string.Empty);
		}
		set
		{
			CustomPlayerPrefs.SetString("last_try_character_id", value);
		}
	}

	private string LastButOneTryCharacterID
	{
		get
		{
			return CustomPlayerPrefs.GetString("last_but_one_try_character_id", string.Empty);
		}
		set
		{
			CustomPlayerPrefs.SetString("last_but_one_try_character_id", value);
		}
	}

	public ShopItemCharacter TryCharacter
	{
		get
		{
			return tryCharacter;
		}
	}

	private DateTime LastTryCharacter
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_try_character", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_try_character", value);
		}
	}

	public TimeSpan LastTryCharacterInterval
	{
		get
		{
			return DateTime.Now - LastTryCharacter;
		}
	}

	public int PriceForTryCharacter
	{
		get
		{
			return priceForTryCharacter;
		}
	}

	public int PriceForTryCharacterGems
	{
		get
		{
			return priceForTryCharacterGems;
		}
	}

	public bool IsUpgradeAvailable
	{
		get
		{
			bool flag = LastUpgradeInterval.TotalMinutes >= (double)MinutesBetweenUpgrade;
			flag &= LastProposalWithTimerInterval.TotalMinutes >= (double)MinutesBetweenProposalsWithTimer;
			flag &= statisticsManager.SpentBucks == 0;
			flag &= statisticsManager.MathesCount >= minMatchesUpgrade;
			flag &= statisticsManager.TotalGameTime.TotalMinutes + statisticsManager.GameTime.TotalMinutes >= (double)minMinutesInGameForUpgrade;
			flag &= !CachedManagers.CachedShopManager.AllIncludedBought;
			flag &= !Upgrade.Bought;
			flag &= !UpgradeNew.Bought;
			return flag & (PackCharactersBought(UpgradeNew) < selfCharactersBoughtForUpgradeUnavailable);
		}
	}

	private int MinutesBetweenUpgrade
	{
		get
		{
			return (statisticsManager.SpentBucks <= MinSpentBucksForNewTimers) ? minutesBetweenUpgradeAfterPay : minutesBetweenUpgrade;
		}
	}

	public ShopItemUpgradeNew UpgradeNew
	{
		get
		{
			if (upgradeNew == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i].GetType() == typeof(ShopItemUpgradeNew))
					{
						upgradeNew = availableInApps[i] as ShopItemUpgradeNew;
						break;
					}
				}
			}
			return upgradeNew;
		}
	}

	public DateTime LastUpgrade
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_upgrade", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_upgrade", value);
		}
	}

	public TimeSpan LastUpgradeInterval
	{
		get
		{
			return DateTime.Now - LastUpgrade;
		}
	}

	public bool IsForcedPackAvailable { get; set; }

	public bool IsForcedPack
	{
		get
		{
			bool flag = false;
			int mathesCount = statisticsManager.MathesCount;
			for (int i = 0; i < matchesForForcedUpgrade.Length; i++)
			{
				if (mathesCount == matchesForForcedUpgrade[i])
				{
					flag = true;
				}
			}
			flag &= SingletonMonoBehaviour<ProposalManager>.Instance.IsUpgradeAvailable || SingletonMonoBehaviour<ProposalManager>.Instance.IsChampionPackAvailable || SingletonMonoBehaviour<ProposalManager>.Instance.IsVipPackAvailable;
			return flag & IsForcedPackAvailable;
		}
	}

	public bool IsShowingForcedPack { get; set; }

	public bool IsChampionPackAvailable
	{
		get
		{
			bool flag = statisticsManager.MathesCount >= minMatchesForChampionPack;
			flag &= LastChampionPackInterval.TotalMinutes >= (double)MinutesBetweenChampionPack;
			flag &= LastProposalWithTimerInterval.TotalMinutes >= (double)MinutesBetweenProposalsWithTimer;
			flag &= !CachedManagers.CachedShopManager.AllIncludedBought;
			flag &= !ChampionNew.Bought;
			flag &= !Champion.Bought;
			flag &= PackCharactersBought(ChampionNew) < selfCharactersBoughtForChampionUnavailable;
			return flag & ((statisticsManager.SpentBucks >= minSpentBucksForChampionPack && statisticsManager.SpentBucks <= maxSpentBucksForChampionPack) || PackCharactersBought(UpgradeNew) > charactersBoughtForUpgradeUnavailable);
		}
	}

	private int MinutesBetweenChampionPack
	{
		get
		{
			return (statisticsManager.SpentBucks <= MinSpentBucksForNewTimers) ? minutesBetweenChampionPackAfterPay : minutesBetweenChampionPack;
		}
	}

	public ShopItemChampionNew ChampionNew
	{
		get
		{
			if (championNew == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i].GetType() == typeof(ShopItemChampionNew))
					{
						championNew = availableInApps[i] as ShopItemChampionNew;
						break;
					}
				}
			}
			return championNew;
		}
	}

	public int MinSpentBucksForChampionPack
	{
		get
		{
			return minSpentBucksForChampionPack;
		}
	}

	public int MaxSpentBucksForChampionPack
	{
		get
		{
			return maxSpentBucksForChampionPack;
		}
	}

	public DateTime LastChampionPack
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_champion_pack", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_champion_pack", value);
		}
	}

	public TimeSpan LastChampionPackInterval
	{
		get
		{
			return DateTime.Now - LastChampionPack;
		}
	}

	public bool IsVipPackAvailable
	{
		get
		{
			bool flag = LastVipPackInterval.TotalMinutes >= (double)MinutesBetweenVipPack;
			flag &= LastProposalWithTimerInterval.TotalMinutes >= (double)MinutesBetweenProposalsWithTimer;
			flag &= statisticsManager.MathesCount >= minMatchesForVipPack;
			flag &= statisticsManager.TotalGameTime.TotalMinutes + statisticsManager.GameTime.TotalMinutes >= (double)minMinutesInGameForVipPack;
			flag &= !CachedManagers.CachedShopManager.AllIncludedBought;
			flag &= !CachedManagers.CachedShopManager.AllCharactersBought;
			flag &= !VipPack.Bought;
			flag &= !IsMaxCharacterForVipPackBought;
			return flag & (statisticsManager.SpentBucks >= minSpentBucksForVipPack || PackCharactersBought(ChampionNew) > charactersBoughtForChampionUnavailable);
		}
	}

	private bool IsMaxCharacterForVipPackBought
	{
		get
		{
			ShopItemCharacter[] characters = VipPack.Characters;
			List<ShopItemCharacter> list = new List<ShopItemCharacter>();
			for (int i = 0; i < characters.Length; i++)
			{
				if (characters[i].Bought)
				{
					list.Add(characters[i]);
				}
			}
			return (float)list.Count > maxPercentBoughtCharactersForVipPack * (float)characters.Length;
		}
	}

	private int MinutesBetweenVipPack
	{
		get
		{
			return (statisticsManager.SpentBucks <= MinSpentBucksForNewTimers) ? minutesBetweenVipPackAfterPay : minutesBetweenVipPack;
		}
	}

	public ShopItemVipPack VipPack
	{
		get
		{
			if (vipPack == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i] is ShopItemVipPack)
					{
						vipPack = availableInApps[i] as ShopItemVipPack;
						break;
					}
				}
			}
			return vipPack;
		}
	}

	private DateTime LastVipPack
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_vip_pack", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_vip_pack", value);
		}
	}

	public TimeSpan LastVipPackInterval
	{
		get
		{
			return DateTime.Now - LastVipPack;
		}
	}

	public ShopItemNoAds RemoveAds
	{
		get
		{
			if (removeAds == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i].GetType() == typeof(ShopItemNoAds))
					{
						removeAds = availableInApps[i] as ShopItemNoAds;
						break;
					}
				}
			}
			return removeAds;
		}
	}

	private ShopItemUpgrade Upgrade
	{
		get
		{
			if (upgrade == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i].GetType() == typeof(ShopItemUpgrade))
					{
						upgrade = availableInApps[i] as ShopItemUpgrade;
						break;
					}
				}
			}
			return upgrade;
		}
	}

	private ShopItemChampion Champion
	{
		get
		{
			if (champion == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i].GetType() == typeof(ShopItemChampion))
					{
						champion = availableInApps[i] as ShopItemChampion;
						break;
					}
				}
			}
			return champion;
		}
	}

	private ShopItemAllIncluded Allincluded
	{
		get
		{
			if (allincluded == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i].GetType() == typeof(ShopItemAllIncluded))
					{
						allincluded = availableInApps[i] as ShopItemAllIncluded;
						break;
					}
				}
			}
			return allincluded;
		}
	}

	public bool IsDoubleCoinsAvailable
	{
		get
		{
			bool flag = false;
			if (DoubleCoins != null)
			{
				flag = !DoubleCoins.Bought;
			}
			flag &= LastDoubleCoinsInterval.TotalMinutes >= (double)MinutesBetweenDoubleCoins;
			flag &= LastProposalWithTimerInterval.TotalMinutes >= (double)MinutesBetweenProposalsWithTimer;
			return flag & (statisticsManager.MathesCount >= minMatchesForDoubleCoins);
		}
	}

	private int MinutesBetweenDoubleCoins
	{
		get
		{
			return (statisticsManager.SpentBucks <= MinSpentBucksForNewTimers) ? minutesBetweenDoubleCoinsAfterPay : minutesBetweenDoubleCoins;
		}
	}

	public ShopItemDoubleCoins DoubleCoins
	{
		get
		{
			if (doubleCoins == null)
			{
				List<ShopItem> availableInApps = CachedManagers.CachedShopManager.AvailableInApps;
				for (int i = 0; i < availableInApps.Count; i++)
				{
					if (availableInApps[i].GetType() == typeof(ShopItemDoubleCoins))
					{
						doubleCoins = availableInApps[i] as ShopItemDoubleCoins;
						break;
					}
				}
			}
			return doubleCoins;
		}
	}

	public DateTime LastDoubleCoins
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_double_coins", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_double_coins", value);
		}
	}

	public TimeSpan LastDoubleCoinsInterval
	{
		get
		{
			return DateTime.Now - LastDoubleCoins;
		}
	}

	public bool IsPackAvailable
	{
		get
		{
			bool flag = LastPackInterval.TotalMinutes >= (double)MinutesBetweenPack;
			flag &= LastProposalWithTimerInterval.TotalMinutes >= (double)MinutesBetweenProposalsWithTimer;
			flag &= statisticsManager.MathesCount >= minMatchesForPack;
			flag &= !CachedManagers.CachedShopManager.AllIncludedBought;
			flag &= !CachedManagers.CachedShopManager.AllCharactersBought;
			return flag & (buyPack != null);
		}
	}

	private int MinutesBetweenPack
	{
		get
		{
			return (statisticsManager.SpentBucks <= MinSpentBucksForNewTimers) ? minutesBetweenPackAfterPay : minutesBetweenPack;
		}
	}

	public DateTime LastPack
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_pack", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_pack", value);
		}
	}

	public TimeSpan LastPackInterval
	{
		get
		{
			return DateTime.Now - LastPack;
		}
	}

	public ShopItemPack BuyPack
	{
		get
		{
			return buyPack;
		}
	}

	public bool IsFreeCoinsAvailable
	{
		get
		{
			bool flag = LastFreeCoinsInterval.TotalMinutes >= (double)minutesBetweenFreeCoins;
			flag &= statisticsManager.MathesCount >= minMatchesForFreeCoins;
			return false;
		}
	}

	private DateTime LastFreeCoins
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_free_coins", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_free_coins", value);
		}
	}

	public TimeSpan LastFreeCoinsInterval
	{
		get
		{
			return DateTime.Now - LastFreeCoins;
		}
	}

	public bool IsInterstitialMenuAvalaible
	{
		get
		{
			bool flag = true;
			flag &= !CachedManagers.CachedShopManager.NoAdsBought;
			flag &= statisticsManager.MathesCount >= minMatchesForInterstitial;
			flag &= CachedManagers.CachedAdvertisingManager.IsInterstitialAvailable;
			return flag & (CountCloseSettingsScreen >= countCloseSettingsScreen || CountCloseAchievementsScreen >= countCloseAchievementsScreen);
		}
	}

	public bool IsInterstitialMenuAfterRouletteScreenAvailable
	{
		get
		{
			bool flag = true;
			flag = false;
			flag &= !CachedManagers.CachedShopManager.NoAdsBought;
			flag &= statisticsManager.MathesCount >= minMatchesForInterstitial;
			flag &= CachedManagers.CachedAdvertisingManager.IsInterstitialAvailable;
			return flag & ((float)statisticsManager.MathesCount * 2.5f <= (float)statisticsManager.FreeChestsOpened);
		}
	}

	public int MinMatchesForInterstitial
	{
		get
		{
			return minMatchesForInterstitial;
		}
	}

	public int CountCloseSettingsScreen { get; set; }

	public int CountCloseAchievementsScreen { get; set; }

	private int MinutesBetweenProposalsWithTimer
	{
		get
		{
			return (statisticsManager.SpentBucks <= MinSpentBucksForNewTimers) ? minutesBetweenProposalsWithTimerAfterPay : minutesBetweenProposalsWithTimer;
		}
	}

	public bool IsNeedShowForcedTimerPopup { get; set; }

	private DateTime LastProposalWithTimer
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_proposal_with_timer", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_proposal_with_timer", value);
		}
	}

	public GUITimer ProposalTimer
	{
		get
		{
			return proposalTimer;
		}
	}

	public TimeSpan LastProposalWithTimerInterval
	{
		get
		{
			return DateTime.Now - LastProposalWithTimer;
		}
	}

	public ProposalMenuType LastProposalWithTimerType
	{
		get
		{
			return (ProposalMenuType)CustomPlayerPrefs.GetInt("last_proposal_with_timer_type", 0);
		}
		private set
		{
			if (LastProposalWithTimerType != value)
			{
				CustomPlayerPrefs.SetInt("last_proposal_with_timer_type", (int)value);
				proposalTimer.IsTimerActive = value != ProposalMenuType.None;
			}
		}
	}

	private DateTime LastProposalWithTimerStart
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_proposal_with_timer_start", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_proposal_with_timer_start", value);
		}
	}

	private int MinSpentBucksForNewTimers
	{
		get
		{
			return minSpentBucksForNewTimers;
		}
	}

	public GUITimer FreeChestTimer
	{
		get
		{
			return freeChestTimer;
		}
	}

	public DateTime LastFreeChestTimerStart
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_free_chest_timer_start", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_free_chest_timer_start", value);
		}
	}

	public DateTime LastFreeChestAccelerator
	{
		get
		{
			return CustomPlayerPrefs.GetDateTime("last_free_chest_accelerator", DateTime.MinValue);
		}
		set
		{
			CustomPlayerPrefs.SetDateTime("last_free_chest_accelerator", value);
		}
	}

	public float FreeChestJumpDelay
	{
		get
		{
			return freeChestJumpDelay;
		}
	}

	public bool IsFreeChestAvailable
	{
		get
		{
			bool flag = statisticsManager.MathesCount >= minMatchesForFreeChest;
			return flag & ((int)freeChestTimer.TimerInSeconds == 0);
		}
	}

	public int MinMatchesForFreeChest
	{
		get
		{
			return minMatchesForFreeChest;
		}
	}

	public bool IsCrossPromoShowing { get; set; }

	public int SecondsBeforeFirstGift
	{
		get
		{
			return secondsBeforeFirstGift;
		}
	}

	public bool IsGiftAvailable
	{
		get
		{
			bool flag = statisticsManager.MathesCount >= minMatchesForDailyGift;
			return flag & CachedManagers.CachedGiftManager.IsGiftAvailable;
		}
	}

	public ShopItemCharacter GiftCharacter
	{
		get
		{
			return giftCharacter;
		}
	}

	public bool IsSuperGiftAvailable
	{
		get
		{
			bool flag = statisticsManager.MathesCount >= minMatchesForSuperGift;
			if (flag)
			{
				if ((DateTime.Now - CachedManagers.CachedGiftManager.DateLastSuperGift).TotalDays < 1.0)
				{
					flag = SuperGiftNumber < 3;
				}
				else
				{
					CachedManagers.CachedGiftManager.DateLastSuperGift = DateTime.Now;
					SuperGiftNumber = 0;
					flag = true;
				}
			}
			return CachedManagers.CachedRemoteManager.GetRemoteValueForKey("IsSuperGiftEnabled", false) && flag;
		}
	}

	public int SuperGiftNumber
	{
		get
		{
			return CustomPlayerPrefs.GetInt("super_gift_number", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("super_gift_number", value);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		statisticsManager = CachedManagers.CachedStatisticsManager;
		IsNeedShowForcedTimerPopup = true;
		IsNeedShowInvitePopup = true;
		LLApplicationStateRegister.Initialize();
	}

	private void Start()
	{
		lastMatchesInterstitialResult = statisticsManager.MathesCount;
		float num = 0f;
		ProposalMenuType lastProposalWithTimerType = LastProposalWithTimerType;
		if (lastProposalWithTimerType != ProposalMenuType.None)
		{
			float num2 = TimerDurationByProposalType(lastProposalWithTimerType);
			num = num2 - (float)(DateTime.Now - LastProposalWithTimerStart).TotalSeconds;
			if (num <= 5f)
			{
				num = 0f;
				LastProposalWithTimerType = ProposalMenuType.None;
				DateTime dateTime = (LastProposalWithTimer = LastProposalWithTimerStart + TimeSpan.FromSeconds(num2));
				switch (lastProposalWithTimerType)
				{
				case ProposalMenuType.VipPack:
				case ProposalMenuType.Upgrade:
				case ProposalMenuType.ChampionPack:
					LastVipPack = dateTime;
					LastChampionPack = dateTime;
					LastUpgrade = dateTime;
					break;
				case ProposalMenuType.BuyPack:
					LastPack = dateTime;
					break;
				case ProposalMenuType.X2:
					LastDoubleCoins = dateTime;
					break;
				}
			}
		}
		proposalTimer.TimerInSeconds = num;
		proposalTimer.IsTimerActive = LastProposalWithTimerType != ProposalMenuType.None;
		InitializeFreeChestTimer();
		LLPromoFetcher.Initialize();
		IsOnVideoFinishEventSubscribed = false;
	}

	private void OnEnable()
	{
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		GameManager.OnGameStart += GameManager_OnGameStart;
		GameManager.OnGameStop += GameManager_OnGameStop;
		ProposalTimer.OnTimerChanged += ProposalTimer_OnTimerChanged;
		TapToStartScreen.OnLoaderRemoved += TapToStartScreen_OnLoaderRemoved;
		LLTapdaqManager.OnTapdaqInterstitialShow += LLTapdaqManager_OnTapdaqInterstitialShow;
		LLTapdaqManager.OnTapdaqInterstitialFinish += LLTapdaqManager_OnTapdaqInterstitialFinish;
		LLApplicationStateRegister.OnApplicationEnteredBackground += LLApplicationStateRegister_OnApplicationEnteredBackground;
		MiniBankPopup.OnBankItemPurchsed += MiniBankPopup_OnBankItemPurchsed;
	}

	private void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		GameManager.OnGameStart -= GameManager_OnGameStart;
		GameManager.OnGameStop -= GameManager_OnGameStop;
		ProposalTimer.OnTimerChanged -= ProposalTimer_OnTimerChanged;
		TapToStartScreen.OnLoaderRemoved -= TapToStartScreen_OnLoaderRemoved;
		LLTapdaqManager.OnTapdaqInterstitialShow -= LLTapdaqManager_OnTapdaqInterstitialShow;
		LLTapdaqManager.OnTapdaqInterstitialFinish -= LLTapdaqManager_OnTapdaqInterstitialFinish;
		LLApplicationStateRegister.OnApplicationEnteredBackground -= LLApplicationStateRegister_OnApplicationEnteredBackground;
		MiniBankPopup.OnBankItemPurchsed -= MiniBankPopup_OnBankItemPurchsed;
	}

	private void Update()
	{
		proposalTimer.UpdateTimer(Time.unscaledDeltaTime);
		freeChestTimer.UpdateTimer(Time.unscaledDeltaTime);
	}

	private IEnumerator OnApplicationPause(bool isPaused)
	{
		yield return null;
		if (!isPaused)
		{
			proposalTimer.ForceTimeChangeEvent();
			freeChestTimer.ForceTimeChangeEvent();
		}
	}

	public void GetProposalResult(Action<bool> callback)
	{
		ProposalResultType currentProposalResult = CurrentProposalResult;
		switch (currentProposalResult)
		{
		case ProposalResultType.Video:
		{
			lastFreeVideo = DateTime.Now;
			Action<bool> videoCallback = delegate(bool result)
			{
				if (result)
				{
					GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW, GoliathIds.AdReward.CHEST_ROULETTE);
					CachedManagers.CachedRouletteManager.KeysCount++;
				}
				else
				{
					GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW);
				}
				if (IsEpicChestResultAvalaible)
				{
					CurrentProposalResult = ProposalResultType.Chest;
				}
				else
				{
					CurrentProposalResult = ((!result) ? ProposalResultType.Video : ProposalResultType.None);
				}
				if (callback != null)
				{
					callback(result);
				}
			};
			if (!CachedManagers.CachedSubscriptionManager.IsSubscriptionActive)
			{
				CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoAvailable)
				{
					videoCallback(isVideoAvailable);
					if (isVideoAvailable)
					{
						Analytics.CustomEvent("Video_Chest_Finished");
					}
				});
			}
			else
			{
				videoCallback(true);
			}
			break;
		}
		case ProposalResultType.Rate:
			SingletonMonoBehaviour<RateMeManager>.Instance.RateAppAction();
			CurrentProposalResult = ProposalResultType.None;
			if (callback != null)
			{
				callback(false);
			}
			break;
		case ProposalResultType.NoAds:
		{
			RemoveAds.OnItemPurchased += ShopItem_OnItemPurchased;
			RemoveAds.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			bool flag = RemoveAds.TryBuy();
			if (!flag)
			{
				RemoveAds.OnItemPurchased -= ShopItem_OnItemPurchased;
				RemoveAds.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				CachedManagers.CachedTK2DUIManager.InputEnabled = true;
			}
			CurrentProposalResult = ProposalResultType.None;
			if (callback != null)
			{
				callback(flag);
			}
			break;
		}
		case ProposalResultType.None:
		case ProposalResultType.Chest:
			if (IsEpicChestResultAvalaible)
			{
				CurrentProposalResult = ProposalResultType.Chest;
			}
			else
			{
				CurrentProposalResult = ProposalResultType.None;
			}
			if (callback != null)
			{
				callback(IsEpicChestResultAvalaible);
			}
			break;
		}
	}

	public void ShowInterstitialResult()
	{
		if (IsInterstitialResultAvalaible)
		{
			lastInterstitialResult = DateTime.Now;
			lastMatchesInterstitialResult = statisticsManager.MathesCount;
			CachedManagers.CachedAdvertisingManager.ShowInterstitial();
		}
	}

	public void ShowInterstitialMenu(bool isAfterSettingsScreen)
	{
		if (IsInterstitialMenuAvalaible)
		{
			if (isAfterSettingsScreen)
			{
				CountCloseSettingsScreen = 0;
			}
			else
			{
				CountCloseAchievementsScreen = 0;
			}
			CachedManagers.CachedAdvertisingManager.ShowInterstitial();
		}
	}

	public void ShowInterstitialMenuAfterRoulette()
	{
		if (IsInterstitialMenuAfterRouletteScreenAvailable)
		{
			CachedManagers.CachedAdvertisingManager.ShowInterstitial();
		}
	}

	public void ShowInterstitialCrosspromo(bool isForcedSHowing = false)
	{
		if (isForcedSHowing || statisticsManager.MenuScreensCount == 3 || statisticsManager.MenuScreensCount % 10 == 1)
		{
			IsCrossPromoShowing = true;
		}
	}

	public void GetProposalMenu(GUIProposalMenu currentProposal, ProposalMenuType forcedType = ProposalMenuType.None)
	{
		switch ((forcedType != ProposalMenuType.None) ? forcedType : CurrentProposalMenu)
		{
		case ProposalMenuType.TryNew:
		{
			LastTryCharacter = DateTime.Now;
			LastTryCharacterMatches = statisticsManager.MathesCount;
			Action<bool> proposalCallback = GetProposalCallback();
			if (CachedManagers.CachedSubscriptionManager.IsSubscriptionActive)
			{
				if (statisticsManager.Gems >= priceForTryCharacterGems)
				{
					SpendCoinsForTryCharacter();
				}
				else
				{
					(CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true) as MiniBankPopup).InitPopup(priceForTryCharacterGems);
				}
			}
			else
			{
				IsOnVideoFinishEventSubscribed = true;
				CachedManagers.CachedAdvertisingManager.ShowVideo(proposalCallback);
			}
			break;
		}
		case ProposalMenuType.BuyPack:
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Pack);
			break;
		case ProposalMenuType.VipPack:
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.VipPack);
			break;
		case ProposalMenuType.ChampionPack:
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.ChampionPack);
			break;
		case ProposalMenuType.FreeCoins:
			LastFreeCoins = DateTime.Now;
			CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool result)
			{
				if (result)
				{
					GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW, GoliathIds.AdReward.COINS);
					statisticsManager.AddCoins(coinsGetFreeCoins);
				}
			});
			break;
		case ProposalMenuType.Gift:
			currentProposal.Deactivate();
			CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Gift);
			break;
		case ProposalMenuType.SuperGift:
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.SuperGift);
			break;
		case ProposalMenuType.Upgrade:
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Upgrade);
			break;
		case ProposalMenuType.X2:
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.DoubleCoins);
			break;
		case ProposalMenuType.FreeChest:
		{
			(currentProposal as FreeChestProposalMenu).Deactivate(false);
			LastFreeChestTimerStart = DateTime.Now;
			statisticsManager.FreeChestsOpened++;
			InitializeFreeChestTimer();
			CachedManagers.CachedNotifyManager.UpdateNotifications(0f, string.Empty);
			MenuScreen menuScreen = CachedManagers.CachedGUIScreenManager.MenuScreen;
			if (menuScreen != null)
			{
				menuScreen.HideMenuAndShowRouletteScreen();
			}
			break;
		}
		case ProposalMenuType.None:
			break;
		case ProposalMenuType.NextChest:
			break;
		}
	}

	public void GenerateProposalResult()
	{
		CurrentProposalResult = ProposalResultType.None;
		if (IsEpicChestResultAvalaible)
		{
			CurrentProposalResult = ProposalResultType.Chest;
		}
		else if (IsRateMeAvalaible)
		{
			CurrentProposalResult = ProposalResultType.Rate;
			LastRate = DateTime.Now;
		}
		else if (IsNoAdsAvalaible)
		{
			CurrentProposalResult = ProposalResultType.NoAds;
			statisticsManager.LastNoAds = DateTime.Now;
		}
		else if (IsFreeVideoAvalaible)
		{
			CurrentProposalResult = ProposalResultType.Video;
		}
		else
		{
			CurrentProposalResult = ProposalResultType.Video;
		}
	}

	public void GenerateProposalMenu(bool needUpdateMenuProposals = false)
	{
		CurrentProposalMenu = ProposalMenuType.None;
		nonBoughtCharacters.Clear();
		for (int i = 0; i < CachedManagers.CachedShopManager.AvailableCharacters.Count; i++)
		{
			ShopItemCharacter shopItemCharacter = CachedManagers.CachedShopManager.AvailableCharacters[i];
			if (shopItemCharacter != null && !shopItemCharacter.Availible && shopItemCharacter.CanBuyInShop)
			{
				nonBoughtCharacters.Add(shopItemCharacter);
			}
		}
		string lastTryCharacterID = LastTryCharacterID;
		string lastButOneTryCharacterID = LastButOneTryCharacterID;
		tryCharacter = null;
		if (nonBoughtCharacters.Count > 0)
		{
			do
			{
				tryCharacter = nonBoughtCharacters.RandomObject();
			}
			while ((nonBoughtCharacters.Count > 2 && (tryCharacter.ShopItemID == lastTryCharacterID || tryCharacter.ShopItemID == lastButOneTryCharacterID)) || (nonBoughtCharacters.Count == 2 && tryCharacter.ShopItemID == lastTryCharacterID));
			LastButOneTryCharacterID = lastTryCharacterID;
			LastTryCharacterID = tryCharacter.ShopItemID;
		}
		nonBoughtPack.Clear();
		buyPack = null;
		for (int j = 0; j < CachedManagers.CachedShopManager.AvailablePacks.Count; j++)
		{
			ShopItemPack shopItemPack = CachedManagers.CachedShopManager.AvailablePacks[j];
			if (shopItemPack != null && !shopItemPack.Availible)
			{
				nonBoughtPack.Add(shopItemPack);
			}
		}
		if (nonBoughtPack.Count > 0)
		{
			buyPack = nonBoughtPack.RandomObject();
		}
		for (int k = 0; k < proposalsMenuPriority.Count; k++)
		{
			if (ProposalMenuConditionByType(proposalsMenuPriority[k]))
			{
				CurrentProposalMenu = proposalsMenuPriority[k];
				break;
			}
		}
		if (CurrentProposalMenu == ProposalMenuType.None)
		{
			if (IsSuperGiftAvailable)
			{
				CurrentProposalMenu = ProposalMenuType.SuperGift;
			}
			else if (statisticsManager.MathesCount >= minMatchesForFreeChest)
			{
				CurrentProposalMenu = ProposalMenuType.FreeChest;
			}
		}
		MenuScreen menuScreen = CachedManagers.CachedGUIScreenManager.MenuScreen;
		if (needUpdateMenuProposals && menuScreen != null)
		{
			menuScreen.UpdateProposals();
		}
	}

	public void ResetBuyCharacter()
	{
		if (buyCharacter != null)
		{
			buyCharacter.IsTry = false;
			buyCharacter = null;
		}
	}

	public bool IsProposalWithTimer(ProposalMenuType proposalType)
	{
		bool flag = false;
		flag = flag || proposalType == ProposalMenuType.VipPack;
		flag = flag || proposalType == ProposalMenuType.BuyPack;
		flag = flag || proposalType == ProposalMenuType.ChampionPack;
		flag = flag || proposalType == ProposalMenuType.Upgrade;
		return flag || proposalType == ProposalMenuType.X2;
	}

	public int TimerDurationByProposalType(ProposalMenuType proposalType)
	{
		int result = 0;
		bool flag = statisticsManager.SpentBucks >= MinSpentBucksForNewTimers;
		switch (proposalType)
		{
		case ProposalMenuType.BuyPack:
			result = ((!flag) ? timerPackDuration : timerPackDurationAfterPay);
			break;
		case ProposalMenuType.VipPack:
			result = ((!flag) ? timerVipPackDuration : timerVipPackDurationAfterPay);
			break;
		case ProposalMenuType.ChampionPack:
			result = ((!flag) ? timerChampionPackDuration : timerChampionPackDurationAfterPay);
			break;
		case ProposalMenuType.Upgrade:
			result = ((!flag) ? timerUpgradeDuration : timerUpgradeDurationAfterPay);
			break;
		case ProposalMenuType.X2:
			result = ((!flag) ? timerDoubleCoinsDuration : timerDoubleCoinsDurationAfterPay);
			break;
		}
		return result;
	}

	public void UpgradePackWasBought()
	{
		ProposalMenuType lastProposalWithTimerType = LastProposalWithTimerType;
		if (lastProposalWithTimerType == ProposalMenuType.Upgrade || lastProposalWithTimerType == ProposalMenuType.VipPack || lastProposalWithTimerType == ProposalMenuType.ChampionPack)
		{
			proposalTimer.UpdateTimer(float.MaxValue);
		}
	}

	public void ShowVideoForSaveTournametResult(Action<bool> callback)
	{
		Action<bool> videoCallback = delegate(bool result)
		{
			if (callback != null)
			{
				callback(result);
			}
		};
		CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoAvailable)
		{
			videoCallback(isVideoAvailable);
		});
	}

	private bool ProposalMenuConditionByType(ProposalMenuType proposalType)
	{
		bool result = false;
		switch (proposalType)
		{
		case ProposalMenuType.VipPack:
			result = IsVipPackAvailable;
			break;
		case ProposalMenuType.BuyPack:
			result = IsPackAvailable;
			break;
		case ProposalMenuType.ChampionPack:
			result = IsChampionPackAvailable;
			break;
		case ProposalMenuType.FreeChest:
			result = IsFreeChestAvailable;
			break;
		case ProposalMenuType.FreeCoins:
			result = IsFreeCoinsAvailable;
			break;
		case ProposalMenuType.Gift:
			result = IsGiftAvailable;
			break;
		case ProposalMenuType.SuperGift:
			result = IsSuperGiftAvailable;
			break;
		case ProposalMenuType.TryNew:
			result = IsTryCharacterAvailable;
			break;
		case ProposalMenuType.Upgrade:
			result = IsUpgradeAvailable;
			break;
		case ProposalMenuType.X2:
			result = IsDoubleCoinsAvailable;
			break;
		case ProposalMenuType.None:
			result = false;
			break;
		}
		ProposalMenuType lastProposalWithTimerType = LastProposalWithTimerType;
		if ((lastProposalWithTimerType == ProposalMenuType.VipPack && !IsVipPackAvailable) || (lastProposalWithTimerType == ProposalMenuType.BuyPack && !IsPackAvailable) || (lastProposalWithTimerType == ProposalMenuType.ChampionPack && !IsChampionPackAvailable) || (lastProposalWithTimerType == ProposalMenuType.Upgrade && !IsUpgradeAvailable) || (lastProposalWithTimerType == ProposalMenuType.X2 && !IsDoubleCoinsAvailable))
		{
			LastProposalWithTimerType = ProposalMenuType.None;
			LastProposalWithTimer = DateTime.Now;
		}
		if (lastProposalWithTimerType != ProposalMenuType.None && IsProposalWithTimer(proposalType) && proposalType != lastProposalWithTimerType)
		{
			result = false;
		}
		return result;
	}

	private void InitializeFreeChestTimer()
	{
		float value = 0f;
		int freeChestsOpened = statisticsManager.FreeChestsOpened;
		if (freeChestsOpened >= 1)
		{
			value = freeChestTimerDurations[Mathf.Clamp(freeChestsOpened - 1, 0, freeChestTimerDurations.Length - 1)];
			value -= (float)(DateTime.Now - LastFreeChestTimerStart).TotalSeconds;
		}
		freeChestTimer.TimerInSeconds = Mathf.Clamp(value, 0f, float.PositiveInfinity);
		freeChestTimer.IsTimerActive = true;
	}

	private int PackCharactersBought(ShopItemUpgradeNew item)
	{
		int num = 0;
		for (int i = 0; i < item.Characters.Length; i++)
		{
			if (item.Characters[i].Bought)
			{
				num++;
			}
		}
		return num;
	}

	private void SpendCoinsForTryCharacter()
	{
		CachedManagers.CachedStatisticsManager.DecreaseCoins(priceForTryCharacter);
		Action<bool> proposalCallback = GetProposalCallback();
		proposalCallback(true);
		Analytics.CustomEvent("Try_Character_Coins");
	}

	private void SpendGemsForTryCharacter()
	{
		CachedManagers.CachedStatisticsManager.DecreaseGems(priceForTryCharacterGems);
		Action<bool> proposalCallback = GetProposalCallback();
		proposalCallback(true);
		Analytics.CustomEvent("Try_Character_Gems");
	}

	private Action<bool> GetProposalCallback()
	{
		return delegate(bool result)
		{
			if (result)
			{
				ResetBuyCharacter();
				buyCharacter = tryCharacter;
				buyCharacter.IsTry = true;
				if ((bool)buyCharacter)
				{
					ShopItemCharacter shopItemCharacter = ObjectsSpawner.GetShopItemCharacter(objectsSpawner.FirstSelectedCharacterNameType);
					if (shopItemCharacter.Bought || shopItemCharacter.DefaultCharacter)
					{
						objectsSpawner.BeforeTrySelectedCharacterNameType = objectsSpawner.FirstSelectedCharacterNameType;
					}
					CharacterNameType firstSelectedCharacter = ((buyCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? buyCharacter.Infos[0].characterNameType : buyCharacter.CharacterUpgradeInfo.characterNameType);
					ObjectsSpawner.SetFirstSelectedCharacter(firstSelectedCharacter);
					CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.TryCharacter);
					GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW, GoliathIds.AdReward.CHARACTER);
				}
			}
			else
			{
				GameGoliath.Instance.TrackAdvertising(GoliathIds.AdType.VIDEO_AD, GoliathIds.AdPhase.SHOW);
			}
		};
	}

	private void GameManager_OnGameStop()
	{
		isAvalaibleMoreAmmo = false;
		isAvalaibleMoreTime = false;
		isAvalaibleMoreLife = false;
		isAvalaibleSecondFatalityChanse = false;
	}

	private void GameManager_OnGameStart()
	{
		isAvalaibleMoreAmmo = true;
		isAvalaibleMoreTime = true;
		isAvalaibleMoreLife = true;
		isAvalaibleSecondFatalityChanse = true;
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		if (shopItem == removeAds && OnRemoveAdsPurchaseFailed != null)
		{
			OnRemoveAdsPurchaseFailed();
		}
	}

	private void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		if (shopItem == removeAds && OnRemoveAdsPurchased != null)
		{
			OnRemoveAdsPurchased();
		}
		if (shopItem == buyPack && buyPack.FirstCharacter != null)
		{
			CharacterNameType firstSelectedCharacter = ((buyPack.FirstCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? buyPack.FirstCharacter.Infos[0].characterNameType : buyPack.FirstCharacter.CharacterUpgradeInfo.characterNameType);
			ObjectsSpawner.SetFirstSelectedCharacter(firstSelectedCharacter);
		}
		if (shopItem.GetType() == typeof(ShopItemNoAds))
		{
			Analytics.CustomEvent("No_Ads_Chest_Result_Bought");
		}
	}

	private void ProposalTimer_OnTimerChanged(int time)
	{
		if (time == 0)
		{
			switch (LastProposalWithTimerType)
			{
			case ProposalMenuType.VipPack:
			case ProposalMenuType.Upgrade:
			case ProposalMenuType.ChampionPack:
				LastUpgrade = DateTime.Now;
				LastChampionPack = DateTime.Now;
				LastVipPack = DateTime.Now;
				break;
			case ProposalMenuType.BuyPack:
				LastPack = DateTime.Now;
				break;
			case ProposalMenuType.X2:
				LastDoubleCoins = DateTime.Now;
				break;
			case ProposalMenuType.None:
				return;
			}
			LastProposalWithTimerType = ProposalMenuType.None;
			LastProposalWithTimer = DateTime.Now;
			GenerateProposalMenu(true);
		}
	}

	private void TapToStartScreen_OnLoaderRemoved()
	{
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (IsNeedShowForcedTimerPopup)
			{
				GetProposalMenu(null, LastProposalWithTimerType);
			}
		}, 0.5f);
		if (IsRateUsForcedAvailable && (bool)statisticsManager)
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.WhatsNew, true, null, true);
		}
		else if (UnityEngine.Random.value < chanseForShowInviteProposal && CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
		{
			CachedManagers.CachedInviteManager.SetInviteCharacter();
			GiftInviteCharacter currentInviteCharacter = CachedManagers.CachedInviteManager.CurrentInviteCharacter;
			if (currentInviteCharacter.inviteCharacter != CharacterNameType.None && (CachedManagers.CachedInviteManager.CurrentInviteCharacter.inviteWeight > 0f || CachedManagers.CachedInviteManager.CurrentInviteCharacter.forcedWeight > 0f))
			{
				ShopItemCharacter shopItemCharacter = ObjectsSpawner.GetShopItemCharacter(currentInviteCharacter.inviteCharacter);
				if (shopItemCharacter != null && !shopItemCharacter.Availible)
				{
					CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Invite, true, null, true);
				}
				else if (!CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
				{
					CustomPlayerPrefs.SetString("forced_rate_game", Application.version);
				}
			}
			else if (!CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
			{
				CustomPlayerPrefs.SetString("forced_rate_game", Application.version);
			}
		}
		else if (!CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
		{
			CustomPlayerPrefs.SetString("forced_rate_game", Application.version);
		}
		IsNeedShowInvitePopup = false;
	}

	private void LLTapdaqManager_OnTapdaqInterstitialFinish()
	{
		IsCrossPromoShowing = false;
	}

	private void LLTapdaqManager_OnTapdaqInterstitialShow(bool isShowed)
	{
		IsCrossPromoShowing = isShowed;
	}

	private void LLApplicationStateRegister_OnApplicationEnteredBackground(bool isBackgroundEntered)
	{
		if (!isBackgroundEntered)
		{
			if (!((DateTime.Now - crossPromoDateTime).TotalSeconds >= (double)delayForForcedCrossPromo) || CachedManagers.CachedStatisticsManager.MathesCount < minMatchesForSpecialEvents)
			{
				return;
			}
			GUIScreenType activeScreen = CachedManagers.CachedGUIScreenManager.ActiveScreen;
			if (activeScreen == GUIScreenType.Versus || activeScreen == GUIScreenType.Roulette || activeScreen == GUIScreenType.Shop || activeScreen == GUIScreenType.Game || CachedManagers.CachedSubscriptionManager.IsSubscriptionActive)
			{
				return;
			}
			if (!IsCrossPromoShowing && !CachedManagers.CachedAdvertisingManager.IsAdsShowing)
			{
				ShowInterstitialCrosspromo(true);
			}
			if (CachedManagers.CachedShopManager.TrialActivated || !CachedManagers.CachedRemoteManager.IsSubscriptionPopupShowed)
			{
				return;
			}
			ShopItem subscriptionWeek = CachedManagers.CachedShopManager.SubscriptionWeek;
			if (subscriptionWeek != null && subscriptionWeek.GetType() == typeof(ShopItemSubscription) && (subscriptionWeek as ShopItemSubscription).IsTrial)
			{
				GUIPopup activePopup = CachedManagers.CachedGUIPopupManager.ActivePopup;
				PopupType popupType = ((!(activePopup == null)) ? activePopup.TypePopup : PopupType.None);
				if (popupType != PopupType.AutoRenewableSubscribe && popupType != PopupType.AutoRenewableSubscribeFoolish && popupType != PopupType.AutoRenewableSubscribeReward)
				{
					CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.AutoRenewableSubscribeFoolish, true, null, true);
				}
			}
		}
		else
		{
			crossPromoDateTime = DateTime.Now;
		}
	}

	private void AdvertisingManager_OnVideoAdFinish(bool isVideoAvailable)
	{
		IsOnVideoFinishEventSubscribed = false;
		if (isVideoAvailable)
		{
			Analytics.CustomEvent("Try_Character_Video");
		}
	}

	private void MiniBankPopup_OnBankItemPurchsed(bool areGemsPurchsed)
	{
		if (CachedManagers.CachedGUIPopupManager.ActivePopup != null && CachedManagers.CachedGUIPopupManager.ActivePopup.TypePopup == PopupType.TryCharacterProposal)
		{
			if (areGemsPurchsed)
			{
				SpendGemsForTryCharacter();
			}
			else
			{
				SpendCoinsForTryCharacter();
			}
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 50, 150f, 50f), "Upgrade"))
		{
			ProposalTimer.TimerInSeconds = 1000f;
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Upgrade);
		}
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 100, 150f, 50f), "Champion"))
		{
			ProposalTimer.TimerInSeconds = 1000f;
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.ChampionPack);
		}
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 150, 150f, 50f), "VipPack"))
		{
			ProposalTimer.TimerInSeconds = 1000f;
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.VipPack);
		}
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 200, 150f, 50f), "DoubleCoins"))
		{
			ProposalTimer.TimerInSeconds = 1000f;
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.DoubleCoins);
		}
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 250, 150f, 50f), "CharactersPack"))
		{
			ProposalTimer.TimerInSeconds = 1000f;
			GenerateProposalMenu();
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Pack);
		}
		if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 300, 150f, 50f), "ShowCrossPromo"))
		{
			ShowInterstitialCrosspromo();
		}
	}
}
