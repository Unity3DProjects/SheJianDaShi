using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
	[SerializeField]
	private EffectType effectType;

	[SerializeField]
	[ResourceLink]
	protected AssetLink effect;

	[SerializeField]
	private Vector3 localPosition;

	private void Awake()
	{
		if (effectType != EffectType.None)
		{
			InstantiatePrefab(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(effectType), base.transform, localPosition);
		}
		else if (effect != null && !string.IsNullOrEmpty(effect.assetGUID))
		{
			InstantiatePrefab((GameObject)effect.GetAsset(), base.transform, localPosition);
		}
	}

	private GameObject InstantiatePrefab(GameObject prefab, Transform parent, Vector3 localPosition)
	{
		GameObject gameObject = null;
		if (prefab != null)
		{
			gameObject = Object.Instantiate(prefab);
			AddObject(gameObject, parent, localPosition);
			if (gameObject != null)
			{
				gameObject.transform.localScale = prefab.transform.localScale;
			}
		}
		return gameObject;
	}

	private void AddObject(GameObject obj, Transform parent, Vector3 localPosition)
	{
		if (obj != null)
		{
			obj.transform.parent = parent;
			obj.transform.localPosition = localPosition;
			obj.transform.localRotation = Quaternion.identity;
		}
	}
}
