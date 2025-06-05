using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ShopItemPack : ShopItem
{
	[SerializeField]
	private string shopItemFullPriceID;

	[SerializeField]
	private string localizedDescription;

	[SerializeField]
	private ShopItemCharacter[] characters;

	[SerializeField]
	private tk2dSprite iconProposal;

	[SerializeField]
	private MicroachievementType achievementType;

	[SerializeField]
	private string popupSpriteName;

	private bool isRestoredItem;

	public string ProductFullPriceID
	{
		get
		{
			return BundlePrefix.PrefixInAppPurchaseID() + shopItemFullPriceID;
		}
	}

	public string PriceStringSale
	{
		get
		{
			string text = null;
			if (base.IsDataReceived)
			{
				return LLStoreManager.LocalizedPriceProduct(ProductFullPriceID);
			}
			return string.Empty;
		}
	}

	public string LocalizedDescription
	{
		get
		{
			return SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(localizedDescription);
		}
	}

	public bool Availible
	{
		get
		{
			bool flag = base.Bought;
			if (!flag)
			{
				for (int i = 0; i < characters.Length; i++)
				{
					if (characters[i] != null && characters[i].Availible)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}
	}

	public ShopItemCharacter FirstCharacter
	{
		get
		{
			ShopItemCharacter result = null;
			if (characters != null && characters.Length > 0)
			{
				result = characters[0];
			}
			return result;
		}
	}

	public int IconSpriteID
	{
		get
		{
			return iconProposal.spriteId;
		}
	}

	public ShopItemCharacter[] Characters
	{
		get
		{
			return characters;
		}
	}

	public MicroachievementType AchievementType
	{
		get
		{
			return achievementType;
		}
	}

	public string PopupSpriteName
	{
		get
		{
			return popupSpriteName;
		}
	}

	protected override void OpenShopItem(bool isPlayHappySound = false)
	{
		StatisticsManager instance = SingletonMonoBehaviour<StatisticsManager>.Instance;
		Analytics.CustomEvent("Pack_Purchased", new Dictionary<string, object>
		{
			{ "PackName", shopItemID },
			{
				"GameTimeMinutes",
				instance.TotalGameTime.Minutes
			},
			{ "MatchesCount", instance.MathesCount }
		});
		for (int i = 0; i < characters.Length; i++)
		{
			ShopItemCharacter shopItemCharacter = characters[i];
			if (shopItemCharacter != null)
			{
				shopItemCharacter.OpenCharacter(!isRestoredItem, false);
			}
		}
		SingletonMonoBehaviour<ICloudManager>.Instance.SaveProgressToICloud();
		base.OpenShopItem(true);
	}

	public override void PurchaseComplete(LLTransactionState transactionState)
	{
		isRestoredItem = transactionState == LLTransactionState.Restored;
		base.PurchaseComplete(transactionState);
	}
}
