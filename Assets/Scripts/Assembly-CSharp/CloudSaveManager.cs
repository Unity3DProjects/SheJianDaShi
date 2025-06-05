using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnSaveLoaded;

	protected static CloudSaveManager instance;

	private CloudSaveImplementation implementation;

	public static CloudSaveManager Instance
	{
		get
		{
			return instance;
		}
	}

	public static event Action OnSaveLoaded
	{
		add
		{
			Action action = CloudSaveManager.m_OnSaveLoaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref CloudSaveManager.m_OnSaveLoaded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = CloudSaveManager.m_OnSaveLoaded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref CloudSaveManager.m_OnSaveLoaded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(instance);
		implementation = new AndroidCloudSave();
	}

	public void LoadFromCloud()
	{
		implementation.LoadFromCloud();
	}

	public void SaveToCloud()
	{
		implementation.SaveToCloud();
	}

	public void PerformLoadFromCloud()
	{
		implementation.PerformLoadFromCloud();
	}

	public void PerformSaveToCloud()
	{
		implementation.PerformSaveToCloud();
	}

	public static void SaveLoadedCallback()
	{
		if (CloudSaveManager.OnSaveLoaded != null)
		{
			CloudSaveManager.OnSaveLoaded();
		}
	}
}
