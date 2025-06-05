using UnityEngine;

public class JellyFish : WeaponGeneral
{
	[SerializeField]
	private float reflectionSpeed;

	[SerializeField]
	private bool isExplodeByHit;

	[SerializeField]
	private string destroySound;

	private float jellyLifeTime;

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		collisionInfo.isDontApplyRagdoll = true;
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			RemoveAllActivations();
			if (collidable is Ground)
			{
				if (isExplodeByHit)
				{
					if ((bool)base.CachedEmission)
					{
						base.CachedEmission.EmitOnExplosion(base.CachedTransform.position);
					}
					Disabled();
					level.RemoveWeapon(this);
				}
				AudioManager.PlayAudioWithKey(destroySound);
			}
		}
		return collidable;
	}

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		if (!isActivated)
		{
			return;
		}
		jellyLifeTime += deltaTime;
		if (jellyLifeTime >= periodicalDamageInfo.duration)
		{
			if ((bool)base.CachedEmission)
			{
				base.CachedEmission.EmitOnExplosion(base.CachedTransform.position);
			}
			Disabled();
			level.RemoveWeapon(this);
			AudioManager.PlayAudioWithKey(destroySound);
		}
	}

	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		if (collidedObject is CharacterBase || collidedObject is Bird)
		{
			level.TurnActionsChangeCount(1);
			base.DriveInto(collidedObject, collisionInfo, depth);
			periodicalDamageInfo.position = base.transform.position;
			level.ApplyPeriodicalDamage(periodicalDamageInfo);
			isActivated = true;
			level.TurnActionsChangeCount(-1);
			AudioManager.PlayAudioWithKey("jellyfish_shock");
		}
	}

	protected override void EmitObjects()
	{
		if ((bool)base.CachedEmission)
		{
			base.CachedEmission.EmitObjects(level, base.CachedTransform.position, 90f, reflectionSpeed);
		}
		Disabled();
		level.RemoveWeapon(this);
	}
}
