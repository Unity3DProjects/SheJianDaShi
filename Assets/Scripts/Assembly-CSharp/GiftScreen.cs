using System;
using System.Collections;
using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using UnityEngine;
using UnityEngine.Analytics;

public class GiftScreen : GUIScreen
{
	private const string WIN_REWARD_ARCHER = "ui.gift.labelwin.postfix_archer";

	private const string DEFAULT_ANIMATION = "default";

	private const string WIN_ANIMATION = "win";

	private const string IDLE_ANIMATION = "idle";

	private const int FREE_GEMS_RECEIVED_BEFORE_SUBSCRIBE = 1;

	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private tk2dTextMesh[] gemsLabels;

	[SerializeField]
	private tk2dUIItem[] bankButtons;

	[SerializeField]
	private tk2dUIItem[] returnButtons;

	[SerializeField]
	private GUILayoutCell[] giftsRootCells;

	[SerializeField]
	private GameObject[] claimLayout;

	[SerializeField]
	private GameObject[] twoClaimLayout;

	[SerializeField]
	private GameObject[] timerLayout;

	[SerializeField]
	private tk2dTextMesh[] timerLabel;

	[SerializeField]
	private tk2dUIItem[] claimButtons;

	[SerializeField]
	private tk2dUIItem[] simpleClaimButtons;

	[SerializeField]
	private tk2dTextMesh[] dayLabels;

	[SerializeField]
	private tk2dSprite[] rewardSprites;

	[SerializeField]
	private tk2dSlicedSprite[] rewardCoinSprites;

	[SerializeField]
	private tk2dTextMesh[] rewardCoinLabels;

	[SerializeField]
	private GUILayoutCell[] videoLabelCells;

	[SerializeField]
	private GUILayoutCell archerGetCell;

	[SerializeField]
	private GUIControl controlFade;

	[SerializeField]
	private float timeDelayCoins;

	[SerializeField]
	private float timeDelayChest;

	[SerializeField]
	private float timeDelayArcher;

	[SerializeField]
	private tk2dUIItem getButton;

	[SerializeField]
	private tk2dTextMesh flagDescription;

	[SerializeField]
	private Transform animationTransform;

	[SerializeField]
	private SkeletonAnimation defaultAnimation;

	[SerializeField]
	private float timeDelayGetArcher;

	[SerializeField]
	private GameObject[] rootsGift;

	[SerializeField]
	private GameObject[] rootsArcher;

	[SerializeField]
	private float timeDelayReturnAfterCoins = 3f;

	[SerializeField]
	private GameObject[] bankAlertObjects;

	[Header("iPhone X")]
	[SerializeField]
	private GUILayoutCell[] bottomOffsetCells;

