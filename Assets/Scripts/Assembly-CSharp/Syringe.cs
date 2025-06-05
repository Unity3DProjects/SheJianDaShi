using UnityEngine;

public class Syringe : WeaponGeneral
{
	[SerializeField]
	private GameObject piston;

	private bool needsToInject;

	private float pistonInitialPositionX;

	private float pistonTargetPositionX = 78f;

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		if (needsToInject)
		{
			float num = Mathf.Abs(piston.transform.localPosition.x - pistonTargetPositionX);
			if (num <= 2f)
			{
				needsToInject = false;
			}
			float x = Mathf.Lerp(pistonTargetPositionX, pistonInitialPositionX, deltaTime * 0.1f);
			piston.transform.localPosition = new Vector2(x, piston.transform.localPosition.y);
		}
	}

	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		base.DriveInto(collidedObject, collisionInfo, depth);
		needsToInject = true;
		pistonInitialPositionX = piston.transform.localPosition.x;
	}
}
