using System;
using System.Collections.Generic;
using System.Reflection;
using MiniJSON;
using UnityEngine;

namespace Serialization
{
	public static class SerializeUtility
	{
		public static FieldInfo GetDeepField(Type type, string name, BindingFlags flags)
		{
			FieldInfo fieldInfo = null;
			while (fieldInfo == null && type != null)
			{
				fieldInfo = type.GetField(name, flags);
				type = type.BaseType;
			}
			return fieldInfo;
		}

		public static FieldInfo[] GetDeepFields(Type type, BindingFlags flags)
		{
			List<FieldInfo> list = new List<FieldInfo>();
			while (type != null)
			{
				FieldInfo[] fields = type.GetFields(flags);
				FieldInfo[] array = fields;
				foreach (FieldInfo fieldInfo in array)
				{
					if (!fieldInfo.IsNotSerialized)
					{
						list.Add(fieldInfo);
					}
				}
				type = type.BaseType;
			}
			return list.ToArray();
		}

		public static Dictionary<string, object> FindFields(UnityEngine.Object unityObject, Type attribute = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			FieldInfo[] deepFields = GetDeepFields(unityObject.GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo[] array = deepFields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (fieldInfo != null && attribute != null && fieldInfo.IsDefined(attribute, true))
				{
					object value = fieldInfo.GetValue(unityObject);
					dictionary.Add(fieldInfo.Name, value);
				}
			}
			return dictionary;
		}

		public static List<SerializedField> DiffFields(UnityEngine.Object original, UnityEngine.Object instance, Type attribute = null)
		{
			List<SerializedField> list = new List<SerializedField>();
			Dictionary<string, object> dictionary = FindFields(instance, attribute);
			Dictionary<string, object> dictionary2 = FindFields(original, attribute);
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				if (dictionary2.ContainsKey(item.Key))
				{
					string text = JsonConvert.SerializeObject(item.Value);
					string text2 = JsonConvert.SerializeObject(dictionary2[item.Key]);
					if (text != text2)
					{
						SerializedField serializedField = new SerializedField();
						serializedField.field = item.Key;
						serializedField.jsonValue = Json.Serialize(item.Value);
						list.Add(serializedField);
						CustomDebug.Log(item.Key + " : " + item.Value);
					}
				}
			}
			return list;
		}
	}
}
