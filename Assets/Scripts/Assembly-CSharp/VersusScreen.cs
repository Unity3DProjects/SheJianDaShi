using System;
using System.Collections.Generic;
using UnityEngine;

public class VersusScreen : GUIScreen
{
	private enum VSScreenTournamentState
	{
		None = 0,
		BigShift = 1,
		LittleShift = 2,
		EnemyDefeat = 3,
		EnemyEnable = 4,
		VSAnimation = 5,
		HideScreen = 6
	}

	private const string AVATAR_BIRDS = "avatar_birds";

	private const string TEXT_BIRDS = "ui.versus.label.birds";

	private const string AVATAR_APPLE = "avatar_apple";

	private const string TEXT_APPLE = "ui.versus.label.apple";

	[SerializeField]
	private tk2dBaseSprite[] firstCharacterSprites;

	[SerializeField]
	private GameObject[] firstCharacterMembershipFrames;

	[SerializeField]
	private tk2dTextMesh[] firstCharacterLabels;

	[SerializeField]
	private SkeletonAnimation[] firstCharacterAnimations;

	[SerializeField]
	private tk2dBaseSprite[] firstCharacterFlagSprites;

	[SerializeField]
	private tk2dBaseSprite[] firstCharacterMultiplayerBackSprites;

	[SerializeField]
	private tk2dBaseSprite[] firstCharacterMultiplayerFlagSprites;

	[SerializeField]
	private tk2dTextMesh[] firstCharacterMultiplayerLabels;

	[SerializeField]
	private tk2dBaseSprite[] secondCharacterSprites;

	[SerializeField]
	private GameObject[] secondCharacterMembershipFrames;

	[SerializeField]
	private tk2dTextMesh[] secondCharacterLabels;

	[SerializeField]
	private SkeletonAnimation[] secondCharacterAnimations;

	[SerializeField]
	private tk2dBaseSprite[] secondCharacterFlagSprites;

	[SerializeField]
	private tk2dBaseSprite[] secondCharacterMultiplayerBackSprites;

	[SerializeField]
	private tk2dBaseSprite[] secondCharacterMultiplayerFlagSprites;

	[SerializeField]
	private tk2dTextMesh[] secondCharacterMultiplayerLabels;

	[SerializeField]
	private SkeletonAnimation[] versusAnimations;

	[SerializeField]
	private LanguageSkeletonDataAsset defaultLanguageAsset;

	[SerializeField]
	private List<LanguageSkeletonDataAsset> otherLanguagesAssets;

	[SerializeField]
	private GameObject[] landscapeBackAnimations;

	[SerializeField]
	private GameObject[] portraitBackAnimations;

	[SerializeField]
	private float timeShow = 1f;

	[SerializeField]
	private float delayFirst;

	[SerializeField]
	private float delayVersus;

	[SerializeField]
	private float delaySecond;

	[SerializeField]
	private float delayVSAppearSound;

	[SerializeField]
	private GUITournamentAvatar tournamentAvatarPrefab;

	[SerializeField]
	private GUILayoutCell[] firstPlayerHandlerCell;

	[SerializeField]
	private Transform[] firstPlayerContainer;

	[SerializeField]
	private GUILayoutCell secondPlayerLandscapeHandlerCell;

	[SerializeField]
	private Transform secondPlayerLandscapeContainer;

	[SerializeField]
	private GUILayoutCell secondPlayerPortraitHandlerCell;

	[SerializeField]
	private Transform secondPlayerPortraitContainer;

	[SerializeField]
	private float avatarsIntervalLandscape;

	[SerializeField]
	private float avatarsIntervalPortrait;

	[SerializeField]
	private TweenPosition secondPlayerLandscapeTweener;

	[SerializeField]
	private TweenPosition secondPlayerPortraitTweener;

	[SerializeField]
	private float bigShiftEasyDuration;

	[SerializeField]
	private float bigShiftMediumDuration;

	[SerializeField]
	private float bigShiftHardDuration;

