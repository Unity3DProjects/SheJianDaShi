using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;

public sealed class LLStoreManager
{
	private delegate void LLStoreManagerProductDelegate(string productID, bool isValidProduct);

	private delegate void LLStoreManagerPurchaseSuccessDelegate(string productID, string rewardID, string transactionID, int transactionState);

	private delegate void LLStoreManagerPurchaseFailedDelegate(string productID, string rewardID);

	private delegate void LLStoreManagerRestoreDelegate(bool status);

	private const string METHOD_REGISTER_CALLBACKS = "LLStoreManagerRegisterCallbacks";

	private const string METHOD_REQUEST_PRODUCTS = "LLStoreManagerRequestProducts";

	private const string METHOD_SET_CONSUMABLE_PRODUCTS = "LLStoreManagerSetConsumableProducts";

	private const string METHOD_PURCHASE_PRODUCT = "LLStoreManagerPurchaseProduct";

	private const string METHOD_IS_AVAILABLE_PRODUCT = "LLStoreManagerIsAvailableProduct";

	private const string METHOD_LOCALIZED_PRICE_PRODUCT = "LLStoreManagerLocalizedPriceProduct";

	private const string METHOD_LOCALIZED_TITLE_PRODUCT = "LLStoreManagerLocalizedTitleProduct";

	private const string METHOD_LOCALIZED_DESCRIPTION_PRODUCT = "LLStoreManagerLocalizedDescriptionProduct";

	private const string METHOD_RESTORE_PURCHASES = "LLStoreManagerRestorePurchases";

	private const string METHOD_CAN_MAKE_PURCHASE = "LLStoreManagerCanMakePurchase";

	private const string METHOD_GET_PURCHASE_SIGNATURE = "LLStoreManagerGetPurchaseSignature";

	private const string METHOD_GET_PURCHASE_DATA = "LLStoreManagerGetPurchaseData";

	private const string METHOD_LOAD_OWNED_PRODUCTS = "LLStoreManagerLoadOwnedProducts";

	private const string METHOD_SET_SUBSCRIPTIONS = "LLStoreManagerSetSubscriptions";

	private const string METHOD_SUBSCRIPTION_FOR_ID = "LLStoreManagerSubscriptionForID";

	private const string METHOD_SUBSCRIPTION_FOR_DATE = "LLStoreManagerSubscriptionForDate";

	private const string METHOD_LAST_SUBSCRIPTION = "LLStoreManagerLastSubscription";

	private const string METHOD_IS_SUBSCRIPTION_TRIAL = "LLStoreManagerIsSubscriptionTrial";

	private static bool isInitialized;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, bool> m_OnReceiveProduct;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string, string, LLTransactionState> m_OnPurchaseSuccessful;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string> m_OnPurchaseFailed;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnRestoreComplete;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState> m_OnPurchaseSuccessfulAndroid;

