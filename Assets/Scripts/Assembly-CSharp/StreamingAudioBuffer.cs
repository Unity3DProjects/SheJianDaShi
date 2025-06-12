using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamingAudioBuffer : MonoBehaviour
{
	private struct BufferItem
	{
		public AudioClip Clip;

		public float MemorySize;

		public float LastLoadTime;

		public BufferItem(AudioClip clipParam, float memorySizeParam)
		{
			Clip = clipParam;
			MemorySize = memorySizeParam;
			LastLoadTime = float.MaxValue;
		}

		public void UpdateLastLoadTime()
		{
			LastLoadTime = Time.realtimeSinceStartup;
		}
	}

	private const string STREAMING_ASSETS_FOLDER = "StreamingAssets";

	private const float MAX_BUFFER_SIZE = 10f;

	private const float CLIPS_SIZE_TO_UNLOAD = 5f;

	private const float MB_MULTIPLIER = 9.7E-07f;

	private const float DELAY_TO_UNLOAD_CLIP = 0.5f;

	private Dictionary<string, BufferItem> cachedClips = new Dictionary<string, BufferItem>();

	private List<string> loadingClips = new List<string>();

	private string streamingAssetsPath;

	private float currentBufferSize;

	private bool isBufferUnloadingAssets;

	private bool IsBufferUnloadingAssets
	{
		get
		{
			return isBufferUnloadingAssets;
		}
		set
		{
			if (isBufferUnloadingAssets != value)
			{
				isBufferUnloadingAssets = value;
				if (value)
				{
					UnloadAssetsFromBuffer();
				}
			}
		}
	}

	private void Awake()
	{
		streamingAssetsPath = Application.streamingAssetsPath.AppendWWWFilePrefix();
	}

	public void LoadClip(string clipName, string clipPath, Action<AudioClip> onFinish)
	{
		if (cachedClips.ContainsKey(clipName))
		{
			cachedClips[clipName].UpdateLastLoadTime();
			onFinish(cachedClips[clipName].Clip);
		}
		else if (!loadingClips.Contains(clipName))
		{
			loadingClips.Add(clipName);
			StartCoroutine(LoadClipCoroutine(clipName, clipPath, onFinish));
		}
	}

	public void UnloadClip(string audioClipNameToUnload, float time = 0f)
	{
		if (cachedClips.ContainsKey(audioClipNameToUnload))
		{
			currentBufferSize -= cachedClips[audioClipNameToUnload].MemorySize;
			UnityEngine.Object.Destroy(cachedClips[audioClipNameToUnload].Clip, time);
			cachedClips.Remove(audioClipNameToUnload);
		}
	}

	public void UnloadAssets()
	{
		IsBufferUnloadingAssets = true;
	}

	private IEnumerator LoadClipCoroutine(string clipName, string clipPath, Action<AudioClip> onFinish)
	{
		
		Debug.LogError($"从StreamingAssets加载：{clipName}————路径{clipPath}");
		
		int startIndex = clipPath.IndexOf("StreamingAssets", StringComparison.Ordinal);
		if (startIndex > -1)
		{
			clipPath = clipPath.Substring(startIndex + "StreamingAssets".Length);
		}
		string fullpath = streamingAssetsPath + clipPath;
		using (WWW www = new WWW(fullpath))
		{
			yield return www;
			AudioClip audioClip = www.GetAudioClip(false, false, AudioType.OGGVORBIS);
			audioClip.name = clipName;
			float bufferItemSize = (float)www.bytesDownloaded * 9.7E-07f;
			cachedClips.Add(clipName, new BufferItem(audioClip, bufferItemSize));
			loadingClips.Remove(clipName);
			if (onFinish != null)
			{
				onFinish(audioClip);
			}
			IncreaseBufferSize(bufferItemSize);
		}
	}

	private void IncreaseBufferSize(float newAudioClipSize)
	{
		currentBufferSize += newAudioClipSize;
		if (currentBufferSize > 10f)
		{
			IsBufferUnloadingAssets = true;
		}
	}

	private void UnloadAssetsFromBuffer()
	{
		float num = 0f;
		List<string> usedAudioClipNames = SoundsManager.GetUsedAudioClipNames();
		BufferItem[] array = new BufferItem[cachedClips.Values.Count];
		cachedClips.Values.CopyTo(array, 0);
		List<BufferItem> list = new List<BufferItem>(array);
		list.Sort((BufferItem a, BufferItem b) => a.LastLoadTime.CompareTo(b.LastLoadTime));
		List<string> list2 = new List<string>();
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			BufferItem bufferItem = list[num2];
			if (!usedAudioClipNames.Contains(bufferItem.Clip.name))
			{
				list2.Add(bufferItem.Clip.name);
				num += bufferItem.MemorySize;
				if (num > 5f)
				{
					break;
				}
			}
		}
		array = null;
		list.Clear();
		float num3 = 0f;
		foreach (string item in list2)
		{
			UnloadClip(item, num3);
			num3 += 0.5f;
		}
		IsBufferUnloadingAssets = false;
	}
}
