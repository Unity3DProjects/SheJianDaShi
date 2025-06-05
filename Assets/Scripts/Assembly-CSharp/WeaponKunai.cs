public class WeaponKunai : WeaponGeneral
{
	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null && base.Activations.Exists((WeaponActivation t) => t.Type == WeaponActivationType.Tap))
		{
			RemoveAllActivations();
		}
		return collidable;
	}
}
