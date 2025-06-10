using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;

public class GUIShopSlot : MonoBehaviour
{
	private const string BENEFIT_POSTFIX = "ui.bank.benefit.label";

	private const int FREE_GEMS_RECEIVED_BEFORE_INVITES = 0;

	private const int FREE_GEMS_RECEIVED_BEFORE_SUBSCRIBE = 1;

	public static Action OnBankItemPurchased;

	[SerializeField]
	private GUILayouter internalLayouter;

	[SerializeField]
	private tk2dSprite bestValueSprite;

	[SerializeField]
	private tk2dSprite mostPopularSprite;

	[SerializeField]
	private tk2dSprite rewardIcon;

	[SerializeField]
	private tk2dTextMesh descriptionLabel;

	[SerializeField]
	private tk2dTextMesh rewardValueLabelGems;

	[SerializeField]
	private tk2dTextMesh rewardValueLabelCoins;

	[SerializeField]
	private GameObject gemsRewardObject;

	[SerializeField]
	private GameObject coinsRewardObject;

	[SerializeField]
	private tk2dTextMesh benefitLabel;

	[SerializeField]
	[ResourceLink]
	private AssetLink shineIdleEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink shineIdleOverlayerEffectPrefab;

	[SerializeField]
	private tk2dUIItem buyButton;

	[SerializeField]
	private GameObject priceRoot;

	[SerializeField]
	private GameObject loadingRoot;

	[SerializeField]
	private tk2dTextMesh priceLabel;

	[SerializeField]
	private GameObject regularSlotRoot;

	[SerializeField]
	private GameObject timerSlotRoot;

	[SerializeField]
	private GameObject adsAndInvitesRoot;

	[SerializeField]
	private GameObject adsRoot;

	[SerializeField]
	private GameObject invitesRoot;

	[SerializeField]
	private tk2dUIItem adsAndInvitesButton;

	[SerializeField]
	private tk2dTextMesh timerLabel;

	private ShopItemGems cachedShopItem;

	private bool isAdsShopSlot;

	private GUITimer timer = new GUITimer();

	private bool isInitialized;

	private Managers cachedManagers;

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

	private void OnEnable()
	{
		buyButton.OnClick += BuyButton_OnClick;
		adsAndInvitesButton.OnClick += AdsAndInvitesButton_OnClick;
		SubscribeManager.OnSubscribeForGems = (Action)Delegate.Combine(SubscribeManager.OnSubscribeForGems, new Action(SubscribeManager_OnSubscribeForGems));
		timer.OnTimerChanged += Timer_OnTimerChanged;
	}

	private void OnDisable()
	{
		buyButton.OnClick -= BuyButton_OnClick;
		adsAndInvitesButton.OnClick -= AdsAndInvitesButton_OnClick;
		SubscribeManager.OnSubscribeForGems = (Action)Delegate.Remove(SubscribeManager.OnSubscribeForGems, new Action(SubscribeManager_OnSubscribeForGems));
		if (cachedShopItem != null)
		{
			cachedShopItem.OnPriceChanged -= ShopItem_OnPriceChanged;
		}
		timer.OnTimerChanged -= Timer_OnTimerChanged;
	}

	private void Update()
	{
		timer.UpdateTimer(Time.unscaledDeltaTime);
	}

