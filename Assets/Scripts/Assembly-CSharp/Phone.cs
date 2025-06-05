using UnityEngine;

public class Phone : WeaponGeneral
{
	[SerializeField]
	private float activationTime;

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable is CharacterBase)
		{
			OnTimerFinished();
			isCollideEnabled = false;
		}
		return collidable;
	}

	protected override void OnHitActivation()
	{
		WeaponActivation weaponActivation = base.gameObject.AddComponent<WeaponActivation>();
		weaponActivation.Init(WeaponActivationType.Timer, CollideObjectType.None, this, "OnTimerFinished");
		weaponActivation.ActivationTime = activationTime;
		weaponActivation.IsAutoRemoved = true;
		AddActivation(weaponActivation);
	}

	protected override void OnTimerFinished()
	{
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null)
		{
			component.Explode(this, level);
		}
	}
}
