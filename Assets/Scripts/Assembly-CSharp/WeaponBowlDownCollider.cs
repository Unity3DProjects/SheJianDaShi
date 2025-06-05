using UnityEngine;

public class WeaponBowlDownCollider : WeaponCollider
{
	[SerializeField]
	private float collisionForce;

	public float CollisionForce
	{
		get
		{
			return collisionForce;
		}
	}

	public override void OnCollide(WeaponGeneral weapon, CollisionInfo collisionInfo, ICollidable collidedObject)
	{
		collidedObject.ApplyForce(collisionForce, ForceMode.VelocityChange, collisionInfo);
		if (collidedObject is CharacterBase && shakeDuration > 0f)
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(shakeDuration, shakeAmplitude);
		}
		if (collidedObject is CharacterBase || collidedObject is Bird)
		{
			weapon.CachedEmission.EmitOnHit(collisionInfo.position, collisionInfo.otherCollider.transform);
		}
	}
}
