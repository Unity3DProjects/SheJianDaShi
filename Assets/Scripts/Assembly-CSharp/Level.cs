using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;
using Debug = UnityEngine.Debug;

public class Level : MonoBehaviour
{
	private struct LevelBodyShotsStatistics
	{
		public int headShots;

		public int bodyShots;

		public int legShots;
	}

	protected struct WeaponUnfollowBounds
	{
		public float min;

		public float max;
	}

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<int> m_OnPlayerChanged;

	protected const int LOADED_CHARACTERS_SPRITE_COLLECTIONS_COUNT = 5;

	private const float RAYCAST_MISTERY_BOX_LENGTH = 10000f;

	private const int RAYCAST_MISTERY_BOX_COUNT = 1;

	[SerializeField]
	protected Transform charactersTransfrom;

	[SerializeField]
	protected Transform weaponsTransform;

	[SerializeField]
	protected Transform fxsTransform;

	[SerializeField]
	protected GameType gameType;

	[SerializeField]
	protected bool isSecondArcherEnabled;

	[SerializeField]
	protected bool isForceTurnsChangeDisabled = true;

	[SerializeField]
	protected bool isTurnable = true;

	[SerializeField]
	protected bool isForcedSwitchPlayersEnabled;

	[SerializeField]
	protected float minEnemySpotDistance = 1040f;

	[SerializeField]
	protected float maxEnemySpotDistance = 1240f;

	[SerializeField]
	protected float weaponLimitLow = -600f;

	[SerializeField]
	protected float weaponLimitHigh = 2000f;

	[SerializeField]
	protected float cameraMoveToWinnerDuration = 2f;

	[SerializeField]
	protected AnimationCurve cameraMoveToWinnerCurve;

	[SerializeField]
	private ArrowDragger arrowDraggerPrefab;

	[SerializeField]
	private int weaponsCacheSize = 10;

	[SerializeField]
	private int emittedObjectsCacheSize = 10;

	[SerializeField]
	protected int drivenIntoWeaponsCacheSize = 1;

	[SerializeField]
	protected float weaponDeactivationDistance;

	[SerializeField]
	protected LevelCameraSettings cameraSettings;

	[SerializeField]
	private FloorSettings[] floorSettings;

	[SerializeField]
	private float turnTimerClockAnimationDuration;

	[SerializeField]
	private MisteryBox misteryBoxPrefab;

	[SerializeField]
	private bool isMisteryBoxAvailable;

	[SerializeField]
	private int misteryBoxMinMatchesCount = 5;

	[SerializeField]
	private Vector3 misteryBoxLeftSpawnPoint;

	[SerializeField]
	private Vector3 misteryBoxRightSpawnPoint;

	protected float turnTimer;

	protected float activeTurnTimeoutTimer;

	protected const float maxActiveTurnTime = 15f;

	protected float cachedTurnTimerDuration = float.MaxValue;

	protected float cachedTurnTimerFinishHimDuration = float.MaxValue;

	protected bool isLevelFinished;

	protected Character characterFirst;

	protected Character characterSecond;

	protected Location location;

	protected WeaponGeneral currentWeapon;

	protected WeaponGeneral cameraTargetWeapon;

	protected List<WeaponGeneral> weapons = new List<WeaponGeneral>();

	protected List<WeaponGeneral> removedWeapons = new List<WeaponGeneral>();

	protected List<EmittedObject> emittedObjects = new List<EmittedObject>();

	protected List<EmittedObject> removedEmittedObjects = new List<EmittedObject>();

	protected ObjectsSpawner objectsSpawner;

	protected Character activeCharacter;

	protected ArrowDragger arrowDragger;

	protected CameraController cameraController;

	private Managers cachedManagers;

	protected bool isWeaponReleased;

	protected float multiplier;

	protected GameScreen gameScreen;

	protected bool wasWeaponCreated;

	private DistanceMark distanceMark;

	private bool isDistanceMarkEnabled = true;

	private LevelBodyShotsStatistics bodyShotsStatistics = default(LevelBodyShotsStatistics);

	private int turnActiveActions;

	private int earnedCoins;

	private bool isYoursNearWeaponsChecked;

	private float isYoursNearWeaponsCheckingTime;

	private bool isOthersNearWeaponsChecked;

	private Character lostCharacter;

	private bool isCoinsEnoughForPurchase;

	private bool isActiveForcedSwitchPlayerTimer;

	protected List<CharacterNameType> charactersWithLoadedSpriteCollections = new List<CharacterNameType>();

	private int misteryBoxLayerMask;

	private MisteryBox misteryBox;

	private bool isMistreryBoxSpawned;

	private RaycastHit[] results = new RaycastHit[1];

	private bool isWeaponActive;

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

	public GameType GameType
	{
		get
		{
			return gameType;
		}
	}

	public Character ActiveCharacter
	{
		get
		{
			return activeCharacter;
		}
	}

	public Character InactiveArcher
	{
		get
		{
			return (!(activeCharacter == characterFirst)) ? characterFirst : characterSecond;
		}
	}

	public Character CharacterFirst
	{
		get
		{
			return characterFirst;
		}
	}

	public Character CharacterSecond
	{
		get
		{
			return characterSecond;
		}
	}

	public WeaponGeneral CurrentWeapon
	{
		get
		{
			return currentWeapon;
		}
	}

	public Location Location
	{
		get
		{
			return location;
		}
	}

	public bool IsTurnable
	{
		get
		{
			return isTurnable;
		}
	}

	public Rect Bounds
	{
		get
		{
			float num = WeaponTargetUnfollowBounds.min - 600f;
			float num2 = WeaponTargetUnfollowBounds.max + 600f;
			float num3 = weaponLimitLow;
			float num4 = weaponLimitHigh;
			return new Rect(num, num3, num2 - num, num4 - num3);
		}
	}

	public Transform FXsTransform
	{
		get
		{
			return fxsTransform;
		}
	}

	public bool IsWeaponReleased
	{
		get
		{
			return isWeaponReleased;
		}
	}

	public bool IsActiveForcedSwitchPlayerTimer
	{
		get
		{
			return isActiveForcedSwitchPlayerTimer && isForcedSwitchPlayersEnabled;
		}
		set
		{
			isActiveForcedSwitchPlayerTimer = value;
		}
	}

