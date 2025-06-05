using UnityEngine;

public class PropertyValueAttribute : PropertyAttribute
{
	protected string propertyName;

	public string PropertyName
	{
		get
		{
			return propertyName;
		}
	}

	public PropertyValueAttribute(string propertySetter = null)
	{
		propertyName = propertySetter;
	}
}
