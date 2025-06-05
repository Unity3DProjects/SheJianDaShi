using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Mit_Goliath_Native.Utils;
using Prime31;
using UnityEngine;

public class MCInnAppsmanager : SingletonMonoBehaviour<MCInnAppsmanager>, ReceiptValidatorDelegate
{
	public static class ConsumeResultForwarder
	{
		public static void OnConsumeSucceeeded(GooglePurchase purchase, Consumable consumable)
		{
			SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseSuccessful(purchase, consumable.Receipt);
		}

		public static void OnConsumeFailed(string productId, string error)
		{
			SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseFailed(productId, error);
		}
	}

	private const int MAX_CONSUME_ATTEMPTS = 3;

	private const string PURCHASES_KEY = "purchases_key";

	private const string SKUS_KEY = "skus_key";

	private bool isBillingSupported;

	private bool isInitingIAB;

	private bool isQueryingInventory;

	private bool isQueryInventoryOk;

	private bool hasValidated;

	private bool isValid;

	private bool isMakingPurchase;

	private bool isProbingNetwork;

	private GooglePurchase purchasedProduct;

	private GoogleSkuInfo attemptedPurchaseProduct;

	private ReceiptValidator receiptValidator;

	private Receipt lastReceipt;

	private ProcessConsumables processConsumables;

	private List<GooglePurchase> purchacesList;

	private List<GoogleSkuInfo> skusList;

	private List<SubscriptionResult> activeSubscriptions;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<bool> m_OnQueryInventoryCompleted;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<List<GooglePurchase>, List<GoogleSkuInfo>> m_ReceivedProductsIDS;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<string, string> m_PurchaseFailed;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<GooglePurchase, Receipt> m_PurchaseSuccessful;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnRequestPurchase;

	public bool IsQueryInventoryOK
	{
		get
		{
			return isQueryInventoryOk;
		}
	}

	public static bool IsPurchasesInSandbox
	{
		get
		{
			return Debug.isDebugBuild;
		}
	}

