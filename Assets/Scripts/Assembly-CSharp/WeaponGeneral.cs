using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGeneral : LevelObject, ICollidable
{
	public const float GRAVITY_ACCELERATION = 981f;

	public const float MULTIPLAYER_CLIENT_ACTIVATION_DELAY = 0.25f;

	[SerializeField]
	protected string weaponName;

	[SerializeField]
	protected WeaponType weaponType;

	[SerializeField]
	protected WeaponLaunchType launchType;

	[SerializeField]
	protected TutorialType tutorialType = TutorialType.Aiming;

	[SerializeField]
	protected FatalityActivationType fatalityActivationType;

	[SerializeField]
	protected Vector2 anchorPoint = new Vector2(0.5f, 0.5f);

	[SerializeField]
	protected Vector2 anchorPointInPixels = new Vector2(0f, 0f);

	[SerializeField]
	protected float boneAngle;

	[SerializeField]
	protected bool isPhysical;

	[SerializeField]
	protected float angularVelocity;

	[SerializeField]
	protected bool isRotateFromDirection;

	[SerializeField]
	protected int gravityRatio = 100;

	[SerializeField]
	protected float maxSpeed = 1600f;

	[SerializeField]
	protected float lifeTime = float.PositiveInfinity;

	[SerializeField]
	protected PeriodicalDamageInfo periodicalDamageInfo;

	[SerializeField]
	protected bool isAccelerationEnabled;

	[SerializeField]
	protected float acceleration;

	[SerializeField]
	protected bool ignoreEmissionAngleForRotation;

	[SerializeField]
	protected MeshFilter weaponMesh;

	[SerializeField]
	[Range(0f, 1f)]
	protected float bounciness;

	[SerializeField]
	protected DecalType decalType;

	[SerializeField]
	protected bool isUnfollowedAfterActivation;

	[SerializeField]
	protected bool isDisabledAfterCollision;

	[SerializeField]
	protected float unfollowDuration = 6.5f;

	[SerializeField]
	protected string createSound = "weapon_create";

	[SerializeField]
	protected string idleSound;

	[SerializeField]
	protected string shootSound = "weapon_shoot";

	[SerializeField]
	protected string flyingSound;

	[SerializeField]
	protected string activationSound;

	[SerializeField]
	protected string activationFlyingSound;

	[SerializeField]
	protected string groundHitSound;

	[SerializeField]
	protected string groundPierceSound;

	[SerializeField]
	protected string characterHitSound;

	[SerializeField]
	protected string characterPierceSound;

	[SerializeField]
	protected bool isRotatingFlyingSound;

	[SerializeField]
	protected float whooshDelayBetweenSounds = 0.25f;

	[SerializeField]
	protected float whooshMinVelocity = 6.5f;

	[SerializeField]
	protected float whooshMaxVelocity = 35f;

	[SerializeField]
	private bool isOneTimeActivated;

	protected float emitOnCollideMinVelocity = 500f;

	protected bool isWeaponVisible;

	protected bool isRotatingSoundPlaying;

	private Transform cachedTransform;

	private Rigidbody cachedRigidbody;

	protected Vector2 anchorPointOffset = Vector2.zero;

	protected List<Collider> colliders;

	protected List<WeaponActivation> activations;

	protected List<WeaponActivation> removedActivations = new List<WeaponActivation>();

	protected bool autoActivationEnabled;

	protected ShootType shootType;

	protected List<AutoActivation> autoActivations;

	protected List<AutoActivation> removedAutoActivations = new List<AutoActivation>();

	protected Vector3 previousPosition;

	protected Vector3 velocity;

	protected TrailVertex trail;

	protected bool isCollideEnabled;

	protected GameObject idleEffect;

	protected float currentLifeTime;

	protected float disableSqrVelocityBorder = 20f;

	protected float disableAngularVelocityBorder = 0.1f;

	protected bool isWeaponAfterCollisionAvailable = true;

	internal Level level;

	internal bool isActive;

	internal bool isActivated;

	internal bool isFlying;

	internal float startVelocity;

	internal float emissionAngle;

	internal Emission cachedEmission;

	internal float whooshDelay;

	private float disableCheckTimeout;

	protected float damagePercent = 1f;

	protected bool isMultipleTimeActivated = true;

	private bool isEmittedWeapon;

	private CollisionInfo tempCollisionInfo = new CollisionInfo();

	private List<ICollidable> tempList = new List<ICollidable>();

	public float StartVelocity
	{
		get
		{
			return startVelocity;
		}
	}

	public float EmissionAngle
	{
		get
		{
			return emissionAngle;
		}
	}

	public WeaponType WeaponType
	{
		get
		{
			return weaponType;
		}
	}

	public TutorialType TutorialType
	{
		get
		{
			return tutorialType;
		}
	}

	public WeaponLaunchType LaunchType
	{
		get
		{
			return launchType;
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

	internal Rigidbody CachedRigidbody
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

	public bool IsActive
	{
		get
		{
			return isActive;
		}
		protected set
		{
			if (isActive != value)
			{
				isActive = value;
				if (isActive)
				{
					level.TurnActionsChangeCount(1);
				}
				else
				{
					level.TurnActionsChangeCount(-1);
				}
			}
		}
	}

	public List<Collider> Colliders
	{
		get
		{
			if (colliders == null)
			{
				colliders = new List<Collider>(GetComponentsInChildren<Collider>());
			}
			return colliders;
		}
	}

	public Vector2 AnchorPointOffset
	{
		get
		{
			return anchorPointOffset;
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return velocity;
		}
		set
		{
			if (velocity != value)
			{
				velocity = value;
			}
		}
	}

	public bool IsPhysical
	{
		get
		{
			return isPhysical;
		}
	}

	public float Gravity
	{
		get
		{
			return 981f * (float)gravityRatio / 100f;
		}
	}

	public float MaxSpeed
	{
		get
		{
			return maxSpeed;
		}
		private set
		{
			if (maxSpeed != value)
			{
				maxSpeed = value;
			}
		}
	}

	public float Bounciness
	{
		get
		{
			return bounciness;
		}
	}

	public Level Level
	{
		get
		{
			return level;
		}
	}

	public float BoneAngle
	{
		get
		{
			return boneAngle;
		}
	}

	public Emission CachedEmission
	{
		get
		{
			if (cachedEmission == null)
			{
				cachedEmission = GetComponent<Emission>();
			}
			return cachedEmission;
		}
	}

	public virtual bool IsWeaponVisible
	{
		get
		{
			return isWeaponVisible;
		}
		set
		{
			if (isWeaponVisible == value)
			{
				return;
			}
			isWeaponVisible = value;
			if (isWeaponVisible)
			{
				SetIdleEffectEnabled(true);
				AudioManager.PlayAudioWithKey(createSound);
				AudioManager.PlayAudioWithKey(idleSound);
				if (!Level.ActiveCharacter.IsAI && !Level.ActiveCharacter.IsMultiplayerEnemy)
				{
					SingletonMonoBehaviour<TutorialManager>.Instance.ShowFirstTutorialAnimation(tutorialType);
				}
			}
			else
			{
				AudioManager.StopAudioWithKey(idleSound);
			}
		}
	}

	protected List<WeaponActivation> Activations
	{
		get
		{
			if (activations == null)
			{
				activations = new List<WeaponActivation>();
				GetComponents(activations);
			}
			return activations;
		}
	}

	protected List<AutoActivation> AutoActivations
	{
		get
		{
			if (autoActivations == null)
			{
				autoActivations = new List<AutoActivation>();
				GetComponents(autoActivations);
			}
			return autoActivations;
		}
	}

	protected GameObject IdleEffect
	{
		get
		{
			if (idleEffect == null && (bool)CachedEmission)
			{
				idleEffect = CachedEmission.StartIdleEffect(CachedTransform, CachedTransform.position, velocity);
			}
			return idleEffect;
		}
	}

	public bool IsRotateFromDirection
	{
		get
		{
			return isRotateFromDirection;
		}
		set
		{
			isRotateFromDirection = value;
		}
	}

	public float DamagePercent
	{
		get
		{
			return damagePercent;
		}
		set
		{
			if (damagePercent != value)
			{
				damagePercent = value;
			}
		}
	}

	public bool IsEmittedWeapon
	{
		get
		{
			return isEmittedWeapon;
		}
		set
		{
			if (isEmittedWeapon != value)
			{
				isEmittedWeapon = value;
			}
		}
	}

	protected virtual void Awake()
	{
		if (tk2dSystem.IsRetina)
		{
			tk2dSprite[] componentsInChildren = GetComponentsInChildren<tk2dSprite>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ForceBuild();
				componentsInChildren[i].scale *= 0.5f;
			}
		}
		if ((bool)weaponMesh)
		{
			anchorPointOffset = new Vector2((anchorPoint.x - 0.5f) * weaponMesh.mesh.bounds.size.x, (anchorPoint.y - 0.5f) * weaponMesh.mesh.bounds.size.y);
		}
		else
		{
			anchorPointOffset = anchorPointInPixels;
		}
		IsActive = false;
		isCollideEnabled = false;
		isFlying = false;
		SetCollidersEnabled(false);
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (isCollideEnabled)
		{
			string boneName = null;
			SkeletonUtilityBone component = collision.collider.transform.parent.GetComponent<SkeletonUtilityBone>();
			if ((bool)component)
			{
				boneName = component.boneName;
			}
			ContactPoint contactPoint = collision.contacts[0];
			CollisionInfo collisionInfo = tempCollisionInfo;
			collisionInfo.collider = contactPoint.thisCollider;
			collisionInfo.otherCollider = contactPoint.otherCollider;
			collisionInfo.position = contactPoint.point;
			collisionInfo.direction = Velocity.normalized;
			collisionInfo.decalType = decalType;
			collisionInfo.boneName = boneName;
			collisionInfo.fatality = WeaponType;
			collisionInfo.fatalityActvationType = fatalityActivationType;
			collisionInfo.speedRatio = Velocity.magnitude / maxSpeed;
			Collided(collisionInfo);
		}
	}

	private void OnDestroy()
	{
		StopAllSounds();
	}

	public virtual void Init(Level level)
	{
		this.level = level;
		if (bounciness > 0f)
		{
			foreach (Collider collider in Colliders)
			{
				collider.material.bounciness = bounciness;
			}
		}
		periodicalDamageInfo.weaponType = weaponType;
	}

	public virtual void SetShootingInfo(Vector3 position, ShootingInfo shootingInfo, bool ignoreCharacterTransform = false)
	{
		autoActivationEnabled = shootingInfo.autoActivation;
		shootType = shootingInfo.type;
		previousPosition = position;
		startVelocity = shootingInfo.velocity;
		emissionAngle = shootingInfo.angle;
		if (!ignoreCharacterTransform && level.ActiveCharacter.IsFlipped)
		{
			emissionAngle = 180f - emissionAngle;
		}
		velocity = new Vector3(Mathf.Cos(shootingInfo.angle * ((float)Math.PI / 180f)) * shootingInfo.velocity, Mathf.Sin(shootingInfo.angle * ((float)Math.PI / 180f)) * shootingInfo.velocity);
		CachedTransform.position = previousPosition;
		CachedTransform.localPosition += (Vector3)anchorPointOffset;
		if (ignoreCharacterTransform)
		{
			CachedTransform.rotation = Quaternion.identity;
		}
	}

	public virtual void PrepareBeforeShoot(Character shootCharacter)
	{
		float num = ((!ignoreEmissionAngleForRotation) ? emissionAngle : 0f);
		if (shootCharacter.IsFlipped)
		{
			num = 180f - num;
		}
		if (CachedTransform != null)
		{
			CachedTransform.position = shootCharacter.WeaponAnchorTransform.position;
			CachedTransform.localPosition += (Vector3)anchorPointOffset;
			CachedTransform.rotation = Quaternion.Euler(0f, 0f, num);
			previousPosition = CachedTransform.position;
			velocity = new Vector3(Mathf.Cos(emissionAngle * ((float)Math.PI / 180f)) * startVelocity, Mathf.Sin(emissionAngle * ((float)Math.PI / 180f)) * startVelocity);
			if (shootCharacter.IsFlipped)
			{
				CachedTransform.localRotation = Quaternion.Euler(0f, 0f, 180f - CachedTransform.localRotation.eulerAngles.z);
			}
		}
		shootCharacter.IsActive = false;
		level.WeaponAlreadyReleased();
		level.WeaponShoot(this, shootCharacter);
	}

	public virtual void Shoot()
	{
		if (autoActivationEnabled)
		{
			foreach (AutoActivation autoActivation in AutoActivations)
			{
				if (autoActivation.AutoActivationEnabled(shootType))
				{
					autoActivation.CalculateActivationTime(this, level, previousPosition, level.InactiveArcher.transform.position);
				}
			}
		}
		if ((bool)CachedEmission)
		{
			CachedEmission.EmitOnShoot(CachedTransform.position);
			CachedEmission.StartTrails(CachedTransform, CachedTransform.position, velocity);
			if (IdleEffect != null)
			{
				IdleEffect.StopFX(false);
				SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
				{
					IdleEffect.PlayFX();
				}, 0f);
			}
		}
		SetCollidersEnabled(true);
		IsActive = true;
		isFlying = true;
		isCollideEnabled = true;
		if (IsPhysical)
		{
			CachedRigidbody.isKinematic = false;
			CachedRigidbody.velocity = velocity;
			CachedRigidbody.maxAngularVelocity = Mathf.Abs(angularVelocity);
			float z = ((!(velocity.x > 0f)) ? angularVelocity : (0f - angularVelocity));
			CachedRigidbody.angularVelocity = new Vector3(0f, 0f, z);
		}
		AudioManager.PlayAudioWithKey(shootSound);
		if (!isRotatingSoundPlaying)
		{
			AudioManager.PlayAudioWithKey(flyingSound);
		}
		SingletonMonoBehaviour<TutorialManager>.Instance.WeaponShoot();
	}

	public virtual void UpdateWeapon(float deltaTime)
	{
		if (lifeTime != float.PositiveInfinity)
		{
			currentLifeTime += deltaTime;
			if (currentLifeTime >= lifeTime)
			{
				currentLifeTime = 0f;
				Disabled(true);
				level.RemoveWeapon(this);
				return;
			}
		}
		if (isFlying && !level.ActiveCharacter.ShootBounds.Contains(CachedTransform.position) && !level.ActiveCharacter.PartsCollidersEnabled && level.CurrentWeapon == this)
		{
			level.ActiveCharacter.SetCollidersEnabled(true);
		}
		if (IsPhysical)
		{
			UpdatePhysicalWeapon(deltaTime);
		}
		else
		{
			UpdateNonPhysicalWeapon(deltaTime);
		}
		if (IsActive)
		{
			UpdatePreviousPosition();
		}
		if (!level.Bounds.Contains(CachedTransform.position))
		{
			Disabled(true);
			level.RemoveWeapon(this);
			level.WeaponCollidedWithObject(null);
			level.MissesCount++;
		}
		if (CachedTransform.lossyScale.x < 0.9f || CachedTransform.lossyScale.y < 0.9f)
		{
			SetIdleEffectEnabled(false);
		}
		else
		{
			SetIdleEffectEnabled(true);
		}
		UpdateActivations(deltaTime);
		UpdateRotationSound(deltaTime);
	}

	public virtual ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = null;
		tempList.Clear();
		collisionInfo.otherCollider.gameObject.GetComponentsInParent(false, tempList);
		ICollidable[] array = tempList.ToArray();
		ICollidable[] array2 = array;
		foreach (ICollidable collidable2 in array2)
		{
			if (collidable2.OnCollision(collisionInfo))
			{
				collidable = collidable2;
				break;
			}
		}
		if (collidable != null && !(collidable is PowerUp))
		{
			if (CachedEmission != null && !(collidable is CharacterBase) && !(collidable is Bird) && Velocity.magnitude > emitOnCollideMinVelocity)
			{
				CachedEmission.EmitOnCollision(collisionInfo.position);
			}
			if (isDisabledAfterCollision && !(collidable is PowerUp))
			{
				isFlying = false;
			}
			level.WeaponCollidedWithObject(collisionInfo.otherCollider.gameObject);
			if (IsShotMissedForCollidedObject(collidable))
			{
				level.MissesCount++;
			}
			if (collidable is CharacterBase && (level.GameType == GameType.VSComputer || level.GameType == GameType.Tournament) && level.ActiveCharacter == level.CharacterFirst && (CharacterBase)collidable != level.CharacterFirst)
			{
				BodyPartsHelper.BodyPartsType bodyPartsType = BodyPartsHelper.TypeByName(collisionInfo.boneName);
				if (bodyPartsType == BodyPartsHelper.BodyPartsType.BodyPartsTypeHead)
				{
					SingletonMonoBehaviour<MicroachievementsManager>.Instance.CheckLevelAchievementReaching(MicroachievementType.Headshot);
				}
			}
			if (collidable is CharacterBase || collidable is Bird)
			{
				isCollideEnabled = false;
			}
			if (!level.IsMultiplayerSecondaryPlayer && (collidable is Bird || (collidable is CharacterBase && !(collidable as CharacterBase).IsFinishHim)) && collisionInfo.collider != null)
			{
				WeaponDamage[] componentsInChildren = collisionInfo.collider.GetComponentsInChildren<WeaponDamage>();
				WeaponDamage[] array3 = componentsInChildren;
				foreach (WeaponDamage weaponDamage in array3)
				{
					weaponDamage.OnCollide(this, collisionInfo, collidable);
				}
			}
			if (collisionInfo.collider != null)
			{
				WeaponCollider[] componentsInChildren2 = collisionInfo.collider.GetComponentsInChildren<WeaponCollider>();
				WeaponCollider[] array4 = componentsInChildren2;
				foreach (WeaponCollider weaponCollider in array4)
				{
					if (weaponCollider is WeaponDriveIntoCollider)
					{
						if (collidable is CharacterBase || collidable is Bird)
						{
							AudioManager.PlayAudioWithKey(characterPierceSound);
						}
						else if (collidable is Ground)
						{
							AudioManager.PlayAudioWithKey(groundPierceSound);
						}
					}
					else if (collidable is CharacterBase)
					{
						AudioManager.PlayAudioWithKey(characterHitSound);
					}
					else if (collidable is Ground)
					{
						AudioManager.PlayAudioWithKey(groundHitSound);
					}
					weaponCollider.OnCollide(this, collisionInfo, collidable);
				}
			}
			if (level.GameType == GameType.Multiplayer && collidable is CharacterBase)
			{
				WeaponParams weaponParams = new WeaponParams();
				weaponParams.weaponPosition = base.transform.position;
				weaponParams.weaponRotation = base.transform.eulerAngles;
				int weaponColliderIndex = Colliders.FindIndex((Collider collider) => collider == collisionInfo.collider);
				int characterColliderIndex = (collidable as CharacterBase).PartsColliders.FindIndex((Collider collider) => collider == collisionInfo.otherCollider);
				BodyPartsHelper.BodyPartsType bodyPartsType2 = BodyPartsHelper.TypeByName(collisionInfo.boneName);
				bool isDriveIntoLegs = bodyPartsType2 == BodyPartsHelper.BodyPartsType.BodyPartsTypeLegs;
				(level as LevelMultiplayer).WeaponCollideCharacter(collidable as CharacterBase, weaponParams, weaponColliderIndex, characterColliderIndex, isDriveIntoLegs);
				if ((bool)(collidable as CharacterBase).masksManager)
				{
					(collidable as CharacterBase).masksManager.ApplyDamage(collisionInfo);
				}
			}
		}
		if (isMultipleTimeActivated)
		{
			Activate(WeaponActivationType.None, collidable);
			if (isOneTimeActivated)
			{
				isMultipleTimeActivated = false;
			}
		}
		return collidable;
	}

	public virtual void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		foreach (Collider collider in Colliders)
		{
			collider.enabled = false;
		}
		if (collidedObject is CharacterBase)
		{
			((CharacterBase)collidedObject).AddDriveIntoWeaponColliders(Colliders);
			BodyPartsHelper.BodyPartsType bodyPartsType = BodyPartsHelper.TypeByName(collisionInfo.boneName);
			if (bodyPartsType == BodyPartsHelper.BodyPartsType.BodyPartsTypeLegs)
			{
				((CharacterBase)collidedObject).AddDriveIntoLegsWeapon(this);
			}
			else
			{
				((CharacterBase)collidedObject).AddDriveIntoWeapon(this);
			}
		}
		CachedTransform.position = previousPosition;
		CachedRigidbody.isKinematic = true;
		CachedTransform.parent = collisionInfo.otherCollider.transform;
		if (isRotateFromDirection)
		{
			CachedTransform.right = velocity;
		}
		ChangeSortingLayerTo("UnderCharacter");
		Vector3 vector = collisionInfo.otherCollider.transform.InverseTransformVector(Velocity);
		if (level.GameType != GameType.Multiplayer || !level.IsMultiplayerSecondaryPlayer || collidedObject is Ground)
		{
			float num = Mathf.Min(velocity.magnitude / maxSpeed, 1f) * depth;
			CachedTransform.localPosition += vector.normalized * num;
		}
		if (isWeaponAfterCollisionAvailable)
		{
			Disabled();
		}
	}

	public virtual void Disabled(bool forced = false)
	{
		SingletonMonoBehaviour<TutorialManager>.Instance.WeaponWasDisabled(this);
		isCollideEnabled = false;
		isFlying = false;
		if (CachedEmission != null)
		{
			CachedEmission.StopTrails();
		}
		if (forced || lifeTime == float.PositiveInfinity)
		{
			IsActive = false;
		}
		if (CachedRigidbody != null)
		{
			CachedRigidbody.isKinematic = true;
		}
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		Collider[] array = componentsInChildren;
		foreach (Collider collider in array)
		{
			collider.enabled = false;
		}
		StopAllSounds();
	}

	public virtual void RemoveFromCharacter()
	{
		CachedTransform.parent = level.transform;
		if (CachedRigidbody != null)
		{
			CachedRigidbody.isKinematic = false;
			CachedRigidbody.useGravity = true;
			CachedRigidbody.velocity = (CachedTransform.position - CachedTransform.parent.position).normalized * UnityEngine.Random.Range(100f, 200f);
			CachedRigidbody.maxAngularVelocity = UnityEngine.Random.Range(5f, 10f);
			CachedRigidbody.angularVelocity = new Vector3(0f, 0f, CachedRigidbody.maxAngularVelocity);
		}
		base.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
		level.RemoveWeapon(this);
	}

	public void ForceWeaponParams(WeaponParams weaponParams)
	{
		base.transform.position = weaponParams.weaponPosition;
		base.transform.rotation = Quaternion.Euler(weaponParams.weaponRotation);
	}

	public void ForceWeaponCollide(WeaponParams weaponParams, int weaponColliderIndex, int characterColliderIndex, bool isDriveIntoLegs, CharacterBase character)
	{
		ForceWeaponParams(weaponParams);
		UpdatePreviousPosition();
		CollisionInfo collisionInfo = new CollisionInfo();
		if (Colliders.Count > 0)
		{
			collisionInfo.collider = ((Colliders.Count <= weaponColliderIndex) ? Colliders[0] : Colliders[weaponColliderIndex]);
		}
		else
		{
			collisionInfo.collider = null;
		}
		if (character.PartsColliders.Count > 0)
		{
			collisionInfo.otherCollider = ((character.PartsColliders.Count <= characterColliderIndex) ? character.PartsColliders[0] : character.PartsColliders[characterColliderIndex]);
		}
		else
		{
			collisionInfo.otherCollider = null;
		}
		collisionInfo.position = weaponParams.weaponPosition;
		collisionInfo.direction = Velocity.normalized;
		collisionInfo.fatality = WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = Velocity.magnitude / maxSpeed;
		collisionInfo.decalType = decalType;
		collisionInfo.boneName = ((!isDriveIntoLegs) ? "body" : "thigh");
		if (collisionInfo.collider != null || collisionInfo.otherCollider != null)
		{
			Collided(collisionInfo);
		}
	}

	public void ForceGroundCollide(WeaponParams weaponParams)
	{
		ForceWeaponParams(weaponParams);
		Disabled();
	}

	public void ForceMaxSpeed(float newSpeed)
	{
		maxSpeed = newSpeed;
	}

	private void UpdateActivations(float deltaTime)
	{
		int i = 0;
		for (int count = Activations.Count; i < count; i++)
		{
			Activations[i].UpdateActivation(deltaTime, this, WeaponActivationType.Timer, null);
		}
		int count2 = removedActivations.Count;
		if (count2 > 0)
		{
			for (int j = 0; j < count2; j++)
			{
				WeaponActivation weaponActivation = removedActivations[j];
				Activations.Remove(weaponActivation);
				UnityEngine.Object.Destroy(weaponActivation);
			}
			removedActivations.Clear();
		}
		if (!autoActivationEnabled)
		{
			return;
		}
		int k = 0;
		for (int count3 = AutoActivations.Count; k < count3; k++)
		{
			AutoActivation autoActivation = AutoActivations[k];
			if (autoActivation != null && autoActivation.IsNeedActivate(CachedTransform.position, level.InactiveArcher.CachedTransform.position, deltaTime) && Activations.Count > 0)
			{
				Activate(Activations[0].Type, null);
				removedAutoActivations.Add(autoActivation);
			}
		}
		int l = 0;
		for (int count4 = removedAutoActivations.Count; l < count4; l++)
		{
			AutoActivations.Remove(removedAutoActivations[l]);
		}
		removedAutoActivations.Clear();
	}

	public virtual void Activate(WeaponActivationType type, ICollidable collidedObject)
	{
		if (level.IsMultiplayerSecondaryPlayer)
		{
			IEnumerator enumerator = ActivationCoroutine(type, collidedObject, true);
			enumerator.MoveNext();
			StartCoroutine(enumerator);
		}
		else
		{
			ActivationCoroutine(type, collidedObject, false).MoveNext();
		}
	}

	public void MultiplayerActivate()
	{
		IEnumerator enumerator = ActivationCoroutine(WeaponActivationType.Tap, null, false);
		enumerator.MoveNext();
		StartCoroutine(enumerator);
	}

	public void RemoveAllActivations()
	{
		removedActivations.AddRange(Activations);
	}

	public void RemoveActivation(WeaponActivation activation)
	{
		removedActivations.Add(activation);
	}

	public void AddActivation(WeaponActivation activation)
	{
		Activations.Add(activation);
	}

	protected virtual void OnTapActivation()
	{
	}

	protected virtual void OnHitActivation()
	{
	}

	protected virtual void OnTimerFinished()
	{
	}

	protected virtual void EmitObjects()
	{
		if (CachedEmission != null)
		{
			CachedEmission.EmitObjects(level, CachedTransform.position, Mathf.Atan2(CachedRigidbody.velocity.y, CachedRigidbody.velocity.x) * 57.29578f, CachedRigidbody.velocity.magnitude);
		}
		Disabled();
		level.RemoveWeapon(this);
	}

	protected virtual void UpdatePreviousPosition()
	{
		previousPosition = CachedTransform.position;
	}

	protected virtual void SetCollidersEnabled(bool isEnabled)
	{
		for (int i = 0; i < Colliders.Count; i++)
		{
			Colliders[i].enabled = isEnabled;
		}
	}

	protected virtual void UpdateNonPhysicalWeapon(float deltaTime)
	{
		if (isFlying)
		{
			Vector3 vector = Vector3.zero;
			if (isAccelerationEnabled)
			{
				Vector3 normalized = velocity.normalized;
				normalized *= acceleration * deltaTime;
				normalized.z = 0f;
				vector = normalized;
			}
			velocity = new Vector3(velocity.x + vector.x, velocity.y - Gravity * deltaTime + vector.y);
			previousPosition.x += velocity.x * deltaTime;
			previousPosition.y += velocity.y * deltaTime;
			Vector3 vector2 = previousPosition - CachedTransform.position;
			if (isRotateFromDirection && velocity.sqrMagnitude > 30f)
			{
				float z = 90f - Mathf.Atan2(vector2.x, vector2.y) * 57.29578f;
				CachedTransform.rotation = Quaternion.Euler(CachedTransform.rotation.x, CachedTransform.rotation.y, z);
			}
			CachedTransform.position = previousPosition;
			Vector3 localPosition = CachedTransform.localPosition;
			localPosition.z = 0f;
			CachedTransform.localPosition = localPosition;
		}
	}

	protected virtual void UpdatePhysicalWeapon(float deltaTime)
	{
		if (isFlying)
		{
			Vector3 vector = Vector3.zero;
			if (isAccelerationEnabled)
			{
				Vector3 normalized = velocity.normalized;
				normalized *= acceleration * deltaTime;
				normalized.z = 0f;
				vector = normalized;
			}
			velocity = new Vector3(CachedRigidbody.velocity.x + vector.x, CachedRigidbody.velocity.y + vector.y);
			if (isRotateFromDirection && velocity.sqrMagnitude > 30f)
			{
				CachedTransform.right = CachedRigidbody.velocity;
				if (level.ActiveCharacter.IsFlipped)
				{
					float num = 180f - CachedTransform.localRotation.eulerAngles.z;
					if (Mathf.Abs(num - 180f) < 1f)
					{
						num = 0f;
					}
					CachedTransform.localRotation = Quaternion.Euler(0f, 180f, num);
				}
			}
		}
		if (IsActive && isWeaponAfterCollisionAvailable)
		{
			CheckDisablePhysicalWeapon(deltaTime, isFlying);
		}
	}

	protected virtual void CheckDisablePhysicalWeapon(float deltaTime, bool flying = false)
	{
		disableCheckTimeout += deltaTime;
		if (disableCheckTimeout > 0.2f)
		{
			disableCheckTimeout = 0f;
			if (CachedRigidbody.velocity.sqrMagnitude < disableSqrVelocityBorder && Mathf.Abs(CachedRigidbody.angularVelocity.z) <= disableAngularVelocityBorder)
			{
				Disabled(true);
				CachedRigidbody.Sleep();
			}
		}
	}

	protected virtual void ChangeSortingLayerTo(string sortingLayerName)
	{
		tk2dSprite[] componentsInChildren = GetComponentsInChildren<tk2dSprite>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].CachedRenderer.sortingLayerName = sortingLayerName;
		}
		if (!IdleEffect)
		{
			return;
		}
		ParticleSystemRenderer[] componentsInChildren2 = IdleEffect.GetComponentsInChildren<ParticleSystemRenderer>();
		ParticleSystemRenderer[] array = componentsInChildren2;
		foreach (ParticleSystemRenderer particleSystemRenderer in array)
		{
			if (particleSystemRenderer.sortingLayerName == "FX_Weapon_Front" || particleSystemRenderer.sortingLayerName == "Default")
			{
				particleSystemRenderer.sortingLayerName = "FX_Weapon_UnderCharacter";
			}
		}
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
	}

	protected virtual void SetIdleEffectEnabled(bool enabled)
	{
		if ((bool)IdleEffect)
		{
			IdleEffect.SetActive(enabled);
		}
	}

	protected virtual void StopAllSounds()
	{
		if (SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			AudioManager.StopAudioWithKey(activationFlyingSound);
			AudioManager.StopAudioWithKey(idleSound);
			AudioManager.StopAudioWithKey(flyingSound);
		}
	}

	private bool IsShotMissedForCollidedObject(ICollidable collidedObject)
	{
		bool result = true;
		if (level.GameType == GameType.VSComputer || level.GameType == GameType.Tournament)
		{
			if (level.ActiveCharacter == level.CharacterFirst)
			{
				if (collidedObject is CharacterBase)
				{
					result = (CharacterBase)collidedObject != level.CharacterSecond;
				}
			}
			else
			{
				result = false;
			}
		}
		else if (level.GameType == GameType.BirdsHunt)
		{
			if (collidedObject is FlyingObject)
			{
				result = false;
			}
		}
		else if (level.GameType == GameType.AppleShoot)
		{
			if (collidedObject is Apple)
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void Activate(WeaponActivation activation, WeaponActivationType type, ICollidable collidedObject)
	{
		if (!(activation != null) || !activation.IsActivationEnabled(type, collidedObject))
		{
			return;
		}
		if (activation.Type == WeaponActivationType.Tap)
		{
			if (level.IsMultiplayerSecondaryPlayer && level is LevelMultiplayer && (level as LevelMultiplayer).ClientReceivedCollideMessage)
			{
				return;
			}
			SingletonMonoBehaviour<TutorialManager>.Instance.WeaponWasActivated(this);
			level.WeaponWasActivatedByTap(this);
		}
		isActivated = true;
		if (isUnfollowedAfterActivation)
		{
			level.UnfollowWeaponWithVelocity(Velocity, unfollowDuration);
		}
		if (CachedEmission != null)
		{
			CachedEmission.EmitOnActivate(CachedTransform.position, CachedTransform);
			CachedEmission.StartTrails(CachedTransform, CachedTransform.position, velocity, EmitType.Activate);
		}
		activation.Activate(this, type, collidedObject);
		AudioManager.StopAudioWithKey(flyingSound);
		AudioManager.PlayAudioWithKey(activationSound);
		AudioManager.PlayAudioWithKey(activationFlyingSound);
	}

	private void UpdateRotationSound(float deltaTime)
	{
		if (!isRotatingFlyingSound || !isCollideEnabled)
		{
			return;
		}
		whooshDelay += deltaTime;
		if (!(whooshDelay > whooshDelayBetweenSounds))
		{
			return;
		}
		float magnitude = CachedRigidbody.angularVelocity.magnitude;
		if (magnitude > whooshMinVelocity)
		{
			float num = (whooshMaxVelocity - whooshMinVelocity) * 0.5f;
			float whooshPower = Mathf.Clamp((magnitude - (whooshMinVelocity + num)) / num, -1f, 1f);
			float whooshDuration = Mathf.Clamp((float)Math.PI * 2f / magnitude, 0f, 0.5f);
			AudioManager.PlayAudioWithKey(flyingSound, delegate(SoundObject obj)
			{
				if (obj != null && obj != null)
				{
					obj.PlayingDuration = whooshDuration;
					obj.Volume = 0.5f + whooshPower * 0.5f;
					obj.Pitch = Mathf.Pow(1.3f, whooshPower);
				}
			});
		}
		whooshDelay = 0f;
	}

	private IEnumerator ActivationCoroutine(WeaponActivationType type, ICollidable collidedObject, bool delayed)
	{
		foreach (WeaponActivation activation in Activations)
		{
			if (activation.Type != WeaponActivationType.Tap)
			{
				Activate(activation, type, collidedObject);
			}
		}
		if (delayed)
		{
			yield return new WaitUntil(() => false);
			yield return new WaitForSeconds(0.25f);
		}
		foreach (WeaponActivation activation2 in Activations)
		{
			if (activation2.Type == WeaponActivationType.Tap)
			{
				Activate(activation2, type, collidedObject);
			}
		}
	}

	public virtual ShootingInfo GetShootingInfo(Vector3 startPosition, Vector3 targetPosition, float angle)
	{
		float x = targetPosition.x;
		float y = targetPosition.y;
		float x2 = startPosition.x;
		float y2 = startPosition.y;
		float num = Mathf.Cos(angle * ((float)Math.PI / 180f));
		float num2 = Mathf.Sin(angle * ((float)Math.PI / 180f));
		float num3 = -490.5f * num;
		float num4 = 0f;
		float num5 = (x - x2) * num2 + (y2 - y) * num;
		float f = Mathf.Pow(num4, 2f) - 4f * num3 * num5;
		float num6 = (0f - num4 + Mathf.Pow(f, 0.5f)) / (2f * num3);
		float num7 = (0f - num4 - Mathf.Pow(f, 0.5f)) / (2f * num3);
		float num8 = ((!(num7 > 0f)) ? num6 : num7);
		float num9 = (x - x2) / (num8 * num);
		return new ShootingInfo(angle, num9);
	}

	public virtual ShootingInfo GetShootingInfoForMaxForce(Vector3 startPosition, Vector3 targetPosition, float maxVelocity)
	{
		float x = targetPosition.x;
		float y = targetPosition.y;
		float x2 = startPosition.x;
		float y2 = startPosition.y;
		float a = 981f * Mathf.Pow(x - x2, 2f);
		float b = -2f * Mathf.Abs(x - x2) * Mathf.Pow(maxVelocity, 2f);
		float c = 981f * Mathf.Pow(x - x2, 2f) - 2f * Mathf.Abs(y2 - y) * Mathf.Pow(maxVelocity, 2f);
		float[] solutions = GetSolutions(a, b, c);
		float f = Mathf.Min(solutions[0], solutions[1]);
		float angle = Mathf.Atan(f) * 57.29578f;
		return new ShootingInfo(angle, maxVelocity);
	}

	public virtual ShootingInfo GetShootingInfoForApple(Vector3 startPosition, Vector3 targetPosition, float maxVelocity)
	{
		float x = targetPosition.x;
		float y = targetPosition.y;
		float x2 = startPosition.x;
		float y2 = startPosition.y;
		float num = 981f * Mathf.Pow(x - x2, 2f);
		float b = -2f * Mathf.Abs(x - x2) * Mathf.Pow(maxVelocity, 2f);
		float c = num + 2f * Mathf.Abs(y - y2) * Mathf.Pow(maxVelocity, 2f);
		float[] solutions = GetSolutions(num, b, c);
		float f = Mathf.Min(solutions[0], solutions[1]);
		float angle = Mathf.Atan(f) * 57.29578f;
		return new ShootingInfo(angle, maxVelocity);
	}

	public virtual float[] GetTimeToPositionX(float startPositionX, float targetPositionX)
	{
		float num = Mathf.Cos(emissionAngle * ((float)Math.PI / 180f));
		float num2 = startVelocity;
		return GetSolutions(0f, (0f - num2) * num, targetPositionX - startPositionX);
	}

	public virtual float[] GetTimeToPositionY(float startPositionY, float targetPositionY)
	{
		float num = Mathf.Sin(emissionAngle * ((float)Math.PI / 180f));
		float num2 = startVelocity;
		return GetSolutions(-490.5f, num2 * num, startPositionY - targetPositionY);
	}

	public virtual float GetTimeForTrajectoryPeak()
	{
		float num = Mathf.Sin(emissionAngle * ((float)Math.PI / 180f));
		float num2 = startVelocity;
		return num2 * num / 981f;
	}

	public virtual float GetPositionYForTime(float time, float startPositionY)
	{
		float num = Mathf.Sin(emissionAngle * ((float)Math.PI / 180f));
		float num2 = startVelocity;
		return startPositionY + num2 * num * time - 490.5f * Mathf.Pow(time, 2f);
	}

	public virtual float GetPositionXForTime(float time, float startPositionX)
	{
		float num = Mathf.Cos(emissionAngle * ((float)Math.PI / 180f));
		float num2 = startVelocity;
		return startPositionX + num2 * num * time;
	}

	public float[] GetSolutions(float a, float b, float c)
	{
		float num = Mathf.Pow(b, 2f) - 4f * a * c;
		float[] result = null;
		if (num >= 0f)
		{
			float num2 = (0f - b + Mathf.Pow(num, 0.5f)) / (2f * a);
			float num3 = (0f - b - Mathf.Pow(num, 0.5f)) / (2f * a);
			result = new float[2] { num2, num3 };
		}
		return result;
	}

	public float GetMinPositiveValue(float[] values)
	{
		float num = float.MaxValue;
		for (int i = 0; i < values.Length; i++)
		{
			if (values[i] > 0f && values[i] < num)
			{
				num = values[i];
			}
		}
		return num;
	}

	public virtual bool OnCollision(CollisionInfo collisionInfo)
	{
		return false;
	}

	public void ApplyDamage(CollisionInfo collisionInfo)
	{
	}

	public void ApplyForce(float force, ForceMode forceMode, CollisionInfo collisionInfo)
	{
	}

	public bool IsAffectedInRadius(Vector3 position, float radius)
	{
		return false;
	}

	public List<Collider> AffectedObjects(Vector3 position, float radius)
	{
		return null;
	}
}
