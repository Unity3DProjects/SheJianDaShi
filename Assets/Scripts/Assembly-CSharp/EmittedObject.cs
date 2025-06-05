using System.Collections.Generic;
using UnityEngine;

public class EmittedObject : LevelObject
{
	[SerializeField]
	protected WeaponType weaponType;

	[SerializeField]
	protected PeriodicalDamageInfo damageInfo;

	[SerializeField]
	[ResourceLink]
	protected AssetLink emitEffectPrefab;

	[SerializeField]
	protected float emitRadius;

	[SerializeField]
	protected Collider objectCollider;

	protected Level level;

	protected bool isActivated;

	protected bool isNeedDestroy;

	private Rigidbody cachedRigidbody;

	private Transform cachedTransform;

	private float characterCollisionDelay = 0.1f;

	private float lifeTime;

	protected ParticleSystemHandler emitEffect;

	protected Rigidbody CachedRigidbody
	{
		get
		{
			if (cachedRigidbody == null)
			{
				cachedRigidbody = GetComponent<Rigidbody>();
			}
			return cachedRigidbody;
		}
	}

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	protected void OnCollisionEnter(Collision collision)
	{
		Collider collider = collision.collider;
		ICollidable componentInParent = collider.gameObject.GetComponentInParent<ICollidable>();
		Vector3 point = collision.contacts[0].point;
		if (componentInParent != null && !isActivated)
		{
			if (componentInParent is Ground)
			{
				OnGroundCollided(collider, point);
			}
			else if (componentInParent is CharacterBase && lifeTime > characterCollisionDelay)
			{
				isActivated = true;
				Activate((CharacterBase)componentInParent, collider);
				DestroyColliders();
			}
		}
	}

	protected void OnTriggerEnter(Collider collider)
	{
		ICollidable componentInParent = collider.gameObject.GetComponentInParent<ICollidable>();
		if (componentInParent != null && !isActivated && componentInParent is CharacterBase && lifeTime > characterCollisionDelay)
		{
			isActivated = true;
			Activate((CharacterBase)componentInParent, collider);
			DestroyColliders();
		}
	}

	public void Init(Level level, Vector3 velocity)
	{
		this.level = level;
		level.TurnActionsChangeCount(1);
		emitEffect = (emitEffectPrefab.GetInstance() as GameObject).GetComponent<ParticleSystemHandler>();
		emitEffect.transform.parent = CachedTransform;
		emitEffect.transform.localPosition = Vector3.zero;
		damageInfo.weaponType = weaponType;
		CachedRigidbody.velocity = velocity;
	}

	public virtual void UpdateObject(float deltaTime)
	{
		lifeTime += deltaTime;
		if (emitEffect != null)
		{
			if (!emitEffect.IsAlive() && isNeedDestroy)
			{
				level.RemoveEmittedObject(this);
			}
			else
			{
				AffectCharacters(CachedTransform.position, emitRadius);
			}
			if (lifeTime > 5f && !isActivated)
			{
				isActivated = true;
				DestroyColliders();
			}
		}
	}

	public virtual void RemoveObject()
	{
		if (!isNeedDestroy)
		{
			emitEffect.Stop();
			isNeedDestroy = true;
		}
		if (!isActivated)
		{
			level.TurnActionsChangeCount(-1);
		}
	}

	protected virtual void Activate(CharacterBase character, Collider collider)
	{
		if (isActivated && !isNeedDestroy)
		{
			character.DamageHandler.ApplyPeriodicalDamage(damageInfo, collider);
			emitEffect.Stop();
			isNeedDestroy = true;
		}
	}

	protected virtual void OnGroundCollided(Collider collider, Vector3 position)
	{
		CachedTransform.position = new Vector3(position.x, level.Location.GetPositionOnFloor(position.x).y);
		AffectAtPosition(position);
	}

	private void DestroyColliders()
	{
		if (objectCollider != null)
		{
			Object.Destroy(objectCollider);
			objectCollider = null;
		}
		if (CachedRigidbody != null)
		{
			Object.Destroy(CachedRigidbody);
		}
		level.TurnActionsChangeCount(-1);
	}

	private void AffectAtPosition(Vector2 position)
	{
		base.gameObject.layer = LayerMask.NameToLayer("Character");
		isActivated = true;
		AffectCharacters(position, emitRadius);
		DestroyColliders();
	}

	private void AffectCharacter(CharacterBase character, Vector3 position, float radius)
	{
		if (character != null && character.RootRigidBody != null)
		{
			List<Collider> list = character.AffectedObjects(position, radius);
			list.RemoveAll((Collider obj) => obj.gameObject.layer == LayerMask.NameToLayer("CharacterParts"));
			if (list.Count > 0)
			{
				Activate(character, list.RandomObject());
			}
		}
	}

	protected virtual void AffectCharacters(Vector3 position, float radius)
	{
		if (level.IsWeaponReleased)
		{
			AffectCharacter(level.ActiveCharacter, position, radius);
			AffectCharacter(level.InactiveArcher, position, radius);
		}
	}
}
