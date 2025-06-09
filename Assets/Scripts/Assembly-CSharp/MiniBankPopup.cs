using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class MiniBankPopup : GUIPopup
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnBankItemPurchsed;

	[SerializeField]
	private GUIShopSlot slotPrefab;

	[SerializeField]
	private GUILayoutCell leftParentCell;

	[SerializeField]
	private GUILayoutCell rightParentCell;

	private ShopItemGems leftShopItem;

	private ShopItemGems rightShopItem;

	private int necessaryAmount;

	public static event Action<bool> OnBankItemPurchsed
	{
		add
		{
			Action<bool> action = MiniBankPopup.m_OnBankItemPurchsed;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MiniBankPopup.m_OnBankItemPurchsed, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MiniBankPopup.m_OnBankItemPurchsed;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MiniBankPopup.m_OnBankItemPurchsed, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GUIShopSlot.OnBankItemPurchased = (Action)Delegate.Combine(GUIShopSlot.OnBankItemPurchased, new Action(GUIShopSlot_OnBankItemPurchased));
	}

	protected override void OnDisable()
	{
		base.OnEnable();
		GUIShopSlot.OnBankItemPurchased = (Action)Delegate.Remove(GUIShopSlot.OnBankItemPurchased, new Action(GUIShopSlot_OnBankItemPurchased));
	}

	public void InitPopup(int requiredAmount = 0, bool isCoinsPopup = false)
	{
		necessaryAmount = requiredAmount;
		if (isCoinsPopup)
		{
			List<ShopItemGems> bankItems = base.CachedManagers.CachedShopManager.BankItems;
			List<ShopItemGems> list = new List<ShopItemGems>();
			for (int i = 0; i < bankItems.Count; i++)
			{
				if (bankItems[i] is ShopItemCoins)
				{
					list.Add(bankItems[i]);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].RewardValue > necessaryAmount || j == list.Count - 1)
				{
					leftShopItem = list[j];
					rightShopItem = list[Mathf.Clamp(j + 2, j, list.Count - 1)];
					break;
				}
			}
			InitSlot(leftParentCell, leftShopItem);
			InitSlot(rightParentCell, rightShopItem);
			return;
		}
		List<ShopItemGems> bankItems2 = base.CachedManagers.CachedShopManager.BankItems;
		List<ShopItemGems> list2 = new List<ShopItemGems>();
		for (int k = 0; k < bankItems2.Count; k++)
		{
			if (!(bankItems2[k] is ShopItemCoins))
			{
				list2.Add(bankItems2[k]);
			}
		}
		for (int l = 0; l < list2.Count; l++)
		{
			if (list2[l].RewardValue > necessaryAmount || l == list2.Count - 1)
			{
				leftShopItem = list2[l];
				rightShopItem = list2[Mathf.Clamp(l + 2, l, list2.Count - 1)];
				break;
			}
		}
		InitSlot(leftParentCell, leftShopItem);
		InitSlot(rightParentCell, rightShopItem);
	}

	private void InitSlot(GUILayoutCell parentCell, ShopItemGems shopItem)
	{
		GUIShopSlot gUIShopSlot = UnityEngine.Object.Instantiate(slotPrefab);
		gUIShopSlot.Init(shopItem, parentCell, true);
		gUIShopSlot.transform.parent = parentCell.CachedTransform;
		gUIShopSlot.transform.localPosition = Vector3.zero;
		gUIShopSlot.transform.localScale = Vector3.one;
		tk2dBaseMesh[] componentsInChildren = gUIShopSlot.GetComponentsInChildren<tk2dBaseMesh>();
		SkeletonAnimation[] componentsInChildren2 = gUIShopSlot.GetComponentsInChildren<SkeletonAnimation>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].CachedRenderer.sortingLayerName = "Overlayer";
			componentsInChildren[i].CachedRenderer.sortingOrder = 60;
		}
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].MeshRenderer.sortingLayerName = "Overlayer";
			componentsInChildren2[j].MeshRenderer.sortingOrder = 60;
		}
	}

	private void GUIShopSlot_OnBankItemPurchased()
	{
		CloseButton_OnClick();
		if (MiniBankPopup.m_OnBankItemPurchsed != null)
		{
			bool obj = leftShopItem.GetType() != typeof(ShopItemCoins);
			MiniBankPopup.m_OnBankItemPurchsed(obj);
			
		}
	}
}
