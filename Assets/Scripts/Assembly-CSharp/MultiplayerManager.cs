using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using GooglePlayGames;
using MiniJSON;
using UnityEngine;

public class MultiplayerManager : SingletonMonoBehaviour<MultiplayerManager>
{
	private enum MultiplayerMatchState
	{
		None = 0,
		ReadyToStart = 1,
		Game = 2
	}

	public enum MultiplayerMessageType
	{
		None = 0,
		MainPlayerIdentifier = 1,
		LevelSettings = 2,
		ReadyToStart = 3,
		CreateMatch = 4,
		SwitchCharacter = 5,
		WeaponAiming = 6,
		WeaponShoot = 7,
		WeaponActivation = 8,
		RestoreRagdoll = 9,
		EmissionSettings = 10,
		Health = 11,
		RagdollTransform = 12,
		ApplyRagdoll = 13,
		WeaponCollideCharacter = 14,
		Emoji = 15,
		PlayerAcceptedInvite = 16
	}

	[Serializable]
	public struct LevelSettings
	{
		public LocationType locationType;

		public List<Vector2> floorHeights;

		public float leftCharacterXPosition;

		public float rightCharacterXPosition;
	}

	[Serializable]
	public struct MultiplayerMatchSettings
	{
		public LocalPlayerSettings currentPlayerSettings;

		public LocalPlayerSettings enemySettings;

		public LevelSettings levelSettings;
	}

	[Serializable]
	public struct MultiplayerMessage
	{
		public MultiplayerMessageType messageType;
	}

	[Serializable]
	public struct MultiplayerMessageMainPlayerIdentifier
	{
		public MultiplayerMessageType messageType;

		public float randomNumber;

		public CharacterNameType characterID;

		public string playerCountryCode;

		public string playerNickname;

		public bool isMembershipPlayer;
	}

	[Serializable]
	public struct MultiplayerMessageLevelSettings
	{
		public MultiplayerMessageType messageType;

		public LevelSettings levelSettings;
	}

	[Serializable]
	public struct MultiplayerMessageShootSettings
	{
		public MultiplayerMessageType messageType;

		public float shootAngle;

		public float shootVelocity;
	}

	[Serializable]
	public struct MultiplayerMessageHealth
	{
		public MultiplayerMessageType messageType;

		public float health;

		public bool isLeftCharacter;
	}

	[Serializable]
	public class MultiplayerMessageRagdollRestore
	{
		public MultiplayerMessageType messageType;

		public float restoreXPosition;

		public bool isLeftCharacter;
	}

	[Serializable]
	public class MultiplayerMessageRagdollTransform
	{
		public MultiplayerMessageType messageType;

		public RagdollParams ragdollParams;
	}

	[Serializable]
	public class MultiplayerMessageEmissionSettings
	{
		public MultiplayerMessageType messageType;

		public List<EmissionSettings> emissionSettings;

		public List<int> trashCanEmissionObjectsNumbers;
	}

	[Serializable]
	public class MultiplayerMessageWeaponParams
	{
		public MultiplayerMessageType messageType;

		public WeaponParams weaponParams;
	}

	[Serializable]
	public class MultiplayerMessageRagdollApply
	{
		public MultiplayerMessageType messageType;

		public bool isLeftCharacter;
	}

	[Serializable]
	public class MultiplayerMessageWeaponCollideCharacter
	{
		public MultiplayerMessageType messageType;

		public WeaponParams weaponParams;

		public int weaponColliderIndex;

		public int characterColliderIndex;

		public bool isDriveIntoLegs;

		public bool isLeftCharacter;
	}

	[Serializable]
	public struct MultiplayerMessageEmoji
	{
		public MultiplayerMessageType messageType;

		public EmojiType emojiType;
	}

	[Serializable]
	public class MultiplayerMessageSwitchCharacter
	{
		public MultiplayerMessageType messageType;
	}

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnMatchCreated;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float, float> m_OnWeaponAimingReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float, float> m_OnWeaponShootReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float, bool> m_OnHealthMessageReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<EmojiType> m_OnEmojiReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnPlayerDisconnected;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerMessageWeaponParams> m_OnWeaponTapActivationReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerMessageRagdollRestore> m_OnRagdollRestoreReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerMessageSwitchCharacter> m_OnSwitchCharacterReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerMessageRagdollTransform> m_OnRagdollPositionsReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerMessageRagdollApply> m_OnRagdollApplyReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerMessageWeaponCollideCharacter> m_OnWeaponCollideCharacterReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<MultiplayerMessageEmissionSettings> m_OnEmissionSettingsReceived;

	[SerializeField]
	private LevelMultiplayer levelMultiplayer;

	[SerializeField]
	private float turnTimerDuration;

	[SerializeField]
	private float finishHimTimerDuration;

	private MultiplayerMatchSettings matchSettings;

	private MultiplayerMatchState matchState;

	private float mainPlayerRandomNumber;

