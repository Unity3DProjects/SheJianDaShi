using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : RegisteredComponent
{
	[SerializeField]
	private bool mute;

	[SerializeField]
	private bool loop;

	[SerializeField]
	private int priority = 128;

	[SerializeField]
	private float volume = 1f;

	[SerializeField]
	private float pitch = 1f;

	[SerializeField]
	private float pan;

	[SerializeField]
	private float spatialBlend;

	[SerializeField]
	private float spread;

	[SerializeField]
	private float minDistance = 1f;

	[SerializeField]
	private float maxDistance = 500f;

	[SerializeField]
	private bool overrideZAxis;

	[SerializeField]
	private float zAxisPosition;

	[NonSerialized]
	private SoundSubItem subItem;

	protected bool isDirty;

	protected float masterVolume = 1f;

	protected float startPlayingTime;

	protected bool isPaused;

	protected bool isPlaying = true;

	protected bool isStopped;

	protected float playingTime;

	protected bool scheduledNextItem;

	protected float totalPlayingTime;

	protected float playingDuration;

	private HashSet<string> pauseSources = new HashSet<string>();

	private SoundFader fader = new SoundFader();

	private Action onStopCallback;

	private Transform cachedTransform;

	public Transform CachedTransfrom
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

	public bool Mute
	{
		get
		{
			return mute;
		}
		set
		{
			if (mute != value)
			{
				mute = value;
				IsDirty = true;
				ApplyMuteChange();
			}
		}
	}

	public bool Loop
	{
		get
		{
			return loop;
		}
		set
		{
			if (loop != value)
			{
				loop = value;
				IsDirty = true;
				ApplyLoopChange();
			}
		}
	}

	public int Priority
	{
		get
		{
			return priority;
		}
		set
		{
			priority = value;
		}
	}

	public float Volume
	{
		get
		{
			return volume;
		}
		set
		{
			if (volume != value)
			{
				volume = value;
				IsDirty = true;
				ApplyVolumeChange();
			}
		}
	}

	public float MasterVolume
	{
		get
		{
			return masterVolume;
		}
		set
		{
			if (masterVolume != value)
			{
				masterVolume = value;
				IsDirty = true;
				ApplyVolumeChange();
			}
		}
	}

	public float TotalVolume
	{
		get
		{
			return MasterVolume * Volume * fader.Fade;
		}
	}

	public float Pitch
	{
		get
		{
			return pitch;
		}
		set
		{
			if (pitch != value)
			{
				pitch = value;
				IsDirty = true;
				ApplyPitchChange();
			}
		}
	}

	public float Pan
	{
		get
		{
			return pan;
		}
		set
		{
			if (pan != value)
			{
				pan = value;
				IsDirty = true;
				ApplyPanChange();
			}
		}
	}

	public float Spread
	{
		get
		{
			return spread;
		}
		set
		{
			if (spread != value)
			{
				spread = value;
				IsDirty = true;
				ApplySpreadChange();
			}
		}
	}

	public float SpatialBlend
	{
		get
		{
			return spatialBlend;
		}
		set
		{
			if (spatialBlend != value)
			{
				spatialBlend = value;
				IsDirty = true;
				ApplySpatialBlendChange();
			}
		}
	}

	public float MinDistance
	{
		get
		{
			return minDistance;
		}
		set
		{
			if (minDistance != value)
			{
				minDistance = value;
				IsDirty = true;
				ApplyMinDistanceChange();
			}
		}
	}

	public float MaxDistance
	{
		get
		{
			return maxDistance;
		}
		set
		{
			if (maxDistance != value)
			{
				maxDistance = value;
				IsDirty = true;
				ApplyMaxDistanceChange();
			}
		}
	}

	public bool OverrideZAxis
	{
		get
		{
			return overrideZAxis;
		}
		set
		{
			if (overrideZAxis != value)
			{
				overrideZAxis = value;
				IsDirty = true;
				ApplyOverrideZAxisChange();
			}
		}
	}

	public float ZAxisPosition
	{
		get
		{
			return zAxisPosition;
		}
		set
		{
			if (zAxisPosition != value)
			{
				zAxisPosition = value;
				IsDirty = true;
				ApplyZAxisPositionChange();
			}
		}
	}

	public string SoundID
	{
		get
		{
			return subItem.Item.Name;
		}
	}

	public SoundSubItem SubItem
	{
		get
		{
			return subItem;
		}
		internal set
		{
			subItem = value;
			ApplySubItem();
		}
	}

	public bool IsMusic { get; set; }

	public float StartPlayingTime
	{
		get
		{
			return startPlayingTime;
		}
		internal set
		{
			startPlayingTime = value;
		}
	}

	public float PlayingTime
	{
		get
		{
			return playingTime;
		}
		set
		{
		}
	}

	public bool IsPlaying
	{
		get
		{
			return isPlaying;
		}
	}

	public bool IsPaused
	{
		get
		{
			return isPaused;
		}
		protected set
		{
			if (isPaused != value)
			{
				isPaused = value;
				ApplyPause();
			}
		}
	}

	public float PlayingDuration
	{
		get
		{
			return playingDuration;
		}
		set
		{
			if (playingDuration != value)
			{
				playingDuration = value;
			}
		}
	}

	protected bool IsDirty
	{
		get
		{
			return isDirty;
		}
		set
		{
			isDirty = value;
		}
	}

	public Action Callback
	{
		get
		{
			return onStopCallback;
		}
		set
		{
			onStopCallback = value;
		}
	}

	protected virtual void ApplySubItem()
	{
	}

	protected virtual void ApplyPause()
	{
	}

	protected virtual void ApplyMuteChange()
	{
	}

	protected virtual void ApplyLoopChange()
	{
	}

	protected virtual void ApplyPriorityChange()
	{
	}

	protected virtual void ApplyVolumeChange()
	{
	}

	protected virtual void ApplyPitchChange()
	{
	}

	protected virtual void ApplyPanChange()
	{
	}

	protected virtual void ApplySpatialBlendChange()
	{
	}

	protected virtual void ApplySpreadChange()
	{
	}

	protected virtual void ApplyMinDistanceChange()
	{
	}

	protected virtual void ApplyMaxDistanceChange()
	{
	}

	protected virtual void ApplyOverrideZAxisChange()
	{
	}

	protected virtual void ApplyZAxisPositionChange()
	{
	}

	public virtual void Play(float delay, float fadeIn = 0f)
	{
		StopAllCoroutines();
		if (base.gameObject.activeInHierarchy)
		{
			isPaused = true;
			StartCoroutine(WaitForSeconds(delay, delegate
			{
				playingTime = 0f;
				isPlaying = true;
				isPaused = false;
				pauseSources.Clear();
				fader.Reset();
				FadeIn(fadeIn);
				OnPlay();
				StartCoroutine(ElapseFader(fadeIn));
			}));
		}
	}

	public virtual void OnPlay()
	{
	}

	public void Pause(bool pause, string pauseSource, float fadeTime = 0f)
	{
		if (isStopped)
		{
			return;
		}
		if (pause)
		{
			StopAllCoroutines();
			FadeOut(fadeTime);
			pauseSources.Add(pauseSource);
			StartCoroutine(ElapseFader(fadeTime, delegate
			{
				IsPaused = true;
			}));
		}
		else if (pauseSources.Remove(pauseSource) && pauseSources.Count == 0)
		{
			StopAllCoroutines();
			FadeIn(fadeTime);
			StartCoroutine(ElapseFader(fadeTime, delegate
			{
				IsPaused = false;
			}));
		}
	}

	public void Pause()
	{
		IsPaused = true;
	}

	public virtual void Stop(float fadeOut = 0f)
	{
		if (isStopped)
		{
			return;
		}
		isStopped = true;
		StopAllCoroutines();
		FadeOut(fadeOut);
		if (base.gameObject.activeInHierarchy && fadeOut > 0f && !isPaused)
		{
			StartCoroutine(ElapseFader(fadeOut, delegate
			{
				StopSound();
			}));
		}
		else
		{
			StopSound();
		}
	}

	internal virtual void StopSound()
	{
		playingTime = 0f;
		isPlaying = false;
		isPaused = false;
		pauseSources.Clear();
		scheduledNextItem = false;
	}

	protected IEnumerator ElapseFader(float time, Action action = null)
	{
		time = Mathf.Max(0f, time);
		for (float currentTime = 0f; currentTime <= time; currentTime += Time.deltaTime)
		{
			fader.Elapsed = currentTime;
			IsDirty = true;
			ApplyVolumeChange();
			yield return null;
		}
		yield return null;
		if (action != null)
		{
			action();
		}
	}

	private IEnumerator WaitForSeconds(float delay, Action action)
	{
		yield return new WaitForSecondsRealtime(delay);
		if (action != null)
		{
			action();
		}
	}

	protected void FadeIn(float fadeTime)
	{
		fader.FadeIn(fadeTime);
	}

	protected void FadeOut(float fadeTime)
	{
		fader.FadeOut(fadeTime);
	}

	internal bool DoesBelongToCategory(string categoryName)
	{
		for (SoundsCategory soundsCategory = SubItem.Item.Category; soundsCategory != null; soundsCategory = soundsCategory.ParentCategory)
		{
			if (soundsCategory.Name == categoryName)
			{
				return true;
			}
		}
		return false;
	}

	internal static void CopyParameters(SoundObject source, SoundObject target)
	{
		target.Mute = source.mute;
		target.Priority = source.priority;
		target.Volume = source.volume;
		target.masterVolume = source.masterVolume;
		target.Pitch = source.pitch;
		target.Pan = source.pan;
		target.SpatialBlend = source.spatialBlend;
		target.Spread = source.spread;
		target.MinDistance = source.minDistance;
		target.MaxDistance = source.maxDistance;
		target.OverrideZAxis = source.OverrideZAxis;
		target.ZAxisPosition = source.ZAxisPosition;
	}

	internal virtual void ApplySettings(SoundObject source)
	{
		CopyParameters(source, this);
	}

	internal virtual void DestroyAudioObject()
	{
		StopAllCoroutines();
		ObjectPoolController.Destroy(base.gameObject);
	}

	internal virtual bool IsFinish()
	{
		return !IsPlaying && !IsPaused;
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{
		Stop();
		DestroyAudioObject();
	}

	protected virtual void Update()
	{
		if (isPlaying && !isPaused)
		{
			MasterVolume = SingletonMonoBehaviour<SoundsManager>.Instance.Volume * SubItem.Item.Volume * SubItem.Item.Category.VolumeTotal;
			playingTime += Time.unscaledDeltaTime;
			totalPlayingTime += Time.unscaledDeltaTime;
			if (playingDuration > 0f && totalPlayingTime > playingDuration)
			{
				DestroyAudioObject();
			}
		}
	}

	protected virtual void LateUpdate()
	{
		if (SubItem.Item.OverlapTime > 0f && !isStopped && PlayingTime > SubItem.Item.OverlapTime && !scheduledNextItem)
		{
			scheduledNextItem = true;
			if (IsMusic)
			{
				SoundsManager.PlayMusic(SoundID);
			}
			else
			{
				SoundsManager.Play(SoundID, base.transform.parent);
			}
			Stop(SubItem.FadeOut);
		}
		if (IsFinish())
		{
			DestroyAudioObject();
		}
		IsDirty = false;
	}

	protected override void OnDestroy()
	{
		Stop();
		base.OnDestroy();
	}
}
