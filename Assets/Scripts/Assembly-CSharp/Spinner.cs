using System.Collections.Generic;
using UnityEngine;

public class Spinner : WeaponGeneral
{
	private enum SpinnerState
	{
		None = 0,
		InHand = 1,
		Flying = 2,
		BeforeActivation = 3,
		Activation = 4,
		AfterActivation = 5,
		Falling = 6,
		Fatality = 7
	}

	private const float BEFORE_ACTIVATION_RESULT_ANGLE_X = 90f;

	private const float EFFECTS_COLLIDER_RADIUS_MULTIPLIER = 0.7f;

	[SerializeField]
	private Collider meshCollider;

	[SerializeField]
	private SphereCollider collisionCollider;

	[SerializeField]
	private Transform meshTransform;

	[SerializeField]
	private Transform rotationXTransform;

	[SerializeField]
	private float startAngleY = 30f;

	[SerializeField]
	private float startAngleXMin = 15f;

	[SerializeField]
	private float startAngleXMax = 45f;

	[SerializeField]
	private float rotationSpeedXFlying = 500f;

	[SerializeField]
	private float rotationSpeedYFlying = 900f;

	[SerializeField]
	private float rotationSpeedYBeforeActivationChangeDuration = 0.2f;

	[SerializeField]
	private AnimationCurve rotationSpeedYBeforeActivationChangeCurve;

	[SerializeField]
	private float rotationSpeedYActivation = 1800f;

	[SerializeField]
	private float rotationSpeedYAfterActivationChangeDuration = 0.2f;

	[SerializeField]
	private AnimationCurve rotationSpeedYAfterActivationChangeCurve;

	[SerializeField]
	private float rotationSpeedYFalling = 360f;

	[SerializeField]
	private float activationRadius = 500f;

	[SerializeField]
	private float activationDuration = 2f;

	[SerializeField]
	private float activationFlySpeed = 500f;

	[SerializeField]
	private float damageRadius = 50f;

	[SerializeField]
	private float delayBetweenDamage = 0.2f;

	[SerializeField]
	private float damagePerRotationHit = 5f;

	[SerializeField]
	[ResourceLink]
	private AssetLink activationEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink rotateHitEffectPrefab;

	[SerializeField]
	private int maxEffectsPerHit = 5;

	[SerializeField]
	private float beforeActivationDuration = 0.1f;

	[SerializeField]
	private float afterActivationFallingDelay = 0.5f;

	[Header("Jolting")]
	[SerializeField]
	private float joltingDelay = 0.2f;

	[SerializeField]
	private float joltingRadius = 300f;

	[SerializeField]
	private float joltingOffset = 30f;

	private SpinnerState currentSpinnerState;

	private float sumXAngle;

	private float beforeActivationStartAngleX;

	private float currentBeforeActivationDuration;

	private float currentActivationDuration;

	private GameObject activationEffect;

	private Character affectedCharacter;

	private Collider affectedCharacterBodyCollider;

	private List<Rigidbody> affectedCharacterRigidbodies;

	private float currentDelayBetweenDamage;

	private float currentAfterActivationFallingDelay;

	private float currentRotationSpeedY;

	private bool isRotationSpeedYChanging;

	private AnimationCurve rotationSpeedYChangingCurve;

	private float beginRotationSpeedYChanging;

	private float endRotationSpeedYChanging;

	private float currentRotationSpeedYChangeDuration;

	private float totalRotationSpeedYChangeDuration;

	private float joltingTimer;

	private bool isJoltingForward;

	private bool isCollisionColliderEnabled;

	private List<GameObject> bloodEffects = new List<GameObject>();

	private int hitEffectsInCurrentFrame;

