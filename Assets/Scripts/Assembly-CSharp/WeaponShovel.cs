using UnityEngine;

public class WeaponShovel : WeaponGeneral
{
	[SerializeField]
	private float activatedSpeed;

	[SerializeField]
	private bool isRotateFromDirectionAfterActivation = true;

	protected override void OnTapActivation()
	{
		isRotateFromDirection = isRotateFromDirectionAfterActivation;
		float x = ((!(base.CachedRigidbody.velocity.x > 0f)) ? (0f - activatedSpeed) : activatedSpeed);
		base.CachedRigidbody.velocity = new Vector3(x, 0f);
	}
}
