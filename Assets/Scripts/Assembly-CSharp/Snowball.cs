public class Snowball : WeaponGeneral
{
	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			ExplodeComponent component = GetComponent<ExplodeComponent>();
			if (component != null)
			{
				component.Explode(this, level);
			}
		}
		return collidable;
	}
}
