using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GoliathSimple;
using MiniJSON;
using Mit_Goliath_Native;
using Mit_Goliath_Native.Utils;
using Prime31;
using UnityEngine;
using Json = MiniJSON.Json;

public class GameGoliath : GoliathManager
{
	protected static GameGoliath instance;

	private static string SESSION_END_KEY = "session_end_key";

	private string user_id;

	private string sessionId;

	private DateTime sessionBeginDate;

	private Dictionary<string, object> initEvent;

	private static bool LOGS_ARE_ENABLED = true;

	private bool fetchingData;

	private bool blockSessionEnd;

	public bool SessionBeginSent;

	public static GameGoliath Instance
	{
		get
		{
			return instance;
		}
	}

	private new void Awake()
	{
		base.Awake();
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		GoliathStart();
	}

	private void OnEnable()
	{
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.OnRequestPurchase += BlockSessionEnd;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseFailed += delegate
		{
			UnblockSessionEnd();
		};
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseSuccessful += delegate
		{
			UnblockSessionEnd();
		};
		GameCenterManager.OnWillAuthenticate += BlockSessionEnd;
		GameCenterManager.OnAuthentication += delegate
		{
			UnblockSessionEnd();
		};
		MCAdsManager.OnInterstitialShow += delegate
		{
			BlockSessionEnd();
		};
		MCAdsManager.OnInterstitialFinish += UnblockSessionEnd;
		MCAdsManager.OnVideoShow += delegate
		{
			BlockSessionEnd();
		};
		MCAdsManager.OnVideoFinish += delegate
		{
			UnblockSessionEnd();
		};
	}

	private void OnDisable()
	{
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.OnRequestPurchase -= BlockSessionEnd;
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseFailed -= delegate
		{
			UnblockSessionEnd();
		};
		SingletonMonoBehaviour<MCInnAppsmanager>.Instance.PurchaseSuccessful -= delegate
		{
			UnblockSessionEnd();
		};
		GameCenterManager.OnWillAuthenticate -= BlockSessionEnd;
		GameCenterManager.OnAuthentication -= delegate
		{
			UnblockSessionEnd();
		};
		MCAdsManager.OnInterstitialShow -= delegate
		{
			BlockSessionEnd();
		};
		MCAdsManager.OnInterstitialFinish -= UnblockSessionEnd;
		MCAdsManager.OnVideoShow -= delegate
		{
			BlockSessionEnd();
		};
		MCAdsManager.OnVideoFinish -= delegate
		{
			UnblockSessionEnd();
		};
	}

