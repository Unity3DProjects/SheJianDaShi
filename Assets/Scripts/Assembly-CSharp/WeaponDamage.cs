using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
	[SerializeField]
	private float damage;

	public void OnCollide(WeaponGeneral weapon, CollisionInfo collisionInfo, ICollidable collidedObject)
	{
		collisionInfo.damage = Mathf.Min(weapon.Velocity.magnitude / weapon.MaxSpeed, 1f) * damage * weapon.DamagePercent;
		collidedObject.ApplyDamage(collisionInfo);
	}
}
