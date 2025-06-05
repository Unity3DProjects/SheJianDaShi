using UnityEngine;

public class GameCenterLoginPopup : GUIPopup
{
	private const string popupTitleText = "ui.popup.settings.gamecenter.header";

	private const string popupBodyText = "ui.popup.settings.gamecenter.progressunsuccessful";

	[SerializeField]
	private tk2dUIItem loginButton;

	public void Start()
	{
		tk2dTextMesh[] componentsInChildren = GetComponentsInChildren<tk2dTextMesh>();
		tk2dTextMesh[] array = componentsInChildren;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			if (tk2dTextMesh2.text == "ui.popup.settings.gamecenter.header")
			{
				tk2dTextMesh2.text = "ui.popup.settings.googleplaylogin.header";
			}
			else if (tk2dTextMesh2.text == "ui.popup.settings.gamecenter.progressunsuccessful")
			{
				tk2dTextMesh2.text = "ui.popup.settings.googleplaylogin.progressunsuccessful";
			}
		}
	}

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
		LLICloudManager.OpenGameCenter();
		Hide();
	}
}
