using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using AOT;
using UnityEngine;

public static class LLSoundManager
{
	public delegate void LLCallbackDelegate(string str);

	public static string SOUND_MANAGER = "com.inventain.android.SoundsManager";

	public static string SOUND_MANAGER_INIT_OPTIONS = "InitOptions";

	public static string SOUND_MANAGER_PLAY_SOUND = "LLSoundsManagerPlaySound";

	public static string SOUND_MANAGER_UPDATE_SOUND = "LLSoundsManagerUpdateSound";

	public static string SOUND_MANAGER_STOP_SOUND = "LLSoundsManagerStopSound";

	public static string SOUND_MANAGER_PAUSE_SOUND = "LLSoundsManagerPauseSound";

	public static string SOUND_MANAGER_LISTENER_POSITION = "LLSoundsManagerSetListenerPosition";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnStoppedSound;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_OnErrorSound;

	private static readonly int SOURCE_AMOUNT = 28;

	private static bool initialized;

	public static event Action<string> OnStoppedSound
	{
		add
		{
			Action<string> action = LLSoundManager.m_OnStoppedSound;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSoundManager.m_OnStoppedSound, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = LLSoundManager.m_OnStoppedSound;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSoundManager.m_OnStoppedSound, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> OnErrorSound
	{
		add
		{
			Action<string> action = LLSoundManager.m_OnErrorSound;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSoundManager.m_OnErrorSound, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = LLSoundManager.m_OnErrorSound;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref LLSoundManager.m_OnErrorSound, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private static void LLSoundsManagerInit(int sources, LLCallbackDelegate callbackStop, LLCallbackDelegate callbackError)
	{
	}

	private static void LLSoundsManagerPlaySound(string uid, string filePath, float volume, float pitch, bool looped, float x, float y, float z, float minDistance, float maxDistance, float rolloff)
	{
		AndroidPlugin.Instance.CallStatic(SOUND_MANAGER, SOUND_MANAGER_PLAY_SOUND, uid, filePath, volume, pitch, looped, x, y, z, minDistance, maxDistance, rolloff);
	}

	private static void LLSoundsManagerUpdateSound(string uid, float volume, float pitch, bool looped, float x, float y, float z, float minDistance, float maxDistance, float rolloff)
	{
		AndroidPlugin.Instance.CallStatic(SOUND_MANAGER, SOUND_MANAGER_UPDATE_SOUND, uid, volume, pitch, looped, x, y, z, minDistance, maxDistance, rolloff);
	}

	private static void LLSoundsManagerStopSound(string uid)
	{
		AndroidPlugin.Instance.CallStatic(SOUND_MANAGER, SOUND_MANAGER_STOP_SOUND, uid);
	}

	private static void LLSoundsManagerPauseSound(string uid, bool pause)
	{
		AndroidPlugin.Instance.CallStatic(SOUND_MANAGER, SOUND_MANAGER_PAUSE_SOUND, uid, pause);
	}

	private static void LLSoundsManagerSetListenerPosition(float x, float y, float z)
	{
		AndroidPlugin.Instance.CallStatic(SOUND_MANAGER, SOUND_MANAGER_LISTENER_POSITION, x, y, z);
	}

	public static void Initialize()
	{
		if (!initialized)
		{
			LLSoundsManagerInit(SOURCE_AMOUNT, OnStoppedSoundCallback, OnErrorSoundCallback);
			initialized = true;
		}
	}

	public static void PlaySound(string fileName)
	{
		PlaySound(fileName, fileName, 1f, 1f, false, Vector3.zero, 1f, 10000f, 1f);
	}

	public static void PlaySound(string uid, string filePath, float volume, float pitch, bool looped, Vector3 position, float minDistance, float maxDistance, float rolloff)
	{
		Initialize();
		LLSoundsManagerPlaySound(uid, filePath, volume, pitch, looped, position.x, position.y, position.z, minDistance, maxDistance, rolloff);
	}

	public static void UpdateSound(string uid, float volume, float pitch, bool looped, Vector3 position, float minDistance, float maxDistance, float rolloff)
	{
		Initialize();
		LLSoundsManagerUpdateSound(uid, volume, pitch, looped, position.x, position.y, position.z, minDistance, maxDistance, rolloff);
	}

	public static void StopSound(string uid)
	{
		Initialize();
		LLSoundsManagerStopSound(uid);
	}

	public static void PauseSound(string uid, bool pause)
	{
		Initialize();
		LLSoundsManagerPauseSound(uid, pause);
	}

	public static void SetListenerPosition(Vector3 position)
	{
		Initialize();
		LLSoundsManagerSetListenerPosition(position.x, position.y, position.z);
	}

	[MonoPInvokeCallback(typeof(LLCallbackDelegate))]
	private static void OnStoppedSoundCallback(string uid)
	{
		if (LLSoundManager.OnStoppedSound != null)
		{
			LLSoundManager.OnStoppedSound(uid);
		}
	}

	[MonoPInvokeCallback(typeof(LLCallbackDelegate))]
	private static void OnErrorSoundCallback(string eror)
	{
		CustomDebug.Log(eror);
		if (LLSoundManager.OnErrorSound != null)
		{
			LLSoundManager.OnErrorSound(eror);
		}
	}
}
