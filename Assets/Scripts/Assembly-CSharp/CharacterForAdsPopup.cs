using System;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;

public class CharacterForAdsPopup : GUIPopup
{
	private const string LABEL_KEY = "ui.popup.character.by.ads.label";

	[SerializeField]
	private tk2dTextMesh videosCountLabel;

	[SerializeField]
	private tk2dUIItem showAdsButton;

	[SerializeField]
	private tk2dSprite characterSprite;

	[SerializeField]
	private Transform characterEffectTransform;

	[SerializeField]
	private tk2dTextMesh popupLabel;

	private ShopItemCharacter character;

	private GUITimer timer = new GUITimer();

	private GameObject characterEffect;

	private int currentAdsCount;

	public ShopItemCharacter Character
	{
		set
		{
			if (value != null)
			{
				character = value;
				DateTime dateTime = DateTime.Now.Subtract(SingletonMonoBehaviour<StatisticsManager>.Instance.CheatedTime);
				DateTime dateTime2 = CustomPlayerPrefs.GetDateTime("character_timer_started" + character.ShopItemID, dateTime);
				if ((dateTime2 - dateTime).TotalSeconds > 0.0 && !timer.IsTimerActive)
				{
					timer.TimerInSeconds = (float)(dateTime2 - dateTime).TotalSeconds;
					timer.IsTimerActive = true;
				}
				string iconPopupName = base.ObjectsSpawner.GetGUICharacter(character.Infos[0].characterNameType).IconPopupName;
				if (iconPopupName != string.Empty)
				{
					characterSprite.SetSprite(iconPopupName);
				}
				StringBuilder stringBuilder = new StringBuilder(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("ui.popup.character.by.ads.label"));
				stringBuilder.Replace("%", character.LockedByAdsInfo.totalAdsCount.ToString());
				stringBuilder.Append(base.ObjectsSpawner.GetCharacterLocalizedName(character.Infos[0].characterNameType));
				popupLabel.text = stringBuilder.ToString();
				UpdatePopup();
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		showAdsButton.OnClick += ShowAdsButton_OnClick;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		showAdsButton.OnClick -= ShowAdsButton_OnClick;
	}

	protected override void Update()
	{
		base.Update();
		timer.UpdateTimer(Time.unscaledDeltaTime);
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			characterEffect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.GUIPopupInvite));
			characterEffect.transform.parent = characterEffectTransform;
			characterEffect.transform.localPosition = Vector3.zero;
		}, 0f);
	}

	private void UpdatePopup()
	{
		currentAdsCount = CustomPlayerPrefs.GetInt("character_ads_count" + character.ShopItemID, 0);
		StringBuilder stringBuilder = new StringBuilder(CustomPlayerPrefs.GetInt("character_ads_count" + character.ShopItemID, 0).ToString());
		stringBuilder.Append("/");
		stringBuilder.Append(character.LockedByAdsInfo.totalAdsCount);
		videosCountLabel.text = stringBuilder.ToString();
	}

	private void ShowAdsButton_OnClick()
	{
		DateTime currentVideoTime = DateTime.Now.Subtract(SingletonMonoBehaviour<StatisticsManager>.Instance.CheatedTime);
		DateTime dateTime = CustomPlayerPrefs.GetDateTime("character_ads_count_by_day" + character.ShopItemID + 0, currentVideoTime);
		if (((currentVideoTime - dateTime).TotalSeconds < (double)character.LockedByAdsInfo.timeBetweenFirstAndLastChainItem && CustomPlayerPrefs.GetInt("character_ads_count" + character.ShopItemID, 0) >= character.LockedByAdsInfo.adsChainLenght) || timer.IsTimerActive)
		{
			if (!timer.IsTimerActive)
			{
				timer.TimerInSeconds = character.LockedByAdsInfo.waitingTime;
				timer.IsTimerActive = true;
				CustomPlayerPrefs.SetDateTime("character_timer_started" + character.ShopItemID, currentVideoTime.Add(new TimeSpan(0, 0, character.LockedByAdsInfo.waitingTime)));
				base.CachedManagers.CachedNotifyManager.UpdateNotifications(character.LockedByAdsInfo.waitingTime, base.ObjectsSpawner.GetCharacterLocalizedName(character.Infos[0].characterNameType));
			}
			Hide(delegate
			{
				(base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.CharacterForAdsTimer) as CharacterForAdsTimerPopup).Character = character;
			});
			return;
		}
		base.CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoAvailable)
		{
			if (isVideoAvailable)
			{
				for (int i = 1; i < character.LockedByAdsInfo.adsChainLenght; i++)
				{
					DateTime dateTime2 = CustomPlayerPrefs.GetDateTime("character_ads_count_by_day" + character.ShopItemID + i, currentVideoTime);
					int num = i - 1;
					CustomPlayerPrefs.SetDateTime("character_ads_count_by_day" + character.ShopItemID + num, dateTime2);
				}
				int num2 = character.LockedByAdsInfo.adsChainLenght - 1;
				CustomPlayerPrefs.SetDateTime("character_ads_count_by_day" + character.ShopItemID + num2, currentVideoTime);
				CustomPlayerPrefs.SetInt("character_ads_count" + character.ShopItemID, currentAdsCount + 1);
				UpdatePopup();
				if (currentAdsCount == 10 || currentAdsCount == 20 || currentAdsCount == 50 || currentAdsCount == 80)
				{
					StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
					Analytics.CustomEvent("Ads" + currentAdsCount + "_character_" + character.ShopItemID);
				}
				if (currentAdsCount == character.LockedByAdsInfo.totalAdsCount)
				{
					Hide(delegate
					{
						StatisticsManager cachedStatisticsManager2 = base.CachedManagers.CachedStatisticsManager;
						Analytics.CustomEvent("Ads_character_" + character.ShopItemID + "complited");
						if (SingletonMonoBehaviour<GiftManager>.InstanceIfExist != null)
						{
							SingletonMonoBehaviour<GiftManager>.Instance.OpenSpecialGiftCharacter(character);
						}
						base.CachedManagers.CachedNotifyManager.IsAdsPopupNotificationEnabled = false;
						base.CachedManagers.CachedNotifyManager.UpdateNotifications(0f, string.Empty);
					});
				}
			}
		});
	}
}
