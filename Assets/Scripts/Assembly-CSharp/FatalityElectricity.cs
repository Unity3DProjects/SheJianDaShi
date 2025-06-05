using System.Collections;
using Spine;
using UnityEngine;

public class FatalityElectricity : FatalityBase
{
	private GameObject currentAniamtionEffect;

	private GameObject currentEffect;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			StartAnimation(base.CachedManager.AnimationShock);
			StartCoroutine(AnimationFinished(base.CachedManager.AnimationShock.Duration - 0.2f));
			base.CachedManager.CurrentCharacter.StartElectricityEffect();
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		StopAnimation(base.CachedManager.AnimationShock);
	}

	protected IEnumerator AnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.CachedEffect.EmitShockEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedManager.CharacterAnimation.transform.position, base.transform.eulerAngles);
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
		base.CachedManager.CurrentCharacter.StopElectricityEffect(0f);
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
		AudioManager.PlayAudioWithKey("fatality_electricity");
	}
}
