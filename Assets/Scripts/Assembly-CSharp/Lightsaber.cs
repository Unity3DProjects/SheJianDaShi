using UnityEngine;

public class Lightsaber : WeaponGeneral
{
	[SerializeField]
	private TweenScale bladeScaler;

	[SerializeField]
	private Collider bladeTrigger;

	private bool isBladeActive = true;

	private bool isCharacterDamaged;

	private bool IsBladeActive
	{
		get
		{
			return isBladeActive;
		}
		set
		{
			if (isBladeActive != value)
			{
				isBladeActive = value;
				if (!isBladeActive)
				{
					AudioManager.PlayAudioWithKey("lightsaber_off");
				}
			}
		}
	}

	private bool IsCharacterDamaged
	{
		get
		{
			return isCharacterDamaged;
		}
		set
		{
			if (isCharacterDamaged != value)
			{
				isCharacterDamaged = value;
				if (isCharacterDamaged)
				{
					AudioManager.PlayAudioWithKey("lightsaber_character_hit");
				}
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		disableAngularVelocityBorder = 0.3f;
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		if (IsBladeActive)
		{
			Ground ground = collision.collider.GetComponent<Ground>();
			if (!ground)
			{
				ground = collision.collider.GetComponentInParent<Ground>();
			}
			if ((bool)ground)
			{
				TurnOffBlade();
			}
		}
	}

	protected override void OnTriggerEnter(Collider collider)
	{
		base.OnTriggerEnter(collider);
		if (!isCollideEnabled)
		{
			return;
		}
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = bladeTrigger;
		collisionInfo.direction = Vector3.down;
		collisionInfo.otherCollider = collider;
		collisionInfo.position = collider.transform.position;
		collisionInfo.decalType = decalType;
		collisionInfo.fatality = base.WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
		ICollidable[] componentsInParent = collider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Character)
			{
				SkeletonUtilityBone component = collider.transform.parent.GetComponent<SkeletonUtilityBone>();
				if ((bool)component)
				{
					collisionInfo.boneName = component.boneName;
				}
				Collided(collisionInfo);
				isCollideEnabled = true;
				if ((collidable as Character).IsFinishHim)
				{
					AudioManager.PlayAudioWithKey(characterHitSound);
					FatalityBase activeFatality = (collidable as Character).FatalityManager.ActiveFatality;
					if (!activeFatality)
					{
						(collidable as Character).FatalityManager.StartFatality(collisionInfo);
					}
					else if (activeFatality is FatalityBreak)
					{
						(activeFatality as FatalityBreak).BreakPart(collisionInfo);
					}
				}
				IsCharacterDamaged = true;
				break;
			}
			if (collidable is Ground && IsBladeActive)
			{
				AudioManager.PlayAudioWithKey("lightsaber_blade_ground_hit");
				break;
			}
		}
	}

	public override void Shoot()
	{
		base.Shoot();
		AudioManager.StopAudioWithKey(idleSound);
	}

	protected override void SetCollidersEnabled(bool isEnabled)
	{
		if ((bool)level && (bool)level.InactiveArcher && level.InactiveArcher.IsFinishHim)
		{
			foreach (Collider collider in base.Colliders)
			{
				collider.enabled = true;
				if (collider != bladeTrigger)
				{
					collider.gameObject.layer = LayerMask.NameToLayer("IgnoreCharacterCollisions");
				}
			}
			return;
		}
		base.SetCollidersEnabled(isEnabled);
	}

	protected void TurnOffBlade()
	{
		bladeScaler.enabled = true;
		base.IdleEffect.StopFX(true);
		if ((bool)base.CachedEmission)
		{
			base.CachedEmission.StopTrails();
		}
		IsBladeActive = false;
	}
}
