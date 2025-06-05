using System;
using GooglePlayGames;
using UnityEngine;

public class InvitationPopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem yesButton;

	[SerializeField]
	private tk2dUIItem noButton;

	protected override void OnEnable()
	{
		yesButton.OnClick += YesButton_OnClick;
		noButton.OnClick += NoButton_OnClick;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		yesButton.OnClick -= YesButton_OnClick;
		noButton.OnClick -= NoButton_OnClick;
		base.OnDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
		GameObject.Find("ChallengerLabel").GetComponent<tk2dTextMesh>().text = string.Format(SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey("ui.popup.invitation.description"), SingletonMonoBehaviour<GameCenterManager>.Instance.CurrentInvitation.Inviter.DisplayName);
	}

	private void YesButton_OnClick()
	{
		SingletonMonoBehaviour<GameCenterManager>.Instance.needsToShowInvitation = false;
		LLMultiplayerManager.Instance.PlayerAcceptedInvitation(string.Empty);
		Hide();
	}

	private void NoButton_OnClick()
	{
		SingletonMonoBehaviour<GameCenterManager>.Instance.needsToShowInvitation = false;
		PlayGamesPlatform.Instance.RealTime.DeclineInvitation(SingletonMonoBehaviour<GameCenterManager>.Instance.CurrentInvitation.InvitationId);
		Hide();
	}
}