	public void Init(ShopItemGems shopItem, GUILayoutCell parentCell = null, bool isNeedOverlayerEffect = false)
	{
		if (!isInitialized)
		{
			cachedShopItem = shopItem;
			if (cachedShopItem != null)
			{
				cachedShopItem.OnPriceChanged += ShopItem_OnPriceChanged;
			}
			isAdsShopSlot = cachedShopItem is ShopItemGemForAds;
			if (parentCell != null)
			{
				parentCell.LayoutHandlerObjects.Add(internalLayouter.gameObject);
			}
			rewardIcon.SetSprite(cachedShopItem.GUIItem.IconSpriteName);
			rewardIcon.scale = cachedShopItem.GUIItem.ScaleFactor * Vector3.one;
			bestValueSprite.gameObject.SetActive(cachedShopItem.IsBestValue);
			mostPopularSprite.gameObject.SetActive(cachedShopItem.IsMostPopular);
			descriptionLabel.text = cachedShopItem.GUIItem.ItemDescription;
			benefitLabel.gameObject.SetActive(cachedShopItem.BenefitValue > 0);
			StringBuilder stringBuilder = new StringBuilder("+");
			stringBuilder.Append(cachedShopItem.BenefitValue);
			stringBuilder.Append(CachedManagers.CachedLocalisationManager.GetTextByKey("ui.bank.benefit.label"));
			benefitLabel.text = stringBuilder.ToString();
			
			Debug.LogError("商店全改为广告获取------------------------------------------");
			
			if (isAdsShopSlot)
			{
				if (CachedManagers.CachedStatisticsManager.FreeGemsReceived > 1)
				{
					DateTime dateTime = DateTime.Now.Subtract(CachedManagers.CachedStatisticsManager.CheatedTime);
					DateTime dateTime2 = CustomPlayerPrefs.GetDateTime("free_gems_timer_ended", dateTime);
					if ((dateTime2 - dateTime).TotalSeconds > 0.0 && !timer.IsTimerActive)
					{
						timer.TimerInSeconds = (float)(dateTime2 - dateTime).TotalSeconds;
						timer.IsTimerActive = true;
						timer.UpdateTimer(Time.unscaledDeltaTime);
					}
				}
			}
			else
			{
				SetPrice();
			}
			if (cachedShopItem is ShopItemCoins)
			{
				gemsRewardObject.SetActive(false);
				coinsRewardObject.SetActive(true);
			}
			else
			{
				gemsRewardObject.SetActive(true);
				coinsRewardObject.SetActive(false);
			}
			if (isNeedOverlayerEffect)
			{
				if (shineIdleOverlayerEffectPrefab != null && !string.IsNullOrEmpty(shineIdleOverlayerEffectPrefab.assetGUID))
				{
					GameObject gameObject = (GameObject)shineIdleOverlayerEffectPrefab.GetInstance();
					gameObject.transform.parent = rewardIcon.CachedTransform;
					gameObject.transform.localPosition = Vector3.zero;
				}
			}
			else if (shineIdleEffectPrefab != null && !string.IsNullOrEmpty(shineIdleEffectPrefab.assetGUID))
			{
				GameObject gameObject2 = (GameObject)shineIdleEffectPrefab.GetInstance();
				gameObject2.transform.parent = rewardIcon.CachedTransform;
				gameObject2.transform.localPosition = Vector3.zero;
			}
			isInitialized = true;
		}
		switch (CachedManagers.CachedStatisticsManager.FreeGemsReceived)
		{
		case 0:
			rewardValueLabelGems.text = ((!isAdsShopSlot) ? cachedShopItem.RewardValue.ToString() : CachedManagers.CachedGiftManager.GiftInviteGems.gemsCount.ToString());
			break;
		case 1:
			rewardValueLabelGems.text = ((!isAdsShopSlot) ? cachedShopItem.RewardValue.ToString() : CachedManagers.CachedGiftManager.GiftSubscribeGems.gemsCount.ToString());
			break;
		default:
			rewardValueLabelGems.text = ((!isAdsShopSlot) ? cachedShopItem.RewardValue.ToString() : CachedManagers.CachedGiftManager.GiftAdsGems.ToString());
			break;
		}
		rewardValueLabelCoins.text = rewardValueLabelGems.text;
		invitesRoot.SetActive(CachedManagers.CachedStatisticsManager.FreeGemsReceived <= 1);
		adsRoot.SetActive(CachedManagers.CachedStatisticsManager.FreeGemsReceived > 1);
		regularSlotRoot.SetActive(!isAdsShopSlot);
		timerSlotRoot.SetActive(isAdsShopSlot && timer.IsTimerActive);
		adsAndInvitesRoot.SetActive(isAdsShopSlot && !timer.IsTimerActive);
	}

	public void Refresh()
	{
		if (cachedShopItem != null)
		{
			Init(cachedShopItem);
		}
	}

