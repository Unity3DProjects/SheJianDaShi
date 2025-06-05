using System;
using UnityEngine;

public class AutoRenewableSubscriptionFoolishPopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem[] closeButtons;

	[SerializeField]
	private tk2dUIItem[] tryForFreeButtons;

	[SerializeField]
	private GameObject[] chineseLayouters;

	[SerializeField]
	private GameObject[] englishLayouters;

	private ShopItem subscriptionWeekItem;

	private bool isLanguageChinese;

	private SystemLanguage currentLanguage;

	protected override void OnEnable()
	{
		base.OnEnable();
		tk2dUIItem[] array = closeButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += CloseButton_OnClick;
		}
		tk2dUIItem[] array2 = tryForFreeButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick += WeekButton_OnClick;
		}
		subscriptionWeekItem = base.CachedManagers.CachedShopManager.SubscriptionWeek;
		if (subscriptionWeekItem != null)
		{
			subscriptionWeekItem.OnPriceChanged += ShopItem_OnPriceChanged;
		}
		currentLanguage = base.CachedManagers.CachedStatisticsManager.CurrentLanguage;
		isLanguageChinese = currentLanguage == SystemLanguage.Chinese || currentLanguage == SystemLanguage.ChineseSimplified || currentLanguage == SystemLanguage.ChineseTraditional;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		tk2dUIItem[] array = closeButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= CloseButton_OnClick;
		}
		tk2dUIItem[] array2 = tryForFreeButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick -= WeekButton_OnClick;
		}
		if (subscriptionWeekItem != null)
		{
			subscriptionWeekItem.OnPriceChanged -= ShopItem_OnPriceChanged;
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		tk2dUIItem[] array = tryForFreeButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.gameObject.SetActive(false);
		}
		for (int j = 0; j < chineseLayouters.Length; j++)
		{
			chineseLayouters[j].gameObject.SetActive(isLanguageChinese);
		}
		for (int k = 0; k < englishLayouters.Length; k++)
		{
			englishLayouters[k].gameObject.SetActive(!isLanguageChinese);
		}
		SetPrice();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	private void SetPrice()
	{
		if (subscriptionWeekItem != null)
		{
			tk2dUIItem[] array = tryForFreeButtons;
			foreach (tk2dUIItem tk2dUIItem2 in array)
			{
				tk2dUIItem2.gameObject.SetActive((subscriptionWeekItem as ShopItemSubscription).IsTrial);
			}
		}
	}

	private void WeekButton_OnClick()
	{
		if (subscriptionWeekItem != null && !subscriptionWeekItem.Bought)
		{
			subscriptionWeekItem.OnItemPurchased += ShopItem_OnItemPurchased;
			subscriptionWeekItem.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			tk2dUIManager.Instance.InputEnabled = false;
			if (!subscriptionWeekItem.TryBuy())
			{
				subscriptionWeekItem.OnItemPurchased -= ShopItem_OnItemPurchased;
				subscriptionWeekItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				tk2dUIManager.Instance.InputEnabled = true;
			}
		}
	}

	private void ShopItem_OnPriceChanged(ShopItem shopItem)
	{
		SetPrice();
	}

	private void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
		Hide();
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
	}
}
