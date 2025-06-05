public class LoveArrow : WeaponGeneral
{
	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		if (collidedObject is Ground)
		{
			isCollideEnabled = false;
			RemoveAllActivations();
			EmitObjects();
		}
		base.DriveInto(collidedObject, collisionInfo, depth);
	}

	protected override void OnTapActivation()
	{
		base.OnTapActivation();
		AudioManager.PlayAudioWithKey("frozen_spear_fatality_explode_3");
	}
}
