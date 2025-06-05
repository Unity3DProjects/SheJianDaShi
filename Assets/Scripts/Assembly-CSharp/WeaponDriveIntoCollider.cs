using UnityEngine;

public class WeaponDriveIntoCollider : WeaponCollider
{
	[SerializeField]
	private float minSpeedRatio;

	[SerializeField]
	private float penetrationDepth;

	public override void OnCollide(WeaponGeneral weapon, CollisionInfo collisionInfo, ICollidable collidedObject)
	{
		float num = Mathf.Min(weapon.Velocity.magnitude / weapon.MaxSpeed, 1f);
		if (collidedObject is CharacterBase && num < minSpeedRatio)
		{
			weapon.IsRotateFromDirection = false;
			base.OnCollide(weapon, collisionInfo, collidedObject);
			return;
		}
		weapon.DriveInto(collidedObject, collisionInfo, penetrationDepth);
		if (collidedObject is CharacterBase || collidedObject is Bird)
		{
			weapon.CachedEmission.EmitOnHitDriveInto(collisionInfo.position, collisionInfo.otherCollider.transform);
		}
	}
}
