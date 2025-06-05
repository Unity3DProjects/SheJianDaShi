using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class RemoveAdsPopup : GUIPopup
{
	public static Action OnRemoveAdsPurchased;

	private const string EVENT_KICK = "Kick";

	private const string EVENT_PHANTOM_CREED_KICK = "Kick_Phantom_Creed";

	private const string EVENT_HARLEE_KICK = "Kick_Harley";

	[SerializeField]
	private tk2dTextMesh coinsLabel;

	[SerializeField]
	private tk2dUIItem buyButton;

	[SerializeField]
	private GameObject priceRoot;

	[SerializeField]
	private GameObject loadingRoot;

	[SerializeField]
	private tk2dTextMesh currentPriceLabel;

	private ShopItemNoAds currentShopItem;

	[SerializeField]
	[ResourceLink]
	private AssetLink smokeEffectPrefab;

	[SerializeField]
	private SkeletonAnimation adsManAnimation;

	[SerializeField]
	private LanguageSkeletonDataAsset defaultLanguageAsset;

	[SerializeField]
	private List<LanguageSkeletonDataAsset> otherLanguagesAssets;

	private EventData cachedEventKick;

	private EventData cachedEventHarleyKick;

	private EventData cachedEventPhantomCreedKick;

	private EventData CachedEventKick
	{
		get
		{
			if (cachedEventKick == null)
			{
				cachedEventKick = adsManAnimation.Skeleton.Data.FindEvent("Kick");
			}
			return cachedEventKick;
		}
	}

	private EventData CachedEventHarleyKick
	{
		get
		{
			if (cachedEventHarleyKick == null)
			{
				cachedEventHarleyKick = adsManAnimation.Skeleton.Data.FindEvent("Kick_Harley");
			}
			return cachedEventHarleyKick;
		}
	}

	private EventData CachedEventPhantomCreeKick
	{
		get
		{
			if (cachedEventPhantomCreedKick == null)
			{
				cachedEventPhantomCreedKick = adsManAnimation.Skeleton.Data.FindEvent("Kick_Phantom_Creed");
			}
			return cachedEventPhantomCreedKick;
		}
	}

	private void Awake()
	{
		SystemLanguage currentLanguage = SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage;
		LanguageSkeletonDataAsset languageSkeletonDataAsset = defaultLanguageAsset;
		if (!defaultLanguageAsset.languages.Contains(currentLanguage))
		{
			for (int i = 0; i < otherLanguagesAssets.Count; i++)
			{
				if (otherLanguagesAssets[i].languages.Contains(currentLanguage))
				{
					languageSkeletonDataAsset = otherLanguagesAssets[i];
					break;
				}
			}
		}
		adsManAnimation.SkeletonDataAsset = languageSkeletonDataAsset.skeletonDataAsset;
		adsManAnimation.Reset();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		buyButton.OnClick += BuyButton_OnClick;
		currentShopItem = SingletonMonoBehaviour<ProposalManager>.Instance.RemoveAds;
		currentShopItem.OnPriceChanged += ShopItem_OnPriceChanged;
		adsManAnimation.State.Event += SkeletonAnimation_State_Event;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		buyButton.OnClick -= BuyButton_OnClick;
		if (currentShopItem != null)
		{
			currentShopItem.OnPriceChanged -= ShopItem_OnPriceChanged;
		}
		adsManAnimation.State.Event -= SkeletonAnimation_State_Event;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SetPrice();
		coinsLabel.text = currentShopItem.CoinsReward.ToString();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	protected virtual void SetPrice()
	{
		if (currentShopItem != null)
		{
			string priceString = currentShopItem.PriceString;
			bool flag = !string.IsNullOrEmpty(priceString);
			priceRoot.SetActive(flag);
			loadingRoot.SetActive(!flag);
			if (flag)
			{
				currentPriceLabel.text = priceString;
			}
		}
	}

	private void ShopItem_OnPriceChanged(ShopItem shopItem)
	{
		SetPrice();
	}

	private void BuyButton_OnClick()
	{
		if (currentShopItem != null && !currentShopItem.Bought)
		{
			currentShopItem.OnItemPurchased += ShopItem_OnItemPurchased;
			currentShopItem.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			tk2dUIManager.Instance.InputEnabled = false;
			if (!currentShopItem.TryBuy())
			{
				currentShopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
				currentShopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				tk2dUIManager.Instance.InputEnabled = true;
			}
		}
	}

	protected virtual void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
		if (OnRemoveAdsPurchased != null)
		{
			OnRemoveAdsPurchased();
		}
		Hide();
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
	}

	private void SkeletonAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventKick && smokeEffectPrefab != null && !string.IsNullOrEmpty(smokeEffectPrefab.assetGUID))
		{
			GameObject smoke = (GameObject)smokeEffectPrefab.GetInstance();
			smoke.transform.parent = adsManAnimation.transform;
			smoke.transform.position = adsManAnimation.transform.position;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				if (smoke != null)
				{
					UnityEngine.Object.Destroy(smoke);
					smoke = null;
				}
			}, 1.3f);
		}
		if (e.Data == CachedEventHarleyKick)
		{
			AudioManager.PlayAudioWithKey("remove_ads_popup_harley_kick");
		}
		if (e.Data == CachedEventPhantomCreeKick)
		{
			AudioManager.PlayAudioWithKey("remove_ads_popup_phantom_creed_kick");
		}
	}
}
