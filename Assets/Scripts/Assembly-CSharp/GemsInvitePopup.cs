using System;
using System.Text;
using UnityEngine;

public class GemsInvitePopup : GUIPopup
{
	private const string UI_POPUP_INVITE_CHARACTER_MESSAGE = "ui.popup.menu.invite.label.bottom";

	[SerializeField]
	private tk2dUIItem sendButton;

	[SerializeField]
	private tk2dTextMesh invitedFriendsCountLabel;

	[SerializeField]
	private tk2dTextMesh rewardValueLabel;

	[SerializeField]
	private GUILayoutCell invitesCell;

	[SerializeField]
	private float invitesCellSizeInvites;

	[SerializeField]
	private float invitesCountCellSubscribe;

	[SerializeField]
	private GameObject invitesButtonRoot;

	[SerializeField]
	private GameObject subscribeButtonRoot;

	[SerializeField]
	private GameObject inviteLabelObject;

	[SerializeField]
	private GameObject subscribeLabelObject;

	[SerializeField]
	private SystemLanguage[] languagesWithoutLikeIcon;

	[SerializeField]
	private GameObject likeIcon;

	[SerializeField]
	[ResourceLink]
	private AssetLink shineIdleEffectPrefab;

	[SerializeField]
	private Transform shineIdleTransform;

	private bool isSubscriptionPopup;

	protected override void OnEnable()
	{
		base.OnEnable();
		sendButton.OnClick += SendButton_OnClick;
		GiftManager.OnGiftGemsOpen += GiftManager_OnGiftGemsOpen;
		SubscribeManager.OnSubscribeForGems = (Action)Delegate.Combine(SubscribeManager.OnSubscribeForGems, new Action(SubscribeManager_OnSubscribeForGems));
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		sendButton.OnClick -= SendButton_OnClick;
		GiftManager.OnGiftGemsOpen -= GiftManager_OnGiftGemsOpen;
		SubscribeManager.OnSubscribeForGems = (Action)Delegate.Remove(SubscribeManager.OnSubscribeForGems, new Action(SubscribeManager_OnSubscribeForGems));
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		RefreshInvitePopup();
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public void Init(bool isSubscription, int rewardValue)
	{
		isSubscriptionPopup = isSubscription;
		invitesButtonRoot.SetActive(!isSubscription);
		subscribeButtonRoot.SetActive(isSubscription);
		invitesCell.gameObject.SetActive(!isSubscription);
		invitesCell.SizeValue = ((!isSubscription) ? invitesCellSizeInvites : invitesCountCellSubscribe);
		rewardValueLabel.text = rewardValue.ToString();
		inviteLabelObject.SetActive(!isSubscription);
		subscribeLabelObject.SetActive(isSubscription);
		likeIcon.SetActive(true);
		for (int i = 0; i < languagesWithoutLikeIcon.Length; i++)
		{
			if (languagesWithoutLikeIcon[i] == SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage)
			{
				likeIcon.SetActive(false);
				break;
			}
		}
		if (shineIdleEffectPrefab != null && !string.IsNullOrEmpty(shineIdleEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)shineIdleEffectPrefab.GetInstance();
			gameObject.transform.parent = shineIdleTransform;
			gameObject.transform.localPosition = Vector3.zero;
		}
	}

	private void RefreshInvitePopup()
	{
		if (invitedFriendsCountLabel != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(LocalisationManager.LocalizedStringOrSource("ui.popup.menu.invite.label.bottom"));
			stringBuilder.Replace("%tag_number%", string.Format("{0}/{1}", base.CachedManagers.CachedInviteManager.GemsInvitesCount.ToString(), base.CachedManagers.CachedGiftManager.GiftInviteGems.invitesNeededCount));
			invitedFriendsCountLabel.text = stringBuilder.ToString();
		}
	}

	private void SendButton_OnClick()
	{
		AudioManager.PlaySingleAudioWithKey("button");
		if (!isSubscriptionPopup)
		{
			base.CachedManagers.CachedInviteManager.InviteForGems(delegate
			{
				RefreshInvitePopup();
			});
		}
		else
		{
			base.CachedManagers.CachedSubscribeManager.SubscribeForGem();
			Hide();
		}
	}

	private void GiftManager_OnGiftGemsOpen()
	{
		Hide();
	}

	private void SubscribeManager_OnSubscribeForGems()
	{
		Hide();
	}
}
