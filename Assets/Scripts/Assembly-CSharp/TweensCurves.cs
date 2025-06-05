using System.Collections.Generic;
using UnityEngine;

public class TweensCurves : ScriptableObject
{
	[SerializeField]
	private List<CurvesData> curves = new List<CurvesData>();

	private static TweensCurves instance;

	public static TweensCurves Instance
	{
		get
		{
			if (instance == null && instance == null)
			{
				instance = ScriptableObject.CreateInstance<TweensCurves>();
			}
			return instance;
		}
	}

	public List<string> IdList
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("default");
			foreach (CurvesData curf in curves)
			{
				list.Add(curf.id);
			}
			return list;
		}
	}

	private bool IsCurveExists(string id)
	{
		return curves.Exists((CurvesData data) => data.id.Equals(id));
	}

	private CurvesData FindCurve(string id)
	{
		return curves.Find((CurvesData data) => data.id.Equals(id));
	}

	public void AddCurve(string id, AnimationCurve newCurve)
	{
		while (IsCurveExists(id) || id.Equals("default"))
		{
			id += "_new";
		}
		curves.Add(new CurvesData(id, newCurve));
	}

	public AnimationCurve GetCurve(string id)
	{
		if (IsCurveExists(id))
		{
			return new AnimationCurve(FindCurve(id).curve.keys);
		}
		if (id.Equals("default"))
		{
			return new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
		}
		return null;
	}
}
