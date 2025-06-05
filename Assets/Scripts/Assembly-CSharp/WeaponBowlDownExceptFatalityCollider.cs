using UnityEngine;

public class WeaponBowlDownExceptFatalityCollider : WeaponBowlDownCollider
{
	public override void OnCollide(WeaponGeneral weapon, CollisionInfo collisionInfo, ICollidable collidedObject)
	{
		collidedObject.ApplyForce(base.CollisionForce, ForceMode.VelocityChange, collisionInfo);
		if (collidedObject is CharacterBase && shakeDuration > 0f)
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(shakeDuration, shakeAmplitude);
		}
		if ((collidedObject is CharacterBase && !(collidedObject as CharacterBase).IsFinishHim) || collidedObject is Bird)
		{
			weapon.CachedEmission.EmitOnHit(collisionInfo.position, collisionInfo.otherCollider.transform);
		}
	}
}
