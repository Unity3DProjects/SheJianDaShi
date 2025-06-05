using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Spine;
using UnityEngine;

public class FatalityFreeze : FatalityBase
{
	[SerializeField]
	private BoneParticleSystem[] boneSystems;

	[SerializeField]
	private float freezeExplodeDelay = 1f;

	private SkeletonUtilityBone cachedBody;

	private List<GameObject> particleSystems = new List<GameObject>();

	private bool isFreezing;

	protected SkeletonUtilityBone CachedBody
	{
		get
		{
			if (cachedBody == null)
			{
				cachedBody = base.CachedManager.GetUtilityBoneBody();
			}
			return cachedBody;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (base.IsActive)
		{
			return;
		}
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
				isFreezing = true;
			}
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		if (base.IsActive && !isFreezing)
		{
			base.UpdateFatality(deltaTime);
		}
	}

	public override void StopFatality()
	{
		foreach (GameObject particleSystem in particleSystems)
		{
			particleSystem.StopFX(true);
		}
		isFreezing = false;
		base.StopFatality();
		StopAnimation(base.CachedManager.AnimationFreeze);
	}

	protected override void PlayCrowdHappySound()
	{
	}

	protected override void AnimationEnd(Spine.Animation anim)
	{
	}

	private IEnumerator CoroutineAnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		StopAnimation(base.CachedManager.AnimationFreeze);
		isFreezing = false;
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_freeze");
		base.CachedEffect.EmitFreezeEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedManager.CharacterAnimation.transform.position, base.transform.eulerAngles);
		yield return new WaitForSeconds(freezeExplodeDelay);
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_1");
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_2");
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_3");
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_4");
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_5");
		_003CPlayCrowdHappySound_003E__BaseCallProxy0();
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CPlayCrowdHappySound_003E__BaseCallProxy0()
	{
		base.PlayCrowdHappySound();
	}
}
