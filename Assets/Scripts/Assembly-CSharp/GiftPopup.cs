using System;
using UnityEngine;

public class GiftPopup : GUIPopup
{
	private const string ANIMATION_IDLE = "idle";

	private const string ANIMATION_SKELETON_NAME = "Skeleton";

	private const string ANIMATION_SKELETON_BEEFY_NAME = "SkeletonBeefy";

	private const string ANIMATION_SKELETON_PLUMPY_NAME = "SkeletonPlumpy";

	[SerializeField]
	private SkeletonAnimation archerAnimation;

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

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.GUIPopupGiftConfetti));
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
		Action<GUIControl> onFinished2 = onFinished;
		item = SingletonMonoBehaviour<ProposalManager>.Instance.GiftCharacter;
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
				SingletonMonoBehaviour<GUIPopupManager>.Instance.HideActivePopup();
			};
		}
		base.Show(onFinished2, immediately, delay, callInvokes);
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
}
