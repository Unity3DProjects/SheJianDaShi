using UnityEngine;

public class SpineSlot : PropertyAttribute
{
	public string startsWith = string.Empty;

	public string dataField = string.Empty;

	public bool containsBoundingBoxes;

	public SpineSlot(string startsWith = "", string dataField = "", bool containsBoundingBoxes = false)
	{
		this.startsWith = startsWith;
		this.dataField = dataField;
		this.containsBoundingBoxes = containsBoundingBoxes;
	}
}
