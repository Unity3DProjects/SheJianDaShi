using UnityEngine;

public class FireworkArrow : WeaponGeneral
{
	[SerializeField]
	private float activationTimer;

	private float activationTime;

	private bool isActivatedAfterTap;

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		if (!isActivatedAfterTap)
		{
			return;
		}
		activationTime += deltaTime;
		if (activationTime >= activationTimer)
		{
			ExplodeComponent component = GetComponent<ExplodeComponent>();
			if (component != null)
			{
				component.Explode(this, level);
				AudioManager.PlayAudioWithKey("arrow_firework_main_explosion");
			}
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable result = base.Collided(collisionInfo);
		RemoveAllActivations();
		base.IdleEffect.StopFX(true);
		isActivatedAfterTap = false;
		return result;
	}

	protected override void OnTapActivation()
	{
		isActivatedAfterTap = true;
	}
}
