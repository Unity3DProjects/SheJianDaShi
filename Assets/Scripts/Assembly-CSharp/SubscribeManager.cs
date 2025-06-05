using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class SubscribeManager : SingletonMonoBehaviour<SubscribeManager>
{
	private const string UNLOCKED_SUBSCRIBE_KEY = "unlocked_subscribe_key";

	public static Action OnSubscribeForGems;

	private GiftSubscribeCharacter currentSubscribeCharacter;

	private bool isChosenSubscribeCharacterOpened = true;

	private Managers cachedManagers;

	protected Managers CachedManagers
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

	public GiftSubscribeCharacter CurrentSubscribeCharacter
	{
		get
		{
			return currentSubscribeCharacter;
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		CharacterNameType characterNameType = (CharacterNameType)CustomPlayerPrefs.GetInt("unlocked_subscribe_key");
		if (characterNameType == CharacterNameType.None || !focus || !isChosenSubscribeCharacterOpened)
		{
			return;
		}
		ShopItemCharacter shopItemCharacter = SingletonMonoBehaviour<LevelsManager>.Instance.ObjectsSpawner.GetShopItemCharacter(characterNameType);
		if (shopItemCharacter != null && !shopItemCharacter.Availible)
		{
			SingletonMonoBehaviour<GiftManager>.Instance.OpenSpecialGiftCharacter(shopItemCharacter);
			if (shopItemCharacter.Availible)
			{
				Analytics.CustomEvent("Subscribe_Character_" + shopItemCharacter.ShopItemID + "_Complete");
			}
		}
		CustomPlayerPrefs.SetString("unlocked_subscribe_key", null);
		isChosenSubscribeCharacterOpened = false;
	}

	public void SetSubscribeCharacter(ShopItemCharacter subscribeCharacter = null)
	{
		isChosenSubscribeCharacterOpened = false;
		List<GiftSubscribeCharacter> subscribeGiftCharacters = SingletonMonoBehaviour<Managers>.Instance.CachedGiftManager.SubscribeGiftCharacters;
		for (int i = 0; i < subscribeGiftCharacters.Count; i++)
		{
			if (subscribeGiftCharacters[i].subscribeCharacter == subscribeCharacter.Infos[0].characterNameType)
			{
				currentSubscribeCharacter = subscribeGiftCharacters[i];
				break;
			}
		}
		if (currentSubscribeCharacter.subscribeCharacter.IsNull() || currentSubscribeCharacter.subscribeCharacterSpriteName.IsNull())
		{
			currentSubscribeCharacter = subscribeGiftCharacters.RandomObject();
		}
	}

	public void GetSubscribe()
	{
		AudioManager.PlaySingleAudioWithKey("button");
		SubscribeForCharacter();
	}

	private void SubscribeForCharacter()
	{
		int num = currentSubscribeCharacter.subscribeLinks.Length;
		if (num > 0)
		{
			Application.OpenURL(currentSubscribeCharacter.subscribeLinks[DateTime.Now.DayOfYear % num].id);
			isChosenSubscribeCharacterOpened = true;
			CustomPlayerPrefs.SetInt("unlocked_subscribe_key", (int)CurrentSubscribeCharacter.subscribeCharacter);
		}
	}

	public void SubscribeForGem()
	{
		Application.OpenURL(CachedManagers.CachedGiftManager.GiftSubscribeGems.link);
		CachedManagers.CachedStatisticsManager.AddGems(CachedManagers.CachedGiftManager.GiftSubscribeGems.gemsCount);
		CachedManagers.CachedStatisticsManager.FreeGemsReceived++;
		if (OnSubscribeForGems != null)
		{
			OnSubscribeForGems();
		}
		Analytics.CustomEvent("Gems_Subscribe");
	}
}