	private SpinnerState CurrentSpinnerState
	{
		get
		{
			return currentSpinnerState;
		}
		set
		{
			if (currentSpinnerState == value)
			{
				return;
			}
			currentSpinnerState = value;
			switch (currentSpinnerState)
			{
			case SpinnerState.InHand:
			{
				float x = Random.Range(startAngleXMin, startAngleXMax);
				meshTransform.localRotation = Quaternion.Euler(x, startAngleY, 0f);
				break;
			}
			case SpinnerState.Flying:
				meshTransform.rotation = Quaternion.identity;
				ChangeYRotationSpeed(rotationSpeedYFlying, 0f);
				break;
			case SpinnerState.BeforeActivation:
				base.CachedRigidbody.constraints |= RigidbodyConstraints.FreezePositionX;
				base.CachedRigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
				meshCollider.enabled = false;
				beforeActivationStartAngleX = sumXAngle % 360f;
				ChangeYRotationSpeed(rotationSpeedYActivation, rotationSpeedYBeforeActivationChangeDuration, rotationSpeedYBeforeActivationChangeCurve);
				break;
			case SpinnerState.Activation:
			{
				float sqrMagnitude = (level.CharacterFirst.BodyTransform.position - base.CachedTransform.position).sqrMagnitude;
				float sqrMagnitude2 = (level.CharacterSecond.BodyTransform.position - base.CachedTransform.position).sqrMagnitude;
				float num = Mathf.Min(sqrMagnitude, sqrMagnitude2);
				bool flag = true;
				if (num <= activationRadius * activationRadius)
				{
					affectedCharacter = ((!(sqrMagnitude < sqrMagnitude2)) ? level.CharacterSecond : level.CharacterFirst);
					foreach (Transform item in affectedCharacter.BodyTransform)
					{
						Collider component = item.GetComponent<Collider>();
						if (component != null)
						{
							affectedCharacterBodyCollider = component;
							break;
						}
					}
					if (affectedCharacter.IsFinishHim)
					{
						flag = false;
						CurrentSpinnerState = SpinnerState.Fatality;
					}
					else
					{
						IsCollisionColliderEnabled = true;
						affectedCharacter.ForceNotRestore = true;
						affectedCharacter.StartDefaultAnimation();
						affectedCharacter.SkeletonAnimation.State.ClearTrack(0);
						affectedCharacter.StartFaceDefaultAnimation();
						affectedCharacter.ApplyCharacterRagdoll(true);
						affectedCharacterRigidbodies = new List<Rigidbody>(affectedCharacter.RagdollRigidbodies);
						for (int j = 0; j < affectedCharacterRigidbodies.Count; j++)
						{
							if (affectedCharacterRigidbodies[j] != null)
							{
								affectedCharacterRigidbodies[j].interpolation = RigidbodyInterpolation.Extrapolate;
								affectedCharacterRigidbodies[j].useGravity = false;
							}
						}
					}
				}
				if (flag && activationEffectPrefab != null && !string.IsNullOrEmpty(activationEffectPrefab.assetGUID))
				{
					activationEffect = Object.Instantiate(activationEffectPrefab.GetAsset()) as GameObject;
					activationEffect.transform.parent = base.CachedTransform;
					activationEffect.transform.localPosition = Vector3.zero;
				}
				break;
			}
			case SpinnerState.AfterActivation:
				if (affectedCharacter != null)
				{
					for (int i = 0; i < affectedCharacterRigidbodies.Count; i++)
					{
						if (affectedCharacterRigidbodies[i] != null)
						{
							affectedCharacterRigidbodies[i].useGravity = true;
						}
					}
					IsCollisionColliderEnabled = false;
				}
				ChangeYRotationSpeed(rotationSpeedYFalling, rotationSpeedYAfterActivationChangeDuration, rotationSpeedYAfterActivationChangeCurve);
				AudioManager.StopAudioWithKey("spinner_activation_idle", 0.5f);
				AudioManager.PlayAudioWithKey("spinner_idle");
				break;
			case SpinnerState.Falling:
				if (activationEffect != null)
				{
					Object.Destroy(activationEffect);
				}
				base.CachedRigidbody.constraints &= (RigidbodyConstraints)(-3);
				base.CachedRigidbody.constraints &= (RigidbodyConstraints)(-5);
				meshCollider.enabled = true;
				break;
			case SpinnerState.Fatality:
				if (affectedCharacter != null)
				{
					CollisionInfo collisionInfo = new CollisionInfo();
					collisionInfo.position = base.CachedTransform.position;
					collisionInfo.otherCollider = meshCollider;
					collisionInfo.collider = affectedCharacterBodyCollider;
					collisionInfo.fatality = weaponType;
					collisionInfo.fatalityActvationType = FatalityActivationType.Explode;
					affectedCharacter.FatalityManager.StartFatality(collisionInfo);
					float resultYRotationSpeed = ((!(affectedCharacter.CachedTransform.position.x < collisionInfo.position.x)) ? rotationSpeedYActivation : (0f - rotationSpeedYActivation));
					ChangeYRotationSpeed(resultYRotationSpeed, 0f);
				}
				break;
			}
		}
	}

