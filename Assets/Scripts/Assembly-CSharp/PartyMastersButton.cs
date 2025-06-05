using UnityEngine;

public class PartyMastersButton : MonoBehaviour
{
	[SerializeField]
	private PartyMastersButtonType buttonType;

	[SerializeField]
	private GUIControl showControl;

	[SerializeField]
	private tk2dUIItem downloadButton;

	private void OnEnable()
	{
		downloadButton.OnClick += DownloadButton_OnClick;
	}

	private void OnDisable()
	{
		downloadButton.OnClick -= DownloadButton_OnClick;
	}

	public void Show()
	{
		if (showControl != null)
		{
			showControl.Show();
		}
	}

	public void Hide()
	{
		if (showControl != null)
		{
			showControl.Hide();
		}
	}

	private void DownloadButton_OnClick()
	{
		SingletonMonoBehaviour<StatisticsManager>.Instance.IsPartyMastersButtonClicked = true;
		if (showControl != null)
		{
			showControl.Hide();
		}
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.playgendary.partymasters");
	}
}
