using UnityEngine;

public class BodyPartsHelper : MonoBehaviour
{
	public enum BodyPartsType
	{
		BodyPartsTypeNone = 0,
		BodyPartsTypeHead = 1,
		BodyPartsTypeBody = 2,
		BodyPartsTypeLegs = 3
	}

	public static BodyPartsType TypeByName(string name)
	{
		BodyPartsType result = BodyPartsType.BodyPartsTypeNone;
		if (!string.IsNullOrEmpty(name))
		{
			if (name.Contains("head"))
			{
				result = BodyPartsType.BodyPartsTypeHead;
			}
			else if (name.Contains("body"))
			{
				result = BodyPartsType.BodyPartsTypeBody;
			}
			else if (name.Contains("thigh") || name.Contains("shin"))
			{
				result = BodyPartsType.BodyPartsTypeLegs;
			}
		}
		return result;
	}
}
