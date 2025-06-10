using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Mit_Goliath_Native.Utils;
using Prime31;
using Spine;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MenuScreen : GUIScreen
{
	private const string NORMAL_MODE_BUTTON = "btn_mode";

	private const string SELECTED_MODE_BUTTON = "btn_mode_selected";

	private const string CHOOSE_PLAYER_1 = "ui.main.choose_character1";

	private const string CHOOSE_PLAYER_2 = "ui.main.choose_character2";

	private const string TOURNAMENT_MODE = "ui.main.mode_tournament";

	private const string TOURNAMENT_UNLOCK_DESCRIPTION = "ui.main.tournament_mode.unlock.description";

	private const string TOURNAMENT_EASY_LABEL = "ui.main.tournament_mode.easy.description";

	private const string TOURNAMENT_MEDIUM_LOCKED_LABEL = "ui.main.tournament_mode.normal.description.locked";

	private const string TOURNAMENT_MEDIUM_UNLOCKED_LABEL = "ui.main.tournament_mode.normal.description.unlocked";

	private const string TOURNAMENT_HARD_LOCKED_LABEL = "ui.main.tournament_mode.legendary.description.locked";

	private const string TOURNAMENT_HARD_UNLOCKED_LABEL = "ui.main.tournament_mode.legendary.description.unlocked.android";

	private const string CHARACTERS_UNLOCKED_KEY = "ui.main.characters_unlock";

	private const string ANIMATION_CHOOSE_IDLE = "idle";

	private const string ANIMATION_CHOOSE_PICK = "pick";

	private const float ALL_OPPONENTS_FOUND_DELAY = 5f;

	private const int FREE_GEMS_RECEIVED_BEFORE_SUBSCRIBE = 1;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnInitializeCharacters;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnUpdateBuyButtons;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnCheckBoughtCharacters;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnContentPositionChange;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnCharacterFirstSelection;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSecondPlayerMustChoose;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool, bool> m_OnStartSpecialIdleAnimations;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnPlayButtonEnable;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<TournamentDifficultType> m_OnCurrentTournamentDifficult;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnMultiplayerInvitationReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<Action> m_OnCharacterChoosenCallback;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnDeleteMultiplayerChosenSprites;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MenuPhase> m_OnChangeCurrentPhase;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnShouldBuyCharacterOnCoinsTutorialLoad;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnShouldBuyCharacterOnGemsTutorialLoad;

	[SerializeField]
	private MenuScreenScrollView menuScreenScrollView;

	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private tk2dTextMesh[] gemsLabels;

	[SerializeField]
	private tk2dUIItem[] bankButtons;

	[SerializeField]
	private tk2dUIItem[] computerButtons;

	[SerializeField]
	private tk2dUIItem[] humanButtons;

	[SerializeField]
	private tk2dUIItem[] birdsHuntButtons;

	[SerializeField]
	private tk2dUIItem[] appleShootButtons;

	[SerializeField]
	private tk2dUIItem[] tournamentButtons;

	[SerializeField]
	private tk2dUIItem[] multiplayerButtons;

	[SerializeField]
	private tk2dUIItem[] vsHumanLockedButton;

	[SerializeField]
	private TweenPosition[] vsHumanLockedTweener;

	[SerializeField]
	private tk2dUIItem[] birdsHuntLockedButton;

	[SerializeField]
	private TweenPosition[] birdsHuntLockedTweener;

	[SerializeField]
	private tk2dUIItem[] appleShootLockedButton;

	[SerializeField]
	private TweenPosition[] appleShootLockedTweener;

	[SerializeField]
	private tk2dUIItem[] tournamentLockedButton;

	[SerializeField]
	private TweenPosition[] tournamentLockedTweener;

	[SerializeField]
	private tk2dUIItem[] multiplayerLockedButton;

	[SerializeField]
	private TweenPosition[] multiplayerLockedTweener;

	[SerializeField]
	private tk2dTextMesh[] vsHumanLockedCountLabel;

	[SerializeField]
	private tk2dTextMesh[] birdsHuntLockedCountLabel;

	[SerializeField]
	private tk2dTextMesh[] appleShootLockedCountLabel;

	[SerializeField]
	private tk2dTextMesh[] tournamentLockedCountLabel;

	[SerializeField]
	private tk2dTextMesh[] multiplayerLockedCountLabel;

	[SerializeField]
	private tk2dUIItem[] backButtons;

	[SerializeField]
	private GameObject[] modeSelectionInfoRoots;

	[SerializeField]
	private GameObject[] modeSelectionIconRoots;

	[SerializeField]
	private GameObject[] characterSelectionRoots;

	[SerializeField]
	private GameObject[] tournamentDifficultSelectionRoots;

	[SerializeField]
	private tk2dBaseSprite[] computerButtonsBack;

	[SerializeField]
	private tk2dBaseSprite[] humanButtonsBack;

	[SerializeField]
	private tk2dBaseSprite[] birdsButtonBack;

	[SerializeField]
	private tk2dBaseSprite[] appleShootButtonsBack;

	[SerializeField]
	private tk2dBaseSprite[] tournamentButtonsBack;

	[SerializeField]
	private tk2dBaseSprite[] multiplayerButtonsBack;

	[SerializeField]
	private tk2dUIItem[] settingsButtons;

	[SerializeField]
	private tk2dUIItem[] achievementButtons;

	private List<NotificationButton> notificationButttonsScript;

	[SerializeField]
	private GUIProposalMenu[] proposalsMenu;

	[SerializeField]
	private GUIProposalMenu[] proposalsMenuSuperGift;

	[SerializeField]
	private tk2dUIItem[] tournamentEasyButton;

	[SerializeField]
	private tk2dTextMesh[] tournamentEasyLabel;

	[SerializeField]
	private tk2dUIItem[] tournamentMediumButton;

	[SerializeField]
	private GameObject[] tournamentMediumLocked;

	[SerializeField]
	private tk2dTextMesh[] tournamentMediumLabel;

	[SerializeField]
	private tk2dUIItem[] tournamentHardButton;

	[SerializeField]
	private GameObject[] tournamentHardLocked;

	[SerializeField]
	private tk2dTextMesh[] tournamentHardLabel;

	[SerializeField]
	private tk2dSprite chosePlayerTwoSprite;

	[SerializeField]
	private tk2dSprite chosePlayersOneAndTwoSprite;

	[SerializeField]
	private GUIControl controlAchievement;

	[SerializeField]
	private tk2dTextMesh[] textsAchievement;

	[SerializeField]
	private tk2dBaseSprite[] iconsAchievement;

	[SerializeField]
	private GUILayoutCell[] countCellsAchievement;

	[SerializeField]
	private tk2dTextMesh[] countLabelsAchiviement;

	[SerializeField]
	private float timeAchievementShowing;

	[SerializeField]
	private float timeAchievementShow;

	[SerializeField]
	private float timeAchievementHidding;

	[SerializeField]
	private float delayAddEffect;

	[SerializeField]
	private GameObject[] firstWinSprite;

	[SerializeField]
	private TweenColor[] fadeTweener;

	[SerializeField]
	private GameObject[] touchBlocker;

	[SerializeField]
	private float timeDelayRouletteScreen;

	[SerializeField]
	private GUIControl controlFade;

	[SerializeField]
	private SkeletonAnimation[] followUsAnimation;

	[SerializeField]
	private tk2dUIItem[] followUsButton;

	[SerializeField]
	private tk2dUIItem[] facebookButton;

	[SerializeField]
	private string facebookLink;

	[SerializeField]
	private string facebookAppLink;

	[SerializeField]
	private tk2dUIItem[] twitterButton;

	[SerializeField]
	private string twitterLink;

	[SerializeField]
	private tk2dUIItem[] instagramButton;

	[SerializeField]
	private string instagramLink;

	[SerializeField]
	private tk2dUIItem[] youtubeButton;

	[SerializeField]
	private string youtubeLink;

	[SerializeField]
	private GameObject[] removeAdsBannerRoot;

	[SerializeField]
	private tk2dUIItem[] removeAdsBannerButton;

	[SerializeField]
	private GameObject[] bankAlertObjects;

	[Header("iPhone X")]
	[SerializeField]
	private GUILayoutCell bottomOffsetCell;

	[SerializeField]
	private float bottomIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell leftProposalCell;

	[SerializeField]
	private float leftProposalIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell rightAnimationOffsetCell;

	[SerializeField]
	private float rightAnimationIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] bottomAnimationOffsetCell;

	[SerializeField]
	private GUILayoutCell scrollOffsetCell;

	[SerializeField]
	private GUILayoutCell tournamentDifficultOffsetCell;

	[SerializeField]
	private GUILayoutCell modeSelectionOffsetCell;

	[SerializeField]
	private GUILayoutCell[] leftButtonsCells;

	[SerializeField]
	private GUILayoutCell[] rightButtonsCells;

	[SerializeField]
	private float rightButtonsIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] portraitTopOffsetCells;

	[SerializeField]
	private float portraitTopOffset;

	[SerializeField]
	private GUILayoutCell[] portraitBottomOffsetCells;

	[SerializeField]
	private float portraitBottomOffset;

	[SerializeField]
	private GUILayoutCell portraitInfoCell;

	[SerializeField]
	private float portraitInfoIPhoneXSize;

	[SerializeField]
	private GUILayoutCell[] leftGirlOffsetCells;

	[SerializeField]
	private float leftGirlIPhoneXOffset;

	private GameType currentSelectedMode = GameType.VSHuman;

	private MenuPhase currentPhase;

	private TournamentDifficultType currentTournamentDifficult = TournamentDifficultType.Easy;

	private AdMobManager cachedAdMobManager;

	private GUITimer timer = new GUITimer();

	private bool isCharactersInitialized;

	private List<GameObject> currentEffects = new List<GameObject>();

	private Spine.Animation cachedAnimationFollowUsShow;

	private Spine.Animation cachedAnimationFollowUsShown;

	private Spine.Animation cachedAnimationFollowUsHide;

	private Spine.Animation cachedAnimationFollowUsHidden;

	private bool isFollowUsShown;

	public MenuScreenScrollView MenuScreenScrollView
	{
		get
		{
			return menuScreenScrollView;
		}
	}

	private Spine.Animation CachedAnimationFollowUsShow
	{
		get
		{
			if (cachedAnimationFollowUsShow == null)
			{
				cachedAnimationFollowUsShow = followUsAnimation[0].Skeleton.Data.FindAnimation("show");
			}
			return cachedAnimationFollowUsShow;
		}
	}

	private Spine.Animation CachedAnimationFollowUsShown
	{
		get
		{
			if (cachedAnimationFollowUsShown == null)
			{
				cachedAnimationFollowUsShown = followUsAnimation[0].Skeleton.Data.FindAnimation("shown");
			}
			return cachedAnimationFollowUsShown;
		}
	}

	private Spine.Animation CachedAnimationFollowUsHide
	{
		get
		{
			if (cachedAnimationFollowUsHide == null)
			{
				cachedAnimationFollowUsHide = followUsAnimation[0].Skeleton.Data.FindAnimation("hide");
			}
			return cachedAnimationFollowUsHide;
		}
	}

	private Spine.Animation CachedAnimationFollowUsHidden
	{
		get
		{
			if (cachedAnimationFollowUsHidden == null)
			{
				cachedAnimationFollowUsHidden = followUsAnimation[0].Skeleton.Data.FindAnimation("hidden");
			}
			return cachedAnimationFollowUsHidden;
		}
	}

	private AdMobManager CachedAdmobManager
	{
		get
		{
			if (cachedAdMobManager == null)
			{
				cachedAdMobManager = SingletonMonoBehaviour<AdMobManager>.Instance;
			}
			return cachedAdMobManager;
		}
	}

	public List<NotificationButton> NotificationButtons
	{
		get
		{
			return notificationButttonsScript;
		}
	}

	public static event Action OnInitializeCharacters
	{
		add
		{
			Action action = MenuScreen.m_OnInitializeCharacters;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnInitializeCharacters, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MenuScreen.m_OnInitializeCharacters;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnInitializeCharacters, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnUpdateBuyButtons
	{
		add
		{
			Action action = MenuScreen.m_OnUpdateBuyButtons;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnUpdateBuyButtons, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MenuScreen.m_OnUpdateBuyButtons;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnUpdateBuyButtons, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnCheckBoughtCharacters
	{
		add
		{
			Action action = MenuScreen.m_OnCheckBoughtCharacters;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCheckBoughtCharacters, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MenuScreen.m_OnCheckBoughtCharacters;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCheckBoughtCharacters, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnContentPositionChange
	{
		add
		{
			Action action = MenuScreen.m_OnContentPositionChange;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnContentPositionChange, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MenuScreen.m_OnContentPositionChange;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnContentPositionChange, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnCharacterFirstSelection
	{
		add
		{
			Action<bool> action = MenuScreen.m_OnCharacterFirstSelection;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCharacterFirstSelection, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MenuScreen.m_OnCharacterFirstSelection;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCharacterFirstSelection, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnSecondPlayerMustChoose
	{
		add
		{
			Action<bool> action = MenuScreen.m_OnSecondPlayerMustChoose;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnSecondPlayerMustChoose, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MenuScreen.m_OnSecondPlayerMustChoose;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnSecondPlayerMustChoose, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool, bool> OnStartSpecialIdleAnimations
	{
		add
		{
			Action<bool, bool> action = MenuScreen.m_OnStartSpecialIdleAnimations;
			Action<bool, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnStartSpecialIdleAnimations, (Action<bool, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool, bool> action = MenuScreen.m_OnStartSpecialIdleAnimations;
			Action<bool, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnStartSpecialIdleAnimations, (Action<bool, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnPlayButtonEnable
	{
		add
		{
			Action<bool> action = MenuScreen.m_OnPlayButtonEnable;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnPlayButtonEnable, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MenuScreen.m_OnPlayButtonEnable;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnPlayButtonEnable, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<TournamentDifficultType> OnCurrentTournamentDifficult
	{
		add
		{
			Action<TournamentDifficultType> action = MenuScreen.m_OnCurrentTournamentDifficult;
			Action<TournamentDifficultType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCurrentTournamentDifficult, (Action<TournamentDifficultType>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<TournamentDifficultType> action = MenuScreen.m_OnCurrentTournamentDifficult;
			Action<TournamentDifficultType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCurrentTournamentDifficult, (Action<TournamentDifficultType>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnMultiplayerInvitationReceived
	{
		add
		{
			Action<bool> action = MenuScreen.m_OnMultiplayerInvitationReceived;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnMultiplayerInvitationReceived, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MenuScreen.m_OnMultiplayerInvitationReceived;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnMultiplayerInvitationReceived, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<Action> OnCharacterChoosenCallback
	{
		add
		{
			Action<Action> action = MenuScreen.m_OnCharacterChoosenCallback;
			Action<Action> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCharacterChoosenCallback, (Action<Action>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Action> action = MenuScreen.m_OnCharacterChoosenCallback;
			Action<Action> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnCharacterChoosenCallback, (Action<Action>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnDeleteMultiplayerChosenSprites
	{
		add
		{
			Action action = MenuScreen.m_OnDeleteMultiplayerChosenSprites;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnDeleteMultiplayerChosenSprites, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MenuScreen.m_OnDeleteMultiplayerChosenSprites;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnDeleteMultiplayerChosenSprites, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MenuPhase> OnChangeCurrentPhase
	{
		add
		{
			Action<MenuPhase> action = MenuScreen.m_OnChangeCurrentPhase;
			Action<MenuPhase> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnChangeCurrentPhase, (Action<MenuPhase>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MenuPhase> action = MenuScreen.m_OnChangeCurrentPhase;
			Action<MenuPhase> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnChangeCurrentPhase, (Action<MenuPhase>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnShouldBuyCharacterOnCoinsTutorialLoad
	{
		add
		{
			Action<bool> action = MenuScreen.m_OnShouldBuyCharacterOnCoinsTutorialLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnShouldBuyCharacterOnCoinsTutorialLoad, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MenuScreen.m_OnShouldBuyCharacterOnCoinsTutorialLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnShouldBuyCharacterOnCoinsTutorialLoad, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnShouldBuyCharacterOnGemsTutorialLoad
	{
		add
		{
			Action<bool> action = MenuScreen.m_OnShouldBuyCharacterOnGemsTutorialLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnShouldBuyCharacterOnGemsTutorialLoad, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MenuScreen.m_OnShouldBuyCharacterOnGemsTutorialLoad;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MenuScreen.m_OnShouldBuyCharacterOnGemsTutorialLoad, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		notificationButttonsScript = new List<NotificationButton>();
	}

	private void Start()
	{
		Transform transform = base.transform.Find("LandscapeRoot/VerticalLayout/RootCell/HorizontalLayout/CharacterPreviewCell/VerticalLayout(ModeSelection)");
		Transform transform2 = transform.Find("__FlexSpace");
		Transform transform3 = transform.Find("__FixedSpace");
		GameObject gameObject = UnityEngine.Object.Instantiate(MiniclipPrefabs.Instance.newsFeedButton, transform);
		gameObject.transform.SetLocalPositionX(0f);
		notificationButttonsScript.Add(gameObject.GetComponent<NotificationButton>());
		GameObject gameObject2 = UnityEngine.Object.Instantiate(transform3.gameObject, transform);
		gameObject2.transform.SetAsLastSibling();
		gameObject2.gameObject.GetComponent<GUILayoutCell>().SizeValue = 20f;
		gameObject.transform.SetAsLastSibling();
		transform2.SetAsLastSibling();
	}

	private void OnEnable()
	{
		MenuScreenScrollView.OnCharactersInitialized += MenuScreenScrollView_OnCharactersInitialized;
		MenuScreenScrollView.OnDeleteMultiplayerChosenSprites += MenuScreenScrollView_OnDeleteMultiplayerChosenSprites;
		MenuScreenScrollView.OnSetPhase += MenuScreenScrollView_OnSetPhase;
		tk2dUIItem[] array = followUsButton;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += FollowUsButton_OnClick;
		}
		tk2dUIItem[] array2 = facebookButton;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick += FacebookButton_OnClick;
		}
		tk2dUIItem[] array3 = twitterButton;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick += TwitterButton_OnClick;
		}
		tk2dUIItem[] array4 = instagramButton;
		foreach (tk2dUIItem tk2dUIItem5 in array4)
		{
			tk2dUIItem5.OnClick += InstagramButton_OnClick;
		}
		tk2dUIItem[] array5 = youtubeButton;
		foreach (tk2dUIItem tk2dUIItem6 in array5)
		{
			tk2dUIItem6.OnClick += YoutubeButton_OnClick;
		}
		GameManager cachedGameManager = base.CachedManagers.CachedGameManager;
		if (cachedGameManager.CurrentGameType != GameType.None)
		{
			StartCoroutine(EnableRotation());
		}
		if (tk2dSystem.IsRetina)
		{
			GameObject[] array6 = firstWinSprite;
			foreach (GameObject gameObject in array6)
			{
				gameObject.transform.localPosition = new Vector3(67f, 72f, 0f);
			}
		}
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		bool flag = cachedStatisticsManager.IsUnlockedMode(GameType.VSHuman);
		bool flag2 = cachedStatisticsManager.IsUnlockedMode(GameType.BirdsHunt);
		bool flag3 = cachedStatisticsManager.IsUnlockedMode(GameType.AppleShoot);
		bool flag4 = cachedStatisticsManager.IsUnlockedMode(GameType.Tournament);
		bool flag5 = cachedStatisticsManager.IsUnlockedMode(GameType.Multiplayer);
		tk2dUIItem[] array7 = computerButtons;
		foreach (tk2dUIItem tk2dUIItem7 in array7)
		{
			tk2dUIItem7.OnClick += ComputerButton_OnClick;
			tk2dUIItem7.OnDown += ComputerButton_OnDown;
			tk2dUIItem7.OnUp += ComputerButton_OnUp;
		}
		tk2dUIItem[] array8 = humanButtons;
		foreach (tk2dUIItem tk2dUIItem8 in array8)
		{
			tk2dUIItem8.OnClick += HumanButton_OnClick;
			tk2dUIItem8.OnDown += HumanButton_OnDown;
			tk2dUIItem8.OnUp += HumanButton_OnUp;
		}
		tk2dUIItem[] array9 = birdsHuntButtons;
		foreach (tk2dUIItem tk2dUIItem9 in array9)
		{
			tk2dUIItem9.OnClick += BirdsHuntButton_OnClick;
			tk2dUIItem9.OnDown += BirdsHuntButton_OnDown;
			tk2dUIItem9.OnUp += BirdsHuntButton_OnUp;
		}
		tk2dUIItem[] array10 = appleShootButtons;
		foreach (tk2dUIItem tk2dUIItem10 in array10)
		{
			tk2dUIItem10.OnClick += AppleShootButton_OnClick;
			tk2dUIItem10.OnDown += AppleShootButton_OnDown;
			tk2dUIItem10.OnUp += AppleShootButton_OnUp;
		}
		tk2dUIItem[] array11 = tournamentButtons;
		foreach (tk2dUIItem tk2dUIItem11 in array11)
		{
			tk2dUIItem11.OnClick += TournamentButton_OnClick;
			tk2dUIItem11.OnDown += TournamentButton_OnDown;
			tk2dUIItem11.OnUp += TournamentButton_OnUp;
		}
		tk2dUIItem[] array12 = multiplayerButtons;
		foreach (tk2dUIItem tk2dUIItem12 in array12)
		{
			tk2dUIItem12.OnClick += MultiplayerButton_OnClick;
			tk2dUIItem12.OnDown += MultiplayerButton_OnDown;
			tk2dUIItem12.OnUp += MultiplayerButton_OnUp;
		}
		tk2dUIItem[] array13 = vsHumanLockedButton;
		foreach (tk2dUIItem tk2dUIItem13 in array13)
		{
			tk2dUIItem13.OnClick += VSHumanLockedButton_OnClick;
		}
		tk2dUIItem[] array14 = birdsHuntLockedButton;
		foreach (tk2dUIItem tk2dUIItem14 in array14)
		{
			tk2dUIItem14.OnClick += BirdsHuntLockedButton_OnClick;
		}
		tk2dUIItem[] array15 = appleShootLockedButton;
		foreach (tk2dUIItem tk2dUIItem15 in array15)
		{
			tk2dUIItem15.OnClick += AppleShootLockedButton_OnClick;
		}
		tk2dUIItem[] array16 = tournamentLockedButton;
		foreach (tk2dUIItem tk2dUIItem16 in array16)
		{
			tk2dUIItem16.OnClick += TournamentLockedButton_OnClick;
		}
		tk2dUIItem[] array17 = multiplayerLockedButton;
		foreach (tk2dUIItem tk2dUIItem17 in array17)
		{
			tk2dUIItem17.OnClick += MultiplayerLockedButton_OnClick;
		}
		tk2dUIItem[] array18 = bankButtons;
		foreach (tk2dUIItem tk2dUIItem18 in array18)
		{
			tk2dUIItem18.OnClick += BankButton_OnClick;
		}
		int matchesCount = cachedStatisticsManager.MathesCount - cachedStatisticsManager.LosesCount - cachedStatisticsManager.MatchesCountVSHuman;
		if (!flag)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.VSHumanUnlockBorder, vsHumanLockedCountLabel);
		}
		if (!flag2)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.BirdsHuntUnlockBorder, birdsHuntLockedCountLabel);
		}
		if (!flag3)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.AppleShootUnlockBorder, appleShootLockedCountLabel);
		}
		if (!flag4)
		{
			SetLockedLabelText(cachedStatisticsManager.WinsCount + cachedStatisticsManager.MatchesCountVSHuman, cachedGameManager.TournamentUnlockBorder, tournamentLockedCountLabel);
		}
		if (!flag5)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.MultiplayerUnlockBorder, multiplayerLockedCountLabel);
		}
		tk2dUIItem[] array19 = tournamentEasyButton;
		foreach (tk2dUIItem tk2dUIItem19 in array19)
		{
			tk2dUIItem19.OnClick += TournamentEasyButton_OnClick;
		}
		tk2dUIItem[] array20 = tournamentMediumButton;
		foreach (tk2dUIItem tk2dUIItem20 in array20)
		{
			tk2dUIItem20.OnClick += TournamentMediumButton_OnClick;
		}
		tk2dUIItem[] array21 = tournamentHardButton;
		foreach (tk2dUIItem tk2dUIItem21 in array21)
		{
			tk2dUIItem21.OnClick += TournamentHardButton_OnClick;
		}
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			CheckAvailableLevels();
		}, 0f);
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnGemsChanged;
		tk2dUIItem[] array22 = backButtons;
		foreach (tk2dUIItem tk2dUIItem22 in array22)
		{
			tk2dUIItem22.OnClick += BackButton_OnClick;
		}
		tk2dUIItem[] array23 = settingsButtons;
		foreach (tk2dUIItem tk2dUIItem23 in array23)
		{
			tk2dUIItem23.OnClick += SettingsButton_OnClick;
		}
		tk2dUIItem[] array24 = achievementButtons;
		foreach (tk2dUIItem tk2dUIItem24 in array24)
		{
			tk2dUIItem24.OnClick += AchievementButton_OnClick;
		}
		foreach (NotificationButton item in notificationButttonsScript)
		{
			item.enable();
		}
		GUIProposalMenu[] array25 = proposalsMenu;
		foreach (GUIProposalMenu gUIProposalMenu in array25)
		{
			gUIProposalMenu.OnProposalClick = (Action<GUIProposalMenu>)Delegate.Combine(gUIProposalMenu.OnProposalClick, new Action<GUIProposalMenu>(Proposal_OnProposalClick));
		}
		GiftManager.OnGiftAvailable += GiftManager_OnGiftAvailable;
		GiftManager.OnSpecialGiftCharacterOpen += GiftManager_OnSpecialGiftCharacterOpen;
		MicroachievementsManager.OnReachedAchievement += MicroachievementsManager_OnReachedAchievement;
		RemoveAdsPopup.OnRemoveAdsPurchased = (Action)Delegate.Combine(RemoveAdsPopup.OnRemoveAdsPurchased, new Action(RemoveAdsPopup_OnRemoveAdsPurchased));
		tk2dUIItem[] array26 = removeAdsBannerButton;
		foreach (tk2dUIItem tk2dUIItem25 in array26)
		{
			tk2dUIItem25.OnClick += RemoveAdsBannerButton_OnClick;
		}
		SetVolumeForSound("theme_menu_screen", 1f);
		SetVolumeForSound("theme_menu_screen_radio", 0f);
		base.CachedManagers.CachedProposalManager.GenerateProposalMenu();
		UpdateProposals();
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		Queue<MicroachievementInfo> queueAhievements = base.CachedManagers.CachedMicroachievementsManager.QueueAhievements;
		if (queueAhievements.Count != 0)
		{
			MicroachievementsManager_OnReachedAchievement(queueAhievements);
		}
		GameObject[] array27 = firstWinSprite;
		foreach (GameObject gameObject2 in array27)
		{
			gameObject2.SetActive(base.CachedManagers.CachedStatisticsManager.DaysSinceLastFirstWin >= 1);
		}
		GUIScreenManager cachedGUIScreenManager = base.CachedManagers.CachedGUIScreenManager;
		if (cachedGUIScreenManager.IsNeedPlayUnlockVSHumanModeEffect)
		{
			PlayUnlockEffect(humanButtons);
		}
		if (cachedGUIScreenManager.IsNeedPlayUnlockAppleModeEffect)
		{
			PlayUnlockEffect(appleShootButtons);
		}
		if (cachedGUIScreenManager.IsNeedPlayUnlockBirdHuntModeEffect)
		{
			PlayUnlockEffect(birdsHuntButtons);
		}
		if (cachedGUIScreenManager.IsNeedPlayUnlockTournamentModeEffect)
		{
			PlayUnlockEffect(tournamentButtons);
		}
		if (cachedGUIScreenManager.IsNeedPlayUnlockMultiplayerModeEffect)
		{
			PlayUnlockEffect(multiplayerButtons);
		}
		TapToStartScreen.OnLoaderRemoved += TapToStartScreen_OnLoaderRemoved;
		ShopManager.OnAllProductsReceived += ShopManager_OnAllProductsReceived;
		MultiplayerManager.OnPlayerDisconnected += MultiplayerManager_OnPlayerDisconnected;
		LLMultiplayerManager.OnMatchmakerCancelled += LLMultiplayerManager_OnMatchmakerCancelled;
		LLMultiplayerManager.OnAllOpponentsFound += LLMultiplayerManager_OnAllOpponentsFound;
		ICloudManager.OnProgressDownloaded += ICloudManager_OnProgressDownloaded;
		RemoveAdsBannerEnabled();
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		base.CachedManagers.CachedResourceManager.UnloadGeneralSpriteCollections(0.1f);
		timer.OnTimerChanged += Timer_OnTimerChanged;
		UpdateBankAlertIcons();
		SuperGiftPopup.OnAllRewardsReceived += SuperGiftPopup_OnAllRewardsReceived;
		FreeChestPopup.OnVideoWatched += FreeChestPopup_OnVideoWatched;
		CloudSaveManager.OnSaveLoaded += Update;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.ReceivedProductsIDS += MCInnAppsmanager_ReceivedProductsIDS;
	}

	private void OnDisable()
	{
		MenuScreenScrollView.OnCharactersInitialized -= MenuScreenScrollView_OnCharactersInitialized;
		MenuScreenScrollView.OnDeleteMultiplayerChosenSprites -= MenuScreenScrollView_OnDeleteMultiplayerChosenSprites;
		MenuScreenScrollView.OnSetPhase -= MenuScreenScrollView_OnSetPhase;
		tk2dUIItem[] array = followUsButton;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= FollowUsButton_OnClick;
		}
		tk2dUIItem[] array2 = facebookButton;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick -= FacebookButton_OnClick;
		}
		tk2dUIItem[] array3 = twitterButton;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick -= TwitterButton_OnClick;
		}
		tk2dUIItem[] array4 = instagramButton;
		foreach (tk2dUIItem tk2dUIItem5 in array4)
		{
			tk2dUIItem5.OnClick -= InstagramButton_OnClick;
		}
		tk2dUIItem[] array5 = youtubeButton;
		foreach (tk2dUIItem tk2dUIItem6 in array5)
		{
			tk2dUIItem6.OnClick -= YoutubeButton_OnClick;
		}
		tk2dUIItem[] array6 = computerButtons;
		foreach (tk2dUIItem tk2dUIItem7 in array6)
		{
			tk2dUIItem7.OnClick -= ComputerButton_OnClick;
			tk2dUIItem7.OnDown -= ComputerButton_OnDown;
			tk2dUIItem7.OnUp -= ComputerButton_OnUp;
		}
		tk2dUIItem[] array7 = humanButtons;
		foreach (tk2dUIItem tk2dUIItem8 in array7)
		{
			tk2dUIItem8.OnClick -= HumanButton_OnClick;
			tk2dUIItem8.OnDown -= HumanButton_OnDown;
			tk2dUIItem8.OnUp -= HumanButton_OnUp;
		}
		tk2dUIItem[] array8 = birdsHuntButtons;
		foreach (tk2dUIItem tk2dUIItem9 in array8)
		{
			tk2dUIItem9.OnClick -= BirdsHuntButton_OnClick;
			tk2dUIItem9.OnDown -= BirdsHuntButton_OnDown;
			tk2dUIItem9.OnUp -= BirdsHuntButton_OnUp;
		}
		tk2dUIItem[] array9 = appleShootButtons;
		foreach (tk2dUIItem tk2dUIItem10 in array9)
		{
			tk2dUIItem10.OnClick -= AppleShootButton_OnClick;
			tk2dUIItem10.OnDown -= AppleShootButton_OnDown;
			tk2dUIItem10.OnUp -= AppleShootButton_OnUp;
		}
		tk2dUIItem[] array10 = tournamentButtons;
		foreach (tk2dUIItem tk2dUIItem11 in array10)
		{
			tk2dUIItem11.OnClick -= TournamentButton_OnClick;
			tk2dUIItem11.OnDown -= TournamentButton_OnDown;
			tk2dUIItem11.OnUp -= TournamentButton_OnUp;
		}
		tk2dUIItem[] array11 = multiplayerButtons;
		foreach (tk2dUIItem tk2dUIItem12 in array11)
		{
			tk2dUIItem12.OnClick -= MultiplayerButton_OnClick;
			tk2dUIItem12.OnDown -= MultiplayerButton_OnDown;
			tk2dUIItem12.OnUp -= MultiplayerButton_OnUp;
		}
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnGemsChanged;
		tk2dUIItem[] array12 = backButtons;
		foreach (tk2dUIItem tk2dUIItem13 in array12)
		{
			tk2dUIItem13.OnClick -= BackButton_OnClick;
		}
		tk2dUIItem[] array13 = settingsButtons;
		foreach (tk2dUIItem tk2dUIItem14 in array13)
		{
			tk2dUIItem14.OnClick -= SettingsButton_OnClick;
		}
		tk2dUIItem[] array14 = achievementButtons;
		foreach (tk2dUIItem tk2dUIItem15 in array14)
		{
			tk2dUIItem15.OnClick -= AchievementButton_OnClick;
		}
		foreach (NotificationButton item in notificationButttonsScript)
		{
			item.disable();
		}
		GUIProposalMenu[] array15 = proposalsMenu;
		foreach (GUIProposalMenu gUIProposalMenu in array15)
		{
			gUIProposalMenu.OnProposalClick = (Action<GUIProposalMenu>)Delegate.Remove(gUIProposalMenu.OnProposalClick, new Action<GUIProposalMenu>(Proposal_OnProposalClick));
		}
		tk2dUIItem[] array16 = tournamentEasyButton;
		foreach (tk2dUIItem tk2dUIItem16 in array16)
		{
			tk2dUIItem16.OnClick -= TournamentEasyButton_OnClick;
		}
		tk2dUIItem[] array17 = tournamentMediumButton;
		foreach (tk2dUIItem tk2dUIItem17 in array17)
		{
			tk2dUIItem17.OnClick -= TournamentMediumButton_OnClick;
		}
		tk2dUIItem[] array18 = tournamentHardButton;
		foreach (tk2dUIItem tk2dUIItem18 in array18)
		{
			tk2dUIItem18.OnClick -= TournamentHardButton_OnClick;
		}
		tk2dUIItem[] array19 = vsHumanLockedButton;
		foreach (tk2dUIItem tk2dUIItem19 in array19)
		{
			tk2dUIItem19.OnClick -= VSHumanLockedButton_OnClick;
		}
		tk2dUIItem[] array20 = birdsHuntLockedButton;
		foreach (tk2dUIItem tk2dUIItem20 in array20)
		{
			tk2dUIItem20.OnClick -= BirdsHuntLockedButton_OnClick;
		}
		tk2dUIItem[] array21 = appleShootLockedButton;
		foreach (tk2dUIItem tk2dUIItem21 in array21)
		{
			tk2dUIItem21.OnClick -= AppleShootLockedButton_OnClick;
		}
		tk2dUIItem[] array22 = tournamentLockedButton;
		foreach (tk2dUIItem tk2dUIItem22 in array22)
		{
			tk2dUIItem22.OnClick -= TournamentLockedButton_OnClick;
		}
		tk2dUIItem[] array23 = multiplayerLockedButton;
		foreach (tk2dUIItem tk2dUIItem23 in array23)
		{
			tk2dUIItem23.OnClick -= MultiplayerLockedButton_OnClick;
		}
		tk2dUIItem[] array24 = bankButtons;
		foreach (tk2dUIItem tk2dUIItem24 in array24)
		{
			tk2dUIItem24.OnClick -= BankButton_OnClick;
		}
		GiftManager.OnGiftAvailable -= GiftManager_OnGiftAvailable;
		GiftManager.OnSpecialGiftCharacterOpen -= GiftManager_OnSpecialGiftCharacterOpen;
		MicroachievementsManager.OnReachedAchievement -= MicroachievementsManager_OnReachedAchievement;
		RemoveAdsPopup.OnRemoveAdsPurchased = (Action)Delegate.Remove(RemoveAdsPopup.OnRemoveAdsPurchased, new Action(RemoveAdsPopup_OnRemoveAdsPurchased));
		tk2dUIItem[] array25 = removeAdsBannerButton;
		foreach (tk2dUIItem tk2dUIItem25 in array25)
		{
			tk2dUIItem25.OnClick -= RemoveAdsBannerButton_OnClick;
		}
		if (SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			SetVolumeForSound("theme_menu_screen", 0f);
			SetVolumeForSound("theme_menu_screen_radio", 1f);
		}
		TapToStartScreen.OnLoaderRemoved -= TapToStartScreen_OnLoaderRemoved;
		ShopManager.OnAllProductsReceived -= ShopManager_OnAllProductsReceived;
		MultiplayerManager.OnPlayerDisconnected -= MultiplayerManager_OnPlayerDisconnected;
		LLMultiplayerManager.OnMatchmakerCancelled -= LLMultiplayerManager_OnMatchmakerCancelled;
		LLMultiplayerManager.OnAllOpponentsFound -= LLMultiplayerManager_OnAllOpponentsFound;
		ICloudManager.OnProgressDownloaded -= ICloudManager_OnProgressDownloaded;
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		timer.OnTimerChanged -= Timer_OnTimerChanged;
		SuperGiftPopup.OnAllRewardsReceived -= SuperGiftPopup_OnAllRewardsReceived;
		FreeChestPopup.OnVideoWatched -= FreeChestPopup_OnVideoWatched;
		CloudSaveManager.OnSaveLoaded -= Update;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.ReceivedProductsIDS -= MCInnAppsmanager_ReceivedProductsIDS;
	}

	private void OnDestroy()
	{
		TutorialManager instanceIfExist = SingletonMonoBehaviour<TutorialManager>.InstanceIfExist;
		if (instanceIfExist != null)
		{
			instanceIfExist.StopBuyCharacterOnCoinsTutorial();
		}
		if (SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			AudioManager.StopAudioWithKey("theme_menu_screen");
			AudioManager.StopAudioWithKey("theme_menu_screen_radio");
		}
	}

	private void Update()
	{
		TweenColor[] array = fadeTweener;
		foreach (TweenColor tweenColor in array)
		{
			if (!tweenColor.gameObject.activeInHierarchy && tweenColor.enabled)
			{
				tweenColor.Update();
			}
		}
		timer.UpdateTimer(Time.unscaledDeltaTime);
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.CachedManagers.CachedSubscriptionManager.SendSubscriptionStateEvent();
		base.CachedManagers.CachedStatisticsManager.MenuScreensCount++;
		controlAchievement.HideImmediately();
		AudioManager.StopAudioWithKey("crowd_loop", 1f);
		SetPhase(MenuPhase.Mode);
		SetMode(GameType.VSComputer);
		ProposalManager cachedProposalManager = base.CachedManagers.CachedProposalManager;
		cachedProposalManager.ResetBuyCharacter();
		MenuScreen.m_OnInitializeCharacters?.Invoke();
		if (cachedProposalManager.GiftCharacter != null && !cachedProposalManager.GiftCharacter.Bought)
		{
			cachedProposalManager.GiftCharacter.PurchaseComplete(LLTransactionState.RePurchased);
			MenuScreen.m_OnCheckBoughtCharacters?.Invoke();
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.GiftPopup);
		}
		else if (cachedProposalManager.IsForcedPack && !SingletonMonoBehaviour<ShopManager>.Instance.AllIncludedBought)
		{
			cachedProposalManager.IsNeedShowForcedTimerPopup = false;
			PopupType popupType = PopupType.Upgrade;
			int spentBucks = base.CachedManagers.CachedStatisticsManager.SpentBucks;
			ProposalMenuType lastProposalWithTimerType = cachedProposalManager.LastProposalWithTimerType;
			bool flag = cachedProposalManager.ProposalTimer.TimerInSeconds == 0f;
			flag = flag || (lastProposalWithTimerType != ProposalMenuType.Upgrade && lastProposalWithTimerType != ProposalMenuType.VipPack && lastProposalWithTimerType != ProposalMenuType.ChampionPack);
			popupType = ((spentBucks < cachedProposalManager.MinSpentBucksForChampionPack) ? PopupType.Upgrade : ((spentBucks <= cachedProposalManager.MaxSpentBucksForChampionPack) ? PopupType.ChampionPack : (cachedProposalManager.IsVipPackAvailable ? PopupType.None : PopupType.None)));
			if (popupType != PopupType.None)
			{
				
				 cachedProposalManager.IsShowingForcedPack = flag;
			 	 cachedProposalManager.IsForcedPackAvailable = false;
				 base.CachedManagers.CachedGUIPopupManager.ShowPopup(popupType);
				
			}
		}
		else if (cachedProposalManager.IsNeedShowForcedTimerPopup)
		{
			cachedProposalManager.IsNeedShowForcedTimerPopup = false;
			if (cachedProposalManager.LastProposalWithTimerType != ProposalMenuType.None && !cachedProposalManager.IsNeedShowInvitePopup)
			{
				GUIProposalMenu[] array = proposalsMenu;
				foreach (GUIProposalMenu gUIProposalMenu in array)
				{
					gUIProposalMenu.Deactivate();
				}
				cachedProposalManager.GetProposalMenu(null, cachedProposalManager.LastProposalWithTimerType);
			}
		}
		else if (cachedProposalManager.IsPromoFetcherAvalaible && base.CachedManagers.CachedGUIPopupManager.ActivePopup == null)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.PromoFetcherPopup);
			base.CachedManagers.CachedStatisticsManager.LastPromoFetcher = DateTime.Now;
		}
		base.CachedManagers.CachedShopManager.CheckForNotReceivedItems();
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		AudioManager.StopAudioWithKey("theme_main");
		AudioManager.StopAudioWithKey("theme_result_screen");
		if (!base.CachedManagers.CachedGameManager.IsTapToStartScreenExist)
		{
			cachedProposalManager.ShowInterstitialCrosspromo();
		}
		MenuScreen.m_OnCharacterFirstSelection?.Invoke(true);
		if (SingletonMonoBehaviour<RateMeManager>.Instance.IsRateMeAvalaible)
		{
			MCServices.ShowRatePopup();
		}
		else if (SingletonMonoBehaviour<GUIPopupManager>.Instance.ActivePopup == null && BowmastersNewsfeed.Instance.AreThereShowableUrgents())
		{
			BowmastersNewsfeed.Instance.ShowUrgentBoard();
		}
		base.Show(onFinished, immediately, delay, callInvokes);
		if (base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive && base.CachedManagers.CachedSubscriptionManager.SubscriptionRewardCount > 0 && base.CachedManagers.CachedGUIPopupManager.ActivePopup == null)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.AutoRenewableSubscribeReward, true, null, true);
		}
		CustomPlayerPrefs.Save();
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		controlAchievement.HideImmediately();
		for (int i = 0; i < currentEffects.Count; i++)
		{
			UnityEngine.Object.Destroy(currentEffects[i]);
		}
		currentEffects.Clear();
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	public void UpdateProposals()
	{
		ProposalManager cachedProposalManager = base.CachedManagers.CachedProposalManager;
		ProposalMenuType currentProposalMenu = cachedProposalManager.CurrentProposalMenu;
		GUIProposalMenu[] array = proposalsMenu;
		foreach (GUIProposalMenu gUIProposalMenu in array)
		{
			if ((gUIProposalMenu.TypeProposal == currentProposalMenu)&&(gUIProposalMenu.TypeProposal==ProposalMenuType.Gift||gUIProposalMenu.TypeProposal==ProposalMenuType.FreeChest))
			{
				gUIProposalMenu.Activate();
			}
			else
			{
				gUIProposalMenu.Deactivate();
			}
		}
		if (currentProposalMenu == ProposalMenuType.TryNew && base.CachedManagers.CachedGUIPopupManager.ActivePopup == null)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.TryCharacterProposal);
			cachedProposalManager.LastTryCharacterGeneratedMatches = base.CachedManagers.CachedStatisticsManager.MathesCount;
		}
	}

	public void HideMenuAndShowRouletteScreen()
	{
		controlFade.gameObject.SetActive(true);
		controlFade.Show(delegate
		{
			ChangeTouchBlockerActivity(true);
			base.CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.white;
			base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(false, delegate
			{
				ChangeTouchBlockerActivity(false);
				controlFade.HideImmediately();
				controlFade.gameObject.SetActive(false);
				base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Roulette, delegate
				{
					base.CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.black;
				});
			}, false);
		}, false, timeDelayRouletteScreen, true);
	}

	public void OnInvitationReceived(Action callback)
	{
		MenuScreen.m_OnMultiplayerInvitationReceived?.Invoke(true);
		MenuScreen.m_OnCharacterChoosenCallback?.Invoke(callback);
		if (base.CachedManagers.CachedGUIScreenManager.ActiveScreen == GUIScreenType.Main)
		{
			if (LLMultiplayerManager.IsCurrentGameVersionNotTheLatest)
			{
				base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MultiplayerBuildVersion, true, null, true);
			}
			else
			{
				MultiplayerButton_OnClick();
			}
		}
	}

	public void UpdateMainMenuScreen(bool isForced)
	{
		Update();
		MenuScreen.m_OnCheckBoughtCharacters?.Invoke();
		CheckAvailableLevels();
		base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
		if (isForced)
		{
			MenuScreenScrollView.InitializeCharacters(true);
			MenuScreenScrollView.UpdateCharacterCharacteristics();
		}
	}

	private void SetMode(GameType gt)
	{
		currentSelectedMode = gt;
		base.CachedManagers.CachedGameManager.CurrentGameType = currentSelectedMode;
	}

	private void SetPhase(MenuPhase newPhase)
	{
		bool flag = false;
		if (((currentPhase == MenuPhase.Mode || currentPhase == MenuPhase.Tournament) && newPhase == MenuPhase.First) || (currentPhase == MenuPhase.First && (newPhase == MenuPhase.Tournament || newPhase == MenuPhase.Mode)) || (currentPhase == MenuPhase.Mode && newPhase == MenuPhase.Tournament) || (currentPhase == MenuPhase.Tournament && newPhase == MenuPhase.Mode))
		{
			flag = true;
		}
		TweenCallback callback = null;
		if (currentSelectedMode == GameType.Multiplayer && currentPhase == MenuPhase.First && newPhase == MenuPhase.Mode && MenuScreen.m_OnStartSpecialIdleAnimations != null)
		{
			MenuScreen.m_OnStartSpecialIdleAnimations(true, false);
		}
		currentPhase = newPhase;
		MenuScreen.m_OnChangeCurrentPhase?.Invoke(currentPhase);
		if (currentPhase != MenuPhase.Mode && isFollowUsShown)
		{
			FollowButtonAction(CachedAnimationFollowUsHidden, CachedAnimationFollowUsHidden);
			isFollowUsShown = !isFollowUsShown;
		}
		switch (currentPhase)
		{
		case MenuPhase.Mode:
			callback = delegate
			{
				GameObject[] array2 = modeSelectionInfoRoots;
				foreach (GameObject gameObject in array2)
				{
					gameObject.SetActive(true);
				}
				GameObject[] array3 = modeSelectionIconRoots;
				foreach (GameObject gameObject2 in array3)
				{
					gameObject2.SetActive(true);
				}
				GameObject[] array4 = characterSelectionRoots;
				foreach (GameObject gameObject3 in array4)
				{
					gameObject3.SetActive(false);
				}
				GameObject[] array5 = tournamentDifficultSelectionRoots;
				foreach (GameObject gameObject4 in array5)
				{
					gameObject4.SetActive(false);
				}
			};
			break;
		case MenuPhase.Tournament:
			callback = delegate
			{
				GameObject[] array2 = modeSelectionInfoRoots;
				foreach (GameObject gameObject in array2)
				{
					gameObject.SetActive(false);
				}
				GameObject[] array3 = modeSelectionIconRoots;
				foreach (GameObject gameObject2 in array3)
				{
					gameObject2.SetActive(true);
				}
				GameObject[] array4 = characterSelectionRoots;
				foreach (GameObject gameObject3 in array4)
				{
					gameObject3.SetActive(false);
				}
				GameObject[] array5 = tournamentDifficultSelectionRoots;
				foreach (GameObject gameObject4 in array5)
				{
					gameObject4.SetActive(true);
				}
			};
			break;
		case MenuPhase.First:
		case MenuPhase.Second:
			SingletonMonoBehaviour<ShopManager>.Instance.CheckForNotReceivedItems();
			callback = delegate
			{
				GameObject[] array2 = modeSelectionInfoRoots;
				foreach (GameObject gameObject in array2)
				{
					gameObject.SetActive(false);
				}
				GameObject[] array3 = modeSelectionIconRoots;
				foreach (GameObject gameObject2 in array3)
				{
					gameObject2.SetActive(false);
				}
				GameObject[] array4 = tournamentDifficultSelectionRoots;
				foreach (GameObject gameObject3 in array4)
				{
					gameObject3.SetActive(false);
				}
				GameObject[] array5 = characterSelectionRoots;
				foreach (GameObject gameObject4 in array5)
				{
					gameObject4.SetActive(true);
				}
				MenuScreen.m_OnUpdateBuyButtons?.Invoke();
				MenuScreen.m_OnContentPositionChange?.Invoke();
			};
			break;
		}
		if (flag)
		{
			ChangeTouchBlockerActivity(true);
			controlFade.gameObject.SetActive(true);
			TweenColor[] array = fadeTweener;
			foreach (TweenColor t in array)
			{
				t.gameObject.SetActive(true);
				t.IsReversed = false;
				t.ResetTween();
				t.SetOnFinishedDelegate(delegate(ITweener tw)
				{
					callback(null);
					TweenColor tweenColor = tw as TweenColor;
					tweenColor.IsReversed = true;
					tweenColor.ResetTween();
					tweenColor.SetOnFinishedDelegate(delegate
					{
						ChangeTouchBlockerActivity(false);
						t.gameObject.SetActive(false);
					});
					tweenColor.enabled = true;
				});
				t.enabled = true;
			}
		}
		else
		{
			callback(null);
		}
		if (currentPhase == MenuPhase.First)
		{
			AudioManager.StopAudioWithKey("player_two_choose");
			AudioManager.PlayAudioWithKey("player_one_choose");
		}
		else if (currentPhase == MenuPhase.Second)
		{
			AudioManager.StopAudioWithKey("player_one_choose");
			AudioManager.PlayAudioWithKey("player_two_choose");
		}
	}

	private void SetVolumeForSound(string soundName, float volume)
	{
		SoundObject[] playingAudioObjects = SoundsManager.GetPlayingAudioObjects(soundName);
		if (playingAudioObjects.Length == 0)
		{
			AudioManager.PlayAudioWithKey(soundName);
			playingAudioObjects = SoundsManager.GetPlayingAudioObjects(soundName);
		}
		SoundObject[] array = playingAudioObjects;
		foreach (SoundObject soundObject in array)
		{
			soundObject.Volume = volume;
		}
	}

	private void ShowAchievement(Queue<MicroachievementInfo> queue)
	{
		if (queue.Count <= 0)
		{
			return;
		}
		MicroachievementInfo info = queue.Peek();
		if (!info.isRestoredAchievement)
		{
			if (!string.IsNullOrEmpty(info.localisationName))
			{
				tk2dTextMesh[] array = textsAchievement;
				foreach (tk2dTextMesh tk2dTextMesh2 in array)
				{
					tk2dTextMesh2.text = info.LocalisationName();
				}
			}
			if (!string.IsNullOrEmpty(info.CurrentSpriteName()))
			{
				tk2dBaseSprite[] array2 = iconsAchievement;
				foreach (tk2dBaseSprite tk2dBaseSprite2 in array2)
				{
					tk2dBaseSprite2.SetSprite(info.CurrentSpriteName());
				}
			}
			if (info.coinsAward > 0)
			{
				GUILayoutCell[] array3 = countCellsAchievement;
				foreach (GUILayoutCell gUILayoutCell in array3)
				{
					gUILayoutCell.gameObject.SetActive(true);
				}
				tk2dTextMesh[] array4 = countLabelsAchiviement;
				foreach (tk2dTextMesh tk2dTextMesh3 in array4)
				{
					tk2dTextMesh3.text = info.AwardString();
				}
			}
			else
			{
				GUILayoutCell[] array5 = countCellsAchievement;
				foreach (GUILayoutCell gUILayoutCell2 in array5)
				{
					gUILayoutCell2.gameObject.SetActive(false);
				}
			}
			AudioManager.PlayAudioWithKey("achievement");
			if (info.type == MicroachievementType.Bullseye)
			{
				AudioManager.PlayAudioWithKey("bullseye");
				AudioManager.PlayAudioWithKey("achievement_bull_head");
			}
			else if (info.type == MicroachievementType.Headshot)
			{
				AudioManager.PlayAudioWithKey("headshot");
				AudioManager.PlayAudioWithKey("achievement_bull_head");
			}
			StartCoroutine(CoroutineShowAchievement(info));
		}
		else
		{
			Queue<MicroachievementInfo> queueAhievements = base.CachedManagers.CachedMicroachievementsManager.QueueAhievements;
			if (queueAhievements.Count > 0)
			{
				queueAhievements.Dequeue();
				ShowAchievement(queueAhievements);
			}
		}
	}

	private void PlayUnlockEffect(tk2dUIItem[] buttons)
	{
		foreach (tk2dUIItem tk2dUIItem2 in buttons)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIMenuUnlockModeButton));
			gameObject.transform.parent = tk2dUIItem2.transform;
			gameObject.transform.localPosition = Vector3.zero;
			currentEffects.Add(gameObject);
		}
	}

	private void SetTournamentDescription(string labelKey, TournamentDifficultType difficult, tk2dTextMesh[] labels)
	{
		string textByKey = base.CachedManagers.CachedLocalisationManager.GetTextByKey(labelKey);
		textByKey = textByKey.Replace("%", base.CachedManagers.CachedTournamentManager.TournamentCoinsAward(difficult).Substring(1));
		foreach (tk2dTextMesh tk2dTextMesh2 in labels)
		{
			tk2dTextMesh2.text = textByKey;
		}
	}

	private void StartLockedButtonTweener(TweenPosition[] tweener)
	{
		foreach (TweenPosition tweenPosition in tweener)
		{
			if (tweenPosition.gameObject.activeInHierarchy)
			{
				tweenPosition.ResetTween();
				tweenPosition.enabled = true;
			}
		}
	}

	private void ChangeModeSprite(tk2dBaseSprite[] modeSprite, bool isSelected)
	{
		foreach (tk2dBaseSprite tk2dBaseSprite2 in modeSprite)
		{
			tk2dBaseSprite2.SetSprite((!isSelected) ? "btn_mode" : "btn_mode_selected");
		}
	}

	private void ChangeTouchBlockerActivity(bool isActive)
	{
		GameObject[] array = touchBlocker;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(isActive);
		}
	}

	private void SetLockedLabelText(int matchesCount, int border, tk2dTextMesh[] labels)
	{
		string textByKey = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.main.lockedlabel");
		string text = string.Format("{0} {1} {2}", matchesCount, textByKey, border);
		foreach (tk2dTextMesh tk2dTextMesh2 in labels)
		{
			tk2dTextMesh2.text = text;
		}
	}

	private void CheckAvailableLevels()
	{
		GameManager cachedGameManager = base.CachedManagers.CachedGameManager;
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		bool flag = cachedStatisticsManager.IsUnlockedMode(GameType.VSHuman);
		bool flag2 = cachedStatisticsManager.IsUnlockedMode(GameType.BirdsHunt);
		bool flag3 = cachedStatisticsManager.IsUnlockedMode(GameType.AppleShoot);
		bool flag4 = cachedStatisticsManager.IsUnlockedMode(GameType.Tournament);
		bool flag5 = cachedStatisticsManager.IsUnlockedMode(GameType.Multiplayer);
		tk2dUIItem[] array = humanButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.gameObject.SetActive(flag);
		}
		tk2dUIItem[] array2 = birdsHuntButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.gameObject.SetActive(flag2);
		}
		tk2dUIItem[] array3 = appleShootButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.gameObject.SetActive(flag3);
		}
		tk2dUIItem[] array4 = tournamentButtons;
		foreach (tk2dUIItem tk2dUIItem5 in array4)
		{
			tk2dUIItem5.gameObject.SetActive(flag4);
		}
		tk2dUIItem[] array5 = multiplayerButtons;
		foreach (tk2dUIItem tk2dUIItem6 in array5)
		{
			tk2dUIItem6.gameObject.SetActive(flag5);
		}
		tk2dUIItem[] array6 = vsHumanLockedButton;
		foreach (tk2dUIItem tk2dUIItem7 in array6)
		{
			tk2dUIItem7.gameObject.SetActive(!flag);
		}
		tk2dUIItem[] array7 = birdsHuntLockedButton;
		foreach (tk2dUIItem tk2dUIItem8 in array7)
		{
			tk2dUIItem8.gameObject.SetActive(!flag2);
		}
		tk2dUIItem[] array8 = appleShootLockedButton;
		foreach (tk2dUIItem tk2dUIItem9 in array8)
		{
			tk2dUIItem9.gameObject.SetActive(!flag3);
		}
		tk2dUIItem[] array9 = tournamentLockedButton;
		foreach (tk2dUIItem tk2dUIItem10 in array9)
		{
			tk2dUIItem10.gameObject.SetActive(!flag4);
		}
		tk2dUIItem[] array10 = multiplayerLockedButton;
		foreach (tk2dUIItem tk2dUIItem11 in array10)
		{
			tk2dUIItem11.gameObject.SetActive(!flag5);
		}
		int matchesCount = cachedStatisticsManager.MathesCount - cachedStatisticsManager.LosesCount - cachedStatisticsManager.MatchesCountVSHuman;
		if (!flag)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.VSHumanUnlockBorder, vsHumanLockedCountLabel);
		}
		if (!flag2)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.BirdsHuntUnlockBorder, birdsHuntLockedCountLabel);
		}
		if (!flag3)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.AppleShootUnlockBorder, appleShootLockedCountLabel);
		}
		if (!flag4)
		{
			SetLockedLabelText(cachedStatisticsManager.WinsCount + cachedStatisticsManager.MatchesCountVSHuman, cachedGameManager.TournamentUnlockBorder, tournamentLockedCountLabel);
		}
		if (!flag5)
		{
			SetLockedLabelText(matchesCount, cachedGameManager.MultiplayerUnlockBorder, multiplayerLockedCountLabel);
		}
		bool isMediumTournamentUnlocked = base.CachedManagers.CachedTournamentManager.IsMediumTournamentUnlocked;
		bool isHardTournamentUnlocked = base.CachedManagers.CachedTournamentManager.IsHardTournamentUnlocked;
		tk2dUIItem[] array11 = tournamentMediumButton;
		foreach (tk2dUIItem tk2dUIItem12 in array11)
		{
			tk2dUIItem12.gameObject.SetActive(isMediumTournamentUnlocked);
		}
		GameObject[] array12 = tournamentMediumLocked;
		foreach (GameObject gameObject in array12)
		{
			gameObject.SetActive(!isMediumTournamentUnlocked);
		}
		tk2dUIItem[] array13 = tournamentHardButton;
		foreach (tk2dUIItem tk2dUIItem13 in array13)
		{
			tk2dUIItem13.gameObject.SetActive(isHardTournamentUnlocked);
		}
		GameObject[] array14 = tournamentHardLocked;
		foreach (GameObject gameObject2 in array14)
		{
			gameObject2.SetActive(!isHardTournamentUnlocked);
		}
		string labelKey = "ui.main.tournament_mode.easy.description";
		SetTournamentDescription(labelKey, TournamentDifficultType.Easy, tournamentEasyLabel);
		labelKey = ((!isMediumTournamentUnlocked) ? "ui.main.tournament_mode.normal.description.locked" : "ui.main.tournament_mode.normal.description.unlocked");
		SetTournamentDescription(labelKey, TournamentDifficultType.Medium, tournamentMediumLabel);
		labelKey = ((!isHardTournamentUnlocked) ? "ui.main.tournament_mode.legendary.description.locked" : "ui.main.tournament_mode.legendary.description.unlocked.android");
		SetTournamentDescription(labelKey, TournamentDifficultType.Hard, tournamentHardLabel);
	}

	private void FollowButtonAction(Spine.Animation firstAnimation, Spine.Animation secondAnimation)
	{
		SkeletonAnimation[] array = followUsAnimation;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			skeletonAnimation.State.SetAnimation(0, firstAnimation, false);
		}
		tk2dUIItem[] array2 = followUsButton;
		foreach (tk2dUIItem tk2dUIItem2 in array2)
		{
			tk2dUIItem2.enabled = false;
		}
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			SkeletonAnimation[] array3 = followUsAnimation;
			foreach (SkeletonAnimation skeletonAnimation2 in array3)
			{
				skeletonAnimation2.State.SetAnimation(0, secondAnimation, true);
			}
			tk2dUIItem[] array4 = followUsButton;
			foreach (tk2dUIItem tk2dUIItem3 in array4)
			{
				tk2dUIItem3.enabled = true;
			}
		}, firstAnimation.Duration * 0.5f);
	}

	private void RemoveAdsBannerEnabled()
	{
		bool active = !base.CachedManagers.CachedShopManager.NoAdsBought && base.CachedManagers.CachedStatisticsManager.MathesCount >= base.CachedManagers.CachedProposalManager.MinMatchesForInterstitial;
		GameObject[] array = removeAdsBannerRoot;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(active);
		}
	}

	private void UpdateBankAlertIcons()
	{
		DateTime dateTime = default(DateTime);
		DateTime dateTime2 = default(DateTime);
		if (base.CachedManagers.CachedStatisticsManager.FreeGemsReceived > 1)
		{
			dateTime = DateTime.Now.Subtract(base.CachedManagers.CachedStatisticsManager.CheatedTime);
			dateTime2 = CustomPlayerPrefs.GetDateTime("free_gems_timer_ended", dateTime);
			if ((dateTime2 - dateTime).TotalSeconds > 0.0 && !timer.IsTimerActive)
			{
				timer.TimerInSeconds = (float)(dateTime2 - dateTime).TotalSeconds;
				timer.IsTimerActive = true;
				timer.UpdateTimer(Time.unscaledDeltaTime);
			}
		}
		for (int i = 0; i < bankAlertObjects.Length; i++)
		{
			bankAlertObjects[i].SetActive(!base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive && (base.CachedManagers.CachedStatisticsManager.FreeGemsReceived <= 1 || !timer.IsTimerActive));
		}
	}

	private void MenuScreenScrollView_OnSetPhase(MenuPhase obj)
	{
		SetPhase(obj);
	}

	private void MenuScreenScrollView_OnDeleteMultiplayerChosenSprites()
	{
		MenuScreen.m_OnDeleteMultiplayerChosenSprites?.Invoke();
	}

	private void MenuScreenScrollView_OnCharactersInitialized(bool obj)
	{
		isCharactersInitialized = obj;
	}

	private void ComputerButton_OnDown()
	{
		ChangeModeSprite(computerButtonsBack, true);
	}

	private void ComputerButton_OnUp()
	{
		ChangeModeSprite(computerButtonsBack, false);
	}

	private void HumanButton_OnDown()
	{
		ChangeModeSprite(humanButtonsBack, true);
	}

	private void HumanButton_OnUp()
	{
		ChangeModeSprite(humanButtonsBack, false);
	}

	private void BirdsHuntButton_OnDown()
	{
		ChangeModeSprite(birdsButtonBack, true);
	}

	private void BirdsHuntButton_OnUp()
	{
		ChangeModeSprite(birdsButtonBack, false);
	}

	private void AppleShootButton_OnDown()
	{
		ChangeModeSprite(appleShootButtonsBack, true);
	}

	private void AppleShootButton_OnUp()
	{
		ChangeModeSprite(appleShootButtonsBack, false);
	}

	private void TournamentButton_OnDown()
	{
		ChangeModeSprite(tournamentButtonsBack, true);
	}

	private void TournamentButton_OnUp()
	{
		ChangeModeSprite(tournamentButtonsBack, false);
	}

	private void MultiplayerButton_OnDown()
	{
		ChangeModeSprite(multiplayerButtonsBack, true);
	}

	private void MultiplayerButton_OnUp()
	{
		ChangeModeSprite(multiplayerButtonsBack, false);
	}

	private void BackButton_OnClick()
	{
		switch (currentPhase)
		{
		case MenuPhase.Mode:
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
			androidJavaClass.CallStatic(AndroidPluginNames.BOWMASTERS_ACTIVITY_SHOW_EXIT_POPUP);
			break;
		}
		case MenuPhase.Tournament:
			SetPhase(MenuPhase.Mode);
			break;
		case MenuPhase.First:
			if (currentSelectedMode == GameType.Multiplayer && MenuScreen.m_OnDeleteMultiplayerChosenSprites != null)
			{
				MenuScreen.m_OnDeleteMultiplayerChosenSprites();
			}
			SetPhase(MenuPhase.Mode);
			break;
		case MenuPhase.Second:
			SetPhase(MenuPhase.First);
			MenuScreen.m_OnDeleteMultiplayerChosenSprites?.Invoke();
			break;
		}
	}

	private void SettingsButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Settings);
	}

	private void AchievementButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Achievements);
	}

	private void StatisticsManager_OnCoinsChanged(int coinsCount = 0)
	{
		tk2dTextMesh[] array = coinsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
		}
		if (coinsCount != 0)
		{
			tk2dTextMesh[] array2 = coinsLabels;
			foreach (tk2dTextMesh tk2dTextMesh3 in array2)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAchievementAddCoins));
				gameObject.transform.parent = tk2dTextMesh3.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}
			if (isCharactersInitialized && MenuScreen.m_OnShouldBuyCharacterOnCoinsTutorialLoad != null)
			{
				MenuScreen.m_OnShouldBuyCharacterOnCoinsTutorialLoad(true);
			}
		}
	}

	private void StatisticsManager_OnGemsChanged(int gemsCount = 0)
	{
		tk2dTextMesh[] array = gemsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Gems.ToString();
		}
		if (gemsCount != 0)
		{
			tk2dTextMesh[] array2 = gemsLabels;
			foreach (tk2dTextMesh tk2dTextMesh3 in array2)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAddGems));
				gameObject.transform.parent = tk2dTextMesh3.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}
			if (isCharactersInitialized && MenuScreen.m_OnShouldBuyCharacterOnGemsTutorialLoad != null)
			{
				MenuScreen.m_OnShouldBuyCharacterOnGemsTutorialLoad(true);
			}
		}
	}

	private void ComputerButton_OnClick()
	{
		MenuScreen.m_OnPlayButtonEnable?.Invoke(true);
		SetMode(GameType.VSComputer);
		if (base.CachedManagers.CachedStatisticsManager.IsUnlockedMode(GameType.VSComputer))
		{
			SetPhase(MenuPhase.First);
		}
	}

	private void HumanButton_OnClick()
	{
		MenuScreen.m_OnPlayButtonEnable?.Invoke(true);
		SetMode(GameType.VSHuman);
		base.CachedManagers.CachedGUIScreenManager.IsNeedPlayUnlockVSHumanModeEffect = false;
		if (base.CachedManagers.CachedStatisticsManager.IsUnlockedMode(GameType.VSHuman))
		{
			SetPhase(MenuPhase.First);
		}
	}

	private void BirdsHuntButton_OnClick()
	{
		MenuScreen.m_OnPlayButtonEnable?.Invoke(true);
		SetMode(GameType.BirdsHunt);
		base.CachedManagers.CachedGUIScreenManager.IsNeedPlayUnlockBirdHuntModeEffect = false;
		if (base.CachedManagers.CachedStatisticsManager.IsUnlockedMode(GameType.BirdsHunt))
		{
			SetPhase(MenuPhase.First);
		}
	}

	private void VSHumanLockedButton_OnClick()
	{
		StartLockedButtonTweener(vsHumanLockedTweener);
	}

	private void BirdsHuntLockedButton_OnClick()
	{
		StartLockedButtonTweener(birdsHuntLockedTweener);
	}

	private void AppleShootButton_OnClick()
	{
		MenuScreen.m_OnPlayButtonEnable?.Invoke(true);
		SetMode(GameType.AppleShoot);
		base.CachedManagers.CachedGUIScreenManager.IsNeedPlayUnlockAppleModeEffect = false;
		if (base.CachedManagers.CachedStatisticsManager.IsUnlockedMode(GameType.AppleShoot))
		{
			SetPhase(MenuPhase.First);
		}
	}

	private void AppleShootLockedButton_OnClick()
	{
		StartLockedButtonTweener(appleShootLockedTweener);
	}

	private void TournamentButton_OnClick()
	{
		SetMode(GameType.Tournament);
		base.CachedManagers.CachedGUIScreenManager.IsNeedPlayUnlockTournamentModeEffect = false;
		if (base.CachedManagers.CachedStatisticsManager.IsUnlockedMode(GameType.Tournament))
		{
			if (base.CachedManagers.CachedTournamentManager.LoadTournament())
			{
				GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.ResumeTournament);
				gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(ResumePopup_OnPopupHide));
			}
			else
			{
				SetPhase(MenuPhase.Tournament);
			}
		}
	}

	private void MultiplayerButton_OnClick()
	{
		MenuScreen.m_OnPlayButtonEnable?.Invoke(true);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
			return;
		}
		if (LLMultiplayerManager.IsCurrentGameVersionNotTheLatest)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MultiplayerBuildVersion, true, null, true);
			return;
		}
		SetMode(GameType.Multiplayer);
		base.CachedManagers.CachedGUIScreenManager.IsNeedPlayUnlockMultiplayerModeEffect = false;
		if (base.CachedManagers.CachedStatisticsManager.IsUnlockedMode(GameType.Multiplayer))
		{
			SetPhase(MenuPhase.First);
		}
	}

	private void TournamentLockedButton_OnClick()
	{
		StartLockedButtonTweener(tournamentLockedTweener);
	}

	private void MultiplayerLockedButton_OnClick()
	{
		StartLockedButtonTweener(multiplayerLockedTweener);
	}

	private void TournamentEasyButton_OnClick()
	{
		currentTournamentDifficult = TournamentDifficultType.Easy;
		MenuScreen.m_OnCurrentTournamentDifficult?.Invoke(currentTournamentDifficult);
		SetPhase(MenuPhase.First);
	}

	private void TournamentMediumButton_OnClick()
	{
		currentTournamentDifficult = TournamentDifficultType.Medium;
		MenuScreen.m_OnCurrentTournamentDifficult?.Invoke(currentTournamentDifficult);
		SetPhase(MenuPhase.First);
	}

	private void TournamentHardButton_OnClick()
	{
		currentTournamentDifficult = TournamentDifficultType.Hard;
		MenuScreen.m_OnCurrentTournamentDifficult?.Invoke(currentTournamentDifficult);
		SetPhase(MenuPhase.First);
	}

	private void Proposal_OnProposalClick(GUIProposalMenu sender)
	{
		GUIProposalMenu[] array = proposalsMenu;
		foreach (GUIProposalMenu gUIProposalMenu in array)
		{
			if (gUIProposalMenu.TypeProposal != ProposalMenuType.FreeChest && sender.GetType() != typeof(FreeChestProposalMenu) && gUIProposalMenu.TypeProposal != ProposalMenuType.Gift && sender.GetType() != typeof(DailyGiftProposalMenu))
			{
				gUIProposalMenu.Deactivate();
			}
		}
		base.CachedManagers.CachedProposalManager.GetProposalMenu(sender);
	}

	private void GiftManager_OnGiftAvailable()
	{
		base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
	}

	private void GiftManager_OnSpecialGiftCharacterOpen(ShopItemCharacter character)
	{
		MenuScreen.m_OnInitializeCharacters?.Invoke();
	}

	private void MicroachievementsManager_OnReachedAchievement(Queue<MicroachievementInfo> queue)
	{
		if (!controlAchievement.IsShown && !controlAchievement.IsAnimating)
		{
			ShowAchievement(queue);
		}
	}

	private void ResumePopup_OnPopupHide(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(ResumePopup_OnPopupHide));
		ResumeTournamentPopup resumeTournamentPopup = popup as ResumeTournamentPopup;
		if (resumeTournamentPopup.ResumeType == TournamentResumeType.Resume)
		{
			base.CachedManagers.CachedStatisticsManager.ResetStatistics();
			TournamentManager cachedTournamentManager = base.CachedManagers.CachedTournamentManager;
			base.ObjectsSpawner.SetFirstSelectedCharacter(cachedTournamentManager.PlayerCharacter);
			base.ObjectsSpawner.SetSecondSelectedCharacter(cachedTournamentManager.EnemyCharacter);
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Versus);
		}
		else if (resumeTournamentPopup.ResumeType == TournamentResumeType.New)
		{
			base.CachedManagers.CachedTournamentManager.ResetTournament();
			SetPhase(MenuPhase.Tournament);
		}
	}

	private void TapToStartScreen_OnLoaderRemoved()
	{
		base.CachedManagers.CachedProposalManager.ShowInterstitialCrosspromo();
	}

	private void MCInnAppsmanager_ReceivedProductsIDS(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		MenuScreen.m_OnUpdateBuyButtons.Invoke();
		MenuScreen.m_OnCheckBoughtCharacters?.Invoke();
	}

	private void ShopManager_OnAllProductsReceived()
	{
		MenuScreen.m_OnUpdateBuyButtons?.Invoke();
	}

	private void ICloudManager_OnProgressDownloaded()
	{
		UpdateMainMenuScreen(true);
	}

	private void MultiplayerManager_OnPlayerDisconnected(bool isLocalPlayer)
	{
		MenuScreen.m_OnMultiplayerInvitationReceived?.Invoke(false);
		MenuScreen.m_OnPlayButtonEnable?.Invoke(true);
	}

	private void LLMultiplayerManager_OnMatchmakerCancelled()
	{
		MenuScreen.m_OnStartSpecialIdleAnimations?.Invoke(true, true);
		MenuScreen.m_OnPlayButtonEnable?.Invoke(true);
		MenuScreen.m_OnDeleteMultiplayerChosenSprites?.Invoke();
	}

	private void LLMultiplayerManager_OnAllOpponentsFound()
	{
		MenuScreen.m_OnPlayButtonEnable?.Invoke(false);
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null && MenuScreen.m_OnPlayButtonEnable != null)
			{
				MenuScreen.m_OnPlayButtonEnable(true);
			}
		}, 5f);
	}

	private void FollowUsButton_OnClick()
	{
		if (!isFollowUsShown)
		{
			FollowButtonAction(CachedAnimationFollowUsShow, CachedAnimationFollowUsShown);
		}
		else
		{
			FollowButtonAction(CachedAnimationFollowUsHide, CachedAnimationFollowUsHidden);
		}
		isFollowUsShown = !isFollowUsShown;
	}

	private void FacebookButton_OnClick()
	{
		Application.OpenURL(facebookLink);
	}

	private void TwitterButton_OnClick()
	{
		Application.OpenURL(twitterLink);
	}

	private void InstagramButton_OnClick()
	{
		Application.OpenURL(instagramLink);
	}

	private void YoutubeButton_OnClick()
	{
		Application.OpenURL(youtubeLink);
	}

	private void RemoveAdsPopup_OnRemoveAdsPurchased()
	{
		RemoveAdsBannerEnabled();
	}

	private void RemoveAdsBannerButton_OnClick()
	{
		base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.RemoveAdsPopup);
	}

	private void BankButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Shop);
		base.CachedManagers.CachedGUIScreenManager.BeforeShopScreenType = GUIScreenType.Main;
	}

	private void GUIScreenManager_OnFontsChanged()
	{
		if (currentPhase == MenuPhase.First)
		{
			AudioManager.StopAudioWithKey("player_two_choose");
			AudioManager.PlayAudioWithKey("player_one_choose");
		}
		else if (currentPhase == MenuPhase.Second)
		{
			AudioManager.StopAudioWithKey("player_one_choose");
			AudioManager.PlayAudioWithKey("player_two_choose");
		}
		MenuScreen.m_OnUpdateBuyButtons?.Invoke();
	}

	private void Timer_OnTimerChanged(int time)
	{
		if (time == 0)
		{
			UpdateBankAlertIcons();
		}
	}

	private void SuperGiftPopup_OnAllRewardsReceived()
	{
		GUIProposalMenu[] array = proposalsMenuSuperGift;
		foreach (GUIProposalMenu gUIProposalMenu in array)
		{
			gUIProposalMenu.Deactivate();
		}
		base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
	}

	private void FreeChestPopup_OnVideoWatched(bool isProposalPopup)
	{
		if (isProposalPopup)
		{
			GUIProposalMenu[] array = proposalsMenu;
			foreach (GUIProposalMenu gUIProposalMenu in array)
			{
				if (gUIProposalMenu.GetType() == typeof(FreeChestProposalMenu))
				{
					(gUIProposalMenu as FreeChestProposalMenu).ChangeTimerActiveState(false);
					(gUIProposalMenu as FreeChestProposalMenu).UpdateMainButton(false);
					(gUIProposalMenu as FreeChestProposalMenu).GetAward();
				}
			}
		}
		else
		{
			MenuScreenScrollView.CharacterRouletteOpen();
			HideMenuAndShowRouletteScreen();
		}
	}

	private IEnumerator CoroutineShowAchievement(MicroachievementInfo info)
	{
		controlAchievement.Show();
		yield return new WaitForSeconds(timeAchievementShowing);
		controlAchievement.ShowImmediately();
		Queue<MicroachievementInfo> achievementsQueue = base.CachedManagers.CachedMicroachievementsManager.QueueAhievements;
		if (info.coinsAward > 0)
		{
			yield return new WaitForEndOfFrame();
			GUILayoutCell[] array = countCellsAchievement;
			foreach (GUILayoutCell gUILayoutCell in array)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAchievementSpawnCoins));
				gameObject.transform.parent = gUILayoutCell.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}
			yield return new WaitForSeconds(delayAddEffect);
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.ACHIEVEMENT, GoliathIds.CurrencyName.COINS, info.coinsAward, (currentPhase != MenuPhase.Mode) ? GoliathIds.Placement.CHARACTER_SELECTION : GoliathIds.Placement.MAIN_MENU);
			base.CachedManagers.CachedStatisticsManager.AddCoins(info.coinsAward);
			if (achievementsQueue.Count > 0)
			{
				achievementsQueue.Dequeue();
			}
			yield return new WaitForEndOfFrame();
			tk2dTextMesh[] array2 = coinsLabels;
			foreach (tk2dTextMesh tk2dTextMesh2 in array2)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAchievementAddCoins));
				gameObject2.transform.parent = tk2dTextMesh2.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject2);
			}
			yield return new WaitForSeconds(timeAchievementShow - delayAddEffect);
		}
		else
		{
			yield return new WaitForSeconds(timeAchievementShow);
		}
		if (info.gemsAward > 0)
		{
			base.CachedManagers.CachedStatisticsManager.AddGems(info.gemsAward);
		}
		controlAchievement.Hide();
		yield return new WaitForSeconds(timeAchievementHidding);
		controlAchievement.HideImmediately();
		ShowAchievement(achievementsQueue);
	}

	private IEnumerator EnableRotation()
	{
		yield return new WaitForEndOfFrame();
		Screen.orientation = ScreenOrientation.Unknown;
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
		if (GameManager.IsIPhoneX)
		{
			Screen.autorotateToPortraitUpsideDown = false;
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(500f, 200f, 100f, 100f), "Initialize"))
		{
			UpdateMainMenuScreen(true);
		}
		if (GUI.Button(new Rect(600f, 400f, 150f, 100f), "Show popup 1"))
		{
			SingletonMonoBehaviour<GDPRManager>.Instance.ShowIntroTargetedAdsPopup();
		}
		if (GUI.Button(new Rect(600f, 500f, 150f, 100f), "Show popup 2"))
		{
			SingletonMonoBehaviour<GDPRManager>.Instance.ShowSettingsTargetedAdsPopup();
		}
		if (GUI.Button(new Rect(600f, 600f, 150f, 100f), "Show popup 3"))
		{
			SingletonMonoBehaviour<GDPRManager>.Instance.ShowPrivacyPolicyPopup();
		}
		if (GUI.Button(new Rect(600f, 700f, 150f, 100f), "Remove Consents"))
		{
			GDPR.ForgetUser();
		}
		if (GUI.Button(new Rect(600f, 800f, 150f, 100f), "TA: Relevant"))
		{
			SingletonMonoBehaviour<GDPRManager>.Instance.IsRelevant = !SingletonMonoBehaviour<GDPRManager>.Instance.IsRelevant;
		}
		if (GUI.Button(new Rect(600f, 900f, 150f, 100f), "Show Sub Popup 1"))
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.AutoRenewableSubscribeFoolish);
		}
		if (GUI.Button(new Rect(600f, 1000f, 150f, 100f), "Show Sub Popup 2"))
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.AutoRenewableSubscribe);
		}
		if (GUI.Button(new Rect(600f, 1100f, 150f, 100f), "Request Video"))
		{
			Debug.Log("VideoAds::pressButton");
			MCMoPubManager.Instance.RequestRewardedVideoAd();
		}
		if (GUI.Button(new Rect(600f, 1200f, 150f, 100f), "Show Video"))
		{
			MCMoPubManager.Instance.ShowRewardedVideoAd();
		}
	}

	public override void BackButtonPressed()
	{
		BackButton_OnClick();
	}
}
