using System;
using UnityEngine;

public class SimpleScrollBar : MonoBehaviour, ILayoutCellHandler
{
	[SerializeField]
	private tk2dUIScrollView scrollView;

	[SerializeField]
	private Transform beginTransform;

	[SerializeField]
	private Transform endTransform;

	[SerializeField]
	private tk2dBaseSprite barSprite;

	[SerializeField]
	private bool isInversed;

	private Transform cachedTransform;

	private Vector3 directionVector = Vector3.right;

	private float scrollLength;

	private float barHalfSize;

	private Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public void RepositionForCell(LayoutCellInfo info)
	{
		directionVector = endTransform.position - beginTransform.position;
		scrollLength = directionVector.magnitude - barSprite.GetBounds().size.x * barSprite.transform.lossyScale.x;
		directionVector.Normalize();
		barHalfSize = barSprite.GetBounds().size.x * 0.5f * barSprite.transform.lossyScale.x;
		ScrollView_OnContentPositionChanged();
	}

	private void OnEnable()
	{
		tk2dUIScrollView obj = scrollView;
		obj.OnContentPositionChange = (Action)Delegate.Combine(obj.OnContentPositionChange, new Action(ScrollView_OnContentPositionChanged));
	}

	private void OnDisable()
	{
		tk2dUIScrollView obj = scrollView;
		obj.OnContentPositionChange = (Action)Delegate.Remove(obj.OnContentPositionChange, new Action(ScrollView_OnContentPositionChanged));
	}

	private void ScrollView_OnContentPositionChanged()
	{
		float num = 0f;
		if (scrollView.scrollDirection == ScrollDirection.Horizontal)
		{
			num = (0f - scrollView.contentContainer.localPosition.x) * (float)((!isInversed) ? 1 : (-1));
		}
		else if (scrollView.scrollDirection == ScrollDirection.Vertical)
		{
			num = (0f - scrollView.contentContainer.localPosition.y) * (float)((!isInversed) ? 1 : (-1));
		}
		float num2 = Mathf.Clamp01(num / scrollView.contentLength);
		CachedTransform.position = beginTransform.position + directionVector * (barHalfSize + num2 * scrollLength);
	}
}
