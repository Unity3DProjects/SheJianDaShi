using System;
using UnityEngine;

public class UpgradePopup : GUIPopup
{
	private const string COINS_REWARD_PREFIX = "+";

	[SerializeField]
	private tk2dTextMesh[] mainPriceLabels;

	[SerializeField]
	private tk2dTextMesh[] minorPriceLabels;

	[SerializeField]
	private tk2dUIItem[] buttonsBuy;

	[SerializeField]
	private tk2dTextMesh[] timerLabels;

	[SerializeField]
	private TweenPosition[] showTweeners;

	[SerializeField]
	private tk2dUIItem closeButtonAdditional;

	[SerializeField]
	private GameObject[] timerCells;

	[SerializeField]
	private tk2dTextMesh[] headerLabels;

	[SerializeField]
	private tk2dTextMesh[] coinsLabels;

	[SerializeField]
	private string headerForcedTextKey;

	[SerializeField]
	private GameObject[] priceRoots;

	[SerializeField]
	private GameObject[] loadingRoots;

	[SerializeField]
	protected ShopItem shopItem;

	private ShopItemUpgradeNew currentPack;

	protected virtual bool IsForced
	{
		get
		{
			if (SingletonMonoBehaviour<Managers>.InstanceIfExist != null && SingletonMonoBehaviour<ProposalManager>.InstanceIfExist != null)
			{
				return base.CachedManagers.CachedProposalManager.IsShowingForcedPack;
			}
			return false;
		}
		set
		{
			if (SingletonMonoBehaviour<Managers>.InstanceIfExist != null && SingletonMonoBehaviour<ProposalManager>.InstanceIfExist != null)
			{
				base.CachedManagers.CachedProposalManager.IsShowingForcedPack = value;
			}
		}
	}

