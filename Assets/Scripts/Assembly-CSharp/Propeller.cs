using UnityEngine;

public class Propeller : WeaponGeneral
{
	[SerializeField]
	private float timeIdle;

	[SerializeField]
	private float rotationSpeed;

	[SerializeField]
	private float rotationDamage;

	[SerializeField]
	private float rotationRadius;

	[SerializeField]
	private float damageInterval;

	[SerializeField]
	private SphereCollider rotationTrigger;

	[SerializeField]
	private float antigravityMultiplier;

	private float damageTime;

	private bool rotationDamageEnabled;

	protected bool isDelaying;

	protected bool isReturning;

	private float currentDelay;

	private float currentTime;

	private ShootingInfo startInfo;

	protected Vector3 delayingVelocity;

	protected override void Awake()
	{
		base.Awake();
		rotationTrigger.radius = rotationRadius;
	}

	protected override void OnTriggerEnter(Collider otherCollider)
	{
		base.OnTriggerEnter(otherCollider);
		ApplyRotationDamage(otherCollider);
	}

	private void OnTriggerStay(Collider otherCollider)
	{
		ApplyRotationDamage(otherCollider);
	}

	public override void PrepareBeforeShoot(Character shootCharacter)
	{
		if (!isActive)
		{
			base.PrepareBeforeShoot(shootCharacter);
			SetCollisionWithCharacter(shootCharacter, false);
		}
	}

	public override void Shoot()
	{
		base.Shoot();
		delayingVelocity = -base.CachedRigidbody.velocity;
		rotationTrigger.enabled = false;
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable is Character)
		{
			isCollideEnabled = true;
		}
		return collidable;
	}

	protected override void OnTapActivation()
	{
		if (isFlying)
		{
			isDelaying = true;
			rotationTrigger.enabled = true;
			FreezePosition(base.CachedRigidbody);
			currentDelay = timeIdle;
			characterHitSound = "propeller_character_hit";
		}
	}

	protected override void UpdatePhysicalWeapon(float deltaTime)
	{
		if (!isDelaying)
		{
			if (isActive)
			{
				if (isReturning)
				{
					base.CachedRigidbody.AddForce((0f - antigravityMultiplier) * Physics.gravity, ForceMode.Acceleration);
					currentTime -= deltaTime;
					if (currentTime < 0f && isWeaponAfterCollisionAvailable)
					{
						Disabled(true);
						level.RemoveWeapon(this);
					}
				}
				else
				{
					currentTime += deltaTime;
				}
			}
			base.UpdatePhysicalWeapon(deltaTime);
		}
		else
		{
			damageTime += deltaTime;
			if (damageTime >= damageInterval)
			{
				damageTime -= damageInterval;
				rotationDamageEnabled = true;
			}
			if (currentDelay > 0f)
			{
				currentDelay -= deltaTime;
			}
			else
			{
				OnRotationEnd();
			}
		}
		if (isActive && isWeaponAfterCollisionAvailable)
		{
			base.CachedTransform.eulerAngles += new Vector3(0f, 0f, rotationSpeed * deltaTime);
		}
	}

	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		if (!isDelaying)
		{
			base.DriveInto(collidedObject, collisionInfo, depth);
		}
	}

	private void ApplyRotationDamage(Collider otherCollider)
	{
		if (isDelaying)
		{
			if (rotationDamageEnabled && isCollideEnabled)
			{
				ApplyDamage(rotationTrigger, otherCollider, false);
				rotationDamageEnabled = false;
			}
		}
		else
		{
			ApplyDamage(base.Colliders.RandomObject(), otherCollider);
		}
	}

	private void ApplyDamage(Collider thisCollider, Collider otherCollider, bool collidedWithGround = true)
	{
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = thisCollider;
		collisionInfo.direction = Vector3.down;
		collisionInfo.otherCollider = otherCollider;
		collisionInfo.position = otherCollider.transform.position;
		collisionInfo.damage = rotationDamage;
		collisionInfo.decalType = decalType;
		collisionInfo.fatality = base.WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
		ICollidable[] componentsInParent = otherCollider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Character)
			{
				SkeletonUtilityBone component = otherCollider.transform.parent.GetComponent<SkeletonUtilityBone>();
				if ((bool)component)
				{
					collisionInfo.boneName = component.boneName;
				}
				isCollideEnabled = true;
				CollideWithCharacter(collisionInfo, collidable, otherCollider);
				break;
			}
			if (collidedWithGround)
			{
				Collided(collisionInfo);
				break;
			}
		}
	}

	public void FreezePosition(Rigidbody rigid)
	{
		if ((bool)rigid)
		{
			int constraints = (int)rigid.constraints;
			constraints |= 2;
			constraints |= 4;
			rigid.constraints = (RigidbodyConstraints)constraints;
		}
	}

	public void UnFreezePosition(Rigidbody rigid)
	{
		if ((bool)rigid)
		{
			int constraints = (int)rigid.constraints;
			constraints &= -3;
			constraints &= -5;
			rigid.constraints = (RigidbodyConstraints)constraints;
		}
	}

	protected override void SetCollidersEnabled(bool isEnabled)
	{
		if ((bool)level && (bool)level.InactiveArcher && level.InactiveArcher.IsFinishHim)
		{
			foreach (Collider collider in base.Colliders)
			{
				collider.enabled = true;
				EnableTriggerCollider(collider);
			}
			return;
		}
		base.SetCollidersEnabled(isEnabled);
	}

	protected virtual void SetCollisionWithCharacter(Character character, bool enabled)
	{
		if (!character)
		{
			return;
		}
		Collider[] componentsInChildren = character.GetComponentsInChildren<Collider>();
		if (componentsInChildren == null || colliders == null)
		{
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < colliders.Count; j++)
			{
				Physics.IgnoreCollision(componentsInChildren[i], colliders[j], !enabled);
			}
		}
	}

	protected virtual void CollideWithCharacter(CollisionInfo collisionInfo, ICollidable collidableObject, Collider otherCollider)
	{
		if ((collidableObject as Character).IsFinishHim)
		{
			AudioManager.PlayAudioWithKey(characterHitSound);
			FatalityBase activeFatality = (collidableObject as Character).FatalityManager.ActiveFatality;
			if (!activeFatality)
			{
				(collidableObject as Character).FatalityManager.StartFatality(collisionInfo);
			}
			else if (activeFatality is FatalityBreak)
			{
				(activeFatality as FatalityBreak).BreakPart(collisionInfo);
			}
		}
		else
		{
			(collidableObject as Character).ApplyDamage(collisionInfo);
			if ((bool)base.CachedEmission)
			{
				base.CachedEmission.EmitOnHit(otherCollider.ClosestPointOnBounds(base.transform.position));
			}
		}
	}

	protected virtual void EnableTriggerCollider(Collider currentCollider)
	{
		currentCollider.isTrigger = true;
	}

	protected virtual void OnRotationEnd()
	{
		rotationDamageEnabled = false;
		rotationTrigger.enabled = false;
		isDelaying = false;
		isReturning = true;
		UnFreezePosition(base.CachedRigidbody);
		base.CachedRigidbody.velocity = delayingVelocity;
		AudioManager.StopAudioWithKey(characterHitSound);
		characterHitSound = string.Empty;
	}
}
