using System;
using System.Collections;
using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class ResultScreen : GUIScreen
{
	private enum ButtonsAppearState
	{
		None = 0,
		KeyButton = 1,
		NextButton = 2
	}

	private const string FLAG_RECORD_LABEL = "ui.result.flag.record";

	private const string FLAG_USUAL_RECORD_LABEL = "ui.result.flag.usual.best";

	private const string FLAG_USUAL_CURRENT_LABEL = "ui.result.flag.usual.current";

	private const string EPIC_WIN = "ui.result.flag.epicwin";

	private const string EPIC_LOSE = "ui.result.flag.epiclose";

	private const string WIN_PLAYER1 = "ui.result.flag.player1win";

	private const string WIN_PLAYER2 = "ui.result.flag.player2win";

	private const string TOURNAMENT_WIN = "ui.result.flag.tournamentwin";

	private const string BUTTON_RATEUS = "ui.result.key.button_rate";

	private const string BUTTON_NOADS = "ui.proposal.menu.remove.ads.description";

	private const string BUTTON_FREECOINS = "ui.result.key.button_coins";

	private const string BUTTON_FREE_LOCK = "ui.result.key.button.free.lock";

	private const string BUTTON_CHEST = "ui.result.key.button_chest";

	private const string CUP_EASY = "Cup_Easy";

	private const string CUP_EASY_SOCKET = "Cup_Easy_Socket";

	private const string CUP_MEDIUM = "Cup_Normal";

	private const string CUP_MEDIUM_SOCKET = "Cup_Normal_Socket";

	private const string CUP_HARD = "Cup_Legendary";

	private const string CUP_HARD_SOCKET = "Cup_Legendary_Socket";

	private const string COINS_REWARD_PREFIX = "+";

	private const string SAVE_FOR_GEMS_TEXT = "ui.main.tournament_mode.save.for.gems.label";

	[SerializeField]
	private tk2dUIItem[] gameCenterButtons;

	[SerializeField]
	private tk2dUIItem[] nextButtons;

	[SerializeField]
	private tk2dUIItem[] keyButtons;

	[SerializeField]
	private GameObject[] tournamentLayouter;

	[SerializeField]
	private GameObject[] mainLayouter;

	[SerializeField]
	private GameObject[] tournamentEndLayouter;

	[SerializeField]
	private GameObject[] tournamentBattleWinLayouter;

	[SerializeField]
	private GameObject[] tournamentLoseLayouter;

	[SerializeField]
	private tk2dBaseSprite[] cupSprite;

	[SerializeField]
	private float cupspriteWinScale = 1.2f;

	[SerializeField]
	private float cupSpriteWinOffset = -18f;

	[SerializeField]
	private TweenPosition[] cupTweeners;

	[SerializeField]
	private GUIControl controlFade;

	[SerializeField]
	private float easyTournamentEndDelay = 2f;

	[SerializeField]
	private float mediumTournamentEndDelay = 2f;

	[SerializeField]
	private float hardTournamentEndDelay = 2f;

	[SerializeField]
	private tk2dUIItem[] claimRewardButton;

	[SerializeField]
	private tk2dUIItem[] nextBattleButton;

	[SerializeField]
	private tk2dUIItem[] exitButton;

	[SerializeField]
	private tk2dUIItem[] saveForVideoButton;

	[SerializeField]
	private tk2dUIItem[] saveForGemsButton;

	[SerializeField]
	private tk2dTextMesh[] saveForGemsLabel;

	[SerializeField]
	private tk2dUIItem[] leaveTournamentButton;

	[SerializeField]
	private GameObject[] recordFlags;

	[SerializeField]
	private tk2dTextMesh[] recordFlagLabels;

	[SerializeField]
	private GameObject[] usualFlags;

	[SerializeField]
	private tk2dTextMesh[] usualFlagBestLabels;

	[SerializeField]
	private tk2dTextMesh[] usualFlagCurrentLabels;

	[SerializeField]
	private tk2dBaseSprite[] flagSprites;

	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private tk2dTextMesh[] gemsLabels;

	[SerializeField]
	private GUILayoutCell[] avatarCells;

	[SerializeField]
	private SkeletonAnimation characterAvatarAnimation;

	[SerializeField]
	private GUIChest[] chests;

	[SerializeField]
	private tk2dTextMesh[] keyDescriptionLabels;

	[SerializeField]
	private SkeletonAnimation[] buttonAnimations;

	[SerializeField]
	private string slotIconAnimation;

	[SerializeField]
	private string attachmentPlayAnimation;

	[SerializeField]
	private string attachmentRateAnimation;

	[SerializeField]
	private string slotKeyAnimation;

	[SerializeField]
	private string attachmentKeyAnimation;

	[SerializeField]
	private string attachmentStarAnimation;

	[SerializeField]
	private GameObject avatarBackground;

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
	private tk2dTextMesh[] coinsRewardLabel;

	[SerializeField]
	private GameObject[] coinsRewardRootCell;

	[SerializeField]
	private float keyButtonTweenerDelay;

	[SerializeField]
	private TweenScale[] keyButtonTweener;

	[SerializeField]
	private float keyButtonTweenerDuration;

	[SerializeField]
	private AnimationCurve keyButtonTweenerCurve;

	[SerializeField]
	private float nextButtonTweenerDelay;

	[SerializeField]
	private TweenScale[] nextButtonTweener;

	[SerializeField]
	private float nextButtonTweenerDuration;

	[SerializeField]
	private AnimationCurve nextButtonTweenerCurve;

	[SerializeField]
	private float forcedMenuScreenDelay = 2f;

	private const float chestAndButtonsDelay = 2f;

	private bool chestDelayFinished;

	[SerializeField]
	private tk2dSprite[] backSprites;

	[SerializeField]
	private TweenPosition[] landscapeTweens;

	[SerializeField]
	private TweenPosition landscapeInfoTween;

	[SerializeField]
	private TweenPosition[] portraitTweens;

	[SerializeField]
	private TweenPosition portraitInfoTween;

	[SerializeField]
	private GUILayoutCell landscapeCharacterPreviewCell;

	[SerializeField]
	private GUILayoutCell landscapeBorderCell;

	[SerializeField]
	private GUILayoutCell portraitCharacterPreviewCell;

	[SerializeField]
	private GUILayoutCell portraitBorderCell;

	[Header("iPhone X")]
	[SerializeField]
	private GUILayoutCell leftButtonsCell;

	[SerializeField]
	private GUILayoutCell[] rightButtonsCells;

	[SerializeField]
	private float rightButtonsIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] topBarCells;

	[SerializeField]
	private float topBarIPhoneXSize;

	private GUICharacter avatar;

	private bool firstWinner;

	private List<GameObject> currentEffects = new List<GameObject>();

	private ObjectsSpawner cachedSpawner;

	private ButtonsAppearState buttonsState;

	private bool needForceMenuScreen;

	private Dictionary<GUILayoutCell, SkeletonAnimation> animationAvatar = new Dictionary<GUILayoutCell, SkeletonAnimation>();

	private bool isTournamentProgressSaved;

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

	private ButtonsAppearState ButtonsState
	{
		get
		{
			return buttonsState;
		}
		set
		{
			if (buttonsState == value)
			{
				return;
			}
			buttonsState = value;
			switch (buttonsState)
			{
			case ButtonsAppearState.KeyButton:
				base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						for (int j = 0; j < keyButtonTweener.Length; j++)
						{
							TweenScale tweenScale = keyButtonTweener[j];
							tweenScale.duration = keyButtonTweenerDuration;
							tweenScale.animationCurve = keyButtonTweenerCurve;
							tweenScale.SetOnFinishedDelegate(delegate
							{
								tk2dUIItem[] array2 = keyButtons;
								foreach (tk2dUIItem tk2dUIItem3 in array2)
								{
									tk2dUIItem3.enabled = true;
								}
								if (base.CachedManagers.CachedTutorialManager.IsFreeChestsEnded)
								{
									ButtonsState = ButtonsAppearState.NextButton;
								}
							});
							tweenScale.enabled = true;
						}
					}
				}, keyButtonTweenerDelay);
				break;
			case ButtonsAppearState.NextButton:
			{
				tk2dUIItem[] array = nextButtons;
				foreach (tk2dUIItem tk2dUIItem2 in array)
				{
					tk2dUIItem2.enabled = false;
				}
				base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						for (int j = 0; j < nextButtonTweener.Length; j++)
						{
							TweenScale tweenScale = nextButtonTweener[j];
							tweenScale.duration = nextButtonTweenerDuration;
							tweenScale.animationCurve = nextButtonTweenerCurve;
							tweenScale.SetOnFinishedDelegate(delegate
							{
								tk2dUIItem[] array2 = nextButtons;
								foreach (tk2dUIItem tk2dUIItem3 in array2)
								{
									tk2dUIItem3.enabled = true;
								}
							});
							tweenScale.enabled = true;
						}
					}
				}, nextButtonTweenerDelay);
				break;
			}
			}
		}
	}

	private void Awake()
	{
		tk2dSpriteCollection component = MiniclipPrefabs.Instance.spriteCollection.GetComponent<tk2dSpriteCollection>();
		tk2dSpriteCollectionData spriteCollection = component.spriteCollection;
		tk2dUIItem[] array = gameCenterButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dSprite componentInChildren = tk2dUIItem2.GetComponentInChildren<tk2dSprite>();
			componentInChildren.SetSprite(spriteCollection, "games_controller_white");
		}
		GameObject[] array2 = coinsRewardRootCell;
		foreach (GameObject gameObject in array2)
		{
			tk2dTextMesh[] componentsInChildren = gameObject.GetComponentsInChildren<tk2dTextMesh>();
			foreach (tk2dTextMesh tk2dTextMesh2 in componentsInChildren)
			{
				if (tk2dTextMesh2.text.Equals("ui.result.coins.reward"))
				{
					tk2dTextMesh2.text = "ui.result.coins.reward.android";
					break;
				}
			}
		}
		tk2dUIItem[] array3 = nextButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array3)
		{
			tk2dTextMesh[] componentsInChildren2 = tk2dUIItem3.GetComponentsInChildren<tk2dTextMesh>();
			foreach (tk2dTextMesh tk2dTextMesh3 in componentsInChildren2)
			{
				if (tk2dTextMesh3.text.Equals("ui.result.button_play"))
				{
					tk2dTextMesh3.text = "ui.result.button_play.android";
					break;
				}
			}
		}
		nextButtonTweenerDelay = 0f;
		base.CachedManagers.CachedGameManager.IsGamePaused = false;
		GUILayoutCell[] array4 = avatarCells;
		foreach (GUILayoutCell gUILayoutCell in array4)
		{
			if (avatarBackground != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(avatarBackground);
				gameObject2.transform.parent = gUILayoutCell.transform;
				gameObject2.transform.localPosition = avatarBackground.transform.localPosition;
			}
		}
		if (tk2dSystem.IsRetina)
		{
			cupSpriteWinOffset *= 2f;
		}
		base.CachedManagers.CachedProposalManager.GenerateProposalResult();
		base.CachedManagers.CachedResourceManager.UnloadGeneralSpriteCollections(0.1f);
		if (GameManager.IsIPhoneX)
		{
			leftButtonsCell.SizeValue = 0f;
			for (int num = 0; num < rightButtonsCells.Length; num++)
			{
				rightButtonsCells[num].SizeValue = rightButtonsIPhoneXOffset;
			}
			for (int num2 = 0; num2 < topBarCells.Length; num2++)
			{
				topBarCells[num2].SizeValue = topBarIPhoneXSize;
			}
		}
	}

	private void OnEnable()
	{
		StartCoroutine(EnableRotation());
		tk2dUIItem[] array = gameCenterButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += GameCenterButton_OnClick;
		}
		tk2dUIItem[] array2 = nextButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick += NextButton_OnClick;
		}
		tk2dUIItem[] array3 = keyButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick += KeyButton_OnClick;
		}
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnGemsChanged;
		GUIChest[] array4 = chests;
		foreach (GUIChest gUIChest in array4)
		{
			gUIChest.OnOpenLock = (Action)Delegate.Combine(gUIChest.OnOpenLock, new Action(Chest_OnOpenLock));
			gUIChest.OnSpawnEnd = (Action)Delegate.Combine(gUIChest.OnSpawnEnd, new Action(Chest_OnSpawnEnd));
		}
		MicroachievementsManager.OnReachedAchievement += MicroachievementsManager_OnReachedAchievement;
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		controlAchievement.HideImmediately();
		Queue<MicroachievementInfo> queueAhievements = base.CachedManagers.CachedMicroachievementsManager.QueueAhievements;
		if (queueAhievements.Count != 0)
		{
			MicroachievementsManager_OnReachedAchievement(queueAhievements);
		}
		bool flag = base.CachedManagers.CachedGameManager.CurrentGameType == GameType.Tournament;
		GameObject[] array5 = tournamentLayouter;
		foreach (GameObject gameObject in array5)
		{
			gameObject.SetActive(flag);
		}
		GameObject[] array6 = mainLayouter;
		foreach (GameObject gameObject2 in array6)
		{
			gameObject2.SetActive(!flag);
		}
		tk2dUIItem[] array7 = claimRewardButton;
		foreach (tk2dUIItem tk2dUIItem5 in array7)
		{
			tk2dUIItem5.OnClick += ClaimReward_OnClick;
		}
		tk2dUIItem[] array8 = nextBattleButton;
		foreach (tk2dUIItem tk2dUIItem6 in array8)
		{
			tk2dUIItem6.OnClick += NextBattle_OnClick;
		}
		tk2dUIItem[] array9 = exitButton;
		foreach (tk2dUIItem tk2dUIItem7 in array9)
		{
			tk2dUIItem7.OnClick += ExitButton_OnClick;
		}
		tk2dUIItem[] array10 = saveForVideoButton;
		foreach (tk2dUIItem tk2dUIItem8 in array10)
		{
			tk2dUIItem8.OnClick += SaveForVideoButton_OnClick;
		}
		tk2dUIItem[] array11 = saveForGemsButton;
		foreach (tk2dUIItem tk2dUIItem9 in array11)
		{
			tk2dUIItem9.OnClick += SaveForGemsButton_OnClick;
		}
		tk2dUIItem[] array12 = leaveTournamentButton;
		foreach (tk2dUIItem tk2dUIItem10 in array12)
		{
			tk2dUIItem10.OnClick += LeaveTournament_OnClick;
		}
		if (characterAvatarAnimation != null)
		{
			GUILayoutCell[] array13 = avatarCells;
			foreach (GUILayoutCell gUILayoutCell in array13)
			{
				SkeletonAnimation skeletonAnimation = UnityEngine.Object.Instantiate(characterAvatarAnimation);
				skeletonAnimation.transform.parent = gUILayoutCell.transform;
				skeletonAnimation.transform.localPosition = Vector3.zero;
				AnimationAvatar.Add(gUILayoutCell, skeletonAnimation);
			}
		}
		tk2dSprite[] array14 = backSprites;
		foreach (tk2dSprite tk2dSprite2 in array14)
		{
			tk2dSprite2.gameObject.SetActive(false);
		}
		ProposalManager.OnRemoveAdsPurchased = (Action)Delegate.Combine(ProposalManager.OnRemoveAdsPurchased, new Action(ProposalManager_OnRemoveAdsPurchased));
		ProposalManager.OnRemoveAdsPurchaseFailed = (Action)Delegate.Combine(ProposalManager.OnRemoveAdsPurchaseFailed, new Action(ProposalManager_OnRemoveAdsPurchaseFailed));
		for (int num9 = 0; num9 < saveForVideoButton.Length; num9++)
		{
			saveForVideoButton[num9].gameObject.SetActive(!base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
		}
		for (int num10 = 0; num10 < saveForGemsButton.Length; num10++)
		{
			saveForGemsButton[num10].gameObject.SetActive(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
		}
		for (int num11 = 0; num11 < saveForGemsLabel.Length; num11++)
		{
			saveForGemsLabel[num11].text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.main.tournament_mode.save.for.gems.label") + base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice;
		}
	}

	private void OnDisable()
	{
		tk2dUIItem[] array = gameCenterButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= GameCenterButton_OnClick;
		}
		tk2dUIItem[] array2 = nextButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick -= NextButton_OnClick;
		}
		tk2dUIItem[] array3 = keyButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick -= KeyButton_OnClick;
		}
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnGemsChanged;
		GUIChest[] array4 = chests;
		foreach (GUIChest gUIChest in array4)
		{
			gUIChest.OnOpenLock = (Action)Delegate.Remove(gUIChest.OnOpenLock, new Action(Chest_OnOpenLock));
			gUIChest.OnSpawnEnd = (Action)Delegate.Remove(gUIChest.OnSpawnEnd, new Action(Chest_OnSpawnEnd));
		}
		Scheduler instanceIfExist = SingletonMonoBehaviour<Scheduler>.InstanceIfExist;
		if (instanceIfExist != null)
		{
			instanceIfExist.UnscheduleMethod(avatar, avatar.schedulAnimation);
		}
		MicroachievementsManager.OnReachedAchievement -= MicroachievementsManager_OnReachedAchievement;
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		controlAchievement.HideImmediately();
		tk2dUIItem[] array5 = claimRewardButton;
		foreach (tk2dUIItem tk2dUIItem5 in array5)
		{
			tk2dUIItem5.OnClick -= ClaimReward_OnClick;
		}
		tk2dUIItem[] array6 = nextBattleButton;
		foreach (tk2dUIItem tk2dUIItem6 in array6)
		{
			tk2dUIItem6.OnClick -= NextBattle_OnClick;
		}
		tk2dUIItem[] array7 = exitButton;
		foreach (tk2dUIItem tk2dUIItem7 in array7)
		{
			tk2dUIItem7.OnClick -= ExitButton_OnClick;
		}
		tk2dUIItem[] array8 = saveForVideoButton;
		foreach (tk2dUIItem tk2dUIItem8 in array8)
		{
			tk2dUIItem8.OnClick -= SaveForVideoButton_OnClick;
		}
		tk2dUIItem[] array9 = saveForGemsButton;
		foreach (tk2dUIItem tk2dUIItem9 in array9)
		{
			tk2dUIItem9.OnClick -= SaveForGemsButton_OnClick;
		}
		tk2dUIItem[] array10 = leaveTournamentButton;
		foreach (tk2dUIItem tk2dUIItem10 in array10)
		{
			tk2dUIItem10.OnClick -= LeaveTournament_OnClick;
		}
		List<GUILayoutCell> list = new List<GUILayoutCell>(AnimationAvatar.Keys);
		foreach (GUILayoutCell item in list)
		{
			if (AnimationAvatar[item] != null)
			{
				UnityEngine.Object.Destroy(AnimationAvatar[item].gameObject);
				AnimationAvatar[item] = null;
			}
		}
		AnimationAvatar.Clear();
		ProposalManager.OnRemoveAdsPurchased = (Action)Delegate.Remove(ProposalManager.OnRemoveAdsPurchased, new Action(ProposalManager_OnRemoveAdsPurchased));
		ProposalManager.OnRemoveAdsPurchaseFailed = (Action)Delegate.Remove(ProposalManager.OnRemoveAdsPurchaseFailed, new Action(ProposalManager_OnRemoveAdsPurchaseFailed));
	}

	private void Update()
	{
		GUIChest[] array = chests;
		foreach (GUIChest gUIChest in array)
		{
			if (chestDelayFinished)
			{
				gUIChest.UpdateChest(Time.deltaTime);
			}
		}
		for (int j = 0; j < keyButtonTweener.Length; j++)
		{
			if (!keyButtonTweener[j].gameObject.activeInHierarchy && keyButtonTweener[j].enabled)
			{
				keyButtonTweener[j].Update();
			}
		}
		for (int k = 0; k < nextButtonTweener.Length; k++)
		{
			if (!nextButtonTweener[k].gameObject.activeInHierarchy && nextButtonTweener[k].enabled)
			{
				nextButtonTweener[k].Update();
			}
		}
	}

	public void SetWinner(Character loser)
	{
		firstWinner = true;
		GameType currentGameType = base.CachedManagers.CachedGameManager.CurrentGameType;
		Level currentLevel = base.CachedManagers.CachedLevelsManager.CurrentLevel;
		CharacterNameType characterNameType = base.ObjectsSpawner.FirstSelectedCharacterNameType;
		base.ObjectsSpawner.IsFirstcharacterWin = loser != currentLevel.CharacterFirst;
		switch (currentGameType)
		{
		case GameType.VSHuman:
			firstWinner = loser != currentLevel.CharacterFirst;
			if (loser == currentLevel.CharacterFirst)
			{
				characterNameType = base.ObjectsSpawner.SecondSelectedCharacterNameType;
			}
			base.ObjectsSpawner.CharacterWinnerDoubleActiveType = ((!firstWinner) ? base.ObjectsSpawner.SecondCharacterDoubleType : base.ObjectsSpawner.FirstCharacterDoubleType);
			break;
		case GameType.VSComputer:
		case GameType.Tournament:
			firstWinner = loser != currentLevel.CharacterFirst;
			base.ObjectsSpawner.CharacterWinnerDoubleActiveType = base.ObjectsSpawner.FirstCharacterDoubleType;
			break;
		case GameType.Multiplayer:
		{
			LevelMultiplayer levelMultiplayer = currentLevel as LevelMultiplayer;
			firstWinner = loser != levelMultiplayer.LocalPlayerCharacter;
			base.ObjectsSpawner.CharacterWinnerDoubleActiveType = ((!(levelMultiplayer.LocalPlayerCharacter == levelMultiplayer.CharacterFirst)) ? base.ObjectsSpawner.SecondCharacterDoubleType : base.ObjectsSpawner.FirstCharacterDoubleType);
			break;
		}
		case GameType.BirdsHunt:
		case GameType.AppleShoot:
			base.ObjectsSpawner.CharacterWinnerDoubleActiveType = base.ObjectsSpawner.FirstCharacterDoubleType;
			break;
		}
		GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
		gUICharacter.SetMultipleCharacter(base.ObjectsSpawner.CharacterWinnerDoubleActiveType);
		if ((bool)gUICharacter)
		{
			gUICharacter.ClearAll();
			gUICharacter.AddedAvatar(true, characterAvatarAnimation);
			avatar = gUICharacter;
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		controlAchievement.HideImmediately();
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			AudioManager.StopAudioWithKey("crowd_loop", 2f);
		}, 5f);
		GameType type = base.CachedManagers.CachedGameManager.CurrentGameType;
		Level currentLevel = base.CachedManagers.CachedLevelsManager.CurrentLevel;
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		SetWinner(currentLevel.LostCharacter);
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
		Action<GUIControl> onFinished2 = delegate(GUIControl cont)
		{
			if (onFinished != null)
			{
				onFinished(cont);
			}
			if (type != GameType.Tournament)
			{
				GUIChest[] array6 = chests;
				foreach (GUIChest gUIChest in array6)
				{
					GUIChestType chestType = GUIChestType.OneLock;
					if (base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive && base.CachedManagers.CachedTutorialManager.IsFreeChestsEnded)
					{
						chestType = GUIChestType.ThreeLocks;
					}
					gUIChest.ChestType = chestType;
					gUIChest.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
				}
				if (type == GameType.VSComputer && base.CachedManagers.CachedProposalManager.IsBuyCharacterAvalaible && firstWinner)
				{
					GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.BuyCharacter);
					gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(HidePopup));
				}
				else if (base.CachedManagers.CachedProposalManager.BuyCharacter != null)
				{
					base.CachedManagers.CachedProposalManager.ResetBuyCharacter();
				}
			}
			if (base.CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
			{
				SingletonMonoBehaviour<AdvertisingManager>.Instance.ShowInterstitial();
			}
		};
		switch (type)
		{
		default:
			if (type != GameType.Tournament && type != GameType.Multiplayer)
			{
				break;
			}
			goto case GameType.VSComputer;
		case GameType.BirdsHunt:
		case GameType.AppleShoot:
		{
			bool flag = cachedStatisticsManager.Score >= ((type != GameType.AppleShoot) ? cachedStatisticsManager.HighScoreBirdsKilled : cachedStatisticsManager.HighScoreFruitsSmashed);
			avatar.StartAnimation((!flag) ? avatar.AnimationIdle : avatar.AnimationWin);
			break;
		}
		case GameType.VSHuman:
		{
			avatar.StartAnimationOnResultScreen(true);
			for (int num = 0; num < coinsRewardRootCell.Length; num++)
			{
				coinsRewardRootCell[num].SetActive(false);
			}
			break;
		}
		case GameType.VSComputer:
			avatar.StartAnimationOnResultScreen(firstWinner);
			break;
		}
		string text = null;
		switch (type)
		{
		default:
		{
			if (type != GameType.Tournament)
			{
				if (type == GameType.Multiplayer)
				{
					text = "Level_Multiplayer_Finished";
					if (firstWinner)
					{
						CharacterNameType characterNameType = ((!currentLevel.IsMultiplayerSecondaryPlayer) ? base.ObjectsSpawner.FirstSelectedCharacterNameType : base.ObjectsSpawner.SecondSelectedCharacterNameType);
						ShopItemCharacter shopItemCharacter = base.ObjectsSpawner.GetShopItemCharacter(characterNameType);
						Analytics.CustomEvent("Character_Multiplayer_Win", new Dictionary<string, object>
						{
							{
								"CharacterName",
								shopItemCharacter.GetAnalyticsKey()
							},
							{
								"GameTimeMinutes",
								cachedStatisticsManager.TotalGameTime.Minutes
							}
						});
					}
				}
				break;
			}
			TournamentDifficultType difficultType = base.CachedManagers.CachedTournamentManager.CurrentTournament.settings.difficultType;
			if (!firstWinner)
			{
				switch (difficultType)
				{
				case TournamentDifficultType.Easy:
					text = "Level_Tournament_Lose_Easy";
					break;
				case TournamentDifficultType.Medium:
					text = "Level_Tournament_Lose_Medium";
					break;
				case TournamentDifficultType.Hard:
					text = "Level_Tournament_Lose_Hard";
					break;
				}
			}
			else if (firstWinner && base.CachedManagers.CachedTournamentManager.IsTournamentSuccessfullyEnded)
			{
				switch (difficultType)
				{
				case TournamentDifficultType.Easy:
					text = "Level_Tournament_Finished_Easy";
					break;
				case TournamentDifficultType.Medium:
					text = "Level_Tournament_Finished_Medium";
					break;
				case TournamentDifficultType.Hard:
					text = "Level_Tournament_Finished_Hard";
					break;
				}
			}
			break;
		}
		case GameType.AppleShoot:
			text = "Level_AppleShoot_Finished";
			break;
		case GameType.BirdsHunt:
			text = "Level_BirdsHunt_Finished";
			break;
		case GameType.VSComputer:
			if (firstWinner)
			{
				Analytics.CustomEvent("Level_VSComputer_Win", new Dictionary<string, object>
				{
					{ "WinsCount", cachedStatisticsManager.WinsCount },
					{
						"GameTimeMinutes",
						cachedStatisticsManager.TotalGameTime.Minutes
					}
				});
			}
			else
			{
				Analytics.CustomEvent("Level_VSComputer_Lose", new Dictionary<string, object>
				{
					{ "LosesCount", cachedStatisticsManager.LosesCount },
					{
						"GameTimeMinutes",
						cachedStatisticsManager.TotalGameTime.Minutes
					}
				});
			}
			break;
		case GameType.VSHuman:
			text = "Level_VSHuman_Finished";
			break;
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
		InitializeFlags(type);
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		if (type != GameType.Tournament)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GUIChest[] array6 = chests;
				foreach (GUIChest gUIChest in array6)
				{
					ParticleSystem[] componentsInChildren = gUIChest.GetComponentsInChildren<ParticleSystem>();
					foreach (ParticleSystem particleSystem in componentsInChildren)
					{
						particleSystem.Play();
					}
				}
				chestDelayFinished = true;
				InitializeProposal();
				GUIChest[] array7 = chests;
				foreach (GUIChest gUIChest2 in array7)
				{
					gUIChest2.InitChest();
					gUIChest2.SpawnChest();
				}
				tk2dUIItem[] array8 = keyButtons;
				foreach (tk2dUIItem tk2dUIItem2 in array8)
				{
					tk2dUIItem2.enabled = false;
				}
				tk2dUIItem[] array9 = nextButtons;
				foreach (tk2dUIItem tk2dUIItem3 in array9)
				{
					tk2dUIItem3.enabled = false;
				}
				if (keyButtons[0].gameObject.activeSelf)
				{
					ButtonsState = ButtonsAppearState.KeyButton;
				}
				else
				{
					ButtonsState = ButtonsAppearState.NextButton;
				}
			}, 2f);
		}
		else
		{
			bool isTournamentSuccessfullyEnded = base.CachedManagers.CachedTournamentManager.IsTournamentSuccessfullyEnded;
			GameObject[] array = tournamentEndLayouter;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(isTournamentSuccessfullyEnded);
			}
			GameObject[] array2 = tournamentBattleWinLayouter;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.SetActive(firstWinner && !isTournamentSuccessfullyEnded);
			}
			GameObject[] array3 = tournamentLoseLayouter;
			foreach (GameObject gameObject3 in array3)
			{
				gameObject3.SetActive(!firstWinner);
			}
			tk2dBaseSprite[] array4 = cupSprite;
			foreach (tk2dBaseSprite s in array4)
			{
				s.SetSprite(CupSpriteNameByDifficultAndEndState(base.CachedManagers.CachedTournamentManager.CurrentTournament.settings.difficultType, isTournamentSuccessfullyEnded));
				if (isTournamentSuccessfullyEnded)
				{
					s.transform.localPosition = Vector3.up * cupSpriteWinOffset;
					s.transform.localScale = new Vector3(cupspriteWinScale, cupspriteWinScale, 1f);
					SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
					{
						GameObject gameObject4 = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIResultWinCup));
						gameObject4.transform.parent = s.transform;
						gameObject4.transform.localPosition = Vector3.one;
					}, 0f);
					TweenPosition[] array5 = cupTweeners;
					foreach (TweenPosition tweenPosition in array5)
					{
						tweenPosition.SetEndStateImmediately();
					}
				}
			}
		}
		SetVolumeForSound("theme_result_screen", 1f);
		SetVolumeForSound("theme_main", 0f);
		ProposalManager cachedProposalManager = base.CachedManagers.CachedProposalManager;
		cachedProposalManager.GenerateProposalMenu();
		if (cachedProposalManager.IsFoolishSubscribeAvalaible)
		{
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.AutoRenewableSubscribeFoolish);
			base.CachedManagers.CachedStatisticsManager.LastFoolishSubscribe = DateTime.Now;
		}
		else if (cachedProposalManager.CurrentProposalMenu == ProposalMenuType.X2)
		{
			int num7 = cachedStatisticsManager.CoinsFromLastGame + base.CachedManagers.CachedMicroachievementsManager.CoinsFromAchievementsQueue;
			if (num7 >= cachedProposalManager.MinCoinsForDoubleCoinsResultPopup)
			{
				base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.DoubleCoins);
			}
		}
		else if (cachedProposalManager.IsForcedDoubleCoins)
		{
			bool flag2 = cachedProposalManager.ProposalTimer.TimerInSeconds <= 0f;
			flag2 |= cachedProposalManager.LastProposalWithTimerType != ProposalMenuType.X2;
			cachedProposalManager.IsShowingForcedDoubleCoins = flag2;
			base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.DoubleCoins);
		}
		SingletonMonoBehaviour<ICloudManager>.Instance.SaveProgressToICloud();
		base.Show(onFinished2, immediately, delay, callInvokes);
	}

	private void InitializeProposal()
	{
		tk2dUIItem[] array = keyButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.gameObject.SetActive(true);
		}
		if (!base.CachedManagers.CachedTutorialManager.IsFreeChestsEnded)
		{
			tk2dTextMesh[] array2 = keyDescriptionLabels;
			foreach (tk2dTextMesh tk2dTextMesh2 in array2)
			{
				tk2dTextMesh2.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.result.key.button_chest");
			}
			return;
		}
		switch (base.CachedManagers.CachedProposalManager.CurrentProposalResult)
		{
		case ProposalResultType.Chest:
		{
			tk2dUIItem[] array10 = keyButtons;
			foreach (tk2dUIItem tk2dUIItem4 in array10)
			{
				tk2dUIItem4.gameObject.SetActive(false);
			}
			break;
		}
		case ProposalResultType.Video:
		{
			string text = "ui.result.key.button_coins";
			text += ".android";
			if (base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive)
			{
				text = "ui.result.key.button.free.lock";
			}
			else
			{
				SkeletonAnimation[] array4 = buttonAnimations;
				foreach (SkeletonAnimation skeletonAnimation in array4)
				{
					skeletonAnimation.Skeleton.SetAttachment(slotIconAnimation, attachmentPlayAnimation);
					skeletonAnimation.Skeleton.SetAttachment(slotKeyAnimation, attachmentKeyAnimation);
				}
			}
			tk2dTextMesh[] array5 = keyDescriptionLabels;
			foreach (tk2dTextMesh tk2dTextMesh3 in array5)
			{
				tk2dTextMesh3.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey(text);
			}
			break;
		}
		case ProposalResultType.Rate:
		{
			tk2dTextMesh[] array8 = keyDescriptionLabels;
			foreach (tk2dTextMesh tk2dTextMesh5 in array8)
			{
				tk2dTextMesh5.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.result.key.button_rate");
			}
			SkeletonAnimation[] array9 = buttonAnimations;
			foreach (SkeletonAnimation skeletonAnimation3 in array9)
			{
				skeletonAnimation3.Skeleton.SetAttachment(slotIconAnimation, attachmentRateAnimation);
				skeletonAnimation3.Skeleton.SetAttachment(slotKeyAnimation, attachmentStarAnimation);
			}
			break;
		}
		case ProposalResultType.NoAds:
		{
			tk2dTextMesh[] array6 = keyDescriptionLabels;
			foreach (tk2dTextMesh tk2dTextMesh4 in array6)
			{
				tk2dTextMesh4.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.proposal.menu.remove.ads.description");
			}
			SkeletonAnimation[] array7 = buttonAnimations;
			foreach (SkeletonAnimation skeletonAnimation2 in array7)
			{
				skeletonAnimation2.Skeleton.SetAttachment(slotIconAnimation, attachmentRateAnimation);
				skeletonAnimation2.Skeleton.SetAttachment(slotKeyAnimation, attachmentStarAnimation);
			}
			break;
		}
		case ProposalResultType.None:
		{
			tk2dUIItem[] array3 = keyButtons;
			foreach (tk2dUIItem tk2dUIItem3 in array3)
			{
				tk2dUIItem3.gameObject.SetActive(false);
			}
			break;
		}
		}
	}

	private void SetVolumeForSound(string soundName, float volume)
	{
		SoundObject[] playingAudioObjects = SoundsManager.GetPlayingAudioObjects(soundName);
		if (playingAudioObjects != null)
		{
			SoundObject[] array = playingAudioObjects;
			foreach (SoundObject soundObject in array)
			{
				soundObject.Volume = volume;
			}
		}
	}

	private void ShowAchievement(Queue<MicroachievementInfo> queue)
	{
		if (queue.Count <= 0)
		{
			return;
		}
		bool flag = false;
		GUIChest[] array = chests;
		foreach (GUIChest gUIChest in array)
		{
			flag |= gUIChest.IsChestOpening;
		}
		if (flag)
		{
			return;
		}
		MicroachievementInfo info = queue.Peek();
		if (!info.isRestoredAchievement)
		{
			if (!string.IsNullOrEmpty(info.localisationName))
			{
				tk2dTextMesh[] array2 = textsAchievement;
				foreach (tk2dTextMesh tk2dTextMesh2 in array2)
				{
					tk2dTextMesh2.text = info.LocalisationName();
				}
			}
			if (!string.IsNullOrEmpty(info.CurrentSpriteName()))
			{
				tk2dBaseSprite[] array3 = iconsAchievement;
				foreach (tk2dBaseSprite tk2dBaseSprite2 in array3)
				{
					tk2dBaseSprite2.SetSprite(info.CurrentSpriteName());
				}
			}
			if (info.coinsAward > 0)
			{
				GUILayoutCell[] array4 = countCellsAchievement;
				foreach (GUILayoutCell gUILayoutCell in array4)
				{
					gUILayoutCell.gameObject.SetActive(true);
				}
				tk2dTextMesh[] array5 = countLabelsAchiviement;
				foreach (tk2dTextMesh tk2dTextMesh3 in array5)
				{
					tk2dTextMesh3.text = info.AwardString();
				}
			}
			else
			{
				GUILayoutCell[] array6 = countCellsAchievement;
				foreach (GUILayoutCell gUILayoutCell2 in array6)
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

	private void InitializeFlags(GameType type)
	{
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		bool flag = true;
		bool flag2 = false;
		if (type == GameType.AppleShoot || type == GameType.BirdsHunt)
		{
			int num = ((type != GameType.AppleShoot) ? cachedStatisticsManager.HighScoreBirdsKilled : cachedStatisticsManager.HighScoreFruitsSmashed);
			flag = false;
			flag2 = cachedStatisticsManager.Score >= num;
			if (flag2)
			{
				tk2dTextMesh[] array = recordFlagLabels;
				foreach (tk2dTextMesh tk2dTextMesh2 in array)
				{
					tk2dTextMesh2.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.result.flag.record") + cachedStatisticsManager.Score;
				}
			}
			else
			{
				tk2dTextMesh[] array2 = usualFlagBestLabels;
				foreach (tk2dTextMesh tk2dTextMesh3 in array2)
				{
					tk2dTextMesh3.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.result.flag.usual.best") + num;
				}
				tk2dTextMesh[] array3 = usualFlagCurrentLabels;
				foreach (tk2dTextMesh tk2dTextMesh4 in array3)
				{
					tk2dTextMesh4.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.result.flag.usual.current") + cachedStatisticsManager.Score;
				}
			}
		}
		else
		{
			string key = ((!firstWinner) ? "ui.result.flag.epiclose" : "ui.result.flag.epicwin");
			switch (type)
			{
			case GameType.VSHuman:
				key = ((!firstWinner) ? "ui.result.flag.player2win" : "ui.result.flag.player1win");
				break;
			case GameType.Tournament:
				if (base.CachedManagers.CachedTournamentManager.IsTournamentSuccessfullyEnded)
				{
					key = "ui.result.flag.tournamentwin";
				}
				break;
			}
			key = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(key);
			tk2dBaseSprite[] array4 = flagSprites;
			foreach (tk2dBaseSprite tk2dBaseSprite2 in array4)
			{
				tk2dBaseSprite2.SetSprite(key);
				if (firstWinner || type == GameType.VSHuman)
				{
					PlayWinEffect(tk2dBaseSprite2.transform);
				}
			}
		}
		tk2dBaseSprite[] array5 = flagSprites;
		foreach (tk2dBaseSprite tk2dBaseSprite3 in array5)
		{
			tk2dBaseSprite3.gameObject.SetActive(flag);
		}
		GameObject[] array6 = recordFlags;
		foreach (GameObject gameObject in array6)
		{
			gameObject.SetActive(!flag && flag2);
			if (!flag && flag2)
			{
				PlayWinEffect(gameObject.transform);
			}
		}
		GameObject[] array7 = usualFlags;
		foreach (GameObject gameObject2 in array7)
		{
			gameObject2.SetActive(!flag && !flag2);
		}
	}

	private void PlayWinEffect(Transform parentTransform)
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIResultWin));
			gameObject.transform.parent = parentTransform;
			gameObject.transform.localPosition = Vector3.zero;
			currentEffects.Add(gameObject);
			gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIResultWinConfetti));
			gameObject.transform.parent = parentTransform;
			gameObject.transform.localPosition = new Vector3(0f, 30f, 0f);
			gameObject.transform.localPosition *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
			gameObject.transform.localScale *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
			if (tk2dSystem.IsRetina)
			{
				ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
				if (component != null)
				{
					ParticleSystem.MainModule main = component.main;
					main.startLifetimeMultiplier = 4f;
				}
			}
			currentEffects.Add(gameObject);
		}, 0f);
	}

	private string CupSpriteNameByDifficultAndEndState(TournamentDifficultType difficult, bool isTournamentEnded)
	{
		string result = "Cup_Easy_Socket";
		switch (difficult)
		{
		case TournamentDifficultType.Easy:
			result = ((!isTournamentEnded) ? "Cup_Easy_Socket" : "Cup_Easy");
			break;
		case TournamentDifficultType.Medium:
			result = ((!isTournamentEnded) ? "Cup_Normal_Socket" : "Cup_Normal");
			break;
		case TournamentDifficultType.Hard:
			result = ((!isTournamentEnded) ? "Cup_Legendary_Socket" : "Cup_Legendary");
			break;
		}
		return result;
	}

	private void GameCenterButton_OnClick()
	{
		base.CachedManagers.CachedGameCenterManager.OpenLeaderboard();
	}

	private void NextButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Main);
		});
	}

	private void KeyButton_OnClick()
	{
		needForceMenuScreen = base.CachedManagers.CachedProposalManager.CurrentProposalResult == ProposalResultType.Rate;
		bool isNoAds = base.CachedManagers.CachedProposalManager.CurrentProposalResult == ProposalResultType.NoAds;
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
		base.CachedManagers.CachedProposalManager.GetProposalResult(delegate(bool result)
		{
			InitializeProposal();
			if (!base.CachedManagers.CachedTutorialManager.IsFreeChestsEnded)
			{
				tk2dUIItem[] array = keyButtons;
				foreach (tk2dUIItem tk2dUIItem2 in array)
				{
					tk2dUIItem2.gameObject.SetActive(false);
				}
			}
			if (result && !isNoAds)
			{
				GUIChest[] array2 = chests;
				foreach (GUIChest gUIChest in array2)
				{
					gUIChest.OpenLock();
				}
				if (base.CachedManagers.CachedProposalManager.IsEpicChestResultAvalaible)
				{
					tk2dUIItem[] array3 = nextButtons;
					foreach (tk2dUIItem tk2dUIItem3 in array3)
					{
						tk2dUIItem3.gameObject.SetActive(false);
					}
				}
			}
			else if (!isNoAds || (isNoAds && !result))
			{
				base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
				if (needForceMenuScreen)
				{
					base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
					{
						if (this != null)
						{
							NextButton_OnClick();
						}
					}, forcedMenuScreenDelay);
				}
			}
		});
	}

	private void StatisticsManager_OnCoinsChanged(int obj = 0)
	{
		tk2dTextMesh[] array = coinsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
		}
		for (int j = 0; j < coinsRewardLabel.Length; j++)
		{
			coinsRewardLabel[j].text = string.Format("{0}{1}", "+", base.CachedManagers.CachedStatisticsManager.CoinsFromLastGame.ToString());
		}
	}

	private void StatisticsManager_OnGemsChanged(int gemsCount = 0)
	{
		tk2dTextMesh[] array = gemsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Gems.ToString();
		}
	}

	private void Chest_OnOpenLock()
	{
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		InitializeProposal();
	}

	private void Chest_OnSpawnEnd()
	{
		for (int i = 0; i < landscapeTweens.Length; i++)
		{
			landscapeTweens[i].endPosition = Vector3.left * ((float)Screen.width / 2f + landscapeBorderCell.RecievedRect.width);
			landscapeTweens[i].endPosition *= ((!(GUIScreenManager.GuiScale > 1f)) ? 1f : GUIScreenManager.GuiScale);
		}
		if (landscapeTweens.Length > 0)
		{
			landscapeInfoTween.endPosition = -1f * landscapeTweens[0].endPosition;
		}
		for (int j = 0; j < portraitTweens.Length; j++)
		{
			portraitTweens[j].endPosition = Vector3.up * ((float)Screen.height / 2f + portraitBorderCell.RecievedRect.height);
			portraitTweens[j].endPosition *= ((!(GUIScreenManager.GuiScale > 1f)) ? 1f : GUIScreenManager.GuiScale);
		}
		if (portraitTweens.Length > 0)
		{
			portraitInfoTween.endPosition = -1f * portraitTweens[0].endPosition;
		}
		if (base.CachedManagers.CachedRouletteManager.KeysCount == SingletonMonoBehaviour<RouletteManager>.Instance.LocksCount)
		{
			GUIChest[] array = chests;
			foreach (GUIChest gUIChest in array)
			{
				gUIChest.OpenLock();
			}
		}
		else
		{
			base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		}
	}

	private void MicroachievementsManager_OnReachedAchievement(Queue<MicroachievementInfo> queue)
	{
		if (!controlAchievement.IsShown && !controlAchievement.IsAnimating)
		{
			ShowAchievement(queue);
		}
	}

	private void HidePopup(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(HidePopup));
		CharacterNameType lastCharacterNameType = base.CachedManagers.CachedStatisticsManager.LastCharacterNameType;
		ShopItemCharacter shopItemCharacter = base.ObjectsSpawner.GetShopItemCharacter(lastCharacterNameType);
		if (!shopItemCharacter.Bought)
		{
			CharacterNameType beforeTrySelectedCharacterNameType = base.ObjectsSpawner.BeforeTrySelectedCharacterNameType;
			base.ObjectsSpawner.SetFirstSelectedCharacter(beforeTrySelectedCharacterNameType);
		}
		GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.GAME_PLAYED, GoliathIds.CurrencyName.COINS, base.CachedManagers.CachedStatisticsManager.CoinsFromLastGame, GoliathIds.Placement.END_GAME, base.CachedManagers.CachedStatisticsManager.Coins - base.CachedManagers.CachedStatisticsManager.CoinsFromLastGame);
	}

	private void LeaveTournament_OnClick()
	{
		NextButton_OnClick();
	}

	private void ExitButton_OnClick()
	{
		if (!isTournamentProgressSaved)
		{
			GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.LeavingTournamentOnResultPopup, true, null, true);
			gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(LeavingTournament_OnPopupHide));
		}
		else
		{
			NextButton_OnClick();
		}
	}

	private void LeavingTournament_OnPopupHide(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(LeavingTournament_OnPopupHide));
		switch ((popup as LeavingTournamentOnResultScreenPopup).SavingState)
		{
		case SavingState.ExitWithSave:
			base.CachedManagers.CachedTournamentManager.SaveTournament();
			isTournamentProgressSaved = true;
			NextButton_OnClick();
			break;
		case SavingState.ExitWithoutSave:
			NextButton_OnClick();
			break;
		}
	}

	private void SaveForVideoButton_OnClick()
	{
		if (isTournamentProgressSaved)
		{
			return;
		}
		base.CachedManagers.CachedProposalManager.ShowVideoForSaveTournametResult(delegate(bool result)
		{
			if (result)
			{
				base.CachedManagers.CachedTournamentManager.SaveTournament();
			}
			isTournamentProgressSaved = result;
			for (int i = 0; i < saveForVideoButton.Length; i++)
			{
				saveForVideoButton[i].gameObject.SetActive(!result);
			}
		});
	}

	private void SaveForGemsButton_OnClick()
	{
		if (isTournamentProgressSaved)
		{
			return;
		}
		if (base.CachedManagers.CachedStatisticsManager.Gems >= base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice)
		{
			base.CachedManagers.CachedStatisticsManager.DecreaseGems(base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice);
			base.CachedManagers.CachedTournamentManager.SaveTournament();
			isTournamentProgressSaved = true;
			for (int i = 0; i < saveForGemsButton.Length; i++)
			{
				saveForGemsButton[i].gameObject.SetActive(false);
			}
		}
		else
		{
			GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true);
			(gUIPopup as MiniBankPopup).InitPopup(base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice);
		}
	}

	private void NextBattle_OnClick()
	{
		base.CachedManagers.CachedStatisticsManager.ResetStatistics();
		base.ObjectsSpawner.SetSecondSelectedCharacter(base.CachedManagers.CachedTournamentManager.EnemyCharacter);
		base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Versus);
	}

	private void ClaimReward_OnClick()
	{
		for (int i = 0; i < claimRewardButton.Length; i++)
		{
			claimRewardButton[i].enabled = false;
		}
		MicroachievementType type = MicroachievementType.None;
		TournamentDifficultType difficultType = base.CachedManagers.CachedTournamentManager.CurrentTournament.settings.difficultType;
		float delay = 0f;
		switch (difficultType)
		{
		case TournamentDifficultType.Easy:
			type = MicroachievementType.TournamentWinEasy;
			delay = easyTournamentEndDelay;
			break;
		case TournamentDifficultType.Medium:
			type = MicroachievementType.TournamentWinMedium;
			delay = mediumTournamentEndDelay;
			break;
		case TournamentDifficultType.Hard:
			type = MicroachievementType.TournamentWinHard;
			delay = hardTournamentEndDelay;
			break;
		}
		if (difficultType == TournamentDifficultType.Hard)
		{
			controlFade.gameObject.SetActive(true);
			controlFade.Show(delegate
			{
				base.CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.white;
				base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
				{
					base.CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.white;
					base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Roulette, delegate
					{
						base.CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.black;
						if (type != MicroachievementType.None)
						{
							base.CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(type);
							base.CachedManagers.CachedTournamentManager.TournamentFinished();
						}
					});
				}, false);
			}, false, delay, true);
			return;
		}
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				NextButton_OnClick();
				if (type != MicroachievementType.None)
				{
					base.CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(type);
					base.CachedManagers.CachedTournamentManager.TournamentFinished();
				}
			}
		}, delay);
	}

	private void ProposalManager_OnRemoveAdsPurchased()
	{
		GUIChest[] array = chests;
		foreach (GUIChest gUIChest in array)
		{
			gUIChest.OpenLock();
		}
	}

	private void ProposalManager_OnRemoveAdsPurchaseFailed()
	{
		NextButton_OnClick();
		tk2dUIItem[] array = nextButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
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
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.ACHIEVEMENT, GoliathIds.CurrencyName.COINS, info.coinsAward, GoliathIds.Placement.END_GAME);
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
		if (GUI.Button(new Rect(0f, 100f, 100f, 100f), "Reset chest"))
		{
			base.CachedManagers.CachedRouletteManager.KeysCount = 0;
			InitializeProposal();
			GUIChest[] array = chests;
			foreach (GUIChest gUIChest in array)
			{
				gUIChest.InitChest();
				gUIChest.SpawnChest();
			}
		}
		if (GUI.Button(new Rect(0f, 200f, 100f, 100f), "+ keys") && base.CachedManagers.CachedRouletteManager.KeysCount < SingletonMonoBehaviour<RouletteManager>.Instance.LocksCount)
		{
			base.CachedManagers.CachedRouletteManager.KeysCount++;
			tk2dUIItem[] array2 = keyButtons;
			foreach (tk2dUIItem tk2dUIItem2 in array2)
			{
				tk2dUIItem2.gameObject.SetActive(false);
			}
			base.CachedManagers.CachedProposalManager.CurrentProposalResult = ProposalResultType.Chest;
			GUIChest[] array3 = chests;
			foreach (GUIChest gUIChest2 in array3)
			{
				gUIChest2.OpenLock();
			}
		}
		if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 100f, 100f), "NextBattle"))
		{
			NextBattle_OnClick();
		}
		if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 200, 100f, 100f), "SaveExit"))
		{
			NextButton_OnClick();
		}
		if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 300, 100f, 100f), "LeaveTour"))
		{
			NextButton_OnClick();
		}
		if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 400, 100f, 100f), "ClaimReward"))
		{
			ClaimReward_OnClick();
		}
	}

	public override void BackButtonPressed()
	{
		if (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType == GameType.Tournament)
		{
			LeaveTournament_OnClick();
		}
		else if (SingletonMonoBehaviour<TutorialManager>.Instance.IsFreeChestsEnded)
		{
			NextButton_OnClick();
		}
	}
}
