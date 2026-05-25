using System.Collections;
using UnityEngine;

public class FatalityPickaxe : FatalitySatelite
{
	[SerializeField]
	private FatalityTNTs fatalityDynamitsPrefab;

	[SerializeField]
	private int gutsMultiplier = 3;

	private FatalityTNTs fatalityDynamits;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			if (fatalityDynamitsPrefab != null)
			{
				fatalityDynamits = Object.Instantiate(fatalityDynamitsPrefab);
				fatalityDynamits.transform.parent = base.CachedManager.transform;
				fatalityDynamits.transform.localEulerAngles = Vector3.zero;
				fatalityDynamits.transform.localPosition = Vector3.zero;
				fatalityDynamits.StartFatalityBrick(base.gameObject);
			}
		}
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		AudioManager.PlayAudioWithKey("pickaxe_fatality_explode");
		if (!GoreContentFilter.AllowInternalOrgans)
		{
			base.CachedManager.CurrentCharacter.StartDefaultAnimation();
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(1f);
			PlayCrowdHappySound();
			return;
		}
		base.CachedManager.CharacterAnimation.gameObject.SetActive(false);
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedHead.transform.position, base.CachedHead.transform.eulerAngles);
		base.CachedEffect.EmitBrain(base.transform, base.CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGuts(base.transform, base.CachedBody.transform.position, Vector3.up);
		for (int i = 0; i < gutsMultiplier; i++)
		{
			base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, base.CachedBody.transform.position, base.CachedBody.transform.eulerAngles);
		}
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(1f);
		PlayCrowdHappySound();
	}

	protected override IEnumerator CreateSatellite()
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			AudioManager.StopAudioWithKey("arrow_firework_idle");
		}, delayBeforeStart - 0.2f);
		yield return new WaitForSeconds(delayBeforeStart);
		fatalityDynamits.ExplodeAllTNTs();
		ExplodeCharacter();
	}

	protected override void SetSateliteVelocity()
	{
	}
}
