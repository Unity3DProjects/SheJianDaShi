using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LocalisationManager : SingletonMonoBehaviour<LocalisationManager>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnLanguageChanged;

	[SerializeField]
	private TextAsset keysFile;

	[SerializeField]
	private LanguageAsset defaultLanguageAsset;

	[SerializeField]
	private List<LanguageAsset> otherLanguagesAssets;

	[SerializeField]
	private List<tk2dFontData> fontsData;

	private Dictionary<string, string> internal_allTexts;

	private LanguageAsset currentLanguageAsset;

	private List<tk2dFontData> defaultLanguageFonts = new List<tk2dFontData>();

	private List<tk2dFontData> currentLanguageFonts = new List<tk2dFontData>();

	public string CurrentLanguageAssetLanguageName
	{
		get
		{
			return currentLanguageAsset.languageName;
		}
	}

	public List<SystemLanguage> CurrentLanguageAssetLanguages
	{
		get
		{
			return currentLanguageAsset.languages;
		}
	}

	public List<tk2dFontData> DefaultLanguageFonts
	{
		get
		{
			if (defaultLanguageFonts.Count == 0 && defaultLanguageAsset.languages != null && defaultLanguageAsset.textAsset != null && defaultLanguageAsset.fontsAsset != null)
			{
				string[] array = CSVReader.SplitCsvGridByRows(defaultLanguageAsset.fontsAsset.text);
				for (int i = 0; i < array.Length; i++)
				{
					for (int j = 0; j < fontsData.Count; j++)
					{
						if (fontsData[j].name == string.Format("{0} data", array[i]))
						{
							defaultLanguageFonts.Add(fontsData[j]);
							break;
						}
					}
				}
			}
			return defaultLanguageFonts;
		}
	}

	public List<tk2dFontData> CurrentLanguageFonts
	{
		get
		{
			return currentLanguageFonts;
		}
		private set
		{
			currentLanguageFonts = value;
			int num = DefaultLanguageFonts.Count - currentLanguageFonts.Count;
			if (num > 0)
			{
				tk2dFontData item = currentLanguageFonts[currentLanguageFonts.Count - 1];
				for (int i = 0; i < num; i++)
				{
					currentLanguageFonts.Add(item);
				}
			}
		}
	}

	public List<tk2dFontData> FontsData
	{
		get
		{
			return fontsData;
		}
	}

	private TextAsset KeysFile
	{
		get
		{
			return keysFile;
		}
		set
		{
			if (keysFile != value)
			{
				keysFile = value;
				internal_allTexts = null;
			}
		}
	}

	private Dictionary<string, string> AllTexts
	{
		get
		{
			if (internal_allTexts == null)
			{
				internal_allTexts = new Dictionary<string, string>();
				string[,] array = CSVReader.SplitCsvGrid(keysFile.text);
				// Debug.LogError($"翻译文本长度 {array.Length}");
				for (int i = 0; i < array.GetUpperBound(1); i++)
				{
					if (string.IsNullOrEmpty(array[0, i]))
					{
						continue;
					}
					if (internal_allTexts.ContainsKey(array[0, i]))
					{
						CustomDebug.LogError("KEY ALLREADY EXISTS = " + array[0, i]);
						continue;
					}
					string text = array[1, i];
					if (!string.IsNullOrEmpty(text))
					{
						text = text.Replace("%tag_line%", "\n");
						text = text.Replace("%tag_comma%", ",");
						internal_allTexts.Add(array[0, i], text);
					}
					else
					{
						CustomDebug.LogWarning("Null ref in key : " + array[0, i]);
					}
				}
			}
			return internal_allTexts;
		}
	}

	public static event Action OnLanguageChanged
	{
		add
		{
			Action action = LocalisationManager.m_OnLanguageChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LocalisationManager.m_OnLanguageChanged, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LocalisationManager.m_OnLanguageChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LocalisationManager.m_OnLanguageChanged, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		currentLanguageAsset = defaultLanguageAsset;
		currentLanguageFonts = DefaultLanguageFonts;
		base.Awake();
	}

	public string GetTextByKey(string key)
	{
		string value = null;
		if (!AllTexts.TryGetValue(key, out value))
		{
			value = key;
		}
		return value;
	}

	public static string LocalizedStringOrSource(string source)
	{
		string text = source;
		if ((bool)SingletonMonoBehaviour<LocalisationManager>.InstanceIfExist)
		{
			string textByKey = SingletonMonoBehaviour<LocalisationManager>.Instance.GetTextByKey(text);
			if (textByKey != null)
			{
				text = textByKey;
			}
		}
		return text;
	}

	public void ChangeLanguage(SystemLanguage currentLanguage)
	{
		LanguageAsset languageAsset = defaultLanguageAsset;
		if (!defaultLanguageAsset.languages.Contains(currentLanguage))
		{
			for (int i = 0; i < otherLanguagesAssets.Count; i++)
			{
				if (otherLanguagesAssets[i].languages.Contains(currentLanguage))
				{
					languageAsset = otherLanguagesAssets[i];
					break;
				}
			}
		}
		if (currentLanguageAsset.languages == languageAsset.languages)
		{
			return;
		}
		currentLanguageAsset = languageAsset;
		KeysFile = languageAsset.textAsset;
		if (languageAsset.fontsAsset != null)
		{
			string[] array = CSVReader.SplitCsvGridByRows(languageAsset.fontsAsset.text);
			List<tk2dFontData> list = new List<tk2dFontData>();
			for (int j = 0; j < array.Length; j++)
			{
				for (int k = 0; k < fontsData.Count; k++)
				{
					if (fontsData[k].name == string.Format("{0} data", array[j]))
					{
						list.Add(fontsData[k]);
						break;
					}
				}
			}
			CurrentLanguageFonts = new List<tk2dFontData>(list);
		}
		LocalisationManager.m_OnLanguageChanged?.Invoke();
	}
}
