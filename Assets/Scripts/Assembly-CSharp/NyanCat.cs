using UnityEngine;

public class NyanCat : Mjolnir
{
	[SerializeField]
	private int reboundQuantity;

	private bool collideWithCharacterWasDone;

	private int currentReboundQuantity;

	public override bool IsWeaponVisible
	{
		get
		{
			return base.IsWeaponVisible;
		}
		set
		{
			if (value && isWeaponVisible != value)
			{
				CreateSoundPlay();
			}
			base.IsWeaponVisible = value;
		}
	}

	protected override void OnTriggerEnter(Collider otherCollider)
	{
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		activationSound = string.Empty;
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			if (collidable is CharacterBase)
			{
				OnHitActivation();
			}
			else
			{
				currentReboundQuantity++;
				if (currentReboundQuantity >= reboundQuantity)
				{
					ExplodeComponent component = GetComponent<ExplodeComponent>();
					if (component != null)
					{
						component.Explode(this, level, destroyByExplode);
						AudioManager.PlayAudioWithKey("nyan_cat_main_explosion");
					}
				}
				isCollideEnabled = true;
			}
		}
		return collidable;
	}

	public override void Activate(WeaponActivationType type, ICollidable collidedObject)
	{
		base.Activate(type, collidedObject);
		AudioManager.PlayAudioWithKey("nyan_cat_activation_2");
	}

	protected override void OnTapActivation()
	{
		base.CachedRigidbody.useGravity = false;
		base.CachedRigidbody.velocity = base.CachedRigidbody.velocity.normalized * 2f * base.CachedRigidbody.velocity.magnitude;
		WeaponActivation weaponActivation = base.gameObject.AddComponent<WeaponActivation>();
		weaponActivation.Init(WeaponActivationType.Hit, CollideObjectType.All, this, "OnHitActivation");
		weaponActivation.IsAutoRemoved = true;
		AddActivation(weaponActivation);
	}

	protected override void OnHitActivation()
	{
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null && !collideWithCharacterWasDone)
		{
			component.Explode(this, level, destroyByExplode);
			collideWithCharacterWasDone = true;
			ExplosionSoundPlay();
		}
		base.CachedRigidbody.isKinematic = true;
	}

	protected virtual void CreateSoundPlay()
	{
		AudioManager.PlayAudioWithKey("nyan_cat_create_2");
	}

	protected virtual void ExplosionSoundPlay()
	{
		AudioManager.PlayAudioWithKey("nyan_cat_main_explosion");
	}

	protected override void OnHit(ICollidable collidedObject)
	{
	}

	protected override void OnExplode(ExplodeComponent component)
	{
	}
}
