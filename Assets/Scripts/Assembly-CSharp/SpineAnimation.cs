using UnityEngine;

public class SpineAnimation : PropertyAttribute
{
	public string startsWith = string.Empty;

	public string dataField = string.Empty;

	public SpineAnimation(string startsWith = "", string dataField = "")
	{
		this.startsWith = startsWith;
		this.dataField = dataField;
	}
}