	private float opponentRandomNumber;

	private bool invitedCharacterSet;

	private bool inviteMode;

	private bool invitedPlayerChoseCharacter;

	private bool wasFirstIdentifierMessageSended;

	private Managers cachedManagers;

	private bool canReceiveWeaponAimMessage;

	private Managers CachedManagers
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

	public MultiplayerMatchSettings MatchSettings
	{
		get
		{
			return matchSettings;
		}
	}

	public LocalPlayerSettings LeftPlayerSettings
	{
		get
		{
			if (IsMainPlayer)
			{
				return matchSettings.currentPlayerSettings;
			}
			return matchSettings.enemySettings;
		}
	}

	public LocalPlayerSettings RightPlayerSettings
	{
		get
		{
			if (IsMainPlayer)
			{
				return matchSettings.enemySettings;
			}
			return matchSettings.currentPlayerSettings;
		}
	}

	public bool IsMainPlayer
	{
		get
		{
			return matchSettings.currentPlayerSettings.isMainPlayer;
		}
		private set
		{
			matchSettings.currentPlayerSettings.isMainPlayer = value;
			if (value)
			{
				GenerateLevelSettings();
				SendLevelSettingsMessage();
				matchState = MultiplayerMatchState.ReadyToStart;
			}
		}
	}

	public float TurnTimerDuration
	{
		get
		{
			return turnTimerDuration;
		}
	}

	public float FinishHimTimerDuration
	{
		get
		{
			return finishHimTimerDuration;
		}
	}

