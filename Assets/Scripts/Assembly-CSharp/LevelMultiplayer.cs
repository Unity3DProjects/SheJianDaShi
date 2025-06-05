using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;

public class LevelMultiplayer : Level
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float, float> m_OnWeaponAiming;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float, float> m_OnWeaponShoot;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float, bool> m_OnHealthChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageWeaponParams> m_OnWeaponTapActivation;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageRagdollRestore> m_OnRagdollRestore;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageSwitchCharacter> m_OnSwitchCharacter;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageRagdollTransform> m_OnRagdollPositionChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageWeaponParams> m_OnWeaponParamsChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageRagdollApply> m_OnRagdollApply;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter> m_OnWeaponCollideCharacter;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerManager.MultiplayerMessageEmissionSettings> m_OnObjectsEmitted;

	[SerializeField]
	private float leftCharacterPositionLeftBorder = -2500f;

	[SerializeField]
	private float leftCharacterPositionRightBorder = -1500f;

	private MultiplayerManager cachedMultiplayerManager;

	private bool isActivationMessageEnabled = true;

	private Character loseCharacter;

	public bool ClientReceivedCollideMessage { get; private set; }

	public FloorSettings MultiplayerFloorSetings
	{
		get
		{
			return CurrentFloorSettings(MatchesForFloorSettings);
		}
	}

	private MultiplayerManager CachedMultiplayerManager
	{
		get
		{
			if (cachedMultiplayerManager == null)
			{
				cachedMultiplayerManager = base.CachedManagers.CachedMultiplayerManager;
			}
			return cachedMultiplayerManager;
		}
	}

	public Character LocalPlayerCharacter
	{
		get
		{
			if (CachedMultiplayerManager.IsMainPlayer)
			{
				return base.CharacterFirst;
			}
			return base.CharacterSecond;
		}
	}

	public Character EnemyCharacter
	{
		get
		{
			return AnotherCharacter(LocalPlayerCharacter);
		}
	}

	public LevelObject[] LastEmittedObjectsPrefabs { get; set; }

	public static event Action<float, float> OnWeaponAiming
	{
		add
		{
			Action<float, float> action = LevelMultiplayer.m_OnWeaponAiming;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponAiming, (Action<float, float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float, float> action = LevelMultiplayer.m_OnWeaponAiming;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponAiming, (Action<float, float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<float, float> OnWeaponShoot
	{
		add
		{
			Action<float, float> action = LevelMultiplayer.m_OnWeaponShoot;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponShoot, (Action<float, float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float, float> action = LevelMultiplayer.m_OnWeaponShoot;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponShoot, (Action<float, float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<float, bool> OnHealthChanged
	{
		add
		{
			Action<float, bool> action = LevelMultiplayer.m_OnHealthChanged;
			Action<float, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnHealthChanged, (Action<float, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float, bool> action = LevelMultiplayer.m_OnHealthChanged;
			Action<float, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnHealthChanged, (Action<float, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageWeaponParams> OnWeaponTapActivation
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action = LevelMultiplayer.m_OnWeaponTapActivation;
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponTapActivation, (Action<MultiplayerManager.MultiplayerMessageWeaponParams>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action = LevelMultiplayer.m_OnWeaponTapActivation;
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponTapActivation, (Action<MultiplayerManager.MultiplayerMessageWeaponParams>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageRagdollRestore> OnRagdollRestore
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageRagdollRestore> action = LevelMultiplayer.m_OnRagdollRestore;
			Action<MultiplayerManager.MultiplayerMessageRagdollRestore> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnRagdollRestore, (Action<MultiplayerManager.MultiplayerMessageRagdollRestore>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageRagdollRestore> action = LevelMultiplayer.m_OnRagdollRestore;
			Action<MultiplayerManager.MultiplayerMessageRagdollRestore> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnRagdollRestore, (Action<MultiplayerManager.MultiplayerMessageRagdollRestore>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageSwitchCharacter> OnSwitchCharacter
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageSwitchCharacter> action = LevelMultiplayer.m_OnSwitchCharacter;
			Action<MultiplayerManager.MultiplayerMessageSwitchCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnSwitchCharacter, (Action<MultiplayerManager.MultiplayerMessageSwitchCharacter>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageSwitchCharacter> action = LevelMultiplayer.m_OnSwitchCharacter;
			Action<MultiplayerManager.MultiplayerMessageSwitchCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnSwitchCharacter, (Action<MultiplayerManager.MultiplayerMessageSwitchCharacter>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageRagdollTransform> OnRagdollPositionChanged
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageRagdollTransform> action = LevelMultiplayer.m_OnRagdollPositionChanged;
			Action<MultiplayerManager.MultiplayerMessageRagdollTransform> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnRagdollPositionChanged, (Action<MultiplayerManager.MultiplayerMessageRagdollTransform>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageRagdollTransform> action = LevelMultiplayer.m_OnRagdollPositionChanged;
			Action<MultiplayerManager.MultiplayerMessageRagdollTransform> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnRagdollPositionChanged, (Action<MultiplayerManager.MultiplayerMessageRagdollTransform>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageWeaponParams> OnWeaponParamsChanged
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action = LevelMultiplayer.m_OnWeaponParamsChanged;
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponParamsChanged, (Action<MultiplayerManager.MultiplayerMessageWeaponParams>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action = LevelMultiplayer.m_OnWeaponParamsChanged;
			Action<MultiplayerManager.MultiplayerMessageWeaponParams> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponParamsChanged, (Action<MultiplayerManager.MultiplayerMessageWeaponParams>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageRagdollApply> OnRagdollApply
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageRagdollApply> action = LevelMultiplayer.m_OnRagdollApply;
			Action<MultiplayerManager.MultiplayerMessageRagdollApply> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnRagdollApply, (Action<MultiplayerManager.MultiplayerMessageRagdollApply>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageRagdollApply> action = LevelMultiplayer.m_OnRagdollApply;
			Action<MultiplayerManager.MultiplayerMessageRagdollApply> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnRagdollApply, (Action<MultiplayerManager.MultiplayerMessageRagdollApply>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter> OnWeaponCollideCharacter
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter> action = LevelMultiplayer.m_OnWeaponCollideCharacter;
			Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponCollideCharacter, (Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter> action = LevelMultiplayer.m_OnWeaponCollideCharacter;
			Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnWeaponCollideCharacter, (Action<MultiplayerManager.MultiplayerMessageWeaponCollideCharacter>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerManager.MultiplayerMessageEmissionSettings> OnObjectsEmitted
	{
		add
		{
			Action<MultiplayerManager.MultiplayerMessageEmissionSettings> action = LevelMultiplayer.m_OnObjectsEmitted;
			Action<MultiplayerManager.MultiplayerMessageEmissionSettings> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnObjectsEmitted, (Action<MultiplayerManager.MultiplayerMessageEmissionSettings>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerManager.MultiplayerMessageEmissionSettings> action = LevelMultiplayer.m_OnObjectsEmitted;
			Action<MultiplayerManager.MultiplayerMessageEmissionSettings> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LevelMultiplayer.m_OnObjectsEmitted, (Action<MultiplayerManager.MultiplayerMessageEmissionSettings>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		MultiplayerManager.OnWeaponAimingReceived += MultiplayerManager_OnWeaponAimingReceived;
		MultiplayerManager.OnWeaponShootReceived += MultiplayerManager_OnWeaponShootReceived;
		MultiplayerManager.OnWeaponTapActivationReceived += MultiplayerManager_OnWeaponTapActivationReceived;
		MultiplayerManager.OnRagdollRestoreReceived += MultiplayerManager_OnRagdollRestoreReceived;
		MultiplayerManager.OnSwitchCharacterReceived += MultiplayerManager_OnSwitchCharacterReceived;
		MultiplayerManager.OnRagdollPositionsReceived += MultiplayerManager_OnRagdollPositionsReceived;
		MultiplayerManager.OnRagdollApplyReceived += MultiplayerManager_OnRagdollApplyReceived;
		MultiplayerManager.OnWeaponCollideCharacterReceived += MultiplayerManager_OnWeaponCollideCharacterReceived;
		MultiplayerManager.OnEmissionSettingsReceived += MultiplayerManager_OnEmissionSettingsReceived;
		MultiplayerManager.OnHealthMessageReceived += MultiplayerManager_OnHealthMessageReceived;
		MultiplayerManager.OnPlayerDisconnected += MultiplayerManager_OnPlayerDisconnected;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		MultiplayerManager.OnWeaponAimingReceived -= MultiplayerManager_OnWeaponAimingReceived;
		MultiplayerManager.OnWeaponShootReceived -= MultiplayerManager_OnWeaponShootReceived;
		MultiplayerManager.OnWeaponTapActivationReceived -= MultiplayerManager_OnWeaponTapActivationReceived;
		MultiplayerManager.OnRagdollRestoreReceived -= MultiplayerManager_OnRagdollRestoreReceived;
		MultiplayerManager.OnSwitchCharacterReceived -= MultiplayerManager_OnSwitchCharacterReceived;
		MultiplayerManager.OnRagdollPositionsReceived -= MultiplayerManager_OnRagdollPositionsReceived;
		MultiplayerManager.OnRagdollApplyReceived -= MultiplayerManager_OnRagdollApplyReceived;
		MultiplayerManager.OnWeaponCollideCharacterReceived -= MultiplayerManager_OnWeaponCollideCharacterReceived;
		MultiplayerManager.OnEmissionSettingsReceived -= MultiplayerManager_OnEmissionSettingsReceived;
		MultiplayerManager.OnHealthMessageReceived -= MultiplayerManager_OnHealthMessageReceived;
		MultiplayerManager.OnPlayerDisconnected -= MultiplayerManager_OnPlayerDisconnected;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		SetMenuSelectedCharacterID();
	}

	public override void Init(ObjectsSpawner objectsSpawner, CameraController cameraController)
	{
		base.Init(objectsSpawner, cameraController);
		EnemyCharacter.IsMultiplayerEnemy = true;
		if (base.IsMultiplayerSecondaryPlayer)
		{
			base.CharacterFirst.SetCollidersEnabled(false, true);
			base.CharacterSecond.SetCollidersEnabled(false, true);
		}
		ClientReceivedCollideMessage = false;
	}

	public override void EnableDragging()
	{
		if (base.ActiveCharacter == LocalPlayerCharacter)
		{
			arrowDragger.IsDraggingEnabled = true;
		}
	}

	public void WeaponAiming(WeaponGeneral weapon)
	{
		if (LevelMultiplayer.OnWeaponAiming != null && base.ActiveCharacter == LocalPlayerCharacter)
		{
			LevelMultiplayer.OnWeaponAiming(weapon.EmissionAngle, weapon.StartVelocity);
		}
	}

	public override void ReleaseWeapon()
	{
		base.ReleaseWeapon();
		base.IsActiveForcedSwitchPlayerTimer = false;
		if (LevelMultiplayer.OnWeaponShoot != null && base.ActiveCharacter == LocalPlayerCharacter)
		{
			LevelMultiplayer.OnWeaponShoot(base.CurrentWeapon.EmissionAngle, base.CurrentWeapon.StartVelocity);
		}
	}

	public override void WeaponWasActivatedByTap(WeaponGeneral weapon)
	{
		if (isActivationMessageEnabled && LevelMultiplayer.OnWeaponTapActivation != null)
		{
			WeaponParams weaponParams = new WeaponParams();
			weaponParams.weaponPosition = weapon.transform.position;
			weaponParams.weaponRotation = weapon.transform.eulerAngles;
			LevelMultiplayer.OnWeaponTapActivation(MultiplayerManager.CreateWeaponActivationMessage(weaponParams));
		}
	}

	public void ApplyCharacterRagdoll(CharacterBase character)
	{
		if (!base.IsMultiplayerSecondaryPlayer)
		{
			bool isLeftCharacter = character == base.CharacterFirst;
			if (LevelMultiplayer.OnRagdollApply != null)
			{
				LevelMultiplayer.OnRagdollApply(MultiplayerManager.CreateRagdollApplyMessage(isLeftCharacter));
			}
		}
	}

	public void WeaponCollideCharacter(CharacterBase character, WeaponParams WeaponParams, int weaponColliderIndex, int characterColliderIndex, bool isDriveIntoLegs)
	{
		if (!base.IsMultiplayerSecondaryPlayer)
		{
			bool isLeftCharacter = character == base.CharacterFirst;
			if (LevelMultiplayer.OnWeaponCollideCharacter != null && base.CurrentWeapon != null)
			{
				WeaponParams weaponParams = new WeaponParams();
				weaponParams.weaponPosition = base.CurrentWeapon.transform.position;
				weaponParams.weaponRotation = base.CurrentWeapon.transform.eulerAngles;
				LevelMultiplayer.OnWeaponCollideCharacter(MultiplayerManager.CreateWeaponCollideCharacterMessage(weaponParams, weaponColliderIndex, characterColliderIndex, isDriveIntoLegs, isLeftCharacter));
			}
		}
	}

	public void ObjectsWasEmitted(List<EmissionSettings> emissionSettings, List<int> trashCanEmissionObjectsNumbers)
	{
		if (LevelMultiplayer.OnObjectsEmitted != null && CachedMultiplayerManager.IsMainPlayer)
		{
			LevelMultiplayer.OnObjectsEmitted(MultiplayerManager.CreateEmissionSettingsMessage(emissionSettings, trashCanEmissionObjectsNumbers));
		}
	}

	public float GenerateLeftCharacterXPosition()
	{
		return UnityEngine.Random.Range(leftCharacterPositionLeftBorder, leftCharacterPositionRightBorder);
	}

	public float GenerateRightCharacterXPosition(float leftCharacterXPosition)
	{
		return UnityEngine.Random.Range(leftCharacterXPosition + minEnemySpotDistance, leftCharacterXPosition + maxEnemySpotDistance);
	}

	public void IncreaseTurnCountTemporally(float duration)
	{
		TurnActionsChangeCount(1);
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			TurnActionsChangeCount(-1);
		}, duration);
	}

	public void HealthChanged(CharacterBase currentCharacter, float health)
	{
		if (CachedMultiplayerManager.IsMainPlayer)
		{
			bool arg = currentCharacter as Character == base.CharacterFirst;
			if (LevelMultiplayer.OnHealthChanged != null)
			{
				LevelMultiplayer.OnHealthChanged(health, arg);
			}
		}
	}

	public void CharacterWasRestored(CharacterBase restoredCharacter, float restoreXPosition)
	{
		if (CachedMultiplayerManager.IsMainPlayer)
		{
			bool isLeftCharacter = restoredCharacter as Character == base.CharacterFirst;
			if (LevelMultiplayer.OnRagdollRestore != null)
			{
				LevelMultiplayer.OnRagdollRestore(MultiplayerManager.CreateRagdollRestoreMessage(restoreXPosition, isLeftCharacter));
			}
		}
	}

	public void RagdollPositionChanged(CharacterBase currentCharacter, List<Vector3> ragdollPositions, List<Vector3> ragdollRotations)
	{
		if (CachedMultiplayerManager.IsMainPlayer && !base.IsMainBattleFinished)
		{
			RagdollParams ragdollParams = new RagdollParams();
			ragdollParams.ragdollPositions = ragdollPositions;
			ragdollParams.ragdollRotations = ragdollRotations;
			ragdollParams.isLeftCharacter = currentCharacter as Character == base.CharacterFirst;
			if (LevelMultiplayer.OnRagdollPositionChanged != null)
			{
				LevelMultiplayer.OnRagdollPositionChanged(MultiplayerManager.CreateRagdollTransformMessage(ragdollParams));
			}
		}
	}

	public override void OnCharacterDead(Character character)
	{
		base.OnCharacterDead(character);
		if (!(gameScreen != null))
		{
			return;
		}
		TurnTimer turnTimer = gameScreen.TurnTimer;
		for (int i = 0; i < turnTimer.TimerAnimations.Length; i++)
		{
			if (((LocalPlayerCharacter == base.CharacterFirst && !base.IsMultiplayerSecondaryPlayer) || (LocalPlayerCharacter == base.CharacterSecond && base.IsMultiplayerSecondaryPlayer)) && turnTimer.TimerAnimations[i].AnimationName == "idle" && LocalPlayerCharacter.Health > 0f)
			{
				turnTimer.TimerAnimations[i].State.SetAnimation(0, turnTimer.CachedAnimationDisappear, false);
			}
		}
	}

	protected override void NextTurn(bool switchArcher = false)
	{
		if (CachedMultiplayerManager.IsMainPlayer || !switchArcher)
		{
			base.NextTurn(switchArcher);
			if (switchArcher && LevelMultiplayer.OnSwitchCharacter != null)
			{
				LevelMultiplayer.OnSwitchCharacter(MultiplayerManager.CreateSwitchCharacterMessage());
			}
		}
	}

	protected override Vector3 LeftArcherPosition()
	{
		float leftCharacterXPosition = CachedMultiplayerManager.MatchSettings.levelSettings.leftCharacterXPosition;
		return location.GetCharacterPositionForRange(leftCharacterXPosition, leftCharacterXPosition);
	}

	protected override Vector3 RightArcherPosition()
	{
		float rightCharacterXPosition = CachedMultiplayerManager.MatchSettings.levelSettings.rightCharacterXPosition;
		return location.GetCharacterPositionForRange(rightCharacterXPosition, rightCharacterXPosition);
	}

	protected override void UpdateLevelWithForcedSwitchPlayer(float deltaTime, float timerDuration)
	{
		turnTimer += deltaTime;
		float value = (timerDuration - turnTimer) / timerDuration;
		if (gameScreen != null)
		{
			gameScreen.SetTurnTimerFill(value);
		}
		if (!base.IsMultiplayerSecondaryPlayer && turnTimer > timerDuration)
		{
			SwitchCharacter();
			if (LevelMultiplayer.OnSwitchCharacter != null)
			{
				LevelMultiplayer.OnSwitchCharacter(MultiplayerManager.CreateSwitchCharacterMessage());
			}
		}
	}

	protected override void EndLevel(Character loseCharacter)
	{
		SetMenuSelectedCharacterID();
		base.EndLevel(loseCharacter);
	}

	private void SwitchCharacter()
	{
		wasWeaponCreated = false;
		arrowDragger.CancelTouch();
		base.ActiveCharacter.TimeOutWithPrepareShooting();
		SwitchActiveArcher();
		UpdateArcherPosition();
		UpdateCameraPosition();
		CreateObjectsForTurn();
		turnTimer = 0f;
		base.IsActiveForcedSwitchPlayerTimer = false;
	}

	private void SetMenuSelectedCharacterID()
	{
		if (base.IsMultiplayerSecondaryPlayer && objectsSpawner != null)
		{
			objectsSpawner.SetFirstSelectedCharacter(objectsSpawner.SecondSelectedCharacterNameType);
		}
	}

	private void MultiplayerManager_OnWeaponAimingReceived(float angle, float velocity)
	{
		if (base.ActiveCharacter != null)
		{
			base.ActiveCharacter.UserAimingActionDetected();
			base.ActiveCharacter.SetCurrentAngleAndVelocity(new ShootingInfo(angle, velocity), 0f);
		}
	}

	private void MultiplayerManager_OnWeaponTapActivationReceived(MultiplayerManager.MultiplayerMessageWeaponParams message)
	{
		if ((bool)currentWeapon)
		{
			isActivationMessageEnabled = false;
			base.CurrentWeapon.ForceWeaponParams(message.weaponParams);
			base.CurrentWeapon.MultiplayerActivate();
			isActivationMessageEnabled = true;
		}
	}

	private void MultiplayerManager_OnSwitchCharacterReceived(MultiplayerManager.MultiplayerMessageSwitchCharacter message)
	{
		SwitchCharacter();
		ClientReceivedCollideMessage = false;
	}

	private void MultiplayerManager_OnWeaponShootReceived(float angle, float velocity)
	{
		base.ActiveCharacter.SetCurrentAngleAndVelocity(new ShootingInfo(angle, velocity), 0f);
		ReleaseWeapon();
	}

	private void MultiplayerManager_OnEmissionSettingsReceived(MultiplayerManager.MultiplayerMessageEmissionSettings message)
	{
		LevelObject[] array = LastEmittedObjectsPrefabs;
		if (array == null && base.CurrentWeapon != null && base.CurrentWeapon.CachedEmission != null)
		{
			array = base.CurrentWeapon.CachedEmission.EmittedObjectsPrefabs;
		}
		if (array != null)
		{
			if (message.trashCanEmissionObjectsNumbers == null)
			{
				Emission.CreateEmittedObjects(this, array, message.emissionSettings);
			}
			else
			{
				EmissionTrashCan emissionTrashCan = new EmissionTrashCan();
				emissionTrashCan.CreateEmittedTrashCanObjects(this, array, message.emissionSettings, message.trashCanEmissionObjectsNumbers);
			}
		}
		LastEmittedObjectsPrefabs = null;
	}

	private void MultiplayerManager_OnHealthMessageReceived(float health, bool isLeftCharacter)
	{
		Character character = ((!isLeftCharacter) ? base.CharacterSecond : base.CharacterFirst);
		character.ForceHealth(health);
	}

	private void MultiplayerManager_OnRagdollRestoreReceived(MultiplayerManager.MultiplayerMessageRagdollRestore message)
	{
		Character character = ((!message.isLeftCharacter) ? base.CharacterSecond : base.CharacterFirst);
		character.ForceRestore(message.restoreXPosition);
	}

	private void MultiplayerManager_OnRagdollPositionsReceived(MultiplayerManager.MultiplayerMessageRagdollTransform message)
	{
		Character character = ((!message.ragdollParams.isLeftCharacter) ? base.CharacterSecond : base.CharacterFirst);
		character.ForceRagdollTransform(message.ragdollParams.ragdollPositions, message.ragdollParams.ragdollRotations);
	}

	private void MultiplayerManager_OnRagdollApplyReceived(MultiplayerManager.MultiplayerMessageRagdollApply message)
	{
		Character character = ((!message.isLeftCharacter) ? base.CharacterSecond : base.CharacterFirst);
		character.ApplyCharacterRagdoll();
		ClientReceivedCollideMessage = true;
	}

	private void MultiplayerManager_OnWeaponCollideCharacterReceived(MultiplayerManager.MultiplayerMessageWeaponCollideCharacter message)
	{
		Character character = ((!message.isLeftCharacter) ? base.CharacterSecond : base.CharacterFirst);
		if (base.CurrentWeapon != null)
		{
			base.CurrentWeapon.ForceWeaponCollide(message.weaponParams, message.weaponColliderIndex, message.characterColliderIndex, message.isDriveIntoLegs, character);
			ClientReceivedCollideMessage = true;
		}
	}

	private void MultiplayerManager_OnPlayerDisconnected(bool isLocalPlayerDisconnected)
	{
		if (!isLevelFinished)
		{
			isLevelFinished = true;
			bool flag = ((!isLocalPlayerDisconnected) ? (LocalPlayerCharacter.Health <= 0f) : (EnemyCharacter.Health > 0f));
			loseCharacter = ((!flag) ? EnemyCharacter : LocalPlayerCharacter);
			base.CachedManagers.CachedGUIPopupManager.HideActivePopup(true);
			PopupType typePopup = ((!isLocalPlayerDisconnected) ? PopupType.NoInternetOpponent : PopupType.NoInternet);
			GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(typePopup, true, null, true);
			gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(NoInternetPopup_OnPopupHide));
		}
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		string customEventName = ((!isLocalPlayerDisconnected) ? "Level_Multiplayer_Enemy_Disconnected" : "Level_Multiplayer_Local_Disconnected");
		Analytics.CustomEvent(customEventName, new Dictionary<string, object>
		{
			{ "MatchesCount", cachedStatisticsManager.MathesCount },
			{
				"GameTimeMinutes",
				cachedStatisticsManager.TotalGameTime.Minutes
			}
		});
	}

	private void NoInternetPopup_OnPopupHide(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(NoInternetPopup_OnPopupHide));
		EndLevel(loseCharacter);
	}
}
