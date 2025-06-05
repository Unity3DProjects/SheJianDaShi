using UnityEngine;

public class Medallion : WeaponGeneral
{
	public const string IDLE_ANIMATION_NAME = "idle";

	[SerializeField]
	private SkeletonAnimation animationMedallion;

	[SerializeField]
	private SkeletonRagdoll ragdoll;

	[SerializeField]
	private Rigidbody medalionRigidBody;

	private bool isRagdollActive;

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		if (!isRagdollActive && animationMedallion != null && ragdoll != null && isFlying)
		{
			isRagdollActive = true;
			animationMedallion.State.SetAnimation(0, "idle", true);
			ragdoll.Apply();
			Rigidbody rootRigidbody = ragdoll.RootRigidbody;
			HingeJoint hingeJoint = rootRigidbody.gameObject.AddComponent<HingeJoint>();
			hingeJoint.connectedBody = medalionRigidBody;
			hingeJoint.enableCollision = ragdoll.enableJointCollision;
		}
	}

	public override void Disabled(bool isEnabled)
	{
		base.Disabled(isEnabled);
		Collider[] componentsInChildren = GetComponentsInChildren<MeshCollider>();
		Collider[] array = componentsInChildren;
		foreach (Collider collider in array)
		{
			collider.enabled = true;
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable is Ground && isFlying)
		{
			AudioManager.PlayAudioWithKey("chain_drop");
		}
		if (collidable is Character)
		{
			bool isFinishHim = (collidable as Character).IsFinishHim;
			ExplodeComponent component = GetComponent<ExplodeComponent>();
			if (component != null)
			{
				component.Explode(this, level, isFinishHim, isFinishHim);
			}
		}
		return collidable;
	}
}