	public static event Action OnMatchCreated
	{
		add
		{
			Action action = MultiplayerManager.m_OnMatchCreated;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnMatchCreated, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = MultiplayerManager.m_OnMatchCreated;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnMatchCreated, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<float, float> OnWeaponAimingReceived
	{
		add
		{
			Action<float, float> action = MultiplayerManager.m_OnWeaponAimingReceived;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponAimingReceived, (Action<float, float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float, float> action = MultiplayerManager.m_OnWeaponAimingReceived;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponAimingReceived, (Action<float, float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<float, float> OnWeaponShootReceived
	{
		add
		{
			Action<float, float> action = MultiplayerManager.m_OnWeaponShootReceived;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponShootReceived, (Action<float, float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float, float> action = MultiplayerManager.m_OnWeaponShootReceived;
			Action<float, float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponShootReceived, (Action<float, float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<float, bool> OnHealthMessageReceived
	{
		add
		{
			Action<float, bool> action = MultiplayerManager.m_OnHealthMessageReceived;
			Action<float, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnHealthMessageReceived, (Action<float, bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float, bool> action = MultiplayerManager.m_OnHealthMessageReceived;
			Action<float, bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnHealthMessageReceived, (Action<float, bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<EmojiType> OnEmojiReceived
	{
		add
		{
			Action<EmojiType> action = MultiplayerManager.m_OnEmojiReceived;
			Action<EmojiType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnEmojiReceived, (Action<EmojiType>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<EmojiType> action = MultiplayerManager.m_OnEmojiReceived;
			Action<EmojiType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnEmojiReceived, (Action<EmojiType>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnPlayerDisconnected
	{
		add
		{
			Action<bool> action = MultiplayerManager.m_OnPlayerDisconnected;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnPlayerDisconnected, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MultiplayerManager.m_OnPlayerDisconnected;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnPlayerDisconnected, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerMessageWeaponParams> OnWeaponTapActivationReceived
	{
		add
		{
			Action<MultiplayerMessageWeaponParams> action = MultiplayerManager.m_OnWeaponTapActivationReceived;
			Action<MultiplayerMessageWeaponParams> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponTapActivationReceived, (Action<MultiplayerMessageWeaponParams>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerMessageWeaponParams> action = MultiplayerManager.m_OnWeaponTapActivationReceived;
			Action<MultiplayerMessageWeaponParams> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponTapActivationReceived, (Action<MultiplayerMessageWeaponParams>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerMessageRagdollRestore> OnRagdollRestoreReceived
	{
		add
		{
			Action<MultiplayerMessageRagdollRestore> action = MultiplayerManager.m_OnRagdollRestoreReceived;
			Action<MultiplayerMessageRagdollRestore> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnRagdollRestoreReceived, (Action<MultiplayerMessageRagdollRestore>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerMessageRagdollRestore> action = MultiplayerManager.m_OnRagdollRestoreReceived;
			Action<MultiplayerMessageRagdollRestore> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnRagdollRestoreReceived, (Action<MultiplayerMessageRagdollRestore>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerMessageSwitchCharacter> OnSwitchCharacterReceived
	{
		add
		{
			Action<MultiplayerMessageSwitchCharacter> action = MultiplayerManager.m_OnSwitchCharacterReceived;
			Action<MultiplayerMessageSwitchCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnSwitchCharacterReceived, (Action<MultiplayerMessageSwitchCharacter>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerMessageSwitchCharacter> action = MultiplayerManager.m_OnSwitchCharacterReceived;
			Action<MultiplayerMessageSwitchCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnSwitchCharacterReceived, (Action<MultiplayerMessageSwitchCharacter>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerMessageRagdollTransform> OnRagdollPositionsReceived
	{
		add
		{
			Action<MultiplayerMessageRagdollTransform> action = MultiplayerManager.m_OnRagdollPositionsReceived;
			Action<MultiplayerMessageRagdollTransform> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnRagdollPositionsReceived, (Action<MultiplayerMessageRagdollTransform>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerMessageRagdollTransform> action = MultiplayerManager.m_OnRagdollPositionsReceived;
			Action<MultiplayerMessageRagdollTransform> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnRagdollPositionsReceived, (Action<MultiplayerMessageRagdollTransform>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerMessageRagdollApply> OnRagdollApplyReceived
	{
		add
		{
			Action<MultiplayerMessageRagdollApply> action = MultiplayerManager.m_OnRagdollApplyReceived;
			Action<MultiplayerMessageRagdollApply> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnRagdollApplyReceived, (Action<MultiplayerMessageRagdollApply>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerMessageRagdollApply> action = MultiplayerManager.m_OnRagdollApplyReceived;
			Action<MultiplayerMessageRagdollApply> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnRagdollApplyReceived, (Action<MultiplayerMessageRagdollApply>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerMessageWeaponCollideCharacter> OnWeaponCollideCharacterReceived
	{
		add
		{
			Action<MultiplayerMessageWeaponCollideCharacter> action = MultiplayerManager.m_OnWeaponCollideCharacterReceived;
			Action<MultiplayerMessageWeaponCollideCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponCollideCharacterReceived, (Action<MultiplayerMessageWeaponCollideCharacter>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerMessageWeaponCollideCharacter> action = MultiplayerManager.m_OnWeaponCollideCharacterReceived;
			Action<MultiplayerMessageWeaponCollideCharacter> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnWeaponCollideCharacterReceived, (Action<MultiplayerMessageWeaponCollideCharacter>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<MultiplayerMessageEmissionSettings> OnEmissionSettingsReceived
	{
		add
		{
			Action<MultiplayerMessageEmissionSettings> action = MultiplayerManager.m_OnEmissionSettingsReceived;
			Action<MultiplayerMessageEmissionSettings> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnEmissionSettingsReceived, (Action<MultiplayerMessageEmissionSettings>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<MultiplayerMessageEmissionSettings> action = MultiplayerManager.m_OnEmissionSettingsReceived;
			Action<MultiplayerMessageEmissionSettings> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MultiplayerManager.m_OnEmissionSettingsReceived, (Action<MultiplayerMessageEmissionSettings>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static MultiplayerMessageWeaponParams CreateWeaponActivationMessage(WeaponParams weaponParams)
	{
		MultiplayerMessageWeaponParams multiplayerMessageWeaponParams = new MultiplayerMessageWeaponParams();
		multiplayerMessageWeaponParams.messageType = MultiplayerMessageType.WeaponActivation;
		multiplayerMessageWeaponParams.weaponParams = weaponParams;
		return multiplayerMessageWeaponParams;
	}

	public static MultiplayerMessageSwitchCharacter CreateSwitchCharacterMessage()
	{
		MultiplayerMessageSwitchCharacter multiplayerMessageSwitchCharacter = new MultiplayerMessageSwitchCharacter();
		multiplayerMessageSwitchCharacter.messageType = MultiplayerMessageType.SwitchCharacter;
		return multiplayerMessageSwitchCharacter;
	}

	public static MultiplayerMessageWeaponCollideCharacter CreateWeaponCollideCharacterMessage(WeaponParams weaponParams, int weaponColliderIndex, int characterColliderIndex, bool isDriveIntoLegs, bool isLeftCharacter)
	{
		MultiplayerMessageWeaponCollideCharacter multiplayerMessageWeaponCollideCharacter = new MultiplayerMessageWeaponCollideCharacter();
		multiplayerMessageWeaponCollideCharacter.messageType = MultiplayerMessageType.WeaponCollideCharacter;
		multiplayerMessageWeaponCollideCharacter.weaponParams = weaponParams;
		multiplayerMessageWeaponCollideCharacter.weaponColliderIndex = weaponColliderIndex;
		multiplayerMessageWeaponCollideCharacter.characterColliderIndex = characterColliderIndex;
		multiplayerMessageWeaponCollideCharacter.isDriveIntoLegs = isDriveIntoLegs;
		multiplayerMessageWeaponCollideCharacter.isLeftCharacter = isLeftCharacter;
		return multiplayerMessageWeaponCollideCharacter;
	}

	public static MultiplayerMessageRagdollApply CreateRagdollApplyMessage(bool isLeftCharacter)
	{
		MultiplayerMessageRagdollApply multiplayerMessageRagdollApply = new MultiplayerMessageRagdollApply();
		multiplayerMessageRagdollApply.messageType = MultiplayerMessageType.ApplyRagdoll;
		multiplayerMessageRagdollApply.isLeftCharacter = isLeftCharacter;
		return multiplayerMessageRagdollApply;
	}

	public static MultiplayerMessageRagdollRestore CreateRagdollRestoreMessage(float restoreXPosition, bool isLeftCharacter)
	{
		MultiplayerMessageRagdollRestore multiplayerMessageRagdollRestore = new MultiplayerMessageRagdollRestore();
		multiplayerMessageRagdollRestore.messageType = MultiplayerMessageType.RestoreRagdoll;
		multiplayerMessageRagdollRestore.restoreXPosition = restoreXPosition;
		multiplayerMessageRagdollRestore.isLeftCharacter = isLeftCharacter;
		return multiplayerMessageRagdollRestore;
	}

	public static MultiplayerMessageRagdollTransform CreateRagdollTransformMessage(RagdollParams ragdollParams)
	{
		MultiplayerMessageRagdollTransform multiplayerMessageRagdollTransform = new MultiplayerMessageRagdollTransform();
		multiplayerMessageRagdollTransform.messageType = MultiplayerMessageType.RagdollTransform;
		multiplayerMessageRagdollTransform.ragdollParams = ragdollParams;
		return multiplayerMessageRagdollTransform;
	}

	public static MultiplayerMessageEmissionSettings CreateEmissionSettingsMessage(List<EmissionSettings> emissionSettings, List<int> trashCanEmissionObjectsNumbers)
	{
		MultiplayerMessageEmissionSettings multiplayerMessageEmissionSettings = new MultiplayerMessageEmissionSettings();
		multiplayerMessageEmissionSettings.messageType = MultiplayerMessageType.EmissionSettings;
		multiplayerMessageEmissionSettings.emissionSettings = emissionSettings;
		multiplayerMessageEmissionSettings.trashCanEmissionObjectsNumbers = trashCanEmissionObjectsNumbers;
		return multiplayerMessageEmissionSettings;
	}

	protected override void Awake()
	{
		base.Awake();
		LLMultiplayerManager.TryRegisterInviteListener();
	}

	private void OnEnable()
	{
		LLMultiplayerManager.OnLocalPlayerAuthenticated += LLMultiplayerManager_OnLocalPlayerAuthenticated;
		LLMultiplayerManager.OnAllOpponentsFound += LLMultiplayerManager_OnAllOpponentsFound;
		LLMultiplayerManager.OnReceiveMessageFromOpponent += LLMultiplayerManager_OnReceiveMessageFromOpponent;
		LLMultiplayerManager.OnPlayerDisconnected += LLMultiplayerManager_OnPlayerDisconnected;
		LLMultiplayerManager.OnPlayerAcceptedInvitation += LLMultiplayerManager_OnPlayerAcceptedInvitation;
		LLMultiplayerManager.OnPlayerConnected += LLMultiplayerManager_OnPlayerConnected;
		LLMultiplayerManager.OnFindingMatch += LLMultiplayerManager_OnFindingMatch;
		GameCenterManager.OnAuthentication += GameCenterManager_OnAuthentication;
		GameManager.OnGameStop += GameManager_OnGameStop;
		LevelMultiplayer.OnWeaponAiming += LevelMultiplayer_OnWeaponAiming;
		LevelMultiplayer.OnWeaponShoot += LevelMultiplayer_OnWeaponShoot;
		LevelMultiplayer.OnWeaponTapActivation += LevelMultiplayer_OnWeaponTapActivation;
		LevelMultiplayer.OnRagdollRestore += LevelMultiplayer_OnRagdollRestore;
		LevelMultiplayer.OnSwitchCharacter += LevelMultiplayer_OnSwitchCharacter;
		LevelMultiplayer.OnRagdollPositionChanged += LevelMultiplayer_OnRagdollPositionChanged;
		LevelMultiplayer.OnWeaponParamsChanged += LevelMultiplayer_OnWeaponParamsChanged;
		LevelMultiplayer.OnRagdollApply += LevelMultiplayer_OnRagdollApply;
		LevelMultiplayer.OnWeaponCollideCharacter += LevelMultiplayer_OnWeaponCollideCharacter;
		LevelMultiplayer.OnObjectsEmitted += LevelMultiplayer_OnObjectsEmitted;
		LevelMultiplayer.OnHealthChanged += LevelMultiplayer_OnHealthChanged;
		GUIPopupManager.OnOverlayedPopupHide += GUIPopupManager_OnOverlayedPopupHide;
	}

	private void OnDisable()
	{
		LLMultiplayerManager.OnLocalPlayerAuthenticated -= LLMultiplayerManager_OnLocalPlayerAuthenticated;
		LLMultiplayerManager.OnAllOpponentsFound -= LLMultiplayerManager_OnAllOpponentsFound;
		LLMultiplayerManager.OnReceiveMessageFromOpponent -= LLMultiplayerManager_OnReceiveMessageFromOpponent;
		LLMultiplayerManager.OnPlayerDisconnected -= LLMultiplayerManager_OnPlayerDisconnected;
		LLMultiplayerManager.OnPlayerAcceptedInvitation -= LLMultiplayerManager_OnPlayerAcceptedInvitation;
		LLMultiplayerManager.OnPlayerConnected -= LLMultiplayerManager_OnPlayerConnected;
		LLMultiplayerManager.OnFindingMatch -= LLMultiplayerManager_OnFindingMatch;
		GameCenterManager.OnAuthentication -= GameCenterManager_OnAuthentication;
		GameManager.OnGameStop -= GameManager_OnGameStop;
		LevelMultiplayer.OnWeaponAiming -= LevelMultiplayer_OnWeaponAiming;
		LevelMultiplayer.OnWeaponShoot -= LevelMultiplayer_OnWeaponShoot;
		LevelMultiplayer.OnWeaponTapActivation -= LevelMultiplayer_OnWeaponTapActivation;
		LevelMultiplayer.OnRagdollRestore -= LevelMultiplayer_OnRagdollRestore;
		LevelMultiplayer.OnSwitchCharacter -= LevelMultiplayer_OnSwitchCharacter;
		LevelMultiplayer.OnRagdollPositionChanged -= LevelMultiplayer_OnRagdollPositionChanged;
		LevelMultiplayer.OnWeaponParamsChanged -= LevelMultiplayer_OnWeaponParamsChanged;
		LevelMultiplayer.OnRagdollApply -= LevelMultiplayer_OnRagdollApply;
		LevelMultiplayer.OnWeaponCollideCharacter -= LevelMultiplayer_OnWeaponCollideCharacter;
		LevelMultiplayer.OnObjectsEmitted -= LevelMultiplayer_OnObjectsEmitted;
		LevelMultiplayer.OnHealthChanged -= LevelMultiplayer_OnHealthChanged;
		GUIPopupManager.OnOverlayedPopupHide -= GUIPopupManager_OnOverlayedPopupHide;
	}

	public bool CreateMatch(CharacterNameType currentPlayerCharacterID)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
			return false;
		}
		wasFirstIdentifierMessageSended = false;
		matchSettings.currentPlayerSettings.characterID = currentPlayerCharacterID;
		matchSettings.currentPlayerSettings.countryCode = LLMultiplayerManager.LocalPlayerCountryCode;
		matchSettings.currentPlayerSettings.nickname = LLMultiplayerManager.LocalPlayerNickname;
		matchSettings.currentPlayerSettings.isMembershipPlayer = CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		LLMultiplayerManager.AuthenticateLocalPlayer();
		return true;
	}

	public void SetLocalPlayerSettings(CharacterNameType currentPlayerCharacterID)
	{
		matchSettings.currentPlayerSettings.characterID = currentPlayerCharacterID;
		matchSettings.currentPlayerSettings.countryCode = LLMultiplayerManager.LocalPlayerCountryCode;
		matchSettings.currentPlayerSettings.nickname = LLMultiplayerManager.LocalPlayerNickname;
		matchSettings.currentPlayerSettings.isMembershipPlayer = CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
	}

	public void SendEmojiToOpponent(EmojiType type)
	{
		MultiplayerMessageEmoji multiplayerMessageEmoji = default(MultiplayerMessageEmoji);
		multiplayerMessageEmoji.messageType = MultiplayerMessageType.Emoji;
		multiplayerMessageEmoji.emojiType = type;
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(multiplayerMessageEmoji));
	}

	private int CharacterDifficultByID(CharacterNameType characterID)
	{
		int result = 1;
		TournamentManager cachedTournamentManager = CachedManagers.CachedTournamentManager;
		Predicate<CharacterNameType> match = (CharacterNameType item) => item == characterID;
		if (cachedTournamentManager.MediumCharacters.Find(match) != CharacterNameType.None)
		{
			result = 2;
		}
		else if (cachedTournamentManager.HardCharacters.Find(match) != CharacterNameType.None)
		{
			result = 3;
		}
		else if (cachedTournamentManager.BossCharacters.Find(match) != CharacterNameType.None)
		{
			result = 4;
		}
		return result;
	}

	private void GenerateLevelSettings()
	{
		matchSettings.levelSettings.locationType = CachedManagers.CachedLocationsManager.CurrentLocation;
		matchSettings.levelSettings.floorHeights = CachedManagers.CachedLocationsManager.GetLocationPrefab().GenerateFloorHeights(levelMultiplayer.MultiplayerFloorSetings);
		float leftCharacterXPosition = levelMultiplayer.GenerateLeftCharacterXPosition();
		matchSettings.levelSettings.leftCharacterXPosition = leftCharacterXPosition;
		matchSettings.levelSettings.rightCharacterXPosition = levelMultiplayer.GenerateRightCharacterXPosition(leftCharacterXPosition);
	}

	private void SendMainPlayerIdentifierMessage()
	{
		mainPlayerRandomNumber = UnityEngine.Random.value;
		MultiplayerMessageMainPlayerIdentifier multiplayerMessageMainPlayerIdentifier = default(MultiplayerMessageMainPlayerIdentifier);
		multiplayerMessageMainPlayerIdentifier.messageType = MultiplayerMessageType.MainPlayerIdentifier;
		multiplayerMessageMainPlayerIdentifier.randomNumber = mainPlayerRandomNumber;
		multiplayerMessageMainPlayerIdentifier.characterID = matchSettings.currentPlayerSettings.characterID;
		multiplayerMessageMainPlayerIdentifier.playerCountryCode = matchSettings.currentPlayerSettings.countryCode;
		multiplayerMessageMainPlayerIdentifier.playerNickname = matchSettings.currentPlayerSettings.nickname;
		multiplayerMessageMainPlayerIdentifier.isMembershipPlayer = matchSettings.currentPlayerSettings.isMembershipPlayer;
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(multiplayerMessageMainPlayerIdentifier));
	}

	private void SendLevelSettingsMessage()
	{
		MultiplayerMessageLevelSettings multiplayerMessageLevelSettings = default(MultiplayerMessageLevelSettings);
		multiplayerMessageLevelSettings.messageType = MultiplayerMessageType.LevelSettings;
		multiplayerMessageLevelSettings.levelSettings = matchSettings.levelSettings;
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(multiplayerMessageLevelSettings));
	}

	private void SendWeaponShootMessage(float angle, float velocity, MultiplayerMessageType messageType = MultiplayerMessageType.WeaponAiming)
	{
		MultiplayerMessageShootSettings multiplayerMessageShootSettings = default(MultiplayerMessageShootSettings);
		multiplayerMessageShootSettings.messageType = messageType;
		multiplayerMessageShootSettings.shootAngle = angle;
		multiplayerMessageShootSettings.shootVelocity = velocity;
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(multiplayerMessageShootSettings));
	}

	private void SendMessageWithoutParams(MultiplayerMessageType messageType)
	{
		MultiplayerMessage multiplayerMessage = default(MultiplayerMessage);
		multiplayerMessage.messageType = messageType;
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(multiplayerMessage));
	}

	private void CheckMainCharacter()
	{
		int num = CharacterDifficultByID(matchSettings.currentPlayerSettings.characterID);
		int num2 = CharacterDifficultByID(matchSettings.enemySettings.characterID);
		if (num != num2)
		{
			IsMainPlayer = num < num2;
			matchSettings.enemySettings.isMainPlayer = !IsMainPlayer;
		}
		else if (mainPlayerRandomNumber != opponentRandomNumber)
		{
			IsMainPlayer = mainPlayerRandomNumber > opponentRandomNumber;
			matchSettings.enemySettings.isMainPlayer = !IsMainPlayer;
		}
		else
		{
			SendMainPlayerIdentifierMessage();
		}
	}

	private void LLMultiplayerManager_OnReceiveMessageFromOpponent(string receivedMessage)
	{
		switch (Json.Deserialize<MultiplayerMessage>(receivedMessage).messageType)
		{
		case MultiplayerMessageType.MainPlayerIdentifier:
		{
			MultiplayerMessageMainPlayerIdentifier multiplayerMessageMainPlayerIdentifier = Json.Deserialize<MultiplayerMessageMainPlayerIdentifier>(receivedMessage);
			matchSettings.enemySettings.characterID = multiplayerMessageMainPlayerIdentifier.characterID;
			matchSettings.enemySettings.countryCode = multiplayerMessageMainPlayerIdentifier.playerCountryCode;
			matchSettings.enemySettings.nickname = multiplayerMessageMainPlayerIdentifier.playerNickname;
			matchSettings.enemySettings.isMembershipPlayer = multiplayerMessageMainPlayerIdentifier.isMembershipPlayer;
			opponentRandomNumber = multiplayerMessageMainPlayerIdentifier.randomNumber;
			if (!inviteMode || invitedCharacterSet)
			{
				if (CachedManagers.CachedGUIPopupManager.ActivePopup != null && CachedManagers.CachedGUIPopupManager.ActivePopup.TypePopup == PopupType.WaitForOpponentChoosing)
				{
					invitedPlayerChoseCharacter = true;
					CachedManagers.CachedGUIPopupManager.ActivePopup.Hide();
				}
				if (!wasFirstIdentifierMessageSended)
				{
					wasFirstIdentifierMessageSended = true;
					SendMainPlayerIdentifierMessage();
				}
				CheckMainCharacter();
			}
			break;
		}
		case MultiplayerMessageType.LevelSettings:
		{
			MultiplayerMessageLevelSettings multiplayerMessageLevelSettings = Json.Deserialize<MultiplayerMessageLevelSettings>(receivedMessage);
			matchSettings.levelSettings = multiplayerMessageLevelSettings.levelSettings;
			matchState = MultiplayerMatchState.ReadyToStart;
			SendMessageWithoutParams(MultiplayerMessageType.ReadyToStart);
			break;
		}
		case MultiplayerMessageType.ReadyToStart:
			if (IsMainPlayer && matchState == MultiplayerMatchState.ReadyToStart)
			{
				SendMessageWithoutParams(MultiplayerMessageType.CreateMatch);
				matchState = MultiplayerMatchState.Game;
				MultiplayerManager.m_OnMatchCreated?.Invoke();
			}
			break;
		case MultiplayerMessageType.CreateMatch:
			if (!IsMainPlayer && matchState == MultiplayerMatchState.ReadyToStart)
			{
				matchState = MultiplayerMatchState.Game;
				MultiplayerManager.m_OnMatchCreated?.Invoke();
			}
			break;
		case MultiplayerMessageType.WeaponAiming:
			if (matchState == MultiplayerMatchState.Game && canReceiveWeaponAimMessage)
			{
				MultiplayerMessageShootSettings multiplayerMessageShootSettings2 = Json.Deserialize<MultiplayerMessageShootSettings>(receivedMessage);
				MultiplayerManager.m_OnWeaponAimingReceived?.Invoke(multiplayerMessageShootSettings2.shootAngle, multiplayerMessageShootSettings2.shootVelocity);
			}
			break;
		case MultiplayerMessageType.WeaponShoot:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageShootSettings multiplayerMessageShootSettings = Json.Deserialize<MultiplayerMessageShootSettings>(receivedMessage);
				canReceiveWeaponAimMessage = false;
				MultiplayerManager.m_OnWeaponShootReceived?.Invoke(multiplayerMessageShootSettings.shootAngle, multiplayerMessageShootSettings.shootVelocity);
			}
			break;
		case MultiplayerMessageType.WeaponActivation:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageWeaponParams obj = Json.Deserialize<MultiplayerMessageWeaponParams>(receivedMessage);
				MultiplayerManager.m_OnWeaponTapActivationReceived?.Invoke(obj);
			}
			break;
		case MultiplayerMessageType.RestoreRagdoll:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageRagdollRestore obj7 = Json.Deserialize<MultiplayerMessageRagdollRestore>(receivedMessage);
				MultiplayerManager.m_OnRagdollRestoreReceived?.Invoke(obj7);
			}
			break;
		case MultiplayerMessageType.SwitchCharacter:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageSwitchCharacter obj4 = Json.Deserialize<MultiplayerMessageSwitchCharacter>(receivedMessage);
				MultiplayerManager.m_OnSwitchCharacterReceived?.Invoke(obj4);
			}
			break;
		case MultiplayerMessageType.EmissionSettings:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageEmissionSettings obj2 = Json.Deserialize<MultiplayerMessageEmissionSettings>(receivedMessage);
				MultiplayerManager.m_OnEmissionSettingsReceived?.Invoke(obj2);
			}
			break;
		case MultiplayerMessageType.Health:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageHealth multiplayerMessageHealth = Json.Deserialize<MultiplayerMessageHealth>(receivedMessage);
				MultiplayerManager.m_OnHealthMessageReceived?.Invoke(multiplayerMessageHealth.health, multiplayerMessageHealth.isLeftCharacter);
			}
			break;
		case MultiplayerMessageType.RagdollTransform:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageRagdollTransform obj6 = Json.Deserialize<MultiplayerMessageRagdollTransform>(receivedMessage);
				MultiplayerManager.m_OnRagdollPositionsReceived?.Invoke(obj6);
			}
			break;
		case MultiplayerMessageType.ApplyRagdoll:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageRagdollApply obj5 = Json.Deserialize<MultiplayerMessageRagdollApply>(receivedMessage);
				MultiplayerManager.m_OnRagdollApplyReceived?.Invoke(obj5);
			}
			break;
		case MultiplayerMessageType.WeaponCollideCharacter:
			canReceiveWeaponAimMessage = true;
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageWeaponCollideCharacter obj3 = Json.Deserialize<MultiplayerMessageWeaponCollideCharacter>(receivedMessage);
				MultiplayerManager.m_OnWeaponCollideCharacterReceived?.Invoke(obj3);
			}
			break;
		case MultiplayerMessageType.Emoji:
			if (matchState == MultiplayerMatchState.Game)
			{
				MultiplayerMessageEmoji multiplayerMessageEmoji = Json.Deserialize<MultiplayerMessageEmoji>(receivedMessage);
				MultiplayerManager.m_OnEmojiReceived?.Invoke(multiplayerMessageEmoji.emojiType);
			}
			break;
		case MultiplayerMessageType.PlayerAcceptedInvite:
			if (matchState == MultiplayerMatchState.None)
			{
				invitedPlayerChoseCharacter = false;
				CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.WaitForOpponentChoosing);
			}
			break;
		}
	}

	private void LLMultiplayerManager_OnAllOpponentsFound()
	{
		inviteMode = false;
		canReceiveWeaponAimMessage = true;
		if (!wasFirstIdentifierMessageSended)
		{
			wasFirstIdentifierMessageSended = true;
			SendMainPlayerIdentifierMessage();
		}
	}

	private void LLMultiplayerManager_OnPlayerAcceptedInvitation()
	{
		if (CachedManagers.CachedGUIScreenManager.ActiveScreen == GUIScreenType.Main)
		{
			inviteMode = true;
			invitedCharacterSet = false;
			canReceiveWeaponAimMessage = true;
			CachedManagers.CachedGUIScreenManager.MenuScreen.OnInvitationReceived(delegate
			{
				SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.MatchmakingWaiting);
				PlayGamesPlatform.Instance.RealTime.LeaveRoom();
				wasFirstIdentifierMessageSended = false;
				PlayGamesPlatform.Instance.RealTime.AcceptInvitation(SingletonMonoBehaviour<GameCenterManager>.Instance.CurrentInvitation.InvitationId, AndroidRealTimeMultiplayerListener.Instance);
				invitedCharacterSet = true;
				SendMainPlayerIdentifierMessage();
				CheckMainCharacter();
			});
		}
	}

	private void LLMultiplayerManager_OnLocalPlayerAuthenticated()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.NoInternet, true, null, true);
			return;
		}
		wasFirstIdentifierMessageSended = false;
		LLMultiplayerManager.FindMatch();
	}

	private void LLMultiplayerManager_OnPlayerConnected()
	{
		if (inviteMode)
		{
			SendMessageWithoutParams(MultiplayerMessageType.PlayerAcceptedInvite);
		}
	}

	private void LLMultiplayerManager_OnFindingMatch()
	{
		LLMultiplayerManager.canCancelMatchmaking = true;
		Invoke("CancelMatchmaking", 30f);
	}

	public void CancelMatchmaking()
	{
		if (LLMultiplayerManager.canCancelMatchmaking)
		{
			PlayGamesPlatform.Instance.RealTime.LeaveRoom();
			SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.FailedToFindOpponent);
		}
	}

	private void LLMultiplayerManager_OnPlayerDisconnected(bool isLocalPlayer)
	{
		matchState = MultiplayerMatchState.None;
		float delay = ((!(SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel == null)) ? 0f : 5f);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			MultiplayerManager.m_OnPlayerDisconnected?.Invoke(isLocalPlayer);
		}, delay);
	}

	private void LevelMultiplayer_OnWeaponAiming(float angle, float velocity)
	{
		SendWeaponShootMessage(angle, velocity);
	}

	private void LevelMultiplayer_OnWeaponShoot(float angle, float velocity)
	{
		SendWeaponShootMessage(angle, velocity, MultiplayerMessageType.WeaponShoot);
	}

	private void LevelMultiplayer_OnWeaponTapActivation(MultiplayerMessageWeaponParams message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void LevelMultiplayer_OnRagdollRestore(MultiplayerMessageRagdollRestore message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void LevelMultiplayer_OnSwitchCharacter(MultiplayerMessageSwitchCharacter message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void LevelMultiplayer_OnObjectsEmitted(MultiplayerMessageEmissionSettings message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void LevelMultiplayer_OnHealthChanged(float health, bool isLeftCharacter)
	{
		MultiplayerMessageHealth multiplayerMessageHealth = default(MultiplayerMessageHealth);
		multiplayerMessageHealth.messageType = MultiplayerMessageType.Health;
		multiplayerMessageHealth.health = health;
		multiplayerMessageHealth.isLeftCharacter = isLeftCharacter;
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(multiplayerMessageHealth));
	}

	private void LevelMultiplayer_OnRagdollPositionChanged(MultiplayerMessageRagdollTransform message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void LevelMultiplayer_OnWeaponParamsChanged(MultiplayerMessageWeaponParams message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void LevelMultiplayer_OnRagdollApply(MultiplayerMessageRagdollApply message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void LevelMultiplayer_OnWeaponCollideCharacter(MultiplayerMessageWeaponCollideCharacter message)
	{
		LLMultiplayerManager.SendMessageToOpponent(Json.Serialize(message));
	}

	private void GameCenterManager_OnAuthentication(bool result)
	{
		if (!result)
		{
			LLMultiplayerManager.ForceGameCenterAlert();
		}
	}

	private void GameManager_OnGameStop()
	{
		if (CachedManagers.CachedGameManager.CurrentGameType == GameType.Multiplayer)
		{
			matchState = MultiplayerMatchState.None;
			LLMultiplayerManager.StopMatch();
		}
	}

	private void GUIPopupManager_OnOverlayedPopupHide()
	{
		if (CachedManagers.CachedGUIPopupManager.ActivePopup.TypePopup == PopupType.WaitForOpponentChoosing)
		{
			if (!invitedPlayerChoseCharacter)
			{
				LLMultiplayerManager.StopMatch();
			}
			invitedPlayerChoseCharacter = false;
		}
	}
}
