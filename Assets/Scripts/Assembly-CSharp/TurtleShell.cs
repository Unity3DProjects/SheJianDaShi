using UnityEngine;

public class TurtleShell : WeaponGeneral
{
	[SerializeField]
	private float multiplyGroundHit;

	[SerializeField]
	private float groundSpeed;

	[SerializeField]
	private float liftPosition;

	[SerializeField]
	private Collider mainCollider;

	private Character currentCharacter;

	private bool isActivate;

	private float direction;

	protected bool isRollSoundPlaying;

	protected float timeAboveGround;

	protected bool IsRollSoundPlaying
	{
		get
		{
			return isRollSoundPlaying;
		}
		set
		{
			if (isRollSoundPlaying != value)
			{
				isRollSoundPlaying = value;
				if (isRollSoundPlaying)
				{
					AudioManager.PlayAudioWithKey("turtle_shell_ground_roll1");
					AudioManager.PlayAudioWithKey("turtle_shell_ground_roll2");
					AudioManager.PlayAudioWithKey("turtle_shell_ground_roll3");
					AudioManager.PlayAudioWithKey("turtle_shell_ground_hit");
				}
				else
				{
					AudioManager.StopAudioWithKey("turtle_shell_ground_roll1");
					AudioManager.StopAudioWithKey("turtle_shell_ground_roll2");
					AudioManager.StopAudioWithKey("turtle_shell_ground_roll3");
					AudioManager.StopAudioWithKey("turtle_shell_ground_hit");
				}
			}
		}
	}

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		if (IsGroundCollision(collision))
		{
			timeAboveGround = 0f;
			IsRollSoundPlaying = true;
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (IsGroundCollision(collision))
		{
			timeAboveGround = 0f;
			if (collision.relativeVelocity.magnitude < 20f)
			{
				IsRollSoundPlaying = false;
			}
		}
	}

	protected override void OnTriggerEnter(Collider collider)
	{
		if (!isCollideEnabled)
		{
			return;
		}
		base.OnTriggerEnter(collider);
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = mainCollider;
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
				(collidable as Character).ApplyCharacterRagdoll();
				Vector3 force = new Vector3(0f, 2500f, 0f);
				Vector3 torque = new Vector3(0f, 0f, 5f);
				(collidable as Character).RootRigidBody.AddForce(force, ForceMode.VelocityChange);
				(collidable as Character).RootRigidBody.AddTorque(torque, ForceMode.VelocityChange);
				Collided(collisionInfo);
				break;
			}
			if (!isActivate)
			{
				Collided(collisionInfo);
				break;
			}
		}
	}

	private void OnDestroy()
	{
		if (SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			IsRollSoundPlaying = false;
		}
	}

	public override void PrepareBeforeShoot(Character shootCharacter)
	{
		base.PrepareBeforeShoot(shootCharacter);
		currentCharacter = shootCharacter;
		isActivate = false;
	}

	public override void Disabled(bool forced = false)
	{
		base.Disabled(forced);
		base.CachedRigidbody.isKinematic = true;
		if (colliders != null)
		{
			for (int i = 0; i < colliders.Count; i++)
			{
				colliders[i].isTrigger = true;
			}
		}
		IsRollSoundPlaying = false;
	}

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		timeAboveGround += deltaTime;
		if (timeAboveGround > 0.2f)
		{
			IsRollSoundPlaying = false;
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable is Character)
		{
			Character character = collidable as Character;
			Collider[] componentsInChildren = character.GetComponentsInChildren<Collider>();
			if (componentsInChildren != null && colliders != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					for (int j = 0; j < colliders.Count; j++)
					{
						Physics.IgnoreCollision(componentsInChildren[i], colliders[j]);
					}
				}
			}
		}
		else
		{
			isCollideEnabled = true;
		}
		return collidable;
	}

	protected override void OnHitActivation()
	{
		isActivate = true;
		direction = ((base.CachedTransform.position.x - currentCharacter.transform.position.x > 0f) ? 1 : (-1));
		base.CachedTransform.localEulerAngles = Vector3.zero;
	}

	protected bool IsGroundCollision(Collision coll)
	{
		ICollidable[] componentsInParent = coll.collider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Ground)
			{
				return true;
			}
		}
		return false;
	}

	protected override void UpdatePhysicalWeapon(float deltaTime)
	{
		if (!isActivate)
		{
			base.UpdatePhysicalWeapon(deltaTime);
		}
		else if (base.IsActive)
		{
			Vector3 vector = base.CachedRigidbody.velocity;
			float num = direction * groundSpeed;
			base.CachedRigidbody.AddForce(new Vector3(num - vector.x, 0f, 0f), ForceMode.VelocityChange);
			base.CachedTransform.localEulerAngles = Vector3.zero;
			base.CachedTransform.position += new Vector3(0f, liftPosition, 0f);
			RaycastHit hitInfo;
			base.CachedRigidbody.SweepTest(Vector3.down, out hitInfo);
			if (hitInfo.distance > 0f)
			{
				base.CachedTransform.position -= new Vector3(0f, hitInfo.distance, 0f);
			}
		}
	}
}
