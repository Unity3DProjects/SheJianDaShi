using UnityEngine;

public class TestSupersonic : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 0.1f;
		float num2 = 0.05f;
		float num3 = 0.08f;
		float num4 = 0.8f;
		float num5 = 0.06f;
		GUI.skin.button.fontSize = (int)(0.035f * (float)Screen.height);
		Rect position = new Rect(num * (float)Screen.width, (num2 + num3 * 0f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position, "KEY = " + LLSupersonicSettings.AppKey());
		Rect position2 = new Rect(num * (float)Screen.width, (num2 + num3 * 1f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position2, "Initialize "))
		{
			LLSupersonicManager.Initialize();
		}
		Rect position3 = new Rect(num * (float)Screen.width, (num2 + num3 * 2f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position3, "Interstitial Avalaible == " + ((!LLSupersonicManager.IsInterstitialAvailable) ? "FALSE" : "TRUE"));
		Rect position4 = new Rect(num * (float)Screen.width, (num2 + num3 * 3f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position4, "Show Interstitial"))
		{
			LLSupersonicManager.ShowInterstitial();
		}
		Rect position5 = new Rect(num * (float)Screen.width, (num2 + num3 * 4f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position5, "Video Avalaible == " + ((!LLSupersonicManager.IsVideoAvailable) ? "FALSE" : "TRUE"));
		Rect position6 = new Rect(num * (float)Screen.width, (num2 + num3 * 5f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position6, "Show Video"))
		{
			LLSupersonicManager.ShowVideo();
		}
		Rect position7 = new Rect(num * (float)Screen.width, (num2 + num3 * 6f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position7, "Version == " + LLSupersonicManager.GetVersion());
	}
}
