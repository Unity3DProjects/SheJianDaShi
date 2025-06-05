using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using GooglePlayGames;
using MiniJSON;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LLMultiplayerManager : MonoBehaviour
{
	public static string latestGameVersion;

	private static LLMultiplayerManager instance;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnLocalPlayerAuthenticated;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnAllOpponentsFound;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnPlayerAcceptedInvitation;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnReceiveMessageFromOpponent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnPlayerConnected;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnPlayerDisconnected;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnMatchmakerCancelled;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnFindingMatch;

	public static bool canCancelMatchmaking;

	private const int GAME_VARIANT = 1;

	public static LLMultiplayerManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new LLMultiplayerManager();
			}
			return instance;
		}
	}

	public static bool IsLocalPlayerAuthenticated
	{
		get
		{
			bool flag = false;
			return Social.localUser.authenticated;
		}
	}

	public static bool IsCurrentGameVersionNotTheLatest
	{
		get
		{
			return false;
		}
	}

	public static string LocalPlayerCountryCode
	{
		get
		{
			string text = "BY";
			return Marshal.PtrToStringAnsi(getCountryCode());
		}
	}

	public static string LocalPlayerNickname
	{
		get
		{
			string text = "Steeeve";
			return Social.localUser.userName;
		}
	}

	public static event Action OnLocalPlayerAuthenticated
	{
		add
		{
			Action action = LLMultiplayerManager.m_OnLocalPlayerAuthenticated;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnLocalPlayerAuthenticated, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLMultiplayerManager.m_OnLocalPlayerAuthenticated;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnLocalPlayerAuthenticated, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnAllOpponentsFound
	{
		add
		{
			Action action = LLMultiplayerManager.m_OnAllOpponentsFound;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnAllOpponentsFound, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLMultiplayerManager.m_OnAllOpponentsFound;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnAllOpponentsFound, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnPlayerAcceptedInvitation
	{
		add
		{
			Action action = LLMultiplayerManager.m_OnPlayerAcceptedInvitation;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnPlayerAcceptedInvitation, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLMultiplayerManager.m_OnPlayerAcceptedInvitation;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnPlayerAcceptedInvitation, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnReceiveMessageFromOpponent
	{
		add
		{
			Action<string> action = LLMultiplayerManager.m_OnReceiveMessageFromOpponent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnReceiveMessageFromOpponent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = LLMultiplayerManager.m_OnReceiveMessageFromOpponent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnReceiveMessageFromOpponent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnPlayerConnected
	{
		add
		{
			Action action = LLMultiplayerManager.m_OnPlayerConnected;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnPlayerConnected, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLMultiplayerManager.m_OnPlayerConnected;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnPlayerConnected, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnPlayerDisconnected
	{
		add
		{
			Action<bool> action = LLMultiplayerManager.m_OnPlayerDisconnected;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnPlayerDisconnected, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = LLMultiplayerManager.m_OnPlayerDisconnected;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnPlayerDisconnected, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnMatchmakerCancelled
	{
		add
		{
			Action action = LLMultiplayerManager.m_OnMatchmakerCancelled;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnMatchmakerCancelled, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLMultiplayerManager.m_OnMatchmakerCancelled;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnMatchmakerCancelled, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnFindingMatch
	{
		add
		{
			Action action = LLMultiplayerManager.m_OnFindingMatch;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnFindingMatch, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = LLMultiplayerManager.m_OnFindingMatch;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLMultiplayerManager.m_OnFindingMatch, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private LLMultiplayerManager()
	{
	}

	[DllImport("mchelper")]
	public static extern IntPtr getCountryCode();

	public static void TryRegisterInviteListener()
	{
	}

	public static void AuthenticateLocalPlayer()
	{
		Social.localUser.Authenticate(delegate(bool success)
		{
			if (!success)
			{
				Debug.Log("AuthenticateLocalPlayer: Failed to authenticate!");
			}
			else
			{
				Instance.LocalPlayerAuthenticated(string.Empty);
			}
		});
	}

	public static void ForceGameCenterAlert()
	{
	}

	public static void SendMessageToOpponent(string message)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(message);
		bool reliable = true;
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliable, bytes);
	}

	public static void FindMatch()
	{
		LLMultiplayerManager.m_OnFindingMatch?.Invoke();
		SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.MatchmakingWaiting);
		PlayGamesPlatform.Instance.RealTime.LeaveRoom();
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(1u, 1u, 1u, AndroidRealTimeMultiplayerListener.Instance);
	}

	public static void StopMatch()
	{
		PlayGamesPlatform.Instance.RealTime.LeaveRoom();
	}

	private void LocalPlayerAuthenticated(string resultJSON)
	{
		LLMultiplayerManager.m_OnLocalPlayerAuthenticated?.Invoke();
	}

	public void AllOpponentsFound(string resultJSON)
	{
		LLMultiplayerManager.m_OnAllOpponentsFound?.Invoke();
	}

	public void PlayerAcceptedInvitation(string resultJSON)
	{
		LLMultiplayerManager.m_OnPlayerAcceptedInvitation?.Invoke();
	}

	public void ReceiveMessageFromOpponent(string resultJSON)
	{
		LLMultiplayerManager.m_OnReceiveMessageFromOpponent?.Invoke(resultJSON);
	}

	private void PlayerConnected(string resultJSON)
	{
		LLMultiplayerManager.m_OnPlayerConnected?.Invoke();
	}

	public void PlayerDisconnected(string resultJSON)
	{
		bool obj = Json.Deserialize<int>(resultJSON) != 0;
		LLMultiplayerManager.m_OnPlayerDisconnected?.Invoke(obj);
	}

	public void MatchmakerCancelled(string resultJSON)
	{
		LLMultiplayerManager.m_OnMatchmakerCancelled?.Invoke();
	}
}
