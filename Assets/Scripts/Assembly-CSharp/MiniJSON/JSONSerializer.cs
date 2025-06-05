using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MiniJSON
{
	internal sealed class JSONSerializer
	{
		private StringBuilder builder;

		public static bool SerializeEnumAsString { get; set; }

		private JSONSerializer()
		{
			builder = new StringBuilder();
		}

		public static string Serialize(object obj)
		{
			JSONSerializer jSONSerializer = new JSONSerializer();
			jSONSerializer.SerializeValue(obj);
			SerializeEnumAsString = false;
			return jSONSerializer.builder.ToString();
		}

		private void SerializeValue(object value)
		{
			if (value != null)
			{
				Type type = value.GetType();
				if (Json.JsonConverters.ContainsKey(type))
				{
					object value2 = Json.JsonConverters[type].Serialize(value);
					SerializeValue(value2);
					return;
				}
			}
			string str;
			IList anArray;
			IDictionary obj;
			if (value == null)
			{
				builder.Append("null");
			}
			else if ((str = value as string) != null)
			{
				SerializeString(str);
			}
			else if (value is bool)
			{
				builder.Append((!(bool)value) ? "false" : "true");
			}
			else if ((anArray = value as IList) != null)
			{
				SerializeArray(anArray);
			}
			else if ((obj = value as IDictionary) != null)
			{
				SerializeDictionary(obj);
			}
			else if (value is char)
			{
				SerializeString(new string((char)value, 1));
			}
			else
			{
				SerializeOther(value);
			}
		}

		private void SerializeDictionary(IDictionary obj)
		{
			bool flag = true;
			builder.Append('{');
			foreach (object key in obj.Keys)
			{
				if (!flag)
				{
					builder.Append(',');
				}
				SerializeValue(key);
				builder.Append(':');
				SerializeValue(obj[key]);
				flag = false;
			}
			builder.Append('}');
		}

		private void SerializeArray(IList anArray)
		{
			builder.Append('[');
			bool flag = true;
			foreach (object item in anArray)
			{
				if (!flag)
				{
					builder.Append(',');
				}
				SerializeValue(item);
				flag = false;
			}
			builder.Append(']');
		}

		private void SerializeString(string str)
		{
			builder.Append('"');
			char[] array = str.ToCharArray();
			char[] array2 = array;
			foreach (char c in array2)
			{
				switch (c)
				{
				case '"':
					builder.Append("\\\"");
					continue;
				case '\\':
					builder.Append("\\\\");
					continue;
				case '\b':
					builder.Append("\\b");
					continue;
				case '\f':
					builder.Append("\\f");
					continue;
				case '\n':
					builder.Append("\\n");
					continue;
				case '\r':
					builder.Append("\\r");
					continue;
				case '\t':
					builder.Append("\\t");
					continue;
				}
				int num = Convert.ToInt32(c);
				if (num >= 32 && num <= 126)
				{
					builder.Append(c);
					continue;
				}
				builder.Append("\\u");
				builder.Append(num.ToString("x4"));
			}
			builder.Append('"');
		}

		private void SerializeOther(object value)
		{
			if (value is float)
			{
				builder.Append(((float)value).ToString("R"));
			}
			else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
			{
				builder.Append(value);
			}
			else if (value is double || value is decimal)
			{
				builder.Append(Convert.ToDouble(value).ToString("R"));
			}
			else if (value is Enum)
			{
				if (SerializeEnumAsString)
				{
					SerializeString(value.ToString());
				}
				else
				{
					builder.Append((int)value);
				}
			}
			else if (value is Guid)
			{
				SerializeString(value.ToString());
			}
			else if (value.GetType().IsClass || value.GetType().IsValueType)
			{
				SerializeClass(value);
			}
			else
			{
				SerializeString(value.ToString());
			}
		}

		private void SerializeClass(object value)
		{
			Type type = value.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (fieldInfo.GetValue(value) != null)
				{
					dictionary.Add(fieldInfo.Name, fieldInfo.GetValue(value));
				}
			}
			SerializeDictionary((IDictionary)dictionary);
		}
	}
}
