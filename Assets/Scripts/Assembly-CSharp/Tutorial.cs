using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	private const string TUTORIAL_EVENT_END = "Tutorial_End";

	private const string TUTORIAL_EVENT_AIMING_END = "Aiming_End";

	private const string TUTORIAL_EVENT_SHOOTING_END = "Shooting_End";

	private const string HINT_EVENT_APPEAR_END = "Appear_End";

	private const string HINT_EVENT_DISAPPEAR_END = "Disappear_End";

	private const string ANIMATION_TUTORIAL = "tutorial";

	private const string ANIMATION_SHOOT_WITHOUT_CARDS = "shoot_1_no_cards";

	private const string ANIMATION_VERSUS_GAME = "shoot_2";

	private const string ANIMATION_APPLE_GAME = "shoot_2_apple";

	private const string ANIMATION_BIRDS_GAME = "shoot_2_bird";

	private const string ANIMATION_HINT_APPEAR = "appear";

	private const string ANIMATION_HINT_DISAPPEAR = "disappear";

	private const string ANIMATION_HINT_IDLE = "idle";

	private const string CONGRATULATIONS_TOP_LABEL = "ui.tutorial.congratulations.toplabel";

	private const string CONGRATULATIONS_BOTTOM_LABEL = "ui.tutorial.congratulations.bottomlabel";

	[SerializeField]
	private TutorialAnimationInfo[] tutorialAnimationInfo;

	[SerializeField]
	private TweenColor tutorialFadeTweener;

	[SerializeField]
	private TweenPosition[] tutorialAnimationTweeners;

	[SerializeField]
	private SkeletonAnimation[] tutorialAnimations;

	[SerializeField]
	private tk2dUIItem[] tutorialCloseButtons;

	[SerializeField]
	private BoxCollider[] tutorialCloseColliders;

	[SerializeField]
	private TweenPosition[] tutorialCloseTweeners;

	[SerializeField]
	private float appearDuration = 0.5f;

	[SerializeField]
	private float disappearDuration = 0.5f;

	[SerializeField]
	[Range(0f, 1f)]
	private float maxFadeAlpha = 0.5f;

	[SerializeField]
	private SkeletonAnimation hintAnimation;

	[SerializeField]
	private tk2dSprite hintWeaponSprite;

	[SerializeField]
	private tk2dTextMesh hintLabel;

	[SerializeField]
	private tk2dUIItem hintButton;

	[SerializeField]
	private TweenScale congratulationsTweener;

	[SerializeField]
	private float congratulationsDuration;

	[SerializeField]
	private tk2dTextMesh congratulationsTopLabel;

	[SerializeField]
	private tk2dTextMesh congratulationsBottomLabel;

	[SerializeField]
	private float tutorialAnimationTimeScale = 1.15f;

	[SerializeField]
	private GUILayoutCell[] iPhoneXCells;

	[SerializeField]
	private GUILayoutCell iPhoneXHintPortraitCell;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnTutorialWillAppear;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnTutorialWillClose;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnTutorialClose;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnCongratulationsDisappear;

	private EventData cachedEventTutorialEnd;

	private EventData cachedEventTutorialAimingEnd;

	private EventData cachedEventTutorialShootingEnd;

	private EventData cachedEventHintAppearEnd;

	private EventData cachedEventHintDisappearEnd;

	private bool isTutorialAnimationPlaying;

	private bool isHintAnimationPlaying;

	private bool isShowTutorialAfterHintDisappear;

	private TutorialType hintType;

	private float congratulationsTime;

	private bool isCongratulationsOnScreen;

	private Managers cachedManagers;

	public GUILayoutCell IPhoneXHintPortraitCell
	{
		get
		{
			return iPhoneXHintPortraitCell;
		}
	}

	public GUILayoutCell[] IPhoneXCells
	{
		get
		{
			return iPhoneXCells;
		}
	}

	private EventData CachedEventTutorialEnd
	{
		get
		{
			if (cachedEventTutorialEnd == null && tutorialAnimations.Length > 0 && (bool)tutorialAnimations[0].SkeletonDataAsset)
			{
				cachedEventTutorialEnd = tutorialAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Tutorial_End");
			}
			return cachedEventTutorialEnd;
		}
	}

	private EventData CachedEventTutorialAimingEnd
	{
		get
		{
			if (cachedEventTutorialAimingEnd == null && tutorialAnimations.Length > 0 && (bool)tutorialAnimations[0].SkeletonDataAsset)
			{
				cachedEventTutorialAimingEnd = tutorialAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Aiming_End");
			}
			return cachedEventTutorialAimingEnd;
		}
	}

	private EventData CachedEventTutorialShootingEnd
	{
		get
		{
			if (cachedEventTutorialShootingEnd == null && tutorialAnimations.Length > 0 && (bool)tutorialAnimations[0].SkeletonDataAsset)
			{
				cachedEventTutorialShootingEnd = tutorialAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Shooting_End");
			}
			return cachedEventTutorialShootingEnd;
		}
	}

	private EventData CachedEventHintDisappearEnd
	{
		get
		{
			if (cachedEventHintDisappearEnd == null && (bool)hintAnimation.SkeletonDataAsset)
			{
				cachedEventHintDisappearEnd = hintAnimation.SkeletonDataAsset.GetSkeletonData(true).FindEvent("Disappear_End");
			}
			return cachedEventHintDisappearEnd;
		}
	}

	private EventData CachedEventHintAppearEnd
	{
		get
		{
			if (cachedEventHintAppearEnd == null && (bool)hintAnimation.SkeletonDataAsset)
			{
				cachedEventHintAppearEnd = hintAnimation.SkeletonDataAsset.GetSkeletonData(true).FindEvent("Appear_End");
			}
			return cachedEventHintAppearEnd;
		}
	}

	private bool IsHintAnimationPlaying
	{
		get
		{
			return isHintAnimationPlaying;
		}
		set
		{
			if (isHintAnimationPlaying != value)
			{
				isHintAnimationPlaying = value;
				hintAnimation.gameObject.SetActive(value);
			}
		}
	}

	public bool IsTutorialAnimationPlaying
	{
		get
		{
			return isTutorialAnimationPlaying;
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

	public static event Action OnTutorialWillAppear
	{
		add
		{
			Action action = Tutorial.m_OnTutorialWillAppear;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnTutorialWillAppear, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = Tutorial.m_OnTutorialWillAppear;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnTutorialWillAppear, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnTutorialWillClose
	{
		add
		{
			Action action = Tutorial.m_OnTutorialWillClose;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnTutorialWillClose, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = Tutorial.m_OnTutorialWillClose;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnTutorialWillClose, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnTutorialClose
	{
		add
		{
			Action action = Tutorial.m_OnTutorialClose;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnTutorialClose, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = Tutorial.m_OnTutorialClose;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnTutorialClose, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnCongratulationsDisappear
	{
		add
		{
			Action action = Tutorial.m_OnCongratulationsDisappear;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnCongratulationsDisappear, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = Tutorial.m_OnCongratulationsDisappear;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Tutorial.m_OnCongratulationsDisappear, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		if (tk2dSystem.IsRetina)
		{
			Vector3 localPosition = hintLabel.transform.localPosition;
			hintLabel.transform.localPosition = new Vector3(localPosition.x * 2f, localPosition.y, localPosition.z);
			hintAnimation.transform.localPosition *= 2f;
			hintButton.transform.localPosition *= 2f;
		}
		tk2dUIItem[] array = tutorialCloseButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dTextMesh componentInChildren = tk2dUIItem2.GetComponentInChildren<tk2dTextMesh>();
			componentInChildren.text = "ui.game.tutorial.close";
		}
	}

	private void OnEnable()
	{
		tk2dUIItem[] array = tutorialCloseButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += TutorialButton_OnClick;
		}
		SkeletonAnimation[] array2 = tutorialAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array2)
		{
			if (skeletonAnimation != null && skeletonAnimation.State != null)
			{
				skeletonAnimation.State.Event += TutorialAnimation_Event;
			}
		}
		hintAnimation.State.Event += HintAnimation_Event;
		hintButton.OnClick += HintButton_OnClick;
		hintAnimation.gameObject.SetActive(false);
		congratulationsTopLabel.text = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("ui.tutorial.congratulations.toplabel");
		congratulationsBottomLabel.text = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("ui.tutorial.congratulations.bottomlabel");
	}

	private void OnDisable()
	{
		tk2dUIItem[] array = tutorialCloseButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= TutorialButton_OnClick;
		}
		SkeletonAnimation[] array2 = tutorialAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array2)
		{
			if (skeletonAnimation != null && skeletonAnimation.State != null)
			{
				skeletonAnimation.State.Event -= TutorialAnimation_Event;
			}
		}
		hintAnimation.State.Event -= HintAnimation_Event;
		hintButton.OnClick -= HintButton_OnClick;
	}

	private void Update()
	{
		if (isTutorialAnimationPlaying)
		{
			SkeletonAnimation[] array = tutorialAnimations;
			foreach (SkeletonAnimation skeletonAnimation in array)
			{
				skeletonAnimation.Update(Time.unscaledDeltaTime * tutorialAnimationTimeScale);
			}
		}
		if (IsHintAnimationPlaying)
		{
			hintAnimation.Update(Time.unscaledDeltaTime);
		}
		if (isCongratulationsOnScreen)
		{
			congratulationsTime += Time.unscaledDeltaTime;
			if (congratulationsTime > congratulationsDuration)
			{
				congratulationsTime = 0f;
				isCongratulationsOnScreen = false;
				ScaleCongratulations(false);
			}
		}
	}

	public void ShowTutorialAnimation(TutorialType type, bool isCloseButtonImmediately = true)
	{
		LoadTutorialAnimation(type);
		Tutorial.m_OnTutorialWillAppear?.Invoke();
		MoveTutorialAnimation(delegate
		{
			isTutorialAnimationPlaying = true;
		}, true, isCloseButtonImmediately);
	}

	public void HideTutorialAnimation()
	{
		MoveTutorialAnimation(delegate
		{
			Tutorial.m_OnTutorialClose?.Invoke();
		}, false);
	}

	public void ShowTutorialHint(TutorialType type)
	{
		if (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType == GameType.Multiplayer || !(hintAnimation.AnimationName != "idle"))
		{
			return;
		}
		hintAnimation.AnimationName = "appear";
		hintAnimation.loop = true;
		TutorialAnimationInfo tutorialAnimationInfo = InfoByType(type);
		if (!string.IsNullOrEmpty(tutorialAnimationInfo.hintSpriteName))
		{
			GameType gameType = CachedManagers.CachedLevelsManager.CurrentLevel.GameType;
			GameScreen gameScreen = CachedManagers.CachedGUIScreenManager.GameScreen;
			if (gameScreen != null)
			{
				tk2dSpriteCollectionData tk2dSpriteCollectionData2 = null;
				if (gameType == GameType.AppleShoot || gameType == GameType.BirdsHunt)
				{
					tk2dSpriteCollectionData2 = gameScreen.IconWeaponsSpriteCollectionData;
				}
				else
				{
					Level currentLevel = CachedManagers.CachedLevelsManager.CurrentLevel;
					ObjectsSpawner objectsSpawner = CachedManagers.CachedLevelsManager.ObjectsSpawner;
					CharacterNameType characterNameType = ((!(currentLevel.ActiveCharacter == currentLevel.CharacterFirst)) ? objectsSpawner.SecondSelectedCharacterNameType : objectsSpawner.FirstSelectedCharacterNameType);
					tk2dSpriteCollectionData2 = objectsSpawner.GetGUICharacter(characterNameType).SpriteCollectionsData[0];
				}
				hintWeaponSprite.SetSprite(tk2dSpriteCollectionData2, tutorialAnimationInfo.hintSpriteName);
			}
		}
		IsHintAnimationPlaying = true;
		hintType = type;
	}

	public void HideTutorialHint()
	{
		if (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType != GameType.Multiplayer)
		{
			hintAnimation.AnimationName = "disappear";
			hintAnimation.loop = false;
		}
	}

	public void ShowCongratulations()
	{
		AudioManager.PlayAudioWithKey("tutorial_congratulations");
		ScaleCongratulations(true);
	}

	private void EnableCloseButtonTweener()
	{
		TweenPosition[] array = tutorialCloseTweeners;
		foreach (TweenPosition tweener in array)
		{
			tweener.SetOnFinishedDelegate(delegate
			{
				BoxCollider[] array2 = tutorialCloseColliders;
				foreach (BoxCollider boxCollider in array2)
				{
					boxCollider.enabled = !tweener.IsReversed;
				}
			});
			tweener.enabled = true;
		}
	}

	private void MoveTutorialAnimation(TweenCallback OnTutorialMoveFinish, bool isAppear, bool isCloseButtonImmediately = true)
	{
		float duration = ((!isAppear) ? disappearDuration : appearDuration);
		TweenPosition[] array = tutorialAnimationTweeners;
		foreach (TweenPosition tweenPosition in array)
		{
			tweenPosition.SetOnFinishedDelegate(OnTutorialMoveFinish);
			tweenPosition.IsReversed = !isAppear;
			tweenPosition.duration = duration;
			tweenPosition.ResetTween();
			tweenPosition.enabled = true;
		}
		TweenPosition[] array2 = tutorialCloseTweeners;
		foreach (TweenPosition tweenPosition2 in array2)
		{
			tweenPosition2.IsReversed = !isAppear;
			tweenPosition2.duration = duration;
			tweenPosition2.ResetTween();
			tweenPosition2.enabled = isCloseButtonImmediately;
		}
		tutorialFadeTweener.IsReversed = !isAppear;
		tutorialFadeTweener.duration = duration;
		tutorialFadeTweener.ResetTween();
		if (!tutorialFadeTweener.IsReversed)
		{
			tutorialFadeTweener.endColor = new Color(0f, 0f, 0f, maxFadeAlpha);
		}
		tutorialFadeTweener.enabled = true;
	}

	private void LoadTutorialAnimation(TutorialType type)
	{
		SkeletonDataAsset tutorialAsset = InfoByType(type).tutorialAsset;
		string animationName = "tutorial";
		switch (type)
		{
		case TutorialType.Aiming:
			animationName = "shoot_1_no_cards";
			break;
		case TutorialType.Shooting:
			switch (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType)
			{
			case GameType.AppleShoot:
				animationName = "shoot_2_apple";
				break;
			case GameType.BirdsHunt:
				animationName = "shoot_2_bird";
				break;
			default:
				animationName = "shoot_2";
				break;
			}
			break;
		}
		SkeletonAnimation[] array = tutorialAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			skeletonAnimation.AnimationName = null;
			if (skeletonAnimation.SkeletonDataAsset != tutorialAsset)
			{
				skeletonAnimation.SkeletonDataAsset = tutorialAsset;
				skeletonAnimation.Reset();
				skeletonAnimation.State.Event += TutorialAnimation_Event;
				if (type != TutorialType.Aiming && type != TutorialType.Shooting)
				{
					cachedEventTutorialEnd = tutorialAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Tutorial_End");
				}
			}
			skeletonAnimation.AnimationName = animationName;
			skeletonAnimation.loop = true;
			skeletonAnimation.Update(0.001f);
		}
	}

	private TutorialAnimationInfo InfoByType(TutorialType type)
	{
		if (type == TutorialType.None)
		{
			type = TutorialType.Aiming;
		}
		for (int i = 0; i < tutorialAnimationInfo.Length; i++)
		{
			if (tutorialAnimationInfo[i].type == type)
			{
				return tutorialAnimationInfo[i];
			}
		}
		return default(TutorialAnimationInfo);
	}

	private void ScaleCongratulations(bool isAppear)
	{
		congratulationsTweener.SetOnFinishedDelegate(delegate
		{
			isCongratulationsOnScreen = isAppear;
			if (!isAppear && Tutorial.m_OnCongratulationsDisappear != null)
			{
				Tutorial.m_OnCongratulationsDisappear();
			}
		});
		congratulationsTweener.IsReversed = !isAppear;
		congratulationsTweener.ResetTween();
		congratulationsTweener.enabled = true;
	}

	private void TutorialButton_OnClick()
	{
		isTutorialAnimationPlaying = false;
		EnableCloseButtonTweener();
		HideTutorialAnimation();
		Tutorial.m_OnTutorialWillClose?.Invoke();
	}

	private void HintButton_OnClick()
	{
		HideTutorialHint();
		isShowTutorialAfterHintDisappear = true;
	}

	private void TutorialAnimation_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventTutorialEnd)
		{
			EnableCloseButtonTweener();
		}
		else if (e.Data == CachedEventTutorialAimingEnd)
		{
			LoadTutorialAnimation(TutorialType.Shooting);
		}
		else if (e.Data == CachedEventTutorialShootingEnd)
		{
			EnableCloseButtonTweener();
			LoadTutorialAnimation(TutorialType.Aiming);
		}
	}

	private void HintAnimation_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventHintAppearEnd)
		{
			hintAnimation.AnimationName = "idle";
			hintAnimation.loop = true;
		}
		else if (e.Data == CachedEventHintDisappearEnd)
		{
			if (isShowTutorialAfterHintDisappear)
			{
				SingletonMonoBehaviour<TutorialManager>.Instance.ShowTutorialAnimation(hintType);
				isShowTutorialAfterHintDisappear = false;
			}
			IsHintAnimationPlaying = false;
		}
	}

	public void BackButtonPressed()
	{
		TutorialButton_OnClick();
	}
}
