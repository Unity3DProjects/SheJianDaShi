using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnMusicChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnSoundChanged;

	public const string MUSIC_CATEGORY = "Music";

	public const string UI_CATEGORY = "UI";

	public const string INGAME_CATEGORY = "InGame";

	public const string ENEMIES_CATEGORY = "Enemies";

	private const string MUSIC_KEY = "Music";

	private const string SOUNDS_KEY = "Sounds";

	private const float MUTED_VOLUME_VALUE = 0f;

	private bool isMusicEnabled;

	private bool isSoundsEnabled;

	private bool isMuteSounds;

	private bool isMuteInGameSounds;

	private bool isMuteUISounds;

	private bool isEnableManager;

	private Dictionary<string, float> soundCategoriesVolumes = new Dictionary<string, float>();

	private Queue<Action> actionQueue = new Queue<Action>();

	public bool IsMusicEnabled
	{
		get
		{
			return isMusicEnabled;
		}
		private set
		{
			isMusicEnabled = value;
			if (value)
			{
				SoundsManager.SetCategoryVolume("Music", soundCategoriesVolumes["Music"]);
				CustomPlayerPrefs.SetInt("Music", 1);
			}
			else
			{
				SoundsManager.SetCategoryVolume("Music", 0f);
				CustomPlayerPrefs.SetInt("Music", 0);
			}
			if (AudioManager.OnMusicChanged != null)
			{
				AudioManager.OnMusicChanged(value);
			}
		}
	}

	public bool IsSoundsEnabled
	{
		get
		{
			return isSoundsEnabled;
		}
		private set
		{
			isSoundsEnabled = value;
			if (value)
			{
				EnableSoundsVolume();
				CustomPlayerPrefs.SetInt("Sounds", 1);
			}
			else
			{
				DisableSoundsVolume();
				CustomPlayerPrefs.SetInt("Sounds", 0);
			}
			if (AudioManager.OnSoundChanged != null)
			{
				AudioManager.OnSoundChanged(value);
			}
		}
	}

	public bool IsMuteSounds
	{
		get
		{
			return isMuteSounds;
		}
		private set
		{
			isMuteSounds = value;
			if (isMuteSounds)
			{
				if (IsSoundsEnabled)
				{
					MuteCategoriesExceptSelected(new List<string> { "Music" });
				}
				if (IsMusicEnabled)
				{
					SoundsManager.SetCategoryVolume("Music", 0f);
				}
				return;
			}
			if (IsSoundsEnabled)
			{
				List<string> list = new List<string>();
				list.Add("Music");
				List<string> list2 = list;
				if (isMuteInGameSounds)
				{
					list2.Add("InGame");
				}
				if (isMuteUISounds)
				{
					list2.Add("UI");
				}
				UnMuteCategoriesExceptSelected(list2);
			}
			if (IsMusicEnabled)
			{
				SoundsManager.SetCategoryVolume("Music", soundCategoriesVolumes["Music"]);
			}
		}
	}

	public bool IsMuteInGameSounds
	{
		get
		{
			return isMuteInGameSounds;
		}
		private set
		{
			isMuteInGameSounds = value;
			if (isMuteInGameSounds)
			{
				if (IsSoundsEnabled)
				{
					MuteCategoriesExceptSelected(new List<string> { "Music", "UI" });
				}
				if (IsMusicEnabled)
				{
					SoundsManager.SetCategoryVolume("Music", 0f);
				}
			}
			else
			{
				if (IsSoundsEnabled)
				{
					UnMuteCategoriesExceptSelected(new List<string> { "Music", "UI" });
				}
				if (IsMusicEnabled)
				{
					SoundsManager.SetCategoryVolume("Music", soundCategoriesVolumes["Music"]);
				}
			}
		}
	}

	public bool IsMuteUISounds
	{
		get
		{
			return isMuteUISounds;
		}
		private set
		{
			isMuteUISounds = value;
			if (isMuteUISounds)
			{
				if (IsSoundsEnabled)
				{
					MuteCategoriesExceptSelected(new List<string> { "Music", "InGame" });
				}
				if (IsMusicEnabled)
				{
					SoundsManager.SetCategoryVolume("Music", 0f);
				}
			}
			else
			{
				if (IsSoundsEnabled)
				{
					UnMuteCategoriesExceptSelected(new List<string> { "Music", "InGame" });
				}
				if (IsMusicEnabled)
				{
					SoundsManager.SetCategoryVolume("Music", soundCategoriesVolumes["Music"]);
				}
			}
		}
	}

	public float MusicVolume
	{
		get
		{
			return soundCategoriesVolumes["Music"];
		}
	}

	public static event Action<bool> OnMusicChanged
	{
		add
		{
			Action<bool> action = AudioManager.m_OnMusicChanged;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref AudioManager.m_OnMusicChanged, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = AudioManager.m_OnMusicChanged;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref AudioManager.m_OnMusicChanged, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnSoundChanged
	{
		add
		{
			Action<bool> action = AudioManager.m_OnSoundChanged;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref AudioManager.m_OnSoundChanged, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = AudioManager.m_OnSoundChanged;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref AudioManager.m_OnSoundChanged, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		InitializeSoundCategoriesVolumes();
		DisableSoundsVolume();
		IsMusicEnabled = CustomPlayerPrefs.GetInt("Music", 1) == 1;
		IsSoundsEnabled = CustomPlayerPrefs.GetInt("Sounds", 1) == 1;
	}

	private void OnEnable()
	{
		NativeEventsWrapper.OnApplicationDidReceiveMemoryWarning += NativeEventsWrapper_OnApplicationDidReceiveMemoryWarning;
		tk2dUIItem.OnStaticClick += Tk2dUIItem_OnStaticClick;
		isEnableManager = true;
	}

	private void OnDisable()
	{
		NativeEventsWrapper.OnApplicationDidReceiveMemoryWarning -= NativeEventsWrapper_OnApplicationDidReceiveMemoryWarning;
		tk2dUIItem.OnStaticClick -= Tk2dUIItem_OnStaticClick;
		SoundsManager.StopAllSounds();
		isEnableManager = false;
	}

	private void LateUpdate()
	{
		while (actionQueue.Count > 0)
		{
			Action action = actionQueue.Dequeue();
			if (action != null)
			{
				action();
			}
		}
	}

	public void EnableMusic()
	{
		IsMusicEnabled = true;
	}

	public void DisableMusic()
	{
		IsMusicEnabled = false;
	}

	public void EnableSounds()
	{
		IsSoundsEnabled = true;
	}

	public void DisableSounds()
	{
		IsSoundsEnabled = false;
	}

	public void MuteSounds()
	{
		IsMuteSounds = true;
	}

	public void UnMuteSounds()
	{
		IsMuteSounds = false;
	}

	public void MuteInGameSonds()
	{
		IsMuteInGameSounds = true;
	}

	public void UnMuteInGameSonds()
	{
		IsMuteInGameSounds = false;
	}

	public void MuteUISonds()
	{
		IsMuteUISounds = true;
	}

	public void UnMuteUISonds()
	{
		IsMuteUISounds = false;
	}

	public static void PlayLoopAudioWithKey(string key)
	{
		SingletonMonoBehaviour<AudioManager>.Instance._PlayLoopAudioWithKey(key);
	}

	protected void _PlayLoopAudioWithKey(string audioKey)
	{
		if (!isEnableManager || string.IsNullOrEmpty(audioKey))
		{
			return;
		}
		actionQueue.Enqueue(delegate
		{
			if (!SoundsManager.IsPlaying(audioKey))
			{
				SoundsManager.Play(audioKey);
			}
		});
	}

	public static void PlaySingleAudioWithKey(string k)
	{
		SingletonMonoBehaviour<AudioManager>.Instance._PlaySingleAudioWithKey(k);
	}

	protected void _PlaySingleAudioWithKey(string audioKey)
	{
		if (!isEnableManager || string.IsNullOrEmpty(audioKey))
		{
			return;
		}
		actionQueue.Enqueue(delegate
		{
			if (!SoundsManager.IsPlaying(audioKey))
			{
				SoundsManager.Play(audioKey);
			}
		});
	}

	public static void PlayAudioWithKey(string k, Action<SoundObject> callback = null)
	{
		SingletonMonoBehaviour<AudioManager>.Instance._PlayAudioWithKey(k, callback);
	}

	protected void _PlayAudioWithKey(string audioKey, Action<SoundObject> callback = null)
	{
		if (!isEnableManager || string.IsNullOrEmpty(audioKey))
		{
			return;
		}
		actionQueue.Enqueue(delegate
		{
			SoundObject soundObject = SoundsManager.Play(audioKey);
			if (callback != null && soundObject != null)
			{
				callback(soundObject);
			}
		});
	}

	public static void StopAudioWithKey(string k, float fade = 0f)
	{
		SingletonMonoBehaviour<AudioManager>.Instance._StopAudioWithKey(k, fade);
	}

	protected void _StopAudioWithKey(string audioKey, float fade)
	{
		if (string.IsNullOrEmpty(audioKey))
		{
			return;
		}
		actionQueue.Enqueue(delegate
		{
			if (SoundsManager.IsPlaying(audioKey))
			{
				SoundsManager.Stop(audioKey, fade);
			}
		});
	}

	private void Tk2dUIItem_OnStaticClick(UISoundType obj)
	{
		switch (obj)
		{
		case UISoundType.Button:
			_PlayAudioWithKey("button");
			break;
		case UISoundType.CancelButton:
			_PlayAudioWithKey("no_money");
			break;
		}
	}

	private void InitializeSoundCategoriesVolumes()
	{
		SoundsCategory[] audioCategories = SingletonMonoBehaviour<SoundsManager>.Instance.AudioCategories;
		foreach (SoundsCategory soundsCategory in audioCategories)
		{
			soundCategoriesVolumes.Add(soundsCategory.Name, soundsCategory.Volume);
		}
	}

	private void EnableSoundsVolume()
	{
		foreach (string key in soundCategoriesVolumes.Keys)
		{
			if (!key.Equals("Music"))
			{
				SoundsManager.SetCategoryVolume(key, soundCategoriesVolumes[key]);
			}
		}
	}

	private void DisableSoundsVolume()
	{
		foreach (string key in soundCategoriesVolumes.Keys)
		{
			if (!key.Equals("Music"))
			{
				SoundsManager.SetCategoryVolume(key, 0f);
			}
		}
	}

	private void MuteCategoriesExceptSelected(List<string> categoryNames)
	{
		foreach (string key in soundCategoriesVolumes.Keys)
		{
			if (!categoryNames.Contains(key))
			{
				SoundsManager.SetCategoryVolume(key, 0f);
			}
		}
	}

	private void UnMuteCategoriesExceptSelected(List<string> categoryNames)
	{
		foreach (string key in soundCategoriesVolumes.Keys)
		{
			if (!categoryNames.Contains(key))
			{
				SoundsManager.SetCategoryVolume(key, soundCategoriesVolumes[key]);
			}
		}
	}

	private void NativeEventsWrapper_OnApplicationDidReceiveMemoryWarning()
	{
		SingletonMonoBehaviour<SoundsManager>.Instance.StreamingAudioBuffer.UnloadAssets();
	}
}
