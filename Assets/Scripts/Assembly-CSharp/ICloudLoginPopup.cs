using UnityEngine;

public class ICloudLoginPopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem loginButton;

	protected override void OnEnable()
	{
		base.OnEnable();
		loginButton.OnClick += LoginButton_OnClick;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		loginButton.OnClick -= LoginButton_OnClick;
	}

	private void LoginButton_OnClick()
	{
		SingletonMonoBehaviour<ICloudManager>.Instance.IsLoginPopupActivated = true;
		LLICloudManager.OpenICloud();
		Hide();
	}
}
