using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LanguageAsset
{
	public List<SystemLanguage> languages;

	public TextAsset fontsAsset;

	public TextAsset textAsset;

	public string languageName;
}
