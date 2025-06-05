using UnityEngine;

public class LabelWithShadow : MonoBehaviour
{
	[SerializeField]
	private tk2dTextMesh label;

	[SerializeField]
	private tk2dTextMesh shadow;

	public string Text
	{
		get
		{
			return label.text;
		}
		set
		{
			tk2dTextMesh obj = label;
			shadow.text = value;
			obj.text = value;
		}
	}
}
