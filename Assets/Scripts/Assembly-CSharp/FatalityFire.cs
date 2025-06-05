using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityFire : FatalityBase
{
	[SerializeField]
	private BoneParticleSystem[] boneSystems;

	[SerializeField]
	private FatalityMask masks;

	[SerializeField]
	private FatalityMask decal;

	private List<GameObject> particleSystems = new List<GameObject>();

	private GameObject currentAniamtionEffect;

	private GameObject currentEffect;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (base.IsActive)
		{
			return;
		}
		base.StartFatality(infoCollision);
		StartAnimation(base.CachedManager.AnimationFire);
		StartCoroutine(AnimationFinished(base.CachedManager.AnimationFire.Duration - 0.2f));
		decal.Activate(startFatalityCollider, startFatalityUtilityBone);
		BoneParticleSystem[] array = boneSystems;
		for (int i = 0; i < array.Length; i++)
		{
			BoneParticleSystem boneParticleSystem = array[i];
			if (boneParticleSystem.particleEffectPrefab != null && !string.IsNullOrEmpty(boneParticleSystem.particleEffectPrefab.assetGUID))
			{
				SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(boneParticleSystem.boneName);
				GameObject gameObject = (GameObject)boneParticleSystem.particleEffectPrefab.GetInstance();
				gameObject.transform.parent = utilityBone.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.PlayFX();
				particleSystems.Add(gameObject);
			}
		}
	}

	public override void StopFatality()
	{
		foreach (GameObject particleSystem in particleSystems)
		{
			particleSystem.StopFX(true);
		}
		base.StopFatality();
		StopAnimation(base.CachedManager.AnimationFire);
	}

	protected IEnumerator AnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.CachedEffect.EmitFireEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedManager.CharacterAnimation.transform.position, base.transform.eulerAngles);
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
	}

	protected override void AnimationEnd(Spine.Animation anim)
	{
		masks.Activate(startFatalityCollider, startFatalityUtilityBone);
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_fire_start");
		AudioManager.PlayAudioWithKey("fatality_fire_main");
		if (base.CachedManager.CurrentCharacter.CharacterVoiceType == CharacterVoiceType.ManBeefy)
		{
			AudioManager.PlayAudioWithKey("fatality_fire_beefy_man");
		}
		PlayCrowdHappySound();
	}
}
