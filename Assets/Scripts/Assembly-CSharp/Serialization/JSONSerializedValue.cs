using System;
using UnityEngine;

namespace Serialization
{
	[Serializable]
	public class JSONSerializedValue : ISerializationCallbackReceiver
	{
		[SerializeField]
		private string json;

		[SerializeField]
		private string type;

		private object _value;

		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				type = _value.GetType().ToString();
				json = JsonConvert.SerializeObject(Value);
			}
		}

		public void OnBeforeSerialize()
		{
			if (Value != null)
			{
				json = JsonConvert.SerializeObject(Value);
			}
		}

		public void OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(json))
			{
				Type type = ((!string.IsNullOrEmpty(this.type)) ? Type.GetType(this.type) : typeof(object));
				Value = JsonConvert.DeserializeObject(json, type);
			}
		}
	}
}
