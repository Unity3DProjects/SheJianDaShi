using UnityEngine;

public class SpikySeed : WeaponGeneral
{
	private const string SPIKY_SEED_ANIMATION_NAME = "seed_spikes";

	private const string SPIKY_SEED_FINISH_HIM_ANIMATION_NAME = "seed_spikes_scale";

	[SerializeField]
	private SkeletonAnimation spikySeedAnimation;

	private bool isFirstCollide;

	protected override void Awake()
	{
		if (spikySeedAnimation != null)
		{
			spikySeedAnimation.ignoreMonoUpdate = true;
			spikySeedAnimation.State.SetAnimation(0, "seed_spikes", false);
		}
		base.Awake();
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null && !isFirstCollide)
		{
			isFirstCollide = true;
			if (spikySeedAnimation != null)
			{
				spikySeedAnimation.ignoreMonoUpdate = false;
			}
			AudioManager.PlayAudioWithKey("watermelon_collision_1");
		}
		if (collidable is Character && !(collidable as Character).IsFinishHim)
		{
			Rigidbody rootRigidBody = (collidable as Character).RootRigidBody;
			if (rootRigidBody != null)
			{
				(collidable as Character).RootRigidBody.velocity = velocity;
			}
		}
		else if (collidable is Character && (collidable as Character).IsFinishHim)
		{
			spikySeedAnimation.State.SetAnimation(0, "seed_spikes_scale", false);
		}
		return collidable;
	}
}
