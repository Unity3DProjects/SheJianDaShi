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
			// Parse();
			ParseForResource();
		}
		if (!guidMap.TryGetValue(guid, out value))
		{
			CustomDebug.Log("GUIDMapper.Log can't find path for Guid : " + guid);
		}
		// Debug.LogError(value);
		return value;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void RuntimeInitialize()
	{
		if (!isParse)
		{
			// Parse();
			ParseForResource();
		}
	}

	private static void Parse()
	{
		if (isParse)
		{
			return;
		}
		// Debug.LogError("llllllllllllllmmmmmmmmmmmmmm");
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
            // Debug.LogError($"切割guids到数组后数组的长度：{array.Length}");
			for (int num = array.Length; i < num; i++)
			{
				string text3 = array[i];
				string key = text3.Substring(0, 32);
				string value = text3.Substring(32, text3.Length - 32);
				// if (key == "7f023214c59d14bf4be59f0b7b2f548a")
				// {
					// Debug.LogError($"值为{value}。。。{text3}");
				// }
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
	private static void ParseForResource()
	{
		if (isParse) return;

		// WebGL 平台使用 Resources 加载
		TextAsset guidsAsset = Resources.Load<TextAsset>("guids");
    
		if (guidsAsset == null)
		{
			CustomDebug.LogError("GUIDMapper.Error: Failed to load guids resource");
			isParse = true;
			return;
		}

		string text2 = guidsAsset.text;
		float realtimeSinceStartup = Time.realtimeSinceStartup;

		// 使用跨平台的行分割方法
		string[] array = text2.Split(
			new[] { "\r\n", "\r", "\n" }, 
			StringSplitOptions.RemoveEmptyEntries
		);

		int count = 0;
		for (int i = 0; i < array.Length; i++)
		{
			string text3 = array[i];
			// 添加长度安全检查
			if (text3.Length < 33)
			{
				CustomDebug.LogWarning($"Invalid line in guids: {text3}");
				continue;
			}

			try
			{
				string key = text3.Substring(0, 32);
				string value = text3.Substring(32);
				guidMap.Add(key, value);
				count++;
			}
			catch (Exception e)
			{
				CustomDebug.LogError($"GUIDMapper parse error on line {i}: {e.Message}");
			}
		}

		realtimeSinceStartup = (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f;
		CustomDebug.Log($"GUIDMapper.Parse: {realtimeSinceStartup:F6} ms, 已加载 {count} 个对象");
		isParse = true;
    
		// WebGL 内存优化：显式卸载资源
		Resources.UnloadAsset(guidsAsset);
	}
}
