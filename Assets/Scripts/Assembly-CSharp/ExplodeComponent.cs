using UnityEngine;

public class ExplodeComponent : MonoBehaviour
{
	[SerializeField]
	private float explodeForce = 1f;

	[SerializeField]
	private float minExplodeForce = 0.3f;

	[SerializeField]
	private float explodeRadius = 500f;

	[SerializeField]
	private float explodeDamage = 10f;

	[SerializeField]
	private string explosionSound = string.Empty;

	[SerializeField]
	private float shakeDuration = 0.5f;

	[SerializeField]
	private float shakeAmplitude = 5f;

	public float ExplodeRadius
	{
		get
		{
			return explodeRadius;
		}
	}

	public static float AddExplodeForce(Rigidbody rigidbody, ExplosionInfo explosionInfo)
	{
		float num = 0f;
		if (rigidbody != null)
		{
			float num2 = Vector3.Distance(rigidbody.worldCenterOfMass, explosionInfo.position);
			num = Mathf.Max((explosionInfo.radius - num2) / explosionInfo.radius, explosionInfo.minForce);
			Vector3 vector = Vector3.Normalize(rigidbody.worldCenterOfMass - explosionInfo.position);
			rigidbody.AddForce(explosionInfo.force * (rigidbody.mass / 100f) * num * vector);
		}
		return num;
	}

	public void AddExplodeForceFromWeapon(Rigidbody rigidbody, WeaponGeneral weapon)
	{
		ExplosionInfo explosionInfo = new ExplosionInfo
		{
			position = weapon.CachedTransform.transform.position,
			force = 3000000f * explodeForce,
			minForce = minExplodeForce,
			radius = explodeRadius,
			damage = explodeDamage,
			weaponType = weapon.WeaponType
		};
		float num = Vector3.Distance(rigidbody.worldCenterOfMass, explosionInfo.position);
		float num2 = 0f;
		num2 = Mathf.Max((explosionInfo.radius - num) / explosionInfo.radius, explosionInfo.minForce);
		Vector3 vector = Vector3.Normalize(rigidbody.worldCenterOfMass - explosionInfo.position);
		rigidbody.AddForce(explosionInfo.force * (rigidbody.mass / 100f) * num2 * vector);
	}

	public void Explode(WeaponGeneral weapon, Level level, bool withDestroy = true, bool withDisable = true, bool isFatalityStartedWithDelay = false, float fatalityDelay = 0f)
	{
		if (shakeDuration > 0f)
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(shakeDuration, shakeAmplitude, !level.CameraSettings.isMoving);
		}
		Emission component = base.gameObject.GetComponent<Emission>();
		if ((bool)component)
		{
			component.EmitOnExplosion(weapon.CachedTransform.position);
		}
		AudioManager.PlayAudioWithKey(explosionSound);
		level.Explode(new ExplosionInfo
		{
			position = weapon.CachedTransform.transform.position,
			force = 3000000f * explodeForce,
			minForce = minExplodeForce,
			radius = explodeRadius,
			damage = explodeDamage,
			weaponType = weapon.WeaponType,
			isFatalityStartedWithDelay = isFatalityStartedWithDelay,
			fatalityDelay = fatalityDelay
		});
		if (withDisable)
		{
			weapon.Disabled();
		}
		if (withDestroy)
		{
			level.RemoveWeapon(weapon);
		}
	}
}
