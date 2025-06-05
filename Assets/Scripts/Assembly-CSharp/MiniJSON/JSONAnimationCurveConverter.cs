using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJSON
{
	public class JSONAnimationCurveConverter : JSONCustomConverter
	{
		public override bool IsCanBeDeserialized(string type)
		{
			return type.Equals("AnimationCurve");
		}

		public override bool IsCanBeDeserialized(Type type)
		{
			return type.Equals(typeof(AnimationCurve));
		}

		public override object Serialize(object obj)
		{
			AnimationCurve animationCurve = (AnimationCurve)obj;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("__type", "AnimationCurve");
			dictionary.Add("keys", animationCurve.keys);
			return dictionary;
		}

		public override object Deserialize(object obj)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			object[] array = dictionary["keys"] as object[];
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				Keyframe key = (Keyframe)Json.JsonConverters[typeof(Keyframe)].Deserialize(array[i]);
				animationCurve.AddKey(key);
			}
			return animationCurve;
		}
	}
}
