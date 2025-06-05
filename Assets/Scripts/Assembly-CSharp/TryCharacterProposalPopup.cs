using System;
using UnityEngine;

public class TryCharacterProposalPopup : GUIPopup
{
	private const string ANIMATION_IDLE = "idle";

	private const string COINS_PREFIX_KEY = "ui.popup.menu.trycharacter.proposal.button.coins";

	private const string ANIMATION_SKELETON_NAME = "Skeleton";

	private const string ANIMATION_SKELETON_BEEFY_NAME = "SkeletonBeefy";

	private const string ANIMATION_SKELETON_PLUMPY_NAME = "SkeletonPlumpy";

	[SerializeField]
	private SkeletonAnimation archerAnimation;

	[SerializeField]
	private tk2dUIItem tryFreeButton;

	[SerializeField]
	private tk2dUIItem tryForCoinsButton;

	[SerializeField]
	private tk2dUIItem tryForGemsButton;

	[SerializeField]
	private tk2dTextMesh[] priceLabels;

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

	protected ShopItemCharacter item;

	private void Awake()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIPopupBuyCharacterConfetti));
		gameObject.transform.parent = confettiEffectParent;
		gameObject.transform.localPosition = Vector3.zero;
		if (gameObject != null && tk2dSystem.IsRetina)
		{
			ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].transform.localScale = Vector3.one * 2f;
			}
		}
	}

	protected override void OnEnable()
	{
		tryFreeButton.OnClick += TryButton_OnClick;
		if (tryForCoinsButton != null)
		{
			tryForCoinsButton.OnClick += TryButton_OnClick;
		}
		if (tryForGemsButton != null)
		{
			tryForGemsButton.OnClick += TryButton_OnClick;
		}
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		tryFreeButton.OnClick -= TryButton_OnClick;
		if (tryForCoinsButton != null)
		{
			tryForCoinsButton.OnClick -= TryButton_OnClick;
		}
		if (tryForGemsButton != null)
		{
			tryForGemsButton.OnClick -= TryButton_OnClick;
		}
		base.OnDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		if (tryForCoinsButton != null && tryForGemsButton != null)
		{
			bool flag = false;
			bool isSubscriptionActive = base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
			tryFreeButton.gameObject.SetActive(!flag && !isSubscriptionActive);
			tryForCoinsButton.gameObject.SetActive(flag);
			tryForGemsButton.gameObject.SetActive(!flag && isSubscriptionActive);
			tk2dTextMesh[] array = priceLabels;
			foreach (tk2dTextMesh tk2dTextMesh2 in array)
			{
				tk2dTextMesh2.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.popup.menu.trycharacter.proposal.button.coins");
				tk2dTextMesh2.text += ((!isSubscriptionActive) ? base.CachedManagers.CachedProposalManager.PriceForTryCharacter.ToString() : base.CachedManagers.CachedProposalManager.PriceForTryCharacterGems.ToString());
			}
		}
		Action<GUIControl> onFinished2 = onFinished;
		item = base.CachedManagers.CachedProposalManager.TryCharacter;
		GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(item.Infos[0].characterNameType);
		if (item != null)
		{
			if ((bool)gUICharacter && (bool)gUICharacter.SkeletonAsset)
			{
				archerAnimation.SkeletonDataAsset = gUICharacter.SkeletonAsset;
				archerAnimation.gameObject.transform.localScale *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					FadeSkeletons(archerAnimation);
					archerAnimation.Reset();
					archerAnimation.SkeletonDataAsset.Reset();
					archerAnimation.Initialize();
					archerAnimation.State.ClearTracks();
					archerAnimation.State.SetAnimation(0, "idle", true);
				}, 0.05f);
			}
			bool isPremium = item.IsPremium;
			flagPremium.SetActive(isPremium);
			flagUsual.SetActive(!isPremium);
			characterName.text = base.ObjectsSpawner.GetCharacterLocalizedName(item.Infos[0].characterNameType);
			characterDescription.text = base.ObjectsSpawner.GetCharacterLocalizedDescription(item.Infos[0].characterNameType);
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

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		tryFreeButton.enabled = false;
		if (tryForCoinsButton != null)
		{
			tryForCoinsButton.enabled = false;
		}
		if (tryForGemsButton != null)
		{
			tryForGemsButton.enabled = false;
		}
		base.Hide(onFinished, immediately, delay, callInvokes);
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

	protected virtual void TryButton_OnClick()
	{
		base.CachedManagers.CachedProposalManager.GetProposalMenu(null);
		Hide();
	}

	protected override void CloseButton_OnClick()
	{
		base.CachedManagers.CachedProposalManager.GenerateProposalMenu(true);
		base.CloseButton_OnClick();
	}
}
