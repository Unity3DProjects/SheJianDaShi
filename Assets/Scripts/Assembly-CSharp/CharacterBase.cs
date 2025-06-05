using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class CharacterBase : LevelObject, ICollidable
{
	private const string BODY_BONE_NAME = "body";

	private const int ELECTRICITY_ANIMATION_TRACK_ENTRY = 6;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<CharacterBase> m_OnCharacterGroundCollided;

	[SerializeField]
	protected CharacterType characterType;

	[SerializeField]
	protected CharacterVoiceType characterVoiceType;

	[SerializeField]
	protected SkeletonAnimation skeletonAnimation;

	[SerializeField]
	protected SkeletonDataAsset skeletonDataAssetClean;

	[SerializeField]
	protected SkeletonRagdoll ragdoll;

	[SerializeField]
	protected Transform rootTransform;

	[SerializeField]
	protected float health = 3f;

	[SerializeField]
	protected float minVelocityRoot = 2000f;

	[SerializeField]
	protected float maxVelocityRoot = 3000f;

	[SerializeField]
	protected Transform bodyTransform;

	[SerializeField]
	protected float restoreVelocityRoot = 1.5f;

	[SerializeField]
	protected float restoreRagdollTime = 2f;

	[SerializeField]
	public MaskManager masksManager;

	[SerializeField]
	protected FatalityManager fatalityManager;

	[SerializeField]
	private float headMassRatio = 1f;

	[SerializeField]
	private CharacterDamageHandler damageHandlerPrefab;

	[SerializeField]
	protected Collider headCollider;

	[SerializeField]
	private float minDamageForPainSound = 10f;

	[SerializeField]
	private float appleHeightOffset = 160f;

	[SerializeField]
	protected SkeletonUtilityBone[] flippingBones;

	[SerializeField]
	private bool isAvailableAsAppleBoy = true;

	[SerializeField]
	private CharacterMode characterMode;

	[Header("DoubleCharacterSettings")]
	[SerializeField]
	private tk2dSpriteCollectionData spriteCollectionData1;

	[SerializeField]
	private tk2dSpriteCollectionData spriteCollectionData2;

	private bool isSecondCharacterActive;

	protected CharacterAnimationCache animationCache;

	protected Level level;

	protected List<Collider> partsColliders;

	protected List<Collider> drivedIntoColliders = new List<Collider>();

	protected List<WeaponGeneral> drivenIntoWeapons = new List<WeaponGeneral>();

	protected List<WeaponGeneral> drivenIntoLegsWeapons = new List<WeaponGeneral>();

	protected bool isFlipped;

	protected bool isRestoreNeeded;

	protected bool isInvincible;

	protected float initialHealth;

	protected float lastTurnDamageAmount;

	protected float restoreCheckTimeout;

	protected float mustRestoreTimeout;

	protected CharacterDamageHandler damageHandler;

	protected List<Collider> affectedObjects = new List<Collider>();

	protected bool isCharacterDragSoundPlaying;

	protected SoundObject dragSound;

	protected float dragSoundMaxVolumeVelocity = 100f;

	protected float timeAboveGround;

	protected bool isAboveGround;

	protected float characterHitBonesVelocity = 300f;

	protected bool isFinishHim;

	private GameObject electricityEffectHead;

	private GameObject electricityEffectBody;

	protected bool isSuicide;

	protected List<Vector3> ragdollPositions = new List<Vector3>();

	protected List<Vector3> ragdollRotations = new List<Vector3>();

	private int drivenIntoWeaponsCacheSize;

	private float forcedRestoreXPosition;

	protected Transform cachedTransform;

	private Managers cachedManagers;

	public CharacterType CharacterType
	{
		get
		{
			return characterType;
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

	public List<Collider> PartsColliders
	{
		get
		{
			if (partsColliders == null)
			{
				partsColliders = new List<Collider>(GetComponentsInChildren<Collider>());
			}
			return partsColliders;
		}
	}

	public bool PartsCollidersEnabled { get; set; }

	public Rigidbody[] RagdollRigidbodies
	{
		get
		{
			if ((bool)ragdoll)
			{
				return ragdoll.GetRigidbodyArray();
			}
			return null;
		}
	}

	public Rigidbody RootRigidBody
	{
		get
		{
			Rigidbody result = null;
			if ((bool)ragdoll)
			{
				result = ragdoll.RootRigidbody;
			}
			return result;
		}
	}

	public Transform BodyTransform
	{
		get
		{
			return bodyTransform;
		}
	}

	public Rigidbody HeadRigidBody
	{
		get
		{
			Rigidbody result = null;
			if ((bool)ragdoll)
			{
				result = ragdoll.GetRigidbody("head");
			}
			return result;
		}
	}

	public float HeadMassRatio
	{
		get
		{
			return headMassRatio;
		}
	}

	public float Health
	{
		get
		{
			return health;
		}
		private set
		{
			if (health != value && !IsAppleBoy && level.InactiveArcher != null && !level.InactiveArcher.IsFinishHim && !level.IsMultiplayerSecondaryPlayer)
			{
				lastTurnDamageAmount += health - value;
				if ((level.GameType == GameType.VSComputer || level.GameType == GameType.Tournament) && this == level.CharacterSecond && health > value)
				{
					CachedManagers.CachedStatisticsManager.TotalDamageApplied += health - value;
					CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.ArrowToTheKnee, (int)CachedManagers.CachedStatisticsManager.TotalDamageApplied);
				}
				health = value;
				UpdateHealthBar();
				if (level.GameType == GameType.Multiplayer)
				{
					(level as LevelMultiplayer).HealthChanged(this, health);
				}
			}
		}
	}

	public bool IsFlipped
	{
		get
		{
			return isFlipped;
		}
		set
		{
			isFlipped = value;
			rootTransform.localEulerAngles = new Vector3(0f, isFlipped ? 180 : 0, 0f);
			for (int i = 0; i < flippingBones.Length; i++)
			{
				SkeletonUtilityBone skeletonUtilityBone = flippingBones[i];
				skeletonUtilityBone.flipX = isFlipped;
			}
		}
	}

	public bool IsInvincible
	{
		get
		{
			return isInvincible;
		}
		set
		{
			if (isInvincible != value)
			{
				isInvincible = value;
			}
		}
	}

	public SkeletonAnimation SkeletonAnimation
	{
		get
		{
			return skeletonAnimation;
		}
	}

	public CharacterDamageHandler DamageHandler
	{
		get
		{
			if (damageHandler == null && damageHandlerPrefab != null && !isInvincible)
			{
				damageHandler = UnityEngine.Object.Instantiate(damageHandlerPrefab);
				damageHandler.transform.parent = CachedTransform;
			}
			return damageHandler;
		}
	}

	public int DrivenIntoWeaponsCacheSize
	{
		get
		{
			return drivenIntoWeaponsCacheSize;
		}
		set
		{
			drivenIntoWeaponsCacheSize = value;
		}
	}

	public int HealthPercents
	{
		get
		{
			return (int)(Health * 100f / initialHealth);
		}
	}

	public bool IsFinishHim
	{
		get
		{
			return isFinishHim;
		}
		protected set
		{
			if (isFinishHim == value)
			{
				return;
			}
			isFinishHim = value;
			if (isFinishHim)
			{
				CachedManagers.CachedGameManager.CameraController.StartBlur();
				GameScreen gameScreen = CachedManagers.CachedGUIScreenManager.GameScreen;
				if (gameScreen != null)
				{
					gameScreen.ShowFinishHim(characterVoiceType != CharacterVoiceType.Woman);
				}
			}
		}
	}

	public FatalityManager FatalityManager
	{
		get
		{
			return fatalityManager;
		}
	}

	protected bool IsCharacterDragSoundPlaying
	{
		get
		{
			return isCharacterDragSoundPlaying;
		}
		set
		{
			if (isCharacterDragSoundPlaying == value)
			{
				return;
			}
			isCharacterDragSoundPlaying = value;
			if (isCharacterDragSoundPlaying)
			{
				AudioManager.PlayAudioWithKey("character_ground_drag", delegate(SoundObject obj)
				{
					dragSound = obj;
				});
			}
			else
			{
				AudioManager.StopAudioWithKey("character_ground_drag");
				dragSound = null;
			}
		}
	}

	protected bool IsAboveGround
	{
		get
		{
			return isAboveGround;
		}
		set
		{
			if (isAboveGround != value)
			{
				isAboveGround = value;
				if (isAboveGround)
				{
					IsCharacterDragSoundPlaying = false;
				}
			}
		}
	}

	public bool IsAppleBoy { get; set; }

	public Vector3 BoyPosition
	{
		get
		{
			Vector3 position = CachedTransform.position;
			Rigidbody rootRigidBody = RootRigidBody;
			if ((bool)rootRigidBody && !rootRigidBody.isKinematic)
			{
				position = rootRigidBody.transform.position;
			}
			return position;
		}
	}

	public Rigidbody HeadColliderBody
	{
		get
		{
			return headCollider.GetComponentInParent<Rigidbody>();
		}
	}

	public CharacterAnimationCache AnimationChache
	{
		get
		{
			return animationCache;
		}
	}

	public Collider HeadCollider
	{
		get
		{
			return headCollider;
		}
	}

	public float AppleHeightOffset
	{
		get
		{
			return appleHeightOffset;
		}
	}

	public CharacterVoiceType CharacterVoiceType
	{
		get
		{
			return characterVoiceType;
		}
	}

	public bool IsSuicide
	{
		get
		{
			return isSuicide;
		}
		set
		{
			if (isSuicide == value)
			{
				return;
			}
			isSuicide = value;
			Level currentLevel = CachedManagers.CachedLevelsManager.CurrentLevel;
			if (isSuicide && currentLevel != null)
			{
				Character activeCharacter = currentLevel.ActiveCharacter;
				if (activeCharacter != null)
				{
					activeCharacter.RestoreArcher();
				}
			}
		}
	}

	public bool ForceNotRestore { get; set; }

	public bool IsAvailableAsAppleBoy
	{
		get
		{
			return isAvailableAsAppleBoy;
		}
	}

	public SkeletonDataAsset SkeletonDataAssetClean
	{
		get
		{
			return skeletonDataAssetClean;
		}
	}

	protected Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	public static event Action<CharacterBase> OnCharacterGroundCollided
	{
		add
		{
			Action<CharacterBase> action = CharacterBase.m_OnCharacterGroundCollided;
			Action<CharacterBase> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref CharacterBase.m_OnCharacterGroundCollided, (Action<CharacterBase>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<CharacterBase> action = CharacterBase.m_OnCharacterGroundCollided;
			Action<CharacterBase> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref CharacterBase.m_OnCharacterGroundCollided, (Action<CharacterBase>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected virtual void Awake()
	{
		skeletonAnimation.Reset();
		skeletonAnimation.gameObject.AddComponent<CustomRenderQueue>();
	}

	protected virtual void OnEnable()
	{
		animationCache = new CharacterAnimationCache();
		animationCache.SkeletonAnimation = skeletonAnimation;
		animationCache.AnimationState.Event += AnimationState_Event;
		animationCache.AnimationState.Start += AnimationStart_Event;
		animationCache.AnimationState.Complete += AnimationComplete_Event;
		FatalityManager obj = fatalityManager;
		obj.OnFatalityEnd = (Action<WeaponType>)Delegate.Combine(obj.OnFatalityEnd, new Action<WeaponType>(FatalityManager_OnFatalityEnd));
		Level.OnPlayerChanged += Level_OnPlayerChanged;
	}

	protected virtual void OnDisable()
	{
		animationCache.AnimationState.Event -= AnimationState_Event;
		animationCache.AnimationState.Start -= AnimationStart_Event;
		animationCache.AnimationState.Complete -= AnimationComplete_Event;
		FatalityManager obj = fatalityManager;
		obj.OnFatalityEnd = (Action<WeaponType>)Delegate.Remove(obj.OnFatalityEnd, new Action<WeaponType>(FatalityManager_OnFatalityEnd));
		Level.OnPlayerChanged -= Level_OnPlayerChanged;
	}

	protected void OnDestroy()
	{
		if (SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			IsCharacterDragSoundPlaying = false;
		}
	}

	public virtual void Init(Level level, float forcedHealth = -1f)
	{
		this.level = level;
		if (forcedHealth > 0f)
		{
			health = forcedHealth;
		}
		initialHealth = health;
		PartsCollidersEnabled = true;
		fatalityManager.Init(level);
		if ((bool)DamageHandler)
		{
			DamageHandler.Init(this, level);
		}
	}

	public void UpdateCharacter(float deltaTime)
	{
		Rigidbody rootRigidBody = RootRigidBody;
		if (rootRigidBody != null)
		{
			float magnitude = rootRigidBody.velocity.magnitude;
			if (dragSound != null)
			{
				dragSound.Volume = Mathf.InverseLerp(restoreVelocityRoot, dragSoundMaxVolumeVelocity, magnitude);
			}
			if (isRestoreNeeded && level != null && !level.IsMultiplayerSecondaryPlayer && DamageHandler != null && !DamageHandler.IsPeriodicalDamageActive)
			{
				timeAboveGround += deltaTime;
				IsAboveGround = timeAboveGround > 0.2f;
				restoreCheckTimeout += deltaTime;
				mustRestoreTimeout += deltaTime;
				if (restoreCheckTimeout > 0.2f)
				{
					restoreCheckTimeout = 0f;
					bool flag = level.Bounds.Contains(rootRigidBody.position);
					if ((magnitude <= restoreVelocityRoot || !flag || mustRestoreTimeout > 10f) && !ForceNotRestore)
					{
						mustRestoreTimeout = 0f;
						float x = rootRigidBody.position.x;
						LevelCameraSettings levelSettings = CachedManagers.CachedGameManager.CameraController.LevelSettings;
						if (x > levelSettings.maxX && ragdoll != null)
						{
							ragdoll.GetRigidbody("body").AddForce(new Vector3(-1f, 1f, 0f) * 300000000f);
						}
						else if (x < levelSettings.minX && ragdoll != null)
						{
							ragdoll.GetRigidbody("body").AddForce(new Vector3(1f, 1f, 0f) * 300000000f);
						}
						else
						{
							Restore();
						}
					}
				}
			}
		}
		if (level != null && level.GameType == GameType.Multiplayer && !level.IsMultiplayerSecondaryPlayer && ragdoll != null && ragdoll.IsActive && RagdollRigidbodies != null && RagdollRigidbodies.Length > 1)
		{
			int i = 0;
			for (int num = RagdollRigidbodies.Length; i < num; i++)
			{
				Transform transform = RagdollRigidbodies[i].transform;
				if (ragdollPositions.Count <= i)
				{
					ragdollPositions.Add(transform.position);
				}
				else
				{
					ragdollPositions[i] = transform.position;
				}
				if (ragdollRotations.Count <= i)
				{
					ragdollRotations.Add(transform.eulerAngles);
				}
				else
				{
					ragdollRotations[i] = transform.eulerAngles;
				}
			}
			(level as LevelMultiplayer).RagdollPositionChanged(this, ragdollPositions, ragdollRotations);
		}
		skeletonAnimation.Update(deltaTime);
		fatalityManager.UpdateFatality(deltaTime);
		if (DamageHandler != null)
		{
			DamageHandler.UpdateHandler(deltaTime);
		}
	}

	public void UpdatePosition(Vector3 position)
	{
		CachedTransform.position = position;
		Vector3 localPosition = CachedTransform.localPosition;
		CachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y, 0f);
	}

	public void UpdateHealthBar()
	{
		GameScreen gameScreen = CachedManagers.CachedGUIScreenManager.GameScreen;
		if (gameScreen != null)
		{
			float value = Health / initialHealth;
			if (level.CharacterFirst == this)
			{
				gameScreen.SetFirstPlayerHealthFill(value);
			}
			else
			{
				gameScreen.SetSecondPlayerHealthFill(value);
			}
		}
	}

	public virtual void Restore()
	{
		if (IsAppleBoy)
		{
			isRestoreNeeded = false;
			Vector2 targetPos = new Vector2(CachedTransform.position.x, level.DefaultCameraPositionForCharacter(level.ActiveCharacter).y);
			CachedManagers.CachedGameManager.CameraController.TweenMoveToPosition(targetPos, 1f);
			CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
			{
				RestoreAction();
			}, 1f);
		}
		else
		{
			RestoreAction();
		}
	}

	public void PlayIdleAnimation()
	{
		animationCache.AnimationState.SetAnimation(0, animationCache.IdleAnimation, true);
	}

	public void StartDefaultAnimation()
	{
		animationCache.AnimationState.SetAnimation(0, animationCache.DefaultAnimation, true);
	}

	public void StartFaceDefaultAnimation()
	{
		animationCache.AnimationState.SetAnimation(0, animationCache.FaceDefaultAnimation, true);
	}

	public void StartFaceKOAnimation()
	{
		animationCache.AnimationState.SetAnimation(0, animationCache.FaceKOAnimation, true);
	}

	public bool IsPeriodicalDamageAnimationActive()
	{
		Spine.Animation currentAnimation = animationCache.CurrentAnimation;
		if (currentAnimation != null)
		{
			return currentAnimation == animationCache.FireAnimation || currentAnimation == animationCache.ShockAnimation;
		}
		return false;
	}

	public void StopAnimation(int trackIndex)
	{
		animationCache.AnimationState.ClearTrack(trackIndex);
	}

	public void StartFireAnimation()
	{
		if (!IsFinishHim && !RootRigidBody && animationCache.CurrentAnimation != animationCache.FireAnimation)
		{
			animationCache.AnimationState.SetAnimation(0, animationCache.FireAnimation, true);
		}
	}

	public void StartElectricityAnimation()
	{
		if (!IsFinishHim && !RootRigidBody && animationCache.CurrentAnimation != animationCache.ShockAnimation)
		{
			animationCache.AnimationState.SetAnimation(6, animationCache.ShockAnimation, true);
		}
	}

	public void StartElectricityIdleAnimation()
	{
		if (!IsFinishHim && animationCache.CurrentAnimation != animationCache.ShockIdleAnimation)
		{
			animationCache.AnimationState.SetAnimation(3, animationCache.ShockIdleAnimation, false);
			StartElectricityEffect();
			StopElectricityEffect(AnimationChache.ShockIdleAnimation.Duration);
		}
	}

	public void StartFireDeathAnimation()
	{
		if (!RootRigidBody && animationCache.CurrentAnimation != animationCache.DeathFireAnimation)
		{
			animationCache.AnimationState.SetAnimation(0, animationCache.DeathFireAnimation, true);
		}
	}

	public void StartElectricityDeathAnimation()
	{
		if (!RootRigidBody && animationCache.CurrentAnimation != animationCache.DeathShockAnimation)
		{
			animationCache.AnimationState.SetAnimation(0, animationCache.DeathShockAnimation, true);
		}
	}

	public void StartGreetingAnimation()
	{
		animationCache.SetAnimation(CharacterAnimationType.Spawn, 100f, CharacterAnimationType.Idle);
	}

	public void ApplyDamageAnimation()
	{
		animationCache.AnimationState.SetAnimation(0, animationCache.FaceScreamAnimation, false);
		animationCache.AnimationState.AddAnimation(0, animationCache.FaceKOAnimation, true, 0f);
	}

	public void SetLevel(Level level)
	{
		this.level = level;
	}

	public void ApplyCharacterRagdoll(bool restoreNeeded = false)
	{
		if ((bool)RootRigidBody)
		{
			return;
		}
		ragdoll.colliderLayer = LayerMask.NameToLayer("CharacterParts");
		ragdoll.Apply();
		HeadRigidBody.mass = RootRigidBody.mass * HeadMassRatio;
		if (IsAppleBoy)
		{
			(level as LevelAppleShoot).DestroyAppleJoint();
		}
		if (level.GameType == GameType.Multiplayer)
		{
			(level as LevelMultiplayer).ApplyCharacterRagdoll(this);
			if (level.IsMultiplayerSecondaryPlayer && !level.IsMainBattleFinished)
			{
				Rigidbody[] ragdollRigidbodies = RagdollRigidbodies;
				for (int i = 0; i < ragdollRigidbodies.Length; i++)
				{
					ragdollRigidbodies[i].isKinematic = true;
					ragdollRigidbodies[i].useGravity = false;
					HingeJoint component = ragdollRigidbodies[i].GetComponent<HingeJoint>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
					}
				}
			}
		}
		if (restoreNeeded)
		{
			level.TurnActionsChangeCount(1);
			isRestoreNeeded = !IsFinishHim;
		}
	}

	public void ApplyExplosionForce(ExplosionInfo explosionInfo)
	{
		List<Collider> list = new List<Collider>(PartsColliders);
		list.RemoveAll((Collider obj) => obj == null || ((bool)obj && obj.isTrigger));
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.otherCollider = list.RandomObject();
		collisionInfo.position = CachedTransform.position;
		collisionInfo.fatality = explosionInfo.weaponType;
		collisionInfo.fatalityActvationType = FatalityActivationType.Explode;
		if (IsFinishHim && !explosionInfo.isFatalityStartedWithDelay)
		{
			fatalityManager.StartFatality(collisionInfo);
		}
		else if (IsFinishHim && explosionInfo.isFatalityStartedWithDelay)
		{
			RootRigidBody.velocity = new Vector3(0f, restoreVelocityRoot + 1f, 0f);
			ExplodeComponent.AddExplodeForce(ragdoll.GetRigidbody("body"), explosionInfo);
			StartCoroutine(StartFatalityWithDelay(explosionInfo.fatalityDelay, collisionInfo));
		}
		else if (!level.IsMultiplayerSecondaryPlayer && !explosionInfo.isFatalityStartedWithDelay)
		{
			ApplyDamage(collisionInfo);
			RootRigidBody.velocity = new Vector3(0f, restoreVelocityRoot + 1f, 0f);
			float num = ExplodeComponent.AddExplodeForce(ragdoll.GetRigidbody("body"), explosionInfo);
			Health -= explosionInfo.damage * num;
		}
	}

	public void ApplyPeriodicalDamage(PeriodicalDamageInfo damageInfo)
	{
		if (IsFinishHim)
		{
			List<Collider> list = new List<Collider>(PartsColliders);
			list.RemoveAll((Collider obj) => obj == null || obj.isTrigger);
			CollisionInfo collisionInfo = new CollisionInfo();
			collisionInfo.otherCollider = list.RandomObject();
			collisionInfo.position = CachedTransform.position;
			collisionInfo.fatality = damageInfo.weaponType;
			collisionInfo.fatalityActvationType = FatalityActivationType.Periodical;
			fatalityManager.StartFatality(collisionInfo);
		}
		else
		{
			Health -= damageInfo.damage;
		}
	}

	public void SetCollidersEnabled(bool enabled, bool force = false)
	{
		if (!force && (IsInvincible || (level.IsMultiplayerSecondaryPlayer && !level.IsMainBattleFinished)))
		{
			return;
		}
		int i = 0;
		for (int count = PartsColliders.Count; i < count; i++)
		{
			Collider collider = PartsColliders[i];
			if (collider != null && !drivedIntoColliders.Contains(collider))
			{
				collider.enabled = enabled;
			}
		}
		PartsCollidersEnabled = enabled;
	}

	public void AddDriveIntoWeaponColliders(List<Collider> colliders)
	{
		drivedIntoColliders.AddRange(colliders);
	}

	public void AddDriveIntoWeapon(WeaponGeneral weapon)
	{
		drivenIntoWeapons.Add(weapon);
		if (drivenIntoWeapons.Count > DrivenIntoWeaponsCacheSize)
		{
			if (drivenIntoWeapons[0] != null)
			{
				drivenIntoWeapons[0].RemoveFromCharacter();
			}
			drivenIntoWeapons.RemoveAt(0);
		}
	}

	public void AddDriveIntoLegsWeapon(WeaponGeneral weapon)
	{
		drivenIntoLegsWeapons.Add(weapon);
	}

	public void RemoveDriveIntoWeapon(WeaponGeneral weapon)
	{
		drivenIntoWeapons.Remove(weapon);
	}

	public void RemoveAllDriveIntoWeapon()
	{
		int i = 0;
		for (int count = drivenIntoWeapons.Count; i < count; i++)
		{
			WeaponGeneral weaponGeneral = drivenIntoWeapons[i];
			if (weaponGeneral != null)
			{
				weaponGeneral.RemoveFromCharacter();
			}
		}
		drivenIntoWeapons.Clear();
	}

	public void AddLifePercent(float percent)
	{
		health += initialHealth * percent;
		UpdateHealthBar();
	}

	public void StartElectricityEffect()
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (DamageHandler.ElectricityEffectPrefab != null && electricityEffectHead == null && electricityEffectBody == null)
			{
				electricityEffectHead = UnityEngine.Object.Instantiate(DamageHandler.ElectricityEffectPrefab);
				electricityEffectHead.transform.parent = HeadCollider.transform;
				electricityEffectHead.transform.localPosition = Vector3.zero;
				electricityEffectBody = UnityEngine.Object.Instantiate(DamageHandler.ElectricityEffectPrefab);
				electricityEffectBody.transform.parent = BodyTransform;
				electricityEffectBody.transform.localPosition = Vector3.zero;
			}
		}, 0f);
	}

	public void StopElectricityEffect(float delay)
	{
		StartCoroutine(StopElectricityEffectWithDelay(delay));
	}

	public void SetAnimation(int trackIndex, string animationSetName, bool isLooped, bool isForced = false)
	{
		if (!level.IsMultiplayerSecondaryPlayer || isForced)
		{
			Spine.Animation animation = animationCache.AnimationByName(animationSetName);
			if (animation != null)
			{
				animationCache.AnimationState.SetAnimation(trackIndex, animation, isLooped);
			}
		}
	}

	public void AddAnimation(int trackIndex, string animationAddName, bool isLooped, bool isForced = false)
	{
		if (!level.IsMultiplayerSecondaryPlayer || isForced)
		{
			Spine.Animation animation = animationCache.AnimationByName(animationAddName);
			if (animation != null)
			{
				animationCache.AnimationState.AddAnimation(trackIndex, animation, isLooped, 0f);
			}
		}
	}

	protected virtual void RagdollCollideEnter(Collision coll)
	{
		if (!IsGroundCollision(coll))
		{
			return;
		}
		if (IsAboveGround)
		{
			if (coll.relativeVelocity.magnitude > characterHitBonesVelocity)
			{
				AudioManager.PlayAudioWithKey("character_hit_bones");
				if (characterVoiceType == CharacterVoiceType.Woman)
				{
					AudioManager.PlayAudioWithKey("character_woman_pain");
				}
				else if (characterVoiceType == CharacterVoiceType.ManBeefy)
				{
					AudioManager.PlayAudioWithKey("character_man_beefy_pain");
				}
			}
			timeAboveGround = 0f;
			IsAboveGround = false;
			if (CharacterBase.OnCharacterGroundCollided != null)
			{
				CharacterBase.OnCharacterGroundCollided(this);
			}
		}
		if (!IsFinishHim && coll.relativeVelocity.magnitude > restoreVelocityRoot)
		{
			IsCharacterDragSoundPlaying = true;
		}
	}

	protected virtual void RagdollCollideStay(Collision coll)
	{
		if (IsGroundCollision(coll))
		{
			timeAboveGround = 0f;
		}
	}

	protected virtual void RagdollCollideExit(Collision coll)
	{
	}

	protected void ShiftCharacterOnRestore()
	{
		if (IsAppleBoy)
		{
			return;
		}
		Rigidbody rootRigidBody = RootRigidBody;
		if (!(rootRigidBody != null))
		{
			return;
		}
		float x = rootRigidBody.transform.position.x;
		if (level.GameType == GameType.Multiplayer)
		{
			if (level.IsMultiplayerSecondaryPlayer)
			{
				x = forcedRestoreXPosition;
			}
			else
			{
				(level as LevelMultiplayer).CharacterWasRestored(this, x);
			}
		}
		Vector3 positionOnFloor = level.Location.GetPositionOnFloor(x);
		float num = x - CachedTransform.position.x;
		float num2 = positionOnFloor.y - CachedTransform.position.y;
		CachedTransform.position = new Vector3(positionOnFloor.x, positionOnFloor.y, CachedTransform.position.z);
		Rigidbody[] rigidbodyArray = ragdoll.GetRigidbodyArray();
		Vector3 vector = new Vector3(0f - num, 0f - num2, 0f);
		int i = 0;
		for (int num3 = rigidbodyArray.Length; i < num3; i++)
		{
			rigidbodyArray[i].transform.position += vector;
		}
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

	private float DamageRatioAtBodyPart(BodyPartsHelper.BodyPartsType part)
	{
		float result = 1f;
		switch (part)
		{
		case BodyPartsHelper.BodyPartsType.BodyPartsTypeHead:
			result = 1.75f;
			break;
		case BodyPartsHelper.BodyPartsType.BodyPartsTypeLegs:
			result = 0.75f;
			break;
		}
		return result;
	}

	private void RestoreAction()
	{
		isRestoreNeeded = false;
		IsCharacterDragSoundPlaying = false;
		ShiftCharacterOnRestore();
		RemoveDriveIntoLegsWeapons();
		skeletonAnimation.Skeleton.SetBonesToSetupPose();
		animationCache.AnimationState.SetAnimation(0, animationCache.StandUpAnimation, false);
		TrackEntry current = animationCache.AnimationState.GetCurrent(6);
		if (current != null && current.Animation == animationCache.ShockAnimation)
		{
			current = animationCache.AnimationState.SetAnimation(6, animationCache.ShockAnimation, false);
			current.Time = current.Animation.Duration;
		}
		if (health <= 0f)
		{
			animationCache.AnimationState.AddAnimation(0, animationCache.FinishHimAnimation, true, 0f);
			IsSuicide = this == CachedManagers.CachedLevelsManager.CurrentLevel.ActiveCharacter;
		}
		else
		{
			animationCache.AnimationState.SetAnimation(1, animationCache.FaceKOAnimation, false);
			animationCache.AnimationState.AddAnimation(0, animationCache.IdleAnimation, true, 0f);
		}
		level.TurnActionsChangeCount(-1);
		RemoveRagdoll();
	}

	public void RestoreActionFatality()
	{
		isRestoreNeeded = false;
		IsCharacterDragSoundPlaying = false;
		RemoveDriveIntoLegsWeapons();
		skeletonAnimation.Skeleton.SetBonesToSetupPose();
		animationCache.AnimationState.SetAnimation(0, animationCache.StandUpAnimation, false);
		TrackEntry current = animationCache.AnimationState.GetCurrent(6);
		if (current != null && current.Animation == animationCache.ShockAnimation)
		{
			current = animationCache.AnimationState.SetAnimation(6, animationCache.ShockAnimation, false);
			current.Time = current.Animation.Duration;
		}
		animationCache.AnimationState.AddAnimation(0, animationCache.FinishHimAnimation, true, 0f);
		RemoveRagdoll();
	}

	private void RemoveRagdoll()
	{
		if (!level.IsMultiplayerSecondaryPlayer)
		{
			ragdoll.useGravity = true;
		}
		if (ragdoll.IsActive)
		{
			ragdoll.colliderLayer = base.gameObject.layer;
			ragdoll.Remove();
		}
	}

	private void RemoveDriveIntoLegsWeapons()
	{
		int i = 0;
		for (int count = drivenIntoLegsWeapons.Count; i < count; i++)
		{
			WeaponGeneral weaponGeneral = drivenIntoLegsWeapons[i];
			if (weaponGeneral != null)
			{
				weaponGeneral.RemoveFromCharacter();
			}
		}
		drivenIntoLegsWeapons.Clear();
	}

	private IEnumerator StopElectricityEffectWithDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (electricityEffectHead != null)
		{
			electricityEffectHead.StopFX(false);
			UnityEngine.Object.Destroy(electricityEffectHead);
			electricityEffectHead = null;
		}
		if (electricityEffectBody != null)
		{
			electricityEffectBody.StopFX(false);
			UnityEngine.Object.Destroy(electricityEffectBody);
			electricityEffectBody = null;
		}
	}

	private IEnumerator StartFatalityWithDelay(float delay, CollisionInfo collisioinInfo)
	{
		yield return new WaitForSeconds(delay);
		fatalityManager.StartFatality(collisioinInfo);
	}

	protected virtual void AnimationState_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
	}

	protected virtual void AnimationStart_Event(Spine.AnimationState state, int trackIndex)
	{
	}

	protected virtual void AnimationComplete_Event(Spine.AnimationState state, int trackIndex, int loopCount)
	{
	}

	protected virtual void FatalityManager_OnFatalityEnd(WeaponType type)
	{
		if (IsFinishHim)
		{
			IsFinishHim = false;
		}
	}

	private void Level_OnPlayerChanged(int number)
	{
		Character character = this as Character;
		if (!(character == SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.InactiveArcher) || characterMode != CharacterMode.Alternative)
		{
			return;
		}
		isSecondCharacterActive = !isSecondCharacterActive;
		animationCache.AnimationState.Event -= AnimationState_Event;
		animationCache.AnimationState.Start -= AnimationStart_Event;
		animationCache.AnimationState.Complete -= AnimationComplete_Event;
		tk2dSpriteCollectionData[] spriteCollections = animationCache.SkeletonAnimation.SkeletonDataAsset.spriteCollections;
		if (spriteCollections != null)
		{
			for (int i = 0; i < spriteCollections.Length; i++)
			{
				if (spriteCollections[i] == spriteCollectionData1 || spriteCollections[i] == spriteCollectionData2)
				{
					spriteCollections[i] = ((!isSecondCharacterActive) ? spriteCollectionData1 : spriteCollectionData2);
				}
			}
		}
		animationCache.SkeletonAnimation.SkeletonDataAsset.Reset();
		SkeletonAnimation.Reset();
		SkeletonAnimation.Initialize();
		SkeletonAnimation.Update();
		animationCache.AnimationState.SetAnimation(0, animationCache.IdleAnimation, false);
		animationCache.AnimationState.Event += AnimationState_Event;
		animationCache.AnimationState.Start += AnimationStart_Event;
		animationCache.AnimationState.Complete += AnimationComplete_Event;
	}

	public virtual bool OnCollision(CollisionInfo collisionInfo)
	{
		if (IsFinishHim && !collisionInfo.isDisableFatality)
		{
			fatalityManager.StartFatality(collisionInfo);
		}
		return true;
	}

	public void AddPhysicsObserver()
	{
		Rigidbody rootRigidBody = RootRigidBody;
		if ((bool)rootRigidBody && rootRigidBody.gameObject.GetComponent<ObserverPhysics>() == null)
		{
			ObserverPhysics observerPhysics = rootRigidBody.gameObject.AddComponent<ObserverPhysics>();
			observerPhysics.EventCollisionEnter = RagdollCollideEnter;
			observerPhysics.EventCollisionStay = RagdollCollideStay;
			observerPhysics.EventCollisionExit = RagdollCollideExit;
		}
	}

	public virtual void ApplyDamage(CollisionInfo collisionInfo)
	{
		ApplyDamageAnimation();
		if (!collisionInfo.isDontApplyRagdoll)
		{
			ApplyCharacterRagdoll();
		}
		AddPhysicsObserver();
		float damage = collisionInfo.damage;
		BodyPartsHelper.BodyPartsType part = BodyPartsHelper.TypeByName(collisionInfo.boneName);
		damage *= DamageRatioAtBodyPart(part);
		Health -= damage;
		if (damage > minDamageForPainSound)
		{
			if (characterVoiceType == CharacterVoiceType.Woman)
			{
				AudioManager.PlayAudioWithKey("character_woman_pain");
			}
			else if (characterVoiceType == CharacterVoiceType.ManBeefy)
			{
				AudioManager.PlayAudioWithKey("character_man_beefy_pain");
			}
		}
		if (!collisionInfo.isDisableRestore && !collisionInfo.isDontApplyRagdoll)
		{
			if (!isRestoreNeeded)
			{
				level.TurnActionsChangeCount(1);
			}
			isRestoreNeeded = !IsFinishHim;
		}
		if ((bool)masksManager)
		{
			masksManager.ApplyDamage(collisionInfo);
		}
		AudioManager.PlayAudioWithKey("character_hit_body");
		if (this != level.ActiveCharacter && (level.GameType == GameType.VSComputer || level.GameType == GameType.Tournament))
		{
			CachedManagers.CachedTutorialManager.IsVSCompTutorialInProgress = false;
		}
		if (IsAppleBoy)
		{
			(level as LevelAppleShoot).IsAppleBoyWasDamaged = true;
		}
	}

	public virtual void ApplyForce(float force, ForceMode forceMode, CollisionInfo collisionInfo)
	{
		BodyPartsHelper.BodyPartsType bodyPartsType = BodyPartsHelper.TypeByName(collisionInfo.boneName);
		if (ragdoll.IsActive && bodyPartsType != BodyPartsHelper.BodyPartsType.BodyPartsTypeNone)
		{
			bool flag = bodyPartsType == BodyPartsHelper.BodyPartsType.BodyPartsTypeHead;
			Rigidbody rootRigidBody = RootRigidBody;
			Rigidbody headRigidBody = HeadRigidBody;
			Rigidbody rigidbody = ((!flag) ? rootRigidBody : headRigidBody);
			Rigidbody rigidbody2 = ((!flag) ? headRigidBody : rootRigidBody);
			float num = 0.8f;
			float num2 = 0.5f;
			float num3 = Mathf.Clamp(collisionInfo.speedRatio, 0.3f, 1f);
			Vector3 force2 = collisionInfo.direction * force * num * num3;
			Vector3 force3 = new Vector3(force2.x * num2, 0f, 0f);
			rigidbody.AddForce(force2, forceMode);
			rigidbody2.AddForce(force3, forceMode);
			float z = 2f;
			Vector3 torque = new Vector3(0f, 0f, z);
			rootRigidBody.AddTorque(torque);
		}
	}

	public virtual bool IsAffectedInRadius(Vector3 position, float radius)
	{
		return AffectedObjects(position, radius).Count > 0;
	}

	public virtual List<Collider> AffectedObjects(Vector3 position, float radius)
	{
		affectedObjects.Clear();
		List<Collider> list = new List<Collider>();
		GetComponentsInChildren(list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			WeaponGeneral componentInParent = list[num].gameObject.GetComponentInParent<WeaponGeneral>();
			if (componentInParent != null)
			{
				list.RemoveAt(num);
			}
		}
		float num2 = radius * radius;
		for (int i = 0; i < list.Count; i++)
		{
			Collider collider = list[i];
			Vector3 vector = collider.bounds.ClosestPoint(position);
			if ((vector - position).sqrMagnitude <= num2)
			{
				affectedObjects.Add(collider);
				break;
			}
		}
		return affectedObjects;
	}

	public void ForceHealth(float forcedHealth)
	{
		lastTurnDamageAmount += health - forcedHealth;
		health = forcedHealth;
		UpdateHealthBar();
	}

	public void ForceRestore(float restoreXPosition)
	{
		forcedRestoreXPosition = restoreXPosition;
		RestoreAction();
	}

	public void ForceRagdollTransform(List<Vector3> forcedRagdollPositions, List<Vector3> forcedRagdollRotations)
	{
		if (forcedRagdollPositions.Count > 0 && forcedRagdollPositions.Count == RagdollRigidbodies.Length)
		{
			for (int i = 0; i < RagdollRigidbodies.Length; i++)
			{
				RagdollRigidbodies[i].transform.position = forcedRagdollPositions[i];
				RagdollRigidbodies[i].transform.rotation = Quaternion.Euler(forcedRagdollRotations[i]);
			}
		}
	}
}
