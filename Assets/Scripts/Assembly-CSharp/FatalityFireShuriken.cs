using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FatalityFireShuriken : FatalityHole
{
	[SerializeField]
	private GameObject[] partsOfBody;

	[SerializeField]
	[ResourceLink]
	private AssetLink bloodEffectPrefab;

	[SerializeField]
	private Vector3 portalOffset;

	[SerializeField]
	private float timeBetweenBodiesPartsAppear = 0.1f;

	[SerializeField]
	private Quaternion bloodDirectionAngleUp = Quaternion.Euler(70f, 0f, 90f);

	[SerializeField]
	private Quaternion bloodDirectionAngleDown = Quaternion.Euler(0f, 0f, -90f);

	private GameObject secondPortal;

	protected override IEnumerator CrashCharacter()
	{
		Vector3 portalPlace = ((!(holeEffect != null)) ? Vector3.zero : (holeEffect.transform.position + portalOffset));
		if (holeEffectPrefab != null && !string.IsNullOrEmpty(holeEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)holeEffectPrefab.GetInstance();
			gameObject.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
			gameObject.transform.position = portalPlace;
		}
		AudioManager.PlayAudioWithKey("fire_shuriken_fatality_explode");
		StartCoroutine(RainingBlood(portalPlace));
		GameObject[] array = partsOfBody;
		foreach (GameObject go in array)
		{
			if (go != null)
			{
				Object.Instantiate(go, portalPlace, Quaternion.identity, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
				yield return new WaitForSeconds(timeBetweenBodiesPartsAppear);
			}
		}
		StartCoroutine(RainingBlood(portalPlace));
		AudioManager.StopAudioWithKey("fire_shuriken_fatality_explode");
		_003CPlayCrowdHappySound_003E__BaseCallProxy0();
	}

	private IEnumerator RainingBlood(Vector3 portalPlace)
	{
		if (bloodEffectPrefab == null || string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
		{
			yield break;
		}
		for (int i = 0; i < bloodEffectMultiplier; i++)
		{
			if (i % 2 == 1)
			{
				Object.Instantiate((GameObject)bloodEffectPrefab.GetAsset(), portalPlace, bloodDirectionAngleUp, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
			}
			else
			{
				Object.Instantiate((GameObject)bloodEffectPrefab.GetAsset(), portalPlace, bloodDirectionAngleDown, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
			}
			yield return new WaitForSeconds(timeBetweenBodiesPartsAppear);
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CPlayCrowdHappySound_003E__BaseCallProxy0()
	{
		PlayCrowdHappySound();
	}
}
