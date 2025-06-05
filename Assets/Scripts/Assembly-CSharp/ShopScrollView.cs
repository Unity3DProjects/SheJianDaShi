using UnityEngine;

public class ShopScrollView : tk2dUIScrollView
{
	[SerializeField]
	protected Transform snapTransform;

	[SerializeField]
	protected float swipeThresholdDistance;

	public float CellsOffset { get; set; }

	public Transform SnapTransform
	{
		get
		{
			return snapTransform;
		}
		set
		{
			snapTransform = value;
		}
	}

	protected override float SwipeThreshold()
	{
		return swipeThresholdDistance;
	}

	protected override Transform GetChildForSnap(float destination)
	{
		float num = Mathf.Floor(destination / CellsOffset + 0.5f) * CellsOffset;
		switch (scrollDirection)
		{
		case ScrollDirection.Horizontal:
			snapTransform.localPosition = new Vector3(num, snapTransform.localPosition.y);
			break;
		case ScrollDirection.Vertical:
			snapTransform.localPosition = new Vector3(snapTransform.localPosition.x, num);
			break;
		}
		return snapTransform;
	}
}
