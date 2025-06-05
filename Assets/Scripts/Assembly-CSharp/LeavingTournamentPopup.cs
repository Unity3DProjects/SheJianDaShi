using UnityEngine;

public class LeavingTournamentPopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem yesButton;

	[SerializeField]
	private tk2dUIItem noButton;

	public bool IsLeave { get; private set; }

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

	private void YesButton_OnClick()
	{
		IsLeave = true;
		Hide();
	}

	private void NoButton_OnClick()
	{
		IsLeave = false;
		Hide();
	}
}
