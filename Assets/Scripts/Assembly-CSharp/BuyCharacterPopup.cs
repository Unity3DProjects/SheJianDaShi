using System;
using System.Collections;
using System.Collections.Generic;
using Mit_Goliath_Native.Utils;
using Spine;
using UnityEngine;
using UnityEngine.Analytics;

public class BuyCharacterPopup : GUIPopup
{
	private const string ANIMATION_IDLE = "idle";

	private const string ANIMATION_WIN = "win";

	private const string ANIMATION_SKELETON_NAME = "Skeleton";

	private const string ANIMATION_SKELETON_BEEFY_NAME = "SkeletonBeefy";

	private const string ANIMATION_SKELETON_PLUMPY_NAME = "SkeletonPlumpy";

	[SerializeField]
	private SkeletonAnimation archerAnimation;

	[SerializeField]
	private tk2dUIItem oneButtonLayouterBucksButton;

	[SerializeField]
	private tk2dTextMesh oneButtonLayouterBucksLabel;

	[SerializeField]
	private SkeletonAnimation oneButtonLayouterBucksLoading;

	[SerializeField]
	private tk2dUIItem twoButtonLayouterCoinsButton;

	[SerializeField]
	private tk2dTextMesh twoButtonLayouterCoinsLabel;

	[SerializeField]
	private tk2dUIItem twoButtonLayouterBucksButton;

	[SerializeField]
	private tk2dTextMesh twoButtonLayouterBucksLabel;

	[SerializeField]
	private SkeletonAnimation twoButtonLayouterBucksLoading;

	[SerializeField]
	private GUILayouter oneButtonLayouter;

	[SerializeField]
	private GUILayouter twoButtonLayouter;

	[SerializeField]
	private tk2dBaseSprite coinsButtonBack;

	[SerializeField]
	private tk2dBaseSprite coinsButtonCoinSprite;

	[SerializeField]
	private Color nonActiveColor;

	[SerializeField]
	private GameObject flagPremium;

	[SerializeField]
	private GameObject flagUsual;

	[SerializeField]
	private tk2dTextMesh characterName;

	[SerializeField]
	private tk2dTextMesh characterDescription;

	[SerializeField]
	private Transform confettiEffectParent;

	private ShopItemCharacter item;

