using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class InviteManager : SingletonMonoBehaviour<InviteManager>
{
	private const string SHARE_APP_TEXT = "ui.share.text";

	private Action<bool> callbackSharing;

	private GiftInviteCharacter currentInviteCharacter;

	private Dictionary<CharacterNameType, int> differentInvitesCounters = new Dictionary<CharacterNameType, int>();

	private ObjectsSpawner objectsSpawner;

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

	public GiftInviteCharacter CurrentInviteCharacter
	{
		get
		{
			return currentInviteCharacter;
		}
	}

	public Dictionary<CharacterNameType, int> DifferentInvitesCounters
	{
		get
		{
			string text = CustomPlayerPrefs.GetString("different_invites_count", string.Empty);
			List<GiftInviteCharacter> inviteGiftCharacters = CachedManagers.CachedGiftManager.InviteGiftCharacters;
			int count = inviteGiftCharacters.Count;
			if (count >= 1)
			{
				differentInvitesCounters.Clear();
				if (string.IsNullOrEmpty(text))
				{
					differentInvitesCounters.Add(CharacterNameType.Steve, InvitesCount);
					for (int i = 0; i < inviteGiftCharacters.Count; i++)
					{
						if (!differentInvitesCounters.ContainsKey(inviteGiftCharacters[i].inviteCharacter))
						{
							differentInvitesCounters.Add(inviteGiftCharacters[i].inviteCharacter, 0);
						}
					}
				}
				else
				{
					differentInvitesCounters = Json.Deserialize<Dictionary<CharacterNameType, int>>(text);
					for (int j = 0; j < inviteGiftCharacters.Count; j++)
					{
						if (!differentInvitesCounters.ContainsKey(inviteGiftCharacters[j].inviteCharacter))
						{
							differentInvitesCounters.Add(inviteGiftCharacters[j].inviteCharacter, 0);
						}
					}
				}
			}
			return differentInvitesCounters;
		}
	}

	public int GemsInvitesCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("free_gems_invites_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("free_gems_invites_count", value);
		}
	}

	private List<CharacterNameType> NecessaryInvitedCharactersShowed
	{
		get
		{
			string text = CustomPlayerPrefs.GetString("necesary_invited_characters", string.Empty);
			if (string.IsNullOrEmpty(text))
			{
				return new List<CharacterNameType>();
			}
			return Json.Deserialize<List<CharacterNameType>>(text);
		}
	}

	private int InvitesCount
	{
		get
		{
			return CustomPlayerPrefs.GetInt("invites_count", 0);
		}
		set
		{
			CustomPlayerPrefs.SetInt("invites_count", value);
		}
	}

	private ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = CachedManagers.CachedLevelsManager.ObjectsSpawner;
			}
			return objectsSpawner;
		}
	}

	private void OnEnable()
	{
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		GameManager.OnGameDownload += GameManager_OnGameDownload;
		LLSharingManager.OnShareComplete += LLSharingManager_OnShareComplete;
		LLSharingManager.OnShareDismiss += LLSharingManager_OnShareDismiss;
	}

	private void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		GameManager.OnGameDownload -= GameManager_OnGameDownload;
		LLSharingManager.OnShareComplete -= LLSharingManager_OnShareComplete;
		LLSharingManager.OnShareDismiss -= LLSharingManager_OnShareDismiss;
	}

	public void SetInviteCharacter(ShopItemCharacter inviteCharacter = null)
	{
		List<GiftInviteCharacter> inviteGiftCharacters = CachedManagers.CachedGiftManager.InviteGiftCharacters;
		if (inviteCharacter == null)
		{
			List<GiftInviteCharacter> list = new List<GiftInviteCharacter>();
			List<GiftInviteCharacter> list2 = new List<GiftInviteCharacter>();
			for (int i = 0; i < inviteGiftCharacters.Count; i++)
			{
				ShopItemCharacter shopItemCharacter = ObjectsSpawner.GetShopItemCharacter(inviteGiftCharacters[i].inviteCharacter);
				if (!shopItemCharacter.Availible)
				{
					if (inviteGiftCharacters[i].isForcedShowed && !NecessaryInvitedCharactersShowed.Contains(inviteGiftCharacters[i].inviteCharacter))
					{
						list2.Add(inviteGiftCharacters[i]);
					}
					list.Add(inviteGiftCharacters[i]);
				}
			}
			if (list2.Count > 0)
			{
				RandomizeInviteCharacter(list2, true);
				List<CharacterNameType> necessaryInvitedCharactersShowed = NecessaryInvitedCharactersShowed;
				necessaryInvitedCharactersShowed.Add(currentInviteCharacter.inviteCharacter);
				CustomPlayerPrefs.SetString("necesary_invited_characters", Json.Serialize(necessaryInvitedCharactersShowed));
			}
			else if (list.Count > 0)
			{
				RandomizeInviteCharacter(list, false);
			}
		}
		else
		{
			for (int j = 0; j < inviteGiftCharacters.Count; j++)
			{
				if (inviteGiftCharacters[j].inviteCharacter == inviteCharacter.Infos[0].characterNameType)
				{
					currentInviteCharacter = inviteGiftCharacters[j];
					break;
				}
			}
		}
		if (currentInviteCharacter.inviteCharacter.IsNull() || currentInviteCharacter.inviteCharacterSpriteName.IsNull())
		{
			currentInviteCharacter = inviteGiftCharacters.RandomObject();
		}
	}

	public void InviteForCharacter(Action<bool> callback)
	{
		callbackSharing = delegate(bool obj)
		{
			if (!currentInviteCharacter.inviteCharacter.IsNull() && !currentInviteCharacter.inviteCharacterSpriteName.IsNull())
			{
				CharacterNameType inviteCharacter = currentInviteCharacter.inviteCharacter;
				if (DifferentInvitesCounters.ContainsKey(inviteCharacter))
				{
					int num = DifferentInvitesCounters[inviteCharacter];
					if (obj)
					{
						num++;
						Dictionary<CharacterNameType, int> dictionary = DifferentInvitesCounters;
						dictionary[inviteCharacter] = num;
						CustomPlayerPrefs.SetString("different_invites_count", Json.Serialize(dictionary));
					}
					if (num >= currentInviteCharacter.invitesNeededCount)
					{
						CachedManagers.CachedGiftManager.OpenSpecialGiftCharacter(ObjectsSpawner.GetShopItemCharacter(currentInviteCharacter.inviteCharacter));
					}
					if (callback != null)
					{
						callback(obj);
					}
				}
			}
		};
		MiniclipUtils.ShareAll(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("ui.share.text"), RateMeManager.BowmastersGooglePlayStoreLink);
	}

	public void InviteForGems(Action<bool> callback)
	{
		callbackSharing = delegate(bool obj)
		{
			GiftInviteGems giftInviteGems = CachedManagers.CachedGiftManager.GiftInviteGems;
			if (!giftInviteGems.IsNull())
			{
				if (obj)
				{
					GemsInvitesCount++;
				}
				if (GemsInvitesCount >= giftInviteGems.invitesNeededCount)
				{
					CachedManagers.CachedGiftManager.OpenGiftGems(giftInviteGems.gemsCount);
				}
				if (callback != null)
				{
					callback(obj);
				}
			}
		};
		MiniclipUtils.ShareAll(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("ui.share.text"), RateMeManager.BowmastersGooglePlayStoreLink);
	}

	private void RandomizeInviteCharacter(List<GiftInviteCharacter> inviteCharacters, bool isForced)
	{
		float num = 0f;
		for (int i = 0; i < inviteCharacters.Count; i++)
		{
			num += ((!isForced) ? inviteCharacters[i].inviteWeight : inviteCharacters[i].forcedWeight);
		}
		if (!(num > 0f))
		{
			return;
		}
		double num2 = 0.0;
		double num3 = UnityEngine.Random.value;
		for (int j = 0; j < inviteCharacters.Count; j++)
		{
			num2 += (double)((!isForced) ? inviteCharacters[j].inviteWeight : inviteCharacters[j].forcedWeight);
			if (num3 <= num2 / (double)num)
			{
				currentInviteCharacter = inviteCharacters[j];
				break;
			}
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus && callbackSharing != null)
		{
			callbackSharing(true);
			CloudSaveManager.Instance.SaveToCloud();
			callbackSharing = null;
		}
	}

	private void GameManager_OnGameDownload()
	{
		LLSharingManager.Initialize();
	}

	private void LLSharingManager_OnShareComplete(int obj)
	{
		if (callbackSharing != null)
		{
			callbackSharing(true);
			callbackSharing = null;
		}
	}

	private void LLSharingManager_OnShareDismiss(int obj)
	{
		if (callbackSharing != null)
		{
			callbackSharing(false);
			callbackSharing = null;
		}
	}

	private void CustomGUI_OnDebugGUI()
	{
	}
}
