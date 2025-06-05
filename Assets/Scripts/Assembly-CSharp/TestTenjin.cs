using UnityEngine;

public class TestTenjin : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 0.1f;
		float num2 = 0.05f;
		float num3 = 0.08f;
		float num4 = 0.8f;
		float num5 = 0.06f;
		GUI.skin.button.fontSize = (int)(0.035f * (float)Screen.height);
		Rect position = new Rect(num * (float)Screen.width, (num2 + num3 * 2f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position, "API KEY = " + LLTenjinSettings.ApiKey());
		Rect position2 = new Rect(num * (float)Screen.width, (num2 + num3 * 3f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position2, "Initialize "))
		{
			LLTenjinManager.Initialize();
		}
		Rect position3 = new Rect(num * (float)Screen.width, (num2 + num3 * 7f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position3, "Send Event = TEST"))
		{
			LLTenjinManager.LogEvent("TEST", "054");
		}
		Rect position4 = new Rect(num * (float)Screen.width, (num2 + num3 * 8f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position4, "Send Purchase = 0.54$"))
		{
			LLTenjinManager.LogPurchase("com.some.id", "USD", 1, "0.54", "0000000000000000");
		}
	}
}
