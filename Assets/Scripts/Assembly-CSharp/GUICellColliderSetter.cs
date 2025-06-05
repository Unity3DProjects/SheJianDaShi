using UnityEngine;

public class GUICellColliderSetter : MonoBehaviour, ILayoutCellHandler
{
	[SerializeField]
	private GUILayoutCell rePositionPlace;

	[SerializeField]
	private BoxCollider settedCollider;

	[SerializeField]
	private float sizeMultiplier = 1f;

	private BoxCollider toReposition;

	private BoxCollider ToReposition
	{
		get
		{
			if (toReposition == null)
			{
				if (settedCollider != null)
				{
					toReposition = settedCollider;
				}
				else
				{
					toReposition = GetComponent<BoxCollider>();
				}
			}
			return toReposition;
		}
	}

	public void RepositionForCell(LayoutCellInfo info)
	{
		if ((bool)rePositionPlace)
		{
			float y = (rePositionPlace.transform.position.y - base.transform.position.y) / (Vector3.one / ((float)ScreenDimentions.Height / (float)Screen.height)).y;
			float x = (rePositionPlace.transform.position.x - base.transform.position.x) / (Vector3.one / ((float)ScreenDimentions.Width / (float)Screen.width)).x;
			Vector3 center = new Vector3(x, y, 0f);
			ToReposition.center = center;
			ToReposition.size = new Vector3(rePositionPlace.RecievedRect.size.x * sizeMultiplier, rePositionPlace.RecievedRect.size.y * sizeMultiplier, 1f);
		}
		else
		{
			ToReposition.size = new Vector3(info.cellRect.size.x * sizeMultiplier, info.cellRect.size.y * sizeMultiplier, 1f);
		}
	}
}
