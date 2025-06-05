using UnityEngine;

public class MultiplayerBuildVersionPopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem updateButton;

	protected override void OnEnable()
	{
		base.OnEnable();
		updateButton.OnClick += UpdateButton_OnClick;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		updateButton.OnClick -= UpdateButton_OnClick;
	}

	private void UpdateButton_OnClick()
	{
		SingletonMonoBehaviour<RateMeManager>.Instance.RateAppAction();
		Hide();
	}
}
