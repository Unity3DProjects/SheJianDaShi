using System.Collections.Generic;
using UnityEngine;

public class FatalityEffect : MonoBehaviour
{
	[SerializeField]
	private List<FatalityParticleSystemInfo> explodeEffects;

	[SerializeField]
	private List<FatalityParticleSystemInfo> bloodEffects;

	[SerializeField]
	private List<FatalityParticleSystemInfo> acidEffects;

	[SerializeField]
	private List<FatalityParticleSystemInfo> shockEffects;

	[SerializeField]
	private List<FatalityParticleSystemInfo> fireEffects;

	[SerializeField]
	private List<FatalityParticleSystemInfo> freezeEffects;

	[SerializeField]
	private List<FatalityParticleSystemInfo> brainEffects;

	[SerializeField]
	private List<FatalityParticleSystemInfo> gutsEffects;

	[SerializeField]
	private List<BrutalityEffectInfo> fatalityGUICameraEffects;

	[SerializeField]
	private Rigidbody gutsPrefab;

	[SerializeField]
	private Rigidbody brainPrefab;

	[SerializeField]
	private Rigidbody eyePrefab;

	[SerializeField]
	private float minForceGuts;

	[SerializeField]
	private float maxForceGuts;

	[SerializeField]
	private float minForceBrain;

	[SerializeField]
	private float maxForceBrain;

	[SerializeField]
	private float minForceEye;

	[SerializeField]
	private float maxForceEye;

	[SerializeField]
	private Vector3 angularVelosityParts;

	public void EmitEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		EmitExplodeEffect(characterType, parent, position, eulerAngles);
		EmitBloodEffect(characterType, parent, position, eulerAngles);
		EmitAcidEffect(characterType, parent, position, eulerAngles);
		EmitShockEffect(characterType, parent, position, eulerAngles);
		EmitFireEffect(characterType, parent, position, eulerAngles);
		EmitBrainEffect(characterType, parent, position, eulerAngles);
		EmitGutsEffect(characterType, parent, position, eulerAngles);
	}

	public void EmitExplodeEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		EmitEffect(explodeEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitBloodEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		EmitEffect(bloodEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitAcidEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		EmitEffect(acidEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitShockEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		EmitEffect(shockEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitFireEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		EmitEffect(fireEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitFreezeEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		EmitEffect(freezeEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitBrainEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		if (!GoreContentFilter.AllowInternalOrgans)
		{
			return;
		}
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(0.1f);
		EmitEffect(brainEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitGutsEffect(CharacterType characterType, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		if (!GoreContentFilter.AllowInternalOrgans)
		{
			return;
		}
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(0.1f);
		EmitEffect(gutsEffects.Find((FatalityParticleSystemInfo obj) => obj.characterType == characterType).effectPrefab, parent, position, eulerAngles);
	}

	public void EmitBrain(Transform parent, Vector3 position, Vector3 direction)
	{
		if (!GoreContentFilter.AllowInternalOrgans)
		{
			return;
		}
		EmitObject(brainPrefab, parent, position, direction * Random.Range(minForceBrain, maxForceBrain));
		EmitObject(eyePrefab, parent, position, direction * Random.Range(minForceEye, maxForceEye));
		EmitObject(eyePrefab, parent, position, direction * Random.Range(minForceEye, maxForceEye));
	}

	public void EmitGuts(Transform parent, Vector3 position, Vector3 direction)
	{
		if (!GoreContentFilter.AllowInternalOrgans)
		{
			return;
		}
		EmitObject(gutsPrefab, parent, position, direction * Random.Range(minForceGuts, maxForceGuts));
	}

	public void EmitGUICameraEffects(BrutalityEffectType type)
	{
		GameScreen gameScreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.GameScreen;
		int width = Screen.width;
		int height = Screen.height;
		for (int i = 0; i < fatalityGUICameraEffects.Count; i++)
		{
			if (type == fatalityGUICameraEffects[i].type)
			{
				GUILayouter gUILayouter = ((height >= width) ? gameScreen.VerticalLayouter : gameScreen.HorizontalLayouter);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position, Vector3.forward * 90f);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position + height * Vector3.up, Vector3.zero);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position + width * Vector3.right, Vector3.forward * 180f);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position + width * Vector3.right + height * Vector3.up, Vector3.forward * 270f);
				gUILayouter = ((height >= width) ? gameScreen.HorizontalLayouter : gameScreen.VerticalLayouter);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position, Vector3.forward * 90f);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position + width * Vector3.up, Vector3.zero);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position + height * Vector3.right, Vector3.forward * 180f);
				EmitEffect(fatalityGUICameraEffects[i].effectPrefab, gUILayouter.CachedTransform, gameScreen.transform.position + height * Vector3.right + width * Vector3.up, Vector3.forward * 270f);
			}
		}
	}

	private void EmitEffect(AssetLink effectPrefab, Transform parent, Vector3 position, Vector3 eulerAngles)
	{
		if (effectPrefab != null && !string.IsNullOrEmpty(effectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)effectPrefab.GetInstance();
			gameObject.transform.parent = parent;
			gameObject.transform.eulerAngles = eulerAngles;
			gameObject.transform.position = position;
		}
	}

	private void EmitObject(Rigidbody objectPrefab, Transform parent, Vector3 position, Vector3 velocity)
	{
		if ((bool)objectPrefab)
		{
			Rigidbody rigidbody = Object.Instantiate(objectPrefab);
			rigidbody.transform.parent = parent;
			rigidbody.transform.position = position;
			rigidbody.velocity = velocity;
			rigidbody.angularVelocity = angularVelosityParts;
		}
	}
}
