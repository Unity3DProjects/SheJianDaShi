using UnityEngine;

public class FatalityTNTs : FatalityBricks
{
	[SerializeField]
	[ResourceLink]
	private AssetLink explodeEffectPrefab;

	public void ExplodeAllTNTs()
	{
		int i = 0;
		for (int count = bricks.Count; i < count; i++)
		{
			if (explodeEffectPrefab != null && !string.IsNullOrEmpty(explodeEffectPrefab.assetGUID))
			{
				Object.Instantiate((GameObject)explodeEffectPrefab.GetAsset(), bricks[i].transform.position, Quaternion.identity, base.transform);
			}
			if (bricks[i] is FatalityTNT)
			{
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					AudioManager.PlayAudioWithKey("rocket_explosion");
				}, Random.value * 1f);
			}
			Object.Destroy(bricks[i].gameObject);
		}
	}
}
