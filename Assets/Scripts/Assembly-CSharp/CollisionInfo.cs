using UnityEngine;

public class CollisionInfo
{
	public Collider collider;

	public Collider otherCollider;

	public Vector3 position;

	public Vector3 direction;

	public float damage;

	public float speedRatio;

	public DecalType decalType;

	public string boneName;

	public WeaponType fatality;

	public bool isDontApplyRagdoll;

	public bool isDisableRestore;

	public bool isDisableFatality;

	public FatalityActivationType fatalityActvationType = FatalityActivationType.AnyType;
}
