public class Suitcase : WeaponGeneral
{
	private bool isTrailActive = true;

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable result = base.Collided(collisionInfo);
		if ((bool)base.CachedEmission && isTrailActive)
		{
			isTrailActive = false;
			base.CachedEmission.StopTrails();
		}
		return result;
	}
}
