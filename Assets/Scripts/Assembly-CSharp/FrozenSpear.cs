using UnityEngine;

public class FrozenSpear : WeaponGeneral
{
	[SerializeField]
	private int emissionCount;

	private int currentEmissionCount;

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collisionInfo.collider != null)
		{
			WeaponCollider[] componentsInChildren = collisionInfo.collider.GetComponentsInChildren<WeaponCollider>();
			WeaponCollider[] array = componentsInChildren;
			foreach (WeaponCollider weaponCollider in array)
			{
				if (weaponCollider is WeaponDriveIntoCollider)
				{
					if (collidable is CharacterBase || collidable is Bird)
					{
						AudioManager.PlayAudioWithKey("frozen_spear_character_pierce_1");
						AudioManager.PlayAudioWithKey("frozen_spear_character_pierce_2");
						AudioManager.PlayAudioWithKey("frozen_spear_character_pierce_3");
					}
					AudioManager.PlayAudioWithKey("frozen_spear_pierce");
				}
			}
		}
		return collidable;
	}

	protected override void EmitObjects()
	{
		currentEmissionCount++;
		if (currentEmissionCount == emissionCount)
		{
			WeaponActivation component = GetComponent<WeaponActivation>();
			if ((bool)component)
			{
				RemoveActivation(component);
			}
		}
		Emission component2 = base.gameObject.GetComponent<Emission>();
		if ((bool)component2)
		{
			component2.EmitObjects(level, base.CachedTransform.position, Mathf.Atan2(base.CachedRigidbody.velocity.y, base.CachedRigidbody.velocity.x) * 57.29578f, base.CachedRigidbody.velocity.magnitude);
		}
	}
}
