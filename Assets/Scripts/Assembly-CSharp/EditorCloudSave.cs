using UnityEngine;

public class EditorCloudSave : CloudSaveImplementation
{
	public void LoadFromCloud()
	{
		Log("LoadFromCloud");
	}

	public void SaveToCloud()
	{
		Log("SaveToCloud");
	}

	public void PerformLoadFromCloud()
	{
		Log("PerformLoadFromCloud");
	}

	public void PerformSaveToCloud()
	{
		Log("PerformSaveToCloud");
	}

	private void Log(string message)
	{
		Debug.Log("[EditorCloudSave] " + message);
	}
}
