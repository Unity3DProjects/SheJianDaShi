using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;
using Debug = UnityEngine.Debug;

public class ShopItem : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<ShopItem> m_OnItemPurchased;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<ShopItem> m_OnItemPurchaseFailed;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<ShopItem> m_OnPriceChanged;

	[SerializeField]
	private int priceTier;

	[SerializeField]
	internal string shopItemID;

	[SerializeField]
	private bool isExcludeFromArcher;

	[SerializeField]
	private bool isExcludeFromArcherAdFree;

	public bool IsDataReceived { get; set; }

	public string ShopItemID
	{
		get
		{
			return (shopItemID == null) ? string.Empty : shopItemID;
		}
	}

	public string ProductID
	{
		get
		{
			return "com.playgendary.bowmasters.iap." + ShopItemID;
		}
	}

	public bool Bought
	{
		get
		{
			if (CustomPlayerPrefs.HasKey(ProductID))
			{
				string text = MiniclipUtils.ConvertEncriptedHexToString(CustomPlayerPrefs.GetString(ProductID), ShopItemID + SystemInfo.deviceModel);
				return !string.IsNullOrEmpty(text) && text.Equals(ProductID);
			}
			return false;
		}
	}

	public string ProductReward
	{
		get
		{
			return "{\"nci\":[\"" + ShopItemID + "\"]}";
		}
	}

	public bool IsExcludeProductID
	{
		get
		{
			return isExcludeFromArcher;
		}
	}

	public string PriceString
	{
		get
		{
			string text = null;
			if (IsDataReceived)
			{
				return LLStoreManager.LocalizedPriceProduct(ProductID);
			}
			return string.Empty;
		}
	}

	public virtual bool CanBuy
	{
		get
		{
			bool flag = true;
			// return SingletonMonoBehaviour<ShopManager>.Instance.CanMakePurchase() && IsDataReceived;
			return SingletonMonoBehaviour<ShopManager>.Instance.CanMakePurchase();
		}
	}

	public int PriceTier
	{
		get
		{
			return ConvertTierToUSD(priceTier);
		}
	}

	public event Action<ShopItem> OnItemPurchased
	{
		add
		{
			Action<ShopItem> action = this.m_OnItemPurchased;
			Action<ShopItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnItemPurchased, (Action<ShopItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ShopItem> action = this.m_OnItemPurchased;
			Action<ShopItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnItemPurchased, (Action<ShopItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<ShopItem> OnItemPurchaseFailed
	{
		add
		{
			Action<ShopItem> action = this.m_OnItemPurchaseFailed;
			Action<ShopItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnItemPurchaseFailed, (Action<ShopItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ShopItem> action = this.m_OnItemPurchaseFailed;
			Action<ShopItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnItemPurchaseFailed, (Action<ShopItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<ShopItem> OnPriceChanged
	{
		add
		{
			Action<ShopItem> action = this.m_OnPriceChanged;
			Action<ShopItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnPriceChanged, (Action<ShopItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<ShopItem> action = this.m_OnPriceChanged;
			Action<ShopItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnPriceChanged, (Action<ShopItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public virtual bool IsConsumable()
	{
		return false;
	}

	public bool TryBuy()
	{
		if (CanBuy)
		{
			Debug.LogError("购买");
			// SingletonMonoBehaviour<ShopManager>.Instance.PurchaseProduct(this);
			// return true;
			//TODO:激励--购买角色和物品
			GUIScreenManager.CustomReward = null;
			GUIScreenManager.CustomReward += RewardFun;
			ADManager.Instance.ShowAD_Reward("ScreenManager","InvokeRewardFun");
			// RewardFun();
			return true;
		}
		if (!IsDataReceived)
		{
			
			Debug.LogError("不能购买，走这一步");
			// SingletonMonoBehaviour<ShopManager>.Instance.CheckForNotReceivedItems();
			// return true;
		
			GUIScreenManager.CustomReward = null;
			GUIScreenManager.CustomReward += RewardFun;
			ADManager.Instance.ShowAD_Reward("ScreenManager","InvokeRewardFun");
			// RewardFun();
			return false;
		
		}
		
		return false;
	}


	public void RewardFun()
	{
		SingletonMonoBehaviour<ShopManager>.Instance.PurchaseProduct(this);
	}
	
	
	
	

	public virtual void PurchaseComplete(LLTransactionState transactionState)
	{
		if (!Bought)
		{
			OpenShopItem();
			SingletonMonoBehaviour<StatisticsManager>.Instance.SpentBucks += PriceTier;
			this.m_OnItemPurchased?.Invoke(this);
		}
		if (transactionState == LLTransactionState.Purchased)
		{
			Analytics.CustomEvent(string.Format("{0}_{1}", "Shop_Item_Purchased", shopItemID));
		}
	}

	public virtual void PurchaseFailed()
	{
		this.m_OnItemPurchaseFailed?.Invoke(this);
	}

	public virtual void UpdatePrice()
	{
		this.m_OnPriceChanged?.Invoke(this);
	}

	public virtual void PurchaseSuccessful()
	{
		this.m_OnItemPurchased?.Invoke(this);
	}

	protected virtual void WritePurchaseToPrefs()
	{
		CustomPlayerPrefs.SetInt(ProductID, 1);
	}

	protected virtual void OpenShopItem(bool isPlayHappySound = false)
	{
		if (!IsConsumable())
		{
			CustomPlayerPrefs.SetString(ProductID, MiniclipUtils.ConvertStringToEncriptedHex(ProductID, ShopItemID + SystemInfo.deviceModel));
		}
		if (SingletonMonoBehaviour<GUIScreenManager>.Instance.ActiveScreen == GUIScreenType.Shop)
		{
			CloudSaveManager.Instance.SaveToCloud();
		}
		if (isPlayHappySound)
		{
			AudioManager.PlayAudioWithKey("roulette_win");
		}
		SingletonMonoBehaviour<TutorialManager>.Instance.RefreshTutorial();
	}

	private int ConvertTierToUSD(int tier)
	{
		int result = 0;
		if (tier <= 50)
		{
			result = tier;
		}
		else
		{
			switch (tier)
			{
			case 51:
				result = 55;
				break;
			case 52:
				result = 60;
				break;
			case 53:
				result = 65;
				break;
			case 54:
				result = 70;
				break;
			case 55:
				result = 75;
				break;
			case 56:
				result = 80;
				break;
			case 57:
				result = 85;
				break;
			case 58:
				result = 90;
				break;
			case 59:
				result = 95;
				break;
			case 60:
				result = 100;
				break;
			case 61:
				result = 110;
				break;
			case 62:
				result = 120;
				break;
			case 63:
				result = 125;
				break;
			case 64:
				result = 130;
				break;
			case 65:
				result = 140;
				break;
			case 66:
				result = 150;
				break;
			case 67:
				result = 160;
				break;
			case 68:
				result = 170;
				break;
			case 69:
				result = 175;
				break;
			case 70:
				result = 180;
				break;
			case 71:
				result = 190;
				break;
			case 72:
				result = 200;
				break;
			case 73:
				result = 210;
				break;
			case 74:
				result = 220;
				break;
			case 75:
				result = 230;
				break;
			case 77:
				result = 240;
				break;
			case 78:
				result = 250;
				break;
			case 79:
				result = 300;
				break;
			case 80:
				result = 400;
				break;
			case 81:
				result = 450;
				break;
			case 82:
				result = 500;
				break;
			case 83:
				result = 600;
				break;
			case 84:
				result = 700;
				break;
			case 85:
				result = 800;
				break;
			case 86:
				result = 900;
				break;
			case 87:
				result = 1000;
				break;
			}
		}
		return result;
	}
}
