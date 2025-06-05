using System;
using UnityEngine;

[Serializable]
public class CurvesData
{
	public string id;

	public AnimationCurve curve;

	public Keyframe begin;

	public Keyframe end;

	public CurvesData()
	{
		id = "<enter id>";
		begin = new Keyframe(0f, 0f, 0f, 1f);
		end = new Keyframe(1f, 1f, 1f, 0f);
		curve = new AnimationCurve(begin, end);
	}

	public CurvesData(string id, AnimationCurve newCurve)
	{
		this.id = id;
		begin = newCurve.keys[0];
		end = newCurve.keys[newCurve.keys.Length - 1];
		curve = new AnimationCurve(newCurve.keys);
	}
}
