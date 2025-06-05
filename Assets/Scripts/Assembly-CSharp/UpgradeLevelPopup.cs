using System;
using System.Collections;
using Mit_Goliath_Native.Utils;
using Spine;
using UnityEngine;
using UnityEngine.Analytics;

public class UpgradeLevelPopup : GUIPopup
{
	private const string ANIMATION_IDLE = "idle";

	private const string ANIMATION_WIN = "win";

	private const string ANIMATION_SKELETON_NAME = "Skeleton";

	private const string ANIMATION_SKELETON_BEEFY_NAME = "SkeletonBeefy";

	private const string ANIMATION_SKELETON_PLUMPY_NAME = "SkeletonPlumpy";

	[SerializeField]
	private tk2dUIItem upgradeButton;

	[SerializeField]
	private tk2dTextMesh priceGemsLabel;

	[SerializeField]
	private SkeletonAnimation characterAnimation;

	[SerializeField]
	private tk2dSprite weaponIconSprite;

	[SerializeField]
	private SkeletonAnimation upgradeCharacterAnimation;

	[SerializeField]
	private tk2dSprite upgradeWeaponIconSprite;

	[SerializeField]
	private Transform luminescenceEffectParent;

	[SerializeField]
	private Transform arrowEffectParent;

	private ShopItemCharacter item;

	private MenuScreenScrollView menuScreenScrollView;

	private MenuScreenScrollView MenuScreenScrollView
	{
		get
		{
			if (menuScreenScrollView == null)
			{
				menuScreenScrollView = base.CachedManagers.CachedGUIScreenManager.MenuScreen.MenuScreenScrollView;
			}
			return menuScreenScrollView;
		}
	}

	private void Awake()
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIUpgradeLevelArrow));
			gameObject.transform.parent = arrowEffectParent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one * ((!tk2dSystem.IsRetina) ? 1 : 2);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIUpgradeLevelLuminiscenceIdle));
			gameObject2.transform.parent = luminescenceEffectParent;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.one * ((!tk2dSystem.IsRetina) ? 1 : 2);
		}, 0f);
	}

	protected override void OnEnable()
	{
		upgradeButton.OnClick += UpgradeButton_OnClick;
		item = base.CachedManagers.CachedShopManager.AvailableCharacters[MenuScreenScrollView.SelectedIndex];
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		upgradeButton.OnClick -= UpgradeButton_OnClick;
		base.OnDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = onFinished;
		if (item != null)
		{
			CharacterNameType characterNameType = ((item.CharacterUpgradeInfo.upgradeLevel <= 0) ? item.Infos[0].characterNameType : item.CharacterUpgradeInfo.characterNameType);
			GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(characterNameType);
			if (gUICharacter != null)
			{
				characterAnimation.SkeletonDataAsset = gUICharacter.SkeletonAsset;
				characterAnimation.gameObject.transform.localScale *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
				characterAnimation.Reset();
				FadeSkeletons(characterAnimation);
				characterAnimation.State.SetAnimation(0, "idle", true);
				weaponIconSprite.SetSprite(gUICharacter.WeaponIconName);
			}
			gUICharacter = base.ObjectsSpawner.GetGUICharacter(item.NextLevelCharacterUpgradeInfo.characterNameType);
			if (gUICharacter != null)
			{
				upgradeCharacterAnimation.SkeletonDataAsset = gUICharacter.SkeletonAsset;
				upgradeCharacterAnimation.gameObject.transform.localScale *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
				upgradeCharacterAnimation.Reset();
				FadeSkeletons(upgradeCharacterAnimation);
				upgradeCharacterAnimation.State.SetAnimation(0, "idle", true);
				upgradeWeaponIconSprite.SetSprite(gUICharacter.WeaponIconName);
			}
			priceGemsLabel.text = item.NextLevelCharacterUpgradeInfo.priceGems.ToString();
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

	public void PlayItemPurchasedEffect(GameObject animationEffectPrefab)
	{
		if (animationEffectPrefab != null)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIUpgradeLevelLuminiscence));
				gameObject.transform.parent = luminescenceEffectParent;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one * ((!tk2dSystem.IsRetina) ? 1 : 2);
			}, 0f);
		}
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

	private void UpgradeButton_OnClick()
	{
		if (item != null)
		{
			int priceGems = item.NextLevelCharacterUpgradeInfo.priceGems;
			int gems = base.CachedManagers.CachedStatisticsManager.Gems;
			if (gems >= priceGems)
			{
				base.CachedManagers.CachedStatisticsManager.DecreaseGems(priceGems);
				item.UpgradeCharacterLevel();
				MenuScreenScrollView.InitializeCharacters(true);
				MenuScreenScrollView.UpdateCharacterCharacteristics();
				base.CachedManagers.CachedTutorialManager.StopBuyCharacterOnCoinsTutorial();
				base.CachedManagers.CachedTutorialManager.LoadBuyCharacterOnCoinsTutorial(MenuScreenScrollView);
				StartCoroutine(CoroutineUpgradeCharacter());
				base.CachedManagers.CachedTutorialManager.WasCharacterUpgradedOnGems = true;
				upgradeButton.enabled = false;
				GameGoliath.Instance.TrackCharacterAction(item.GetCharacterID(), GoliathIds.Action.UPGRADE, GoliathIds.Origin.SOFT_PURCHASE, string.Empty);
				GameGoliath.Instance.TrackVirtualCurrencyPurchase(GoliathIds.CurrencyName.GEMS, priceGems.ToString(), item.GetCharacterID(), GoliathIds.ProductFamily.CHARACTER_UPGRADE);
				GameGoliath.Instance.TrackCurrencyFlow(GoliathIds.CurrencyMovement.SPENT, GoliathIds.Context.UPGRADE_CHARACTER, GoliathIds.CurrencyName.GEMS, priceGems, GoliathIds.Placement.CHARACTER_SELECTION, gems);
				Analytics.CustomEvent(string.Format("{0}_{1}", "Upgrade_Character", "ANY"));
				Analytics.CustomEvent(string.Format("{0}_{1}", "Upgrade_Character", item.ShopItemID));
			}
			else
			{
				GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true);
				(gUIPopup as MiniBankPopup).InitPopup(item.NextLevelCharacterUpgradeInfo.priceGems);
			}
		}
	}

	private IEnumerator CoroutineUpgradeCharacter()
	{
		PlayItemPurchasedSound(item.IsPremium);
		EffectType effectType = ((!item.IsPremium) ? EffectType.GUIMenuBuyCharacterCommonAnimation : EffectType.GUIMenuBuyCharacterRareAnimation);
		PlayItemPurchasedEffect(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(effectType));
		AudioManager.PlayAudioWithKey("roulette_win");
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = false;
		characterAnimation.State.SetAnimation(0, "win", false);
		TrackEntry track = upgradeCharacterAnimation.State.SetAnimation(0, "win", false);
		yield return new WaitForSeconds(track.EndTime);
		base.CachedManagers.CachedGUIPopupManager.HideActivePopup();
	}
}