	protected virtual WeaponUnfollowBounds WeaponTargetUnfollowBounds
	{
		get
		{
			WeaponUnfollowBounds result = default(WeaponUnfollowBounds);
			if ((bool)characterSecond)
			{
				bool flag = characterSecond.CachedTransform.position.x > characterFirst.CachedTransform.position.x;
				Character character = ((!flag) ? characterSecond : characterFirst);
				Character character2 = ((!flag) ? characterFirst : characterSecond);
				result.min = character.CachedTransform.position.x - weaponDeactivationDistance;
				result.max = character2.CachedTransform.position.x + weaponDeactivationDistance;
			}
			else
			{
				result.min = characterFirst.CachedTransform.position.x - weaponDeactivationDistance;
				result.max = characterFirst.CachedTransform.position.x + weaponDeactivationDistance;
			}
			return result;
		}
	}

	private int TurnActiveActions
	{
		get
		{
			return turnActiveActions;
		}
		set
		{
			if (turnActiveActions != value)
			{
				turnActiveActions = value;
				if (turnActiveActions == 0 && isWeaponReleased)
				{
					TurnFinished();
				}
			}
		}
	}

	public LevelCameraSettings CameraSettings
	{
		get
		{
			return cameraSettings;
		}
	}

	public int MissesCount { get; set; }

	public bool IsLevelFinished
	{
		get
		{
			return isLevelFinished;
		}
	}

	protected Vector2 CharacterOffset
	{
		get
		{
			Vector2 characterOffset = cameraSettings.characterOffset;
			if (CachedManagers.CachedGUIOrientationManager.CurrentRotationType == GUILayouterRotationType.Landscape)
			{
				characterOffset += new Vector2(0f, -150f);
			}
			return characterOffset;
		}
	}

	public Character LostCharacter
	{
		get
		{
			return lostCharacter;
		}
	}

	public float CameraMoveToWinnerDuration
	{
		get
		{
			return cameraMoveToWinnerDuration;
		}
	}

	protected virtual int MatchesForFloorSettings
	{
		get
		{
			return CachedManagers.CachedStatisticsManager.WinsCount + CachedManagers.CachedStatisticsManager.MatchesCountVSHuman;
		}
	}

	protected virtual FloorSettings[] LevelFloorSettings
	{
		get
		{
			return floorSettings;
		}
	}

	protected float CachedTurnTimerDuration
	{
		get
		{
			if (cachedTurnTimerDuration == float.MaxValue)
			{
				cachedTurnTimerDuration = CachedManagers.CachedMultiplayerManager.TurnTimerDuration;
			}
			return cachedTurnTimerDuration;
		}
	}

	protected float CachedTurnTimerFinishHimDuration
	{
		get
		{
			if (cachedTurnTimerFinishHimDuration == float.MaxValue)
			{
				cachedTurnTimerFinishHimDuration = CachedManagers.CachedMultiplayerManager.FinishHimTimerDuration;
			}
			return cachedTurnTimerFinishHimDuration;
		}
	}

	public bool IsMultiplayerSecondaryPlayer { get; private set; }

	public bool IsMainBattleFinished
	{
		get
		{
			return CharacterFirst.IsFinishHim || CharacterSecond.IsFinishHim || isLevelFinished;
		}
	}

	public bool IsNeedFollowArcherOrWeapon { get; set; }

	private bool IsMisteryBoxSpawnAvailable
	{
		get
		{
			return isMisteryBoxAvailable && CachedManagers.CachedStatisticsManager.MathesCount >= misteryBoxMinMatchesCount && CachedManagers.CachedRemoteManager.GetRemoteValueForKey("IsMisteryBoxEnabled", true) && CachedManagers.CachedStatisticsManager.SecondsSinceLastMisteryBox > CachedManagers.CachedRemoteManager.GetRemoteValueForKey("MisteryBoxReloadTime", 86400) && !CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		}
	}

