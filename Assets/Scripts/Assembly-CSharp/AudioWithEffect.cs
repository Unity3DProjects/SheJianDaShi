using UnityEngine;

public class AudioWithEffect : MonoBehaviour
{
	[SerializeField]
	private string soundName;

	[SerializeField]
	[Range(0f, 1f)]
	private float volume = 1f;

	[SerializeField]
	private bool isStopOnEffectDestroy;

	[SerializeField]
	private float fadeOutDuration;

	private void Awake()
	{
		AudioManager.PlayAudioWithKey(soundName, delegate(SoundObject obj)
		{
			if (obj != null)
			{
				obj.Volume = volume;
			}
		});
	}

	private void OnDestroy()
	{
		if (isStopOnEffectDestroy && SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			AudioManager.StopAudioWithKey(soundName, fadeOutDuration);
		}
	}
}
