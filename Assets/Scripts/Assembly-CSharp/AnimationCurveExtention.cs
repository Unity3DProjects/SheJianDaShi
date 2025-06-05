using UnityEngine;

public static class AnimationCurveExtention
{
	public static float RoundTo(float x, float step)
	{
		return Mathf.Round(x / step) * step;
	}

	public static void RoundEdges(this AnimationCurve curve, float startTime = 0f, float endTime = 1f, float yStep = 0.05f)
	{
		Keyframe[] keys = curve.keys;
		if (keys.Length >= 2)
		{
			keys[0].time = startTime;
			keys[0].value = RoundTo(keys[0].value, yStep);
			keys[keys.Length - 1].time = endTime;
			keys[keys.Length - 1].value = RoundTo(keys[keys.Length - 1].value, yStep);
		}
		curve.keys = keys;
	}
}
