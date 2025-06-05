using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGiftCharacterPopup : GUIPopup
{
	private const string ANIMATION_IDLE = "idle";

	private const string ANIMATION_POSE = "idle_fight";

	private const string HEADER_LABEL_VIDEO_GIFT = "ui.popup.menu.special.gift.video.character.header.label";

	private const string HEADER_LABEL_VIDEO_GIFT_AD_FREE = "ui.popup.menu.special.gift.video.character.header.label.adfree";

	private const string HEADER_LABEL_MULTIPLAYER_GIFT = "ui.popup.menu.special.gift.multiplayer.character.header.label";

	private const string HEADER_LABEL_INVITE_GIFT = "ui.popup.menu.special.gift.invite.character.header.label";

	private const string HEADER_LABEL_SUBSCRIBE_GIFT = "ui.popup.menu.subscribe.description.label";

	private const string HEADER_LABEL_TOURNAMENT_GIFT = "ui.popup.menu.special.gift.legendarytournament.character.header.label";

	private const string DESCRIPTION_TEXT = "ui.popup.menu.character.from.video.description.label";

	private const string ANIMATION_SKELETON_NAME = "Skeleton";

	private const string ANIMATION_SKELETON_BEEFY_NAME = "SkeletonBeefy";

	private const string ANIMATION_SKELETON_PLUMPY_NAME = "SkeletonPlumpy";

	[SerializeField]
	private SkeletonAnimation archerAnimation;

	[SerializeField]
	private tk2dTextMesh headerLabel;

	[SerializeField]
	private tk2dTextMesh descriptionLabel;

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = onFinished;
		GiftManager instance = SingletonMonoBehaviour<GiftManager>.Instance;
		ShopItemCharacter shopItemCharacter = base.ObjectsSpawner.GetShopItemCharacter(instance.LastOpenedSpecialGiftCharacter);
		GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(instance.LastOpenedSpecialGiftCharacter);
		CharacterNameType lastOpenedSpecialGiftCharacter = instance.LastOpenedSpecialGiftCharacter;
		if (shopItemCharacter != null)
		{
			int num = 0;
			string key = string.Empty;
			string key2 = "ui.popup.menu.character.from.video.description.label";
			if (lastOpenedSpecialGiftCharacter == instance.MultiplayerGiftCharacter)
			{
				key = "ui.popup.menu.special.gift.multiplayer.character.header.label";
				num = instance.MultiplayerWinsForGiftCharacter;
			}
			else if (lastOpenedSpecialGiftCharacter == instance.VideoGiftCharacter)
			{
				key = "ui.popup.menu.special.gift.video.character.header.label";
				num = instance.VideosForGiftCharacter;
			}
			else if (lastOpenedSpecialGiftCharacter == instance.TournamentGiftCharacter)
			{
				key = "ui.popup.menu.special.gift.legendarytournament.character.header.label";
				num = instance.LegendaryTournamenWinsForGiftCharacter;
			}
			else if (shopItemCharacter.LockedByAdsInfo.isLockedByAds)
			{
				key = "ui.popup.menu.special.gift.video.character.header.label";
				num = shopItemCharacter.LockedByAdsInfo.totalAdsCount;
			}
			else
			{
				List<GiftInviteCharacter> inviteGiftCharacters = instance.InviteGiftCharacters;
				List<GiftSubscribeCharacter> subscribeGiftCharacters = instance.SubscribeGiftCharacters;
				for (int i = 0; i < inviteGiftCharacters.Count; i++)
				{
					if (inviteGiftCharacters[i].inviteCharacter == lastOpenedSpecialGiftCharacter)
					{
						key = "ui.popup.menu.special.gift.invite.character.header.label";
						num = inviteGiftCharacters[i].invitesNeededCount;
						break;
					}
				}
				for (int j = 0; j < subscribeGiftCharacters.Count; j++)
				{
					if (subscribeGiftCharacters[j].subscribeCharacter == lastOpenedSpecialGiftCharacter)
					{
						key = "ui.popup.menu.subscribe.description.label";
						key2 = string.Empty;
						break;
					}
				}
			}
			headerLabel.text = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(key).Replace("%", num.ToString());
			descriptionLabel.text = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(key2);
			if ((bool)gUICharacter && (bool)gUICharacter.SkeletonAsset)
			{
				if (gUICharacter.CharacterMode == CharacterMode.Random)
				{
					List<SkeletonAnimation> list = gUICharacter.AddDoubleCharacterAnimations(archerAnimation, archerAnimation.transform.parent);
					for (int k = 0; k < list.Count; k++)
					{
						SkeletonAnimation skeletonAnimation = UnityEngine.Object.Instantiate(list[k], archerAnimation.transform.parent);
						UnityEngine.Object.Destroy(list[k].gameObject);
						skeletonAnimation.gameObject.transform.localScale = Vector3.one * ((!tk2dSystem.IsRetina) ? 0.7f : 1.4f);
						skeletonAnimation.Reset();
						FadeSkeletons(skeletonAnimation);
						skeletonAnimation.gameObject.transform.localPosition *= 0.7f;
						skeletonAnimation.State.SetAnimation(0, "idle_fight", true);
						skeletonAnimation.gameObject.AddComponent<CustomRenderQueue>();
					}
					archerAnimation.gameObject.SetActive(false);
				}
				else
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
						FadeSkeletons(archerAnimation);
						archerAnimation.State.SetAnimation(0, "idle", true);
						archerAnimation.gameObject.AddComponent<CustomRenderQueue>();
					}, 0.05f);
				}
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
