using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJSON
{
	public static class Json
	{
		public static Dictionary<Type, JSONCustomConverter> JsonConverters = new Dictionary<Type, JSONCustomConverter>
		{
			{
				typeof(DateTime),
				new JSONDateTimeConverter()
			},
			{
				typeof(AnimationCurve),
				new JSONAnimationCurveConverter()
			},
			{
				typeof(Keyframe),
				new JSONKeyFrameConverter()
			}
		};

		public static Dictionary<string, JSONCustomConverter> JsonConvertersString = new Dictionary<string, JSONCustomConverter>
		{
			{
				"DateTime",
				new JSONDateTimeConverter()
			},
			{
				"AnimationCurve",
				new JSONAnimationCurveConverter()
			},
			{
				"Keyframe",
				new JSONKeyFrameConverter()
			}
		};

		public static T Deserialize<T>(string json)
		{
			return (T)Deserialize(json, typeof(T));
		}

		public static object Deserialize(string json, Type type)
		{
			if (json == null)
			{
				return null;
			}
			return JSONParser.Parse(json, type);
		}

		public static string Serialize(object obj)
		{
			return JSONSerializer.Serialize(obj);
		}

		public static void SerializeTest<T>(T obj)
		{
			string text = Serialize(obj);
			CustomDebug.Log(text);
			T val = Deserialize<T>(text);
			CustomDebug.Log(val);
			CustomDebug.Log(val.GetType());
			string message = Serialize(val);
			CustomDebug.Log(message);
		}
	}
}
