using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.Analytics;

public class GameScreen : GUIScreen
{
	private const string PLAYER_FIRST = "ui.game.turn.player_first";

	private const string PLAYER_SECOND = "ui.game.turn.player_second";

	private const string PLAYER_LEFT_MULTIPLAYER_NAME_PLACEHOLDER = "GOD";

	private const string PLAYER_RIGHT_MULTIPLAYER_NAME_PLACEHOLDER = "DEVIL";

	private const string ANIMATION_FINISH_HIM = "popout";

	[SerializeField]
	private GUILayouter verticalLayouter;

	[SerializeField]
	private GUILayouter horizontalLayouter;

	[SerializeField]
	private tk2dUIItem[] pauseButtons;

	[SerializeField]
	private GUIControl[] scoreControls;

	[SerializeField]
	private tk2dTextMesh[] scorelabels;

	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private tk2dTextMesh[] gemsLabels;

	[SerializeField]
	private GUIControl timerWarningControl;

	[SerializeField]
	private GUIControl timerAddControl;

	[SerializeField]
	private tk2dTextMesh timerLabel;

	[SerializeField]
	private Transform timerIcon;

	[SerializeField]
	private tk2dTextMesh turnsLabel;

	[SerializeField]
	private SkeletonAnimation turnsAnimation;

	[SerializeField]
	private float turnsLabelDurationOnScreen = 1.1f;

	[SerializeField]
	private DistanceMark distanceMark;

	[SerializeField]
	private DraggerGizmos draggerGizmos;

	[SerializeField]
	private LineGizmos lineGizmos;

	[SerializeField]
	private ProgressBar[] firstPlayerHealthBars;

	[SerializeField]
	private ProgressBar[] secondPlayerHealthBars;

	[SerializeField]
	private GameObject[] firstCharacterHealthBarsMembershipFrames;

	[SerializeField]
	private GameObject[] secondCharacterHealthBarsMembershipFrames;

	[SerializeField]
	private tk2dBaseSprite[] firstPlayerSprites;

	[SerializeField]
	private tk2dBaseSprite[] secondPlayerSprites;

	[SerializeField]
	private GameObject[] firstCharacterMembershipFrames;

	[SerializeField]
	private GameObject[] secondCharacterMembershipFrames;

	[SerializeField]
	private GameObject typeVersusRootObject;

	[SerializeField]
	private GameObject typeBidsRootObject;

	[SerializeField]
	private GameObject typeAppleRootObject;

	[SerializeField]
	private GUIControl controlAchievement;

	[SerializeField]
	private tk2dTextMesh textAchievement;

	[SerializeField]
	private tk2dBaseSprite iconAchievement;

	[SerializeField]
	private GUILayoutCell countCellAchievement;

	[SerializeField]
	private tk2dTextMesh countLabelAchiviement;

	[SerializeField]
	private float timeAchievementShow;

	[SerializeField]
	private float delayAddEffect;

	[SerializeField]
	private GameScreenWeapons gameScreenWeaponPrefab;

	[SerializeField]
	private GUIControl finishHimControl;

	[SerializeField]
	private SkeletonAnimation finishHimAnimation;

	[SerializeField]
	private FinishLanguageSkeletonDataAsset defaultLanguageAsset;

	[SerializeField]
	private List<FinishLanguageSkeletonDataAsset> otherLanguagesAssets;

	[SerializeField]
	private float finishHimShowTime;

	[SerializeField]
	private float brutalityEffectHorisontalPositionX;

	[SerializeField]
	private float brutalityEffectVerticalPositionX;

	[SerializeField]
	private int brutalityShakesCount;

	[SerializeField]
	private float bruatlityShakesDelay;

	[SerializeField]
	private float brutalityShakesStartDelay;

	[SerializeField]
	private float brutalityShakesAmplitude;

	[SerializeField]
	private float brutalityShakesDuration;

	[SerializeField]
	private Tutorial tutorialPrefab;

	[SerializeField]
	private Emojis emojisPrefab;

	[SerializeField]
	private TurnTimer turnTimerPrefab;

	[SerializeField]
	private MultiplayerNames playerNamesForMultiplayerPrefab;

	[SerializeField]
	private BoxCollider gameFieldCollider;

	[SerializeField]
	private GUIControl[] appleMissesControls;

	[SerializeField]
	private int birdsTimerWarningBorder = 5;

	[SerializeField]
	private tk2dSpriteCollectionData iconWeaponsSpriteCollectionData;

	[Header("iPhone X")]
	[SerializeField]
	private GUILayoutCell[] topOffsetCells;

