using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalitySubZeroIce : FatalitySatelite
{
	[SerializeField]
	private BoneParticleSystem[] boneSystems;

	private List<GameObject> particleSystems = new List<GameObject>();

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			StartCoroutine(CoroutineAnimationFinished(base.CachedManager.AnimationGrenade.Duration));
			StartAnimation(base.CachedManager.AnimationFreeze);
			AudioManager.PlayAudioWithKey("frozen_spear_fatality_start_1");
			AudioManager.PlayAudioWithKey("frozen_spear_fatality_start_2");
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				AudioManager.StopAudioWithKey("frozen_spear_fatality_start_2", 0.5f);
			}, 1f);
			BoneParticleSystem[] array = boneSystems;
			for (int num = 0; num < array.Length; num++)
			{
				BoneParticleSystem boneParticleSystem = array[num];
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
		base.StartFatality(infoCollision);
	}

	public override void StopFatality()
	{
		foreach (GameObject particleSystem in particleSystems)
		{
			particleSystem.StopFX(true);
		}
		base.StopFatality();
		StopAnimation(base.CachedManager.AnimationFreeze);
	}

	protected override void AnimationEnd(Spine.Animation anim)
	{
	}

	private IEnumerator CoroutineAnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		StopAnimation(base.CachedManager.AnimationFreeze);
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_freeze");
		base.CachedEffect.EmitFreezeEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedManager.CharacterAnimation.transform.position, base.transform.eulerAngles);
	}
}
