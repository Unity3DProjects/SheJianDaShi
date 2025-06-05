using System;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;

public class InvitePopup : GUIPopup
{
	private const string UI_POPUP_INVITE_CHARACTER_MESSAGE = "ui.popup.menu.invite.label.bottom";

	private const string UI_POPUP_INVITE_ORDINARY_CHARACTER_MESSAGE = "ui.popup.menu.invite.label.top.ordinary";

	private const string UI_POPUP_INVITE_LIMITED_CHARACTER_MESSAGE = "ui.popup.menu.invite.label.top.limited";

	[SerializeField]
	private tk2dUIItem sendButton;

	[SerializeField]
	private tk2dSprite characterSprite;

	[SerializeField]
	private tk2dTextMesh nameLabel;

	[SerializeField]
	private tk2dTextMesh descriptionLabel;

	[SerializeField]
	private tk2dTextMesh invitedFriendsCountLabel;

	[SerializeField]
	private Transform characterEffectTransform;

	private GameObject characterEffect;

	protected override void OnEnable()
	{
		base.OnEnable();
		sendButton.OnClick += SendButton_OnClick;
		GiftManager.OnInviteCharacterOpen += GiftManager_OnInviteCharacterOpen;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		sendButton.OnClick -= SendButton_OnClick;
		GiftManager.OnInviteCharacterOpen -= GiftManager_OnInviteCharacterOpen;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		RefreshInvitePopup();
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			characterEffect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.GUIPopupInviteSecond));
			characterEffect.transform.parent = characterEffectTransform;
			characterEffect.transform.localPosition = Vector3.zero;
		}, 0f);
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		if (characterEffect != null)
		{
			UnityEngine.Object.Destroy(characterEffect);
			characterEffect = null;
		}
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void RefreshInvitePopup()
	{
		if (!(invitedFriendsCountLabel != null) || !descriptionLabel || !(characterSprite != null))
		{
			return;
		}
		GiftInviteCharacter currentInviteCharacter = base.CachedManagers.CachedInviteManager.CurrentInviteCharacter;
		if (!currentInviteCharacter.inviteCharacter.IsNull() && !currentInviteCharacter.inviteCharacterSpriteName.IsNull())
		{
			ShopItemCharacter shopItemCharacter = base.ObjectsSpawner.GetShopItemCharacter(currentInviteCharacter.inviteCharacter);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(LocalisationManager.LocalizedStringOrSource("ui.popup.menu.invite.label.bottom"));
			if (base.CachedManagers.CachedInviteManager.DifferentInvitesCounters.ContainsKey(shopItemCharacter.Infos[0].characterNameType))
			{
				stringBuilder.Replace("%tag_number%", string.Format("{0}/{1}", base.CachedManagers.CachedInviteManager.DifferentInvitesCounters[shopItemCharacter.Infos[0].characterNameType].ToString(), currentInviteCharacter.invitesNeededCount.ToString()));
			}
			invitedFriendsCountLabel.text = stringBuilder.ToString();
			string key = ((!shopItemCharacter.IsEventGift) ? "ui.popup.menu.invite.label.top.ordinary" : "ui.popup.menu.invite.label.top.limited");
			descriptionLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey(key);
			characterSprite.SetSprite(currentInviteCharacter.inviteCharacterSpriteName);
		}
	}

	private void SendButton_OnClick()
	{
		AudioManager.PlaySingleAudioWithKey("button");
		SingletonMonoBehaviour<InviteManager>.Instance.InviteForCharacter(delegate
		{
			ShopItemCharacter shopItemCharacter = base.ObjectsSpawner.GetShopItemCharacter(base.CachedManagers.CachedInviteManager.CurrentInviteCharacter.inviteCharacter);
			if (shopItemCharacter.Availible)
			{
				Analytics.CustomEvent("Invite_Character_" + shopItemCharacter.ShopItemID + "_Complete");
			}
			RefreshInvitePopup();
		});
	}

	private void GiftManager_OnInviteCharacterOpen()
	{
		GiftInviteCharacter currentInviteCharacter = base.CachedManagers.CachedInviteManager.CurrentInviteCharacter;
		if (base.CachedManagers.CachedInviteManager.DifferentInvitesCounters[currentInviteCharacter.inviteCharacter] >= currentInviteCharacter.invitesNeededCount)
		{
			Hide();
		}
	}
}