	private void Awake()
	{
		if (tk2dSystem.IsRetina)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIPopupBuyCharacterConfetti));
				gameObject.transform.parent = confettiEffectParent;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one * 2f;
			}, 0f);
		}
	}

	protected override void OnEnable()
	{
		oneButtonLayouterBucksButton.OnClick += BucksButtons_OnClick;
		twoButtonLayouterBucksButton.OnClick += BucksButtons_OnClick;
		twoButtonLayouterCoinsButton.OnClick += CoinsButton_OnClick;
		item = base.CachedManagers.CachedProposalManager.BuyCharacter;
		item.OnPriceChanged += Item_OnPriceChanged;
		base.OnEnable();
		MiniBankPopup.OnBankItemPurchsed += MiniBankPopup_OnBankItemPurchsed;
	}

	protected override void OnDisable()
	{
		oneButtonLayouterBucksButton.OnClick -= BucksButtons_OnClick;
		twoButtonLayouterBucksButton.OnClick -= BucksButtons_OnClick;
		twoButtonLayouterCoinsButton.OnClick -= CoinsButton_OnClick;
		if (item != null)
		{
			item.OnPriceChanged -= Item_OnPriceChanged;
		}
		base.OnDisable();
		MiniBankPopup.OnBankItemPurchsed -= MiniBankPopup_OnBankItemPurchsed;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = onFinished;
		if (item != null)
		{
			GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(item.Infos[0].characterNameType);
			if ((bool)gUICharacter && (bool)gUICharacter.SkeletonAsset)
			{
				archerAnimation.SkeletonDataAsset = gUICharacter.SkeletonAsset;
				archerAnimation.gameObject.transform.localScale *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
				archerAnimation.Reset();
				FadeSkeletons(archerAnimation);
				archerAnimation.State.SetAnimation(0, "idle", true);
			}
			bool isPremium = item.IsPremium;
			string priceString = item.PriceString;
			bool flag = !string.IsNullOrEmpty(priceString);
			tk2dTextMesh tk2dTextMesh2 = ((!isPremium) ? twoButtonLayouterBucksLabel : oneButtonLayouterBucksLabel);
			SkeletonAnimation skeletonAnimation = ((!isPremium) ? twoButtonLayouterBucksLoading : oneButtonLayouterBucksLoading);
			oneButtonLayouter.gameObject.SetActive(isPremium);
			twoButtonLayouter.gameObject.SetActive(!isPremium);
			twoButtonLayouterCoinsLabel.text = item.PriceCoinsString;
			tk2dTextMesh2.gameObject.SetActive(flag);
			skeletonAnimation.gameObject.SetActive(!flag);
			tk2dTextMesh2.text = item.PriceString;
			flagPremium.SetActive(isPremium);
			flagUsual.SetActive(!isPremium);
			characterName.text = base.ObjectsSpawner.GetCharacterLocalizedName(item.Infos[0].characterNameType);
			characterDescription.text = base.ObjectsSpawner.GetCharacterLocalizedDescription(item.Infos[0].characterNameType);
			if (!isPremium)
			{
				Color white = Color.white;
				if (base.CachedManagers.CachedStatisticsManager.Coins < item.PriceCoins)
				{
					twoButtonLayouterCoinsButton.enabled = false;
					white = nonActiveColor;
				}
				else
				{
					twoButtonLayouterCoinsButton.enabled = true;
				}
				twoButtonLayouterCoinsLabel.color = white;
				coinsButtonBack.color = white;
				coinsButtonCoinSprite.color = white;
			}
		}
		else
		{
			onFinished2 = delegate(GUIControl control)
			{
				if (onFinished != null)
				{
					onFinished(control);
				}
				base.CachedManagers.CachedGUIPopupManager.HideActivePopup();
			};
		}
		base.Show(onFinished2, immediately, delay, callInvokes);
	}

	private void PlayItemPurchasedSound(bool isPremium)
	{
		AudioManager.PlayAudioWithKey((!isPremium) ? "buy_character_common" : "buy_character_rare");
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				AudioManager.PlayAudioWithKey("buy_character");
			}
		}, (!isPremium) ? 0.5f : 0.9f);
	}

	private void PlayItemPurchasedEffect(GameObject animationEffectPrefab)
	{
		if (!(animationEffectPrefab != null))
		{
			return;
		}
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(animationEffectPrefab);
			gameObject.transform.parent = archerAnimation.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			ParticleSystemRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sortingLayerName = "Overlayer";
				componentsInChildren[i].sortingOrder = 5;
			}
		}, 0f);
	}

	private void FadeSkeletons(SkeletonAnimation currentAnimation)
	{
		tk2dSpriteCollectionData[] spriteCollections = currentAnimation.SkeletonDataAsset.spriteCollections;
		for (int i = 0; i < spriteCollections.Length; i++)
		{
			if (!(spriteCollections[i].spriteCollectionName == "Skeleton") && !(spriteCollections[i].spriteCollectionName == "SkeletonBeefy") && !(spriteCollections[i].spriteCollectionName == "SkeletonPlumpy"))
			{
				continue;
			}
			Material[] materialInsts = spriteCollections[i].inst.materialInsts;
			if (materialInsts != null)
			{
				for (int j = 0; j < materialInsts.Length; j++)
				{
					materialInsts[j].mainTexture = Texture2D.blackTexture;
				}
			}
		}
	}

	protected override void CloseButton_OnClick()
	{
		base.CachedManagers.CachedProposalManager.ResetBuyCharacter();
		base.CloseButton_OnClick();
	}

	private void BucksButtons_OnClick()
	{
		if (item != null && !item.Bought)
		{
			item.OnItemPurchased += ShopItem_OnItemPurchased;
			item.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
			if (!item.TryBuy())
			{
				item.OnItemPurchased -= ShopItem_OnItemPurchased;
				item.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
			}
		}
	}

	private void CoinsButton_OnClick()
	{
		if (item != null)
		{
			if (base.CachedManagers.CachedStatisticsManager.Coins >= item.PriceCoins)
			{
				GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.SPENT, GoliathIds.Context.SOFT_PURCHASE, GoliathIds.CurrencyName.COINS, item.PriceCoins, GoliathIds.Placement.POPUP_PACKAGE);
				GameGoliath.Instance.TrackVirtualCurrencyPurchase(GoliathIds.CurrencyName.COINS, item.PriceCoins.ToString(), item.ShopItemID, GoliathIds.ProductFamily.CHARACTER);
				base.CachedManagers.CachedStatisticsManager.DecreaseCoins(item.PriceCoins);
				StartCoroutine(CoroutineOpenCharacter());
			}
			else
			{
				(base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true) as MiniBankPopup).InitPopup(item.PriceCoins, true);
			}
		}
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
	}

	private void Item_OnPriceChanged(ShopItem obj)
	{
		bool isPremium = item.IsPremium;
		string priceString = item.PriceString;
		bool flag = !string.IsNullOrEmpty(priceString);
		tk2dTextMesh tk2dTextMesh2 = ((!isPremium) ? twoButtonLayouterBucksLabel : oneButtonLayouterBucksLabel);
		SkeletonAnimation skeletonAnimation = ((!isPremium) ? twoButtonLayouterBucksLoading : oneButtonLayouterBucksLoading);
		tk2dTextMesh2.gameObject.SetActive(flag);
		skeletonAnimation.gameObject.SetActive(!flag);
		tk2dTextMesh2.text = item.PriceString;
	}

	private void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		Analytics.CustomEvent("Rented_Character_Bought", new Dictionary<string, object>
		{
			{ "CharacterName", shopItem.ShopItemID },
			{
				"GameTimeMinutes",
				cachedStatisticsManager.TotalGameTime.Minutes
			},
			{ "DaysSinceFirstLaunch", cachedStatisticsManager.DaysSinceFirstLaunch }
		});
		StartCoroutine(CoroutineOpenCharacter());
	}

	private void MiniBankPopup_OnBankItemPurchsed(bool areGemsPurchased)
	{
		base.CachedManagers.CachedStatisticsManager.DecreaseCoins(item.PriceCoins);
		StartCoroutine(CoroutineOpenCharacter());
	}

	private IEnumerator CoroutineOpenCharacter()
	{
		PlayItemPurchasedSound(item.IsPremium);
		EffectType effectType = ((!item.IsPremium) ? EffectType.GUIMenuBuyCharacterCommonAnimation : EffectType.GUIMenuBuyCharacterRareAnimation);
		PlayItemPurchasedEffect(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(effectType));
		AudioManager.PlayAudioWithKey("roulette_win");
		twoButtonLayouterCoinsButton.enabled = false;
		twoButtonLayouterBucksButton.enabled = false;
		oneButtonLayouterBucksButton.enabled = false;
		item.OpenCharacter(true, true);
		TrackEntry track = archerAnimation.State.SetAnimation(0, "win", false);
		yield return new WaitForSeconds(track.EndTime);
		base.CachedManagers.CachedGUIPopupManager.HideActivePopup();
	}
}
