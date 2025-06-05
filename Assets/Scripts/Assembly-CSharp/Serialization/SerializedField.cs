using System;
using System.Reflection;
using MiniJSON;

namespace Serialization
{
	[Serializable]
	public class SerializedField
	{
		public string field;

		public string jsonValue;

		public void ApplyValue(object obj)
		{
			if (obj == null)
			{
				return;
			}
			FieldInfo deepField = SerializeUtility.GetDeepField(obj.GetType(), field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (deepField != null)
			{
				Type fieldType = deepField.FieldType;
				object obj2 = Json.Deserialize(jsonValue, deepField.FieldType);
				if (fieldType.IsInstanceOfType(obj2))
				{
					deepField.SetValue(obj, obj2);
				}
			}
		}
	}
}