	public static event Action<string, bool> OnReceiveProduct
	{
		add
		{
			Action<string, bool> action = LLStoreManager.m_OnReceiveProduct;
			Action<string, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnReceiveProduct, (Action<string, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, bool> action = LLStoreManager.m_OnReceiveProduct;
			Action<string, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnReceiveProduct, (Action<string, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string, string, LLTransactionState> OnPurchaseSuccessful
	{
		add
		{
			Action<string, string, string, LLTransactionState> action = LLStoreManager.m_OnPurchaseSuccessful;
			Action<string, string, string, LLTransactionState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnPurchaseSuccessful, (Action<string, string, string, LLTransactionState>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string, string, LLTransactionState> action = LLStoreManager.m_OnPurchaseSuccessful;
			Action<string, string, string, LLTransactionState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnPurchaseSuccessful, (Action<string, string, string, LLTransactionState>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string> OnPurchaseFailed
	{
		add
		{
			Action<string, string> action = LLStoreManager.m_OnPurchaseFailed;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnPurchaseFailed, (Action<string, string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string> action = LLStoreManager.m_OnPurchaseFailed;
			Action<string, string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnPurchaseFailed, (Action<string, string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnRestoreComplete
	{
		add
		{
			Action<bool> action = LLStoreManager.m_OnRestoreComplete;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnRestoreComplete, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLStoreManager.m_OnRestoreComplete;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnRestoreComplete, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState> OnPurchaseSuccessfulAndroid
	{
		add
		{
			Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState> action = LLStoreManager.m_OnPurchaseSuccessfulAndroid;
			Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnPurchaseSuccessfulAndroid, (Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState> action = LLStoreManager.m_OnPurchaseSuccessfulAndroid;
			Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLStoreManager.m_OnPurchaseSuccessfulAndroid, (Action<string, string, PurchaseSuccessfulResultAndroid, LLTransactionState>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static void Initialize()
	{
		if (!isInitialized)
		{
			isInitialized = true;
		}
	}

	public static void RequestProducts(string[] productIDs)
	{
		Initialize();
		int num = ((productIDs != null) ? productIDs.Length : 0);
		LLAndroidJavaSingletone<LLStoreManager>.CallStatic("LLStoreManagerRequestProducts", productIDs, num);
	}

	public static void SetConsumableProducts(string[] productIDs)
	{
		Initialize();
		int num = ((productIDs != null) ? productIDs.Length : 0);
		LLAndroidJavaSingletone<LLStoreManager>.CallStatic("LLStoreManagerSetConsumableProducts", productIDs, num);
	}

	public static void SetSubscriptions(string[] subscriptionIDs)
	{
		Initialize();
		int num = ((subscriptionIDs != null) ? subscriptionIDs.Length : 0);
		LLAndroidJavaSingletone<LLStoreManager>.CallStatic("LLStoreManagerSetSubscriptions", subscriptionIDs, num);
	}

	public static void LoadOwnedProductsAndroid()
	{
		Initialize();
		LLAndroidJavaSingletone<LLStoreManager>.CallStatic("LLStoreManagerLoadOwnedProducts");
	}

	public static void PurchaseProduct(string productID, string rewardID)
	{
		Initialize();
		LLAndroidJavaSingletone<LLStoreManager>.CallStatic("LLStoreManagerPurchaseProduct", productID, rewardID);
	}

	public static bool IsAvailableProduct(string productID)
	{
		Initialize();
		bool flag = true;
		return LLAndroidJavaSingletone<LLStoreManager>.CallStatic<bool>("LLStoreManagerIsAvailableProduct", new object[1] { productID });
	}

	public static string LocalizedPriceProduct(string productID)
	{
		Initialize();
		string empty = string.Empty;
		return SingletonMonoBehaviour<MCInnAppsmanager>.Instance.GetStoreProductForSkuWithID(productID).localizedPrice;
	}

	public static string LocalizedTitleProduct(string productID)
	{
		Initialize();
		string empty = string.Empty;
		return LLAndroidJavaSingletone<LLStoreManager>.CallStatic<string>("LLStoreManagerLocalizedTitleProduct", new object[1] { productID });
	}

	public static string LocalizedDescriptionProduct(string productID)
	{
		Initialize();
		string empty = string.Empty;
		return LLAndroidJavaSingletone<LLStoreManager>.CallStatic<string>("LLStoreManagerLocalizedDescriptionProduct", new object[1] { productID });
	}

	public static void RestorePurchases()
	{
		Initialize();
		LLAndroidJavaSingletone<LLStoreManager>.CallStatic("LLStoreManagerRestorePurchases");
	}

	public static bool CanMakePurchase()
	{
		Initialize();
		bool flag = true;
		return LLAndroidJavaSingletone<LLStoreManager>.CallStatic<bool>("LLStoreManagerCanMakePurchase", new object[0]);
	}

	public static bool IsPurchasesInSandbox()
	{
		Initialize();
		return false;
	}

	public static SubscriptionResult SubscriptionForID(string subscriptionID)
	{
		Initialize();
		SubscriptionResult subscriptionResult = default(SubscriptionResult);
		string json = LLAndroidJavaSingletone<LLStoreManager>.CallStatic<string>("LLStoreManagerSubscriptionForID", new object[1] { subscriptionID });
		return CollectSubscriptionResultFromAndroid(json);
	}

	public static SubscriptionResult SubscriptionForDate(DateTime date)
	{
		Initialize();
		SubscriptionResult subscriptionResult = default(SubscriptionResult);
		double num = ConvertToUnixTimestamp(date);
		string json = LLAndroidJavaSingletone<LLStoreManager>.CallStatic<string>("LLStoreManagerSubscriptionForDate", new object[1] { num });
		return CollectSubscriptionResultFromAndroid(json);
	}

	public static SubscriptionResult LastSubscription()
	{
		Initialize();
		SubscriptionResult subscriptionResult = default(SubscriptionResult);
		string json = LLAndroidJavaSingletone<LLStoreManager>.CallStatic<string>("LLStoreManagerLastSubscription", new object[0]);
		return CollectSubscriptionResultFromAndroid(json);
	}

	public static bool IsSubscriptionTrial(string productID)
	{
		Initialize();
		bool flag = true;
		return SingletonMonoBehaviour<MCInnAppsmanager>.Instance.GetStoreProductForSkuWithID(productID).IsFreeTrial;
	}

	public static DateTime ConvertFromUnixTimestamp(double timestamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
	}

	public static double ConvertToUnixTimestamp(DateTime date)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return Math.Floor((date.ToUniversalTime() - dateTime).TotalSeconds);
	}

	private static SubscriptionResult CollectSubscriptionResultFromAndroid(string json)
	{
		SubscriptionResult result = default(SubscriptionResult);
		if (!string.IsNullOrEmpty(json))
		{
			LLSubscriptionResultAndroid lLSubscriptionResultAndroid = JsonConvert.DeserializeObject<LLSubscriptionResultAndroid>(json);
			result.productID = lLSubscriptionResultAndroid.productID;
			result.subscriptionID = lLSubscriptionResultAndroid.orderID;
			result.nextSubscriptionID = string.Empty;
			result.purchaseDate = ConvertFromUnixTimestamp(lLSubscriptionResultAndroid.purchaseDate);
			result.expirationDate = ConvertFromUnixTimestamp(lLSubscriptionResultAndroid.expirationDate);
		}
		return result;
	}

	[MonoPInvokeCallback(typeof(LLStoreManagerProductDelegate))]
	private static void ReceiveProduct(string productID, bool isValidProduct)
	{
		if (LLStoreManager.OnReceiveProduct != null)
		{
			LLStoreManager.OnReceiveProduct(productID, isValidProduct);
		}
	}

	[MonoPInvokeCallback(typeof(LLStoreManagerPurchaseSuccessDelegate))]
	private static void PurchaseSuccessful(string productID, string rewardID, string transactionID, int transactionState)
	{
		if (LLStoreManager.OnPurchaseSuccessful != null)
		{
			LLStoreManager.OnPurchaseSuccessful(productID, rewardID, transactionID, (LLTransactionState)transactionState);
		}
	}

	private static void PurchaseSuccessfulAndroid(string productID, string rewardID, int transactionState)
	{
		if (LLStoreManager.OnPurchaseSuccessfulAndroid != null)
		{
			PurchaseSuccessfulResultAndroid arg = new PurchaseSuccessfulResultAndroid
			{
				signature = LLAndroidJavaSingletone<LLStoreManager>.CallStatic<string>("LLStoreManagerGetPurchaseSignature", new object[1] { productID }),
				purchaseDataJSON = LLAndroidJavaSingletone<LLStoreManager>.CallStatic<string>("LLStoreManagerGetPurchaseData", new object[1] { productID })
			};
			LLStoreManager.OnPurchaseSuccessfulAndroid(productID, rewardID, arg, (LLTransactionState)transactionState);
		}
	}

	[MonoPInvokeCallback(typeof(LLStoreManagerPurchaseFailedDelegate))]
	private static void PurchaseFailed(string productID, string rewardID)
	{
		if (LLStoreManager.OnPurchaseFailed != null)
		{
			LLStoreManager.OnPurchaseFailed(productID, rewardID);
		}
	}

	[MonoPInvokeCallback(typeof(LLStoreManagerRestoreDelegate))]
	private static void RestoreComplete(bool isSuccessful)
	{
		if (LLStoreManager.OnRestoreComplete != null)
		{
			LLStoreManager.OnRestoreComplete(isSuccessful);
		}
	}
}