	public event Action<bool> OnQueryInventoryCompleted
	{
		add
		{
			Action<bool> action = this.m_OnQueryInventoryCompleted;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnQueryInventoryCompleted, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = this.m_OnQueryInventoryCompleted;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnQueryInventoryCompleted, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<List<GooglePurchase>, List<GoogleSkuInfo>> ReceivedProductsIDS
	{
		add
		{
			Action<List<GooglePurchase>, List<GoogleSkuInfo>> action = this.m_ReceivedProductsIDS;
			Action<List<GooglePurchase>, List<GoogleSkuInfo>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_ReceivedProductsIDS, (Action<List<GooglePurchase>, List<GoogleSkuInfo>>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<List<GooglePurchase>, List<GoogleSkuInfo>> action = this.m_ReceivedProductsIDS;
			Action<List<GooglePurchase>, List<GoogleSkuInfo>> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_ReceivedProductsIDS, (Action<List<GooglePurchase>, List<GoogleSkuInfo>>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<string, string> PurchaseFailed
	{
		add
		{
			Action<string, string> action = this.m_PurchaseFailed;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_PurchaseFailed, (Action<string, string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string> action = this.m_PurchaseFailed;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_PurchaseFailed, (Action<string, string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<GooglePurchase, Receipt> PurchaseSuccessful
	{
		add
		{
			Action<GooglePurchase, Receipt> action = this.m_PurchaseSuccessful;
			Action<GooglePurchase, Receipt> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_PurchaseSuccessful, (Action<GooglePurchase, Receipt>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GooglePurchase, Receipt> action = this.m_PurchaseSuccessful;
			Action<GooglePurchase, Receipt> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_PurchaseSuccessful, (Action<GooglePurchase, Receipt>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnRequestPurchase
	{
		add
		{
			Action action = this.m_OnRequestPurchase;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRequestPurchase, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnRequestPurchase;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRequestPurchase, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private new void Awake()
	{
		base.Awake();
		receiptValidator = new ReceiptValidator();
		processConsumables = new ProcessConsumables();
		purchacesList = new List<GooglePurchase>();
		skusList = new List<GoogleSkuInfo>();
		activeSubscriptions = new List<SubscriptionResult>();
		if (PlayerPrefs.HasKey("purchases_key"))
		{
			List<GooglePurchase> list = JsonConvert.DeserializeObject<List<GooglePurchase>>(PlayerPrefs.GetString("purchases_key"));
			if (list != null)
			{
				purchacesList.AddRange(list);
			}
		}
		if (PlayerPrefs.HasKey("skus_key"))
		{
			List<GoogleSkuInfo> list2 = JsonConvert.DeserializeObject<List<GoogleSkuInfo>>(PlayerPrefs.GetString("skus_key"));
			if (list2 != null)
			{
				skusList.AddRange(list2);
			}
		}
		InitGoogleIAB();
	}

	private void Update()
	{
		receiptValidator.Update();
		if (!isQueryingInventory)
		{
			processConsumables.Update();
		}
	}

	public virtual void OnEnable()
	{
		Log("OnEnable");
		GoogleIABManager.purchaseSucceededEvent += PurchaseSucceededCallback;
		GoogleIABManager.purchaseFailedEvent += PurchaseFailedCallback;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += PurchaseCompleteAwaitingVerification;
		processConsumables.OnEnable();
	}

	public virtual void OnDisable()
	{
		Log("OnDisable");
		GoogleIABManager.purchaseSucceededEvent -= PurchaseSucceededCallback;
		GoogleIABManager.purchaseFailedEvent -= PurchaseFailedCallback;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= PurchaseCompleteAwaitingVerification;
		processConsumables.OnDisable();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus && isBillingSupported)
		{
			QueryInventory();
		}
	}

	private void OnApplicationQuit()
	{
		GoogleIAB.unbindService();
	}

	private void InitGoogleIAB()
	{
		if (!isInitingIAB)
		{
			Log("InitGoogleIAB");
			isInitingIAB = true;
			GoogleIABManager.billingSupportedEvent += BillingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent += BillingNotSupportedEvent;
			GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAp6tdEbJJy1jacbXV9fk0/j5jMKTzqAQLEYFBakQWejpYA0tC90tpJvKo7YlIEcdBComXgiARFsO/4Q+so1bCtC+shJsBEbOQhUIi55FH8g9Lkiai+uuFpgEeLeq7vVDfJ33ilqu0o8UQId5tcYI5heVUOBzCh+Zxgqh0kt95JB3/WBK7/pm/6UgOdYNMJOrlhMVvRyKE0ZWAjbmm8CWoLKNNMnU1BNgEarTiwsnwJmrRNMZn5cJX+w/pu9+f7kh16KIg1OMAlS+tsPcfYgRwZ7vTkrraysljvMTNkyuUK9SzKe+Aw45WyGAqB8YcR9iqUoyBOWKp+9rrhY0D4wPJZQIDAQAB");
		}
	}

	private void BillingSupportedEvent()
	{
		Log("BillingSupportedEvent");
		GoogleIABManager.billingSupportedEvent -= BillingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= BillingNotSupportedEvent;
		isInitingIAB = false;
		isBillingSupported = true;
		QueryInventory();
	}

	private void BillingNotSupportedEvent(string message)
	{
		Log("BillingNotSupportedEvent::" + message);
		GoogleIABManager.billingSupportedEvent -= BillingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= BillingNotSupportedEvent;
		isInitingIAB = false;
		isBillingSupported = false;
	}

	public void QueryInventory()
	{
		if (!MiniclipUtils.isNetworkAvailable())
		{
			if (!isProbingNetwork)
			{
				Log("QueryInventory::STARTING PROBE");
				StartCoroutine(NetworkProber());
			}
		}
		else if (!isBillingSupported)
		{
			Log("No billing supported");
			InitGoogleIAB();
		}
		else if (isQueryingInventory)
		{
			Log("Is Already Querying");
		}
		else
		{
			Log("QueryInventory");
			GoogleIABManager.queryInventorySucceededEvent += QueryInventorySucceded;
			GoogleIABManager.queryInventoryFailedEvent += QueryInventoryFailed;
			List<string> inAppPurchasesIds = SingletonMonoBehaviour<ShopManager>.Instance.GetInAppPurchasesIds(SingletonMonoBehaviour<ShopManager>.Instance.AvailableInApps);
			GoogleIAB.queryInventory(inAppPurchasesIds.ToArray());
			isQueryingInventory = true;
		}
	}

	private void QueryInventorySucceded(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Log("QueryInventorySucceded::purchases[" + purchases.Count + "] skus[" + skus.Count + "]");
		GoogleIABManager.queryInventorySucceededEvent -= QueryInventorySucceded;
		GoogleIABManager.queryInventoryFailedEvent -= QueryInventoryFailed;
		StopCoroutine(NetworkProber());
		isProbingNetwork = false;
		purchacesList.Clear();
		purchacesList.AddRange(purchases);
		CustomPlayerPrefs.SetString("purchases_key", JsonConvert.SerializeObject(purchacesList));
		skusList.Clear();
		skusList.AddRange(skus);
		CustomPlayerPrefs.SetString("skus_key", JsonConvert.SerializeObject(skusList));
		CustomPlayerPrefs.Save();
		activeSubscriptions = ExtractSubsFromPurchases(purchacesList, skusList);
		this.OnQueryInventoryCompleted(true);
		isQueryingInventory = false;
		isQueryInventoryOk = true;
		this.ReceivedProductsIDS(purchases, skusList);
	}

	private void QueryInventoryFailed(string error)
	{
		Log("QueryInventoryFailed::" + error);
		GoogleIABManager.queryInventorySucceededEvent -= QueryInventorySucceded;
		GoogleIABManager.queryInventoryFailedEvent -= QueryInventoryFailed;
		isQueryingInventory = false;
		isQueryInventoryOk = false;
		this.OnQueryInventoryCompleted(false);
	}

	public void PurchaseProduct(string productID)
	{
		Log("PurchaseProduct::" + productID);
		if (!isBillingSupported && !isQueryInventoryOk && MiniclipUtils.isNetworkAvailable())
		{
			QueryInventory();
			return;
		}
		this.OnRequestPurchase();
		GoogleIAB.purchaseProduct(productID);
		isMakingPurchase = true;
		attemptedPurchaseProduct = GetStoreProductForGoogleSkuWithID(productID);
	}

	private void PurchaseCompleteAwaitingVerification(string purchaseData, string signature)
	{
		Log("PurchaseCompleteAwaitingVerification::" + purchaseData + "::" + signature);
		hasValidated = false;
		isValid = false;
		receiptValidator.ValidateReceipt(purchaseData, signature);
	}

	private void PurchaseSucceededCallback(GooglePurchase purchase)
	{
		Log("PurchaseSucceeded::" + purchase.ToString());
		attemptedPurchaseProduct = null;
		if (!hasValidated)
		{
			MiniclipUtils.ShowReceiptValidatingPopup(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("system.popup.receiptvalidation.title"), SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("system.popup.receiptvalidation.text"));
			purchasedProduct = purchase;
		}
		else if (hasValidated)
		{
			Receipt receipt = ((lastReceipt != null) ? lastReceipt : new Receipt(string.Empty, string.Empty));
			purchacesList.Add(purchase);
			ProcessPurchase(purchase, receipt);
		}
	}

	private void PurchaseFailedCallback(string message, int response)
	{
		Log("PurchaseFailed::" + message + "::" + response);
		this.PurchaseFailed(attemptedPurchaseProduct.productId, string.Empty);
		attemptedPurchaseProduct = null;
		isMakingPurchase = false;
	}

	public bool IsProductAvailable(string productID)
	{
		return skusList.Find((GoogleSkuInfo x) => x.productId.Equals(productID)) != null;
	}

	public double PriceForProduct(string productID)
	{
		return (double)skusList.Find((GoogleSkuInfo x) => x.productId.Equals(productID)).priceAmountMicros / 1000000.0;
	}

	public int GetNumberPurchases()
	{
		return purchacesList.Count;
	}

	public LLStoreProduct GetStoreProductForSkuWithID(string productID)
	{
		GoogleSkuInfo googleSkuInfo = skusList.Find((GoogleSkuInfo x) => x.productId.Equals(productID));
		if (googleSkuInfo == null)
		{
			return null;
		}
		return new LLStoreProduct(googleSkuInfo.productId, googleSkuInfo.title, googleSkuInfo.price, googleSkuInfo.priceAmountMicros, googleSkuInfo.type == "subs");
	}

	public GoogleSkuInfo GetStoreProductForGoogleSkuWithID(string productID)
	{
		return skusList.Find((GoogleSkuInfo x) => x.productId.Equals(productID));
	}

	public SubscriptionResult SubscriptionForDate(DateTime date)
	{
		return activeSubscriptions.Find((SubscriptionResult sub) => date < sub.expirationDate && date > default(DateTime));
	}

	public SubscriptionResult SubscriptionForID(string subscriptionID)
	{
		return activeSubscriptions.Find((SubscriptionResult sub) => sub.productID == subscriptionID);
	}

	public SubscriptionResult LastSubscription()
	{
		return activeSubscriptions.LastObject();
	}

	public void OnInvalidReceiptCallback(int error, IntPtr receipt)
	{
		Debug.Log("Invalid receipt");
		isValid = false;
		hasValidated = true;
		attemptedPurchaseProduct = GetStoreProductForGoogleSkuWithID(purchasedProduct.productId);
		InvalidReceiptBypass(receipt);
		MiniclipUtils.DismissReceiptValidatingPopup();
		SendInvalidReceiptEvent(purchasedProduct, receiptValidator.NativeReceiptToCSharp(receipt), error);
	}

	public void OnUnableToValidateCallback(int error, IntPtr receipt)
	{
		if (isMakingPurchase && purchasedProduct != null)
		{
			Log("UnableToValidateReceipt(" + error + ", " + receipt + ") - For product \"" + purchasedProduct.productId);
			attemptedPurchaseProduct = GetStoreProductForGoogleSkuWithID(purchasedProduct.productId);
			MiniclipUtils.DismissReceiptValidatingPopup();
			PurchaseFailedCallback("Unable to validate", error);
		}
		isValid = false;
		hasValidated = true;
	}

	public void InvalidReceiptBypass(IntPtr receipt)
	{
		isValid = false;
		OnValidReceiptResponse(receipt);
	}

	public void OnValidReceiptResponse(IntPtr receipt)
	{
		lastReceipt = receiptValidator.NativeReceiptToCSharp(receipt);
		if (purchasedProduct != null)
		{
			Log("SuccessReceiptValidation(" + receipt + ", " + purchasedProduct.ToString() + ")");
			purchacesList.Add(purchasedProduct);
			activeSubscriptions = ExtractSubsFromPurchases(purchacesList, skusList);
			ProcessPurchase(purchasedProduct, lastReceipt);
			purchasedProduct = null;
		}
		else if (isMakingPurchase)
		{
			hasValidated = true;
		}
		else
		{
			isMakingPurchase = false;
			this.PurchaseFailed(string.Empty, string.Empty);
		}
	}

	public void OnSetSuccessCallback(IntPtr receipt)
	{
		isValid = true;
		OnValidReceiptResponse(receipt);
	}

	public void ConsumeProduct(string productId, Receipt receipt = null)
	{
		processConsumables.CreateConsumable(productId, (receipt != null) ? receipt : new Receipt());
	}

	private void ProcessPurchase(GooglePurchase purchaseProduct, Receipt receipt)
	{
		if (SingletonMonoBehaviour<ShopManager>.Instance.IsProductConsumable(purchaseProduct.productId))
		{
			ConsumeProduct(purchaseProduct.productId, receipt);
		}
		else
		{
			this.PurchaseSuccessful(purchaseProduct, receipt);
			MiniclipUtils.DismissReceiptValidatingPopup();
		}
		if (purchasedProduct != null)
		{
			SendInAppEvent(purchasedProduct, receipt);
		}
	}

	private IEnumerator NetworkProber()
	{
		isProbingNetwork = true;
		while (!isQueryInventoryOk)
		{
			if (MiniclipUtils.isNetworkAvailable())
			{
				QueryInventory();
			}
			yield return new WaitForSeconds(30f);
		}
		isProbingNetwork = false;
	}

	private static List<SubscriptionResult> ExtractSubsFromPurchases(List<GooglePurchase> purchases, List<GoogleSkuInfo> availableSkus)
	{
		List<GoogleSkuInfo> list = new List<GoogleSkuInfo>(availableSkus.FindAll((GoogleSkuInfo sku) => sku.type == "subs"));
		List<SubscriptionResult> list2 = new List<SubscriptionResult>();
		foreach (GooglePurchase purchase in purchases)
		{
			GoogleSkuInfo googleSkuInfo = list.Find((GoogleSkuInfo sku) => sku.productId == purchase.productId);
			if (googleSkuInfo != null && !(googleSkuInfo.type != "subs"))
			{
				SubscriptionResult item = default(SubscriptionResult);
				item.productID = purchase.productId;
				item.subscriptionID = purchase.orderId;
				item.purchaseDate = LLStoreManager.ConvertFromUnixTimestamp(purchase.purchaseTime / 1000);
				item.expirationDate = AddISO8601DurationToDate(item.purchaseDate, googleSkuInfo.subscriptionPeriod);
				list2.Add(item);
			}
		}
		list2.Sort((SubscriptionResult x, SubscriptionResult y) => x.purchaseDate.CompareTo(y.purchaseDate));
		return list2;
	}

	private static DateTime AddISO8601DurationToDate(DateTime time, string iso8601Duration)
	{
		char c = iso8601Duration[iso8601Duration.Length - 1];
		int num = int.Parse(iso8601Duration.Substring(1, iso8601Duration.Length - 2));
		switch (c)
		{
		case 'D':
			return time.AddDays(num);
		case 'W':
			return time.AddDays(7 * num);
		case 'M':
			return time.AddMonths(num);
		case 'Y':
			return time.AddYears(num);
		default:
			return time;
		}
	}

	private string GetProductFamily(ShopItem item)
	{
		string empty = string.Empty;
		if (item is ShopItemCharacter)
		{
			return "character";
		}
		if (item is ShopItemDoubleCoins)
		{
			return "promos";
		}
		if (item is ShopItemSubscription)
		{
			return "subscription";
		}
		if (item is ShopItemSubscription)
		{
			return "packs";
		}
		if (item is ShopItemCoins)
		{
			return "coins";
		}
		if (item is ShopItemGems)
		{
			return "gems";
		}
		if (item is ShopItemPack)
		{
			return "pack";
		}
		return "unknown";
	}

	private void SendInvalidReceiptEvent(GooglePurchase purchase, Receipt receipt, int error)
	{
		ShopItem shopItem = SingletonMonoBehaviour<ShopManager>.Instance.AvailableInApps.Find((ShopItem x) => x.ProductID.Equals(purchase.productId));
		float num = (float)(shopItem.PriceTier - 1) + 0.99f;
		GameGoliath.Instance.TrackInvalidInAppReceipt(purchase.orderId, purchase.productId, shopItem.ShopItemID, GetProductFamily(shopItem), num, receipt, error, string.Empty);
	}

	private void SendInAppEvent(GooglePurchase purchase, Receipt receipt)
	{
		ShopItem shopItem = SingletonMonoBehaviour<ShopManager>.Instance.AvailableInApps.Find((ShopItem x) => x.ProductID.Equals(purchase.productId));
		float num = (float)(shopItem.PriceTier - 1) + 0.99f;
		if (shopItem is ShopItemCharacter && isValid)
		{
			ShopItemCharacter shopItemCharacter = shopItem as ShopItemCharacter;
			GameGoliath.Instance.TrackCharacterAction(shopItemCharacter.GetCharacterID(), GoliathIds.Action.ACQUISITION, GoliathIds.Origin.IN_APP_PURCHASE, purchase.orderId);
		}
		else if (shopItem is ShopItemSubscription)
		{
			if (isValid)
			{
				GameGoliath.Instance.TrackSubscriptonAction(shopItem.ShopItemID, GoliathIds.Action.PURCHASED, purchase.orderId);
				SingletonMonoBehaviour<AdjustManager>.Instance.TrackSubscriptionSuccess();
				SingletonMonoBehaviour<AdjustManager>.Instance.TrackSubscriptionRevenue(num);
			}
			else
			{
				SingletonMonoBehaviour<AdjustManager>.Instance.TrackSubscriptionInitiated();
			}
		}
		if (isValid)
		{
			GameGoliath.Instance.TrackInAppPurchase(purchase.orderId, purchase.productId, shopItem.ShopItemID, GetProductFamily(shopItem), num, receipt, string.Empty);
			SingletonMonoBehaviour<AdjustManager>.Instance.TrackRevenue(num);
		}
	}

	private void Log(string message)
	{
	}
}
