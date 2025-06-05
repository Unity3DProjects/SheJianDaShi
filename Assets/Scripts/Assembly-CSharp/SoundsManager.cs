using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : SingletonMonoBehaviour<SoundsManager>
{
	[SerializeField]
	public AudioListener AudioListenerReference;

	[SerializeField]
	private StreamingAudioBuffer streamingAudioBuffer;

	[SerializeField]
	public SoundsCategory[] AudioCategories;

	[SerializeField]
	public SoundSettings SoundSettings;

	[SerializeField]
	public SoundObject SimpleSoundPrefab;

	[SerializeField]
	public SoundObject NativeSoundPrefab;

	[SerializeField]
	public SoundObject StreamingSoundPrefab;

	[SerializeField]
	public bool Persistent;

	[SerializeField]
	public bool UnloadAudioClipsOnDestroy;

	[SerializeField]
	public bool UsePooledAudioObjects = true;

	[SerializeField]
	public bool PlayWithZeroVolume;

	[SerializeField]
	private bool audioDisabled;

	[SerializeField]
	private float mainVolume = 1f;

	protected AudioListener currentAudioListener;

	protected SoundObject currentMusic;

	private static double lastSystemTime = -1.0;

	private static double systemDeltaTime = -1.0;

	private static double systemTime = -1.0;

	private Vector3 listenerLastPosition;

	private bool editorPaused;

	private static List<SoundObject> matchesList = new List<SoundObject>();

	private static List<string> matchesListString = new List<string>();

	private Dictionary<string, SoundItem> items = new Dictionary<string, SoundItem>();

	public StreamingAudioBuffer StreamingAudioBuffer
	{
		get
		{
			return base.gameObject.GetRequiredComponent(ref streamingAudioBuffer);
		}
		set
		{
			streamingAudioBuffer = value;
		}
	}

	public bool DisableAudio
	{
		get
		{
			return audioDisabled;
		}
		set
		{
			if (audioDisabled != value)
			{
				PauseAllSounds(value, "DisableAudio");
				audioDisabled = value;
			}
		}
	}

	public float Volume
	{
		get
		{
			return mainVolume;
		}
		set
		{
			if (value != mainVolume)
			{
				mainVolume = value;
				ApplyVolumeChange(mainVolume);
			}
		}
	}

	private void Start()
	{
		AudioListenerReference = Camera.main.GetComponent<AudioListener>();
		AudioListenerReference.enabled = true;
	}

	public static SoundObject PlayMusic(string audioID)
	{
		return PlayMusic(audioID, 1f, 0f, 0f);
	}

	public static SoundObject PlayMusic(string audioID, float volume, float delay, float startTime)
	{
		AudioListener audioListener = GetCurrentAudioListener();
		if (audioListener == null)
		{
			CustomDebug.LogWarning("No AudioListener found in the scene");
			return null;
		}
		return SingletonMonoBehaviour<SoundsManager>.Instance._PlayMusic(audioID, audioListener.transform.position + audioListener.transform.forward, null, volume, delay);
	}

	public static bool StopMusic(float fadeOut = 0f)
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance._StopMusic(fadeOut);
	}

	public static bool PauseMusic(float fadeOut = 0f)
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance._PauseMusic(fadeOut);
	}

	public static bool UnpauseMusic(float fadeIn = 0f)
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance._UnpauseMusic(fadeIn);
	}

	public static bool IsMusicPaused()
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance._IsMusicPaused();
	}

	public static SoundObject GetCurrentMusic()
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance.currentMusic;
	}

	protected bool _StopMusic(float fadeOut)
	{
		if (currentMusic != null)
		{
			currentMusic.Stop(fadeOut);
			currentMusic = null;
			return true;
		}
		return false;
	}

	protected bool _PauseMusic(float fadeOut)
	{
		if (currentMusic != null)
		{
			currentMusic.Pause(true, string.Empty, fadeOut);
			return true;
		}
		return false;
	}

	protected SoundObject _PlayMusic(string audioID, Vector3 position, Transform parentObj, float volume, float delay)
	{
		if (currentMusic != null && currentMusic.IsPlaying)
		{
			currentMusic.Stop(currentMusic.SubItem.FadeOut);
		}
		currentMusic = Play(audioID, volume, position, parentObj, false);
		currentMusic.IsMusic = true;
		return currentMusic;
	}

	public bool _IsMusicPaused()
	{
		if (currentMusic != null)
		{
			return currentMusic.IsPaused;
		}
		return false;
	}

	public bool _UnpauseMusic(float fadeIn = 0f)
	{
		if (currentMusic != null && currentMusic.IsPaused)
		{
			currentMusic.Pause(false, string.Empty);
			return true;
		}
		return false;
	}

	public static SoundObject Play(string audioID)
	{
		AudioListener audioListener = GetCurrentAudioListener();
		if (audioListener == null)
		{
			CustomDebug.LogWarning("No AudioListener found in the scene");
			return null;
		}
		return Play(audioID, audioListener.transform.position + audioListener.transform.forward, null, 1f);
	}

	public static SoundObject Play(string audioID, Transform parentObj)
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance.Play(audioID, 1f, parentObj.position, parentObj, false);
	}

	public static SoundObject Play(string audioID, Vector3 pos)
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance.Play(audioID, 1f, pos, null, false);
	}

	public static SoundObject Play(string audioID, Vector3 worldPosition, Transform parentObj, float volume)
	{
		return SingletonMonoBehaviour<SoundsManager>.Instance.Play(audioID, volume, worldPosition, parentObj, false);
	}

	protected SoundObject Play(string audioID, float volume, Vector3 worldPosition, Transform parentObj, bool playWithoutAudioObject, SoundObject useExistingAudioObject = null)
	{
		if (audioDisabled)
		{
			return null;
		}
		SoundItem audioItem = GetAudioItem(audioID);
		if (audioItem == null)
		{
			CustomDebug.LogWarning("Audio item with name '" + audioID + "' does not exist");
			return null;
		}
		if (audioItem.lastPlayedTime > 0.0 && systemTime < audioItem.lastPlayedTime + (double)audioItem.MinTimeBetweenPlays)
		{
			return null;
		}
		if (audioItem.MaxInstanceCount > 0)
		{
			SoundObject[] playingAudioObjects = GetPlayingAudioObjects(audioID);
			if (playingAudioObjects.Length >= audioItem.MaxInstanceCount)
			{
				if (audioItem.SkipWhenExeeded)
				{
					return null;
				}
				SoundObject soundObject = null;
				for (int i = 0; i < playingAudioObjects.Length; i++)
				{
					if (soundObject == null || playingAudioObjects[i].StartPlayingTime < soundObject.StartPlayingTime)
					{
						soundObject = playingAudioObjects[i];
					}
				}
				if (soundObject != null)
				{
					soundObject.Stop(soundObject.SubItem.FadeOut);
				}
			}
		}
		return PlayAudioItem(audioItem, worldPosition, parentObj, playWithoutAudioObject, useExistingAudioObject);
	}

	public SoundObject PlayAudioItem(SoundItem sndItem, Vector3 worldPosition, Transform parentObj = null, bool playWithoutAudioObject = false, SoundObject useExistingAudioObj = null)
	{
		SoundSubItem[] array = sndItem.ChooseSubItems(sndItem.SubItemPickMode);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		sndItem.lastPlayedTime = systemTime;
		SoundObject result = null;
		foreach (SoundSubItem soundSubItem in array)
		{
			if (soundSubItem != null)
			{
				result = PlayAudioSubItem(sndItem.Category, sndItem, soundSubItem, worldPosition, parentObj, playWithoutAudioObject, null);
			}
		}
		return result;
	}

	public SoundObject PlayAudioSubItem(SoundsCategory audioCategory, SoundItem audioItem, SoundSubItem subItem, Vector3 worldPosition, Transform parentObj, bool playWithoutAudioObject, SoundObject existingAudioObj, Action<SoundObject> onLoad = null)
	{
		GameObject prefabForSoundType = GetPrefabForSoundType(subItem.SubItemType);
		if (prefabForSoundType == null)
		{
			CustomDebug.LogError("missing prefab for sound : " + audioCategory.Name + "/" + audioItem.Name + "   with type : " + subItem.SubItemType);
			CustomDebug.LogError(subItem.clipref.FullPath);
			return null;
		}
		float num = subItem.Volume;
		if (subItem.RandomVolume != 0f)
		{
			num += UnityEngine.Random.Range(0f - subItem.RandomVolume, subItem.RandomVolume);
			num = Mathf.Clamp01(num);
		}
		float num2 = Volume * audioCategory.VolumeTotal * audioItem.Volume;
		float num3 = num2 * num;
		if (!PlayWithZeroVolume && num3 <= 0f)
		{
			return null;
		}
		SoundSettings source = SoundSettings;
		if (audioCategory.SoundSettingsOverride != null)
		{
			source = audioCategory.SoundSettingsOverride;
		}
		if (audioItem.SoundSettingsOverride != null)
		{
			source = audioItem.SoundSettingsOverride;
		}
		GameObject gameObject;
		SoundObject soundObject;
		if (existingAudioObj == null)
		{
			gameObject = ObjectPoolController.Instantiate(prefabForSoundType, worldPosition, Quaternion.identity);
			if (!audioItem.DestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			gameObject.transform.parent = ((!parentObj) ? base.transform : parentObj);
			soundObject = gameObject.gameObject.GetComponent<SoundObject>();
		}
		else
		{
			gameObject = existingAudioObj.gameObject;
			soundObject = existingAudioObj;
		}
		gameObject.name = "SoundObject:" + subItem.Name;
		soundObject.SubItem = subItem;
		soundObject.ApplySettings(source);
		if (audioItem.overrideAudioSourceSettings)
		{
			soundObject.MinDistance = audioItem.audioSource_MinDistance;
			soundObject.MaxDistance = audioItem.audioSource_MaxDistance;
		}
		soundObject.Volume = num;
		soundObject.MasterVolume = num2;
		soundObject.Loop = audioItem.Loop == SoundLoopMode.LoopSubitem;
		soundObject.Pan = subItem.Pan2D;
		soundObject.Pitch = subItem.GetPitch();
		float num4 = 0f;
		if (subItem.RandomDelay > 0f)
		{
			num4 += UnityEngine.Random.Range(0f, subItem.RandomDelay);
		}
		soundObject.Play(num4 + subItem.Delay + audioItem.Delay, subItem.FadeIn);
		if (onLoad != null)
		{
			onLoad(soundObject);
		}
		return soundObject;
	}

	public static bool Stop(string audioID, float fadeOut = 0f)
	{
		SoundItem audioItem = SingletonMonoBehaviour<SoundsManager>.Instance.GetAudioItem(audioID);
		if (audioItem == null)
		{
			CustomDebug.LogWarning("Audio item with name '" + audioID + "' does not exist");
			return false;
		}
		SoundObject[] playingAudioObjects = GetPlayingAudioObjects(audioID);
		foreach (SoundObject soundObject in playingAudioObjects)
		{
			soundObject.Stop(fadeOut);
		}
		return playingAudioObjects.Length > 0;
	}

	public static bool Stop(string audioID, Transform parentTransform, float fadeOut = 0f)
	{
		SoundItem audioItem = SingletonMonoBehaviour<SoundsManager>.Instance.GetAudioItem(audioID);
		if (audioItem == null)
		{
			CustomDebug.LogWarning("Audio item with name '" + audioID + "' does not exist");
			return false;
		}
		SoundObject[] playingAudioObjects = GetPlayingAudioObjects(audioID);
		foreach (SoundObject soundObject in playingAudioObjects)
		{
			if (soundObject.transform.parent == parentTransform)
			{
				soundObject.Stop(fadeOut);
			}
		}
		return playingAudioObjects.Length > 0;
	}

	public GameObject GetPrefabForSoundType(SoundClipType clipType)
	{
		return SimpleSoundPrefab.gameObject;
	}

	public static AudioListener GetCurrentAudioListener()
	{
		SoundsManager instance = SingletonMonoBehaviour<SoundsManager>.Instance;
		if ((bool)instance && !instance.currentAudioListener)
		{
			if (instance.AudioListenerReference != null)
			{
				instance.currentAudioListener = instance.AudioListenerReference;
			}
			else
			{
				instance.currentAudioListener = (AudioListener)UnityEngine.Object.FindObjectOfType(typeof(AudioListener));
			}
		}
		return instance.currentAudioListener;
	}

	public static SoundObject[] GetPlayingAudioObjects(string soundID, bool includePausedAudio = false)
	{
		matchesList.Clear();
		object[] allOfType = RegisteredComponentController.GetAllOfType(typeof(SoundObject));
		for (int i = 0; i < allOfType.Length; i++)
		{
			SoundObject soundObject = allOfType[i] as SoundObject;
			if ((soundObject.IsPlaying || (includePausedAudio && soundObject.IsPaused)) && (string.IsNullOrEmpty(soundID) || soundObject.SoundID.Equals(soundID)))
			{
				matchesList.Add(soundObject);
			}
		}
		return matchesList.ToArray();
	}

	public static List<string> GetUsedAudioClipNames()
	{
		matchesListString.Clear();
		object[] allOfType = RegisteredComponentController.GetAllOfType(typeof(SoundObject));
		for (int i = 0; i < allOfType.Length; i++)
		{
			SoundObject soundObject = allOfType[i] as SoundObject;
			if ((soundObject.IsPlaying || soundObject.IsPaused) && soundObject.SubItem.SubItemType == SoundClipType.StreamingClip)
			{
				matchesListString.Add(soundObject.SubItem.Name);
			}
		}
		return matchesListString;
	}

	public static void StopAllSounds(float fadeOut = 0f)
	{
		SoundObject[] playingAudioObjects = GetPlayingAudioObjects(string.Empty, true);
		foreach (SoundObject soundObject in playingAudioObjects)
		{
			soundObject.Stop(fadeOut);
		}
	}

	public static void PauseCategory(string categoryName, string pauseSource = "")
	{
		SoundObject[] playingAudioObjects = GetPlayingAudioObjects(string.Empty);
		foreach (SoundObject soundObject in playingAudioObjects)
		{
			if (soundObject.DoesBelongToCategory(categoryName))
			{
				soundObject.Pause(true, pauseSource);
			}
		}
	}

	public static bool IsPlaying(string audioID)
	{
		return GetPlayingAudioObjects(audioID).Length > 0;
	}

	public static void SetCategoryVolume(string categoryName, float volume)
	{
		bool flag = false;
		SoundsCategory category = SingletonMonoBehaviour<SoundsManager>.Instance.GetCategory(categoryName);
		if (category != null)
		{
			category.Volume = volume;
			flag = true;
		}
		if (!flag)
		{
			CustomDebug.LogWarning("No audio category with name " + categoryName);
		}
	}

	public static float GetCategoryVolume(string categoryName)
	{
		SoundsCategory category = SingletonMonoBehaviour<SoundsManager>.Instance.GetCategory(categoryName);
		if (category != null)
		{
			return category.Volume;
		}
		CustomDebug.LogWarning("No audio category with name " + categoryName);
		return 0f;
	}

	private static void UpdateSystemTime()
	{
		double timeSinceLaunch = SystemTime.timeSinceLaunch;
		if (lastSystemTime >= 0.0)
		{
			systemDeltaTime = timeSinceLaunch - lastSystemTime;
			systemTime += systemDeltaTime;
		}
		else
		{
			systemDeltaTime = 0.0;
			systemTime = 0.0;
		}
		lastSystemTime = timeSinceLaunch;
	}

	public void ValidateSounds()
	{
		for (int i = 0; i < AudioCategories.Length; i++)
		{
			AudioCategories[i].Initialize(this);
		}
	}

	internal SoundItem GetAudioItem(string audioID)
	{
		ValidateSounds();
		Dictionary<string, SoundItem> dictionary = AllSoundItems();
		SoundItem value;
		if (dictionary.TryGetValue(audioID, out value))
		{
			return value;
		}
		return null;
	}

	internal Dictionary<string, SoundItem> AllSoundItems()
	{
		items.Clear();
		for (int i = 0; i < AudioCategories.Length; i++)
		{
			SoundsCategory soundsCategory = AudioCategories[i];
			for (int j = 0; j < soundsCategory.AudioItems.Length; j++)
			{
				SoundItem soundItem = soundsCategory.AudioItems[j];
				if (items.ContainsKey(soundItem.Name))
				{
					CustomDebug.LogError(soundItem.Name + " " + soundsCategory.Name);
				}
				else
				{
					items.Add(soundItem.Name, soundItem);
				}
			}
		}
		return items;
	}

	public void PauseAllSounds(bool isPaused, string pauseSource)
	{
		SoundObject[] playingAudioObjects = GetPlayingAudioObjects(string.Empty, true);
		foreach (SoundObject soundObject in playingAudioObjects)
		{
			soundObject.Pause(isPaused, pauseSource);
		}
	}

	private void ApplyVolumeChange(float v)
	{
		SoundObject[] playingAudioObjects = GetPlayingAudioObjects(string.Empty, true);
		SoundObject[] array = playingAudioObjects;
		foreach (SoundObject soundObject in array)
		{
			soundObject.MasterVolume = v;
		}
	}

	public SoundsCategory GetCategory(string categoryName)
	{
		for (int i = 0; i < AudioCategories.Length; i++)
		{
			SoundsCategory soundsCategory = AudioCategories[i];
			if (soundsCategory.Name.Equals(categoryName))
			{
				return soundsCategory;
			}
		}
		return null;
	}

	private void Update()
	{
		UpdateListenerPosition();
		UpdateSystemTime();
	}

	private void UpdateListenerPosition()
	{
		AudioListener audioListener = GetCurrentAudioListener();
		if (audioListener != null)
		{
			Vector3 position = audioListener.transform.position;
			if (position != listenerLastPosition)
			{
				LLSoundManager.SetListenerPosition(position);
			}
			listenerLastPosition = position;
		}
	}

	protected override void OnDestroy()
	{
		StopAllSounds();
		if (UnloadAudioClipsOnDestroy)
		{
		}
		base.OnDestroy();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		PauseAllSounds(pauseStatus, "OnApplicationPause");
	}
}
