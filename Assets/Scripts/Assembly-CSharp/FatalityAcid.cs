using System.Collections;
using Spine;
using UnityEngine;

public class FatalityAcid : FatalityBase
{
	private GameObject currentAniamtionEffect;

	private GameObject currentEffect;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			StartAnimation(base.CachedManager.AnimationAcid);
			StartCoroutine(AnimationFinished(base.CachedManager.AnimationAcid.Duration - 0.2f));
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		StopAnimation(base.CachedManager.AnimationAcid);
	}

	protected IEnumerator AnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.CachedEffect.EmitAcidEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedManager.CharacterAnimation.transform.position, base.transform.eulerAngles);
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
	}

	protected override void AnimationEnd(Spine.Animation anim)
	{
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
	}

	protected override void PlayFatalityStartSound()
	{
		base.PlayFatalityStartSound();
		AudioManager.PlayAudioWithKey("fatality_acid");
		if (base.CachedManager.CurrentCharacter.CharacterVoiceType == CharacterVoiceType.ManNormal)
		{
			AudioManager.PlayAudioWithKey("fatality_acid_normal_man");
		}
	}
}
