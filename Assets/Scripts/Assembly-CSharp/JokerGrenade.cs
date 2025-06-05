public class JokerGrenade : WeaponGeneral
{
	private bool disableExplode;

	public bool DisableExplode
	{
		get
		{
			return disableExplode;
		}
		set
		{
			disableExplode = value;
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null && !DisableExplode)
		{
			ExplodeComponent component = GetComponent<ExplodeComponent>();
			if (component != null)
			{
				component.Explode(this, level);
			}
		}
		return collidable;
	}

	public override void Shoot()
	{
		base.Shoot();
		if (this != null)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				AudioManager.StopAudioWithKey("joker_grenade_flying", 0.5f);
			}, 1.5f);
		}
	}
}
