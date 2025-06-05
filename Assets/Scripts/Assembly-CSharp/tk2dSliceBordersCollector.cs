using System;
using UnityEngine;

public class tk2dSliceBordersCollector : ScriptableSingleton<tk2dSliceBordersCollector>
{
	[Serializable]
	public class NameToBorderPair : SerializablePair<string, tk2dSliceBorders>
	{
	}

	[Serializable]
	public class NameToBorderDictionary : SerializableDictionary<NameToBorderPair, string, tk2dSliceBorders>
	{
	}

	[SerializeField]
	public NameToBorderDictionary borders;

	public tk2dSliceBorders GetBordersForID(string strID)
	{
		tk2dSliceBorders outValue;
		if (borders.TryGetValue(strID, out outValue))
		{
			return outValue;
		}
		return new tk2dSliceBorders();
	}
}
