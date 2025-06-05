using System;
using UnityEngine;
using UnityEngine.Analytics;

public class ShopItemSubscription : ShopItem
{
	public static Action OnSubscriptionActive;

	[SerializeField]
	private SubscriptionItemType subscriptionType;

	private bool isTrial = true;

	private Managers cachedManagers;

	public SubscriptionItemType SubscriptionType
	{
		get
		{
			return subscriptionType;
		}
	}

	public bool IsTrial
	{
		get
		{
			return isTrial;
		}
		set
		{
			if (isTrial != value)
			{
				isTrial = value;
			}
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

	public override void PurchaseComplete(LLTransactionState transactionState)
	{
		if (transactionState == LLTransactionState.RePurchased)
		{
			Analytics.CustomEvent(string.Format("{0}_{1}", "Shop_Item_Purchased", shopItemID));
		}
		if (transactionState == LLTransactionState.Purchased || transactionState == LLTransactionState.RePurchased)
		{
			GUIPopup activePopup = SingletonMonoBehaviour<GUIPopupManager>.Instance.ActivePopup;
			if (activePopup != null)
			{
				string customEventName = ((activePopup.TypePopup != PopupType.AutoRenewableSubscribeFoolish) ? "Subscription_Trial_Button" : "Subscription_Trial_Foolish");
				Analytics.CustomEvent(customEventName);
			}
		}
		PurchaseSuccessful();
		if (OnSubscriptionActive != null)
		{
			OnSubscriptionActive();
		}
		RemoveAdsPopup.OnRemoveAdsPurchased?.Invoke();
		if (CachedManagers.CachedGUIScreenManager.MenuScreen != null)
		{
			CachedManagers.CachedGUIScreenManager.MenuScreen.MenuScreenScrollView.CheckBoughtCharacters();
		}
	}
}
