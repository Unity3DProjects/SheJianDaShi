public class Baguette : WeaponGeneral
{
	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null && collidable is Ground)
		{
			Disabled();
			level.RemoveWeapon(this);
		}
		return collidable;
	}
}
