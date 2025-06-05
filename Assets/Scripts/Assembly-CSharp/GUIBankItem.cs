using UnityEngine;

public class GUIBankItem : MonoBehaviour
{
	[SerializeField]
	private string itemDescription;

	[SerializeField]
	private string iconSpriteName;

	[SerializeField]
	private float scaleFactor;

	public string ItemDescription
	{
		get
		{
			return itemDescription;
		}
	}

	public string IconSpriteName
	{
		get
		{
			return iconSpriteName;
		}
	}

	public float ScaleFactor
	{
		get
		{
			return scaleFactor;
		}
	}
}