	public static event Action<int> OnPlayerChanged
	{
		add
		{
			Action<int> action = Level.m_OnPlayerChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Level.m_OnPlayerChanged, (Action<int>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<int> action = Level.m_OnPlayerChanged;
			Action<int> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Level.m_OnPlayerChanged, (Action<int>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected virtual void OnEnable()
	{
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
		Tutorial.OnTutorialWillAppear += Tutorial_OnTutorialWillAppear;
	}

	protected virtual void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
		Tutorial.OnTutorialWillAppear -= Tutorial_OnTutorialWillAppear;
		arrowDragger.OnTwitchingApply -= ArrowDragger_OnTwitchingApply;
		arrowDragger.OnTwitchingDisable -= ArrowDragger_OnTwitchingDisable;
	}

	protected virtual void OnDestroy()
	{
		if (gameType == GameType.AppleShoot && (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown))
		{
			GUIOrientationManger instanceIfExist = SingletonMonoBehaviour<GUIOrientationManger>.InstanceIfExist;
			if (instanceIfExist != null)
			{
				instanceIfExist.RotateToPortrait();
				Screen.orientation = ScreenOrientation.Portrait;
			}
		}
		GameManager instanceIfExist2 = SingletonMonoBehaviour<GameManager>.InstanceIfExist;
		if (instanceIfExist2 != null)
		{
			CameraController cameraController = instanceIfExist2.CameraController;
			if (cameraController != null)
			{
				cameraController.RemoveBlurImmediately();
			}
		}
	}

	public virtual void Init(ObjectsSpawner objectsSpawner, CameraController cameraController)
	{
		if (gameType == GameType.AppleShoot)
		{
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			if (Input.deviceOrientation != DeviceOrientation.LandscapeLeft && Input.deviceOrientation != DeviceOrientation.LandscapeRight)
			{
				Screen.orientation = ScreenOrientation.LandscapeLeft;
			}
			CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
			{
				CachedManagers.CachedGUIOrientationManager.RotateToLandscape();
				Screen.orientation = ScreenOrientation.Unknown;
				Screen.orientation = ScreenOrientation.AutoRotation;
			}, 0f);
		}
		IsMultiplayerSecondaryPlayer = GameType == GameType.Multiplayer && !CachedManagers.CachedMultiplayerManager.IsMainPlayer;
		this.objectsSpawner = objectsSpawner;
		this.cameraController = cameraController;
		this.cameraController.LevelSettings = cameraSettings;
		multiplier = ((!tk2dSystem.IsRetina) ? 1f : 2f);
		CachedManagers.CachedStatisticsManager.Score = 0;
		CachedManagers.CachedMicroachievementsManager.ResetReachedLevelAchievements();
		arrowDragger = UnityEngine.Object.Instantiate(arrowDraggerPrefab);
		arrowDragger.transform.parent = base.transform;
		arrowDragger.OnTwitchingApply += ArrowDragger_OnTwitchingApply;
		arrowDragger.OnTwitchingDisable += ArrowDragger_OnTwitchingDisable;
		float scaleImmediately = ((!cameraSettings.isMoving) ? cameraSettings.size : (cameraSettings.size * 2f));
		cameraController.SetScaleImmediately(scaleImmediately);
		location = CachedManagers.CachedLocationsManager.GetLocation();
		location.transform.parent = base.transform;
		FloorSettings settings = CurrentFloorSettings(MatchesForFloorSettings);
		location.Init(settings);
		characterFirst = CreateBaseCharacter(false, objectsSpawner.FirstSelectedCharacter) as Character;
		if (!IsMultiplayerSecondaryPlayer)
		{
			CachedManagers.CachedStatisticsManager.LastCharacterNameType = objectsSpawner.FirstSelectedCharacterNameType;
		}
		else
		{
			CachedManagers.CachedStatisticsManager.LastCharacterNameType = objectsSpawner.SecondSelectedCharacterNameType;
		}
		activeCharacter = characterFirst;
		characterFirst.UpdateHealthBar();
		if (isSecondArcherEnabled)
		{
			characterSecond = CreateBaseCharacter(true, objectsSpawner.SecondSelectedCharacter) as Character;
			distanceMark = CachedManagers.CachedGUIScreenManager.GameScreen.DistanceMark;
			if ((bool)characterSecond)
			{
				characterSecond.UpdateHealthBar();
				NavigateToCharacter(characterSecond, 0f);
				CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						NextTurn();
					}
				}, 2f);
			}
		}
		else if (!cameraSettings.isMoving)
		{
			NavigateToCharacter(characterFirst, 0f);
		}
		if (cameraSettings.isMoving)
		{
			cameraController.SetScaleWithDuration(cameraSettings.size, 2f);
		}
		SetVolumeForSound("theme_main", 1f);
		SetVolumeForSound("theme_result_screen", 0f);
		isCoinsEnoughForPurchase = CachedManagers.CachedShopManager.IsCoinsEnoughForPurchase();
		gameScreen = CachedManagers.CachedGUIScreenManager.GameScreen;
		IsNeedFollowArcherOrWeapon = true;
		if (gameType == GameType.Tournament || gameType == GameType.VSComputer || gameType == GameType.Multiplayer)
		{
			ShopItemCharacter shopItemCharacter = objectsSpawner.GetShopItemCharacter(CachedManagers.CachedStatisticsManager.LastCharacterNameType);
			if (shopItemCharacter != null)
			{
				Analytics.CustomEvent("Choose_Character_" + shopItemCharacter.GetAnalyticsKey());
			}
		}
		else if (gameType == GameType.VSHuman)
		{
			ShopItemCharacter shopItemCharacter2 = objectsSpawner.GetShopItemCharacter(objectsSpawner.FirstSelectedCharacterNameType);
			if (shopItemCharacter2 != null)
			{
				Analytics.CustomEvent("Choose_Character_" + shopItemCharacter2.GetAnalyticsKey());
			}
			shopItemCharacter2 = objectsSpawner.GetShopItemCharacter(objectsSpawner.SecondSelectedCharacterNameType);
			if (shopItemCharacter2 != null)
			{
				Analytics.CustomEvent("Choose_Character_" + shopItemCharacter2.GetAnalyticsKey());
			}
		}
		if (!(misteryBoxPrefab != null) || !IsMisteryBoxSpawnAvailable)
		{
			return;
		}
		misteryBoxLayerMask = LayerMask.GetMask("MisteryBox");
		misteryBox = UnityEngine.Object.Instantiate(misteryBoxPrefab);
		misteryBox.transform.parent = base.transform;
		misteryBox.transform.position = characterFirst.transform.position;
		float x = misteryBox.transform.position.x;
		float x2 = misteryBox.MisteryBoxCollider.bounds.size.x;
		float num = x - x2 / 2f;
		float num2 = x + x2 / 2f;
		if (location.GetPositionOnFloor(num + misteryBoxLeftSpawnPoint.x).y == location.GetPositionOnFloor(num2 + misteryBoxLeftSpawnPoint.x).y)
		{
			if (location.GetPositionOnFloor(num + misteryBoxRightSpawnPoint.x).y == location.GetPositionOnFloor(num2 + misteryBoxRightSpawnPoint.x).y)
			{
				misteryBox.transform.position += ((!(UnityEngine.Random.value < 0.5f)) ? misteryBoxRightSpawnPoint : misteryBoxLeftSpawnPoint);
			}
			else
			{
				misteryBox.transform.position += misteryBoxLeftSpawnPoint;
			}
		}
		else if (location.GetPositionOnFloor(num + misteryBoxRightSpawnPoint.x).y == location.GetPositionOnFloor(num2 + misteryBoxRightSpawnPoint.x).y)
		{
			misteryBox.transform.position += misteryBoxRightSpawnPoint;
		}
		else if (Mathf.Abs(location.GetPositionOnFloor(num + misteryBoxLeftSpawnPoint.x).y - location.GetPositionOnFloor(num2 + misteryBoxLeftSpawnPoint.x).y) <= Mathf.Abs(location.GetPositionOnFloor(num + misteryBoxRightSpawnPoint.x).y - location.GetPositionOnFloor(num2 + misteryBoxRightSpawnPoint.x).y))
		{
			misteryBox.transform.position += misteryBoxLeftSpawnPoint;
		}
		else
		{
			misteryBox.transform.position += misteryBoxRightSpawnPoint;
		}
		CachedManagers.CachedStatisticsManager.LastMisteryBox = DateTime.Now;
		misteryBox.OnOpenEnd += MisteryBox_OnOpenEnd;
	}

	public virtual void UpdateLevel(float deltaTime)
	{
		if (IsActiveForcedSwitchPlayerTimer && gameScreen != null)
		{
			SkeletonAnimation[] clockAnimations = gameScreen.TurnTimer.ClockAnimations;
			for (int i = 0; i < clockAnimations.Length; i++)
			{
				clockAnimations[i].ignoreMonoUpdate = true;
			}
			float num = ((!CharacterFirst.IsFinishHim && !CharacterSecond.IsFinishHim) ? CachedTurnTimerDuration : CachedTurnTimerFinishHimDuration);
			UpdateLevelWithForcedSwitchPlayer(deltaTime, num);
			if (num - turnTimer < turnTimerClockAnimationDuration)
			{
				for (int j = 0; j < clockAnimations.Length; j++)
				{
					clockAnimations[j].ignoreMonoUpdate = false;
				}
			}
		}
		if (characterFirst != null)
		{
			characterFirst.UpdateCharacter(deltaTime);
		}
		if (characterSecond != null)
		{
			characterSecond.UpdateCharacter(deltaTime);
		}
		if (currentWeapon != null)
		{
			if (currentWeapon.isFlying && !IsMultiplayerSecondaryPlayer)
			{
				if (!isOthersNearWeaponsChecked && (bool)InactiveArcher && ((Vector2)(currentWeapon.CachedTransform.position - InactiveArcher.CachedTransform.position)).sqrMagnitude < 250000f)
				{
					isOthersNearWeaponsChecked = true;
					InactiveArcher.SetAnimationWithType(CharacterAnimationType.EmoteScare, 50f);
				}
				if (!isYoursNearWeaponsChecked && ((Vector2)(currentWeapon.CachedTransform.position - ActiveCharacter.CachedTransform.position)).sqrMagnitude < 250000f)
				{
					isYoursNearWeaponsCheckingTime += deltaTime;
					if (isYoursNearWeaponsCheckingTime > 5f)
					{
						isYoursNearWeaponsChecked = true;
						ActiveCharacter.SetAnimationWithType(CharacterAnimationType.EmoteScare, 50f);
					}
				}
			}
			if (distanceMark != null)
			{
				RefreshDistanceMark();
			}
		}
		else if (distanceMark != null)
		{
			distanceMark.IsVisible = false;
		}
		if (weapons != null)
		{
			for (int num2 = weapons.Count - 1; num2 >= 0; num2--)
			{
				WeaponGeneral weaponGeneral = weapons[num2];
				if (weaponGeneral != null)
				{
					weaponGeneral.UpdateWeapon(deltaTime);
				}
				else
				{
					weapons.RemoveAt(num2);
				}
			}
		}
		int k = 0;
		for (int count = removedWeapons.Count; k < count; k++)
		{
			WeaponGeneral weaponGeneral2 = removedWeapons[k];
			if (weaponGeneral2 != null)
			{
				if (ActiveCharacter != null)
				{
					ActiveCharacter.RemoveDriveIntoWeapon(weaponGeneral2);
				}
				if (InactiveArcher != null)
				{
					InactiveArcher.RemoveDriveIntoWeapon(weaponGeneral2);
				}
				UnityEngine.Object.Destroy(weaponGeneral2.gameObject);
			}
		}
		removedWeapons.Clear();
		if (emittedObjects != null)
		{
			int l = 0;
			for (int count2 = emittedObjects.Count; l < count2; l++)
			{
				emittedObjects[l].UpdateObject(deltaTime);
			}
		}
		if (removedEmittedObjects != null)
		{
			int m = 0;
			for (int count3 = removedEmittedObjects.Count; m < count3; m++)
			{
				UnityEngine.Object.Destroy(removedEmittedObjects[m].gameObject);
			}
			removedEmittedObjects.Clear();
		}
		if (IsNeedFollowArcherOrWeapon && cameraSettings.isMoving)
		{
			if (InactiveArcher != null && (InactiveArcher.RootRigidBody != null || (bool)InactiveArcher.FatalityManager.ActiveFatality || InactiveArcher.IsPeriodicalDamageAnimationActive()))
			{
				UnfollowWeaponWithVelocity(Vector2.zero);
			}
			else if (cameraTargetWeapon != null && cameraTargetWeapon.IsActive)
			{
				cameraController.UpdateScaleWithWeaponSpeed(cameraTargetWeapon.Velocity.magnitude);
				float x = cameraTargetWeapon.CachedTransform.position.x;
				if (x < WeaponTargetUnfollowBounds.min || x > WeaponTargetUnfollowBounds.max)
				{
					UnfollowWeaponWithVelocity(cameraTargetWeapon.Velocity);
				}
			}
		}
		if (arrowDragger != null)
		{
			arrowDragger.CustomUpdate(deltaTime);
		}
		CachedManagers.CachedTutorialManager.UpdateTutorialManager(deltaTime);
		if (isWeaponReleased && (activeTurnTimeoutTimer += deltaTime) > 15f)
		{
			TurnActiveActions = 0;
		}
	}

	public void UpdateLevelLocation(float deltaTime)
	{
		if (location != null)
		{
			location.UpdateLocation(deltaTime);
		}
	}

	public void RemoveLevel()
	{
		if (distanceMark != null)
		{
			distanceMark.gameObject.SetActive(false);
		}
		arrowDragger.CancelTouch();
	}

	public void RemoveWeapon(WeaponGeneral weapon)
	{
		removedWeapons.Add(weapon);
	}

	public virtual void ReleaseWeapon()
	{
		activeTurnTimeoutTimer = 0f;
		ActiveCharacter.PlayReleaseAnimation();
	}

	public void AddEmittedWeapon(WeaponGeneral emittedWeapon)
	{
		emittedWeapon.CachedTransform.parent = weaponsTransform;
		emittedWeapon.Init(this);
		emittedWeapon.IsEmittedWeapon = true;
		weapons.Add(emittedWeapon);
	}

	public void AddEmittedObject(EmittedObject emittedObject)
	{
		emittedObject.CachedTransform.parent = weaponsTransform;
		emittedObjects.Add(emittedObject);
		if (emittedObjects.Count > emittedObjectsCacheSize)
		{
			emittedObjects[emittedObjects.Count - emittedObjectsCacheSize].RemoveObject();
		}
	}

	public void RemoveEmittedObject(EmittedObject emittedObject)
	{
		removedEmittedObjects.Add(emittedObject);
	}

	public virtual void WeaponShoot(WeaponGeneral weapon, Character characterShoot)
	{
		currentWeapon.CachedTransform.parent = weaponsTransform;
	}

	public void SetCameraTargetWeapon(WeaponGeneral weapon)
	{
		cameraTargetWeapon = weapon;
	}

	public virtual void Explode(ExplosionInfo explosionInfo)
	{
		if ((bool)InactiveArcher && !InactiveArcher.IsInvincible && InactiveArcher.IsAffectedInRadius(explosionInfo.position, explosionInfo.radius))
		{
			InactiveArcher.ApplyExplosionForce(explosionInfo);
		}
		if (!ActiveCharacter.IsInvincible && ActiveCharacter.IsAffectedInRadius(explosionInfo.position, explosionInfo.radius))
		{
			ActiveCharacter.ApplyExplosionForce(explosionInfo);
		}
	}

	public virtual void ApplyPeriodicalDamage(PeriodicalDamageInfo damageInfo)
	{
		if (!IsWeaponReleased)
		{
			return;
		}
		if ((bool)InactiveArcher && !InactiveArcher.IsInvincible)
		{
			List<Collider> list = InactiveArcher.AffectedObjects(damageInfo.position, damageInfo.radius);
			list.RemoveAll((Collider obj) => obj.gameObject.layer == LayerMask.NameToLayer("CharacterParts"));
			if (list.Count > 0)
			{
				InactiveArcher.DamageHandler.ApplyPeriodicalDamage(damageInfo, list.RandomObject());
			}
		}
		if (!ActiveCharacter.IsInvincible)
		{
			List<Collider> list2 = ActiveCharacter.AffectedObjects(damageInfo.position, damageInfo.radius);
			list2.RemoveAll((Collider obj) => obj.gameObject.layer == LayerMask.NameToLayer("CharacterParts"));
			if (list2.Count > 0)
			{
				ActiveCharacter.DamageHandler.ApplyPeriodicalDamage(damageInfo, list2.RandomObject());
			}
		}
	}

	public virtual void SwitchActiveArcher()
	{
		int obj = 1;
		if (activeCharacter == characterFirst)
		{
			if (characterSecond != null)
			{
				activeCharacter = characterSecond;
				activeCharacter.SetCollidersEnabled(false);
				characterFirst.SetCollidersEnabled(true);
				obj = 2;
			}
		}
		else
		{
			activeCharacter = characterFirst;
			activeCharacter.SetCollidersEnabled(false);
			characterSecond.SetCollidersEnabled(true);
		}
		if (currentWeapon != null)
		{
			RefreshDistanceMark(true);
		}
		isDistanceMarkEnabled = true;
		Level.m_OnPlayerChanged?.Invoke(obj);
	}

	public void UpdateArcherPosition()
	{
		isYoursNearWeaponsChecked = false;
		isOthersNearWeaponsChecked = false;
		activeCharacter.UpdatePosition(activeCharacter.CachedTransform.position);
	}

	public void TurnFinished(float delay = 0.5f)
	{
		if (ActiveCharacter.IsSuicide)
		{
			delay = 2.5f;
		}
		if (isLevelFinished || isWeaponActive)
		{
			return;
		}
		isWeaponActive = true;
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				NextTurn(IsTurnable);
				isWeaponActive = false;
			}
		}, delay);
	}

	public void TurnActionsChangeCount(int exchangeCount)
	{
		if (isForceTurnsChangeDisabled)
		{
			TurnActiveActions += exchangeCount;
		}
	}

	public virtual void OnCharacterDead(Character character)
	{
		isLevelFinished = true;
		if (ActiveCharacter.IsAI || ActiveCharacter.IsMultiplayerEnemy)
		{
			SwitchActiveArcher();
		}
		Character winningArcher = AnotherCharacter(character);
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				winningArcher.PlayHappyAnimation();
				NavigateToCharacter(winningArcher, cameraMoveToWinnerDuration, cameraMoveToWinnerCurve);
				if (cameraController != null)
				{
					cameraController.SetScaleWithDuration(1f, 2f);
				}
			}
		}, 1.5f);
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				EndLevel(character);
			}
		}, winningArcher.EndLevelDelay);
	}

	public virtual void EnableDragging()
	{
		if (ActiveCharacter == CharacterFirst)
		{
			arrowDragger.IsDraggingEnabled = true;
		}
	}

	public void IncreaseShotStatisticsForBodyType(BodyPartsHelper.BodyPartsType type)
	{
		switch (type)
		{
		case BodyPartsHelper.BodyPartsType.BodyPartsTypeHead:
			bodyShotsStatistics.headShots++;
			break;
		case BodyPartsHelper.BodyPartsType.BodyPartsTypeBody:
			bodyShotsStatistics.bodyShots++;
			break;
		}
	}

	public void WeaponCollidedWithObject(GameObject collidedObject)
	{
		if (IsCharacterOnScreen(ActiveCharacter) && (bool)InactiveArcher && collidedObject != InactiveArcher.gameObject)
		{
			ActiveCharacter.SetAnimationWithType(CharacterAnimationType.EmoteAnger, 30f);
		}
	}

	public void WeaponAlreadyReleased()
	{
		isWeaponReleased = true;
		if (currentWeapon != null && cameraSettings.isMoving)
		{
			cameraController.SetTarget(currentWeapon.CachedTransform, Vector2.zero, Vector2.zero, 0.5f, !currentWeapon.IsPhysical && gameType != GameType.AppleShoot);
		}
	}

	public virtual void UnfollowWeaponWithVelocity(Vector2 velocity, float duration = 3f)
	{
		if (InactiveArcher != null && cameraSettings.isMoving)
		{
			Transform transform = InactiveArcher.BodyTransform;
			Vector2 offset = CharacterOffset;
			if (!InactiveArcher.RootRigidBody)
			{
				float num = Mathf.Abs(transform.position.y - InactiveArcher.CachedTransform.position.y);
				transform = InactiveArcher.CachedTransform;
				offset = new Vector2(offset.x, offset.y + num);
			}
			if (cameraController.Target != transform)
			{
				cameraController.SetTarget(transform, velocity, offset, duration);
				cameraController.SetScaleWithDuration(1f, 1f);
			}
		}
		cameraTargetWeapon = null;
	}

	public bool IsCharacterOnScreen(Character character)
	{
		Vector2 vector = cameraController.CachedCamera.ScreenCamera.WorldToViewportPoint(character.CachedTransform.position);
		return vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f;
	}

	public virtual void SetWeapon(WeaponType type, int activeButtonNumber)
	{
	}

	public virtual void WeaponWasActivatedByTap(WeaponGeneral weapon)
	{
	}

	public void ForceEmittedObjectsCollidersDisable()
	{
		if (emittedObjects == null)
		{
			return;
		}
		for (int i = 0; i < emittedObjects.Count; i++)
		{
			if (!(emittedObjects[i] != null))
			{
				continue;
			}
			Collider[] componentsInChildren = emittedObjects[i].GetComponentsInChildren<Collider>();
			if (componentsInChildren != null)
			{
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].enabled = false;
				}
			}
		}
	}

	public void TouchOnMisteryBox(TouchInfo touch)
	{
		if (!(misteryBox != null))
		{
			return;
		}
		int num = Physics.RaycastNonAlloc(CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.ScreenToWorldPoint(touch.position), Vector3.forward, results, 10000f, misteryBoxLayerMask);
		for (int i = 0; i < num; i++)
		{
			MisteryBox componentInParent = results[i].collider.GetComponentInParent<MisteryBox>();
			if (componentInParent != null && CachedManagers.CachedGUIPopupManager.ActivePopup == null)
			{
				componentInParent.GetAward();
				break;
			}
		}
	}

	public void SpawnMisteryBox()
	{
		if (misteryBox != null && !isMistreryBoxSpawned)
		{
			misteryBox.Appear();
			isMistreryBoxSpawned = true;
		}
	}

	public void CreateWeaponEffect(AssetLink effectLink, bool isActivatable, Vector3 position, Transform parent = null)
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (effectLink != null && !string.IsNullOrEmpty(effectLink.assetGUID))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate((GameObject)effectLink.GetAsset());
				if (isActivatable)
				{
					GameObject gameObject2 = (GameObject)effectLink.GetAsset();
					EmittedSystemInfo component = gameObject2.GetComponent<EmittedSystemInfo>();
					if ((bool)component && component.IsEmitAtLocaleSpace && (bool)currentWeapon)
					{
						gameObject.transform.parent = currentWeapon.transform;
					}
					else
					{
						gameObject.transform.parent = ((!(parent != null)) ? FXsTransform : parent);
					}
				}
				else
				{
					gameObject.transform.parent = ((!(parent != null)) ? FXsTransform : parent);
				}
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.position = ((!isActivatable || !currentWeapon) ? position : currentWeapon.CachedTransform.position);
			}
		}, 0f);
	}

	protected virtual void UpdateLevelWithForcedSwitchPlayer(float deltaTime, float timerDuration)
	{
		turnTimer += deltaTime;
		float value = (timerDuration - turnTimer) / timerDuration;
		if (gameScreen != null)
		{
			gameScreen.SetTurnTimerFill(value);
		}
		if (turnTimer > timerDuration && !IsMultiplayerSecondaryPlayer)
		{
			arrowDragger.CancelTouch();
			ActiveCharacter.TimeOutWithPrepareShooting();
			SwitchActiveArcher();
			UpdateArcherPosition();
			UpdateCameraPosition();
			CreateObjectsForTurn();
			turnTimer = 0f;
			IsActiveForcedSwitchPlayerTimer = false;
		}
	}

	protected virtual void UpdateCameraPosition()
	{
		if (ActiveCharacter.Health > 0f && !isLevelFinished)
		{
			wasWeaponCreated = true;
			CreateWeapon();
			ActiveCharacter.IsActive = true;
		}
		if (cameraSettings.isMoving)
		{
			NavigateToCharacter(activeCharacter, 1f);
			cameraController.SetScaleWithDuration(cameraSettings.size, 1f);
		}
		else
		{
			UpdateCameraFinished();
		}
	}

	protected virtual void CreateObjectsForTurn()
	{
	}

	protected virtual void NextTurn(bool switchArcher = false)
	{
		turnTimer = 0f;
		wasWeaponCreated = false;
		if (GameType == GameType.VSComputer || GameType == GameType.Tournament)
		{
			if (characterFirst.ShotsCount == 1 && characterSecond.HealthPercents != 100 && characterFirst == activeCharacter)
			{
				CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Bullseye);
			}
			if (characterFirst.ShotsCount == 1 && characterFirst == activeCharacter && gameScreen != null)
			{
				gameScreen.ShowPartyMastersAnnouncer();
			}
		}
		else if ((GameType == GameType.BirdsHunt || GameType == GameType.AppleShoot) && characterFirst.ShotsCount == 1 && CachedManagers.CachedStatisticsManager.Score > 0)
		{
			CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Bullseye);
		}
		if (switchArcher && CachedManagers.CachedTutorialManager.IsSwitchCharacterEnable)
		{
			SwitchActiveArcher();
		}
		else
		{
			ActiveCharacter.SetCollidersEnabled(false);
		}
		UpdateArcherPosition();
		UpdateCameraPosition();
		CreateObjectsForTurn();
	}

	protected virtual FloorSettings CurrentFloorSettings(int duelsCount)
	{
		FloorSettings floorSettings = null;
		for (int i = 0; i < LevelFloorSettings.Length; i++)
		{
			if (duelsCount >= LevelFloorSettings[i].winsCount && (floorSettings == null || floorSettings.winsCount < LevelFloorSettings[i].winsCount))
			{
				floorSettings = LevelFloorSettings[i];
			}
		}
		if (floorSettings == null)
		{
			Debug.LogError("FloorSettings array in Level is empty or wrong!");
		}
		return floorSettings;
	}

	protected virtual WeaponType CreateWeapon(WeaponType type = WeaponType.None)
	{
		WeaponType weaponType = ((type != WeaponType.None) ? type : CurrentWeaponType());
		if (weaponType != WeaponType.None)
		{
			currentWeapon = objectsSpawner.WeaponByType(weaponType);
			if ((bool)currentWeapon)
			{
				ActiveCharacter.WeaponAnchorTransform.localRotation = Quaternion.Euler(0f, 0f, currentWeapon.BoneAngle);
				currentWeapon.CachedTransform.parent = ActiveCharacter.WeaponAnchorTransform;
				currentWeapon.CachedTransform.position = ActiveCharacter.WeaponAnchorTransform.position;
				currentWeapon.CachedTransform.localPosition = currentWeapon.AnchorPointOffset;
				currentWeapon.CachedTransform.localRotation = Quaternion.identity;
				currentWeapon.CachedTransform.localScale = Vector3.one;
				currentWeapon.Init(this);
				weapons.Add(currentWeapon);
				SetCameraTargetWeapon(currentWeapon);
				if (weapons.Count > weaponsCacheSize)
				{
					RemoveWeapon(weapons[0]);
				}
			}
			isWeaponReleased = false;
		}
		else
		{
			EndLevel(ActiveCharacter);
		}
		return weaponType;
	}

	protected virtual WeaponType CurrentWeaponType()
	{
		WeaponType result = ActiveCharacter.AvailableWeapons.RandomObject();
		TutorialManager cachedTutorialManager = CachedManagers.CachedTutorialManager;
		if (cachedTutorialManager.IsVSCompTutorialInProgress && GameType == GameType.VSComputer)
		{
			result = cachedTutorialManager.TutorialWeaponDuel;
		}
		else if (cachedTutorialManager.IsAppleGameTutorialInProgress && GameType == GameType.AppleShoot)
		{
			result = cachedTutorialManager.TutorialWeaponApple;
		}
		else if (cachedTutorialManager.IsBirdsGameTutorialInProgress && GameType == GameType.BirdsHunt)
		{
			result = cachedTutorialManager.TutorialWeaponBirds;
		}
		return result;
	}

	protected void ShowResultScreen(Character loser)
	{
		StatisticsManager cachedStatisticsManager = CachedManagers.CachedStatisticsManager;
		cachedStatisticsManager.CountMatchesInDay++;
		if ((GameType == GameType.VSComputer && loser == CharacterFirst) || (GameType == GameType.Multiplayer && loser == (this as LevelMultiplayer).LocalPlayerCharacter))
		{
			AudioManager.PlayAudioWithKey("lose");
		}
		else
		{
			bool flag = true;
			if (GameType == GameType.VSComputer || GameType == GameType.VSHuman || GameType == GameType.Multiplayer)
			{
				AudioManager.PlayAudioWithKey("win");
			}
			else
			{
				flag = cachedStatisticsManager.Score >= ((GameType != GameType.AppleShoot) ? cachedStatisticsManager.HighScoreBirdsKilled : cachedStatisticsManager.HighScoreFruitsSmashed);
			}
			if (flag)
			{
				AudioManager.PlayAudioWithKey("crowd_glee_low_volume");
				CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						AudioManager.StopAudioWithKey("crowd_glee_low_volume", 2f);
					}
				}, 1f);
			}
		}
		lostCharacter = loser;
		CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
		{
			CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Result, delegate
			{
				CachedManagers.CachedGameManager.TrackGameEnd();
				CachedManagers.CachedGameManager.StopGame();
				CachedManagers.CachedProposalManager.ShowInterstitialResult();
			});
		});
	}

	protected virtual void EndLevel(Character loseCharacter)
	{
		if (GameType != GameType.VSHuman && GameType != GameType.Multiplayer)
		{
			if (GameType == GameType.VSComputer || GameType == GameType.Tournament)
			{
				if (loseCharacter != CharacterFirst)
				{
					if (CachedManagers.CachedStatisticsManager.DaysSinceLastFirstWin >= 1)
					{
						CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.FirstWin);
						CachedManagers.CachedStatisticsManager.LastFirstDailyWin = DateTime.Now;
					}
					CachedManagers.CachedStatisticsManager.WinsCount++;
					CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Win);
					CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.ImAPro, CachedManagers.CachedStatisticsManager.WinsCount);
					List<CharacterNameType> defeatedCharacters = CachedManagers.CachedStatisticsManager.DefeatedCharacters;
					if (defeatedCharacters.Count != objectsSpawner.CharactersNameTypes.Count)
					{
						defeatedCharacters.Add(loseCharacter.CharacterNameType);
						if (defeatedCharacters.Count == objectsSpawner.CharactersNameTypes.Count)
						{
							CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Domination);
						}
						CachedManagers.CachedStatisticsManager.DefeatedCharacters = defeatedCharacters;
					}
				}
				else
				{
					CachedManagers.CachedStatisticsManager.LosesCount++;
				}
			}
			if (MissesCount == 0)
			{
				CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.ShotSpee);
			}
			if (GameType == GameType.BirdsHunt)
			{
				CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Hunter);
			}
			else if (GameType == GameType.AppleShoot)
			{
				CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Sniper);
			}
			else if (GameType == GameType.VSComputer || GameType == GameType.Tournament)
			{
				CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Competitor);
				if (loseCharacter != CharacterFirst && CharacterFirst.HealthPercents < 20)
				{
					CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.TryHard);
				}
			}
		}
		else if (GameType == GameType.VSHuman)
		{
			CachedManagers.CachedStatisticsManager.MatchesCountVSHuman++;
		}
		else if (GameType == GameType.Multiplayer)
		{
			CachedManagers.CachedStatisticsManager.MatchesCountMultiplayer++;
			if (loseCharacter != (this as LevelMultiplayer).LocalPlayerCharacter)
			{
				CachedManagers.CachedStatisticsManager.MatchesWinsMultiplayer++;
				CachedManagers.CachedStatisticsManager.WinsCount++;
			}
			CachedManagers.CachedGUIPopupManager.HideActivePopup(true);
		}
		CachedManagers.CachedStatisticsManager.MathesCount++;
		if (isCoinsEnoughForPurchase && CachedManagers.CachedShopManager.IsCoinsEnoughForPurchase(CachedManagers.CachedMicroachievementsManager.CoinsFromAchievementsQueue))
		{
			CachedManagers.CachedTutorialManager.MatchesForCoinsReminder++;
		}
		else
		{
			CachedManagers.CachedTutorialManager.MatchesForCoinsReminder = 0;
		}
		ShowResultScreen(loseCharacter);
	}

	protected virtual CharacterBase CreateBaseCharacter(bool flipped, CharacterBase selectedCharacter, float forcedHealth = -1f)
	{
		Vector3 position = ((!flipped) ? LeftArcherPosition() : RightArcherPosition());
		CharacterBase characterBase = UnityEngine.Object.Instantiate(selectedCharacter);
		characterBase.CachedTransform.position = position;
		characterBase.CachedTransform.parent = charactersTransfrom;
		characterBase.CachedTransform.localPosition = new Vector3(characterBase.CachedTransform.localPosition.x, characterBase.CachedTransform.localPosition.y, 0f);
		characterBase.IsFlipped = flipped;
		characterBase.DrivenIntoWeaponsCacheSize = drivenIntoWeaponsCacheSize;
		characterBase.Init(this, forcedHealth);
		characterBase.PlayIdleAnimation();
		return characterBase;
	}

	protected virtual Vector3 LeftArcherPosition()
	{
		return location.GetCharacterPositionForRange(-2500f, -1500f);
	}

	protected virtual Vector3 RightArcherPosition()
	{
		return location.GetCharacterPositionForRange(CharacterFirst.CachedTransform.position.x + minEnemySpotDistance, CharacterFirst.CachedTransform.position.x + maxEnemySpotDistance);
	}

	protected virtual void UpdateCameraFinished()
	{
		if (!(this != null) || isLevelFinished)
		{
			return;
		}
		if (ActiveCharacter.IsFinishHim)
		{
			if (CachedManagers.CachedProposalManager.IsAvalaibleSecondFatalityChance && (GameType == GameType.VSComputer || GameType == GameType.Tournament) && ActiveCharacter == CharacterSecond)
			{
				CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.SecondFatalityChancePopup, true, null, true);
			}
			else
			{
				ActiveCharacter.KillCharacter();
			}
		}
		else if (!wasWeaponCreated)
		{
			CreateWeapon();
			wasWeaponCreated = true;
			ActiveCharacter.IsActive = true;
		}
		IsActiveForcedSwitchPlayerTimer = true;
	}

	protected Character AnotherCharacter(Character character)
	{
		return (!(character == CharacterFirst)) ? CharacterFirst : CharacterSecond;
	}

	protected void NavigateToCharacter(CharacterBase character, float duration = 0.5f, AnimationCurve curve = null)
	{
		Vector2 vector = DefaultCameraPositionForCharacter(character);
		if (duration < 0.05f)
		{
			cameraController.Position = vector;
		}
		else
		{
			cameraController.TweenMoveToPosition(vector, duration, UpdateCameraFinished, curve);
		}
	}

	public Vector2 DefaultCameraPositionForCharacter(CharacterBase character)
	{
		return (Vector2)character.BodyTransform.position + CharacterOffset;
	}

	protected void SetVolumeForSound(string soundName, float volume)
	{
		SoundObject[] playingAudioObjects = SoundsManager.GetPlayingAudioObjects(soundName);
		if (playingAudioObjects.Length == 0)
		{
			AudioManager.PlayAudioWithKey(soundName);
			playingAudioObjects = SoundsManager.GetPlayingAudioObjects(soundName);
		}
		SoundObject[] array = playingAudioObjects;
		foreach (SoundObject soundObject in array)
		{
			soundObject.Volume = volume;
		}
	}

	protected void PlayerChanged(int numberPlayer)
	{
		Level.m_OnPlayerChanged?.Invoke(numberPlayer);
	}

	protected CharacterNameType GetCharacterNameTypeForWeapon(WeaponType type)
	{
		CharacterNameType result = CharacterNameType.None;
		List<CharacterNameType> charactersNameTypes = objectsSpawner.CharactersNameTypes;
		for (int i = 0; i < charactersNameTypes.Count; i++)
		{
			Character character = objectsSpawner.GetCharacter(charactersNameTypes[i]);
			if (character.AvailableWeapons != null && character.AvailableWeapons.Length > 0 && character.AvailableWeapons[0] == type)
			{
				result = character.CharacterNameType;
				break;
			}
		}
		return result;
	}

	public virtual void IncreaseScore(int value = 1)
	{
		CachedManagers.CachedStatisticsManager.Score += value;
	}

	private void RefreshDistanceMark(bool isForced = false)
	{
		if (isWeaponReleased && !currentWeapon.isFlying)
		{
			isDistanceMarkEnabled = false;
		}
		if (isDistanceMarkEnabled || isForced)
		{
			float scale = cameraController.Scale;
			float guiScale = GUIScreenManager.GuiScale;
			Vector2 targetResolution = cameraController.CachedCamera.TargetResolution;
			float num = targetResolution.x * guiScale;
			float num2 = targetResolution.y * guiScale;
			Vector2 vector = cameraController.CachedCamera.transform.position - currentWeapon.CachedTransform.position;
			Vector2 vector2 = currentWeapon.CachedTransform.position;
			if (!currentWeapon.isFlying)
			{
				vector2 = (Vector2)ActiveCharacter.CachedTransform.position + CharacterOffset + vector;
			}
			Vector2 vector3 = (Vector2)InactiveArcher.CachedTransform.position + CharacterOffset + vector;
			Vector2 vector4 = vector2 - vector3;
			distanceMark.IsFlipped = vector4.x > 0f;
			float x = num * ((!distanceMark.IsFlipped) ? 0.9f : 0.1f);
			float y = Mathf.Clamp(num2 * 0.5f - vector4.y, num2 * 0.1f, num2 * 0.9f);
			distanceMark.Distance = (int)Mathf.Abs(vector4.x / 100f);
			distanceMark.transform.localPosition = new Vector3(x, y, distanceMark.transform.localPosition.z);
			distanceMark.IsVisible = Mathf.Abs(vector4.x) > num * scale * 0.5f;
		}
	}

	private int HammingWeight(long value)
	{
		long num = 0L;
		while (value != 0)
		{
			num += value & 1;
			value >>= 1;
		}
		return (int)num;
	}

	private void ArrowDragger_OnTwitchingApply()
	{
		if (currentWeapon.LaunchType == WeaponLaunchType.Bow)
		{
			activeCharacter.PlayTwitchAnimation();
		}
	}

	private void ArrowDragger_OnTwitchingDisable()
	{
		activeCharacter.UserAimingActionDetected();
		activeCharacter.UpdateCharacter(0f);
	}

	protected virtual void Tutorial_OnTutorialWillAppear()
	{
	}

	private void MisteryBox_OnOpenEnd()
	{
		misteryBox.OnOpenEnd -= MisteryBox_OnOpenEnd;
		UnityEngine.Object.Destroy(misteryBox.gameObject);
		misteryBox = null;
	}

	private void DebugFatality()
	{
		CachedManagers.CachedGameManager.CameraController.StartBlur();
		Character inactiveArcher = InactiveArcher;
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.otherCollider = inactiveArcher.HeadCollider;
		collisionInfo.position = base.transform.position;
		collisionInfo.fatality = currentWeapon.WeaponType;
		collisionInfo.fatalityActvationType = FatalityActivationType.AnyType;
		InactiveArcher.FatalityManager.StartFatality(collisionInfo);
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(new Rect(0f, 100f, 100f, 100f), "EndLevel"))
		{
			OnCharacterDead(activeCharacter);
		}
		if (GUI.Button(new Rect(0f, 200f, 100f, 100f), "Fatality"))
		{
			DebugFatality();
		}
		if (GUI.Button(new Rect(0f, 400f, 100f, 100f), "UnloadResources"))
		{
			Resources.UnloadUnusedAssets();
		}
		if (GUI.Button(new Rect(0f, 500f, 100f, 100f), "Crash"))
		{
			WeaponGeneral weaponGeneral = null;
			weaponGeneral.Activate(WeaponActivationType.None, null);
		}
	}
}