	[SerializeField]
	private float topIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] leftButtonsCells;

	[SerializeField]
	private float leftBirdsButtonsIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] rightButtonsCells;

	[SerializeField]
	private float sideButtonsIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] healthBarsCells;

	[SerializeField]
	private float healthBarsIPhoneXPortraitSize;

	[SerializeField]
	private GUILayoutCell[] healthBarsMembershipCells;

	[SerializeField]
	private float healthBarsMembershipIPhoneXPortraitSize;

	[SerializeField]
	private GUILayoutCell healthBarsBackCell;

	[SerializeField]
	private float healthBarsBackIPhoneXPortraitSize;

	[SerializeField]
	private GUILayoutCell birdsTopOffsetCell;

	[SerializeField]
	private GUILayoutCell birdsLeftOffsetCell;

	[SerializeField]
	private GUILayoutCell birdsRightOffsetCell;

	[SerializeField]
	private float leftContentIPhoneXOffset;

	[SerializeField]
	private float bottomTutorialButtonIPhoneXOffset;

	[SerializeField]
	private float portraitTutorialHintIPhoneXOffset;

	[SerializeField]
	private float portraitMultiplayerNamesIPhoneXSize;

	[SerializeField]
	private float landscapeMultiplayerNamesIPhoneXSize;

	[SerializeField]
	private GUILayoutCell finishHimIPhoneXCell;

	[SerializeField]
	private float finishHimIPhoneXOffset;

	[SerializeField]
	private GUILayouter partyMastersButtonRootLayouter;

	[SerializeField]
	private GUILayouter partyMastersAnnouncerRootLayouter;

	[SerializeField]
	private GUILayoutCell partyMastersButtonIPhoneXCell;

	[SerializeField]
	private GUILayoutCell partyMastersAnnouncerIPhoneXCell;

	[SerializeField]
	private float partyMastersButtonIPhoneXOffset;

	[SerializeField]
	private PartyMastersButton partyMastersButtonPrefab;

	[SerializeField]
	private PartyMastersButton partyMastersAnnouncerPrefab;

	[SerializeField]
	private float partyMastersAnnouncerHideDelay;

	private Dictionary<WeaponType, string> gameWeaponsIcon = new Dictionary<WeaponType, string>();

	private List<GameObject> currentEffects = new List<GameObject>();

	private const float raycastDistanceGameField = 1000f;

	private int uiLayerMask;

	private int timerSeconds;

	private float portraitTimer;

	private float landscapeTimer;

	private GameScreenWeapons gameScreenWeapon;

	private Tutorial tutorial;

	private Emojis emojis;

	private TurnTimer turnTimer;

	private MultiplayerNames playerNamesForMultiplayer;

	private Spine.Animation cachedAnimationTurnsAppear;

	private Spine.Animation cachedAnimationTurnsDisappear;

	private EventData cachedEventDataAppearEnd;

	private List<GameObject> brutalityEffects = new List<GameObject>();

	private float brutalityTime;

	private int brutalityCount;

	private bool isBrutalityStarted;

	private Vector3 cameraBeforeBrutalityPosition;

	private PartyMastersButton partyMastersButton;

	private PartyMastersButton partyMastersAnnouncer;

	private bool isBarInterrup;

	public GUILayouter HorizontalLayouter
	{
		get
		{
			return horizontalLayouter;
		}
	}

	public GUILayouter VerticalLayouter
	{
		get
		{
			return verticalLayouter;
		}
	}

	public DistanceMark DistanceMark
	{
		get
		{
			return distanceMark;
		}
	}

	public DraggerGizmos DraggerGizmos
	{
		get
		{
			return draggerGizmos;
		}
	}

	public LineGizmos LineGizmos
	{
		get
		{
			return lineGizmos;
		}
	}

	public bool IsBarInterrup
	{
		get
		{
			return isBarInterrup;
		}
		set
		{
			isBarInterrup = value;
		}
	}

	public Tutorial Tutorial
	{
		get
		{
			if (tutorial == null)
			{
				tutorial = UnityEngine.Object.Instantiate(tutorialPrefab);
				tutorial.transform.parent = base.transform;
				tutorial.transform.localPosition = Vector3.zero;
				tutorial.transform.localScale = Vector3.one;
				if (GameManager.IsIPhoneX)
				{
					for (int i = 0; i < tutorial.IPhoneXCells.Length; i++)
					{
						tutorial.IPhoneXCells[i].SizeValue = bottomTutorialButtonIPhoneXOffset;
					}
					tutorial.IPhoneXHintPortraitCell.SizeValue = ((base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType != GUILayouterRotationType.Portrait) ? 0f : portraitTutorialHintIPhoneXOffset);
				}
			}
			return tutorial;
		}
	}

	public Emojis Emojis
	{
		get
		{
			if (emojis == null)
			{
				emojis = UnityEngine.Object.Instantiate(emojisPrefab);
				emojis.transform.parent = base.transform;
				emojis.transform.localPosition = Vector3.zero;
				emojis.transform.localScale = Vector3.one;
				if (GameManager.IsIPhoneX)
				{
					emojis.IPhoneXCell.SizeValue = ((base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType != GUILayouterRotationType.Portrait) ? leftContentIPhoneXOffset : 0f);
				}
			}
			return emojis;
		}
	}

	public TurnTimer TurnTimer
	{
		get
		{
			if (turnTimer == null)
			{
				turnTimer = UnityEngine.Object.Instantiate(turnTimerPrefab);
				turnTimer.transform.parent = base.transform;
				turnTimer.transform.localPosition = Vector3.zero;
				turnTimer.transform.localScale = Vector3.one;
			}
			return turnTimer;
		}
	}

	public MultiplayerNames PlayerNamesForMultiplayer
	{
		get
		{
			if (playerNamesForMultiplayer == null)
			{
				playerNamesForMultiplayer = UnityEngine.Object.Instantiate(playerNamesForMultiplayerPrefab);
				playerNamesForMultiplayer.transform.parent = base.transform;
				playerNamesForMultiplayer.transform.localPosition = Vector3.zero;
				playerNamesForMultiplayer.transform.localScale = Vector3.one;
				if (GameManager.IsIPhoneX)
				{
					playerNamesForMultiplayer.IPhoneXPortraitCell.SizeValue = ((base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType != GUILayouterRotationType.Portrait) ? 0f : (topIPhoneXOffset * 2f));
					for (int i = 0; i < playerNamesForMultiplayer.IPhoneXCells.Length; i++)
					{
						playerNamesForMultiplayer.IPhoneXCells[i].SizeValue = ((base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType != GUILayouterRotationType.Portrait) ? landscapeMultiplayerNamesIPhoneXSize : portraitMultiplayerNamesIPhoneXSize);
					}
				}
			}
			return playerNamesForMultiplayer;
		}
	}

	public tk2dSpriteCollectionData IconWeaponsSpriteCollectionData
	{
		get
		{
			return iconWeaponsSpriteCollectionData;
		}
	}

	private int TimerSeconds
	{
		get
		{
			return timerSeconds;
		}
		set
		{
			if (timerSeconds == value)
			{
				return;
			}
			if (timerSeconds < value)
			{
				timerWarningControl.HideImmediately();
				timerAddControl.Show(delegate(GUIControl obj)
				{
					obj.Hide();
				});
			}
			timerSeconds = value;
			if (timerSeconds <= birdsTimerWarningBorder)
			{
				timerWarningControl.Show(delegate(GUIControl obj)
				{
					obj.Hide();
				});
			}
		}
	}

	private GameScreenWeapons GameScreenWeapon
	{
		get
		{
			if (gameScreenWeapon == null)
			{
				gameScreenWeapon = UnityEngine.Object.Instantiate(gameScreenWeaponPrefab);
				gameScreenWeapon.transform.parent = base.transform;
				gameScreenWeapon.transform.localPosition = Vector3.zero;
				if (GameManager.IsIPhoneX)
				{
					gameScreenWeapon.IPhoneXCell.SizeValue = leftContentIPhoneXOffset;
				}
			}
			return gameScreenWeapon;
		}
	}

	private Spine.Animation CachedAnimationTurnsAppear
	{
		get
		{
			if (cachedAnimationTurnsAppear == null)
			{
				cachedAnimationTurnsAppear = turnsAnimation.Skeleton.Data.FindAnimation("appear");
			}
			return cachedAnimationTurnsAppear;
		}
	}

	private Spine.Animation CachedAnimationTurnsDisappear
	{
		get
		{
			if (cachedAnimationTurnsDisappear == null)
			{
				cachedAnimationTurnsDisappear = turnsAnimation.Skeleton.Data.FindAnimation("disappear");
			}
			return cachedAnimationTurnsDisappear;
		}
	}

	private EventData CachedEventDataAppearEnd
	{
		get
		{
			if (cachedEventDataAppearEnd == null)
			{
				cachedEventDataAppearEnd = turnsAnimation.Skeleton.Data.FindEvent("Appear_End");
			}
			return cachedEventDataAppearEnd;
		}
	}

	private void Awake()
	{
		base.CachedManagers.CachedGameManager.IsGamePaused = false;
		base.CachedManagers.CachedGameManager.ForceResourcesUnload(1f);
		base.CachedManagers.CachedResourceManager.UnloadGeneralSpriteCollections(0.1f);
		gameWeaponsIcon.Clear();
		List<CharacterNameType> charactersNameTypes = base.ObjectsSpawner.CharactersNameTypes;
		for (int i = 0; i < charactersNameTypes.Count; i++)
		{
			if (!gameWeaponsIcon.ContainsKey(base.ObjectsSpawner.GetCharacter(charactersNameTypes[i]).AvailableWeapons[0]))
			{
				gameWeaponsIcon.Add(base.ObjectsSpawner.GetCharacter(charactersNameTypes[i]).AvailableWeapons[0], base.ObjectsSpawner.GetGUICharacter(charactersNameTypes[i]).WeaponIconName);
			}
		}
		bool flag = base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType == GUILayouterRotationType.Portrait;
		if (GameManager.IsIPhoneX)
		{
			for (int j = 0; j < topOffsetCells.Length; j++)
			{
				topOffsetCells[j].SizeValue = topIPhoneXOffset;
			}
			for (int k = 0; k < leftButtonsCells.Length; k++)
			{
				leftButtonsCells[k].SizeValue = 0f;
			}
			for (int l = 0; l < rightButtonsCells.Length; l++)
			{
				rightButtonsCells[l].SizeValue = sideButtonsIPhoneXOffset;
			}
			for (int m = 0; m < healthBarsCells.Length; m++)
			{
				healthBarsCells[m].SizeValue = healthBarsIPhoneXPortraitSize;
			}
			for (int n = 0; n < healthBarsMembershipCells.Length; n++)
			{
				healthBarsMembershipCells[n].SizeValue = healthBarsMembershipIPhoneXPortraitSize;
			}
			healthBarsBackCell.SizeValue = healthBarsBackIPhoneXPortraitSize;
			birdsTopOffsetCell.SizeValue = ((!flag) ? 0f : topIPhoneXOffset);
			birdsLeftOffsetCell.SizeValue = ((!flag) ? 0f : (leftBirdsButtonsIPhoneXOffset / 2f));
			birdsRightOffsetCell.SizeValue = ((!flag) ? sideButtonsIPhoneXOffset : 0f);
			finishHimIPhoneXCell.SizeValue = ((!flag) ? 0f : finishHimIPhoneXOffset);
			partyMastersButtonIPhoneXCell.SizeValue = ((!flag) ? 0f : partyMastersButtonIPhoneXOffset);
			partyMastersAnnouncerIPhoneXCell.SizeValue = ((!flag) ? partyMastersButtonIPhoneXOffset : 0f);
		}
	}

	private void Start()
	{
		uiLayerMask = LayerMask.GetMask("UI");
		if (!SingletonMonoBehaviour<StatisticsManager>.Instance.IsPartyMastersButtonClicked)
		{
			partyMastersButton = UnityEngine.Object.Instantiate(partyMastersButtonPrefab, partyMastersButtonRootLayouter.CachedTransform);
			partyMastersButtonRootLayouter.ResetLayouter();
			partyMastersButtonRootLayouter.Initialize();
		}
		if (SingletonMonoBehaviour<StatisticsManager>.Instance.IsNeedShowPartyMastersAnnouncer)
		{
			partyMastersAnnouncer = UnityEngine.Object.Instantiate(partyMastersAnnouncerPrefab, partyMastersAnnouncerRootLayouter.CachedTransform);
			partyMastersAnnouncerRootLayouter.ResetLayouter();
			partyMastersAnnouncerRootLayouter.Initialize();
		}
	}

	private void OnEnable()
	{
		tk2dUIItem[] array = pauseButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += PauseButton_OnClick;
		}
		StatisticsManager.OnScoreChanged += StatisticsManager_OnScoreChanged;
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnGemsChanged;
		LevelTimer.OnTimerChanged += LevelTimer_OnTimerChanged;
		LevelTimer.OnTimeAdd += LevelTimer_OnTimeAdd;
		Level.OnPlayerChanged += Level_OnPlayerChanged;
		MicroachievementsManager.OnReachedAchievement += MicroachievementsManager_OnReachedAchievement;
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
		Queue<MicroachievementInfo> queueAhievements = base.CachedManagers.CachedMicroachievementsManager.QueueAhievements;
		if (queueAhievements.Count != 0)
		{
			MicroachievementsManager_OnReachedAchievement(queueAhievements);
		}
		turnsAnimation.State.Event += TurnsAnimation_State_Event;
		portraitTimer = 0f;
		landscapeTimer = 0f;
		FatalityBase.OnBrutalityShowed += FatalityBase_OnBrutalityShowed;
		SuperGiftPopup.OnRewardReceived += OnRewardReceived;
		MisteryBoxRewardPopup.OnRewardReceived += OnRewardReceived;
	}

	private void OnDisable()
	{
		tk2dUIItem[] array = pauseButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= PauseButton_OnClick;
		}
		StatisticsManager.OnScoreChanged -= StatisticsManager_OnScoreChanged;
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnGemsChanged;
		LevelTimer.OnTimerChanged -= LevelTimer_OnTimerChanged;
		LevelTimer.OnTimeAdd -= LevelTimer_OnTimeAdd;
		Level.OnPlayerChanged -= Level_OnPlayerChanged;
		MicroachievementsManager.OnReachedAchievement -= MicroachievementsManager_OnReachedAchievement;
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
		turnsAnimation.State.Event -= TurnsAnimation_State_Event;
		SendEvents();
		FatalityBase.OnBrutalityShowed -= FatalityBase_OnBrutalityShowed;
		SuperGiftPopup.OnRewardReceived -= OnRewardReceived;
		MisteryBoxRewardPopup.OnRewardReceived -= OnRewardReceived;
	}

	private void Update()
	{
		if (base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType == GUILayouterRotationType.Landscape)
		{
			landscapeTimer += Time.unscaledDeltaTime;
		}
		else
		{
			portraitTimer += Time.unscaledDeltaTime;
		}
		if (!isBrutalityStarted)
		{
			return;
		}
		if (brutalityTime <= 0f)
		{
			if (brutalityCount < brutalityShakesCount - 1)
			{
				base.CachedManagers.CachedGameManager.CameraController.ApplyShakeWithDuration(brutalityShakesDuration, brutalityShakesAmplitude);
				brutalityTime = bruatlityShakesDelay;
				brutalityCount++;
			}
			else
			{
				AudioManager.PlayAudioWithKey("brutality");
				base.CachedManagers.CachedGameManager.CameraController.TweenMoveToPosition(cameraBeforeBrutalityPosition, 0.1f);
				isBrutalityStarted = false;
			}
		}
		else
		{
			brutalityTime -= Time.unscaledDeltaTime;
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			Queue<MicroachievementInfo> queueAhievements = base.CachedManagers.CachedMicroachievementsManager.QueueAhievements;
			if (queueAhievements.Count > 0)
			{
				ShowAchievement(queueAhievements);
			}
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.CachedManagers.CachedSubscriptionManager.SendSubscriptionStateEvent();
		GameType currentGameType = base.CachedManagers.CachedGameManager.CurrentGameType;
		draggerGizmos.gameObject.SetActive(false);
		typeAppleRootObject.SetActive(false);
		typeBidsRootObject.SetActive(false);
		distanceMark.gameObject.SetActive(distanceMark.IsVisible);
		typeVersusRootObject.SetActive(false);
		turnsAnimation.State.SetAnimation(0, CachedAnimationTurnsDisappear, false);
		turnsAnimation.Update(CachedAnimationTurnsAppear.Duration);
		controlAchievement.HideImmediately();
		switch (currentGameType)
		{
		case GameType.AppleShoot:
			typeAppleRootObject.SetActive(true);
			SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
			break;
		case GameType.BirdsHunt:
			typeBidsRootObject.SetActive(true);
			SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
			break;
		case GameType.VSHuman:
			typeVersusRootObject.SetActive(true);
			SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive, base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
			break;
		case GameType.VSComputer:
		case GameType.Tournament:
			typeVersusRootObject.SetActive(true);
			SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
			break;
		case GameType.Multiplayer:
			typeVersusRootObject.SetActive(true);
			Emojis.gameObject.SetActive(true);
			TurnTimer.gameObject.SetActive(true);
			PlayerNamesForMultiplayer.gameObject.SetActive(true);
			SetMembershipFrames(base.CachedManagers.CachedMultiplayerManager.LeftPlayerSettings.isMembershipPlayer, base.CachedManagers.CachedMultiplayerManager.RightPlayerSettings.isMembershipPlayer);
			break;
		}
		StatisticsManager_OnScoreChanged();
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		LevelTimer_OnTimerChanged();
		for (int i = 0; i < currentEffects.Count; i++)
		{
			UnityEngine.Object.Destroy(currentEffects[i]);
		}
		currentEffects.Clear();
		SetIconPlayer(base.ObjectsSpawner.FirstSelectedCharacterNameType, firstPlayerSprites);
		SetIconPlayer(base.ObjectsSpawner.SecondSelectedCharacterNameType, secondPlayerSprites);
		DistanceMark.SpriteIcon.SetSprite(secondPlayerSprites[0].Collection, secondPlayerSprites[0].spriteId);
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public void SetFirstPlayerHealthFill(float value, bool immediately = false)
	{
		ProgressBar[] array = firstPlayerHealthBars;
		foreach (ProgressBar progressBar in array)
		{
			progressBar.SetFillAmount(value, immediately);
		}
	}

	public void SetSecondPlayerHealthFill(float value, bool immediately = false)
	{
		ProgressBar[] array = secondPlayerHealthBars;
		foreach (ProgressBar progressBar in array)
		{
			progressBar.SetFillAmount(value, immediately);
		}
	}

	public void SetTurnTimerFill(float value, bool immediately = true)
	{
		for (int i = 0; i < TurnTimer.TurnTimerProgressBars.Length; i++)
		{
			TurnTimer.TurnTimerProgressBars[i].SetFillAmount(value, immediately);
		}
	}

	public void ShowFinishHim(bool isMan)
	{
		finishHimControl.gameObject.SetActive(true);
		finishHimAnimation.transform.localScale *= ((base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType != GUILayouterRotationType.Portrait) ? 1f : 0.8f);
		finishHimAnimation.AnimationName = null;
		SystemLanguage currentLanguage = SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage;
		FinishLanguageSkeletonDataAsset finishLanguageSkeletonDataAsset = defaultLanguageAsset;
		if (!defaultLanguageAsset.languages.Contains(currentLanguage))
		{
			for (int i = 0; i < otherLanguagesAssets.Count; i++)
			{
				if (otherLanguagesAssets[i].languages.Contains(currentLanguage))
				{
					finishLanguageSkeletonDataAsset = otherLanguagesAssets[i];
					break;
				}
			}
		}
		SkeletonDataAsset skeletonDataAsset = ((!isMan) ? finishLanguageSkeletonDataAsset.womanSkeletonDataAsset : finishLanguageSkeletonDataAsset.manSkeletonDataAsset);
		if (finishHimAnimation.SkeletonDataAsset != skeletonDataAsset)
		{
			finishHimAnimation.SkeletonDataAsset = skeletonDataAsset;
			finishHimAnimation.Reset();
		}
		finishHimAnimation.AnimationName = "popout";
		finishHimControl.Show(delegate
		{
			finishHimControl.Hide(delegate
			{
				finishHimControl.gameObject.SetActive(false);
			}, false, finishHimShowTime, true);
		});
		AudioManager.PlayAudioWithKey((!isMan) ? "finish_her" : "finish_him");
		AudioManager.PlayAudioWithKey("finish_him_appear");
	}

	private void ShowBrutalityEffect()
	{
		GameObject gameObject = SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.BrutalityHorizontal);
		GameObject gameObject2 = SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.BrutalityVertical);
		AudioManager.PlayAudioWithKey("brutality_sign");
		if (gameObject != null && gameObject2 != null)
		{
			float num = ((Screen.width >= Screen.height) ? Screen.height : Screen.width);
			float num2 = ((Screen.width >= Screen.height) ? Screen.width : Screen.height);
			Vector3 position = base.transform.position + Vector3.up * (num - brutalityEffectHorisontalPositionX * (float)((!tk2dSystem.IsRetina) ? 1 : 2)) + Vector3.right * num2 / 2f;
			GameObject item = UnityEngine.Object.Instantiate(gameObject, position, Quaternion.identity, HorizontalLayouter.CachedTransform);
			brutalityEffects.Add(item);
			position = base.transform.position + Vector3.up * (num2 - brutalityEffectVerticalPositionX * (float)((!tk2dSystem.IsRetina) ? 1 : 2)) + Vector3.right * (num - (float)(20 * ((!tk2dSystem.IsRetina) ? 1 : 2))) / 2f;
			item = UnityEngine.Object.Instantiate(gameObject2, position, Quaternion.identity, VerticalLayouter.CachedTransform);
			brutalityEffects.Add(item);
			brutalityTime = brutalityShakesStartDelay;
			brutalityCount = 0;
			isBrutalityStarted = true;
			cameraBeforeBrutalityPosition = base.CachedManagers.CachedGameManager.CameraController.transform.position;
		}
	}

	public void SetFirstWeapon(WeaponType type)
	{
		if (type != WeaponType.None)
		{
			GameScreenWeapon.IconWeapon.SetSprite(gameWeaponsIcon[type]);
		}
	}

	public void UpdateAppleMisses(int missCount)
	{
		for (int i = 0; i < appleMissesControls.Length; i++)
		{
			if (!appleMissesControls[i].IsAnimating)
			{
				if (i < missCount && !appleMissesControls[i].IsShown)
				{
					appleMissesControls[i].Show();
				}
				else if (i >= missCount && !appleMissesControls[i].IsHidden)
				{
					appleMissesControls[i].Hide();
				}
			}
		}
	}

	public bool IsTouchOnGameField(TouchInfo touch)
	{
		RaycastHit hitInfo;
		Physics.Raycast(base.CachedManagers.CachedGUIScreenManager.UICamera.ScreenCamera.ScreenPointToRay(touch.position), out hitInfo, 1000f, uiLayerMask);
		return hitInfo.collider == gameFieldCollider;
	}

	public void ShowWeapons(bool immediately = false)
	{
		WeaponType[] array = AvailableWeapons();
		if (array != null && array.Length > 0)
		{
			ShowWeaponControl(GameScreenWeapon.ControlWeapon, immediately);
		}
	}

	public void AddCoinsEffect()
	{
		tk2dTextMesh[] array = coinsLabels;
		foreach (tk2dTextMesh t in array)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAchievementAddCoins));
				gameObject.transform.parent = t.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}, 0f);
		}
	}

	public void AddGemsEffect()
	{
		tk2dTextMesh[] array = gemsLabels;
		foreach (tk2dTextMesh t in array)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAddGems));
				gameObject.transform.parent = t.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}, 0f);
		}
	}

	public void PauseBrutalityEffects(bool isPaused)
	{
		if (!isPaused)
		{
			isBrutalityStarted = false;
		}
	}

	public void ShowPartyMastersButton()
	{
		if (partyMastersButton != null)
		{
			partyMastersButton.Show();
		}
	}

	public void ShowPartyMastersAnnouncer()
	{
		if (partyMastersAnnouncer != null)
		{
			SingletonMonoBehaviour<StatisticsManager>.Instance.LastMatchShowedPartyMastersAnnouncer = SingletonMonoBehaviour<StatisticsManager>.Instance.MathesCount + 1;
			partyMastersAnnouncer.Show();
			SingletonMonoBehaviour<Managers>.Instance.CachedScheduler.CallMethodWithDelay(this, delegate
			{
				partyMastersAnnouncer.Hide();
			}, partyMastersAnnouncerHideDelay);
		}
	}

	private void SetIconPlayer(CharacterNameType characterNameType, tk2dBaseSprite[] sprites)
	{
		GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
		if (gUICharacter != null)
		{
			foreach (tk2dBaseSprite tk2dBaseSprite2 in sprites)
			{
				tk2dBaseSprite2.SetSprite(gUICharacter.SpriteCollectionsData[0], gUICharacter.IconShopName);
			}
		}
	}

	private WeaponType[] AvailableWeapons()
	{
		WeaponType[] result = null;
		GameType currentGameType = base.CachedManagers.CachedGameManager.CurrentGameType;
		if (currentGameType == GameType.AppleShoot || currentGameType == GameType.BirdsHunt)
		{
			Level currentLevel = base.CachedManagers.CachedLevelsManager.CurrentLevel;
			if (currentLevel is LevelAppleShoot)
			{
				result = (currentLevel as LevelAppleShoot).AvailableWeapons;
			}
			else if (currentLevel is LevelBirdsHunt)
			{
				result = (currentLevel as LevelBirdsHunt).AvailableWeapons;
			}
		}
		return result;
	}

	private void ShowWeaponControl(GUIControl control, bool immediately = false)
	{
		if (control.IsHidden)
		{
			if (immediately)
			{
				control.ShowImmediately();
			}
			else
			{
				control.Show();
			}
		}
	}

	private void SendEvents()
	{
		if (SingletonMonoBehaviour<Managers>.InstanceIfExist != null)
		{
			string customEventName = ((base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType != GUILayouterRotationType.Landscape) ? "OrientationPortraitOnExit" : "OrientationLandscapeOnExit");
			Analytics.CustomEvent(customEventName);
			Analytics.CustomEvent("OrientationLandscapeTime", new Dictionary<string, object> { 
			{
				"Time",
				(int)landscapeTimer
			} });
			Analytics.CustomEvent("OrientationPortraitTime", new Dictionary<string, object> { 
			{
				"Time",
				(int)portraitTimer
			} });
		}
	}

	private void SetMembershipFrames(bool isFirstPlayerWithFrame, bool isSecondPlayerWithFrame = false)
	{
		for (int i = 0; i < firstCharacterMembershipFrames.Length; i++)
		{
			firstCharacterMembershipFrames[i].SetActive(isFirstPlayerWithFrame);
		}
		for (int j = 0; j < secondCharacterMembershipFrames.Length; j++)
		{
			secondCharacterMembershipFrames[j].SetActive(isSecondPlayerWithFrame);
		}
		for (int k = 0; k < firstCharacterHealthBarsMembershipFrames.Length; k++)
		{
			firstCharacterHealthBarsMembershipFrames[k].SetActive(isFirstPlayerWithFrame);
		}
		for (int l = 0; l < secondCharacterHealthBarsMembershipFrames.Length; l++)
		{
			secondCharacterHealthBarsMembershipFrames[l].SetActive(isSecondPlayerWithFrame);
		}
	}

	private void PauseButton_OnClick()
	{
		if (!base.CachedManagers.CachedGameManager.IsGamePaused)
		{
			if (base.CachedManagers.CachedGameManager.CurrentGameType != GameType.Multiplayer)
			{
				base.CachedManagers.CachedGameManager.IsGamePaused = true;
				base.CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Pause, null, false);
				ADManager.Instance.ShowAD_Black();
			}
			else
			{
				base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Pause);
				ADManager.Instance.ShowAD_White();
			}
		}
	}

	private void StatisticsManager_OnScoreChanged(int obj = 0)
	{
		tk2dTextMesh[] array = scorelabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Score.ToString();
		}
		GUIControl[] array2 = scoreControls;
		foreach (GUIControl gUIControl in array2)
		{
			if (!gUIControl.IsAnimating)
			{
				gUIControl.Show();
			}
		}
	}

	private void StatisticsManager_OnCoinsChanged(int obj = 0)
	{
		tk2dTextMesh[] array = coinsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
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

	private void LevelTimer_OnTimerChanged(float time = 0f)
	{
		TimerSeconds = (int)time;
		string text = Mathf.Floor(time / 60f).ToString("00");
		string text2 = Mathf.Floor(time % 60f).ToString("00");
		string text3 = text + ":" + text2;
		timerLabel.text = text3;
	}

	private void LevelTimer_OnTimeAdd()
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGameAddTime));
			gameObject.transform.parent = timerIcon;
			gameObject.transform.position = timerIcon.position;
			UnityEngine.Object.Destroy(gameObject, 5f);
		}, 0f);
	}

	private void Level_OnPlayerChanged(int player)
	{
		Level currentLevel = base.CachedManagers.CachedLevelsManager.CurrentLevel;
		if ((bool)currentLevel && (bool)currentLevel.ActiveCharacter && currentLevel.ActiveCharacter.Health > 0f && (bool)currentLevel.InactiveArcher && currentLevel.InactiveArcher.Health > 0f)
		{
			GameType currentGameType = base.CachedManagers.CachedGameManager.CurrentGameType;
			if (currentGameType != GameType.Multiplayer)
			{
				turnsLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey((player != 1) ? "ui.game.turn.player_second" : "ui.game.turn.player_first");
				turnsAnimation.State.SetAnimation(0, CachedAnimationTurnsAppear, false);
			}
			else
			{
				string text = base.CachedManagers.CachedMultiplayerManager.LeftPlayerSettings.nickname;
				string text2 = base.CachedManagers.CachedMultiplayerManager.RightPlayerSettings.nickname;
				if (string.IsNullOrEmpty(text))
				{
					text = "GOD";
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = "DEVIL";
				}
				turnsLabel.text = ((player != 1) ? text2 : text);
				turnsAnimation.State.SetAnimation(0, CachedAnimationTurnsAppear, false);
			}
		}
		tk2dBaseSprite[] array = ((player != 1) ? firstPlayerSprites : secondPlayerSprites);
		DistanceMark.SpriteIcon.SetSprite(array[0].Collection, array[0].spriteId);
	}

	private void MicroachievementsManager_OnReachedAchievement(Queue<MicroachievementInfo> queue)
	{
		if (!controlAchievement.IsShown && !controlAchievement.IsAnimating && !base.CachedManagers.CachedLevelsManager.CurrentLevel.IsLevelFinished)
		{
			ShowAchievement(queue);
		}
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType rotationType)
	{
		if (!GameManager.IsIPhoneX)
		{
			return;
		}
		bool flag = rotationType == GUILayouterRotationType.Portrait;
		if (emojis != null)
		{
			emojis.IPhoneXCell.SizeValue = ((!flag) ? leftContentIPhoneXOffset : 0f);
		}
		if (tutorial != null)
		{
			tutorial.IPhoneXHintPortraitCell.SizeValue = ((!flag) ? 0f : portraitTutorialHintIPhoneXOffset);
		}
		if (playerNamesForMultiplayer != null)
		{
			playerNamesForMultiplayer.IPhoneXPortraitCell.SizeValue = ((!flag) ? 0f : (topIPhoneXOffset * 2f));
			for (int i = 0; i < playerNamesForMultiplayer.IPhoneXCells.Length; i++)
			{
				playerNamesForMultiplayer.IPhoneXCells[i].SizeValue = ((!flag) ? landscapeMultiplayerNamesIPhoneXSize : portraitMultiplayerNamesIPhoneXSize);
			}
		}
		birdsTopOffsetCell.SizeValue = ((!flag) ? 0f : (topIPhoneXOffset * 2f));
		birdsLeftOffsetCell.SizeValue = ((!flag) ? 0f : leftBirdsButtonsIPhoneXOffset);
		birdsRightOffsetCell.SizeValue = ((!flag) ? sideButtonsIPhoneXOffset : 0f);
		finishHimIPhoneXCell.SizeValue = ((!flag) ? 0f : finishHimIPhoneXOffset);
		partyMastersButtonIPhoneXCell.SizeValue = ((!flag) ? 0f : partyMastersButtonIPhoneXOffset);
		partyMastersAnnouncerIPhoneXCell.SizeValue = ((!flag) ? partyMastersButtonIPhoneXOffset : 0f);
	}

	private void ShowAchievement(Queue<MicroachievementInfo> queue)
	{
		if (queue.Count <= 0)
		{
			return;
		}
		MicroachievementInfo info = queue.Peek();
		if (!string.IsNullOrEmpty(info.localisationName))
		{
			textAchievement.text = info.LocalisationName();
		}
		if (!string.IsNullOrEmpty(info.CurrentSpriteName()))
		{
			iconAchievement.SetSprite(info.CurrentSpriteName());
		}
		if (info.coinsAward > 0)
		{
			countCellAchievement.gameObject.SetActive(true);
			countLabelAchiviement.text = info.AwardString();
		}
		else
		{
			countCellAchievement.gameObject.SetActive(false);
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
		controlAchievement.Show(delegate
		{
			StartCoroutine(CoroutineOpenAchievement(info));
			controlAchievement.Hide(delegate
			{
				ShowAchievement(queue);
			}, false, timeAchievementShow, true);
		});
	}

	private void TurnsAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data != CachedEventDataAppearEnd)
		{
			return;
		}
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				turnsAnimation.State.SetAnimation(0, CachedAnimationTurnsDisappear, false);
			}
		}, turnsLabelDurationOnScreen);
	}

	private void FatalityBase_OnBrutalityShowed(float delay)
	{
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			ShowBrutalityEffect();
		}, delay);
	}

	private IEnumerator CoroutineOpenAchievement(MicroachievementInfo info)
	{
		if (info.coinsAward > 0)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAchievementSpawnCoins));
				gameObject.transform.parent = countCellAchievement.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}, 0f);
			yield return new WaitForSeconds(delayAddEffect);
			base.CachedManagers.CachedStatisticsManager.AddCoins(info.coinsAward);
			Queue<MicroachievementInfo> achievementsQueue = base.CachedManagers.CachedMicroachievementsManager.QueueAhievements;
			if (achievementsQueue.Count > 0)
			{
				achievementsQueue.Dequeue();
			}
			AddCoinsEffect();
		}
		if (info.gemsAward > 0)
		{
			base.CachedManagers.CachedStatisticsManager.AddGems(info.gemsAward);
			AddGemsEffect();
		}
	}

	private void OnRewardReceived(bool isCoinsReward)
	{
		if (isCoinsReward)
		{
			AddCoinsEffect();
		}
		else
		{
			AddGemsEffect();
		}
	}

	public override void BackButtonPressed()
	{
		if (tutorial != null && tutorial.IsTutorialAnimationPlaying)
		{
			tutorial.BackButtonPressed();
		}
		else
		{
			PauseButton_OnClick();
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(0f, 300f, 100f, 100f), "Turns"))
		{
			turnsAnimation.State.SetAnimation(0, CachedAnimationTurnsAppear, false);
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
	}
}
