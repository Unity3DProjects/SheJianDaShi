using UnityEngine;

public class Watermelon : WeaponGeneral
{
	[SerializeField]
	private GameObject watermelonMesh;

	private float watermelonRotationSpeed;

	protected override void Awake()
	{
		base.Awake();
		watermelonRotationSpeed = watermelonMesh.transform.localRotation.x;
		disableSqrVelocityBorder = 70f;
		disableAngularVelocityBorder = 0.2f;
	}

	public override void UpdateWeapon(float deltaTime)
	{
		watermelonRotationSpeed += 1000f * deltaTime;
		base.UpdateWeapon(deltaTime);
		if (isFlying && isRotateFromDirection)
		{
			watermelonMesh.transform.localRotation = Quaternion.Euler(watermelonRotationSpeed, watermelonMesh.transform.localRotation.y, watermelonMesh.transform.localRotation.z);
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			ExplodeComponent component = GetComponent<ExplodeComponent>();
			if (component != null)
			{
				component.Explode(this, level);
			}
			AudioManager.PlayAudioWithKey("watermelon_collision_1");
		}
		return collidable;
	}
}
