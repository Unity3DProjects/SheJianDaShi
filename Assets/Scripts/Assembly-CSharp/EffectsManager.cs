using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : SingletonMonoBehaviour<EffectsManager>
{
	[Serializable]
	private struct EffectSettings
	{
		public EffectType type;

		[ResourceLink]
		public AssetLink prefabLink;
	}

	[SerializeField]
	private List<EffectSettings> availableEffects;

	private Dictionary<EffectType, AssetLink> effectsPrefabs = new Dictionary<EffectType, AssetLink>();

	public GameObject EffectPrefabByType(EffectType effectType)
	{
		AssetLink value = null;
		if (!effectsPrefabs.TryGetValue(effectType, out value))
		{
			for (int i = 0; i < availableEffects.Count; i++)
			{
				if (availableEffects[i].type == effectType)
				{
					value = availableEffects[i].prefabLink;
					effectsPrefabs.Add(effectType, value);
					break;
				}
			}
		}
		if (value == null || string.IsNullOrEmpty(value.assetGUID))
		{
			CustomDebug.LogError("Effect link for type " + effectType.ToString() + " is not exist");
			GameObject gameObject = new GameObject();
			gameObject.name = "FakeEffect";
			return gameObject;
		}
		return (GameObject)value.GetAsset();
	}
}