	private void SetPrice()
	{
		if (cachedShopItem != null)
		{
			string priceString = "观看视频";
			
			//TODO:修改-----改价格
			Debug.LogError("修改-----改价格");
			bool flag = !string.IsNullOrEmpty(priceString);
			priceRoot.SetActive(flag);
			loadingRoot.SetActive(!flag);
			if (flag)
			{
				priceLabel.text = priceString;
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
		if (OnBankItemPurchased != null)
		{
			OnBankItemPurchased();
		}
		string customEventName = ((CachedManagers.CachedGUIScreenManager.ActiveScreen != GUIScreenType.Shop) ? "Mini_Bank_Purchased" : "Bank_Purchased");
		Analytics.CustomEvent(customEventName, new Dictionary<string, object> { { "Purchase_ID", shopItem.ShopItemID } });
		if (shopItem.GetType() == typeof(ShopItemGems) && !CachedManagers.CachedStatisticsManager.AreGemsBought)
		{
			CachedManagers.CachedStatisticsManager.AreGemsBought = true;
			Analytics.CustomEvent("Gems_Bought_Once");
		}
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
	}

	private void BuyButton_OnClick()
	{
		//TODO:激励--购买物品
		
		if (cachedShopItem != null)
		{
			cachedShopItem.OnItemPurchased += ShopItem_OnItemPurchased;
			cachedShopItem.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			tk2dUIManager.Instance.InputEnabled = false;
			tk2dUIManager.Instance.InputEnabled = true;
			if (!cachedShopItem.TryBuy())
			{
				// Debug.LogError($"购买成功");
				cachedShopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
				cachedShopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				tk2dUIManager.Instance.InputEnabled = true;
			}
		}
	}

	private void AdsAndInvitesButton_OnClick()
	{
		int freeGemsReceived = CachedManagers.CachedStatisticsManager.FreeGemsReceived;
		switch (freeGemsReceived)
		{
		case 0:
			(CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.GemsInvite) as GemsInvitePopup).Init(false, CachedManagers.CachedGiftManager.GiftInviteGems.gemsCount);
			return;
		case 1:
			(CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.GemsInvite) as GemsInvitePopup).Init(true, CachedManagers.CachedGiftManager.GiftSubscribeGems.gemsCount);
			return;
		}
		DateTime currentVideoTime = DateTime.Now.Subtract(CachedManagers.CachedStatisticsManager.CheatedTime);
		DateTime lastVideoTime = CustomPlayerPrefs.GetDateTime("free_gems_ads_count_by_day" + 0, currentVideoTime);
		if (!(cachedShopItem is ShopItemGemForAds))
		{
			return;
		}
		ShopItemGemForAds shopItemForAds = cachedShopItem as ShopItemGemForAds;
		CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoAvailable)
		{
			if (isVideoAvailable)
			{
				for (int i = 1; i < shopItemForAds.AdsInfo.adsChainLenght; i++)
				{
					DateTime dateTime = CustomPlayerPrefs.GetDateTime("free_gems_ads_count_by_day" + i, currentVideoTime);
					int num = i - 1;
					CustomPlayerPrefs.SetDateTime("free_gems_ads_count_by_day" + num, dateTime);
				}
				int num2 = shopItemForAds.AdsInfo.adsChainLenght - 1;
				CustomPlayerPrefs.SetDateTime("free_gems_ads_count_by_day" + num2, currentVideoTime);
				CustomPlayerPrefs.SetInt("free_gems_ads_count", CustomPlayerPrefs.GetInt("free_gems_ads_count", 0) + 1);
				CachedManagers.CachedStatisticsManager.AddGems(CachedManagers.CachedGiftManager.GiftAdsGems);
				if ((currentVideoTime - lastVideoTime).TotalSeconds < (double)shopItemForAds.AdsInfo.timeBetweenFirstAndLastChainItem && CustomPlayerPrefs.GetInt("free_gems_ads_count", 0) >= shopItemForAds.AdsInfo.adsChainLenght && !timer.IsTimerActive)
				{
					timer.TimerInSeconds = (float)shopItemForAds.AdsInfo.waitingTime - (float)(currentVideoTime - lastVideoTime).TotalSeconds;
					timer.IsTimerActive = true;
					CustomPlayerPrefs.SetDateTime("free_gems_timer_ended", currentVideoTime.Add(new TimeSpan(0, 0, shopItemForAds.AdsInfo.waitingTime - (int)(currentVideoTime - lastVideoTime).TotalSeconds)));
				}
				Init(cachedShopItem);
				Analytics.CustomEvent("Video_Free_Gems_Finished");
			}
		});
	}

	private void SubscribeManager_OnSubscribeForGems()
	{
		Init(cachedShopItem);
	}

	private void Timer_OnTimerChanged(int time)
	{
		if (isAdsShopSlot)
		{
			timerLabel.text = string.Format("{0}:{1}:{2}", time / 3600, (time / 60 % 60).ToString("00"), (time % 60).ToString("00"));
			if (time == 0)
			{
				Init(cachedShopItem);
			}
		}
	}
}
