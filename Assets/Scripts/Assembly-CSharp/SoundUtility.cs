using UnityEngine;

public static class SoundUtility
{
	public static float TransformPitch(float pitchSemiTones)
	{
		return Mathf.Pow(2f, pitchSemiTones / 12f);
	}
}
