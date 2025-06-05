using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSettings : SingletonMonoBehaviour<GameSettings>
{
	private const string FILE_NAME = "GameSettings";

	private const string IS_ALL_WEAPONS_UNLOCKED_KEY = "unlockWeapons";

	private const string NEED_SKIP_INTRO_SCENE_KEY = "skipIntro";

	private const string NEED_SHOW_DEBUG_KEY = "needshowdebug";

	private static bool isInitialized;

	[Button]
	public string CreateFileWithSettingsBtn = "CreateFileWithSettings";

	public static bool IsAllWeaponsUnlocked { get; private set; }

	public static bool NeedSkipIntroScene { get; private set; }

	public static bool NeedShowDebug { get; private set; }

	private string SettingsTableName
	{
		get
		{
			return PathUtils.GetStreamingAssetsCSV("GameSettings");
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (!isInitialized)
		{
			isInitialized = true;
			StartCoroutine(TryToLoadFileFromStreamingAssets());
		}
	}

	public static string CurrentDeviceType()
	{
		return "iPhone";
	}

	public static float CurrentDeviceVolume()
	{
		return 1f;
	}

	private void TryToReadFileFromStreamingAssets()
	{
		if (!File.Exists(SettingsTableName))
		{
			return;
		}
		using (FileStream stream = new FileStream(SettingsTableName, FileMode.Open))
		{
			using (StreamReader streamReader = new StreamReader(stream))
			{
				ReadText(streamReader.ReadToEnd());
			}
		}
	}

	private IEnumerator TryToLoadFileFromStreamingAssets()
	{
		WWW www = new WWW("file://" + SettingsTableName);
		yield return www;
		if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
		{
			ReadText(www.text);
		}
	}

	private void ReadText(string text)
	{
		string[,] array = CSVReader.SplitCsvGrid(text);
		for (int i = 0; i < array.GetUpperBound(1); i++)
		{
			string text2 = array[0, i];
			string text3 = array[1, i];
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				switch (text2)
				{
				case "unlockWeapons":
					IsAllWeaponsUnlocked = int.Parse(text3) > 0;
					break;
				case "skipIntro":
					NeedSkipIntroScene = int.Parse(text3) > 0;
					break;
				case "needshowdebug":
					NeedShowDebug = int.Parse(text3) > 0;
					break;
				}
			}
		}
	}

	private void CreateFileWithSettings()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("unlockWeapons", "0");
		dictionary.Add("skipIntro", "0");
		dictionary.Add("needshowdebug", "0");
		CSVReader.SaveCSV(dictionary, SettingsTableName);
		CustomDebug.Log(string.Format("File created at path {0}", SettingsTableName));
	}
}
