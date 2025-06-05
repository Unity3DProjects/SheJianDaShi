using System.Collections;
using UnityEngine;

public class Cat : WeaponGeneral
{
	private GameObject deathEffect;

	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		if (collidedObject is Ground)
		{
			RemoveAllActivations();
			base.transform.localScale = Vector3.zero;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				deathEffect = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.WeaponCatDeath));
				deathEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.gameObject.transform;
				deathEffect.transform.position = base.CachedTransform.position;
			}, 0f);
			StartCoroutine(CoroutineDeathFinished(0.5f));
		}
		base.DriveInto(collidedObject, collisionInfo, depth);
	}

	private IEnumerator CoroutineDeathFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		Object.Destroy(deathEffect);
		Disabled();
		level.RemoveWeapon(this);
	}
}
