using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ShopItemCharacter : ShopItem
{
	private const string LOCKED_CHARACTER = "locked_character";

	private const string CHARACTER_UPGRADE_LEVEL = "character_upgrade_level";

	[SerializeField]
	private List<ShopItemCharacterInfo> infos;

	[SerializeField]
	private bool defaultCharacter;

	[SerializeField]
	private int priceCoins = 500;

	[SerializeField]
	private bool isPremium;

	[SerializeField]
	private bool isDailySpecialGift;

	[SerializeField]
	private bool isVideoSpecialGift;

	[SerializeField]
	private bool isWithoutRoulette;

	[SerializeField]
	private bool isMultiplayerWinsGift;

	[SerializeField]
	private bool isLegendaryTournamentReward;

	[SerializeField]
	private bool isInviteReward;

	[SerializeField]
	private bool isUnavailableInShop;

	[SerializeField]
	private bool isEventGift;

	[SerializeField]
	private bool isSubscribeReward;

	[SerializeField]
	private bool isOnlySubscribeReward;

	[SerializeField]
	private bool isLockedByTimer;

	[SerializeField]
	private bool isIncludedInSubscription;

	[SerializeField]
	private LockedByAdsInfo lockedByAdsInfo;

	[SerializeField]
	private List<UpgradeInfo> upgradeInfo;

	[NonSerialized]
	private bool isTryCharacter;

	private bool isTimeEnded;

	private Managers cachedManagers;

	public bool Availible
	{
		get
		{
			bool flag = base.Bought || defaultCharacter || IsTry;
			if (IsIncludedInSubscription || IsVideoSpecialGift)
			{
				// return flag || false;
				return flag || CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
			}
			return flag;
		}
	}

	public bool CanBuyInShop
	{
		get
		{
			return !isDailySpecialGift && !isVideoSpecialGift && !isMultiplayerWinsGift && !isLegendaryTournamentReward && !isInviteReward && !isUnavailableInShop && !isSubscribeReward && !isOnlySubscribeReward && !lockedByAdsInfo.isLockedByAds && !isIncludedInSubscription;
		}
	}

	public int PriceCoins
	{
		get
		{
			return priceCoins;
		}
	}

	public string PriceCoinsString
	{
		get
		{
			return priceCoins.ToString();
		}
	}

	public bool IsPremium
	{
		get
		{
			return isPremium;
		}
	}

	public bool IsTry
	{
		get
		{
			return isTryCharacter;
		}
		set
		{
			isTryCharacter = value;
		}
	}

	public bool IsDailySpecialGift
	{
		get
		{
			return isDailySpecialGift;
		}
	}

	public bool IsVideoSpecialGift
	{
		get
		{
			return isVideoSpecialGift;
		}
	}

	public bool IsWithoutRoulette
	{
		get
		{
			return isWithoutRoulette;
		}
	}

	public bool IsMultiplayerWinsGift
	{
		get
		{
			return isMultiplayerWinsGift;
		}
		set
		{
			isMultiplayerWinsGift = value;
		}
	}

	public bool IsLegendaryTournamentReward
	{
		get
		{
			return isLegendaryTournamentReward;
		}
	}

	public bool IsInviteReward
	{
		get
		{
			return isInviteReward;
		}
	}

	public bool IsSubscribeReward
	{
		get
		{
			return isSubscribeReward;
		}
	}

	public bool IsOnlySubscribeReward
	{
		get
		{
			return isOnlySubscribeReward;
		}
	}

	public bool DefaultCharacter
	{
		get
		{
			return defaultCharacter;
		}
	}

	public bool IsUnavailableInShop
	{
		get
		{
			return isUnavailableInShop;
		}
	}

	public bool IsEventGift
	{
		get
		{
			return isEventGift;
		}
	}

	public bool IsLockedByTimer
	{
		get
		{
			return isLockedByTimer;
		}
	}

	public LockedByAdsInfo LockedByAdsInfo
	{
		get
		{
			return lockedByAdsInfo;
		}
	}

	public bool IsTimeEnded
	{
		get
		{
			isTimeEnded = CustomPlayerPrefs.GetInt("locked_character" + base.ShopItemID, 0) != 0;
			return isTimeEnded;
		}
		set
		{
			if (isTimeEnded != value)
			{
				isTimeEnded = value;
				if (isTimeEnded)
				{
					CustomPlayerPrefs.SetInt("locked_character" + base.ShopItemID, 1);
				}
			}
		}
	}

	public int UpgradeLevel
	{
		get
		{
			return CustomPlayerPrefs.GetInt("character_upgrade_level" + base.ShopItemID, 0);
		}
		private set
		{
			if (IsUpgradeAvailable)
			{
				CustomPlayerPrefs.SetInt("character_upgrade_level" + base.ShopItemID, value);
			}
		}
	}

	public bool IsUpgradeAvailable
	{
		get
		{
			return GetUpgradeInfo(UpgradeLevel + 1).upgradeLevel != 0;
		}
	}

	public bool IsIncludedInSubscription
	{
		get
		{
			
			//TODO:修改--角色获取方式--订阅改购买
			
			return isIncludedInSubscription;
			// return false;
		}
	}

	public UpgradeInfo NextLevelCharacterUpgradeInfo
	{
		get
		{
			return GetUpgradeInfo(UpgradeLevel + 1);
		}
	}

	public UpgradeInfo CharacterUpgradeInfo
	{
		get
		{
			return GetUpgradeInfo(UpgradeLevel);
		}
	}

	public List<UpgradeInfo> UpgradeInfo
	{
		get
		{
			return upgradeInfo;
		}
	}

	public List<ShopItemCharacterInfo> Infos
	{
		get
		{
			return infos;
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

	public override bool CanBuy
	{
		get
		{
			bool result = true;
			if (!Availible)
			{
				result = CachedManagers.CachedShopManager.CanMakePurchase() && base.IsDataReceived;
			}
			return result;
		}
	}

	public string GetCharacterID()
	{
		string[] array = base.ShopItemID.Split('.');
		string text = array[array.Length - 1];
		if (UpgradeLevel > 0)
		{
			text = string.Format("{0}_upgrade_{1}", text, UpgradeLevel);
		}
		return text;
	}

	public void OpenCharacter(bool isPurchasedCharacter, bool isSavingCharacter, bool bulkOpen = true)
	{
		OpenShopItem();
		if (isSavingCharacter)
		{
			SingletonMonoBehaviour<ICloudManager>.Instance.SaveProgressToICloud();
		}
		if (bulkOpen)
		{
			CachedManagers.CachedMicroachievementsManager.CheckPackAchievementsReaching((!isPurchasedCharacter) ? this : null);
			CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Unite, CachedManagers.CachedShopManager.BoughtCharacters.Count, (!isPurchasedCharacter) ? this : null);
			if (CachedManagers.CachedGUIScreenManager.MenuScreen != null)
			{
				CachedManagers.CachedTutorialManager.LoadBuyCharacterOnCoinsTutorial(CachedManagers.CachedGUIScreenManager.MenuScreen.MenuScreenScrollView);
			}
		}
	}

	public override void PurchaseComplete(LLTransactionState transactionState)
	{
		CachedManagers.CachedStatisticsManager.CharactersBoughtOnBucks++;
		if (transactionState == LLTransactionState.Purchased)
		{
			int charactersBoughtOnBucks = CachedManagers.CachedStatisticsManager.CharactersBoughtOnBucks;
			Analytics.CustomEvent("Character_Purchased", new Dictionary<string, object>
			{
				{ "CharacterName", shopItemID },
				{ "CharactersBoughtOnBucks", charactersBoughtOnBucks },
				{
					"GameTimeMinutes",
					CachedManagers.CachedStatisticsManager.TotalGameTime.Minutes
				}
			});
			string text = null;
			switch (charactersBoughtOnBucks)
			{
			case 1:
				text = "Character_Purchased_First";
				break;
			case 2:
				text = "Character_Purchased_Second";
				break;
			case 3:
				text = "Character_Purchased_Third";
				break;
			}
			if (text != null)
			{
				Analytics.CustomEvent(text, new Dictionary<string, object>
				{
					{ "CharacterName", shopItemID },
					{
						"MatchesCount",
						CachedManagers.CachedStatisticsManager.MathesCount
					},
					{
						"GameTimeMinutes",
						CachedManagers.CachedStatisticsManager.TotalGameTime.Minutes
					}
				});
			}
		}
		base.PurchaseComplete(transactionState);
	}

	public void UpgradeCharacterLevel(int levelAddition = 1)
	{
		UpgradeLevel += levelAddition;
	}

	public void ClearCharacterLevel()
	{
		CustomPlayerPrefs.SetInt("character_upgrade_level" + base.ShopItemID, 0);
	}

	public string GetAnalyticsKey()
	{
		if (CharacterUpgradeInfo.upgradeLevel > 0)
		{
			return string.Format("{0}_U{1}", base.ShopItemID, UpgradeLevel.ToString());
		}
		return base.ShopItemID;
	}

	private UpgradeInfo GetUpgradeInfo(int level)
	{
		UpgradeInfo result = default(UpgradeInfo);
		for (int i = 0; i < upgradeInfo.Count; i++)
		{
			if (upgradeInfo[i].upgradeLevel == level)
			{
				result = upgradeInfo[i];
				string key = string.Format("{0}_{1}", base.ShopItemID, level);
				result.priceGems = CachedManagers.CachedRemoteManager.GetRemoteValueForKey(key, upgradeInfo[i].priceGems);
				break;
			}
		}
		return result;
	}
}
