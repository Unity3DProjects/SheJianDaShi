using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
	[SerializeField]
	protected float shakeDuration = 0.5f;

	[SerializeField]
	protected float shakeAmplitude = 5f;

	public virtual void OnCollide(WeaponGeneral weapon, CollisionInfo collisionInfo, ICollidable collidedObject)
	{
		if (collidedObject is CharacterBase && shakeDuration > 0f)
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(shakeDuration, shakeAmplitude);
		}
		if (!(collidedObject is CharacterBase) && !(collidedObject is Bird))
		{
			return;
		}
		weapon.Velocity = new Vector3((0f - weapon.Velocity.x) * weapon.Bounciness, weapon.Velocity.y, weapon.Velocity.z);
		if (weapon.IsPhysical)
		{
			weapon.CachedRigidbody.velocity = new Vector3((0f - weapon.CachedRigidbody.velocity.x) * weapon.Bounciness, weapon.CachedRigidbody.velocity.y, weapon.CachedRigidbody.velocity.z);
			if ((bool)weapon.CachedEmission)
			{
				weapon.CachedEmission.EmitOnHit(collisionInfo.position, collisionInfo.otherCollider.transform);
			}
		}
	}
}