	private bool IsCollisionColliderEnabled
	{
		get
		{
			return isCollisionColliderEnabled;
		}
		set
		{
			if (isCollisionColliderEnabled != value)
			{
				isCollisionColliderEnabled = value;
				collisionCollider.enabled = isCollideEnabled;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CurrentSpinnerState = SpinnerState.InHand;
		isWeaponAfterCollisionAvailable = false;
		emitOnCollideMinVelocity = 0f;
		collisionCollider.radius = damageRadius * 0.7f;
		IsCollisionColliderEnabled = false;
	}

	public override void Disabled(bool forced = false)
	{
		if (affectedCharacter != null)
		{
			affectedCharacter.ForceNotRestore = false;
		}
		for (int i = 0; i < bloodEffects.Count; i++)
		{
			Object.Destroy(bloodEffects[i], 5f);
		}
		base.Disabled(forced);
	}

	public override void Shoot()
	{
		base.Shoot();
		CurrentSpinnerState = SpinnerState.Flying;
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = null;
		if (CurrentSpinnerState == SpinnerState.Flying)
		{
			ICollidable[] componentsInParent = collisionInfo.otherCollider.gameObject.GetComponentsInParent<ICollidable>(false);
			for (int i = 0; i < componentsInParent.Length; i++)
			{
				if (componentsInParent[i].OnCollision(collisionInfo))
				{
					collidable = componentsInParent[i];
					break;
				}
			}
			if (collidable is CharacterBase && (collidable as CharacterBase).IsFinishHim)
			{
				OnTapActivation();
				base.CachedTransform.position = collisionInfo.position;
				return collidable;
			}
		}
		if (CurrentSpinnerState == SpinnerState.Flying || CurrentSpinnerState == SpinnerState.Falling)
		{
			collidable = base.Collided(collisionInfo);
			if (CurrentSpinnerState == SpinnerState.Falling && collidable is CharacterBase && (collidable as CharacterBase).IsFinishHim)
			{
				OnTapActivation();
				base.CachedTransform.position = collisionInfo.position;
				return collidable;
			}
			if (collidable is CharacterBase || collidable is Ground)
			{
				if (collidable is Character && base.CachedEmission != null)
				{
					base.CachedEmission.EmitOnCollision(collisionInfo.otherCollider.transform.position);
					base.CachedEmission.EmitOnHit(collisionInfo.otherCollider.transform.position);
				}
				Disabled(true);
				level.RemoveWeapon(this);
			}
		}
		return collidable;
	}

	protected override void UpdatePhysicalWeapon(float deltaTime)
	{
		base.UpdatePhysicalWeapon(deltaTime);
		RotateSpinnerAtXAxis(deltaTime);
		RotateSpinnerAtYAxis(deltaTime);
		if (CurrentSpinnerState == SpinnerState.Activation)
		{
			currentActivationDuration += deltaTime;
			if (affectedCharacter != null)
			{
				currentDelayBetweenDamage += deltaTime;
				Vector3 vector = base.CachedTransform.position - affectedCharacter.BodyTransform.position;
				float magnitude = vector.magnitude;
				if (magnitude <= damageRadius && currentDelayBetweenDamage >= delayBetweenDamage)
				{
					HitCharacterByRotation();
					currentDelayBetweenDamage = 0f;
					hitEffectsInCurrentFrame = 0;
					IsCollisionColliderEnabled = true;
				}
				if (!level.IsMultiplayerSecondaryPlayer)
				{
					float f = Mathf.Atan2(vector.y, vector.x);
					float num = Mathf.Min(activationFlySpeed * deltaTime, magnitude - damageRadius * 0.8f);
					Vector3 vector2 = new Vector3(Mathf.Cos(f), Mathf.Sin(f)) * num;
					for (int i = 0; i < affectedCharacterRigidbodies.Count; i++)
					{
						Rigidbody rigidbody = affectedCharacterRigidbodies[i];
						if (rigidbody != null && i == 0)
						{
							rigidbody.position += vector2;
						}
					}
					UpdateJolting(deltaTime, magnitude);
				}
			}
			if (currentActivationDuration >= activationDuration)
			{
				CurrentSpinnerState = SpinnerState.AfterActivation;
			}
		}
		else if (CurrentSpinnerState == SpinnerState.AfterActivation && currentAfterActivationFallingDelay <= afterActivationFallingDelay)
		{
			currentAfterActivationFallingDelay += deltaTime;
			if (currentAfterActivationFallingDelay > afterActivationFallingDelay)
			{
				CurrentSpinnerState = SpinnerState.Falling;
			}
		}
	}

	protected override void OnTriggerEnter(Collider collider)
	{
		base.OnTriggerEnter(collider);
		if (!(affectedCharacter != null) || hitEffectsInCurrentFrame > maxEffectsPerHit)
		{
			return;
		}
		SkeletonUtilityBone component = collider.transform.parent.GetComponent<SkeletonUtilityBone>();
		if (!(component != null))
		{
			return;
		}
		IsCollisionColliderEnabled = false;
		Vector3 vector = collider.ClosestPointOnBounds(base.CachedTransform.position);
		Vector3 vector2 = vector - base.CachedTransform.position;
		vector2 = new Vector3(0f - vector2.y, vector2.x, 0f);
		Vector3 euler = Vector3.forward * (Mathf.Atan2(vector2.y, vector2.x) * 57.29578f);
		ICollidable[] componentsInParent = collider.GetComponentsInParent<ICollidable>();
		foreach (ICollidable collidable in componentsInParent)
		{
			if (collidable is Character)
			{
				ApplySpinnerDamageAndDecals(collidable as Character, 0f, collider);
				if (rotateHitEffectPrefab != null && !string.IsNullOrEmpty(rotateHitEffectPrefab.assetGUID))
				{
					GameObject gameObject = Object.Instantiate(rotateHitEffectPrefab.GetAsset()) as GameObject;
					gameObject.transform.parent = collider.transform;
					gameObject.transform.position = vector;
					gameObject.transform.rotation = Quaternion.Euler(euler);
					bloodEffects.Add(gameObject);
					hitEffectsInCurrentFrame++;
				}
				AudioManager.PlayAudioWithKey("spinner_hit_character");
				break;
			}
		}
	}

	protected override void OnTapActivation()
	{
		base.OnTapActivation();
		if (base.CachedEmission != null)
		{
			base.CachedEmission.StopTrails();
		}
		CurrentSpinnerState = SpinnerState.BeforeActivation;
	}

	private void UpdateJolting(float deltaTime, float distanceToCharacter)
	{
		if (CurrentSpinnerState != SpinnerState.Activation || !(affectedCharacter != null) || !(distanceToCharacter <= joltingRadius))
		{
			return;
		}
		joltingTimer += deltaTime;
		if (!(joltingTimer > joltingDelay))
		{
			return;
		}
		Vector3 vector = affectedCharacter.RootRigidBody.transform.TransformDirection(Vector3.up).normalized * joltingOffset;
		vector *= ((!isJoltingForward) ? (-1f) : 1f);
		for (int i = 0; i < affectedCharacterRigidbodies.Count; i++)
		{
			if (affectedCharacterRigidbodies[i] != null)
			{
				affectedCharacterRigidbodies[i].position += vector;
			}
		}
		isJoltingForward = !isJoltingForward;
		joltingTimer = 0f;
	}

	private void RotateSpinnerAtXAxis(float deltaTime)
	{
		if (CurrentSpinnerState == SpinnerState.Flying)
		{
			float num = rotationSpeedXFlying * deltaTime;
			sumXAngle += num;
			rotationXTransform.localRotation *= Quaternion.Euler(num, 0f, 0f);
		}
		else if (CurrentSpinnerState == SpinnerState.BeforeActivation && currentBeforeActivationDuration < beforeActivationDuration)
		{
			currentBeforeActivationDuration += deltaTime;
			float num2 = Mathf.Clamp01(currentBeforeActivationDuration / beforeActivationDuration);
			float x = beforeActivationStartAngleX + (90f - beforeActivationStartAngleX) * num2;
			rotationXTransform.localRotation = Quaternion.Euler(x, 0f, 0f);
			if (currentBeforeActivationDuration >= beforeActivationDuration)
			{
				CurrentSpinnerState = SpinnerState.Activation;
			}
		}
	}

	private void RotateSpinnerAtYAxis(float deltaTime)
	{
		if (isRotationSpeedYChanging)
		{
			currentRotationSpeedYChangeDuration += deltaTime;
			if (currentRotationSpeedYChangeDuration >= totalRotationSpeedYChangeDuration)
			{
				currentRotationSpeedY = endRotationSpeedYChanging;
				isRotationSpeedYChanging = false;
			}
			else
			{
				float num = Mathf.Clamp01(currentRotationSpeedYChangeDuration / totalRotationSpeedYChangeDuration);
				if (rotationSpeedYChangingCurve != null)
				{
					num = rotationSpeedYChangingCurve.Evaluate(num);
				}
				currentRotationSpeedY = beginRotationSpeedYChanging + (endRotationSpeedYChanging - beginRotationSpeedYChanging) * num;
			}
		}
		meshTransform.localRotation *= Quaternion.Euler(0f, currentRotationSpeedY * deltaTime, 0f);
	}

	private void ChangeYRotationSpeed(float resultYRotationSpeed, float duration, AnimationCurve curve = null)
	{
		isRotationSpeedYChanging = true;
		currentRotationSpeedYChangeDuration = 0f;
		beginRotationSpeedYChanging = currentRotationSpeedY;
		endRotationSpeedYChanging = resultYRotationSpeed;
		totalRotationSpeedYChangeDuration = duration;
		rotationSpeedYChangingCurve = curve;
	}

	private void HitCharacterByRotation()
	{
		if (!(affectedCharacter != null))
		{
			return;
		}
		ICollidable[] componentsInParent = affectedCharacterBodyCollider.GetComponentsInParent<ICollidable>();
		foreach (ICollidable collidable in componentsInParent)
		{
			if (collidable is Character)
			{
				ApplySpinnerDamageAndDecals(collidable as Character, damagePerRotationHit, affectedCharacterBodyCollider);
				break;
			}
		}
	}

	private void ApplySpinnerDamageAndDecals(Character character, float damage, Collider damagedCollider)
	{
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = collisionCollider;
		collisionInfo.otherCollider = damagedCollider;
		collisionInfo.position = damagedCollider.ClosestPointOnBounds(base.CachedTransform.position);
		collisionInfo.damage = damage;
		collisionInfo.decalType = decalType;
		collisionInfo.fatality = base.WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
		collisionInfo.isDisableRestore = true;
		SkeletonUtilityBone component = damagedCollider.transform.parent.GetComponent<SkeletonUtilityBone>();
		if (component != null)
		{
			collisionInfo.boneName = component.boneName;
		}
		character.ApplyDamage(collisionInfo);
	}
}
