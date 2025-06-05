using System;
using UnityEngine;

public class PackPopup : GUIPopup
{
	[SerializeField]
	private tk2dTextMesh packNameLabel;

	[SerializeField]
	private tk2dTextMesh mainPriceLabel;

	[SerializeField]
	private tk2dTextMesh minorPriceLabel;

	[SerializeField]
	private GameObject priceLabelsRoot;

	[SerializeField]
	private SkeletonAnimation loading;

	[SerializeField]
	private tk2dBaseSprite packSprite;

	[SerializeField]
	private tk2dUIItem buttonBuy;

	[SerializeField]
	private tk2dTextMesh timerLabel;

	private ShopItemPack currentPack;

	protected override void OnEnable()
	{
		base.OnEnable();
		buttonBuy.OnClick += ButtonBuy_OnClick;
		currentPack = SingletonMonoBehaviour<ProposalManager>.Instance.BuyPack;
		currentPack.OnPriceChanged += CurrentPack_OnPriceChanged;
		GUITimer proposalTimer = SingletonMonoBehaviour<ProposalManager>.Instance.ProposalTimer;
		proposalTimer.OnTimerChanged += ProposalManager_OnProposalTimerChanged;
		ProposalManager_OnProposalTimerChanged((int)proposalTimer.TimerInSeconds);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (currentPack != null)
		{
			currentPack.OnPriceChanged -= CurrentPack_OnPriceChanged;
		}
		buttonBuy.OnClick -= ButtonBuy_OnClick;
		ProposalManager instanceIfExist = SingletonMonoBehaviour<ProposalManager>.InstanceIfExist;
		if (instanceIfExist != null)
		{
			SingletonMonoBehaviour<ProposalManager>.Instance.ProposalTimer.OnTimerChanged -= ProposalManager_OnProposalTimerChanged;
		}
	}

	private void SetPrice()
	{
		if (currentPack != null)
		{
			packSprite.SetSprite(currentPack.PopupSpriteName);
			packNameLabel.text = currentPack.LocalizedDescription;
			string priceString = currentPack.PriceString;
			if (string.IsNullOrEmpty(priceString))
			{
				priceLabelsRoot.SetActive(false);
				loading.gameObject.SetActive(true);
				return;
			}
			mainPriceLabel.text = priceString;
			minorPriceLabel.text = currentPack.PriceStringSale;
			priceLabelsRoot.SetActive(true);
			loading.gameObject.SetActive(false);
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SetPrice();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		buttonBuy.enabled = false;
		SingletonMonoBehaviour<ProposalManager>.Instance.GenerateProposalMenu(true);
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void ButtonBuy_OnClick()
	{
		if (currentPack != null && !currentPack.Bought)
		{
			currentPack.OnItemPurchased += ShopItem_OnItemPurchased;
			currentPack.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			tk2dUIManager.Instance.InputEnabled = false;
			if (!currentPack.TryBuy())
			{
				currentPack.OnItemPurchased -= ShopItem_OnItemPurchased;
				currentPack.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				tk2dUIManager.Instance.InputEnabled = true;
			}
		}
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
	}

	private void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
		if (currentPack.FirstCharacter != null && currentPack.FirstCharacter.Infos[0].characterNameType != CharacterNameType.None)
		{
			base.ObjectsSpawner.SetFirstSelectedCharacter(currentPack.FirstCharacter.Infos[0].characterNameType);
		}
		SingletonMonoBehaviour<ProposalManager>.Instance.LastPack = DateTime.Now;
		MenuScreen menuScreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.MenuScreen;
		if (menuScreen != null)
		{
			menuScreen.MenuScreenScrollView.CheckBoughtCharacters();
			menuScreen.MenuScreenScrollView.UpdateScrollViewPosition();
		}
		Hide();
	}

	private void CurrentPack_OnPriceChanged(ShopItem shopItem)
	{
		SetPrice();
	}

	private void ProposalManager_OnProposalTimerChanged(int timeInSeconds)
	{
		if (timeInSeconds == 0)
		{
			Hide();
		}
		else
		{
			timerLabel.text = string.Format("{0}:{1}:{2}", timeInSeconds / 3600, (timeInSeconds / 60 % 60).ToString("00"), (timeInSeconds % 60).ToString("00"));
		}
	}
}
