using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FatalityHole : FatalityBase
{
	[SerializeField]
	private float delayBeforeStart = 5f;

	[SerializeField]
	[ResourceLink]
	protected AssetLink holeEffectPrefab;

	[SerializeField]
	protected int bloodEffectMultiplier = 5;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	protected List<string> breakBonesNames = new List<string>();

	protected GameObject holeEffect;

	protected SkeletonUtilityBone CachedHead
	{
		get
		{
			if (cachedHead == null)
			{
				cachedHead = base.CachedManager.GetUtilityBoneHead();
			}
			return cachedHead;
		}
	}

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
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			StartAnimation(base.CachedManager.AnimationHole);
			StartCoroutine(CoroutineAnimationFinished(base.CachedManager.AnimationHole.duration - 0.2f, delayBeforeStart - base.CachedManager.AnimationHole.duration - 0.2f));
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		if (holeEffect != null)
		{
			Object.Destroy(holeEffect);
			holeEffect = null;
		}
	}

	protected override void PlayCrowdHappySound()
	{
	}

	protected virtual IEnumerator CoroutineAnimationFinished(float delayAnimation, float delayFatalityEffect)
	{
		if (holeEffectPrefab != null && !string.IsNullOrEmpty(holeEffectPrefab.assetGUID))
		{
			holeEffect = (GameObject)holeEffectPrefab.GetInstance();
			holeEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
			holeEffect.transform.position = base.CachedManager.CurrentCharacter.HeadColliderBody.transform.position;
		}
		AudioManager.PlayAudioWithKey("turtle_shell_activation");
		AudioManager.PlayAudioWithKey("magic_arrow_activation_1");
		AudioManager.PlayAudioWithKey("magic_arrow_fatality_2");
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			AudioManager.StopAudioWithKey("magic_arrow_fatality_2");
		}, delayFatalityEffect);
		if (base.CachedManager.CurrentCharacter.IsFlipped && holeEffect != null)
		{
			ParticleSystem[] componentsInChildren = holeEffect.GetComponentsInChildren<ParticleSystem>();
			for (int num = 0; num < componentsInChildren.Length; num++)
			{
				Vector3 localScale = componentsInChildren[num].transform.localScale;
				localScale.x *= -1f;
				componentsInChildren[num].transform.localScale = localScale;
			}
		}
		yield return new WaitForSeconds(delayAnimation);
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
		yield return new WaitForSeconds(1f);
		AudioManager.PlayAudioWithKey("magic_arrow_fatality_1");
		yield return new WaitForSeconds(delayFatalityEffect - 1f);
		StartCoroutine(CrashCharacter());
	}

	protected virtual IEnumerator CrashCharacter()
	{
		AudioManager.PlayAudioWithKey("turtle_shell_activation");
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_4", delegate(SoundObject obj)
		{
			if (obj != null)
			{
				obj.Volume = 0.5f;
			}
		});
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, CachedHead.transform.eulerAngles);
		base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
		for (int num = 0; num < bloodEffectMultiplier; num++)
		{
			base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
		}
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(1f);
		yield return new WaitForSeconds(0.1f);
		_003CPlayCrowdHappySound_003E__BaseCallProxy0();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CPlayCrowdHappySound_003E__BaseCallProxy0()
	{
		base.PlayCrowdHappySound();
	}
}
