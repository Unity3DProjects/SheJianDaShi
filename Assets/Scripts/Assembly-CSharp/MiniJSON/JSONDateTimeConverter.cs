using System;
using System.Collections.Generic;

namespace MiniJSON
{
	public class JSONDateTimeConverter : JSONCustomConverter
	{
		internal static readonly long EPOX_TICKS = 621355968000000000L;

		public override bool IsCanBeDeserialized(string type)
		{
			return type.Equals("DateTime");
		}

		public override bool IsCanBeDeserialized(Type type)
		{
			return type.Equals(typeof(DateTime));
		}

		public override object Serialize(object obj)
		{
			string value = ((DateTime)obj).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("__type", "DateTime");
			dictionary.Add("iso", value);
			return dictionary;
		}

		public override object Deserialize(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			string s = dictionary["iso"] as string;
			return DateTime.Parse(s);
		}
	}
}
