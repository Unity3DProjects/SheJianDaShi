using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVReader
{
	private static string COMMENT_KEY = "[@comment]";

	public static void DebugOutputGrid(string[,] grid)
	{
		for (int i = 0; i < grid.GetUpperBound(1); i++)
		{
			if (!string.IsNullOrEmpty(grid[0, i]))
			{
				string text = string.Empty;
				for (int j = 0; j < grid.GetUpperBound(0); j++)
				{
					text = text + grid[j, i] + ", ";
				}
				CustomDebug.Log(text);
			}
		}
	}

	public static string[,] SplitCsvGrid(string csvText)
	{
		string[] array = SplitCsvGridByRows(csvText);
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = SplitCsvLine(array[i]);
			num = Mathf.Max(num, array2.Length);
		}
		string[,] array3 = new string[num + 1, array.Length + 1];
		for (int j = 0; j < array.Length; j++)
		{
			string[] array4 = SplitCsvLine(array[j]);
			for (int k = 0; k < array4.Length; k++)
			{
				array3[k, j] = array4[k];
			}
		}
		return array3;
	}

	public static string[] SplitCsvGridByRows(string csvText)
	{
		string[] array = csvText.Split('\n');
		List<string> list = new List<string>();
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (!text.Contains(COMMENT_KEY))
			{
				list.Add(text);
			}
		}
		return list.ToArray();
	}

	public static string GetJsonStringWithCsvFormat(string json)
	{
		return json.Replace("\"", "\"\"").Replace(",", "\",\"");
	}

	public static string[] SplitCsvLine(string line)
	{
		line = line.Replace("\"\"", "\"");
		List<string> list = new List<string>();
		int startIndex = 0;
		int num = 0;
		for (int i = 0; i < line.Length; i++)
		{
			num++;
			if ((line[i] == ',' && line[i - 1] != '"') || i == line.Length - 1)
			{
				list.Add(line.Substring(startIndex, (i != line.Length - 1) ? (num - 1) : num).Replace("\",\"", ","));
				startIndex = i + 1;
				num = 0;
			}
		}
		return list.ToArray();
	}

	public static void SaveCSV(Dictionary<string, string> keysData, string path = "Assets/Resources/Texts/Localisation/KeysData.csv")
	{
		FileStream stream = new FileStream(path, FileMode.Create);
		StreamWriter streamWriter = new StreamWriter(stream);
		foreach (KeyValuePair<string, string> keysDatum in keysData)
		{
			streamWriter.WriteLine(keysDatum.Key + "," + keysDatum.Value);
		}
		streamWriter.Close();
	}

	public static void SaveLineToCSV(string key, string value, string fileName)
	{
		FileStream stream = new FileStream("Assets/Resources/Texts/" + fileName + ".csv", FileMode.Append);
		StreamWriter streamWriter = new StreamWriter(stream);
		streamWriter.WriteLine(key + "," + value);
		streamWriter.Close();
	}
}
