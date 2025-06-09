using System;
using UnityEngine;

public class SubscribePopup : GUIPopup
{
	private const string INSTAGRAM = "instagram";

	private const string FACEBOOK = "facebook";

	private const string TWITTER = "twitter";

	private const string YOUTUBE = "youtube";

	private const string VK = "vk";

	[SerializeField]
	private tk2dUIItem sendButton;

	[SerializeField]
	private tk2dSprite characterSprite;

	[SerializeField]
	private tk2dSprite socialNetworkIconSprite;

	[SerializeField]
	private tk2dTextMesh nameLabel;

	[SerializeField]
	private tk2dTextMesh sendButtonLabel;

	[SerializeField]
	private Transform characterEffectTransform;

	[SerializeField]
	private SystemLanguage[] languagesWithoutLikeIcon;

	[SerializeField]
	private GameObject likeIcon;

	private GameObject characterEffect;

	private SubscribeType type;

	protected override void OnEnable()
	{
		base.OnEnable();
		sendButton.OnClick += SendButton_OnClick;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		sendButton.OnClick -= SendButton_OnClick;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		RefreshSubscribePopup();
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

	private void RefreshSubscribePopup()
	{
		if (!(characterSprite != null))
		{
			return;
		}
		GiftSubscribeCharacter currentSubscribeCharacter = base.CachedManagers.CachedSubscribeManager.CurrentSubscribeCharacter;
		if (currentSubscribeCharacter.subscribeCharacter.IsNull() || currentSubscribeCharacter.subscribeCharacterSpriteName.IsNull())
		{
			return;
		}
		likeIcon.SetActive(true);
		for (int i = 0; i < languagesWithoutLikeIcon.Length; i++)
		{
			if (languagesWithoutLikeIcon[i] == SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage)
			{
				likeIcon.SetActive(false);
				break;
			}
		}
		characterSprite.SetSprite(currentSubscribeCharacter.subscribeCharacterSpriteName);
		int num = currentSubscribeCharacter.subscribeLinks.Length;
		switch (currentSubscribeCharacter.subscribeLinks[DateTime.Now.DayOfYear % num].type)
		{
		case SubscribeType.Facebook:
			// socialNetworkIconSprite.SetSprite("facebook");
			socialNetworkIconSprite.SetSprite("youtube");
			break;
		case SubscribeType.Instagram:
			// socialNetworkIconSprite.SetSprite("instagram");
			socialNetworkIconSprite.SetSprite("youtube");
			break;
		case SubscribeType.Twitter:
			// socialNetworkIconSprite.SetSprite("twitter");
			socialNetworkIconSprite.SetSprite("youtube");
			break;
		}
	}

	private void SendButton_OnClick()
	{
		Hide();
		base.CachedManagers.CachedSubscribeManager.GetSubscribe();
		RefreshSubscribePopup();
	}
}
