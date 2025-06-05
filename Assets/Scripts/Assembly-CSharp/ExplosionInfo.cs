using UnityEngine;

public struct ExplosionInfo
{
	public Vector3 position;

	public float force;

	public float minForce;

	public float radius;

	public float damage;

	public WeaponType weaponType;

	public bool isFatalityStartedWithDelay;

	public float fatalityDelay;
}
