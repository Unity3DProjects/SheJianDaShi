public class AcidBottle : WeaponGeneral
{
	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null && (collidable is Ground || collidable is CharacterBase))
		{
			AudioManager.PlayAudioWithKey("acid_bottle_corrosion");
		}
		return collidable;
	}
}
