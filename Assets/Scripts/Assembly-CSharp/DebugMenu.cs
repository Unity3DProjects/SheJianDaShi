using UnityEngine;

public class DebugMenu : SingletonMonoBehaviour<DebugMenu>
{
	private bool isVisible;

	private const int cellSize = 80;

	public bool IsWaveBlocked { get; set; }

	public bool IsGiftBlocked { get; set; }

	private void OnGUI()
	{
		if (GameSettings.NeedShowDebug)
		{
			if (GUI.Button(new Rect(0f, 0f, 80f, 80f), (!isVisible) ? "Show" : "Hide"))
			{
				isVisible = !isVisible;
			}
			if (isVisible)
			{
				OtherButtons();
			}
		}
	}

	private void OtherButtons()
	{
		int num = 1;
		if (GUI.Button(new Rect(160f, 80 * num++, 80f, 80f), "RESET\nPREFS"))
		{
			PlayerPrefs.DeleteAll();
		}
		if (GUI.Button(new Rect(160f, 80 * num++, 80f, 80f), "destroy\n debug"))
		{
			Object.DestroyImmediate(base.gameObject);
		}
	}
}
