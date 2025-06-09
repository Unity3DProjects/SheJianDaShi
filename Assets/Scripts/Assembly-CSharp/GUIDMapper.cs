using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class GUIDMapper
{
	private const int GUID_LENGTH = 32;

	private static Dictionary<string, string> guidMap = new Dictionary<string, string>();

	private static bool isParse;

	public static string GUIDToAssetPath(string guid)
	{
		string value = string.Empty;
		if (!isParse)
		{
			Parse();
		}
		if (!guidMap.TryGetValue(guid, out value))
		{
			CustomDebug.Log("GUIDMapper.Log can't find path for Guid : " + guid);
		}
		Debug.LogError(value);
		return value;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void RuntimeInitialize()
	{
		if (!isParse)
		{
			Parse();
		}
	}

	private static void Parse()
	{
		if (isParse)
		{
			return;
		}
		Debug.LogError("llllllllllllllmmmmmmmmmmmmmm");
		string text = Application.streamingAssetsPath + "/guids";
		string text2 = string.Empty;
		bool flag = false;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (text.Contains("file://"))
		{
			using (WWW wWW = new WWW(text))
			{
				while (!wWW.isDone)
				{
					Thread.Sleep(1);
				}
				flag = string.IsNullOrEmpty(wWW.error);
				if (flag)
				{
					text2 = wWW.text;
				}
			}
		}
		else
		{
			flag = File.Exists(text);
			if (flag)
			{
				text2 = File.ReadAllText(text);
				// Debug.LogError($"文本内容{text2}");
			}
		}
		if (flag)
		{
			// string[] array = text2.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			string[] array = File.ReadAllLines(text);
			
			int i = 0;
            Debug.LogError($"切割guids到数组后数组的长度：{array.Length}");
			for (int num = array.Length; i < num; i++)
			{
				string text3 = array[i];
				string key = text3.Substring(0, 32);
				string value = text3.Substring(32, text3.Length - 32);
				if (key == "7f023214c59d14bf4be59f0b7b2f548a")
				{
					Debug.LogError($"值为{value}。。。{text3}");
				}
				guidMap.Add(key, value);
			}
			realtimeSinceStartup = (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f;
			CustomDebug.Log("GUIDMapper.Parse by : " + realtimeSinceStartup.ToString("f10") + " miliseconds for " + guidMap.Keys.Count);
		}
		else
		{
			CustomDebug.LogError("GUIDMapper.Error can't find file : " + text);
		}
		isParse = true;
	}
}
