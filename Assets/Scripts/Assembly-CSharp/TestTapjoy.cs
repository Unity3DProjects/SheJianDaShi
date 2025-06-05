using UnityEngine;

public class TestTapjoy : MonoBehaviour
{
	private void OnGUI()
	{
		float num = 0.1f;
		float num2 = 0.05f;
		float num3 = 0.08f;
		float num4 = 0.8f;
		float num5 = 0.06f;
		GUI.skin.button.fontSize = (int)(0.035f * (float)Screen.height);
		Rect position = new Rect(num * (float)Screen.width, (num2 + num3 * 3f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		GUI.Button(position, "KEY = " + LLTapjoySettings.AppKey());
		Rect position2 = new Rect(num * (float)Screen.width, (num2 + num3 * 5f) * (float)Screen.height, num4 * (float)Screen.width, num5 * (float)Screen.height);
		if (GUI.Button(position2, "Initialize "))
		{
			LLTapjoyManager.Initialize();
		}
	}
}
