using UnityEngine;

public class Dog : NyanCat
{
	[SerializeField]
	private float activatedSpeed;

	protected override void OnTapActivation()
	{
		isRotateFromDirection = true;
		float x = activatedSpeed * ((!(base.CachedRigidbody.velocity.x > 0f)) ? (-1f) : 1f);
		base.CachedRigidbody.velocity = new Vector3(x, 0f);
		WeaponActivation weaponActivation = base.gameObject.AddComponent<WeaponActivation>();
		weaponActivation.Init(WeaponActivationType.Hit, CollideObjectType.All, this, "OnHitActivation");
		weaponActivation.IsAutoRemoved = true;
		AddActivation(weaponActivation);
	}

	protected override void ExplosionSoundPlay()
	{
	}

	protected override void CreateSoundPlay()
	{
	}
}
