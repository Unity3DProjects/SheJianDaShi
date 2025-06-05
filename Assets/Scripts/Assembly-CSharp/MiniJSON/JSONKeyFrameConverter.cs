using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJSON
{
	public class JSONKeyFrameConverter : JSONCustomConverter
	{
		public override bool IsCanBeDeserialized(string type)
		{
			return type.Equals("Keyframe");
		}

		public override bool IsCanBeDeserialized(Type type)
		{
			return type.Equals(typeof(Keyframe));
		}

		public override object Serialize(object obj)
		{
			Keyframe keyframe = (Keyframe)obj;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("inTangent", keyframe.inTangent);
			dictionary.Add("outTangent", keyframe.outTangent);
			dictionary.Add("tangentMode", keyframe.tangentMode);
			dictionary.Add("time", keyframe.time);
			dictionary.Add("value", keyframe.value);
			return dictionary;
		}

		public override object Deserialize(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			return new Keyframe
			{
				inTangent = Convert.ToSingle(dictionary["inTangent"]),
				outTangent = Convert.ToSingle(dictionary["outTangent"]),
				tangentMode = Convert.ToInt32(dictionary["tangentMode"]),
				time = Convert.ToSingle(dictionary["time"]),
				value = Convert.ToSingle(dictionary["value"])
			};
		}
	}
}
