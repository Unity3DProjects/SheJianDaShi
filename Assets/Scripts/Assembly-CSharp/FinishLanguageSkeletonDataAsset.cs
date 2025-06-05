using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FinishLanguageSkeletonDataAsset
{
	public List<SystemLanguage> languages;

	public SkeletonDataAsset manSkeletonDataAsset;

	public SkeletonDataAsset womanSkeletonDataAsset;
}
