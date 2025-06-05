using UnityEngine;

public class Shield : Mjolnir
{
	private float shieldRotationSpeed;

	protected override void Awake()
	{
		base.Awake();
		shieldRotationSpeed = weaponMesh.transform.localRotation.z;
		disableSqrVelocityBorder = 70f;
		disableAngularVelocityBorder = 0.2f;
	}

	public override void UpdateWeapon(float deltaTime)
	{
		shieldRotationSpeed += 1000f * deltaTime;
		base.UpdateWeapon(deltaTime);
		if (isFlying && isRotateFromDirection)
		{
			weaponMesh.transform.localRotation = Quaternion.Euler(weaponMesh.transform.localRotation.x, weaponMesh.transform.localRotation.y, shieldRotationSpeed);
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			AudioManager.PlayAudioWithKey("mjolnir_ground_collision");
		}
		return collidable;
	}

	public override void Activate(WeaponActivationType type, ICollidable collidedObject)
	{
		base.Activate(type, collidedObject);
		AudioManager.PlayAudioWithKey("nyan_cat_activation_2");
	}

	protected override void OnTapActivation()
	{
		base.CachedRigidbody.useGravity = false;
		base.CachedRigidbody.velocity = base.CachedRigidbody.velocity.normalized * 2f * base.CachedRigidbody.velocity.magnitude;
	}
}
