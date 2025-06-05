using UnityEngine;

public class MiniclipPrefabs : MonoBehaviour
{
	private static MiniclipPrefabs _instance;

	public GameObject spriteCollection;

	public GameObject settingsButtons;

	public GameObject miniclipManagers;

	public GameObject newsFeedButton;

	public GameObject newRobinHood;

	public GameObject userIDPrefab;

	public GUIPopup[] miniclipPopupPrefabs;

	public static MiniclipPrefabs Instance
	{
		get
		{
			if (!_instance)
			{
				GameObject gameObject = Resources.Load("Miniclip/Prefabs/MiniclipHelper") as GameObject;
				Object.DontDestroyOnLoad(gameObject);
				_instance = gameObject.GetComponent<MiniclipPrefabs>();
			}
			return _instance;
		}
	}
}