	private void Start()
	{
		sessionBeginDate = DateTime.Now;
		OnSessionBegin();
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			PlayerPrefs.DeleteKey(SESSION_END_KEY);
			if (!blockSessionEnd)
			{
				sessionBeginDate = DateTime.Now;
				OnSessionBegin();
			}
		}
		else
		{
			OnSessionEnd(blockSessionEnd);
		}
	}

	private void OnApplicationQuit()
	{
		OnSessionEnd();
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void OnSessionBegin()
	{
		if (user_id == null && !fetchingData)
		{
			Log("Starting coroutine to fetch advertisingID and google login");
			fetchingData = true;
			StartCoroutine(FetchData());
		}
		else if (user_id != null && string.IsNullOrEmpty(sessionId))
		{
			Log("We have the necessary information to post our init event");
			sessionId = "session_" + sessionBeginDate.Ticks;
			SetUserId(user_id);
			SetSessionId(sessionId);
			if (PlayerPrefs.HasKey(SESSION_END_KEY))
			{
				Dictionary<string, object> dictionary = Json.Deserialize<Dictionary<string, object>>(PlayerPrefs.GetString(SESSION_END_KEY));
				Session session = new Session();
				session.SetLong(GoliathIds.Param_wallet_coins, long.Parse(dictionary[GoliathIds.Param_wallet_coins].ToString()));
				session.Post();
				PlayerPrefs.DeleteKey(SESSION_END_KEY);
			}
			TrackInit();
			ClientInit clientInit = new ClientInit();
			clientInit.Post();
			SessionBeginSent = true;
		}
	}

	private void TrackInit()
	{
		Init init = new Init();
		string loginType = ((!SingletonMonoBehaviour<GameCenterManager>.Instance.isAuthenticated) ? "guest" : "google");
		init.SetLoginType(loginType);
		init.SetInt(GoliathIds.Param_player_wins, SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.WinsCount);
		init.SetLong(GoliathIds.Param_wallet_coins, SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Coins);
		init.SetLong(GoliathIds.Param_wallet_gems, SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Gems);
		init.SetInt(GoliathIds.Param_nb_characters, SingletonMonoBehaviour<ShopManager>.Instance.UnlockedCharacters.Count);
		init.SetString(GoliathIds.Param_game_language, "en");
		init.Post();
	}

	private void OnSessionEnd(bool sessionEndBlocked = false)
	{
		if (!string.IsNullOrEmpty(sessionId))
		{
			if (sessionEndBlocked)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add(GoliathIds.Param_wallet_coins, SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Coins.ToString());
				Dictionary<string, object> serialize = dictionary;
				PlayerPrefs.SetString(SESSION_END_KEY, JsonConvert.SerializeObject(serialize));
				PlayerPrefs.Save();
			}
			else
			{
				Session session = new Session();
				session.SetLong(GoliathIds.Param_wallet_coins, SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Coins);
				session.Post();
			}
		}
	}

	public void TrackInAppPurchase(string purchaseId, string productId, string shopId, string productFamily, double purchaseValue, Receipt receipt, string promotionID = "")
	{
		InAppPurchase inAppPurchase = new InAppPurchase();
		string empty = string.Empty;
		string empty2 = string.Empty;
		GoogleSkuInfo storeProductForGoogleSkuWithID = SingletonMonoBehaviour<MCInnAppsmanager>.Instance.GetStoreProductForGoogleSkuWithID(productId);
		empty = storeProductForGoogleSkuWithID.priceCurrencyCode;
		empty2 = ((double)storeProductForGoogleSkuWithID.priceAmountMicros / 1000000.0).ToString();
		inAppPurchase.SetEventId(purchaseId);
		inAppPurchase.SetProductId(shopId);
		inAppPurchase.SetPurchaseValue(purchaseValue.ToString());
		inAppPurchase.SetProductFamily(productFamily);
		inAppPurchase.SetLocalCurrency(empty);
		inAppPurchase.SetLocalPrice(empty2);
		inAppPurchase.SetReceipt(CleanForJSON(receipt.data));
		inAppPurchase.SetSignedReceipt(receipt.signature);
		if (!string.IsNullOrEmpty(promotionID))
		{
			inAppPurchase.SetString(GoliathIds.Param_promotion_id, promotionID);
		}
		inAppPurchase.Post();
	}

	public void TrackInvalidInAppReceipt(string purchaseId, string productId, string shopId, string productFamily, double purchase_value, Receipt receipt, int error, string promotionID = "")
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		GoogleSkuInfo storeProductForGoogleSkuWithID = SingletonMonoBehaviour<MCInnAppsmanager>.Instance.GetStoreProductForGoogleSkuWithID(productId);
		empty = storeProductForGoogleSkuWithID.priceCurrencyCode;
		empty2 = ((double)storeProductForGoogleSkuWithID.priceAmountMicros / 1000000.0).ToString();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(GoliathIds.Param_invalid_in_app_event_id, purchaseId);
		dictionary.Add(GoliathIds.Param_product_id, shopId);
		dictionary.Add(GoliathIds.Param_product_family, productFamily);
		dictionary.Add(GoliathIds.Param_purchase_value, purchase_value);
		dictionary.Add(GoliathIds.Param_local_currency, empty);
		dictionary.Add(GoliathIds.Param_local_price, empty2);
		dictionary.Add(GoliathIds.Param_receipt, receipt);
		dictionary.Add(GoliathIds.Param_signed_receipt, receipt.signature);
		dictionary.Add(GoliathIds.Param_error, error);
		Dictionary<string, object> dictionary2 = dictionary;
		if (!string.IsNullOrEmpty(promotionID))
		{
			dictionary2.Add(GoliathIds.Param_promotion_id, promotionID);
		}
		PostEvent("invalid_in_app_receipt", dictionary2);
	}

	public void TrackSubscriptonAction(string subscriptionId, GoliathIds.Action action, string referenceId = "")
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(GoliathIds.Param_subscription_id, subscriptionId);
		dictionary.Add(GoliathIds.Param_action, action.ToString());
		Dictionary<string, object> dictionary2 = dictionary;
		if (!string.IsNullOrEmpty(referenceId))
		{
			dictionary2.Add(GoliathIds.Param_reference_id, referenceId);
		}
		PostEvent("subscription_action", dictionary2);
	}

	public void TrackSubscriptonState(string productId, bool isFreeTrial, string purchaseId, int timeToExpire)
	{
		Debug.Log("GameGoliath::TrackSubscriptionState");
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(GoliathIds.Param_product_id, productId);
		dictionary.Add(GoliathIds.Param_is_free_trial, isFreeTrial);
		dictionary.Add(GoliathIds.Param_in_app_purchase_event_id, purchaseId);
		dictionary.Add(GoliathIds.Param_expiration_date, timeToExpire);
		Dictionary<string, object> eventParameters = dictionary;
		PostEvent("subscription_state", eventParameters);
	}

	public void TrackVirtualCurrencyPurchase(GoliathIds.CurrencyName currencyName, string currencyAmount, string productId, GoliathIds.ProductFamily productFamily)
	{
		VirtualCurrencyPurchase virtualCurrencyPurchase = new VirtualCurrencyPurchase();
		virtualCurrencyPurchase.SetCurrencyName(currencyName.ToString());
		virtualCurrencyPurchase.SetCurrencyAmount(currencyAmount);
		virtualCurrencyPurchase.SetProductId(productId);
		virtualCurrencyPurchase.SetProductFamily(productFamily.ToString());
		virtualCurrencyPurchase.Post();
	}

	public void TrackCurrencyFlow(GoliathIds.CurrencyMovement currencyMovement, GoliathIds.Context context, GoliathIds.CurrencyName currencyName, int amount, GoliathIds.Placement placement, int currencyBefore = -1)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(GoliathIds.Param_currency_movement, currencyMovement.ToString());
		dictionary.Add(GoliathIds.Param_context, context.ToString());
		dictionary.Add(GoliathIds.Param_currency_name, currencyName.ToString());
		dictionary.Add(GoliathIds.Param_placement, placement.ToString());
		dictionary.Add(GoliathIds.Param_currency_amount, amount);
		Dictionary<string, object> dictionary2 = dictionary;
		if (currencyName == GoliathIds.CurrencyName.COINS)
		{
			dictionary2.Add(GoliathIds.Param_wallet_coins_before, (currencyBefore >= 0) ? currencyBefore : SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Coins);
			dictionary2.Add(GoliathIds.Param_wallet_gems_before, SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Gems);
		}
		else if (currencyName == GoliathIds.CurrencyName.GEMS)
		{
			dictionary2.Add(GoliathIds.Param_wallet_gems_before, (currencyBefore >= 0) ? currencyBefore : SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Gems);
			dictionary2.Add(GoliathIds.Param_wallet_coins_before, SingletonMonoBehaviour<Managers>.Instance.CachedStatisticsManager.Coins);
		}
		PostEvent("currency_flow", dictionary2);
	}

	public void TrackMatchEnd(GoliathIds.GameMode gameMode, int matchDuration, string characterID = null, GoliathIds.MatchResult result = null, GoliathIds.MatchEndReason endReason = null)
	{
		Match match = new Match();
		match.SetDuration(matchDuration);
		match.SetString(GoliathIds.Param_game_mode, gameMode.ToString());
		if (!string.IsNullOrEmpty(characterID))
		{
			match.SetString(GoliathIds.Param_character, characterID);
		}
		if (result != null)
		{
			match.SetString(GoliathIds.Param_result, result.ToString());
		}
		if (endReason != null)
		{
			match.SetString(GoliathIds.Param_end_reason, endReason.ToString());
		}
		match.Post();
	}

	public void TrackCharacterAction(string characterID, GoliathIds.Action action, GoliathIds.Origin origin, string referenceId = "")
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(GoliathIds.Param_character, characterID);
		dictionary.Add(GoliathIds.Param_action, action.ToString());
		dictionary.Add(GoliathIds.Param_origin, origin.ToString());
		Dictionary<string, object> dictionary2 = dictionary;
		if (!string.IsNullOrEmpty(referenceId))
		{
			dictionary2.Add(GoliathIds.Param_reference_id, referenceId);
		}
		PostEvent("character_action", dictionary2);
	}

	public void TrackAdvertising(GoliathIds.AdType adType, GoliathIds.AdPhase adPhase, GoliathIds.AdReward adReward = null)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(GoliathIds.Param_timestamp, MiniclipUtils.GetUnixTimeMilliseconds());
		dictionary.Add(GoliathIds.Param_ad_type, adType.ToString());
		dictionary.Add(GoliathIds.Param_phase, adPhase);
		Dictionary<string, object> dictionary2 = dictionary;
		if (adReward != null)
		{
			dictionary2.Add(GoliathIds.Param_reward, adReward);
		}
		PostEvent("advertising", dictionary2);
	}

	public void TrackPrivacyPolicyConsent(GoliathIds.GDPRStep step, GoliathIds.GDPRContext context, GoliathIds.GDPRClickResult clickResult = null)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(GoliathIds.Param_step, step.ToString());
		Dictionary<string, object> dictionary2 = dictionary;
		if (clickResult != null)
		{
			dictionary2.Add(GoliathIds.Param_click_result, clickResult);
		}
		if (context != null)
		{
			dictionary2.Add(GoliathIds.Param_context, context);
		}
		PostEvent("privacy_policy_consent", dictionary2);
	}

	private IEnumerator FetchData()
	{
		Log("Starting to get AdvertisingID");
		int numTries = 0;
		while (!MiniclipUtils.IsAdvertisingIdInitialized() && numTries < 4)
		{
			Log("Waiting for AdvertisingID nº" + (numTries++ + 1));
			yield return new WaitForSeconds(0.25f);
		}
		numTries = 0;
		user_id = MiniclipUtils.GetAdvertisingId();
		Log("Got AdvertisingID:" + user_id);
		while (!SingletonMonoBehaviour<GameCenterManager>.Instance.isAuthenticated && numTries < 4)
		{
			Log("Waiting for google auth nº" + (numTries++ + 1));
			yield return new WaitForSeconds(0.5f);
		}
		Log("Tried google auth:" + user_id);
		fetchingData = false;
		OnSessionBegin();
	}

	private static void Log(string logStr)
	{
		if (LOGS_ARE_ENABLED)
		{
			Debug.Log("### GameGoliath - " + logStr);
		}
	}

	private void BlockSessionEnd()
	{
		blockSessionEnd = true;
	}

	private void UnblockSessionEnd()
	{
		blockSessionEnd = false;
	}

	private static string CleanForJSON(string s)
	{
		if (s == null || s.Length == 0)
		{
			return string.Empty;
		}
		char c = '\0';
		int length = s.Length;
		StringBuilder stringBuilder = new StringBuilder(length + 4);
		for (int i = 0; i < length; i++)
		{
			c = s[i];
			switch (c)
			{
			case '\b':
				stringBuilder.Append("\\b");
				continue;
			case '\t':
				stringBuilder.Append("\\t");
				continue;
			case '\n':
				stringBuilder.Append("\\n");
				continue;
			case '\f':
				stringBuilder.Append("\\f");
				continue;
			case '\r':
				stringBuilder.Append("\\r");
				continue;
			}
			if (c != '"')
			{
				if (c == '/')
				{
					stringBuilder.Append('\\');
					stringBuilder.Append(c);
					continue;
				}
				if (c != '\\')
				{
					if (c < ' ')
					{
						string text = "000" + string.Format("X", c);
						stringBuilder.Append("\\u" + text.Substring(text.Length - 4));
					}
					else
					{
						stringBuilder.Append(c);
					}
					continue;
				}
			}
			stringBuilder.Append('\\');
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}
}
