using UnityEngine;

public class SpineEventData : PropertyAttribute
{
	public string startsWith = string.Empty;

	public string dataField = string.Empty;

	public SpineEventData(string startsWith = "", string dataField = "")
	{
		this.startsWith = startsWith;
		this.dataField = dataField;
	}
}
