using UnityEngine;

public class TestAdmob : MonoBehaviour
{
	private bool bannerButtonShow;

	private void OnGUI()
	{
		float num = 0.1f;
		float num2 = 0.05f;
		float num3 = 0.08f;
		float num4 = 0.8f;
		float num5 = 0.06f;
		GUI.skin.button.fontSize = (int)(0.035f * (float)Screen.height);
		Rect position = new Rect(num * (float)Screen.width, (num2 + num3 * 3f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position, "KEY = " + LLAdmobSettings.AppKey());
		Rect position2 = new Rect(num * (float)Screen.width, (num2 + num3 * 4f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position2, "UNIT = " + LLAdmobSettings.UnitID());
		Rect position3 = new Rect(num * (float)Screen.width, (num2 + num3 * 5f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position3, "Initialize "))
		{
			LLAdmobManager.Initialize();
		}
		Rect position4 = new Rect(num * (float)Screen.width, (num2 + num3 * 6f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position4, "Banner Load == " + ((!LLAdmobManager.IsBannerAvailable) ? "FALSE" : "TRUE"));
		Rect position5 = new Rect(num * (float)Screen.width, (num2 + num3 * 7f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (bannerButtonShow && GUI.Button(position5, "Hide Banner"))
		{
			LLAdmobManager.HideBanner();
			bannerButtonShow = false;
		}
		Rect position6 = new Rect(num * (float)Screen.width, (num2 + num3 * 7f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (!bannerButtonShow && GUI.Button(position6, "Show Banner"))
		{
			LLAdmobManager.ShowBanner();
			bannerButtonShow = true;
		}
	}
}
