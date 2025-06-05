public class Marshmallow : WeaponGeneral
{
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
		}
		else
		{
			base.DriveInto(collidedObject, collisionInfo, depth);
		}
	}
}
