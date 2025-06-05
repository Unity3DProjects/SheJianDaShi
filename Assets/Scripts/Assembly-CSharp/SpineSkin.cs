using UnityEngine;

public class SpineSkin : PropertyAttribute
{
	public string startsWith = string.Empty;

	public string dataField = string.Empty;

	public SpineSkin(string startsWith = "", string dataField = "")
	{
		this.startsWith = startsWith;
		this.dataField = dataField;
	}
}
