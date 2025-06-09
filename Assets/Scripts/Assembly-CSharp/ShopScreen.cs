using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : GUIScreen
{
	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private tk2dTextMesh[] gemsLabels;

	[SerializeField]
	private GUIShopSlot slotPrefab;

	[SerializeField]
	private tk2dUIScrollView landscapeScrollView;

	[SerializeField]
	private GUILayoutCell landscapeParentCell;

	[SerializeField]
	private float landscapeCellsInterval;

	[SerializeField]
	private tk2dUIScrollView portraitScrollView;

	[SerializeField]
	private GUILayoutCell portraitParentCell;

	[SerializeField]
	private float portraitCellsInterval;

	[SerializeField]
	private float portraitOffset;

	[SerializeField]
	private float portraitRowOffset;

	[SerializeField]
	private tk2dUIItem[] backButtons;

	[Header("iPhone X")]
	[SerializeField]
	private float additionalIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell leftButtonsCell;

	[SerializeField]
	private GUILayoutCell rightButtonsCell;

	[SerializeField]
	private float rightButtonsIPhoneXOffset;

	[SerializeField]
	private float additionalPortraitIPhoneXOffset;

	[SerializeField]
	private GUILayoutCell topBarCell;

	[SerializeField]
	private float topBarIPhoneXSize;

	[SerializeField]
	private GUILayoutCell bottomBarCell;

	[SerializeField]
	private float bottomBarIPhoneXSize;

	private bool isInitializedLandscape;

	private bool isInitializedPortrait;

	private List<GUIShopSlot> landscapeSlots = new List<GUIShopSlot>();

	private List<GUIShopSlot> portraitSlots = new List<GUIShopSlot>();

	private void Awake()
	{
		landscapeCellsInterval *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
		portraitCellsInterval *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
		portraitOffset *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
		portraitRowOffset *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
		additionalIPhoneXOffset *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
		if (GameManager.IsIPhoneX)
		{
			leftButtonsCell.SizeValue = 0f;
			rightButtonsCell.SizeValue = rightButtonsIPhoneXOffset;
			topBarCell.SizeValue = topBarIPhoneXSize;
			bottomBarCell.SizeValue = bottomBarIPhoneXSize;
		}
	}

	private void OnEnable()
	{
		tk2dUIItem[] array = backButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += BackButton_OnClick;
		}
		StatisticsManager.OnCoinsChanged += StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged += StatisticsManager_OnGemsChanged;
		GiftManager.OnGiftGemsOpen += GiftManager_OnGiftGemsOpen;
	}

	private void OnDisable()
	{
		tk2dUIItem[] array = backButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= BackButton_OnClick;
		}
		StatisticsManager.OnCoinsChanged -= StatisticsManager_OnCoinsChanged;
		StatisticsManager.OnGemsChanged -= StatisticsManager_OnGemsChanged;
		GiftManager.OnGiftGemsOpen -= GiftManager_OnGiftGemsOpen;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		InitializeLandscapeShopSlots(landscapeParentCell);
		InitializePortraitShopSlots(portraitParentCell);
		StatisticsManager_OnCoinsChanged();
		StatisticsManager_OnGemsChanged();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	private void InitializeLandscapeShopSlots(GUILayoutCell parentCell)
	{
		if (isInitializedLandscape)
		{
			return;
		}
		landscapeSlots.Clear();
		bool flag = !base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		int num = ((!flag) ? (base.CachedManagers.CachedShopManager.BankItems.Count - 1) : base.CachedManagers.CachedShopManager.BankItems.Count);
		float F = 1.5f;
		for (int i = 0; i < num; i++)
		{
			ShopItemGems shopItem = base.CachedManagers.CachedShopManager.BankItems[(!flag) ? (i + 1) : i];
			if (!(shopItem.RewardValue == 200 || shopItem.RewardValue == 20000))
			{
				continue;
			}
			GUIShopSlot gUIShopSlot = UnityEngine.Object.Instantiate(slotPrefab);
			gUIShopSlot.Init(shopItem, parentCell);
			gUIShopSlot.transform.parent = landscapeScrollView.contentContainer;
			if (SingletonMonoBehaviour<GUIOrientationManger>.Instance.CurrentRotationType == GUILayouterRotationType.Landscape)
			{
				gUIShopSlot.transform.localPosition = (landscapeCellsInterval * (float)F - ((float)Screen.width * GUIScreenManager.GuiScale - landscapeCellsInterval) / 2f) * Vector3.right;
				// gUIShopSlot.transform.localPosition = (landscapeCellsInterval * (float)i - ((float)Screen.width * GUIScreenManager.GuiScale - landscapeCellsInterval) / 2f) * Vector3.right;
			}
			else
			{
				gUIShopSlot.transform.localPosition = (landscapeCellsInterval * (float)F - ((float)Screen.height * GUIScreenManager.GuiScale - landscapeCellsInterval) / 2f) * Vector3.right;
				// gUIShopSlot.transform.localPosition = (landscapeCellsInterval * (float)i - ((float)Screen.height * GUIScreenManager.GuiScale - landscapeCellsInterval) / 2f) * Vector3.right;
			}

			F+=2;
			gUIShopSlot.transform.localPosition += ((!GameManager.IsIPhoneX) ? 0f : additionalIPhoneXOffset) * Vector3.right;
			gUIShopSlot.transform.localScale = Vector3.one;
			landscapeSlots.Add(gUIShopSlot);
		}
		if (SingletonMonoBehaviour<GUIOrientationManger>.Instance.CurrentRotationType == GUILayouterRotationType.Landscape)
		{
			landscapeScrollView.contentLength = (float)landscapeSlots.Count * landscapeCellsInterval - (float)Screen.width * GUIScreenManager.GuiScale;
		}
		else
		{
			landscapeScrollView.contentLength = (float)landscapeSlots.Count * landscapeCellsInterval - (float)Screen.height * GUIScreenManager.GuiScale;
		}
		landscapeScrollView.contentLength += ((!GameManager.IsIPhoneX) ? 0f : (2f * additionalIPhoneXOffset));
		isInitializedLandscape = true;
	}

	private void InitializePortraitShopSlots(GUILayoutCell parentCell)
	{
		if (isInitializedPortrait)
		{
			return;
		}
		portraitSlots.Clear();
		bool flag = !base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		int num = ((!flag) ? (base.CachedManagers.CachedShopManager.BankItems.Count - 1) : base.CachedManagers.CachedShopManager.BankItems.Count);
		for (int i = 0; i < num; i++)
		{
			ShopItemGems shopItem = base.CachedManagers.CachedShopManager.BankItems[(!flag) ? (i + 1) : i];
			GUIShopSlot gUIShopSlot = UnityEngine.Object.Instantiate(slotPrefab);
			gUIShopSlot.Init(shopItem, parentCell);
			gUIShopSlot.transform.parent = portraitScrollView.contentContainer;
			if (SingletonMonoBehaviour<GUIOrientationManger>.Instance.CurrentRotationType == GUILayouterRotationType.Landscape)
			{
				gUIShopSlot.transform.localPosition = (portraitOffset + portraitCellsInterval * (float)(int)Math.Truncate((float)i / 2f) - ((float)Screen.width * GUIScreenManager.GuiScale - portraitCellsInterval) / 2f) * Vector3.down;
			}
			else
			{
				gUIShopSlot.transform.localPosition = (portraitOffset + portraitCellsInterval * (float)(int)Math.Truncate((float)i / 2f) - ((float)Screen.height * GUIScreenManager.GuiScale - portraitCellsInterval) / 2f) * Vector3.down;
			}
			gUIShopSlot.transform.localPosition += ((!GameManager.IsIPhoneX) ? 0f : additionalPortraitIPhoneXOffset) * Vector3.down;
			gUIShopSlot.transform.localPosition += ((i % 2 != 0) ? (Vector3.right * portraitRowOffset) : (Vector3.left * portraitRowOffset));
			gUIShopSlot.transform.localScale = Vector3.one;
			portraitSlots.Add(gUIShopSlot);
		}
		float num2 = (float)Math.Ceiling((double)portraitSlots.Count / 2.0);
		if (SingletonMonoBehaviour<GUIOrientationManger>.Instance.CurrentRotationType == GUILayouterRotationType.Landscape)
		{
			portraitScrollView.contentLength = (num2 + 0.5f) * portraitCellsInterval - (float)Screen.width * GUIScreenManager.GuiScale;
		}
		else
		{
			portraitScrollView.contentLength = (num2 + 0.5f) * portraitCellsInterval - (float)Screen.height * GUIScreenManager.GuiScale;
		}
		portraitScrollView.contentLength += ((!GameManager.IsIPhoneX) ? 0f : (2f * additionalPortraitIPhoneXOffset));
		isInitializedPortrait = true;
	}

	private void BackButton_OnClick()
	{
		base.CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			base.CachedManagers.CachedGUIScreenManager.ShowScreen(base.CachedManagers.CachedGUIScreenManager.BeforeShopScreenType, null, true);
		});
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
				UnityEngine.Object.Destroy(gameObject, 5f);
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
				UnityEngine.Object.Destroy(gameObject, 5f);
			}
		}
	}

	private void GiftManager_OnGiftGemsOpen()
	{
		for (int i = 0; i < portraitSlots.Count; i++)
		{
			portraitSlots[i].Refresh();
		}
		for (int j = 0; j < landscapeSlots.Count; j++)
		{
			landscapeSlots[j].Refresh();
		}
	}
}
