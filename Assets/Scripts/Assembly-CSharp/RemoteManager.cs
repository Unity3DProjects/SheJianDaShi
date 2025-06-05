using UnityEngine;

public class RemoteManager : SingletonMonoBehaviour<RemoteManager>
{
	private const string REMOTE_SHOW_SUBSCRIPTION_POPUP = "ShowSubscriptionPopup";

	public const string REMOTE_KEY_SUPER_GIFT = "IsSuperGiftEnabled";

	public const string REMOTE_KEY_FATALITY_SECOND_CHANCE = "IsFatalitySecondChanceEnabled";

	public const string REMOTE_KEY_MISTERY_BOX = "IsMisteryBoxEnabled";

	public const string REMOTE_KEY_MISTERY_BOX_REPLAY = "IsMisteryBoxReplayEnabled";

	public const string REMOTE_KEY_DOUBLE_ROULETTE_REWARD = "IsDoubleRouletteRewardEnabled";

	public const string REMOTE_KEY_MISTERY_BOX_RELOAD_TIME = "MisteryBoxReloadTime";

	public const string REMOTE_KEY_PROMO_FETCHER = "isPromoFetcherEnabled";

	private RemoteSettings.UpdatedEventHandler updateHandler;

	private bool isSubscriptionPopupShowed;

	public bool IsSubscriptionPopupShowed
	{
		get
		{
			return isSubscriptionPopupShowed;
		}
	}

	private RemoteSettings.UpdatedEventHandler UpdateHandler
	{
		get
		{
			return updateHandler;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		updateHandler = HandleRemoteUpdate;
		HandleRemoteUpdate();
	}

	private void OnEnable()
	{
		RemoteSettings.Updated += UpdateHandler;
	}

	private void OnDisable()
	{
		RemoteSettings.Updated -= UpdateHandler;
	}

	public int GetRemoteValueForKey(string key, int defaultValue)
	{
		return RemoteSettings.GetInt(key, defaultValue);
	}

	public bool GetRemoteValueForKey(string key, bool defaultValue)
	{
		return RemoteSettings.GetBool(key, defaultValue);
	}

	private void HandleRemoteUpdate()
	{
		isSubscriptionPopupShowed = RemoteSettings.GetBool("ShowSubscriptionPopup", true);
	}
}
