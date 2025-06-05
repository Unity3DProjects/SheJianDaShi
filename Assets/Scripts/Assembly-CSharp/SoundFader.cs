using UnityEngine;

public class SoundFader
{
	private float fadeDuration;

	private float initialValue = 1f;

	private float targetValue = 1f;

	public float Elapsed { get; set; }

	public bool IsFading
	{
		get
		{
			return Elapsed < fadeDuration;
		}
	}

	public float Fade
	{
		get
		{
			float value = ((!(fadeDuration <= 0f)) ? Mathf.Lerp(initialValue, targetValue, Elapsed / fadeDuration) : targetValue);
			return Mathf.Clamp01(value);
		}
	}

	public void FadeIn(float time)
	{
		initialValue = ((!IsFading) ? 0f : Fade);
		Elapsed = 0f;
		fadeDuration = time;
		targetValue = 1f;
	}

	public void FadeOut(float time)
	{
		initialValue = ((!IsFading) ? 1f : Fade);
		Elapsed = 0f;
		fadeDuration = time;
		targetValue = 0f;
	}

	public void Reset()
	{
		Elapsed = 0f;
		fadeDuration = 0f;
		initialValue = 1f;
		targetValue = 1f;
	}
}