	[SerializeField]
	private AnimationCurve bigShiftCurve;

	[SerializeField]
	private float smallShiftDuration;

	[SerializeField]
	private AnimationCurve smallShiftCurve;

	[SerializeField]
	private float tournamentAfterEffectsTimeShow = 1f;

	[SerializeField]
	private float vsAnimationStateDuration = 2f;

	private bool isShowing;

	private ObjectsSpawner cachedSpawner;

	private float leftTime;

	private VSScreenTournamentState vsTournamentState;

	private List<GUITournamentAvatar> currentEnemy = new List<GUITournamentAvatar>();

	private List<GUITournamentAvatar> previousEnemy = new List<GUITournamentAvatar>();

	private List<Tweener> tournamentTweeners = new List<Tweener>();

	private bool isTournamentVS;

	private int oldBigShiftAvatarNumber;

	private bool isNeedPlayAppearEnemySound;

	private bool isNeedPlayAppearVSSound;

	private VSScreenTournamentState VSTournamentState
	{
		get
		{
			return vsTournamentState;
		}
		set
		{
			if (vsTournamentState == value)
			{
				return;
			}
			vsTournamentState = value;
			switch (vsTournamentState)
			{
			case VSScreenTournamentState.BigShift:
			{
				int count = base.CachedManagers.CachedTournamentManager.Enemies.Count;
				Vector3 beginPosition3 = Vector3.down * (count - 1) * avatarsIntervalLandscape;
				Vector3 beginPosition4 = Vector3.left * (count - 1) * avatarsIntervalPortrait;
				TournamentDifficultType difficultType = base.CachedManagers.CachedTournamentManager.CurrentTournament.settings.difficultType;
				float duration = 0f;
				switch (difficultType)
				{
				case TournamentDifficultType.Easy:
					duration = bigShiftEasyDuration;
					break;
				case TournamentDifficultType.Medium:
					duration = bigShiftMediumDuration;
					break;
				case TournamentDifficultType.Hard:
					duration = bigShiftHardDuration;
					break;
				}
				Action onFinished2 = delegate
				{
					VSTournamentState = VSScreenTournamentState.VSAnimation;
				};
				oldBigShiftAvatarNumber = count - 2;
				MoveSecondPlayerTower(secondPlayerLandscapeTweener, beginPosition3, Vector3.zero, duration, bigShiftCurve, onFinished2);
				MoveSecondPlayerTower(secondPlayerPortraitTweener, beginPosition4, Vector3.zero, duration, bigShiftCurve, onFinished2);
				break;
			}
			case VSScreenTournamentState.LittleShift:
			{
				Vector3 beginPosition = Vector3.up * avatarsIntervalLandscape;
				Vector3 beginPosition2 = Vector3.right * avatarsIntervalPortrait;
				Action onFinished = delegate
				{
					VSTournamentState = VSScreenTournamentState.VSAnimation;
				};
				MoveSecondPlayerTower(secondPlayerLandscapeTweener, beginPosition, Vector3.zero, smallShiftDuration, smallShiftCurve, onFinished);
				MoveSecondPlayerTower(secondPlayerPortraitTweener, beginPosition2, Vector3.zero, smallShiftDuration, smallShiftCurve, onFinished);
				base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						AudioManager.PlayAudioWithKey("vs_screen_little_shift");
					}
				}, 0.4f);
				break;
			}
			case VSScreenTournamentState.EnemyEnable:
			{
				for (int num2 = 0; num2 < currentEnemy.Count; num2++)
				{
					currentEnemy[num2].EnableAvatar(false, delegate
					{
						VSTournamentState = VSScreenTournamentState.HideScreen;
					});
				}
				break;
			}
			case VSScreenTournamentState.EnemyDefeat:
			{
				Vector3 vector = Vector3.up * avatarsIntervalLandscape;
				Vector3 vector2 = Vector3.right * avatarsIntervalPortrait;
				MoveSecondPlayerTower(secondPlayerLandscapeTweener, vector, vector, smallShiftDuration, smallShiftCurve, null, true);
				MoveSecondPlayerTower(secondPlayerPortraitTweener, vector2, vector2, smallShiftDuration, smallShiftCurve, null, true);
				for (int num = 0; num < previousEnemy.Count; num++)
				{
					previousEnemy[num].DefeatAvatar(false, delegate
					{
						VSTournamentState = VSScreenTournamentState.LittleShift;
					});
				}
				break;
			}
			case VSScreenTournamentState.VSAnimation:
				base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						VSTournamentState = VSScreenTournamentState.EnemyEnable;
					}
				}, vsAnimationStateDuration);
				break;
			case VSScreenTournamentState.HideScreen:
				break;
			}
		}
	}

	private bool IsNeedPlayAppearVSSound
	{
		set
		{
			if (isNeedPlayAppearVSSound != value)
			{
				isNeedPlayAppearVSSound = value;
				if (isNeedPlayAppearVSSound)
				{
					AudioManager.PlayAudioWithKey("vs_screen_little_shift");
				}
			}
		}
	}

	private void Awake()
	{
		base.CachedManagers.CachedGameManager.IsGamePaused = false;
		if (tk2dSystem.IsRetina)
		{
			avatarsIntervalLandscape *= 2f;
			avatarsIntervalPortrait *= 2f;
		}
		InitializeCharacters();
		List<ShopItemCharacter> availableCharacters = base.CachedManagers.CachedShopManager.AvailableCharacters;
		for (int i = 0; i < availableCharacters.Count; i++)
		{
			CharacterNameType characterNameType = ((availableCharacters[i].CharacterUpgradeInfo.upgradeLevel <= 0) ? availableCharacters[i].Infos[0].characterNameType : availableCharacters[i].CharacterUpgradeInfo.characterNameType);
			GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
			gUICharacter.ClearAll();
		}
		SystemLanguage currentLanguage = SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage;
		LanguageSkeletonDataAsset languageSkeletonDataAsset = defaultLanguageAsset;
		if (!defaultLanguageAsset.languages.Contains(currentLanguage))
		{
			for (int j = 0; j < otherLanguagesAssets.Count; j++)
			{
				if (otherLanguagesAssets[j].languages.Contains(currentLanguage))
				{
					languageSkeletonDataAsset = otherLanguagesAssets[j];
					break;
				}
			}
		}
		SkeletonAnimation[] array = versusAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			if (skeletonAnimation.SkeletonDataAsset != languageSkeletonDataAsset.skeletonDataAsset)
			{
				skeletonAnimation.SkeletonDataAsset = languageSkeletonDataAsset.skeletonDataAsset;
				skeletonAnimation.Reset();
			}
		}
	}

	private void OnEnable()
	{
		if (tk2dSystem.IsRetina)
		{
			GameObject[] array = landscapeBackAnimations;
			foreach (GameObject gameObject in array)
			{
				gameObject.transform.localScale = Vector3.one * 2f;
			}
			GameObject[] array2 = portraitBackAnimations;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.transform.localScale = new Vector3(3f, 2f, 1f);
			}
		}
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
	}

	private void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
	}

	private void Update()
	{
		if ((!isTournamentVS && isShowing) || VSTournamentState == VSScreenTournamentState.HideScreen)
		{
			leftTime -= Time.deltaTime;
			if (leftTime < 0f)
			{
				isShowing = false;
				VSTournamentState = VSScreenTournamentState.None;
				base.CachedManagers.CachedGameManager.StartGame();
			}
		}
		if (!isTournamentVS)
		{
			if (!isShowing || !(leftTime > 0f))
			{
				return;
			}
			float num = timeShow - leftTime;
			if (num >= delayFirst)
			{
				SkeletonAnimation[] array = firstCharacterAnimations;
				foreach (SkeletonAnimation skeletonAnimation in array)
				{
					skeletonAnimation.Update(Time.deltaTime);
				}
			}
			if (num >= delaySecond)
			{
				SkeletonAnimation[] array2 = secondCharacterAnimations;
				foreach (SkeletonAnimation skeletonAnimation2 in array2)
				{
					skeletonAnimation2.Update(Time.deltaTime);
				}
			}
			if (num >= delayVersus)
			{
				SkeletonAnimation[] array3 = versusAnimations;
				foreach (SkeletonAnimation skeletonAnimation3 in array3)
				{
					skeletonAnimation3.Update(Time.deltaTime);
				}
			}
			if (num >= delayVSAppearSound)
			{
				IsNeedPlayAppearVSSound = true;
			}
			return;
		}
		if (VSTournamentState == VSScreenTournamentState.VSAnimation)
		{
			SkeletonAnimation[] array4 = versusAnimations;
			foreach (SkeletonAnimation skeletonAnimation4 in array4)
			{
				skeletonAnimation4.Update(Time.deltaTime);
			}
		}
		else if (VSTournamentState == VSScreenTournamentState.BigShift)
		{
			int num2 = (int)Mathf.Abs(secondPlayerLandscapeContainer.localPosition.y / avatarsIntervalLandscape);
			if (num2 < oldBigShiftAvatarNumber)
			{
				AudioManager.PlayAudioWithKey("vs_screen_tower_shift");
				oldBigShiftAvatarNumber = num2;
			}
		}
		for (int m = 0; m < tournamentTweeners.Count; m++)
		{
			if (!tournamentTweeners[m].gameObject.activeInHierarchy && tournamentTweeners[m].enabled)
			{
				tournamentTweeners[m].Update();
			}
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = delegate(GUIControl cont)
		{
			isShowing = true;
			leftTime = ((!isTournamentVS) ? timeShow : tournamentAfterEffectsTimeShow);
			if (onFinished != null)
			{
				onFinished(cont);
			}
			AudioManager.PlayAudioWithKey("vs_screen_appear");
			AudioManager.StopAudioWithKey("crowd_loop", 1f);
		};
		base.Show(onFinished2, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SkeletonAnimation[] array = firstCharacterAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			if (skeletonAnimation.Skeleton != null)
			{
				skeletonAnimation.Skeleton.SetToSetupPose();
				skeletonAnimation.ForceUpdate();
			}
		}
		SkeletonAnimation[] array2 = secondCharacterAnimations;
		foreach (SkeletonAnimation skeletonAnimation2 in array2)
		{
			if (skeletonAnimation2.Skeleton != null)
			{
				skeletonAnimation2.Skeleton.SetToSetupPose();
				skeletonAnimation2.ForceUpdate();
			}
		}
		SkeletonAnimation[] array3 = versusAnimations;
		foreach (SkeletonAnimation skeletonAnimation3 in array3)
		{
			if (skeletonAnimation3.Skeleton != null)
			{
				skeletonAnimation3.Skeleton.SetToSetupPose();
				skeletonAnimation3.ForceUpdate();
			}
		}
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void InitializeCharacters()
	{
		GameType currentGameType = base.CachedManagers.CachedGameManager.CurrentGameType;
		isTournamentVS = currentGameType == GameType.Tournament;
		bool active = currentGameType == GameType.Multiplayer;
		SkeletonAnimation[] array = firstCharacterAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			skeletonAnimation.gameObject.SetActive(!isTournamentVS);
		}
		SkeletonAnimation[] array2 = secondCharacterAnimations;
		foreach (SkeletonAnimation skeletonAnimation2 in array2)
		{
			skeletonAnimation2.gameObject.SetActive(!isTournamentVS);
		}
		tk2dBaseSprite[] array3 = firstCharacterMultiplayerBackSprites;
		foreach (tk2dBaseSprite tk2dBaseSprite2 in array3)
		{
			tk2dBaseSprite2.gameObject.SetActive(active);
		}
		tk2dBaseSprite[] array4 = secondCharacterMultiplayerBackSprites;
		foreach (tk2dBaseSprite tk2dBaseSprite3 in array4)
		{
			tk2dBaseSprite3.gameObject.SetActive(active);
		}
		tk2dBaseSprite[] array5 = firstCharacterMultiplayerFlagSprites;
		foreach (tk2dBaseSprite tk2dBaseSprite4 in array5)
		{
			tk2dBaseSprite4.gameObject.SetActive(active);
		}
		tk2dBaseSprite[] array6 = secondCharacterMultiplayerFlagSprites;
		foreach (tk2dBaseSprite tk2dBaseSprite5 in array6)
		{
			tk2dBaseSprite5.gameObject.SetActive(active);
		}
		tk2dTextMesh[] array7 = firstCharacterMultiplayerLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array7)
		{
			tk2dTextMesh2.gameObject.SetActive(active);
		}
		tk2dTextMesh[] array8 = secondCharacterMultiplayerLabels;
		foreach (tk2dTextMesh tk2dTextMesh3 in array8)
		{
			tk2dTextMesh3.gameObject.SetActive(active);
		}
		if (!isTournamentVS)
		{
			string text = string.Empty;
			string text2 = null;
			string text3 = string.Empty;
			string text4 = null;
			CharacterNameType firstSelectedCharacterNameType = base.ObjectsSpawner.FirstSelectedCharacterNameType;
			GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(firstSelectedCharacterNameType);
			if (gUICharacter != null)
			{
				text = gUICharacter.IconAvatarName;
				text2 = base.ObjectsSpawner.GetCharacterLocalizedName(firstSelectedCharacterNameType);
			}
			CharacterNameType secondSelectedCharacterNameType = base.ObjectsSpawner.SecondSelectedCharacterNameType;
			GUICharacter gUICharacter2 = base.ObjectsSpawner.GetGUICharacter(base.ObjectsSpawner.SecondSelectedCharacterNameType);
			if (gUICharacter2 != null)
			{
				text3 = gUICharacter2.IconAvatarName;
				text4 = base.ObjectsSpawner.GetCharacterLocalizedName(secondSelectedCharacterNameType);
			}
			switch (currentGameType)
			{
			case GameType.Multiplayer:
			{
				text2 = base.CachedManagers.CachedMultiplayerManager.LeftPlayerSettings.nickname;
				text4 = base.CachedManagers.CachedMultiplayerManager.RightPlayerSettings.nickname;
				tk2dBaseSprite[] array9 = firstCharacterFlagSprites;
				foreach (tk2dBaseSprite tk2dBaseSprite6 in array9)
				{
					tk2dBaseSprite6.gameObject.SetActive(false);
				}
				tk2dBaseSprite[] array10 = secondCharacterFlagSprites;
				foreach (tk2dBaseSprite tk2dBaseSprite7 in array10)
				{
					tk2dBaseSprite7.gameObject.SetActive(false);
				}
				tk2dTextMesh[] array11 = firstCharacterLabels;
				foreach (tk2dTextMesh tk2dTextMesh4 in array11)
				{
					tk2dTextMesh4.gameObject.SetActive(false);
				}
				tk2dTextMesh[] array12 = secondCharacterLabels;
				foreach (tk2dTextMesh tk2dTextMesh5 in array12)
				{
					tk2dTextMesh5.gameObject.SetActive(false);
				}
				tk2dBaseSprite[] array13 = firstCharacterMultiplayerFlagSprites;
				foreach (tk2dBaseSprite tk2dBaseSprite8 in array13)
				{
					string countryCode = base.CachedManagers.CachedMultiplayerManager.LeftPlayerSettings.countryCode;
					if (!string.IsNullOrEmpty(countryCode) && tk2dBaseSprite8.Collection.inst.GetSpriteDefinition(countryCode) != null)
					{
						tk2dBaseSprite8.SetSprite(countryCode);
					}
					else
					{
						tk2dBaseSprite8.SetSprite("XX");
					}
				}
				tk2dBaseSprite[] array14 = secondCharacterMultiplayerFlagSprites;
				foreach (tk2dBaseSprite tk2dBaseSprite9 in array14)
				{
					string countryCode2 = base.CachedManagers.CachedMultiplayerManager.RightPlayerSettings.countryCode;
					if (!string.IsNullOrEmpty(countryCode2) && tk2dBaseSprite9.Collection.inst.GetSpriteDefinition(countryCode2) != null)
					{
						tk2dBaseSprite9.SetSprite(countryCode2);
					}
					else
					{
						tk2dBaseSprite9.SetSprite("XX");
					}
				}
				SetMembershipFrames(base.CachedManagers.CachedMultiplayerManager.LeftPlayerSettings.isMembershipPlayer, base.CachedManagers.CachedMultiplayerManager.RightPlayerSettings.isMembershipPlayer);
				break;
			}
			case GameType.VSComputer:
				SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
				break;
			case GameType.VSHuman:
				SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive, base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
				break;
			case GameType.AppleShoot:
				text3 = "avatar_apple";
				text4 = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.versus.label.apple");
				SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
				break;
			case GameType.BirdsHunt:
				text3 = "avatar_birds";
				text4 = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.versus.label.birds");
				SetMembershipFrames(base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive);
				break;
			}
			if (!string.IsNullOrEmpty(text) && (bool)gUICharacter)
			{
				tk2dBaseSprite[] array15 = firstCharacterSprites;
				foreach (tk2dBaseSprite tk2dBaseSprite10 in array15)
				{
					if (currentGameType != GameType.AppleShoot && currentGameType != GameType.BirdsHunt)
					{
						tk2dBaseSprite10.SetSprite(gUICharacter.SpriteCollectionsData[0], text);
					}
					else
					{
						tk2dBaseSprite10.SetSprite(text);
					}
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				tk2dTextMesh[] array16 = firstCharacterLabels;
				foreach (tk2dTextMesh tk2dTextMesh6 in array16)
				{
					tk2dTextMesh6.text = text2;
				}
				tk2dTextMesh[] array17 = firstCharacterMultiplayerLabels;
				foreach (tk2dTextMesh tk2dTextMesh7 in array17)
				{
					tk2dTextMesh7.text = text2;
				}
			}
			if (!string.IsNullOrEmpty(text3) && gUICharacter2 != null)
			{
				tk2dBaseSprite[] array18 = secondCharacterSprites;
				foreach (tk2dBaseSprite tk2dBaseSprite11 in array18)
				{
					if (currentGameType != GameType.AppleShoot && currentGameType != GameType.BirdsHunt)
					{
						tk2dBaseSprite11.SetSprite(gUICharacter2.SpriteCollectionsData[0], text3);
					}
					else
					{
						tk2dBaseSprite11.SetSprite(text3);
					}
				}
			}
			if (!string.IsNullOrEmpty(text4))
			{
				tk2dTextMesh[] array19 = secondCharacterLabels;
				foreach (tk2dTextMesh tk2dTextMesh8 in array19)
				{
					tk2dTextMesh8.text = text4;
				}
				tk2dTextMesh[] array20 = secondCharacterMultiplayerLabels;
				foreach (tk2dTextMesh tk2dTextMesh9 in array20)
				{
					tk2dTextMesh9.text = text4;
				}
			}
			SkeletonAnimation[] array21 = firstCharacterAnimations;
			foreach (SkeletonAnimation skeletonAnimation3 in array21)
			{
				skeletonAnimation3.Skeleton.SetToSetupPose();
				skeletonAnimation3.State.SetAnimation(0, skeletonAnimation3.AnimationName, false);
				skeletonAnimation3.ForceUpdate();
			}
			SkeletonAnimation[] array22 = secondCharacterAnimations;
			foreach (SkeletonAnimation skeletonAnimation4 in array22)
			{
				skeletonAnimation4.Skeleton.SetToSetupPose();
				skeletonAnimation4.State.SetAnimation(0, skeletonAnimation4.AnimationName, false);
				skeletonAnimation4.ForceUpdate();
			}
		}
		else
		{
			TournamentManager cachedTournamentManager = base.CachedManagers.CachedTournamentManager;
			for (int num17 = 0; num17 < firstPlayerHandlerCell.Length && num17 < firstPlayerContainer.Length; num17++)
			{
				InitializeTournamentAvatar(firstPlayerHandlerCell[num17], firstPlayerContainer[num17], cachedTournamentManager.PlayerCharacter, cachedTournamentManager.CurrentTournament.currentEnemyNumber, false);
			}
			for (int num18 = 0; num18 < cachedTournamentManager.Enemies.Count; num18++)
			{
				InitializeTournamentAvatar(secondPlayerLandscapeHandlerCell, secondPlayerLandscapeContainer, cachedTournamentManager.Enemies[num18], num18);
				InitializeTournamentAvatar(secondPlayerPortraitHandlerCell, secondPlayerPortraitContainer, cachedTournamentManager.Enemies[num18], num18, true, false);
			}
			tournamentTweeners.Add(secondPlayerLandscapeTweener);
			tournamentTweeners.Add(secondPlayerPortraitTweener);
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
			{
				VSTournamentState = ((previousEnemy.Count == 0) ? VSScreenTournamentState.BigShift : VSScreenTournamentState.EnemyDefeat);
			}, 0f);
		}
		SkeletonAnimation[] array23 = versusAnimations;
		foreach (SkeletonAnimation skeletonAnimation5 in array23)
		{
			skeletonAnimation5.Skeleton.SetToSetupPose();
			skeletonAnimation5.State.SetAnimation(0, skeletonAnimation5.AnimationName, false);
			skeletonAnimation5.ForceUpdate();
		}
	}

	private void InitializeTournamentAvatar(GUILayoutCell handlerCell, Transform parentTransform, CharacterNameType character, int number, bool isNumberActive = true, bool isLandscape = true)
	{
		int currentEnemyNumber = base.CachedManagers.CachedTournamentManager.CurrentTournament.currentEnemyNumber;
		GUITournamentAvatar gUITournamentAvatar = UnityEngine.Object.Instantiate(tournamentAvatarPrefab);
		gUITournamentAvatar.Init(handlerCell, character, number, isNumberActive, isLandscape);
		gUITournamentAvatar.transform.parent = parentTransform;
		gUITournamentAvatar.gameObject.SetActive(true);
		float num = number - currentEnemyNumber;
		num *= ((!isLandscape) ? avatarsIntervalPortrait : avatarsIntervalLandscape);
		gUITournamentAvatar.transform.localPosition = ((!isLandscape) ? Vector3.right : Vector3.up) * num;
		if (isNumberActive)
		{
			if (number == currentEnemyNumber)
			{
				currentEnemy.Add(gUITournamentAvatar);
			}
			else if (number == currentEnemyNumber - 1)
			{
				previousEnemy.Add(gUITournamentAvatar);
			}
		}
		tournamentTweeners.AddRange(gUITournamentAvatar.Tweeners);
	}

	private void MoveSecondPlayerTower(TweenPosition tweener, Vector3 beginPosition, Vector3 endPosition, float duration, AnimationCurve curve, Action onFinished, bool isImmediately = false)
	{
		if (isImmediately)
		{
			tweener.transform.localPosition = endPosition;
			if (onFinished != null)
			{
				onFinished();
			}
			return;
		}
		tweener.beginPosition = beginPosition;
		tweener.endPosition = endPosition;
		tweener.ResetTween();
		tweener.duration = duration;
		tweener.animationCurve = curve;
		tweener.SetOnFinishedDelegate(delegate
		{
			if (onFinished != null)
			{
				onFinished();
			}
		});
		tweener.enabled = true;
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
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 200, 0f, 100f, 100f), "Big shift"))
		{
			VSTournamentState = VSScreenTournamentState.BigShift;
		}
	}
}
