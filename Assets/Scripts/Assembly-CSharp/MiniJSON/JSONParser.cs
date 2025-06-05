using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MiniJSON
{
	public class JSONParser : IDisposable
	{
		public enum TOKEN
		{
			NONE = 0,
			CURLY_OPEN = 1,
			CURLY_CLOSE = 2,
			SQUARED_OPEN = 3,
			SQUARED_CLOSE = 4,
			COLON = 5,
			COMMA = 6,
			STRING = 7,
			NUMBER = 8,
			TRUE = 9,
			FALSE = 10,
			NULL = 11
		}

		private const string WORD_BREAK = "{}[],:\"";

		private static Type objectType = typeof(object);

		private static Type dictionaryType = typeof(Dictionary<, >);

		private static Type defaultDictionaryType = typeof(Dictionary<string, object>);

		private StringReader json;

		public char PeekChar
		{
			get
			{
				return Convert.ToChar(json.Peek());
			}
		}

		public char NextChar
		{
			get
			{
				return Convert.ToChar(json.Read());
			}
		}

		public string NextWord
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				while (!IsWordBreak(PeekChar))
				{
					stringBuilder.Append(NextChar);
					if (json.Peek() == -1)
					{
						break;
					}
				}
				return stringBuilder.ToString();
			}
		}

		public TOKEN NextToken
		{
			get
			{
				EatWhitespace();
				if (json.Peek() == -1)
				{
					return TOKEN.NONE;
				}
				switch (PeekChar)
				{
				case '{':
					return TOKEN.CURLY_OPEN;
				case '}':
					json.Read();
					return TOKEN.CURLY_CLOSE;
				case '[':
					return TOKEN.SQUARED_OPEN;
				case ']':
					json.Read();
					return TOKEN.SQUARED_CLOSE;
				case ',':
					json.Read();
					return TOKEN.COMMA;
				case '"':
					return TOKEN.STRING;
				case ':':
					return TOKEN.COLON;
				case '-':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return TOKEN.NUMBER;
				default:
					switch (NextWord)
					{
					case "false":
						return TOKEN.FALSE;
					case "true":
						return TOKEN.TRUE;
					case "null":
						return TOKEN.NULL;
					default:
						return TOKEN.NONE;
					}
				}
			}
		}

		private JSONParser(string jsonString)
		{
			json = new StringReader(jsonString);
		}

		public static bool IsWordBreak(char c)
		{
			return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
		}

		public static object Parse(string jsonString, Type type)
		{
			using (JSONParser jSONParser = new JSONParser(jsonString))
			{
				return jSONParser.ParseValue(type);
			}
		}

		public void Dispose()
		{
			json.Dispose();
			json = null;
		}

		public object ParseObject(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == dictionaryType)
			{
				return ParseDictionary(type);
			}
			if (type.Equals(objectType))
			{
				return ParseDictionary(defaultDictionaryType);
			}
			return ParseClass(type);
		}

		public object ParseDictionary(Type type)
		{
			IDictionary dictionary = Activator.CreateInstance(type) as IDictionary;
			Type type2 = type.GetGenericArguments()[0];
			Type type3 = type.GetGenericArguments()[1];
			json.Read();
			while (true)
			{
				switch (NextToken)
				{
				case TOKEN.COMMA:
					continue;
				case TOKEN.NONE:
					return null;
				case TOKEN.CURLY_CLOSE:
					if (dictionary.Contains("__type"))
					{
						string text = dictionary["__type"] as string;
						if (text != null && Json.JsonConvertersString.ContainsKey(text))
						{
							return Json.JsonConvertersString[text].Deserialize(dictionary);
						}
					}
					return dictionary;
				}
				object obj = ParseValue(type2);
				if (obj == null)
				{
					return null;
				}
				if (NextToken != TOKEN.COLON)
				{
					return null;
				}
				json.Read();
				object value = ParseValue(type3);
				if (obj.GetType() != type2)
				{
					obj = Convert.ChangeType(obj, type2);
				}
				dictionary[obj] = value;
			}
		}

		public object ParseClass(Type type)
		{
			object obj = Activator.CreateInstance(type);
			while (true)
			{
				switch (NextToken)
				{
				case TOKEN.COMMA:
					continue;
				case TOKEN.NONE:
					return null;
				case TOKEN.CURLY_OPEN:
					json.Read();
					continue;
				case TOKEN.CURLY_CLOSE:
					return obj;
				}
				string text = ParseString(typeof(string)) as string;
				if (text == null)
				{
					return null;
				}
				if (NextToken != TOKEN.COLON)
				{
					return null;
				}
				json.Read();
				FieldInfo field = type.GetField(text);
				if (field != null)
				{
					object obj2 = ParseValue(field.FieldType);
					if (obj2 != null && (obj2.GetType() == field.FieldType || typeof(object) == field.FieldType))
					{
						field.SetValue(obj, obj2);
					}
				}
				else
				{
					ParseValue(typeof(object));
				}
			}
		}

		public object ParseArray(Type type)
		{
			Type type2 = ((!type.IsGenericType) ? type.GetElementType() : type.GetGenericArguments()[0]);
			if (type2 == null)
			{
				type2 = objectType;
			}
			Type type3 = typeof(List<>).MakeGenericType(type2);
			IList list = Activator.CreateInstance(type3) as IList;
			json.Read();
			bool flag = true;
			while (flag)
			{
				TOKEN nextToken = NextToken;
				switch (nextToken)
				{
				case TOKEN.NONE:
					return null;
				case TOKEN.SQUARED_CLOSE:
					flag = false;
					break;
				default:
				{
					object value = ParseByToken(nextToken, type2);
					list.Add(value);
					break;
				}
				case TOKEN.COMMA:
					break;
				}
			}
			if (!type.IsGenericType)
			{
				Array array = Array.CreateInstance(type2, list.Count);
				int count = list.Count;
				while (count-- > 0)
				{
					array.SetValue(list[count], count);
				}
				return array;
			}
			return list;
		}

		public object ParseValue(Type type)
		{
			TOKEN nextToken = NextToken;
			if (Json.JsonConverters.ContainsKey(type))
			{
				return ParseDictionary(defaultDictionaryType);
			}
			return ParseByToken(nextToken, type);
		}

		public object ParseByToken(TOKEN token, Type type)
		{
			switch (token)
			{
			case TOKEN.STRING:
				return ParseString(type);
			case TOKEN.NUMBER:
				return ParseNumber(type);
			case TOKEN.CURLY_OPEN:
				return ParseObject(type);
			case TOKEN.SQUARED_OPEN:
				return ParseArray(type);
			case TOKEN.TRUE:
				return true;
			case TOKEN.FALSE:
				return false;
			case TOKEN.NULL:
				return null;
			default:
				return null;
			}
		}

		public object ParseString(Type type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			json.Read();
			bool flag = true;
			while (flag)
			{
				if (json.Peek() == -1)
				{
					flag = false;
					break;
				}
				char nextChar = NextChar;
				switch (nextChar)
				{
				case '"':
					flag = false;
					break;
				case '\\':
					if (json.Peek() == -1)
					{
						flag = false;
						break;
					}
					nextChar = NextChar;
					switch (nextChar)
					{
					case '"':
					case '/':
					case '\\':
						stringBuilder.Append(nextChar);
						break;
					case 'b':
						stringBuilder.Append('\b');
						break;
					case 'f':
						stringBuilder.Append('\f');
						break;
					case 'n':
						stringBuilder.Append('\n');
						break;
					case 'r':
						stringBuilder.Append('\r');
						break;
					case 't':
						stringBuilder.Append('\t');
						break;
					case 'u':
					{
						char[] array = new char[4];
						for (int i = 0; i < 4; i++)
						{
							array[i] = NextChar;
						}
						stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
						break;
					}
					}
					break;
				default:
					stringBuilder.Append(nextChar);
					break;
				}
			}
			string text = stringBuilder.ToString();
			if (type.IsEnum)
			{
				return Enum.Parse(type, text);
			}
			if (type == typeof(Guid))
			{
				return new Guid(text);
			}
			return text;
		}

		public object ParseNumber(Type type)
		{
			string nextWord = NextWord;
			if (type.IsEnum)
			{
				int result;
				int.TryParse(nextWord, out result);
				return Enum.ToObject(type, result);
			}
			if (type == typeof(int))
			{
				int result2;
				int.TryParse(nextWord, out result2);
				return result2;
			}
			if (type == typeof(long))
			{
				long result3;
				long.TryParse(nextWord, out result3);
				return result3;
			}
			if (type == typeof(float))
			{
				float result4;
				float.TryParse(nextWord, out result4);
				return result4;
			}
			if (type == typeof(double))
			{
				double result5;
				double.TryParse(nextWord, out result5);
				return result5;
			}
			if (nextWord.IndexOf('.') == -1)
			{
				int result6;
				int.TryParse(nextWord, out result6);
				return result6;
			}
			float result7;
			float.TryParse(nextWord, out result7);
			return result7;
		}

		public void EatWhitespace()
		{
			while (char.IsWhiteSpace(PeekChar))
			{
				json.Read();
				if (json.Peek() == -1)
				{
					break;
				}
			}
		}
	}
}