	private void Start()
	{
		tk2dTextMesh[] componentsInChildren = base.transform.GetComponentsInChildren<tk2dTextMesh>();
		tk2dTextMesh[] array = componentsInChildren;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			if (tk2dTextMesh2.text.Contains("reward.right"))
			{
				tk2dTextMesh2.text += ".android";
				Debug.Log(tk2dTextMesh2.text + " " + tk2dTextMesh2.FormattedText);
				break;
			}
		}
		GUILayoutCell gUILayoutCell = null;
		GUILayoutCell[] componentsInChildren2 = base.transform.GetComponentsInChildren<GUILayoutCell>();
		GUILayoutCell[] array2 = componentsInChildren2;
		foreach (GUILayoutCell gUILayoutCell2 in array2)
		{
			if (gUILayoutCell2.gameObject.name.Equals("RewardsRootCell"))
			{
				gUILayoutCell = gUILayoutCell2;
				break;
			}
		}
		gUILayoutCell.SizeValue += 30f;
		Transform child = gUILayoutCell.transform.GetChild(0);
		for (int k = 0; k < child.childCount; k++)
		{
			Transform child2 = child.GetChild(k);
			if (child2.gameObject.name.Equals("__FlexSpace"))
			{
				continue;
			}
			Transform child3 = child2.GetChild(child2.childCount - 1);
			if (child2.gameObject.name.Equals("NoAdsRewardRootCell"))
			{
				child3.GetChild(0).GetComponent<GUILayoutCell>().SizeValue += 30f;
				tk2dSprite[] componentsInChildren3 = child2.gameObject.GetComponentsInChildren<tk2dSprite>();
				tk2dSprite[] array3 = componentsInChildren3;
				foreach (tk2dSprite tk2dSprite2 in array3)
				{
					if (tk2dSprite2.gameObject.name.Equals("NoAdsSprite"))
					{
						tk2dSpriteCollection component = MiniclipPrefabs.Instance.spriteCollection.GetComponent<tk2dSpriteCollection>();
						tk2dSpriteCollectionData spriteCollection = component.spriteCollection;
						tk2dSprite2.SetSprite(spriteCollection, "popup_upgrade_no_ads");
					}
				}
			}
			else
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "__FixedSpace";
				GUILayoutCell gUILayoutCell3 = gameObject.AddComponent<GUILayoutCell>();
				gUILayoutCell3.SizeValue = 15f;
				gameObject.transform.parent = child3;
				gameObject.transform.SetSiblingIndex(0);
			}
		}
		SingletonMonoBehaviour<ProposalManager>.Instance.LastUpgrade = DateTime.Now;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		InitializeShopItemVariable();
		if (shopItem != null)
		{
			shopItem.OnPriceChanged += ShopItem_OnPriceChanged;
		}
		currentPack = shopItem as ShopItemUpgradeNew;
		tk2dUIItem[] array = buttonsBuy;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += ButtonBuy_OnClick;
		}
		if (closeButtonAdditional != null)
		{
			closeButtonAdditional.OnClick += CloseButton_OnClick;
		}
		if (!IsForced)
		{
			GUITimer proposalTimer = base.CachedManagers.CachedProposalManager.ProposalTimer;
			proposalTimer.OnTimerChanged += ProposalManager_OnProposalTimerChanged;
			ProposalManager_OnProposalTimerChanged((int)proposalTimer.TimerInSeconds);
		}
		else
		{
			for (int j = 0; j < timerCells.Length; j++)
			{
				timerCells[j].SetActive(false);
			}
			if (!string.IsNullOrEmpty(headerForcedTextKey))
			{
				string textByKey = base.CachedManagers.CachedLocalisationManager.GetTextByKey(headerForcedTextKey);
				for (int k = 0; k < headerLabels.Length; k++)
				{
					headerLabels[k].text = textByKey;
				}
			}
		}
		if (currentPack != null)
		{
			for (int l = 0; l < coinsLabels.Length; l++)
			{
				coinsLabels[l].text = "+" + currentPack.CoinsReward;
			}
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (shopItem != null)
		{
			shopItem.OnPriceChanged -= ShopItem_OnPriceChanged;
		}
		tk2dUIItem[] array = buttonsBuy;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= ButtonBuy_OnClick;
		}
		if (closeButtonAdditional != null)
		{
			closeButtonAdditional.OnClick -= CloseButton_OnClick;
		}
		if (!IsForced)
		{
			ProposalManager instanceIfExist = SingletonMonoBehaviour<ProposalManager>.InstanceIfExist;
			if (instanceIfExist != null)
			{
				instanceIfExist.ProposalTimer.OnTimerChanged -= ProposalManager_OnProposalTimerChanged;
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		for (int i = 0; i < showTweeners.Length; i++)
		{
			if (!showTweeners[i].gameObject.activeInHierarchy && showTweeners[i].enabled)
			{
				showTweeners[i].Update();
			}
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SetPrice();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		tk2dUIItem[] array = buttonsBuy;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.enabled = false;
		}
		if (!IsForced)
		{
			base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
		}
		else
		{
			IsForced = false;
		}
		if (shopItem != null)
		{
			shopItem.OnPriceChanged -= ShopItem_OnItemPriceChanged;
		}
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	protected virtual void InitializeShopItemVariable()
	{
		shopItem = base.CachedManagers.CachedProposalManager.UpgradeNew;
		if (shopItem != null)
		{
			shopItem.OnPriceChanged += ShopItem_OnItemPriceChanged;
		}
	}

	private void SetPrice()
	{
		if (!(shopItem != null))
		{
			return;
		}
		string priceString = "观看视频";
		// string priceString = shopItem.PriceString;
		bool flag = !string.IsNullOrEmpty(priceString);
		tk2dTextMesh[] array = mainPriceLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.gameObject.SetActive(true);
			// tk2dTextMesh2.gameObject.SetActive(flag);
		}
		tk2dTextMesh[] array2 = minorPriceLabels;
		foreach (tk2dTextMesh tk2dTextMesh3 in array2)
		{
			tk2dTextMesh3.gameObject.SetActive(flag);
		}
		GameObject[] array3 = loadingRoots;
		foreach (GameObject gameObject in array3)
		{
			gameObject.gameObject.SetActive(false);
			// gameObject.gameObject.SetActive(!flag);
		}
		GameObject[] array4 = priceRoots;
		foreach (GameObject gameObject2 in array4)
		{
			gameObject2.gameObject.SetActive(flag);
		}
		if (!flag)
		{
			return;
		}
		tk2dTextMesh[] array5 = mainPriceLabels;
		foreach (tk2dTextMesh tk2dTextMesh4 in array5)
		{
			tk2dTextMesh4.text = "观看视频领取";
		}
		tk2dTextMesh[] array6 = minorPriceLabels;
		foreach (tk2dTextMesh tk2dTextMesh5 in array6)
		{
			if (currentPack != null)
			{
				tk2dTextMesh5.text = currentPack.PriceStringSale;
			}
		}
	}

	private void ButtonBuy_OnClick()
	{
		if (shopItem != null && !shopItem.Bought)
		{
			shopItem.OnItemPurchased += ShopItem_OnItemPurchased;
			shopItem.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
			base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
			if (!shopItem.TryBuy())
			{
				shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
				shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
			}
		}
	}

	protected virtual void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		ShopItemUpgradeNew shopItemUpgradeNew = shopItem as ShopItemUpgradeNew;
		if (shopItemUpgradeNew != null && shopItemUpgradeNew.Characters != null && shopItemUpgradeNew.Characters.Length > 0)
		{
			base.ObjectsSpawner.SetFirstSelectedCharacter(shopItemUpgradeNew.Characters[0].Infos[0].characterNameType);
		}
		MenuScreen menuScreen = base.CachedManagers.CachedGUIScreenManager.MenuScreen;
		if (menuScreen != null)
		{
			MenuScreenScrollView menuScreenScrollView = menuScreen.MenuScreenScrollView;
			menuScreenScrollView.CheckBoughtCharacters();
			menuScreenScrollView.UpdateScrollViewPosition();
		}
		base.CachedManagers.CachedProposalManager.UpgradePackWasBought();
		Hide();
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
	}

	private void ShopItem_OnItemPriceChanged(ShopItem shopItem)
	{
		if (!(shopItem == null) && !string.IsNullOrEmpty(shopItem.PriceString))
		{
			tk2dTextMesh[] array = mainPriceLabels;
			foreach (tk2dTextMesh tk2dTextMesh2 in array)
			{
				tk2dTextMesh2.gameObject.SetActive(true);
				tk2dTextMesh2.text = shopItem.PriceString;
			}
			tk2dTextMesh[] array2 = minorPriceLabels;
			foreach (tk2dTextMesh tk2dTextMesh3 in array2)
			{
				tk2dTextMesh3.gameObject.SetActive(false);
			}
			GameObject[] array3 = loadingRoots;
			foreach (GameObject gameObject in array3)
			{
				gameObject.gameObject.SetActive(false);
			}
			GameObject[] array4 = priceRoots;
			foreach (GameObject gameObject2 in array4)
			{
				gameObject2.gameObject.SetActive(false);
			}
		}
	}

	private void ShopItem_OnPriceChanged(ShopItem shopItem)
	{
		SetPrice();
	}

	private void ProposalManager_OnProposalTimerChanged(int timeInSeconds)
	{
		if (timeInSeconds == 0)
		{
			Hide();
			return;
		}
		string text = string.Format("{0}:{1}:{2}", timeInSeconds / 3600, (timeInSeconds / 60 % 60).ToString("00"), (timeInSeconds % 60).ToString("00"));
		tk2dTextMesh[] array = timerLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			tk2dTextMesh2.text = text;
		}
	}
}
