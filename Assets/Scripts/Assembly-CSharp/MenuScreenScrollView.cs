using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Mit_Goliath_Native.Utils;
using Spine;
using UnityEngine;
using UnityEngine.Analytics;
using Debug = UnityEngine.Debug;

public class MenuScreenScrollView : GUIScreen
{
    private const string VIDEO_UNLOCK_DESCRIPTION = "ui.main.shop.fireman.unlock.description";

    private const string MULTIPLAYER_WIN_UNLOCK_DESCRIPTION = "ui.main.shop.santa.unlock.description";

    private const string VIDEO_UNLOCK_AD_FREE_ICON = "get_key_icon_rate";

    private const string VIDEO_UNLOCK_ICON = "get_key_icon_play";

    private const string MULTIPLAYER_WINS_UNLOCK_ICON = "icon_online_pvp";

    private const int ARROW_CHARACTER_COUNT = 3;

    private const int LOADED_CHARACTERS_SPRITE_COLLECTIONS_COUNT = 5;

    private const string MONEY_START = "Money_start";


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // CustomReward?.Invoke();
            // CustomReward=null;
            tk2dUIManager.Instance.InputEnabled = true;

        }
    }



    [CompilerGenerated]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static Action<bool> m_OnCharactersInitialized;

    [CompilerGenerated]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static Action m_OnDeleteMultiplayerChosenSprites;

    [CompilerGenerated]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static Action<MenuPhase> m_OnSetPhase;

    [SerializeField]
    private MenuScreenPartyMasters menuScreenPartyMasters;

    [SerializeField]
    private tk2dUIItem[] buyCharacterOnCoinsButtons;

    [SerializeField]
    private tk2dUIItem[] buyCharacterOnBucksButtons;

    [SerializeField]
    private GameObject[] buyCharacterOnlyBucksLayouter;

    [SerializeField]
    private tk2dUIItem[] buyCharacterOnlyBucksButtons;

    [SerializeField]
    private SkeletonAnimation[] buttonOnlyBucksLoading;

    [SerializeField]
    private GameObject[] buyCharacterOnlyBucksNonButtonLayouter;

    [SerializeField]
    private tk2dSlicedSprite[] buyCharacterOnlyBucksNonButtonSprite;

    [SerializeField]
    private tk2dUIItem[] buyCharacterOnlyBucksRouletteButtons;

    [SerializeField]
    private GameObject[] buyCharacterByLockedWithTimerLayouter;

    [SerializeField]
    private tk2dTextMesh[] buyCharacterByLockedWithTimerLabels;

    [SerializeField]
    private GameObject[] buyCharacterOnlyBucksWithoutRouletteLayouter;

    [SerializeField]
    private tk2dUIItem[] buyCharacterOnlyBucksWithoutRouletteButtons;

    [SerializeField]
    private SkeletonAnimation[] buttonOnlyBucksWithoutRouletteLoading;

    [SerializeField]
    private GameObject[] buyCharacterByInviteLayouter;

    [SerializeField]
    private tk2dUIItem[] buyCharacterByInviteButtons;

    [SerializeField]
    private tk2dUIItem[] buyCharacterByInviteForBucksButtons;

    [SerializeField]
    private SkeletonAnimation[] buttonByInviteLoading;

    [SerializeField]
    private GameObject[] buyCharacterBySubscribeLayouter;

    [SerializeField]
    private tk2dUIItem[] buyCharacterBySubscribeButtons;

    [SerializeField]
    private tk2dUIItem[] buyCharacterBySubscribeForBucksButtons;

    [SerializeField]
    private SkeletonAnimation[] buttonBySubscribeLoading;

    [SerializeField]
    private GameObject[] buyCharacterByAdsLayouter;

    [SerializeField]
    private tk2dUIItem[] buyCharacterByAdsButtons;

    [SerializeField]
    private tk2dUIItem[] buyCharacterByAdsForBucksButtons;

    [SerializeField]
    private SkeletonAnimation[] buttonByAdsLoading;

    [SerializeField]
    private GameObject[] buyCharacterBySubscriptionLayouter;

    [SerializeField]
    private GUILayouter[] dailySpecialLabels;

    [SerializeField]
    private GUILayouter[] legendaryTournamentLabels;

    [SerializeField]
    private GUILayouter[] videoSpecialLabels;

    [SerializeField]
    private tk2dTextMesh[] videoSpecialTopLabel;

    [SerializeField]
    private ProgressBar[] videoSpecialBar;

    [SerializeField]
    private tk2dBaseSprite[] videoSpecialIcons;

    [SerializeField]
    private ProgressBar[] multiplayerSpecialBar;

    [SerializeField]
    private tk2dTextMesh[] buttonCoinsLabels;

    [SerializeField]
    private tk2dTextMesh[] buttonBucksLabels;

    [SerializeField]
    private SkeletonAnimation[] buttonBucksLoading;

    [SerializeField]
    private ShopScrollView landscapeScrollView;

    [SerializeField]
    private ShopScrollView portraitScrollView;

    [SerializeField]
    private AnimationCurve scrollViewOffset;

    [SerializeField]
    private float cellsOffset;

    [SerializeField]
    private float cellsInterval;

    [SerializeField]
    private float inactiveScale = 0.7f;

    [SerializeField]
    private float cellsFadeOffset;

    [SerializeField]
    private float cellsFadeInterval = 0.1f;

    [SerializeField]
    private GUILayoutCell[] avatarCells;

    [SerializeField]
    private GameObject avatarBackground;

    [SerializeField]
    private SkeletonAnimation characterAvatarAnimation;

    [SerializeField]
    private tk2dTextMesh[] characterNameLabels;

    [SerializeField]
    private tk2dTextMesh[] characterDescriptionLabels;

    [SerializeField]
    private tk2dTextMesh[] characterWeaponDescriptionLabels;

    [SerializeField]
    private tk2dSprite[] characterWeaponIconSprites;

    [SerializeField]
    private tk2dSprite[] leftArrowsShop;

    [SerializeField]
    private tk2dSprite[] rightArrowsShop;

    [SerializeField]
    private Color arrowsShopMainColor;

    [SerializeField]
    private tk2dSprite[] characterNameFlagSprites;

    [SerializeField]
    private tk2dSprite[] characterNameFlagSpritesPremium;

    [SerializeField]
    private tk2dUIItem[] playButtons;

    [SerializeField]
    private GameObject[] upgradeLayouter;

    [SerializeField]
    private tk2dUIItem[] playButtonsWithUpgrade;

    [SerializeField]
    private tk2dUIItem[] upgradeButtons;

    [SerializeField]
    private tk2dUIItem[] subscribeButtons;

    [SerializeField]
    private SkeletonAnimation subscribeCatAnimationPortrait;

    [SerializeField]
    private SkeletonAnimation subscribeCatAnimationLandscape;

    [SerializeField]
    private AnimationSettings[] catAnimationsInTrackRepeatTimes;

    [SerializeField]
    private EffectSpawner moneyEffectSpawnerPortrait;

    [SerializeField]
    private EffectSpawner moneyEffectSpawnerLandscape;

    [SerializeField]
    private int matchesForShowSubscribeButtons;

    [SerializeField]
    private GameObject[] tournamentTryCharacterWarning;

    [SerializeField]
    private tk2dSprite chosePlayerOneSprite;

    [SerializeField]
    private float chooseDelay;

    private List<ShopItemCharacter> shopCharacters = new List<ShopItemCharacter>();

    private List<CharacterNameType> charactersWithLoadedSpriteCollections = new List<CharacterNameType>();

    private Dictionary<GUILayoutCell, SkeletonAnimation> animationAvatar = new Dictionary<GUILayoutCell, SkeletonAnimation>();

    private List<SkeletonAnimation> doubleAnimationAvatar = new List<SkeletonAnimation>();

    private List<tk2dSprite> firstPlayerChoseSprites = new List<tk2dSprite>();

    private ShopScrollView[] scrollViews;

    private TournamentDifficultType currentTournamentDifficult = TournamentDifficultType.Easy;

    private bool isCharactersInitialized;

    private bool multiplayerInvitationReceived;

    private MenuPhase currentPhase;

    private bool isIPad;

    private int selectedIndex = -1;

    private int lastSelectedIndex = -1;

    private IEnumerator characterAnimationCoroutine;

    private bool isCharacterFirstSelection = true;

    private bool secondPlayerMustChoose;

    private bool shouldBuyCharacterOnCoinsTutorialLoad;

    private bool shouldBuyCharacterOnGemsTutorialLoad;

    private Action characterChoosenCallback;

    private bool isCountryIdentifyAvailable = true;

    private EventData cachedEventStartMoneyPortrait;

    private EventData cachedEventStartMoneyLandscape;

    private List<GameObject> mainLocations = new List<GameObject>();

    private bool ShouldBuyCharacterOnCoinsTutorialLoad
    {
        set
        {
            if (shouldBuyCharacterOnCoinsTutorialLoad != value && !shouldBuyCharacterOnGemsTutorialLoad && value)
            {
                ShopItemCharacter shopItemCharacter = base.CachedManagers.CachedTutorialManager.LoadBuyCharacterOnCoinsTutorial(this);
                if (shopItemCharacter != null)
                {
                    shouldBuyCharacterOnCoinsTutorialLoad = true;
                    base.ObjectsSpawner.SetFirstSelectedCharacter(shopItemCharacter.Infos[0].characterNameType);
                }
            }
        }
    }

    private bool ShouldBuyCharacterOnGemsTutorialLoad
    {
        set
        {
            if (shouldBuyCharacterOnGemsTutorialLoad != value && value)
            {
                ShopItemCharacter shopItemCharacter = base.CachedManagers.CachedTutorialManager.LoadBuyCharacterOnCoinsTutorial(this);
                if (shopItemCharacter != null)
                {
                    shouldBuyCharacterOnGemsTutorialLoad = true;
                    base.ObjectsSpawner.SetFirstSelectedCharacter(shopItemCharacter.Infos[0].characterNameType);
                }
            }
        }
    }

    private bool IsCharactersInitialized
    {
        get
        {
            return isCharactersInitialized;
        }
        set
        {
            if (isCharactersInitialized != value)
            {
                isCharactersInitialized = value;
                MenuScreenScrollView.m_OnCharactersInitialized?.Invoke(value);
            }
        }
    }

    public Dictionary<GUILayoutCell, SkeletonAnimation> AnimationAvatar
    {
        get
        {
            return animationAvatar;
        }
        set
        {
            animationAvatar = value;
        }
    }

    public List<SkeletonAnimation> DoubleAnimationAvatar
    {
        get
        {
            return doubleAnimationAvatar;
        }
        set
        {
            doubleAnimationAvatar = value;
        }
    }

    public tk2dUIItem[] BuyCharacterOnCoinsButtons
    {
        get
        {
            return buyCharacterOnCoinsButtons;
        }
    }

    public tk2dUIItem[] UpgradeButtons
    {
        get
        {
            return upgradeButtons;
        }
    }

    public float IconInactiveScale
    {
        get
        {
            return inactiveScale;
        }
    }

    public int SelectedIndex
    {
        get
        {
            return selectedIndex;
        }
        set
        {
            if (selectedIndex == value)
            {
                return;
            }
            if (selectedIndex != -1)
            {
                AudioManager.PlayAudioWithKey("scroll");
            }
            selectedIndex = value;
            UpdateCharacterCharacteristics();
            tk2dSprite[] array = leftArrowsShop;
            foreach (tk2dSprite tk2dSprite2 in array)
            {
                if (selectedIndex < 3)
                {
                    tk2dSprite2.color = Color.clear;
                }
                else
                {
                    tk2dSprite2.color = arrowsShopMainColor;
                }
            }
            tk2dSprite[] array2 = rightArrowsShop;
            foreach (tk2dSprite tk2dSprite3 in array2)
            {
                if (selectedIndex > shopCharacters.Count - 3)
                {
                    tk2dSprite3.color = Color.clear;
                }
                else
                {
                    tk2dSprite3.color = arrowsShopMainColor;
                }
            }
            UpdateBuyButtons();
            base.CachedManagers.CachedTutorialManager.CharacterSelectedIndex = selectedIndex;
        }
    }

    private EventData CachedEventStartMoneyPortrait
    {
        get
        {
            if (cachedEventStartMoneyPortrait == null)
            {
                cachedEventStartMoneyPortrait = subscribeCatAnimationPortrait.Skeleton.Data.FindEvent("Money_start");
            }
            return cachedEventStartMoneyPortrait;
        }
    }

    private EventData CachedEventStartMoneyLandscape
    {
        get
        {
            if (cachedEventStartMoneyLandscape == null)
            {
                cachedEventStartMoneyLandscape = subscribeCatAnimationLandscape.Skeleton.Data.FindEvent("Money_start");
            }
            return cachedEventStartMoneyLandscape;
        }
    }

    private MenuScreenPartyMasters CachedMenuScreenPartyMasters
    {
        get
        {
            return menuScreenPartyMasters;
        }
    }

    public static event Action<bool> OnCharactersInitialized
    {
        add
        {
            Action<bool> action = MenuScreenScrollView.m_OnCharactersInitialized;
            Action<bool> action2;
            do
            {
                action2 = action;
                action = Interlocked.CompareExchange(ref MenuScreenScrollView.m_OnCharactersInitialized, (Action<bool>)Delegate.Combine(action2, value), action);
            }
            while (action != action2);
        }
        remove
        {
            Action<bool> action = MenuScreenScrollView.m_OnCharactersInitialized;
            Action<bool> action2;
            do
            {
                action2 = action;
                action = Interlocked.CompareExchange(ref MenuScreenScrollView.m_OnCharactersInitialized, (Action<bool>)Delegate.Remove(action2, value), action);
            }
            while (action != action2);
        }
    }

    public static event Action OnDeleteMultiplayerChosenSprites
    {
        add
        {
            Action action = MenuScreenScrollView.m_OnDeleteMultiplayerChosenSprites;
            Action action2;
            do
            {
                action2 = action;
                action = Interlocked.CompareExchange(ref MenuScreenScrollView.m_OnDeleteMultiplayerChosenSprites, (Action)Delegate.Combine(action2, value), action);
            }
            while (action != action2);
        }
        remove
        {
            Action action = MenuScreenScrollView.m_OnDeleteMultiplayerChosenSprites;
            Action action2;
            do
            {
                action2 = action;
                action = Interlocked.CompareExchange(ref MenuScreenScrollView.m_OnDeleteMultiplayerChosenSprites, (Action)Delegate.Remove(action2, value), action);
            }
            while (action != action2);
        }
    }

    public static event Action<MenuPhase> OnSetPhase
    {
        add
        {
            Action<MenuPhase> action = MenuScreenScrollView.m_OnSetPhase;
            Action<MenuPhase> action2;
            do
            {
                action2 = action;
                action = Interlocked.CompareExchange(ref MenuScreenScrollView.m_OnSetPhase, (Action<MenuPhase>)Delegate.Combine(action2, value), action);
            }
            while (action != action2);
        }
        remove
        {
            Action<MenuPhase> action = MenuScreenScrollView.m_OnSetPhase;
            Action<MenuPhase> action2;
            do
            {
                action2 = action;
                action = Interlocked.CompareExchange(ref MenuScreenScrollView.m_OnSetPhase, (Action<MenuPhase>)Delegate.Remove(action2, value), action);
            }
            while (action != action2);
        }
    }

    private void OnEnable()
    {
        base.CachedManagers.CachedStatisticsManager.LastRoulettePlayedDay = DateTime.Now;
        tk2dUIItem[] array = buyCharacterOnCoinsButtons;
        foreach (tk2dUIItem tk2dUIItem2 in array)
        {
            tk2dUIItem2.OnClick += BuyCharacterOnCoins_OnClick;
        }
        tk2dUIItem[] array2 = buyCharacterOnBucksButtons;
        foreach (tk2dUIItem tk2dUIItem3 in array2)
        {
            tk2dUIItem3.OnClick += BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array3 = buyCharacterOnlyBucksButtons;
        foreach (tk2dUIItem tk2dUIItem4 in array3)
        {
            tk2dUIItem4.OnClick += BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array4 = buyCharacterOnlyBucksWithoutRouletteButtons;
        foreach (tk2dUIItem tk2dUIItem5 in array4)
        {
            tk2dUIItem5.OnClick += BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array5 = buyCharacterByInviteButtons;
        foreach (tk2dUIItem tk2dUIItem6 in array5)
        {
            tk2dUIItem6.OnClick += BuyCharacterByInvite_OnClick;
        }
        tk2dUIItem[] array6 = buyCharacterByInviteForBucksButtons;
        foreach (tk2dUIItem tk2dUIItem7 in array6)
        {
            tk2dUIItem7.OnClick += BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array7 = buyCharacterBySubscribeButtons;
        foreach (tk2dUIItem tk2dUIItem8 in array7)
        {
            tk2dUIItem8.OnClick += BuyCharacterBySubscribe_OnClick;
        }
        tk2dUIItem[] array8 = buyCharacterByAdsButtons;
        foreach (tk2dUIItem tk2dUIItem9 in array8)
        {
            tk2dUIItem9.OnClick += BuyCharacterByAds_OnClick;
        }
        tk2dUIItem[] array9 = buyCharacterBySubscribeForBucksButtons;
        foreach (tk2dUIItem tk2dUIItem10 in array9)
        {
            tk2dUIItem10.OnClick += BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array10 = buyCharacterByAdsForBucksButtons;
        foreach (tk2dUIItem tk2dUIItem11 in array10)
        {
            tk2dUIItem11.OnClick += BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array11 = playButtons;
        foreach (tk2dUIItem tk2dUIItem12 in array11)
        {
            tk2dUIItem12.OnClick += PlayButton_OnClick;
        }
        tk2dUIItem[] array12 = playButtonsWithUpgrade;
        foreach (tk2dUIItem tk2dUIItem13 in array12)
        {
            tk2dUIItem13.OnClick += PlayButton_OnClick;
        }
        tk2dUIItem[] array13 = upgradeButtons;
        foreach (tk2dUIItem tk2dUIItem14 in array13)
        {
            tk2dUIItem14.OnClick += UpgradeButton_OnClick;
        }
        tk2dUIItem[] array14 = buyCharacterOnlyBucksRouletteButtons;
        foreach (tk2dUIItem tk2dUIItem15 in array14)
        {
            tk2dUIItem15.OnClick += RouletteButton_OnClick;
        }
        tk2dUIItem[] array15 = subscribeButtons;
        foreach (tk2dUIItem tk2dUIItem16 in array15)
        {
            tk2dUIItem16.OnClick += SubscribeButton_OnClick;
            tk2dUIItem16.gameObject.SetActive(!base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive && matchesForShowSubscribeButtons <= base.CachedManagers.CachedStatisticsManager.MathesCount);
            tk2dUIItem16.gameObject.SetActive(false);
        }
        MenuScreen.OnStartSpecialIdleAnimations += MenuScreen_OnStartSpecialIdleAnimations;
        MenuScreen.OnSecondPlayerMustChoose += MenuScreen_OnSecondPlayerMustChoose;
        MenuScreen.OnPlayButtonEnable += MenuScreen_OnPlayButtonEnable;
        MenuScreen.OnInitializeCharacters += MenuScreen_OnInitializeCharacters;
        MenuScreen.OnCheckBoughtCharacters += MenuScreen_OnCheckBoughtCharacters;
        MenuScreen.OnCharacterFirstSelection += MenuScreen_OnCharacterFirstSelection;
        MenuScreen.OnUpdateBuyButtons += MenuScreen_OnUpdateBuyButtons;
        MenuScreen.OnContentPositionChange += MenuScreen_OnContentPositionChange;
        MenuScreen.OnCurrentTournamentDifficult += MenuScreen_OnCurrentTournamentDifficult;
        MenuScreen.OnMultiplayerInvitationReceived += MenuScreen_OnMultiplayerInvitationReceived;
        MenuScreen.OnCharacterChoosenCallback += MenuScreen_OnCharacterChoosenCallback;
        MenuScreen.OnDeleteMultiplayerChosenSprites += MenuScreen_OnDeleteMultiplayerChosenSprites;
        MenuScreen.OnChangeCurrentPhase += MenuScreen_OnChangeCurrentPhase;
        MenuScreen.OnShouldBuyCharacterOnCoinsTutorialLoad += MenuScreen_OnShouldBuyCharacterOnCoinsTutorialLoad;
        MenuScreen.OnShouldBuyCharacterOnGemsTutorialLoad += MenuScreen_OnShouldBuyCharacterOnGemsTutorialLoad;
        MultiplayerManager.OnMatchCreated += MultiplayerManager_OnMatchCreated;
        ShopItemSubscription.OnSubscriptionActive = (Action)Delegate.Combine(ShopItemSubscription.OnSubscriptionActive, new Action(ShopItemSubscription_OnSubscriptionActive));
        subscribeCatAnimationPortrait.State.Event += SubscribeCatAnimationPortrait_State_Event;
        subscribeCatAnimationLandscape.State.Event += SubscribeCatAnimationLandscape_State_Event;
        scrollViews = new ShopScrollView[2] { landscapeScrollView, portraitScrollView };
        if (isCharactersInitialized)
        {
            base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
            {
                if (this != null)
                {
                    CheckBoughtCharacters();
                    UpdateScrollViewPosition();
                }
            }, 0f);
        }
        ShopScrollView[] array16 = scrollViews;
        foreach (ShopScrollView shopScrollView in array16)
        {
            shopScrollView.OnContentPositionChange = (Action)Delegate.Combine(shopScrollView.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
        }
        isIPad = SystemInfo.deviceModel.Contains("iPad");
        if (characterAvatarAnimation != null)
        {
            GUILayoutCell[] array17 = avatarCells;
            foreach (GUILayoutCell gUILayoutCell in array17)
            {
                SkeletonAnimation skeletonAnimation = UnityEngine.Object.Instantiate(characterAvatarAnimation);
                skeletonAnimation.transform.parent = gUILayoutCell.transform;
                skeletonAnimation.transform.localPosition = Vector3.zero;
                skeletonAnimation.transform.localScale = Vector3.one;
                animationAvatar.Add(gUILayoutCell, skeletonAnimation);
            }
        }
        base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
        {
            CharacterNameType item = ((shopCharacters[selectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[selectedIndex].Infos[0].characterNameType : shopCharacters[selectedIndex].CharacterUpgradeInfo.characterNameType);
            base.CachedManagers.CachedResourceManager.UnloadEachCharacterSpriteCollections(new List<CharacterNameType> { item }, 0.1f);
        }, 0f);
    }

    private void OnDisable()
    {
        tk2dUIItem[] array = buyCharacterOnCoinsButtons;
        foreach (tk2dUIItem tk2dUIItem2 in array)
        {
            tk2dUIItem2.OnClick -= BuyCharacterOnCoins_OnClick;
        }
        tk2dUIItem[] array2 = buyCharacterOnBucksButtons;
        foreach (tk2dUIItem tk2dUIItem3 in array2)
        {
            tk2dUIItem3.OnClick -= BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array3 = buyCharacterOnlyBucksButtons;
        foreach (tk2dUIItem tk2dUIItem4 in array3)
        {
            tk2dUIItem4.OnClick -= BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array4 = buyCharacterOnlyBucksWithoutRouletteButtons;
        foreach (tk2dUIItem tk2dUIItem5 in array4)
        {
            tk2dUIItem5.OnClick -= BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array5 = buyCharacterByInviteButtons;
        foreach (tk2dUIItem tk2dUIItem6 in array5)
        {
            tk2dUIItem6.OnClick -= BuyCharacterByInvite_OnClick;
        }
        tk2dUIItem[] array6 = buyCharacterByInviteForBucksButtons;
        foreach (tk2dUIItem tk2dUIItem7 in array6)
        {
            tk2dUIItem7.OnClick -= BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array7 = buyCharacterBySubscribeButtons;
        foreach (tk2dUIItem tk2dUIItem8 in array7)
        {
            tk2dUIItem8.OnClick -= BuyCharacterBySubscribe_OnClick;
        }
        tk2dUIItem[] array8 = buyCharacterByAdsButtons;
        foreach (tk2dUIItem tk2dUIItem9 in array8)
        {
            tk2dUIItem9.OnClick -= BuyCharacterByAds_OnClick;
        }
        tk2dUIItem[] array9 = buyCharacterBySubscribeForBucksButtons;
        foreach (tk2dUIItem tk2dUIItem10 in array9)
        {
            tk2dUIItem10.OnClick -= BuyCharacterOnBucks_OnClick;
        }
        tk2dUIItem[] array10 = buyCharacterByAdsForBucksButtons;
        foreach (tk2dUIItem tk2dUIItem11 in array10)
        {
            tk2dUIItem11.OnClick -= BuyCharacterOnBucks_OnClick;
        }
        ShopScrollView[] array11 = scrollViews;
        foreach (ShopScrollView shopScrollView in array11)
        {
            shopScrollView.OnContentPositionChange = (Action)Delegate.Remove(shopScrollView.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
        }
        tk2dUIItem[] array12 = playButtons;
        foreach (tk2dUIItem tk2dUIItem12 in array12)
        {
            tk2dUIItem12.OnClick -= PlayButton_OnClick;
        }
        tk2dUIItem[] array13 = playButtonsWithUpgrade;
        foreach (tk2dUIItem tk2dUIItem13 in array13)
        {
            tk2dUIItem13.OnClick -= PlayButton_OnClick;
        }
        tk2dUIItem[] array14 = upgradeButtons;
        foreach (tk2dUIItem tk2dUIItem14 in array14)
        {
            tk2dUIItem14.OnClick -= UpgradeButton_OnClick;
        }
        tk2dUIItem[] array15 = subscribeButtons;
        foreach (tk2dUIItem tk2dUIItem15 in array15)
        {
            tk2dUIItem15.OnClick -= SubscribeButton_OnClick;
        }
        tk2dUIItem[] array16 = buyCharacterOnlyBucksRouletteButtons;
        foreach (tk2dUIItem tk2dUIItem16 in array16)
        {
            tk2dUIItem16.OnClick -= RouletteButton_OnClick;
        }
        MenuScreen.OnStartSpecialIdleAnimations -= MenuScreen_OnStartSpecialIdleAnimations;
        MenuScreen.OnSecondPlayerMustChoose -= MenuScreen_OnSecondPlayerMustChoose;
        MenuScreen.OnPlayButtonEnable -= MenuScreen_OnPlayButtonEnable;
        MenuScreen.OnInitializeCharacters -= MenuScreen_OnInitializeCharacters;
        MenuScreen.OnCheckBoughtCharacters -= MenuScreen_OnCheckBoughtCharacters;
        MenuScreen.OnCharacterFirstSelection -= MenuScreen_OnCharacterFirstSelection;
        MenuScreen.OnUpdateBuyButtons -= MenuScreen_OnUpdateBuyButtons;
        MenuScreen.OnContentPositionChange -= MenuScreen_OnContentPositionChange;
        MenuScreen.OnCurrentTournamentDifficult -= MenuScreen_OnCurrentTournamentDifficult;
        MenuScreen.OnMultiplayerInvitationReceived -= MenuScreen_OnMultiplayerInvitationReceived;
        MenuScreen.OnCharacterChoosenCallback -= MenuScreen_OnCharacterChoosenCallback;
        MenuScreen.OnDeleteMultiplayerChosenSprites -= MenuScreen_OnDeleteMultiplayerChosenSprites;
        MenuScreen.OnChangeCurrentPhase -= MenuScreen_OnChangeCurrentPhase;
        MenuScreen.OnShouldBuyCharacterOnCoinsTutorialLoad -= MenuScreen_OnShouldBuyCharacterOnCoinsTutorialLoad;
        MenuScreen.OnShouldBuyCharacterOnGemsTutorialLoad -= MenuScreen_OnShouldBuyCharacterOnGemsTutorialLoad;
        MultiplayerManager.OnMatchCreated -= MultiplayerManager_OnMatchCreated;
        ShopItemSubscription.OnSubscriptionActive = (Action)Delegate.Remove(ShopItemSubscription.OnSubscriptionActive, new Action(ShopItemSubscription_OnSubscriptionActive));
        subscribeCatAnimationPortrait.State.Event -= SubscribeCatAnimationPortrait_State_Event;
        subscribeCatAnimationLandscape.State.Event -= SubscribeCatAnimationLandscape_State_Event;
        List<GUILayoutCell> list = new List<GUILayoutCell>(animationAvatar.Keys);
        foreach (GUILayoutCell item in list)
        {
            if (animationAvatar[item] != null)
            {
                UnityEngine.Object.Destroy(animationAvatar[item].gameObject);
                animationAvatar[item] = null;
            }
        }
        animationAvatar.Clear();
        if (DoubleAnimationAvatar == null)
        {
            return;
        }
        for (int num11 = 0; num11 < DoubleAnimationAvatar.Count; num11++)
        {
            if (DoubleAnimationAvatar[num11] != null)
            {
                UnityEngine.Object.Destroy(DoubleAnimationAvatar[num11].gameObject);
                DoubleAnimationAvatar[num11] = null;
            }
        }
        DoubleAnimationAvatar.Clear();
    }

    private void Awake()
    {
        if (tk2dSystem.IsRetina)
        {
            cellsInterval *= 2f;
            cellsOffset *= 2f;
            cellsFadeOffset *= 2f;
        }
    }

    private void OnDestroy()
    {
        ResourceManager instanceIfExist = SingletonMonoBehaviour<ResourceManager>.InstanceIfExist;
        LevelsManager instanceIfExist2 = SingletonMonoBehaviour<LevelsManager>.InstanceIfExist;
        if (instanceIfExist != null && instanceIfExist2 != null)
        {
            ObjectsSpawner objectsSpawner = instanceIfExist2.ObjectsSpawner;
            charactersWithLoadedSpriteCollections.Clear();
            if (objectsSpawner.FirstSelectedCharacterNameType != CharacterNameType.None)
            {
                charactersWithLoadedSpriteCollections.Add(objectsSpawner.FirstSelectedCharacterNameType);
            }
            if (objectsSpawner.SecondSelectedCharacterNameType != CharacterNameType.None)
            {
                charactersWithLoadedSpriteCollections.Add(objectsSpawner.SecondSelectedCharacterNameType);
            }
            instanceIfExist.UnloadEachCharacterSpriteCollections(charactersWithLoadedSpriteCollections);
            instanceIfExist.UnloadSkeletonSpriteCollections(0.5f);
            instanceIfExist.UnloadGeneralSpriteCollections();
        }
    }

    private void Start()
    {
        SetCatAnimationTrack(subscribeCatAnimationPortrait);
        SetCatAnimationTrack(subscribeCatAnimationLandscape);
    }

    public void UpdateBuyButtons()
    {
        ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
        GiftManager cachedGiftManager = base.CachedManagers.CachedGiftManager;
        if (!shopItemCharacter)
        {
            return;
        }
        // if (shopItemCharacter.CanBuyInShop)
        // {
        // 	if (shopItemCharacter.IsPremium && !shopItemCharacter.IsWithoutRoulette)
        // 	{
        // 		if (!shopItemCharacter.Availible)
        // 		{
        // 			bool flag = !base.CachedManagers.CachedProposalManager.AreRouletteCharactersAvailable || base.CachedManagers.CachedStatisticsManager.RoulettePlayedCharacters.Contains((int)shopItemCharacter.Infos[0].characterNameType);
        // 			tk2dUIItem[] array = buyCharacterOnlyBucksRouletteButtons;
        // 			foreach (tk2dUIItem tk2dUIItem2 in array)
        // 			{
        // 				tk2dUIItem2.gameObject.SetActive(!flag);
        // 			}
        // 			GameObject[] array2 = buyCharacterOnlyBucksNonButtonLayouter;
        // 			foreach (GameObject gameObject in array2)
        // 			{
        // 				gameObject.SetActive(flag);
        // 			}
        // 			tk2dSlicedSprite[] array3 = buyCharacterOnlyBucksNonButtonSprite;
        // 			foreach (tk2dSlicedSprite tk2dSlicedSprite2 in array3)
        // 			{
        // 				tk2dSlicedSprite2.gameObject.SetActive(flag);
        // 			}
        // 		}
        // 		GameObject[] array4 = buyCharacterOnlyBucksLayouter;
        // 		foreach (GameObject gameObject2 in array4)
        // 		{
        // 			gameObject2.SetActive(!shopItemCharacter.Availible);
        // 		}
        // 		GameObject[] array5 = buyCharacterOnlyBucksWithoutRouletteLayouter;
        // 		foreach (GameObject gameObject3 in array5)
        // 		{
        // 			gameObject3.SetActive(false);
        // 		}
        // 		tk2dUIItem[] array6 = buyCharacterOnCoinsButtons;
        // 		foreach (tk2dUIItem tk2dUIItem3 in array6)
        // 		{
        // 			tk2dUIItem3.gameObject.SetActive(false);
        // 		}
        // 		tk2dUIItem[] array7 = buyCharacterOnBucksButtons;
        // 		foreach (tk2dUIItem tk2dUIItem4 in array7)
        // 		{
        // 			tk2dUIItem4.gameObject.SetActive(false);
        // 		}
        // 	}
        // 	else if (shopItemCharacter.IsPremium && shopItemCharacter.IsWithoutRoulette)
        // 	{
        // 		GameObject[] array8 = buyCharacterOnlyBucksWithoutRouletteLayouter;
        // 		foreach (GameObject gameObject4 in array8)
        // 		{
        // 			gameObject4.SetActive(!shopItemCharacter.Availible);
        // 		}
        // 		GameObject[] array9 = buyCharacterOnlyBucksLayouter;
        // 		foreach (GameObject gameObject5 in array9)
        // 		{
        // 			gameObject5.SetActive(false);
        // 		}
        // 		tk2dUIItem[] array10 = buyCharacterOnCoinsButtons;
        // 		foreach (tk2dUIItem tk2dUIItem5 in array10)
        // 		{
        // 			tk2dUIItem5.gameObject.SetActive(false);
        // 		}
        // 		tk2dUIItem[] array11 = buyCharacterOnBucksButtons;
        // 		foreach (tk2dUIItem tk2dUIItem6 in array11)
        // 		{
        // 			tk2dUIItem6.gameObject.SetActive(false);
        // 		}
        // 	}
        // 	else
        // 	{
        tk2dUIItem[] array12 = buyCharacterOnCoinsButtons;
        foreach (tk2dUIItem tk2dUIItem7 in array12)
        {
            // tk2dUIItem7.gameObject.SetActive(true);
            tk2dUIItem7.gameObject.SetActive(!shopItemCharacter.Availible);
        }
        tk2dUIItem[] array13 = buyCharacterOnBucksButtons;
        foreach (tk2dUIItem tk2dUIItem8 in array13)
        {
            tk2dUIItem8.gameObject.SetActive(!shopItemCharacter.Availible);
            // tk2dUIItem8.gameObject.SetActive(true);
        }
        GameObject[] array14 = buyCharacterOnlyBucksLayouter;
        foreach (GameObject gameObject6 in array14)
        {
            gameObject6.SetActive(false);
        }
        GameObject[] array15 = buyCharacterOnlyBucksWithoutRouletteLayouter;
        foreach (GameObject gameObject7 in array15)
        {
            gameObject7.SetActive(false);
        }
        // }
        GUILayouter[] array16 = dailySpecialLabels;
        foreach (GUILayouter gUILayouter in array16)
        {
            gUILayouter.gameObject.SetActive(false);
        }
        GUILayouter[] array17 = legendaryTournamentLabels;
        foreach (GUILayouter gUILayouter2 in array17)
        {
            gUILayouter2.gameObject.SetActive(false);
        }
        GUILayouter[] array18 = videoSpecialLabels;
        foreach (GUILayouter gUILayouter3 in array18)
        {
            gUILayouter3.gameObject.SetActive(false);
        }
        GameObject[] array19 = buyCharacterByInviteLayouter;
        foreach (GameObject gameObject8 in array19)
        {
            gameObject8.SetActive(false);
        }
        GameObject[] array20 = buyCharacterBySubscribeLayouter;
        foreach (GameObject gameObject9 in array20)
        {
            gameObject9.SetActive(false);
        }
        GameObject[] array21 = buyCharacterByAdsLayouter;
        foreach (GameObject gameObject10 in array21)
        {
            gameObject10.SetActive(false);
        }
        GameObject[] array22 = buyCharacterBySubscriptionLayouter;
        foreach (GameObject gameObject11 in array22)
        {
            gameObject11.SetActive(false);
        }
        // }
        // else
        // {
        // 	GameObject[] array23 = buyCharacterOnlyBucksLayouter;
        // 	foreach (GameObject gameObject12 in array23)
        // 	{
        // 		gameObject12.SetActive(false);
        // 	}
        // 	GameObject[] array24 = buyCharacterOnlyBucksWithoutRouletteLayouter;
        // 	foreach (GameObject gameObject13 in array24)
        // 	{
        // 		gameObject13.SetActive(false);
        // 	}
        // 	tk2dUIItem[] array25 = buyCharacterOnCoinsButtons;
        // 	foreach (tk2dUIItem tk2dUIItem9 in array25)
        // 	{
        // 		tk2dUIItem9.gameObject.SetActive(false);
        // 	}
        // 	tk2dUIItem[] array26 = buyCharacterOnBucksButtons;
        // 	foreach (tk2dUIItem tk2dUIItem10 in array26)
        // 	{
        // 		tk2dUIItem10.gameObject.SetActive(false);
        // 	}
        // 	GUILayouter[] array27 = dailySpecialLabels;
        // 	foreach (GUILayouter gUILayouter4 in array27)
        // 	{
        // 		gUILayouter4.gameObject.SetActive(shopItemCharacter.IsDailySpecialGift && !shopItemCharacter.Availible);
        // 	}
        // 	GUILayouter[] array28 = legendaryTournamentLabels;
        // 	foreach (GUILayouter gUILayouter5 in array28)
        // 	{
        // 		gUILayouter5.gameObject.SetActive(shopItemCharacter.IsLegendaryTournamentReward && !shopItemCharacter.Availible);
        // 	}
        // 	GameObject[] array29 = buyCharacterByInviteLayouter;
        // 	foreach (GameObject gameObject14 in array29)
        // 	{
        // 		gameObject14.SetActive(shopItemCharacter.IsInviteReward && !shopItemCharacter.Availible);
        // 	}
        // 	GameObject[] array30 = buyCharacterBySubscribeLayouter;
        // 	foreach (GameObject gameObject15 in array30)
        // 	{
        // 		gameObject15.SetActive((shopItemCharacter.IsSubscribeReward || shopItemCharacter.IsOnlySubscribeReward) && !shopItemCharacter.Availible);
        // 	}
        // 	GameObject[] array31 = buyCharacterByAdsLayouter;
        // 	foreach (GameObject gameObject16 in array31)
        // 	{
        // 		gameObject16.SetActive(shopItemCharacter.LockedByAdsInfo.isLockedByAds && !shopItemCharacter.Availible);
        // 	}
        // 	GameObject[] array32 = buyCharacterBySubscriptionLayouter;
        // 	foreach (GameObject gameObject17 in array32)
        // 	{
        // 		gameObject17.SetActive(shopItemCharacter.IsIncludedInSubscription && !shopItemCharacter.Availible);
        // 	}
        // 	if (!shopItemCharacter.Availible && (shopItemCharacter.IsVideoSpecialGift || shopItemCharacter.IsMultiplayerWinsGift))
        // 	{
        // 		GUILayouter[] array33 = videoSpecialLabels;
        // 		foreach (GUILayouter gUILayouter6 in array33)
        // 		{
        // 			gUILayouter6.gameObject.SetActive(true);
        // 		}
        // 		GameObject[] array34 = buyCharacterOnlyBucksLayouter;
        // 		foreach (GameObject gameObject18 in array34)
        // 		{
        // 			gameObject18.SetActive(false);
        // 		}
        // 		GameObject[] array35 = buyCharacterOnlyBucksWithoutRouletteLayouter;
        // 		foreach (GameObject gameObject19 in array35)
        // 		{
        // 			gameObject19.SetActive(false);
        // 		}
        // 		ProgressBar[] array36 = videoSpecialBar;
        // 		foreach (ProgressBar progressBar in array36)
        // 		{
        // 			progressBar.gameObject.SetActive(false);
        // 		}
        // 		ProgressBar[] array37 = multiplayerSpecialBar;
        // 		foreach (ProgressBar progressBar2 in array37)
        // 		{
        // 			progressBar2.gameObject.SetActive(false);
        // 		}
        // 		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
        // 		int num32 = 0;
        // 		string sprite = string.Empty;
        // 		string key = string.Empty;
        // 		int num33 = 0;
        // 		Vector3 scale = Vector3.one;
        // 		ProgressBar[] array38 = null;
        // 		if (shopItemCharacter.IsVideoSpecialGift)
        // 		{
        // 			num32 = cachedStatisticsManager.VideosWatched;
        // 			sprite = "get_key_icon_play";
        // 			key = "ui.main.shop.fireman.unlock.description";
        // 			num33 = cachedGiftManager.VideosForGiftCharacter;
        // 			array38 = videoSpecialBar;
        // 		}
        // 		else if (shopItemCharacter.IsMultiplayerWinsGift)
        // 		{
        // 			num32 = cachedStatisticsManager.MatchesWinsMultiplayer;
        // 			sprite = "icon_online_pvp";
        // 			scale = new Vector3(0.55f, 0.55f, 1f);
        // 			key = "ui.main.shop.santa.unlock.description";
        // 			num33 = cachedGiftManager.MultiplayerWinsForGiftCharacter;
        // 			array38 = multiplayerSpecialBar;
        // 		}
        // 		tk2dBaseSprite[] array39 = videoSpecialIcons;
        // 		foreach (tk2dBaseSprite tk2dBaseSprite2 in array39)
        // 		{
        // 			tk2dBaseSprite2.SetSprite(sprite);
        // 			tk2dBaseSprite2.scale = scale;
        // 		}
        // 		string text = string.Format("{0} {1}/{2}", base.CachedManagers.CachedLocalisationManager.GetTextByKey(key), num32, num33);
        // 		tk2dTextMesh[] array40 = videoSpecialTopLabel;
        // 		foreach (tk2dTextMesh tk2dTextMesh2 in array40)
        // 		{
        // 			tk2dTextMesh2.text = text;
        // 		}
        // 		float f = (float)num32 / (float)num33;
        // 		ProgressBar[] array41 = array38;
        // 		foreach (ProgressBar progressBar3 in array41)
        // 		{
        // 			progressBar3.gameObject.SetActive(true);
        // 			progressBar3.SetFillAmount(f);
        // 		}
        // 	}
        // 	else
        // 	{
        // 		GameObject[] array42 = buyCharacterOnlyBucksLayouter;
        // 		foreach (GameObject gameObject20 in array42)
        // 		{
        // 			gameObject20.SetActive(false);
        // 		}
        // 		GameObject[] array43 = buyCharacterOnlyBucksWithoutRouletteLayouter;
        // 		foreach (GameObject gameObject21 in array43)
        // 		{
        // 			gameObject21.SetActive(false);
        // 		}
        // 		GUILayouter[] array44 = videoSpecialLabels;
        // 		foreach (GUILayouter gUILayouter7 in array44)
        // 		{
        // 			gUILayouter7.gameObject.SetActive(false);
        // 		}
        // 	}
        // }
        tk2dTextMesh[] array45 = buttonCoinsLabels;
        foreach (tk2dTextMesh tk2dTextMesh3 in array45)
        {
            tk2dTextMesh3.text = shopItemCharacter.PriceCoinsString;
        }
        // string priceString = shopItemCharacter.PriceString;
        string priceString = "观看视频";
        if (string.IsNullOrEmpty(priceString))
        {
            tk2dTextMesh[] array46 = buttonBucksLabels;
            foreach (tk2dTextMesh tk2dTextMesh4 in array46)
            {
                // tk2dTextMesh4.gameObject.SetActive(false);


                tk2dTextMesh4.gameObject.SetActive(true);
                tk2dTextMesh4.text = "观看视频";
            }
            SkeletonAnimation[] array47 = buttonBucksLoading;
            foreach (SkeletonAnimation skeletonAnimation in array47)
            {
                skeletonAnimation.gameObject.SetActive(false);
            }
            SkeletonAnimation[] array48 = buttonOnlyBucksLoading;
            foreach (SkeletonAnimation skeletonAnimation2 in array48)
            {
                skeletonAnimation2.gameObject.SetActive(true);
            }
            SkeletonAnimation[] array49 = buttonOnlyBucksWithoutRouletteLoading;
            foreach (SkeletonAnimation skeletonAnimation3 in array49)
            {
                skeletonAnimation3.gameObject.SetActive(true);
            }
            SkeletonAnimation[] array50 = buttonByInviteLoading;
            foreach (SkeletonAnimation skeletonAnimation4 in array50)
            {
                skeletonAnimation4.gameObject.SetActive(true);
            }
            SkeletonAnimation[] array51 = buttonBySubscribeLoading;
            foreach (SkeletonAnimation skeletonAnimation5 in array51)
            {
                skeletonAnimation5.gameObject.SetActive(true);
            }
            SkeletonAnimation[] array52 = buttonByAdsLoading;
            foreach (SkeletonAnimation skeletonAnimation6 in array52)
            {
                skeletonAnimation6.gameObject.SetActive(true);
            }
        }
        else
        {
            tk2dTextMesh[] array53 = buttonBucksLabels;
            foreach (tk2dTextMesh tk2dTextMesh5 in array53)
            {
                tk2dTextMesh5.gameObject.SetActive(true);
                // tk2dTextMesh5.text = shopItemCharacter.PriceString;
                tk2dTextMesh5.text = "观看视频";
            }
            SkeletonAnimation[] array54 = buttonBucksLoading;
            foreach (SkeletonAnimation skeletonAnimation7 in array54)
            {
                skeletonAnimation7.gameObject.SetActive(false);
            }
            SkeletonAnimation[] array55 = buttonOnlyBucksLoading;
            foreach (SkeletonAnimation skeletonAnimation8 in array55)
            {
                skeletonAnimation8.gameObject.SetActive(false);
            }
            SkeletonAnimation[] array56 = buttonOnlyBucksWithoutRouletteLoading;
            foreach (SkeletonAnimation skeletonAnimation9 in array56)
            {
                skeletonAnimation9.gameObject.SetActive(false);
            }
            SkeletonAnimation[] array57 = buttonByInviteLoading;
            foreach (SkeletonAnimation skeletonAnimation10 in array57)
            {
                skeletonAnimation10.gameObject.SetActive(false);
            }
            SkeletonAnimation[] array58 = buttonBySubscribeLoading;
            foreach (SkeletonAnimation skeletonAnimation11 in array58)
            {
                skeletonAnimation11.gameObject.SetActive(false);
            }
            SkeletonAnimation[] array59 = buttonByAdsLoading;
            foreach (SkeletonAnimation skeletonAnimation12 in array59)
            {
                skeletonAnimation12.gameObject.SetActive(false);
            }
        }
        bool flag2 = base.CachedManagers.CachedGameManager.CurrentGameType == GameType.Tournament && shopItemCharacter.IsTry;
        GameObject[] array60 = tournamentTryCharacterWarning;
        foreach (GameObject gameObject22 in array60)
        {
            gameObject22.SetActive(flag2);
        }
        tk2dUIItem[] array61 = playButtons;
        foreach (tk2dUIItem tk2dUIItem11 in array61)
        {
            tk2dUIItem11.gameObject.SetActive(!flag2 && (!shopItemCharacter.IsUpgradeAvailable || (shopItemCharacter.IsUpgradeAvailable && shopItemCharacter.IsTry)));
        }
        if (!flag2)
        {
            tk2dUIItem[] array62 = playButtons;
            foreach (tk2dUIItem tk2dUIItem12 in array62)
            {
                tk2dUIItem12.gameObject.SetActive(shopItemCharacter.Availible && (!shopItemCharacter.IsUpgradeAvailable || (shopItemCharacter.IsUpgradeAvailable && shopItemCharacter.IsTry)));
            }
        }
        GameObject[] array63 = upgradeLayouter;
        foreach (GameObject gameObject23 in array63)
        {
            gameObject23.SetActive(!flag2 && shopItemCharacter.IsUpgradeAvailable && !shopItemCharacter.IsTry);
        }
        if (!flag2)
        {
            GameObject[] array64 = upgradeLayouter;
            foreach (GameObject gameObject24 in array64)
            {
                gameObject24.gameObject.SetActive(shopItemCharacter.Availible && shopItemCharacter.IsUpgradeAvailable && !shopItemCharacter.IsTry);
            }
        }
        if (shopItemCharacter.IsLockedByTimer && !shopItemCharacter.Bought)
        {
            List<LockedByTimerCharacter> lockedByTimerCharacters = cachedGiftManager.LockedByTimerCharacters;
            for (int num60 = 0; num60 < lockedByTimerCharacters.Count; num60++)
            {
                if (lockedByTimerCharacters[num60].lockedCharacter == shopItemCharacter.Infos[0].characterNameType)
                {
                    cachedGiftManager.CurrentLockedCharacter = lockedByTimerCharacters[num60];
                    tk2dTextMesh[] array65 = buyCharacterByLockedWithTimerLabels;
                    foreach (tk2dTextMesh tk2dTextMesh6 in array65)
                    {
                        tk2dTextMesh6.text = cachedGiftManager.TimerInfo;
                    }
                    GameObject[] array66 = buyCharacterByLockedWithTimerLayouter;
                    foreach (GameObject gameObject25 in array66)
                    {
                        gameObject25.SetActive(!cachedGiftManager.IsUnlocked);
                    }
                    GameObject[] array67 = buyCharacterByInviteLayouter;
                    foreach (GameObject gameObject26 in array67)
                    {
                        gameObject26.SetActive(cachedGiftManager.IsUnlocked && !shopItemCharacter.Bought && shopItemCharacter.IsInviteReward);
                    }
                    GameObject[] array68 = buyCharacterBySubscribeLayouter;
                    foreach (GameObject gameObject27 in array68)
                    {
                        gameObject27.SetActive((cachedGiftManager.IsUnlocked && !shopItemCharacter.Bought && shopItemCharacter.IsSubscribeReward) || shopItemCharacter.IsOnlySubscribeReward);
                    }
                    GameObject[] array69 = buyCharacterByAdsLayouter;
                    foreach (GameObject gameObject28 in array69)
                    {
                        gameObject28.SetActive(cachedGiftManager.IsUnlocked && !shopItemCharacter.Bought && shopItemCharacter.LockedByAdsInfo.isLockedByAds);
                    }
                    GameObject[] array70 = buyCharacterOnlyBucksWithoutRouletteLayouter;
                    foreach (GameObject gameObject29 in array70)
                    {
                        gameObject29.SetActive(cachedGiftManager.IsUnlocked && !shopItemCharacter.Bought && shopItemCharacter.IsWithoutRoulette);
                    }
                }
            }
        }
        else
        {
            GameObject[] array71 = buyCharacterByLockedWithTimerLayouter;
            foreach (GameObject gameObject30 in array71)
            {
                gameObject30.SetActive(false);
            }
        }
    }

    public void CheckBoughtCharacters()
    {
        for (int i = 0; i < shopCharacters.Count; i++)
        {
            CharacterNameType characterNameType = ((shopCharacters[i].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[i].Infos[0].characterNameType : shopCharacters[i].CharacterUpgradeInfo.characterNameType);
            GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
            if (gUICharacter != null)
            {
                gUICharacter.CheckBought();
            }
        }
    }

    public void UpdateScrollViewPosition()
    {
        ShouldBuyCharacterOnCoinsTutorialLoad = true;
        ShouldBuyCharacterOnGemsTutorialLoad = true;
        selectedIndex = -1;
        int a = -1;
        CharacterNameType characterNameType = base.ObjectsSpawner.FirstSelectedCharacterNameType;
        if (base.CachedManagers.CachedGameManager.CurrentGameType == GameType.Multiplayer && !base.CachedManagers.CachedMultiplayerManager.IsMainPlayer)
        {
            characterNameType = base.ObjectsSpawner.SecondSelectedCharacterNameType;
        }
        if (characterNameType == CharacterNameType.None)
        {
            CharacterNameType firstSelectedCharacter = ((shopCharacters[0].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[0].Infos[0].characterNameType : shopCharacters[0].CharacterUpgradeInfo.characterNameType);
            base.ObjectsSpawner.SetFirstSelectedCharacter(firstSelectedCharacter);
        }
        if (base.CachedManagers.CachedGiftManager.LastOpenedSpecialGiftCharacter != CharacterNameType.None)
        {
            GUIPopup activePopup = base.CachedManagers.CachedGUIPopupManager.ActivePopup;
            if (activePopup == null || (activePopup != null && activePopup.TypePopup != PopupType.FreeChest))
            {
                characterNameType = base.CachedManagers.CachedGiftManager.LastOpenedSpecialGiftCharacter;
                base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.SpecialGiftCharacter);
                base.CachedManagers.CachedGiftManager.LastOpenedSpecialGiftCharacter = CharacterNameType.None;
            }
        }
        if (characterNameType > CharacterNameType.None)
        {
            a = shopCharacters.IndexOf(base.ObjectsSpawner.GetShopItemCharacter(characterNameType));
        }
        a = Mathf.Max(a, 0);
        ShopScrollView[] array = scrollViews;
        foreach (ShopScrollView shopScrollView in array)
        {
            shopScrollView.contentContainer.localPosition = new Vector3((0f - cellsInterval) * (float)a, 0f, 0f);
        }
        SelectedIndex = a;
        ScrollView_OnContentPositionChange();
    }

    public void UpdateCharacterCharacteristics()
    {
        CharacterNameType characterNameType = ((shopCharacters[selectedIndex].UpgradeLevel <= 0) ? shopCharacters[selectedIndex].Infos[0].characterNameType : shopCharacters[selectedIndex].CharacterUpgradeInfo.characterNameType);
        charactersWithLoadedSpriteCollections.Remove(characterNameType);
        charactersWithLoadedSpriteCollections.Add(characterNameType);
        if (base.gameObject.activeSelf)
        {
            if (characterAnimationCoroutine != null)
            {
                StopCoroutine(characterAnimationCoroutine);
            }
            if (isCharacterFirstSelection)
            {
                characterAnimationCoroutine = CoroutineStartAnimationAvatar(0f);
                isCharacterFirstSelection = false;
            }
            else
            {
                characterAnimationCoroutine = CoroutineStartAnimationAvatar(0.5f);
            }
            StartCoroutine(characterAnimationCoroutine);
        }
        tk2dTextMesh[] array = characterNameLabels;
        foreach (tk2dTextMesh tk2dTextMesh2 in array)
        {
            tk2dTextMesh2.text = base.ObjectsSpawner.GetCharacterLocalizedName(characterNameType);
        }
        tk2dTextMesh[] array2 = characterDescriptionLabels;
        foreach (tk2dTextMesh tk2dTextMesh3 in array2)
        {
            tk2dTextMesh3.text = base.ObjectsSpawner.GetCharacterLocalizedDescription(characterNameType);
        }
        tk2dTextMesh[] array3 = characterWeaponDescriptionLabels;
        foreach (tk2dTextMesh tk2dTextMesh4 in array3)
        {
            tk2dTextMesh4.text = base.ObjectsSpawner.GetCharacterLocalizedWeaponDescription(characterNameType);
        }
        tk2dSprite[] array4 = characterWeaponIconSprites;
        foreach (tk2dSprite tk2dSprite2 in array4)
        {
            tk2dSprite2.SetSprite(base.ObjectsSpawner.GetGUICharacter(characterNameType).WeaponIconName);
        }
        bool isPremium = shopCharacters[selectedIndex].IsPremium;
        tk2dSprite[] array5 = characterNameFlagSprites;
        foreach (tk2dSprite tk2dSprite3 in array5)
        {
            tk2dSprite3.gameObject.SetActive(!isPremium);
        }
        tk2dSprite[] array6 = characterNameFlagSpritesPremium;
        foreach (tk2dSprite tk2dSprite4 in array6)
        {
            tk2dSprite4.gameObject.SetActive(isPremium);
        }
    }

    public void CharacterRouletteOpen()
    {
        List<int> roulettePlayedCharacters = base.CachedManagers.CachedStatisticsManager.RoulettePlayedCharacters;
        roulettePlayedCharacters.Add((int)shopCharacters[SelectedIndex].Infos[0].characterNameType);
        base.CachedManagers.CachedStatisticsManager.RoulettePlayedCharacters = roulettePlayedCharacters;
        UpdateBuyButtons();
    }

    private void DeleteMultiplayerChosenSprites()
    {
        if (firstPlayerChoseSprites.Count > 0)
        {
            foreach (tk2dSprite firstPlayerChoseSprite in firstPlayerChoseSprites)
            {
                if (firstPlayerChoseSprite != null)
                {
                    UnityEngine.Object.Destroy(firstPlayerChoseSprite.gameObject);
                }
            }
            secondPlayerMustChoose = false;
        }
        ScrollView_OnContentPositionChange();
    }

    private void SetFirstCharacter()
    {
        ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
        if ((bool)shopItemCharacter)
        {
            CharacterNameType firstSelectedCharacter = ((shopItemCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? shopItemCharacter.Infos[0].characterNameType : shopItemCharacter.CharacterUpgradeInfo.characterNameType);
            base.ObjectsSpawner.SetFirstSelectedCharacter(firstSelectedCharacter);
        }
    }

    private void SetSecondCharacter()
    {
        ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
        CharacterNameType characterNameType = CharacterNameType.None;
        if ((bool)shopItemCharacter)
        {
            characterNameType = ((shopItemCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? shopItemCharacter.Infos[0].characterNameType : shopItemCharacter.CharacterUpgradeInfo.characterNameType);
        }
        StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
        CharacterNameType firstSelectedCharacterNameType = base.ObjectsSpawner.FirstSelectedCharacterNameType;
        if (base.CachedManagers.CachedGameManager.CurrentGameType == GameType.VSComputer)
        {
            do
            {
                if (cachedStatisticsManager.WinsCount < base.CachedManagers.CachedTutorialManager.WinsToDisableEasyMatches)
                {
                    shopItemCharacter = base.CachedManagers.CachedShopManager.EasyCharacters.RandomObject();
                    characterNameType = shopItemCharacter.Infos[0].characterNameType;
                    continue;
                }
                if (UnityEngine.Random.value < base.ObjectsSpawner.SpecialEnemyChance)
                {
                    List<CharacterNameType> specialEnemyCharacters = base.ObjectsSpawner.SpecialEnemyCharacters;
                    characterNameType = specialEnemyCharacters[UnityEngine.Random.Range(0, specialEnemyCharacters.Count)];
                    continue;
                }
                List<CharacterNameType> defeatedCharacters = cachedStatisticsManager.DefeatedCharacters;
                List<CharacterNameType> charactersNameTypes = base.ObjectsSpawner.CharactersNameTypes;
                if (defeatedCharacters.Count != charactersNameTypes.Count)
                {
                    List<CharacterNameType> list = new List<CharacterNameType>();
                    for (int i = 0; i < charactersNameTypes.Count; i++)
                    {
                        if (!defeatedCharacters.Contains(charactersNameTypes[i]))
                        {
                            list.Add(charactersNameTypes[i]);
                        }
                    }
                    if ((list.Count == 1 && list[0] == firstSelectedCharacterNameType) || list.Count == 0)
                    {
                        characterNameType = charactersNameTypes[UnityEngine.Random.Range(0, charactersNameTypes.Count)];
                        continue;
                    }
                    do
                    {
                        characterNameType = list[UnityEngine.Random.Range(0, list.Count)];
                    }
                    while (characterNameType == firstSelectedCharacterNameType);
                }
                else
                {
                    characterNameType = charactersNameTypes[UnityEngine.Random.Range(0, charactersNameTypes.Count)];
                }
            }
            while (characterNameType == firstSelectedCharacterNameType);
        }
        else if (base.CachedManagers.CachedGameManager.CurrentGameType == GameType.Tournament)
        {
            TournamentManager cachedTournamentManager = base.CachedManagers.CachedTournamentManager;
            cachedTournamentManager.GenerateTournament(currentTournamentDifficult, firstSelectedCharacterNameType);
            characterNameType = cachedTournamentManager.EnemyCharacter;
            string text = null;
            if (currentTournamentDifficult == TournamentDifficultType.Easy)
            {
                text = "Level_Tournament_Started_Easy";
            }
            else if (currentTournamentDifficult == TournamentDifficultType.Medium)
            {
                text = "Level_Tournament_Started_Medium";
            }
            else if (currentTournamentDifficult == TournamentDifficultType.Hard)
            {
                text = "Level_Tournament_Started_Hard";
            }
            if (!string.IsNullOrEmpty(text))
            {
                Analytics.CustomEvent(text, new Dictionary<string, object>
                {
                    { "MatchesCount", cachedStatisticsManager.MathesCount },
                    {
                        "GameTimeMinutes",
                        cachedStatisticsManager.TotalGameTime.Minutes
                    }
                });
            }
        }
        if (characterNameType != CharacterNameType.None)
        {
            base.ObjectsSpawner.SetSecondSelectedCharacter(characterNameType);
        }
    }

    public void InitializeCharacters(bool isForced = false)
    {
        if (isForced)
        {
            ShopScrollView[] array = scrollViews;
            foreach (ShopScrollView shopScrollView in array)
            {
                MeshRenderer[] componentsInChildren = shopScrollView.contentContainer.GetComponentsInChildren<MeshRenderer>();
                for (int j = 0; j < componentsInChildren.Length; j++)
                {
                    UnityEngine.Object.Destroy(componentsInChildren[j].gameObject);
                }
            }
        }
        if (!isCharactersInitialized || isForced)
        {
            if (isIPad)
            {
                landscapeScrollView.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
            shopCharacters = base.CachedManagers.CachedShopManager.AvailableCharacters;
            if (shopCharacters != null)
            {
                for (int k = 0; k < shopCharacters.Count; k++)
                {
                    ShopItemCharacter shopItemCharacter = shopCharacters[k];
                    if (!(shopItemCharacter != null))
                    {
                        continue;
                    }
                    CharacterNameType characterNameType = ((shopItemCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? shopItemCharacter.Infos[0].characterNameType : shopItemCharacter.CharacterUpgradeInfo.characterNameType);
                    GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
                    if ((bool)gUICharacter)
                    {
                        gUICharacter.ClearAll();
                        ShopScrollView[] array2 = scrollViews;
                        foreach (ShopScrollView shopScrollView2 in array2)
                        {
                            gUICharacter.AddedIconShop(shopScrollView2, new Vector3(cellsInterval * (float)k, 0f, 0f), shopScrollView2 == portraitScrollView);
                        }
                        gUICharacter.CheckBought();
                    }
                }
                ShopScrollView[] array3 = scrollViews;
                foreach (ShopScrollView shopScrollView3 in array3)
                {
                    shopScrollView3.SnapTransform.localPosition = shopScrollView3.contentContainer.localPosition;
                    shopScrollView3.CellsOffset = cellsInterval;
                    shopScrollView3.contentLength = (float)(shopCharacters.Count - 1) * cellsInterval;
                }
                for (int n = 0; n < mainLocations.Count; n++)
                {
                    if (mainLocations[n] != null)
                    {
                        UnityEngine.Object.Destroy(mainLocations[n]);
                        mainLocations[n] = null;
                    }
                }
                mainLocations.Clear();
                GUILayoutCell[] array4 = avatarCells;
                foreach (GUILayoutCell gUILayoutCell in array4)
                {
                    if (avatarBackground != null)
                    {
                        GameObject gameObject = UnityEngine.Object.Instantiate(avatarBackground);
                        Vector3 localScale = gameObject.transform.localScale;
                        gameObject.transform.parent = gUILayoutCell.transform;
                        gameObject.transform.localPosition = avatarBackground.transform.localPosition;
                        gameObject.transform.localScale = localScale;
                        mainLocations.Add(gameObject);
                    }
                }
                IsCharactersInitialized = true;
            }
            if (base.ObjectsSpawner != null)
            {
                CharacterNameType firstSelectedCharacterNameType = base.ObjectsSpawner.FirstSelectedCharacterNameType;
                ShopItemCharacter shopItemCharacter2 = base.ObjectsSpawner.GetShopItemCharacter(firstSelectedCharacterNameType);
                if (!shopItemCharacter2.Bought)
                {
                    firstSelectedCharacterNameType = base.ObjectsSpawner.BeforeTrySelectedCharacterNameType;
                    shopItemCharacter2 = base.ObjectsSpawner.GetShopItemCharacter(firstSelectedCharacterNameType);
                    base.ObjectsSpawner.SetFirstSelectedCharacter(firstSelectedCharacterNameType);
                }
            }
        }
        else
        {
            CheckBoughtCharacters();
        }
        if (!isForced)
        {
            UpdateScrollViewPosition();
            return;
        }
        CharacterNameType characterNameType2 = ((shopCharacters[selectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[selectedIndex].Infos[0].characterNameType : shopCharacters[selectedIndex].CharacterUpgradeInfo.characterNameType);
        GUICharacter gUICharacter2 = base.ObjectsSpawner.GetGUICharacter(characterNameType2);
        base.ObjectsSpawner.SetFirstSelectedCharacter(characterNameType2);
        gUICharacter2.AddedAvatar(true, characterAvatarAnimation);
        gUICharacter2.BeginRandomAnimations();
        ScrollView_OnContentPositionChange();
        UpdateBuyButtons();
        UpdateScrollViewPosition();
    }

    private void PlayItemPurchaseFailedSound()
    {
        AudioManager.PlayAudioWithKey("no_money");
    }

    private void PlayItemPurchasedSound(bool isPremium)
    {
        AudioManager.PlayAudioWithKey((!isPremium) ? "buy_character_common" : "buy_character_rare");
        base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
        {
            if (this != null)
            {
                AudioManager.PlayAudioWithKey("buy_character");
            }
        }, (!isPremium) ? 0.5f : 0.9f);
    }

    private void PlayItemPurchasedEffect(ShopItemCharacter character)
    {
        GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(character.Infos[0].characterNameType);
        if (character.IsPremium)
        {
            gUICharacter.PlayItemPurchasedEffect(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIMenuBuyCharacterRare), base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIMenuBuyCharacterRareAnimation));
        }
        else
        {
            gUICharacter.PlayItemPurchasedEffect(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIMenuBuyCharacterCommon), base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIMenuBuyCharacterCommonAnimation));
        }
    }

    private void SetCatAnimationTrack(SkeletonAnimation animation)
    {
        float num = 0f;
        animation.State.ClearTracks();
        for (int i = 0; i < catAnimationsInTrackRepeatTimes.Length; i++)
        {
            for (int j = 0; j < catAnimationsInTrackRepeatTimes[i].repeatTimes; j++)
            {
                num += animation.State.AddAnimation(0, catAnimationsInTrackRepeatTimes[i].animationName, false, 0f).EndTime;
            }
        }
        base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
        {
            if (animation == subscribeCatAnimationLandscape)
            {
                moneyEffectSpawnerLandscape.gameObject.SetActive(false);
            }
            else if (animation == subscribeCatAnimationPortrait)
            {
                moneyEffectSpawnerPortrait.gameObject.SetActive(false);
            }
            SetCatAnimationTrack(animation);
        }, num);
    }

    private void SetMoneyEffectActive(EffectSpawner effectSpawner, bool isActive)
    {
        effectSpawner.gameObject.SetActive(isActive);
    }

    private void MenuScreen_OnShouldBuyCharacterOnCoinsTutorialLoad(bool obj)
    {
        ShouldBuyCharacterOnCoinsTutorialLoad = obj;
    }

    private void MenuScreen_OnShouldBuyCharacterOnGemsTutorialLoad(bool isNeeded)
    {
        ShouldBuyCharacterOnGemsTutorialLoad = isNeeded;
    }

    private void MenuScreen_OnChangeCurrentPhase(MenuPhase obj)
    {
        currentPhase = obj;
    }

    private void MenuScreen_OnContentPositionChange()
    {
        ScrollView_OnContentPositionChange();
    }

    private void MenuScreen_OnDeleteMultiplayerChosenSprites()
    {
        DeleteMultiplayerChosenSprites();
    }

    private void MenuScreen_OnCharacterChoosenCallback(Action obj)
    {
        characterChoosenCallback = obj;
    }

    private void MenuScreen_OnMultiplayerInvitationReceived(bool obj)
    {
        multiplayerInvitationReceived = obj;
    }

    private void MenuScreen_OnCurrentTournamentDifficult(TournamentDifficultType obj)
    {
        currentTournamentDifficult = obj;
    }

    private void MenuScreen_OnUpdateBuyButtons()
    {
        UpdateBuyButtons();
    }

    private void MenuScreen_OnCharacterFirstSelection(bool obj)
    {
        isCharacterFirstSelection = obj;
    }

    private void MenuScreen_OnCheckBoughtCharacters()
    {
        CheckBoughtCharacters();
    }

    private void MenuScreen_OnInitializeCharacters()
    {
        InitializeCharacters();
    }

    private void MultiplayerManager_OnMatchCreated()
    {
        CharacterNameType characterID = base.CachedManagers.CachedMultiplayerManager.LeftPlayerSettings.characterID;
        CharacterNameType characterID2 = base.CachedManagers.CachedMultiplayerManager.RightPlayerSettings.characterID;
        GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterID);
        if (gUICharacter != null)
        {
            gUICharacter.SetMultipleCharacter(characterID);
            base.ObjectsSpawner.SetFirstSelectedCharacter(characterID);
        }
        gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterID2);
        if (gUICharacter != null)
        {
            gUICharacter.SetMultipleCharacter(characterID2);
            base.ObjectsSpawner.SetSecondSelectedCharacter(characterID2);
        }
        base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Versus);
        StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
        string value = ((!base.CachedManagers.CachedMultiplayerManager.IsMainPlayer) ? base.ObjectsSpawner.GetShopItemCharacter(characterID2).GetAnalyticsKey() : base.ObjectsSpawner.GetShopItemCharacter(characterID).GetAnalyticsKey());
        Analytics.CustomEvent("Character_Multiplayer_Participates", new Dictionary<string, object>
        {
            { "CharacterName", value },
            {
                "GameTimeMinutes",
                cachedStatisticsManager.TotalGameTime.Minutes
            }
        });
        Analytics.CustomEvent("Level_Multiplayer_Started", new Dictionary<string, object>
        {
            { "MatchesCount", cachedStatisticsManager.MathesCount },
            {
                "GameTimeMinutes",
                cachedStatisticsManager.TotalGameTime.Minutes
            }
        });
    }

    private void MenuScreen_OnPlayButtonEnable(bool obj)
    {
        tk2dUIItem[] array = playButtons;
        foreach (tk2dUIItem tk2dUIItem2 in array)
        {
            tk2dUIItem2.enabled = obj;
        }
    }

    private void Character_ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
    {
        shopItem.OnItemPurchased -= Character_ShopItem_OnItemPurchased;
        shopItem.OnItemPurchaseFailed -= Character_ShopItem_OnItemPurchaseFailed;
        PlayItemPurchaseFailedSound();
        base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
    }

    private void Character_ShopItem_OnItemPurchased(ShopItem shopItem)
    {
        for (int i = 0; i < shopCharacters.Count; i++)
        {
            if (shopCharacters[i] == shopItem)
            {
                ShopScrollView[] array = scrollViews;
                foreach (ShopScrollView shopScrollView in array)
                {
                    shopScrollView.contentContainer.localPosition = new Vector3((0f - cellsInterval) * (float)i, 0f, 0f);
                }
                base.ObjectsSpawner.GetGUICharacter(shopCharacters[i].Infos[0].characterNameType).CheckBought();
                break;
            }
        }
        ((ShopItemCharacter)shopItem).OpenCharacter(true, true);
        PlayItemPurchasedSound(((ShopItemCharacter)shopItem).IsPremium);
        PlayItemPurchasedEffect((ShopItemCharacter)shopItem);
        ScrollView_OnContentPositionChange();
        shopItem.OnItemPurchased -= Character_ShopItem_OnItemPurchased;
        shopItem.OnItemPurchaseFailed -= Character_ShopItem_OnItemPurchaseFailed;
        UpdateBuyButtons();
        base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
        if ((ShopItemCharacter)shopItem == base.CachedManagers.CachedProposalManager.TryCharacter)
        {
            base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
        }
    }

    private void BuyCharacterOnCoins_OnClick()
    {
        ShopScrollView[] array = scrollViews;
        foreach (ShopScrollView shopScrollView in array)
        {
            shopScrollView.Stop();
        }
        ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
        if (!shopItemCharacter.IsLockedByTimer || base.CachedManagers.CachedGiftManager.IsLockedByTimerCharacterAvaliable)
        {
            if (shopItemCharacter.Availible)
            {
                return;
            }
            if (base.CachedManagers.CachedStatisticsManager.Coins >= shopItemCharacter.PriceCoins)
            {
                base.CachedManagers.CachedStatisticsManager.DecreaseCoins(shopItemCharacter.PriceCoins);
                shopItemCharacter.OpenCharacter(true, true);
                base.ObjectsSpawner.GetGUICharacter(shopItemCharacter.Infos[0].characterNameType).CheckBought();
                GameGoliath.Instance.TrackCharacterAction(shopItemCharacter.GetCharacterID(), GoliathIds.Action.ACQUISITION, GoliathIds.Origin.SOFT_PURCHASE, string.Empty);
                UpdateBuyButtons();
                PlayItemPurchasedEffect(shopItemCharacter);
                base.CachedManagers.CachedTutorialManager.WasCharacterBoughtOnCoins = true;
                PlayItemPurchasedSound(shopItemCharacter.IsPremium);
                ScrollView_OnContentPositionChange();
                if (base.CachedManagers.CachedProposalManager.TryCharacter == shopItemCharacter)
                {
                    base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
                }
            }
            else
            {
                (base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true) as MiniBankPopup).InitPopup(shopItemCharacter.PriceCoins, true);
            }
        }
        else
        {
            base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
        }
    }

    private void BuyCharacterOnBucks_OnClick()
    {
        ShopScrollView[] array = scrollViews;
        foreach (ShopScrollView shopScrollView in array)
        {
            shopScrollView.Stop();
        }
        ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
        if (!shopItemCharacter.IsLockedByTimer || base.CachedManagers.CachedGiftManager.IsLockedByTimerCharacterAvaliable)
        {
            if (!shopItemCharacter.Availible)
            {
                shopItemCharacter.OnItemPurchased += Character_ShopItem_OnItemPurchased;
                shopItemCharacter.OnItemPurchaseFailed += Character_ShopItem_OnItemPurchaseFailed;
                base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
                if (!shopItemCharacter.TryBuy())
                {
                    shopItemCharacter.OnItemPurchased -= Character_ShopItem_OnItemPurchased;
                    shopItemCharacter.OnItemPurchaseFailed -= Character_ShopItem_OnItemPurchaseFailed;
                    base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
                }
            }
        }
        else
        {
            base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
        }
    }

    private void BuyCharacterByInvite_OnClick()
    {
        ShopScrollView[] array = scrollViews;
        foreach (ShopScrollView shopScrollView in array)
        {
            shopScrollView.Stop();
        }
        ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
        if (!shopItemCharacter.IsLockedByTimer || base.CachedManagers.CachedGiftManager.IsLockedByTimerCharacterAvaliable)
        {
            base.CachedManagers.CachedInviteManager.SetInviteCharacter(shopCharacters[SelectedIndex]);
            base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Invite, true, null, true);
        }
        else
        {
            base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
        }
    }

    private void BuyCharacterBySubscribe_OnClick()
    {
        if (!isCountryIdentifyAvailable)
        {
            return;
        }
        isCountryIdentifyAvailable = false;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
            isCountryIdentifyAvailable = true;
            return;
        }
        StartCoroutine(CountryIdentify.CheckCountry(delegate (bool result)
        {
            ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
            if (result || shopItemCharacter.IsOnlySubscribeReward)
            {
                ShopScrollView[] array = scrollViews;
                foreach (ShopScrollView shopScrollView in array)
                {
                    shopScrollView.Stop();
                }
                if (!shopItemCharacter.IsLockedByTimer || base.CachedManagers.CachedGiftManager.IsLockedByTimerCharacterAvaliable)
                {
                    base.CachedManagers.CachedSubscribeManager.SetSubscribeCharacter(shopCharacters[SelectedIndex]);
                    base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Subscribe, true, null, true);
                }
                else
                {
                    base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
                }
            }
            else
            {
                BuyCharacterByInvite_OnClick();
            }
            isCountryIdentifyAvailable = true;
        }));
    }

    private void BuyCharacterByAds_OnClick()
    {
        ShopScrollView[] array = scrollViews;
        foreach (ShopScrollView shopScrollView in array)
        {
            shopScrollView.Stop();
        }
        ShopItemCharacter shopItemCharacter = shopCharacters[SelectedIndex];
        if (!shopItemCharacter.IsLockedByTimer || base.CachedManagers.CachedGiftManager.IsLockedByTimerCharacterAvaliable)
        {
            (base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.CharacterForAds, true, null, true) as CharacterForAdsPopup).Character = shopItemCharacter;
        }
        else
        {
            base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
        }
    }

    private void ScrollView_OnContentPositionChange()
    {
        int num = -1;
        float num2 = 100000f;
        ShopScrollView shopScrollView = null;
        ShopScrollView[] array = scrollViews;
        foreach (ShopScrollView shopScrollView2 in array)
        {
            if (shopScrollView2.isActiveAndEnabled)
            {
                shopScrollView = shopScrollView2;
                break;
            }
        }
        if (!shopScrollView)
        {
            return;
        }
        Vector3 localPosition = shopScrollView.contentContainer.localPosition;
        ShopScrollView[] array2 = scrollViews;
        foreach (ShopScrollView shopScrollView3 in array2)
        {
            shopScrollView3.contentContainer.localPosition = localPosition;
        }
        for (int k = 0; k < shopCharacters.Count; k++)
        {
            CharacterNameType characterNameType = ((shopCharacters[k].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[k].Infos[0].characterNameType : shopCharacters[k].CharacterUpgradeInfo.characterNameType);
            GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
            float num3 = cellsInterval * (float)k + localPosition.x;
            float num4 = Mathf.Abs(num3);
            if (num4 < num2)
            {
                num2 = num4;
                num = k;
            }
            Vector3 vector = new Vector3(cellsInterval * (float)k, 0f, 0f);
            Vector3 one = Vector3.one;
            float num5 = 0f;
            if (num3 < 0f)
            {
                if (num4 <= cellsInterval)
                {
                    num5 = scrollViewOffset.Evaluate(num4 / (2f * cellsInterval));
                    vector = new Vector3(cellsInterval * (float)k, 0f, 0f) - new Vector3(num5 * cellsOffset, 0f);
                    float num6 = Mathf.Clamp(inactiveScale + (1f - inactiveScale) * (1f - num5), inactiveScale, 1f);
                    one = new Vector3(num6, num6, num6);
                }
                else
                {
                    vector = new Vector3(cellsInterval * (float)k - cellsOffset, 0f, 0f);
                    one = new Vector3(inactiveScale, inactiveScale, inactiveScale);
                }
            }
            else if (num3 > 0f)
            {
                if (num4 <= cellsInterval)
                {
                    num5 = scrollViewOffset.Evaluate(1f - num4 / (2f * cellsInterval));
                    vector = new Vector3(cellsInterval * (float)k, 0f, 0f) + new Vector3(num5 * cellsOffset, 0f);
                    float num7 = Mathf.Clamp(inactiveScale + (1f - inactiveScale) * (1f - num5), inactiveScale, 1f);
                    one = new Vector3(num7, num7, num7);
                }
                else
                {
                    vector = new Vector3(cellsInterval * (float)k + cellsOffset, 0f, 0f);
                    one = new Vector3(inactiveScale, inactiveScale, inactiveScale);
                }
            }
            else
            {
                vector = new Vector3(cellsInterval * (float)k, 0f, 0f);
                one = Vector3.one;
            }
            Color newColor = Color.Lerp(Color.white, Color.clear, (num4 - cellsFadeOffset) / cellsFadeInterval);
            gUICharacter.SetParameters(vector, one, newColor, secondPlayerMustChoose);
        }
        SelectedIndex = num;
    }

    private void MenuScreen_OnStartSpecialIdleAnimations(bool obj1, bool obj2)
    {
        CharacterNameType characterNameType = ((shopCharacters[SelectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[SelectedIndex].Infos[0].characterNameType : shopCharacters[SelectedIndex].CharacterUpgradeInfo.characterNameType);
        GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
        if (gUICharacter.CharacterMode == CharacterMode.Random)
        {
            gUICharacter.StartSpecialIdleAnimations(obj1, obj2);
        }
    }

    private void MenuScreen_OnSecondPlayerMustChoose(bool obj)
    {
        secondPlayerMustChoose = obj;
    }

    private void PlayButton_OnClick()
    {
        StartCoroutine(CoroutineChooseCharacter());

        Debug.Log("开战--------------");
    }

    private void UpgradeButton_OnClick()
    {
        base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.UpgradeLevel, true, null, true);
        // ADManager.Instance.ShowAD_Black();
    }

    private void SubscribeButton_OnClick()
    {
        base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.AutoRenewableSubscribe);
    }

    private void ShopItemSubscription_OnSubscriptionActive()
    {
        tk2dUIItem[] array = subscribeButtons;
        foreach (tk2dUIItem tk2dUIItem2 in array)
        {
            tk2dUIItem2.gameObject.SetActive(false);
        }
    }

    private void SubscribeCatAnimationPortrait_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        if (e.Data == CachedEventStartMoneyPortrait && !moneyEffectSpawnerPortrait.gameObject.activeInHierarchy)
        {
            SetMoneyEffectActive(moneyEffectSpawnerPortrait, true);
        }
    }

    private void SubscribeCatAnimationLandscape_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        if (e.Data == CachedEventStartMoneyLandscape && !moneyEffectSpawnerLandscape.gameObject.activeInHierarchy)
        {
            SetMoneyEffectActive(moneyEffectSpawnerLandscape, true);
        }
    }

    private void RouletteButton_OnClick()
    {
        (base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.FreeChest) as FreeChestPopup).Init(true);
    }

    private IEnumerator CoroutineStartAnimationAvatar(float coroutineDelay)
    {
        if (playButtons != null && playButtons.Length > 0)
        {
            for (int i = 0; i < playButtons.Length; i++)
            {
                playButtons[i].enabled = false;
            }
        }
        yield return new WaitForSeconds(coroutineDelay);
        if (playButtons != null && playButtons.Length > 0)
        {
            for (int j = 0; j < playButtons.Length; j++)
            {
                playButtons[j].enabled = true;
            }
        }
        CharacterNameType characterNameType = ((shopCharacters[selectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[selectedIndex].Infos[0].characterNameType : shopCharacters[selectedIndex].CharacterUpgradeInfo.characterNameType);
        GUICharacter guiCharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
        CachedMenuScreenPartyMasters.WillChooseCharacter(characterNameType);
        bool isResetAnimationNedded = true;
        if (lastSelectedIndex != -1)
        {
            CharacterNameType characterNameType2 = ((shopCharacters[lastSelectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[lastSelectedIndex].Infos[0].characterNameType : shopCharacters[lastSelectedIndex].CharacterUpgradeInfo.characterNameType);
            GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType2);
            isResetAnimationNedded = gUICharacter.IsAnimationResetNeeded || guiCharacter.IsAnimationResetNeeded;
        }
        guiCharacter.AddedAvatar(isResetAnimationNedded, characterAvatarAnimation);
        CachedMenuScreenPartyMasters.DidChooseCharacter(characterNameType);
        guiCharacter.BeginRandomAnimations();
        while (charactersWithLoadedSpriteCollections.Count > 5)
        {
            base.CachedManagers.CachedResourceManager.UnloadCharacterSpriteCollections(charactersWithLoadedSpriteCollections[0], 0.1f);
            charactersWithLoadedSpriteCollections.RemoveAt(0);
        }
        lastSelectedIndex = selectedIndex;
    }

    private IEnumerator CoroutineChooseCharacter()
    {
        base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
        switch (currentPhase)
        {
            case MenuPhase.Mode:
                base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
                Debug.Log("开始一");
                break;
            case MenuPhase.First:
                {

                    CharacterNameType characterNameType = ((shopCharacters[SelectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[SelectedIndex].Infos[0].characterNameType : shopCharacters[SelectedIndex].CharacterUpgradeInfo.characterNameType);
                    GUICharacter firstGUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
                    if (firstGUICharacter.CharacterMode == CharacterMode.Random)
                    {
                        firstGUICharacter.PlaySpecialIdleAnimations();
                    }
                    yield return new WaitForSeconds(firstGUICharacter.CurrentMultipleCharacterInfo.menuIdleDelayMultipleCharacter);
                    SetFirstCharacter();
                    if ((bool)firstGUICharacter)
                    {
                        firstGUICharacter.PlayChooseEffect();
                        bool flag = true;
                        Transform[] shopIconTransform = firstGUICharacter.ShopIconTransform;
                        foreach (Transform parent in shopIconTransform)
                        {
                            tk2dSprite tk2dSprite2 = UnityEngine.Object.Instantiate(chosePlayerOneSprite);
                            tk2dSprite2.transform.parent = parent;
                            tk2dSprite2.SortingOrder = ((!flag) ? (-12) : (-1));
                            flag = !flag;
                            tk2dSprite2.transform.localPosition = Vector3.forward;
                            tk2dSprite2.transform.localScale = Vector3.one;
                            firstPlayerChoseSprites.Add(tk2dSprite2);
                        }
                        for (int j = 0; j < shopCharacters.Count; j++)
                        {
                            characterNameType = ((shopCharacters[j].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[j].Infos[0].characterNameType : shopCharacters[j].CharacterUpgradeInfo.characterNameType);
                            GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
                            gUICharacter.IsChoicedFirstPlayer = false;
                        }
                        firstGUICharacter.IsChoicedFirstPlayer = true;
                        secondPlayerMustChoose = true;
                    }
                    AudioManager.PlayAudioWithKey("choose_character");
                    AudioManager.PlayAudioWithKey("choose_character_additional");
                    base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
                    if (base.CachedManagers.CachedGameManager.CurrentGameType == GameType.VSHuman)
                    {
                        Debug.Log("开始22222222222221");
                        ADManager.Instance.ShowBlackAd();
                        if (firstGUICharacter.CharacterMode == CharacterMode.Random)
                        {
                            firstGUICharacter.StartSpecialIdleAnimations(true, true);
                        }
                        MenuScreenScrollView.m_OnSetPhase?.Invoke(MenuPhase.Second);
                    }
                    else if (base.CachedManagers.CachedGameManager.CurrentGameType == GameType.Multiplayer)
                    {
                        Debug.Log("开始1111111111111");
                        tk2dUIItem[] array = playButtons;
                        foreach (tk2dUIItem tk2dUIItem2 in array)
                        {
                            tk2dUIItem2.enabled = false;
                        }
                        characterNameType = ((shopCharacters[SelectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[SelectedIndex].Infos[0].characterNameType : shopCharacters[SelectedIndex].CharacterUpgradeInfo.characterNameType);
                        if (base.ObjectsSpawner.GetGUICharacter(characterNameType).CurrentMultipleCharacterInfo.multipleCharacterType != CharacterNameType.None)
                        {
                            characterNameType = base.ObjectsSpawner.GetGUICharacter(characterNameType).CurrentMultipleCharacterInfo.multipleCharacterType;
                        }
                        if (multiplayerInvitationReceived)
                        {
                            base.CachedManagers.CachedMultiplayerManager.SetLocalPlayerSettings(characterNameType);
                            multiplayerInvitationReceived = false;
                            if (characterChoosenCallback != null)
                            {
                                characterChoosenCallback();
                            }
                        }
                        else
                        {
                            if (!base.CachedManagers.CachedMultiplayerManager.CreateMatch(characterNameType))
                            {
                                tk2dUIItem[] array2 = playButtons;
                                foreach (tk2dUIItem tk2dUIItem3 in array2)
                                {
                                    tk2dUIItem3.enabled = true;
                                }
                            }
                            MenuScreenScrollView.m_OnDeleteMultiplayerChosenSprites?.Invoke();
                        }
                    }
                    else
                    {
                        ADManager.Instance.ShowBlackAd();
                        Debug.Log("开始11111119991");
                        SetSecondCharacter();
                        base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Versus);
                    }
                    yield return new WaitForSeconds(chooseDelay);
                    break;
                }
            case MenuPhase.Second:
                {
                    // Debug.Log("双人选完角色  真正开始游戏");
                    ADManager.Instance.ShowBlackAd();

                    CharacterNameType characterNameType = ((shopCharacters[SelectedIndex].CharacterUpgradeInfo.upgradeLevel <= 0) ? shopCharacters[SelectedIndex].Infos[0].characterNameType : shopCharacters[SelectedIndex].CharacterUpgradeInfo.characterNameType);
                    GUICharacter secondGUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
                    if ((bool)secondGUICharacter)
                    {
                        secondGUICharacter.PlayChooseEffect();
                    }
                    AudioManager.PlayAudioWithKey("choose_character");
                    AudioManager.PlayAudioWithKey("choose_character_additional");
                    if (secondGUICharacter.CharacterMode == CharacterMode.Random)
                    {
                        secondGUICharacter.PlaySpecialIdleAnimations();
                    }
                    SetSecondCharacter();
                    yield return new WaitForSeconds(secondGUICharacter.CurrentMultipleCharacterInfo.menuIdleDelayMultipleCharacter);
                    yield return new WaitForSeconds(chooseDelay);
                    base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
                    base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Versus);
                    break;
                }
        }
    }
}
