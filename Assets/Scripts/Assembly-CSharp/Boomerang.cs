using UnityEngine;

public class Boomerang : Propeller
{
	[SerializeField]
	private float activationTime;

	private bool isBeingActivated;

	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		if (!isDelaying)
		{
			isWeaponAfterCollisionAvailable = false;
			base.DriveInto(collidedObject, collisionInfo, depth);
		}
	}

	protected override void CollideWithCharacter(CollisionInfo collisionInfo, ICollidable collidableObject, Collider otherCollider)
	{
		(collidableObject as Character).ApplyDamage(collisionInfo);
		if ((bool)base.CachedEmission)
		{
			base.CachedEmission.EmitOnHit(otherCollider.ClosestPointOnBounds(base.transform.position));
		}
	}

	protected override void EnableTriggerCollider(Collider currentCollider)
	{
	}

	private void OnCollisionActivation()
	{
		isFlying = false;
		WeaponActivation weaponActivation = base.gameObject.AddComponent<WeaponActivation>();
		AudioManager.PlayAudioWithKey("boomerang_activation_flying");
		weaponActivation.Init(WeaponActivationType.Timer, CollideObjectType.None, this, "OnTimerFinished");
		weaponActivation.ActivationTime = activationTime;
		weaponActivation.IsAutoRemoved = true;
		AddActivation(weaponActivation);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.WeaponBoomerangPulse));
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
		}, 0f);
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if ((bool)level && (bool)level.InactiveArcher && level.InactiveArcher.IsFinishHim)
		{
			collisionInfo.isDisableFatality = true;
			if (collidable is CharacterBase)
			{
				collisionInfo.fatalityActvationType = FatalityActivationType.None;
			}
		}
		OnCollisionActivation();
		return collidable;
	}

	protected override void OnTimerFinished()
	{
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null)
		{
			component.Explode(this, level);
			AudioManager.StopAudioWithKey("boomerang_activation_flying");
		}
		isWeaponAfterCollisionAvailable = true;
	}
}
