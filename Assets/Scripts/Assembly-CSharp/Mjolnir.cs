using UnityEngine;

public class Mjolnir : WeaponGeneral
{
	[SerializeField]
	protected Collider headCollider;

	[SerializeField]
	protected bool destroyByExplode;

	[SerializeField]
	private GameObject periodicalElectricityPrefab;

	private bool explodeDamageWasDone;

	private bool isCharacterCollided;

	private PeriodicalElectricityDamage periodicalElectricity;

	private PeriodicalElectricityDamage PeriodicalElectricity
	{
		get
		{
			if (periodicalElectricity == null)
			{
				GameObject gameObject = Object.Instantiate(periodicalElectricityPrefab);
				gameObject.transform.parent = base.transform;
				periodicalElectricity = gameObject.GetComponent<PeriodicalElectricityDamage>();
			}
			return periodicalElectricity;
		}
	}

	protected override void OnTriggerEnter(Collider otherCollider)
	{
		if (!isCollideEnabled)
		{
			return;
		}
		base.OnTriggerEnter(otherCollider);
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = headCollider;
		collisionInfo.direction = Vector3.down;
		collisionInfo.otherCollider = otherCollider;
		collisionInfo.position = otherCollider.transform.position;
		collisionInfo.decalType = decalType;
		collisionInfo.fatality = base.WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
		ICollidable[] componentsInParent = otherCollider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Character && !isCharacterCollided)
			{
				Collided(collisionInfo);
				isCharacterCollided = true;
				isCollideEnabled = true;
				break;
			}
			if (collidable is Ground)
			{
				Collided(collisionInfo);
				break;
			}
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		if (!isActivated)
		{
			collisionInfo.fatalityActvationType = FatalityActivationType.Collision;
		}
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			if (base.Activations.Exists((WeaponActivation t) => t.Type == WeaponActivationType.Tap))
			{
				RemoveAllActivations();
			}
			OnHit(collidable);
		}
		return collidable;
	}

	protected override void OnTapActivation()
	{
		isRotateFromDirection = true;
		base.CachedRigidbody.velocity = new Vector3(0f, -1000f);
		base.CachedRigidbody.angularVelocity = Vector3.zero;
		foreach (Collider collider in base.Colliders)
		{
			collider.isTrigger = true;
		}
		WeaponActivation weaponActivation = base.gameObject.AddComponent<WeaponActivation>();
		weaponActivation.Init(WeaponActivationType.Hit, CollideObjectType.Ground, this, "OnHitActivation");
		weaponActivation.IsAutoRemoved = true;
		AddActivation(weaponActivation);
		groundHitSound = "mjolnir_activation_ground_hit";
	}

	protected override void OnHitActivation()
	{
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null && !explodeDamageWasDone)
		{
			component.Explode(this, level, destroyByExplode);
			OnExplode(component);
			explodeDamageWasDone = true;
		}
		base.CachedRigidbody.isKinematic = true;
	}

	protected virtual void OnHit(ICollidable collidedObject)
	{
		if (collidedObject is CharacterBase)
		{
			CharacterBase characterBase = collidedObject as CharacterBase;
			characterBase.StartElectricityIdleAnimation();
		}
		else if (collidedObject is Bird)
		{
			(collidedObject as Bird).DamageApplied(FlyingObjectDamageType.Electricity, new Vector2(0f, 1f));
		}
		else if (collidedObject is Ground)
		{
			AudioManager.PlayAudioWithKey("mjolnir_ground_collision");
		}
	}

	protected virtual void OnExplode(ExplodeComponent component)
	{
		if ((bool)level.InactiveArcher && level.InactiveArcher.IsAffectedInRadius(base.transform.position, component.ExplodeRadius))
		{
			level.InactiveArcher.StartElectricityIdleAnimation();
		}
		if (level.ActiveCharacter.IsAffectedInRadius(base.transform.position, component.ExplodeRadius))
		{
			level.ActiveCharacter.StartElectricityIdleAnimation();
		}
	}
}
