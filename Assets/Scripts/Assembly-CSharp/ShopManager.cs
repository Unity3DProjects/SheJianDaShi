using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Prime31;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ShopManager : SingletonMonoBehaviour<ShopManager>
{
	private const string ALL_CHARACTERS_BOUGHT = "AllCharactersBought";

	private const string NO_ADS_BOUGHT = "NoAdsBought";

	private const string HAS_CONVERTED_INAPPS = "unity.player_date";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnTransactionCompleted;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnAllProductsReceived;

	[SerializeField]
	private List<ShopItem> availibleInApps;

	[SerializeField]
	private List<ShopItemCharacter> easyCharacters;

	[SerializeField]
	private List<ShopItemCharacter> newCharacters;

	private List<ShopItemGems> bankItems;

	private List<ShopItemCharacter> availableCharacters;

	private List<ShopItemPack> availablePacks;

	private ShopItem[] items;

	private List<ShopItem> currentPurchaseItems = new List<ShopItem>();

	private Action<bool> restoreCallback;

	private bool isAllProductsReceived;

	private bool championPackBought;

	private bool upgradePackBought;

	private ShopItemVipPack vipPack;

	private Managers cachedManagers;

	public List<ShopItem> AvailableInApps
	{
		get
		{
			return availibleInApps;
		}
	}

	public List<ShopItemCharacter> AvailableCharacters
	{
		get
		{
			if (availableCharacters == null)
			{
				availableCharacters = new List<ShopItemCharacter>();
				for (int i = 0; i < availibleInApps.Count; i++)
				{
					ShopItemCharacter shopItemCharacter = availibleInApps[i] as ShopItemCharacter;
					if (shopItemCharacter != null && shopItemCharacter.Infos[0].characterNameType != CharacterNameType.Kiss)
					{
						availableCharacters.Add(shopItemCharacter);
					}
				}
			}
			return availableCharacters;
		}
	}

	public List<ShopItemCharacter> EasyCharacters
	{
		get
		{
			return easyCharacters;
		}
	}

	public List<ShopItemCharacter> NewCharacters
	{
		get
		{
			return newCharacters;
		}
	}

	public List<ShopItemCharacter> UnlockedCharacters
	{
		get
		{
			List<ShopItemCharacter> list = new List<ShopItemCharacter>();
			List<ShopItemCharacter> list2 = AvailableCharacters;
			for (int i = 0; i < list2.Count; i++)
			{
				ShopItemCharacter shopItemCharacter = list2[i];
				if (shopItemCharacter.Availible)
				{
					list.Add(shopItemCharacter);
				}
			}
			return list;
		}
	}

	public List<ShopItemCharacter> BoughtCharacters
	{
		get
		{
			List<ShopItemCharacter> list = new List<ShopItemCharacter>();
			List<ShopItemCharacter> list2 = AvailableCharacters;
			for (int i = 0; i < list2.Count; i++)
			{
				ShopItemCharacter shopItemCharacter = list2[i];
				if (shopItemCharacter.Bought)
				{
					list.Add(shopItemCharacter);
				}
			}
			return list;
		}
	}

	public List<ShopItemCharacter> DefaultCharacters
	{
		get
		{
			List<ShopItemCharacter> list = new List<ShopItemCharacter>();
			List<ShopItemCharacter> list2 = AvailableCharacters;
			for (int i = 0; i < list2.Count; i++)
			{
				ShopItemCharacter shopItemCharacter = list2[i];
				if (shopItemCharacter.DefaultCharacter)
				{
					list.Add(shopItemCharacter);
				}
			}
			return list;
		}
	}

	public List<ShopItemPack> AvailablePacks
	{
		get
		{
			if (availablePacks == null)
			{
				availablePacks = new List<ShopItemPack>();
				for (int i = 0; i < availibleInApps.Count; i++)
				{
					ShopItemPack shopItemPack = availibleInApps[i] as ShopItemPack;
					if (shopItemPack != null && !(shopItemPack.ShopItemID == "pack.brutal.sale") && !(shopItemPack.ShopItemID == "pack.music.sale") && !(shopItemPack.ShopItemID == "vippacksale"))
					{
						availablePacks.Add(shopItemPack);
					}
				}
			}
			return availablePacks;
		}
	}

	public ShopItemVipPack VipPack
	{
		get
		{
			if (vipPack == null)
			{
				for (int i = 0; i < availibleInApps.Count; i++)
				{
					if (availibleInApps[i].GetType() == typeof(ShopItemVipPack))
					{
						vipPack = availibleInApps[i] as ShopItemVipPack;
						break;
					}
				}
			}
			return vipPack;
		}
	}

	public bool NoAdsBought
	{
		get
		{
			return CustomPlayerPrefs.GetBool("NoAdsBought") || CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		}
		set
		{
			CustomPlayerPrefs.SetBool("NoAdsBought", value);
		}
	}

	public bool TrialActivated
	{
		get
		{
			return !(SubscriptionWeek as ShopItemSubscription).IsTrial;
		}
	}

	public bool AllCharactersBought
	{
		get
		{
			return CustomPlayerPrefs.GetBool("AllCharactersBought");
		}
		set
		{
			CustomPlayerPrefs.SetBool("AllCharactersBought", value);
		}
	}

	public bool AllIncludedBought
	{
		get
		{
			return NoAdsBought && AllCharactersBought;
		}
	}

	public List<WeaponType> AvailableWeapons
	{
		get
		{
			ObjectsSpawner objectsSpawner = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner;
			List<WeaponType> list = new List<WeaponType>();
			List<ShopItemCharacter> unlockedCharacters = UnlockedCharacters;
			for (int i = 0; i < unlockedCharacters.Count; i++)
			{
				ShopItemCharacter shopItemCharacter = unlockedCharacters[i];
				for (int j = 0; j < shopItemCharacter.Infos.Count; j++)
				{
					Character character = objectsSpawner.GetCharacter(shopItemCharacter.Infos[j].characterNameType);
					WeaponType item = character.AvailableWeapons[0];
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			return list;
		}
	}

	public string DefaultCharacterProductID
	{
		get
		{
			List<ShopItemCharacter> list = AvailableCharacters;
			List<ShopItemCharacter> list2 = new List<ShopItemCharacter>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].DefaultCharacter)
				{
					list2.Add(list[i]);
				}
			}
			return list2.RandomObject().ProductID;
		}
	}

	private bool AllProductsReceived
	{
		get
		{
			ShopItem[] array = AvailableInApps.ToArray();
			ShopItem[] array2 = array;
			foreach (ShopItem shopItem in array2)
			{
				if (!SingletonMonoBehaviour<MCInnAppsmanager>.Instance.IsQueryInventoryOK)
				{
					return false;
				}
			}
			if (!isAllProductsReceived)
			{
				isAllProductsReceived = true;
				ShopManager.m_OnAllProductsReceived?.Invoke();
			}
			return true;
		}
	}

	public List<ShopItemGems> BankItems
	{
		get
		{
			if (bankItems == null)
			{
				bankItems = new List<ShopItemGems>();
				for (int i = 0; i < availibleInApps.Count; i++)
				{
					ShopItem shopItem = availibleInApps[i];
					if (shopItem is ShopItemGems && shopItem != null)
					{
						bankItems.Add(shopItem as ShopItemGems);
					}
				}
			}
			return bankItems;
		}
	}

	public ShopItem SubscriptionWeek
	{
		get
		{
			for (int i = 0; i < availibleInApps.Count; i++)
			{
				if (availibleInApps[i] is ShopItemSubscription && (availibleInApps[i] as ShopItemSubscription).SubscriptionType == SubscriptionItemType.Week)
				{
					return availibleInApps[i];
				}
			}
			return null;
		}
	}

	public ShopItem SubscriptionMonth
	{
		get
		{
			for (int i = 0; i < availibleInApps.Count; i++)
			{
				if (availibleInApps[i] is ShopItemSubscription && (availibleInApps[i] as ShopItemSubscription).SubscriptionType == SubscriptionItemType.Month)
				{
					return availibleInApps[i];
				}
			}
			return null;
		}
	}

	public ShopItem SubscriptionYear
	{
		get
		{
			for (int i = 0; i < availibleInApps.Count; i++)
			{
				if (availibleInApps[i] is ShopItemSubscription && (availibleInApps[i] as ShopItemSubscription).SubscriptionType == SubscriptionItemType.Year)
				{
					return availibleInApps[i];
				}
			}
			return null;
		}
	}

	public bool IsPurchasesInSandbox
	{
		get
		{
			return MCInnAppsmanager.IsPurchasesInSandbox;
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

	public static event Action OnTransactionCompleted
	{
		add
		{
			Action action = ShopManager.m_OnTransactionCompleted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ShopManager.m_OnTransactionCompleted, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = ShopManager.m_OnTransactionCompleted;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ShopManager.m_OnTransactionCompleted, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnAllProductsReceived
	{
		add
		{
			Action action = ShopManager.m_OnAllProductsReceived;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ShopManager.m_OnAllProductsReceived, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = ShopManager.m_OnAllProductsReceived;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref ShopManager.m_OnAllProductsReceived, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private new void Awake()
	{
		base.Awake();
		if (CustomPlayerPrefs.HasKey("unity.player_date"))
		{
			return;
		}
		foreach (ShopItemCharacter availableCharacter in AvailableCharacters)
		{
			if (CustomPlayerPrefs.GetBool(availableCharacter.ProductID))
			{
				string value = MiniclipUtils.ConvertStringToEncriptedHex(availableCharacter.ProductID, availableCharacter.ShopItemID + SystemInfo.deviceModel);
				CustomPlayerPrefs.SetString(availableCharacter.ProductID, value);
			}
		}
		CustomPlayerPrefs.SetDateTime("unity.player_date", DateTime.Now);
	}

	private void OnEnable()
	{
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.ReceivedProductsIDS += MCInnAppsManager_ReceivedProductIDS;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseFailed += LLStoreManager_OnPurchaseFailed;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseSuccessful += MCInnAppsManager_PurchaseSuccessful;
	}

	private void Start()
	{
		if (AllIncludedBought)
		{
			List<ShopItemCharacter> list = AvailableCharacters;
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].Bought && (!list[i].IsLockedByTimer || (list[i].IsLockedByTimer && list[i].IsTimeEnded)))
				{
					list[i].PurchaseComplete(LLTransactionState.RePurchased);
				}
			}
		}
		InAppPurchasesRequest(AvailableInApps);
		StartCoroutine(CoroutineOpenCharacter());
	}

	private void OnDisable()
	{
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.ReceivedProductsIDS -= MCInnAppsManager_ReceivedProductIDS;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseFailed -= LLStoreManager_OnPurchaseFailed;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseSuccessful -= MCInnAppsManager_PurchaseSuccessful;
	}

	public bool IsProductConsumable(string productID)
	{
		ShopItem shopItem = AvailableInApps.Find((ShopItem x) => x.ProductID == productID);
		return shopItem.IsConsumable();
	}

	public bool CanMakePurchase()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.NoInternet, true, null, true);
			return false;
		}
		return true;
	}

	public void PurchaseProduct(ShopItem item)
	{
		if (item.IsDataReceived && !currentPurchaseItems.Exists((ShopItem t) => t.ProductID.Equals(item.ProductID)))
		{
			currentPurchaseItems.Add(item);
			SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseProduct(item.ProductID);
		}
	}

	public void RestorePurchases(Action<bool> callback)
	{
		restoreCallback = callback;
	}

	public void CheckForNotReceivedItems()
	{
	}

	public bool IsCoinsEnoughForPurchase(int additionalCoins = 0)
	{
		int num = SingletonMonoBehaviour<StatisticsManager>.Instance.Coins + additionalCoins;
		for (int i = 0; i < AvailableCharacters.Count; i++)
		{
			ShopItemCharacter shopItemCharacter = AvailableCharacters[i];
			if (!shopItemCharacter.Bought && !shopItemCharacter.IsPremium && shopItemCharacter.CanBuyInShop && num >= shopItemCharacter.PriceCoins)
			{
				return true;
			}
		}
		return false;
	}

	public ShopItemCharacter GetShopItemCharacter(string shopItemID)
	{
		for (int i = 0; i < AvailableCharacters.Count; i++)
		{
			if (AvailableCharacters[i].ShopItemID == shopItemID)
			{
				return AvailableCharacters[i];
			}
		}
		return null;
	}

	public List<string> GetInAppPurchasesIds(List<ShopItem> itemsList)
	{
		List<string> list = new List<string>();
		foreach (ShopItem items in itemsList)
		{
			list.Add(items.ProductID);
			if (items.GetType() == typeof(ShopItemPack))
			{
				list.Add((items as ShopItemPack).ProductFullPriceID);
			}
			if (items.GetType() == typeof(ShopItemUpgradeNew))
			{
				list.Add((items as ShopItemUpgradeNew).ProductFullPriceID);
			}
			if (items.GetType() == typeof(ShopItemVipPack))
			{
				list.Add((items as ShopItemVipPack).ProductFullPriceID);
			}
			if (items.GetType() == typeof(ShopItemChampionNew))
			{
				list.Add((items as ShopItemChampionNew).ProductFullPriceID);
			}
		}
		return list;
	}

	public void InAppPurchasesRequest(List<ShopItem> itemsList)
	{
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.QueryInventory();
	}

	private void CheckAvailableInApps()
	{
	}

	private void MCInnAppsManager_ReceivedProductIDS(List<GooglePurchase> purchasesList, List<GoogleSkuInfo> skuInfoList)
	{
		foreach (GooglePurchase purchase in purchasesList)
		{
			ShopItem shopItem = AvailableInApps.Find((ShopItem x) => x.ProductID.Equals(purchase.productId));
			if (!(shopItem == null) && !shopItem.IsDataReceived)
			{
				if (shopItem.IsConsumable())
				{
					Debug.Log("MCInnAppsManager_ReceivedProductIDS::IsConsumable" + purchase.productId);
					SingletonMonoBehaviour<MCInnAppsmanager>.Instance.ConsumeProduct(purchase.productId);
				}
				else
				{
					shopItem.PurchaseComplete(LLTransactionState.Purchased);
				}
			}
		}
		foreach (GoogleSkuInfo skuInfo in skuInfoList)
		{
			ShopItem shopItem2 = AvailableInApps.Find((ShopItem x) => x.ProductID.Equals(skuInfo.productId));
			if (shopItem2 != null && !shopItem2.IsDataReceived)
			{
				shopItem2.UpdatePrice();
				shopItem2.IsDataReceived = true;
			}
		}
	}

	private void MCInnAppsManager_PurchaseSuccessful(GooglePurchase purchase, Receipt receipt)
	{
		ShopItem shopItem = AvailableInApps.Find((ShopItem x) => x.ProductID.Equals(purchase.productId));
		if (shopItem != null)
		{
			currentPurchaseItems.Remove(shopItem);
			shopItem.PurchaseComplete(LLTransactionState.Purchased);
			ShopManager.m_OnTransactionCompleted?.Invoke();
			CustomPlayerPrefs.Save();
		}
	}

	private void LLStoreManager_OnPurchaseFailed(string productID, string rewardID)
	{
		ShopItem[] array = currentPurchaseItems.ToArray();
		ShopItem[] array2 = array;
		foreach (ShopItem shopItem in array2)
		{
			if (shopItem.ProductID.Equals(productID))
			{
				shopItem.PurchaseFailed();
			}
		}
		currentPurchaseItems.Clear();
	}

	private void LLStoreManager_OnRestoreComplete(bool succeded)
	{
		if (restoreCallback != null)
		{
			restoreCallback(succeded);
			restoreCallback = null;
		}
	}

	private IEnumerator CoroutineOpenCharacter()
	{
		while (!AllProductsReceived)
		{
			CheckForNotReceivedItems();
			yield return new WaitForSeconds(5f);
		}
	}
}