	[SerializeField]
	private float bottomIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] topOffsetCells;

	[SerializeField]
	private float topIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell[] leftButtonsCells;

	[SerializeField]
	private GUILayoutCell[] rightButtonsCells;

	[SerializeField]
	private float sideButtonsIPhoneXOffset;

	private List<GUIGift> cachedGift = new List<GUIGift>();

	private bool isNextDayGiftAvailable;

	private int currentNumberGift;

	private int realNumberGift;

	private ClaimButtonsStateType claimButtonsState;

	private List<GameObject> currentEffects = new List<GameObject>();

	private SkeletonAnimation giftCharacterAnimation;

	private SkeletonAnimation giftDoubleCharacterAnimation;

	private GUITimer timer = new GUITimer();

	private int CurrentNumberGift
	{
		get
		{
			return currentNumberGift;
		}
	}

	private int RealNumberGift
	{
		get
		{
			return realNumberGift;
		}
	}

	private ClaimButtonsStateType ClaimButtonsState
	{
		get
		{
			return claimButtonsState;
		}
		set
		{
			switch (value)
			{
			case ClaimButtonsStateType.claim:
			{
				GameObject[] array4 = claimLayout;
				foreach (GameObject gameObject4 in array4)
				{
					gameObject4.SetActive(true);
				}
				GameObject[] array5 = twoClaimLayout;
				foreach (GameObject gameObject5 in array5)
				{
					gameObject5.SetActive(false);
				}
				GameObject[] array6 = timerLayout;
				foreach (GameObject gameObject6 in array6)
				{
					gameObject6.SetActive(false);
				}
				GUILayoutCell[] array7 = videoLabelCells;
				foreach (GUILayoutCell gUILayoutCell in array7)
				{
					gUILayoutCell.gameObject.SetActive(false);
					gUILayoutCell.SizeValue = 0f;
				}
				break;
			}
			case ClaimButtonsStateType.claimClaimX2:
			{
				GameObject[] array8 = claimLayout;
				foreach (GameObject gameObject7 in array8)
				{
					gameObject7.SetActive(false);
				}
				GameObject[] array9 = twoClaimLayout;
				foreach (GameObject gameObject8 in array9)
				{
					gameObject8.SetActive(true);
				}
				GameObject[] array10 = timerLayout;
				foreach (GameObject gameObject9 in array10)
				{
					gameObject9.SetActive(false);
				}
				break;
			}
			case ClaimButtonsStateType.claimPerVideo:
			{
				GameObject[] array11 = claimLayout;
				foreach (GameObject gameObject10 in array11)
				{
					gameObject10.SetActive(true);
				}
				GameObject[] array12 = twoClaimLayout;
				foreach (GameObject gameObject11 in array12)
				{
					gameObject11.SetActive(false);
				}
				GameObject[] array13 = timerLayout;
				foreach (GameObject gameObject12 in array13)
				{
					gameObject12.SetActive(false);
				}
				GUILayoutCell[] array14 = videoLabelCells;
				foreach (GUILayoutCell gUILayoutCell2 in array14)
				{
					gUILayoutCell2.gameObject.SetActive(true);
					gUILayoutCell2.SizeValue = 40f;
				}
				break;
			}
			case ClaimButtonsStateType.timer:
			{
				GameObject[] array = claimLayout;
				foreach (GameObject gameObject in array)
				{
					gameObject.SetActive(false);
				}
				GameObject[] array2 = twoClaimLayout;
				foreach (GameObject gameObject2 in array2)
				{
					gameObject2.SetActive(false);
				}
				GameObject[] array3 = timerLayout;
				foreach (GameObject gameObject3 in array3)
				{
					gameObject3.SetActive(true);
				}
				break;
			}
			}
			claimButtonsState = value;
		}
	}

	private void Awake()
	{
		GUILayoutCell[] array = giftsRootCells;
		foreach (GUILayoutCell gUILayoutCell in array)
		{
			cachedGift.AddRange(gUILayoutCell.GetComponentsInChildren<GUIGift>());
		}
		if (GameManager.IsIPhoneX)
		{
			for (int j = 0; j < bottomOffsetCells.Length; j++)
			{
				bottomOffsetCells[j].SizeValue = bottomIPhoneXOffset;
			}
			for (int k = 0; k < topOffsetCells.Length; k++)
			{
				topOffsetCells[k].SizeValue = topIPhoneXOffset;
			}
			for (int l = 0; l < leftButtonsCells.Length; l++)
			{
				leftButtonsCells[l].SizeValue = 0f;
			}
			for (int m = 0; m < rightButtonsCells.Length; m++)
			{
				rightButtonsCells[m].SizeValue = sideButtonsIPhoneXOffset;
			}
		}
		isNextDayGiftAvailable = true;
	}

	private void OnEnable()
	{
		tk2dUIItem[] array = returnButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += ReturnButton_OnClick;
		}
		GiftManager.OnGiftAvailable += GiftManager_OnGiftAvailable;
		getButton.OnClick += GetButton_OnClick;
		GiftManager.OnTimeForGiftChanged += GiftManager_OnTimeForGiftChanged;
		tk2dUIItem[] array2 = claimButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick += ClaimButton_OnClick;
		}
		tk2dUIItem[] array3 = simpleClaimButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick += SimpleClaimButton_OnClick;
		}
		tk2dUIItem[] array4 = bankButtons;
		foreach (tk2dUIItem tk2dUIItem5 in array4)
		{
			tk2dUIItem5.OnClick += BankButton_OnClick;
		}
		timer.OnTimerChanged += Timer_OnTimerChanged;
		UpdateBankAlertIcons();
		GiftInfoArray currentGiftInfoArray = base.CachedManagers.CachedGiftManager.CurrentGiftInfoArray;
		realNumberGift = base.CachedManagers.CachedGiftManager.NumberGift;
		currentNumberGift = realNumberGift % currentGiftInfoArray.giftInfos.Length;
	}

	private void OnDisable()
	{
		tk2dUIItem[] array = returnButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= ReturnButton_OnClick;
		}
		GiftManager.OnGiftAvailable -= GiftManager_OnGiftAvailable;
		getButton.OnClick -= GetButton_OnClick;
		GiftManager.OnTimeForGiftChanged -= GiftManager_OnTimeForGiftChanged;
		tk2dUIItem[] array2 = claimButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick -= ClaimButton_OnClick;
		}
		tk2dUIItem[] array3 = simpleClaimButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick -= SimpleClaimButton_OnClick;
		}
		tk2dUIItem[] array4 = bankButtons;
		foreach (tk2dUIItem tk2dUIItem5 in array4)
		{
			tk2dUIItem5.OnClick -= BankButton_OnClick;
		}
		timer.OnTimerChanged -= Timer_OnTimerChanged;
	}

	private void Update()
	{
		timer.UpdateTimer(Time.unscaledDeltaTime);
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		GameObject[] array = rootsGift;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(true);
		}
		GameObject[] array2 = rootsArcher;
		foreach (GameObject gameObject2 in array2)
		{
			gameObject2.SetActive(false);
		}
		tk2dUIItem[] array3 = returnButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array3)
		{
			tk2dUIItem2.enabled = true;
		}
		for (int l = 0; l < currentEffects.Count; l++)
		{
			UnityEngine.Object.Destroy(currentEffects[l]);
		}
		currentEffects.Clear();
		controlFade.gameObject.SetActive(true);
		controlFade.HideImmediately();
		controlFade.gameObject.SetActive(false);
		tk2dTextMesh[] array4 = coinsLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array4)
		{
			tk2dTextMesh2.text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
		}
		tk2dTextMesh[] array5 = gemsLabels;
		foreach (tk2dTextMesh tk2dTextMesh3 in array5)
		{
			tk2dTextMesh3.text = base.CachedManagers.CachedStatisticsManager.Gems.ToString();
		}
		InitializeGifts();
		InitializeArcher();
		InitializeClaimButton();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		for (int i = 0; i < currentEffects.Count; i++)
		{
			UnityEngine.Object.Destroy(currentEffects[i]);
		}
		currentEffects.Clear();
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void InitializeArcher()
	{
		GiftInfoArray currentGiftInfoArray = base.CachedManagers.CachedGiftManager.CurrentGiftInfoArray;
		int numberGift = base.CachedManagers.CachedGiftManager.NumberGift;
		if (currentGiftInfoArray.giftInfos == null)
		{
			return;
		}
		for (int i = 0; i < currentGiftInfoArray.giftInfos.Length; i++)
		{
			GiftInfo giftInfo = currentGiftInfoArray.giftInfos[i];
			if (giftInfo.type != GiftType.Archer || i != numberGift)
			{
				continue;
			}
			GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(giftInfo.giftCharacter);
			flagDescription.text = base.ObjectsSpawner.GetCharacterLocalizedName(giftInfo.giftCharacter) + base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.gift.labelwin.postfix_archer");
			if (gUICharacter.CharacterMode == CharacterMode.Random)
			{
				List<SkeletonAnimation> list = gUICharacter.AddDoubleCharacterAnimations(defaultAnimation, animationTransform);
				for (int j = 0; j < list.Count; j++)
				{
					giftDoubleCharacterAnimation = list[i];
					if (giftDoubleCharacterAnimation != null)
					{
						giftDoubleCharacterAnimation.State.SetAnimation(0, "default", true);
						giftDoubleCharacterAnimation.LateUpdate();
					}
				}
			}
			else
			{
				giftCharacterAnimation = gUICharacter.AddCharacterAnimation(defaultAnimation, animationTransform);
				if (giftCharacterAnimation != null)
				{
					giftCharacterAnimation.State.SetAnimation(0, "default", true);
					giftCharacterAnimation.LateUpdate();
				}
			}
		}
	}

	private void InitializeGifts()
	{
		for (int i = 0; i < cachedGift.Count; i++)
		{
			int numberGift = cachedGift[i].NumberGift;
			GiftInfo info = giftInfoForDay(numberGift);
			GiftStateType giftStyle = GiftStateType.FutureDay;
			if (numberGift < CurrentNumberGift)
			{
				giftStyle = GiftStateType.Claimed;
			}
			else if (numberGift == CurrentNumberGift)
			{
				giftStyle = ((!isNextDayGiftAvailable) ? GiftStateType.NextDay : GiftStateType.Today);
			}
			cachedGift[i].SetGiftByInfoAndStyle(info, giftStyle);
			StartCoroutine(FadeCoroutine(cachedGift[i], numberGift < CurrentNumberGift));
		}
		GiftInfo giftInfo = giftInfoForDay(CurrentNumberGift);
		bool flag = giftInfo.type == GiftType.Coins || giftInfo.type == GiftType.Gems;
		tk2dTextMesh[] array = dayLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("ui.gift.label.day" + (CurrentNumberGift + 1));
		}
		tk2dSprite[] array2 = rewardSprites;
		foreach (tk2dSprite tk2dSprite2 in array2)
		{
			tk2dSprite2.SetSprite(giftInfo.spriteName);
		}
		tk2dSlicedSprite[] array3 = rewardCoinSprites;
		foreach (tk2dSlicedSprite tk2dSlicedSprite2 in array3)
		{
			tk2dSlicedSprite2.gameObject.SetActive(flag);
		}
		if (flag)
		{
			tk2dTextMesh[] array4 = rewardCoinLabels;
			foreach (tk2dTextMesh tk2dTextMesh3 in array4)
			{
				tk2dTextMesh3.text = giftInfo.CoinsValue.ToString();
			}
		}
	}

	private void InitializeClaimButton()
	{
		GiftInfo giftInfo = giftInfoForDay(CurrentNumberGift);
		bool flag = giftInfo.type == GiftType.Coins || giftInfo.type == GiftType.Gems;
		if (base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive)
		{
			ClaimButtonsState = ClaimButtonsStateType.claim;
		}
		else if (isNextDayGiftAvailable)
		{
			ClaimButtonsState = ((!flag) ? ClaimButtonsStateType.claim : ClaimButtonsStateType.claimClaimX2);
		}
		else
		{
			ClaimButtonsState = ClaimButtonsStateType.claimPerVideo;
		}
		tk2dUIItem[] array = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = true;
		}
		tk2dUIItem[] array2 = simpleClaimButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.enabled = true;
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

	private void CheckNextDayGift()
	{
		if (isNextDayGiftAvailable && !base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive)
		{
			isNextDayGiftAvailable = false;
			GiftInfoArray currentGiftInfoArray = base.CachedManagers.CachedGiftManager.CurrentGiftInfoArray;
			realNumberGift = base.CachedManagers.CachedGiftManager.NumberGift;
			currentNumberGift = realNumberGift % currentGiftInfoArray.giftInfos.Length;
			InitializeGifts();
			InitializeClaimButton();
			InitializeArcher();
		}
		else
		{
			StartCoroutine(TimerCoroutine());
		}
	}

	private GiftInfo giftInfoForDay(int day)
	{
		GiftInfoArray currentGiftInfoArray = base.CachedManagers.CachedGiftManager.CurrentGiftInfoArray;
		GiftInfo giftInfo = default(GiftInfo);
		if (RealNumberGift <= currentGiftInfoArray.giftInfos.Length)
		{
			giftInfo = currentGiftInfoArray.giftInfos[day];
			if (giftInfo.type == GiftType.Archer)
			{
				ShopItemCharacter shopItemCharacter = base.ObjectsSpawner.GetShopItemCharacter(giftInfo.giftCharacter);
				if (shopItemCharacter.Bought && day >= CurrentNumberGift)
				{
					giftInfo = currentGiftInfoArray.defaultGiftInfos[day];
				}
			}
		}
		else
		{
			giftInfo = currentGiftInfoArray.defaultGiftInfos[day];
		}
		return giftInfo;
	}

	private void ClaimPerVideo(Action callback)
	{
		base.CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoShown)
		{
			if (isVideoShown && callback != null)
			{
				callback();
			}
		});
	}

	private void SimpleClaimButton_OnClick()
	{
		GetGift();
	}

	private void ClaimButton_OnClick()
	{
		//TODO:激励--日常礼包-领取
		// GetReward_CompeleteVideo();
		ADManager.Instance.ShowRewardAD(this.gameObject.name,nameof(GetReward_CompeleteVideo));
		// switch (ClaimButtonsState)
		// {
		// case ClaimButtonsStateType.claimClaimX2:
		// 	ClaimPerVideo(delegate
		// 	{
		// 		GetGift(2);
		// 	});
		// 	break;
		// case ClaimButtonsStateType.claimPerVideo:
		// 	ClaimPerVideo(delegate
		// 	{
		// 		GetGift();
		// 	});
		// 	break;
		// default:
		// 	GetGift();
		// 	break;
		// }
	}

	public void GetReward_CompeleteVideo()
	{
		switch (ClaimButtonsState)
		{
			case ClaimButtonsStateType.claimClaimX2:
					GetGift(2);
				break;
			case ClaimButtonsStateType.claimPerVideo:
					GetGift();
				break;
			default:
				GetGift();
				break;
		}
	}
	
	

	private void GiftManager_OnGiftAvailable()
	{
		InitializeGifts();
	}

	private void GetGift(int counter = 1)
	{
		tk2dUIItem[] array = claimButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		tk2dUIItem[] array2 = simpleClaimButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.enabled = false;
		}
		for (int k = 0; k < cachedGift.Count; k++)
		{
			if (cachedGift[k].NumberGift == CurrentNumberGift && cachedGift[k].isActiveAndEnabled)
			{
				GUIGift_OnGiftOpen(cachedGift[k], counter);
			}
		}
		SingletonMonoBehaviour<GiftManager>.Instance.GiftOpen(null);
	}

	private void GUIGift_OnGiftOpen(GUIGift sender, int counter = 1)
	{
		GameObject newEffect = null;
		int num = 0;
		switch (sender.Info.type)
		{
		case GiftType.Coins:
			GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.EARNED, GoliathIds.Context.DAILY_GIFT, GoliathIds.CurrencyName.COINS, sender.Info.CoinsValue * counter, GoliathIds.Placement.MAIN_MENU, base.CachedManagers.CachedStatisticsManager.Coins - num);
			switch (sender.Info.coinsType)
			{
			case 1:
				AudioManager.PlayAudioWithKey("coins_get_little");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftCoinsSmall));
				break;
			case 2:
				AudioManager.PlayAudioWithKey("coins_get_middle");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftCoinsMedium));
				break;
			case 3:
				AudioManager.PlayAudioWithKey("coins_get_many");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftCoinsLarge));
				break;
			case 4:
				AudioManager.PlayAudioWithKey("coins_get_many");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftCoinsExtraLarge));
				break;
			}
			newEffect.transform.parent = sender.IconGift.transform;
			newEffect.transform.localPosition = Vector3.zero;
			currentEffects.Add(newEffect);
			base.CachedManagers.CachedStatisticsManager.AddCoins(sender.Info.coinsValue * counter);
			StartCoroutine(CoroutineAddCoins());
			break;
		case GiftType.Gems:
			switch (sender.Info.coinsType)
			{
			case 1:
				AudioManager.PlayAudioWithKey("coins_get_little");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAddGems));
				break;
			case 2:
				AudioManager.PlayAudioWithKey("coins_get_middle");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAddGems));
				break;
			case 3:
				AudioManager.PlayAudioWithKey("coins_get_many");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftGemsLarge));
				break;
			case 4:
				AudioManager.PlayAudioWithKey("coins_get_many");
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftGemsExtraLarge));
				break;
			}
			newEffect.transform.parent = sender.IconGift.transform;
			newEffect.transform.localPosition = Vector3.zero;
			currentEffects.Add(newEffect);
			base.CachedManagers.CachedStatisticsManager.AddGems(sender.Info.coinsValue * counter);
			StartCoroutine(CoroutineAddGems());
			break;
		case GiftType.Chest:
		{
			Analytics.CustomEvent("Chest_Opened_Daily_Gift", new Dictionary<string, object>
			{
				{
					"ChestOpened",
					base.CachedManagers.CachedStatisticsManager.OpenedChestsCount
				},
				{
					"GameTimeMinutes",
					base.CachedManagers.CachedStatisticsManager.TotalGameTime.Minutes
				}
			});
			AudioManager.PlayAudioWithKey("gift_chest");
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftChest));
				newEffect.transform.parent = sender.IconGift.transform;
				newEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(newEffect);
			}, 0f);
			tk2dUIItem[] array2 = returnButtons;
			foreach (tk2dUIItem tk2dUIItem3 in array2)
			{
				tk2dUIItem3.enabled = false;
			}
			controlFade.gameObject.SetActive(true);
			controlFade.Show(delegate
			{
				tk2dUIItem[] array3 = returnButtons;
				foreach (tk2dUIItem tk2dUIItem4 in array3)
				{
					tk2dUIItem4.enabled = true;
				}
				base.CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.white;
				base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(false, delegate
				{
					base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Roulette, delegate
					{
						base.CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.black;
					});
				}, false);
			}, false, timeDelayChest, true);
			break;
		}
		case GiftType.Archer:
		{
			AudioManager.PlayAudioWithKey("buy_character_rare");
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				newEffect = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftArcher));
				newEffect.transform.parent = sender.IconGift.transform;
				newEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(newEffect);
			}, 0f);
			if (sender.Info.giftCharacter != CharacterNameType.None)
			{
				ShopItemCharacter shopItemCharacter = base.ObjectsSpawner.GetShopItemCharacter(sender.Info.giftCharacter);
				GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(sender.Info.giftCharacter);
				shopItemCharacter.OpenCharacter(true, true);
				base.CachedManagers.CachedStatisticsManager.CurrentOpenedGiftNumber = sender.NumberGift;
				base.ObjectsSpawner.SetFirstSelectedCharacter(sender.Info.giftCharacter);
				if ((bool)gUICharacter)
				{
					gUICharacter.CheckBought();
				}
				GameGoliath.Instance.TrackCharacterAction(shopItemCharacter.GetCharacterID(), GoliathIds.Action.ACQUISITION, GoliathIds.Origin.DAILY_GIFT, string.Empty);
			}
			tk2dUIItem[] array = returnButtons;
			foreach (tk2dUIItem tk2dUIItem2 in array)
			{
				tk2dUIItem2.enabled = false;
			}
			getButton.enabled = false;
			controlFade.gameObject.SetActive(true);
			controlFade.Show(delegate
			{
				GameObject[] array3 = rootsGift;
				foreach (GameObject gameObject in array3)
				{
					gameObject.SetActive(false);
				}
				GameObject[] array4 = rootsArcher;
				foreach (GameObject gameObject2 in array4)
				{
					gameObject2.SetActive(true);
				}
				if (giftCharacterAnimation != null)
				{
					giftCharacterAnimation.State.SetAnimation(0, "idle", true);
					giftCharacterAnimation.LateUpdate();
				}
				if (giftDoubleCharacterAnimation != null)
				{
					giftDoubleCharacterAnimation.State.SetAnimation(0, "idle", true);
					giftDoubleCharacterAnimation.LateUpdate();
				}
				controlFade.Hide(delegate
				{
					tk2dUIItem[] array5 = returnButtons;
					foreach (tk2dUIItem tk2dUIItem4 in array5)
					{
						tk2dUIItem4.enabled = true;
					}
					getButton.enabled = true;
					controlFade.gameObject.SetActive(false);
				});
			}, false, timeDelayArcher, true);
			break;
		}
		}
	}

	private void ReturnButton_OnClick()
	{
		StopCoroutine(CoroutineAddCoins());
		base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Main);
		});
	}

	private void GetButton_OnClick()
	{
		if (giftCharacterAnimation != null)
		{
			giftCharacterAnimation.State.SetAnimation(0, "win", true);
			giftCharacterAnimation.LateUpdate();
		}
		if (giftDoubleCharacterAnimation != null)
		{
			giftDoubleCharacterAnimation.State.SetAnimation(0, "win", true);
			giftDoubleCharacterAnimation.LateUpdate();
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIGiftArcherGet));
			gameObject.transform.parent = archerGetCell.transform;
			gameObject.transform.localPosition = Vector3.zero;
			currentEffects.Add(gameObject);
		}, 0f);
		StartCoroutine(CoroutineGetArcher());
	}

	private void GiftManager_OnTimeForGiftChanged(TimeSpan obj)
	{
		tk2dTextMesh[] array = timerLabel;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = obj.ToHHMMSS();
		}
	}

	private void BankButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Shop);
		});
		base.CachedManagers.CachedGUIScreenManager.BeforeShopScreenType = GUIScreenType.Gift;
	}

	private void Timer_OnTimerChanged(int time)
	{
		if (time == 0)
		{
			UpdateBankAlertIcons();
		}
	}

	private IEnumerator CoroutineAddCoins()
	{
		tk2dUIItem[] array = returnButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		yield return new WaitForSeconds(timeDelayCoins);
		tk2dTextMesh[] array2 = coinsLabels;
		foreach (tk2dTextMesh l in array2)
		{
			l.text = base.CachedManagers.CachedStatisticsManager.Coins.ToString();
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAchievementAddCoins));
				gameObject.transform.parent = l.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}, 0f);
		}
		tk2dUIItem[] array3 = returnButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array3)
		{
			tk2dUIItem3.enabled = true;
		}
		CheckNextDayGift();
	}

	private IEnumerator CoroutineAddGems()
	{
		tk2dUIItem[] array = returnButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		yield return new WaitForSeconds(timeDelayCoins);
		tk2dTextMesh[] array2 = gemsLabels;
		foreach (tk2dTextMesh l in array2)
		{
			l.text = base.CachedManagers.CachedStatisticsManager.Gems.ToString();
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIAddGems));
				gameObject.transform.parent = l.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentEffects.Add(gameObject);
			}, 0f);
		}
		tk2dUIItem[] array3 = returnButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array3)
		{
			tk2dUIItem3.enabled = true;
		}
		CheckNextDayGift();
	}

	private IEnumerator CoroutineGetArcher()
	{
		getButton.enabled = false;
		yield return new WaitForSeconds(timeDelayGetArcher);
		getButton.enabled = true;
		controlFade.gameObject.SetActive(true);
		controlFade.Show(delegate
		{
			GameObject[] array = rootsGift;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(true);
			}
			GameObject[] array2 = rootsArcher;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.SetActive(false);
			}
			controlFade.Hide(delegate
			{
				tk2dUIItem[] array3 = returnButtons;
				foreach (tk2dUIItem tk2dUIItem2 in array3)
				{
					tk2dUIItem2.enabled = true;
				}
				getButton.enabled = true;
				controlFade.gameObject.SetActive(false);
			});
		}, false, timeDelayArcher, true);
		CheckNextDayGift();
	}

	private IEnumerator TimerCoroutine()
	{
		ClaimButtonsState = ClaimButtonsStateType.timer;
		yield return new WaitForSeconds(timeDelayReturnAfterCoins);
		ReturnButton_OnClick();
	}

	private IEnumerator FadeCoroutine(GUIGift target, bool isFade = true)
	{
		yield return new WaitForEndOfFrame();
		target.Fade(isFade);
	}

	public override void BackButtonPressed()
	{
		ReturnButton_OnClick();
	}
}
