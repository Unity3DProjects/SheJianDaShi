using UnityEngine;

public class ColliderResizer : MonoBehaviour, ILayoutCellHandler
{
	[SerializeField]
	private float yCoof = 1f;

	[SerializeField]
	private float xCoof = 1f;

	[SerializeField]
	private BoxCollider settedCollider;

	private BoxCollider buttonCollider;

	private BoxCollider ButtonCollider
	{
		get
		{
			if (buttonCollider == null)
			{
				if (settedCollider != null)
				{
					buttonCollider = settedCollider;
				}
				else
				{
					buttonCollider = GetComponent<BoxCollider>();
				}
			}
			return buttonCollider;
		}
	}

	public void RepositionForCell(LayoutCellInfo info)
	{
		if (ButtonCollider == null)
		{
			CustomDebug.LogError("Collider not set !");
		}
		else
		{
			ButtonCollider.size = new Vector3(info.cellRect.width * xCoof, info.cellRect.height * yCoof, 1f);
		}
	}
}
