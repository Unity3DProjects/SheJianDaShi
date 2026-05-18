using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class RouletteScreen : GUIScreen
{
	private const string SPIN_EVENT_END = "Spin_End";

	private const string SPIN_EVENT_SLOT_CHANGE = "Item_Switch";

	private const string SPIN_EVENT_FADE_START = "Fade_Start";

	private const string SPIN_EVENT_EFFECTS_START = "Effects_Start";

	private const string SPIN_EVENT_SWITCH_SOUND = "Switch_Sound";

	private const string BUTTON_LABEL_SPIN = "ui.roulette.button_spin";

	private const string BUTTON_LABEL_GET = "ui.roulette.button_get";

	private const string BUTTON_LABEL_IDLE = "ui.roulette.button_idle";

	private const string MONEY_START = "Money_start";

	[SerializeField]
	private RouletteSlotInfo[] rouletteSlotsInfo;

	[SerializeField]
	private GUIRouletteSlot slotPrefab;

	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private tk2dTextMesh[] gemsLabels;

	[SerializeField]
	private SkeletonAnimation[] rouletteAnimations;

	[SerializeField]
	private float timeOpen = 1f;

	[SerializeField]
	private tk2dTextMesh[] wonLabels;

	[SerializeField]
	private GUIControl hideControl;

	[SerializeField]
	private GUIControl hideDescriptionControl;

	[SerializeField]
	private GUIControl fadeControl;

	[SerializeField]
	private int minMatchesForButtons = 5;

	[SerializeField]
	private int roulettesWithFirstDelays;

	[SerializeField]
	private float startSpinFirstDelay;

	[SerializeField]
	private float startSpinSecondDelay;

	[SerializeField]
	private float getRewardFirstDelay;

	[SerializeField]
	private float getRewardSecondDelay;

	[SerializeField]
	private SkeletonAnimation defaultAnimation;

	[SerializeField]
	private tk2dUIItem[] claimButtons;

	[SerializeField]
	private GUIControl[] claimHideControls;

	[SerializeField]
	private tk2dUIItem[] claimX2Buttons;

	[SerializeField]
	private GUIControl[] claimX2Controls;

	[SerializeField]
	private int doubleRewardCounter;

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

	[Header("iPhone X")]
	[SerializeField]
	private GUILayoutCell rightButtonsCell;

	[SerializeField]
	private float rightButtonsIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell topBarCell;

	[SerializeField]
	private float topBarIPhoneXSize;

	[SerializeField]
	private GUILayoutCell bottomBarCell;

	[SerializeField]
	private float bottomBarIPhoneXSize;

	private EventData cachedEvenetSpinEnd;

	private EventData cachedEventFadeStart;

	private EventData cachedEventEffectsStart;

	private EventData cachedEventSwitchSound;

	private bool isSpin;

	private bool isSpined;

	private List<GUIRouletteSlot> cachedSlots = new List<GUIRouletteSlot>();

	private List<GameObject> currentEffects = new List<GameObject>();

	private EventData cachedEventStartMoneyPortrait;

	private EventData cachedEventStartMoneyLandscape;

	private RouletteRewardInfo rewardInfo;

	private bool isMoneyEffectsAvailable;

	private EventData CachedEvenetSpinEnd
	{
		get
		{
			if (cachedEvenetSpinEnd == null && rouletteAnimations.Length > 0 && (bool)rouletteAnimations[0].SkeletonDataAsset)
			{
				cachedEvenetSpinEnd = rouletteAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Spin_End");
			}
			return cachedEvenetSpinEnd;
		}
	}

	private EventData CachedEventFadeStart
	{
		get
		{
			if (cachedEventFadeStart == null && rouletteAnimations.Length > 0 && (bool)rouletteAnimations[0].SkeletonDataAsset)
			{
				cachedEventFadeStart = rouletteAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Fade_Start");
			}
			return cachedEventFadeStart;
		}
	}

	private EventData CachedEventEffectsStart
	{
		get
		{
			if (cachedEventEffectsStart == null && rouletteAnimations.Length > 0 && (bool)rouletteAnimations[0].SkeletonDataAsset)
			{
				cachedEventEffectsStart = rouletteAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Effects_Start");
			}
			return cachedEventEffectsStart;
		}
	}

	private EventData CachedEventSwitchSound
	{
		get
		{
			if (cachedEventSwitchSound == null && rouletteAnimations.Length > 0 && (bool)rouletteAnimations[0].SkeletonDataAsset)
			{
				cachedEventSwitchSound = rouletteAnimations[0].SkeletonDataAsset.GetSkeletonData(true).FindEvent("Switch_Sound");
			}
			return cachedEventSwitchSound;
		}
	}

	private bool IsButtonAvailable
	{
		get
		{
			bool flag = true;
			for (int i = 0; i < cachedSlots.Count; i++)
			{
				if (cachedSlots[i].IsWinnerSlot)
				{
					if (cachedSlots[i].RewardType == RouletteRewardType.Ordinary || cachedSlots[i].RewardType == RouletteRewardType.Premium)
					{
						flag = false;
					}
					break;
				}
			}
			flag &= base.CachedManagers.CachedStatisticsManager.MathesCount >= minMatchesForButtons;
			flag &= !base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
			return flag & base.CachedManagers.CachedRemoteManager.GetRemoteValueForKey("IsDoubleRouletteRewardEnabled", true);
		}
	}

	private float StartSpinDelay
	{
		get
		{
			return (base.CachedManagers.CachedStatisticsManager.OpenedChestsCount >= roulettesWithFirstDelays) ? startSpinSecondDelay : startSpinFirstDelay;
		}
	}

	private float GetRewardDelay
	{
		get
		{
			return (base.CachedManagers.CachedStatisticsManager.OpenedChestsCount >= roulettesWithFirstDelays) ? getRewardSecondDelay : getRewardFirstDelay;
		}
	}

	public SkeletonAnimation DefaultAnimation
	{
		get
		{
			return defaultAnimation;
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

	private void Awake()
	{
		if (GameManager.IsIPhoneX)
		{
			rightButtonsCell.SizeValue = rightButtonsIPhoneXOffset;
			topBarCell.SizeValue = topBarIPhoneXSize;
			bottomBarCell.SizeValue = bottomBarIPhoneXSize;
		}
	}

	private void Start()
	{
		SetCatAnimationTrack(subscribeCatAnimationPortrait);
		SetCatAnimationTrack(subscribeCatAnimationLandscape);
	}

	private void OnEnable()
	{
		SkeletonAnimation[] array = rouletteAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			skeletonAnimation.State.Event += RouletteAnimation_Event;
		}
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnGemsChanged;
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
		subscribeCatAnimationPortrait.State.Event += SubscribeCatAnimationPortrait_State_Event;
		subscribeCatAnimationLandscape.State.Event += SubscribeCatAnimationLandscape_State_Event;
		tk2dUIItem[] array2 = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array2)
		{
			tk2dUIItem2.OnClick += ClaimButton_OnClick;
		}
		tk2dUIItem[] array3 = claimX2Buttons;
		foreach (tk2dUIItem tk2dUIItem3 in array3)
		{
			tk2dUIItem3.OnClick += ClaimX2Button_OnClick;
		}
	}

	private void OnDisable()
	{
		SkeletonAnimation[] array = rouletteAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			skeletonAnimation.State.Event -= RouletteAnimation_Event;
		}
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnGemsChanged;
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
		subscribeCatAnimationPortrait.State.Event -= SubscribeCatAnimationPortrait_State_Event;
		subscribeCatAnimationLandscape.State.Event -= SubscribeCatAnimationLandscape_State_Event;
		tk2dUIItem[] array2 = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array2)
		{
			tk2dUIItem2.OnClick -= ClaimButton_OnClick;
		}
		tk2dUIItem[] array3 = claimX2Buttons;
		foreach (tk2dUIItem tk2dUIItem3 in array3)
		{
			tk2dUIItem3.OnClick -= ClaimX2Button_OnClick;
		}
	}

	private void Update()
	{
		if (isSpin)
		{
			SkeletonAnimation[] array = rouletteAnimations;
			foreach (SkeletonAnimation skeletonAnimation in array)
			{
				skeletonAnimation.Update(Time.deltaTime);
			}
		}
		if (base.IsShown)
		{
			for (int j = 0; j < cachedSlots.Count; j++)
			{
				cachedSlots[j].UpdateSlot(Time.deltaTime);
			}
		}
	}

	private void OnDestroy()
	{
		ResourceManager instanceIfExist = SingletonMonoBehaviour<ResourceManager>.InstanceIfExist;
		if (instanceIfExist != null)
		{
			instanceIfExist.UnloadSkeletonSpriteCollections();
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		tk2dUIItem[] array = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		tk2dUIItem[] array2 = claimX2Buttons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.enabled = false;
		}
		hideControl.HideImmediately();
		hideDescriptionControl.HideImmediately();
		fadeControl.gameObject.SetActive(true);
		fadeControl.ShowImmediately();
		GUIControl[] array3 = claimHideControls;
		foreach (GUIControl gUIControl in array3)
		{
			gUIControl.HideImmediately();
		}
		GUIControl[] array4 = claimX2Controls;
		foreach (GUIControl gUIControl2 in array4)
		{
			gUIControl2.HideImmediately();
		}
		AudioManager.StopAudioWithKey("crowd_loop", 1f);
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		for (int m = 0; m < currentEffects.Count; m++)
		{
			UnityEngine.Object.Destroy(currentEffects[m]);
		}
		currentEffects.Clear();
		SkeletonAnimation[] array5 = rouletteAnimations;
		foreach (SkeletonAnimation skeletonAnimation in array5)
		{
			skeletonAnimation.Skeleton.SetToSetupPose();
			skeletonAnimation.State.SetAnimation(0, skeletonAnimation.AnimationName, false);
			skeletonAnimation.Update(0f);
			skeletonAnimation.ForceUpdate();
		}
		InitializaSlots();
		SetScreenBeforeSpin();
		Action<GUIControl> onFinished2 = delegate(GUIControl control)
		{
			fadeControl.Hide(delegate
			{
				base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, SpinButton_OnClick, StartSpinDelay);
				fadeControl.gameObject.SetActive(false);
			});
			if (onFinished != null)
			{
				onFinished(control);
			}
		};
		base.Show(onFinished2, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		for (int i = 0; i < currentEffects.Count; i++)
		{
			UnityEngine.Object.Destroy(currentEffects[i]);
		}
		currentEffects.Clear();
		Action<GUIControl> onFinished2 = delegate(GUIControl control)
		{
			if (onFinished != null)
			{
				onFinished(control);
			}
			base.CachedManagers.CachedProposalManager.ShowInterstitialMenuAfterRoulette();
		};
		base.Hide(onFinished2, immediately, delay, callInvokes);
	}

	private void InitializaSlots(RouletteRewardInfo predefinedInfo = null)
	{
		if (predefinedInfo == null)
		{
			predefinedInfo = base.CachedManagers.CachedRouletteManager.GenerateReward();
		}
		for (int i = 0; i < rouletteSlotsInfo.Length; i++)
		{
			GUIRouletteSlot gUIRouletteSlot = UnityEngine.Object.Instantiate(slotPrefab);
			gUIRouletteSlot.CachedTransform.parent = rouletteSlotsInfo[i].parentBoneLandscape.CachedTransform;
			gUIRouletteSlot.CachedTransform.localPosition = Vector3.zero;
			gUIRouletteSlot.InitSlot(rouletteSlotsInfo[i]);
			if (predefinedInfo != null)
			{
				gUIRouletteSlot.InitReward(predefinedInfo);
			}
			GUIRouletteSlot gUIRouletteSlot2 = UnityEngine.Object.Instantiate((!rouletteSlotsInfo[i].isWinnerSlot) ? gUIRouletteSlot : slotPrefab);
			gUIRouletteSlot2.CachedTransform.parent = rouletteSlotsInfo[i].parentBonePortrait.CachedTransform;
			gUIRouletteSlot2.CachedTransform.localPosition = Vector3.zero;
			gUIRouletteSlot2.InitSlot(rouletteSlotsInfo[i]);
			if (predefinedInfo != null && rouletteSlotsInfo[i].isWinnerSlot)
			{
				gUIRouletteSlot2.InitReward(predefinedInfo);
			}
			cachedSlots.Add(gUIRouletteSlot);
			cachedSlots.Add(gUIRouletteSlot2);
		}
		if (predefinedInfo != null)
		{
			tk2dTextMesh[] array = wonLabels;
			foreach (tk2dTextMesh tk2dTextMesh2 in array)
			{
				tk2dTextMesh2.text = predefinedInfo.WinDescription();
			}
		}
		rewardInfo = predefinedInfo;
	}

	private void SetScreenBeforeSpin()
	{
		isSpin = false;
		isSpined = false;
		tk2dUIItem[] array = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		tk2dUIItem[] array2 = claimX2Buttons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.enabled = false;
		}
	}

	private void SetScreenUntillSpin()
	{
		isSpin = true;
		tk2dUIItem[] array = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		tk2dUIItem[] array2 = claimX2Buttons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.enabled = false;
		}
	}

	private void SetScreenAfterSpin()
	{
		if (!isSpin && isSpined)
		{
			return;
		}
		isSpin = false;
		isSpined = true;
		if (IsButtonAvailable)
		{
			tk2dUIItem[] array = claimButtons;
			foreach (tk2dUIItem tk2dUIItem2 in array)
			{
				tk2dUIItem2.enabled = true;
			}
			tk2dUIItem[] array2 = claimX2Buttons;
			foreach (tk2dUIItem tk2dUIItem3 in array2)
			{
				tk2dUIItem3.enabled = true;
			}
		}
		else
		{
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, SpinButton_OnClick, GetRewardDelay);
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
		if (isSpined && IsButtonAvailable)
		{
			effectSpawner.gameObject.SetActive(isActive);
		}
	}

	private void RouletteAnimation_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEvenetSpinEnd)
		{
			SetScreenAfterSpin();
		}
		else if (e.Data == CachedEventFadeStart)
		{
			hideDescriptionControl.Show(delegate
			{
				hideControl.Show();
			});
			for (int num = 0; num < cachedSlots.Count; num++)
			{
				cachedSlots[num].PrepareSlot();
			}
			if (IsButtonAvailable)
			{
				GUIControl[] array = claimHideControls;
				foreach (GUIControl gUIControl in array)
				{
					gUIControl.Show();
				}
				GUIControl[] array2 = claimX2Controls;
				foreach (GUIControl gUIControl2 in array2)
				{
					gUIControl2.Show();
				}
				isMoneyEffectsAvailable = true;
			}
		}
		else if (e.Data == CachedEventEffectsStart)
		{
			for (int num4 = 0; num4 < cachedSlots.Count; num4++)
			{
				cachedSlots[num4].ShowSlot();
			}
		}
		else if (e.Data == CachedEventSwitchSound)
		{
			AudioManager.PlayAudioWithKey("roulette_spin");
		}
	}

	private void StatisticsManager_OnCoinsChanged(int coinsCount = 0)
	{
		tk2dTextMesh[] array = coinsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
			GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAchievementAddCoins));
			gameObject.transform.parent = tk2dTextMesh2.transform;
			gameObject.transform.localPosition = Vector3.zero;
			currentEffects.Add(gameObject);
		}
	}

	private void StatisticsManager_OnGemsChanged(int gemsCount = 0)
	{
		tk2dTextMesh[] array = gemsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Gems.ToString();
			if (gemsCount != 0)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAddGems));
				gameObject.transform.parent = tk2dTextMesh2.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}
		}
	}

	private void SpinButton_OnClick()
	{
		if (isSpin)
		{
			return;
		}
		if (isSpined)
		{
			StartCoroutine(CoroutineOpenReward());
			base.CachedManagers.CachedStatisticsManager.OpenedChestsCount++;
			tk2dUIItem[] array = claimButtons;
			foreach (tk2dUIItem tk2dUIItem2 in array)
			{
				tk2dUIItem2.enabled = false;
			}
			tk2dUIItem[] array2 = claimX2Buttons;
			foreach (tk2dUIItem tk2dUIItem3 in array2)
			{
				tk2dUIItem3.enabled = false;
			}
		}
		else
		{
			SetScreenUntillSpin();
			AudioManager.PlayAudioWithKey("roulette_start");
		}
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType obj)
	{
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				SkeletonAnimation[] array = rouletteAnimations;
				foreach (SkeletonAnimation skeletonAnimation in array)
				{
					skeletonAnimation.Update(0f);
				}
			}
		}, 0f);
	}

	private void SubscribeCatAnimationPortrait_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventStartMoneyPortrait && !moneyEffectSpawnerPortrait.gameObject.activeInHierarchy && isMoneyEffectsAvailable)
		{
			SetMoneyEffectActive(moneyEffectSpawnerPortrait, true);
		}
	}

	private void SubscribeCatAnimationLandscape_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventStartMoneyLandscape && !moneyEffectSpawnerLandscape.gameObject.activeInHierarchy && isMoneyEffectsAvailable)
		{
			SetMoneyEffectActive(moneyEffectSpawnerLandscape, true);
		}
	}

	private void ClaimButton_OnClick()
	{
		base.CachedManagers.CachedStatisticsManager.OpenedChestsCount++;
		if (!isMoneyEffectsAvailable)
		{
			base.CachedManagers.CachedRouletteManager.OpenReward(null, 2);
		}
		else
		{
			base.CachedManagers.CachedRouletteManager.OpenReward();
		}
		for (int i = 0; i < cachedSlots.Count; i++)
		{
			cachedSlots[i].OpenSlot();
		}
		AudioManager.PlayAudioWithKey("roulette_win");
		tk2dUIItem[] array = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		tk2dUIItem[] array2 = claimX2Buttons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.enabled = false;
		}
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			base.CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Main);
		}, timeOpen);
	}

	private void ClaimX2Button_OnClick()
	{
		
		//TODO:激励--领取双倍奖励（开宝箱后）
			ADManager.Instance.ShowRewardAD(this.gameObject.name,nameof(GetReward_ClaimX2));
		
		// base.CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoShown)
		// {
		// 	if (isVideoShown)
		// 	{
		// 		GUIControl[] array = claimX2Controls;
		// 		foreach (GUIControl gUIControl in array)
		// 		{
		// 			gUIControl.Hide();
		// 		}
		// 		for (int j = 0; j < claimButtons.Length; j++)
		// 		{
		// 			claimButtons[j].gameObject.SetActive(false);
		// 		}
		// 		for (int k = 0; k < cachedSlots.Count; k++)
		// 		{
		// 			if (cachedSlots[k].IsWinnerSlot)
		// 			{
		// 				cachedSlots[k].DoubleReward(doubleRewardCounter);
		// 			}
		// 		}
		// 		tk2dTextMesh[] array2 = wonLabels;
		// 		foreach (tk2dTextMesh tk2dTextMesh2 in array2)
		// 		{
		// 			tk2dTextMesh2.text = rewardInfo.WinDescription(doubleRewardCounter);
		// 		}
		// 		tk2dUIItem[] array3 = claimX2Buttons;
		// 		foreach (tk2dUIItem tk2dUIItem2 in array3)
		// 		{
		// 			tk2dUIItem2.enabled = false;
		// 		}
		// 		SetMoneyEffectActive(moneyEffectSpawnerPortrait, false);
		// 		SetMoneyEffectActive(moneyEffectSpawnerLandscape, false);
		// 		isMoneyEffectsAvailable = false;
		// 		ClaimButton_OnClick();
		// 	}
		// });
	}

	public void GetReward_ClaimX2()
	{
		if (true)
		{
			GUIControl[] array = claimX2Controls;
			foreach (GUIControl gUIControl in array)
			{
				gUIControl.Hide();
			}
			for (int j = 0; j < claimButtons.Length; j++)
			{
				claimButtons[j].gameObject.SetActive(false);
			}
			for (int k = 0; k < cachedSlots.Count; k++)
			{
				if (cachedSlots[k].IsWinnerSlot)
				{
					cachedSlots[k].DoubleReward(doubleRewardCounter);
				}
			}
			tk2dTextMesh[] array2 = wonLabels;
			foreach (tk2dTextMesh tk2dTextMesh2 in array2)
			{
				tk2dTextMesh2.text = rewardInfo.WinDescription(doubleRewardCounter);
			}
			tk2dUIItem[] array3 = claimX2Buttons;
			foreach (tk2dUIItem tk2dUIItem2 in array3)
			{
				tk2dUIItem2.enabled = false;
			}
			SetMoneyEffectActive(moneyEffectSpawnerPortrait, false);
			SetMoneyEffectActive(moneyEffectSpawnerLandscape, false);
			isMoneyEffectsAvailable = false;
			ClaimButton_OnClick();
		}
	}

	private IEnumerator CoroutineOpenReward()
	{
		for (int i = 0; i < cachedSlots.Count; i++)
		{
			cachedSlots[i].OpenSlot();
		}
		base.CachedManagers.CachedRouletteManager.OpenReward();
		AudioManager.PlayAudioWithKey("roulette_win");
		yield return new WaitForSeconds(timeOpen);
		if (!base.CachedManagers.CachedTutorialManager.IsTutorialMatchesEnded)
		{
			base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
			{
				base.CachedManagers.CachedGameManager.StartTutorialMatch();
			});
		}
		else if (!IsButtonAvailable)
		{
			base.CachedManagers.CachedGUIScreenManager.HideCurrentAndShow(GUIScreenType.Main);
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(0f, 50f, 100f, 50f), "Coins1"))
		{
			SkeletonAnimation[] array = rouletteAnimations;
			foreach (SkeletonAnimation skeletonAnimation in array)
			{
				skeletonAnimation.Skeleton.SetToSetupPose();
				skeletonAnimation.State.SetAnimation(0, skeletonAnimation.AnimationName, false);
				skeletonAnimation.ForceUpdate();
			}
			hideControl.HideImmediately();
			hideDescriptionControl.HideImmediately();
			RouletteRewardInfo predefinedInfo = new RouletteRewardInfo(0, RouletteRewardType.Coins1);
			InitializaSlots(predefinedInfo);
			SetScreenUntillSpin();
		}
		if (GUI.Button(new Rect(0f, 100f, 100f, 50f), "Coins2"))
		{
			SkeletonAnimation[] array2 = rouletteAnimations;
			foreach (SkeletonAnimation skeletonAnimation2 in array2)
			{
				skeletonAnimation2.Skeleton.SetToSetupPose();
				skeletonAnimation2.State.SetAnimation(0, skeletonAnimation2.AnimationName, false);
				skeletonAnimation2.ForceUpdate();
			}
			hideControl.HideImmediately();
			hideDescriptionControl.HideImmediately();
			RouletteRewardInfo predefinedInfo2 = new RouletteRewardInfo(0, RouletteRewardType.Coins2);
			InitializaSlots(predefinedInfo2);
			SetScreenUntillSpin();
		}
		if (GUI.Button(new Rect(0f, 150f, 100f, 50f), "Coins3"))
		{
			SkeletonAnimation[] array3 = rouletteAnimations;
			foreach (SkeletonAnimation skeletonAnimation3 in array3)
			{
				skeletonAnimation3.Skeleton.SetToSetupPose();
				skeletonAnimation3.State.SetAnimation(0, skeletonAnimation3.AnimationName, false);
				skeletonAnimation3.ForceUpdate();
			}
			hideControl.HideImmediately();
			hideDescriptionControl.HideImmediately();
			RouletteRewardInfo predefinedInfo3 = new RouletteRewardInfo(0, RouletteRewardType.Coins3);
			InitializaSlots(predefinedInfo3);
			SetScreenUntillSpin();
		}
		if (GUI.Button(new Rect(0f, 200f, 100f, 50f), "Common"))
		{
			SkeletonAnimation[] array4 = rouletteAnimations;
			foreach (SkeletonAnimation skeletonAnimation4 in array4)
			{
				skeletonAnimation4.Skeleton.SetToSetupPose();
				skeletonAnimation4.State.SetAnimation(0, skeletonAnimation4.AnimationName, false);
				skeletonAnimation4.ForceUpdate();
			}
			hideControl.HideImmediately();
			hideDescriptionControl.HideImmediately();
			RouletteRewardInfo rouletteRewardInfo = new RouletteRewardInfo(0, RouletteRewardType.Ordinary);
			rouletteRewardInfo.archer = base.CachedManagers.CachedShopManager.AvailableCharacters[0];
			InitializaSlots(rouletteRewardInfo);
			SetScreenUntillSpin();
		}
		if (GUI.Button(new Rect(0f, 250f, 100f, 50f), "Rare"))
		{
			SkeletonAnimation[] array5 = rouletteAnimations;
			foreach (SkeletonAnimation skeletonAnimation5 in array5)
			{
				skeletonAnimation5.Skeleton.SetToSetupPose();
				skeletonAnimation5.State.SetAnimation(0, skeletonAnimation5.AnimationName, false);
				skeletonAnimation5.ForceUpdate();
			}
			hideControl.HideImmediately();
			hideDescriptionControl.HideImmediately();
			RouletteRewardInfo rouletteRewardInfo2 = new RouletteRewardInfo(0, RouletteRewardType.Premium);
			rouletteRewardInfo2.archer = base.CachedManagers.CachedShopManager.AvailableCharacters[0];
			InitializaSlots(rouletteRewardInfo2);
			SetScreenUntillSpin();
		}
	}
}
