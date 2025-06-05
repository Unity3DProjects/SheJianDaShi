using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class SortingLayerRenderer : MonoBehaviour
{
	[SerializeField]
	[SortingLayer("SortingLayer")]
	private int sortingLayerID;

	[SerializeField]
	[PropertyValue("SortingOrder")]
	private int sortingOrder;

	private Renderer cachedRenderer;

	public int SortingLayer
	{
		get
		{
			return sortingLayerID;
		}
		set
		{
			sortingLayerID = value;
			if ((bool)CachedRenderer)
			{
				CachedRenderer.sortingLayerID = sortingLayerID;
			}
		}
	}

	public int SortingOrder
	{
		get
		{
			return sortingOrder;
		}
		set
		{
			sortingOrder = value;
			if ((bool)CachedRenderer)
			{
				CachedRenderer.sortingOrder = sortingOrder;
			}
		}
	}

	private Renderer CachedRenderer
	{
		get
		{
			if (cachedRenderer == null)
			{
				cachedRenderer = GetComponent<Renderer>();
			}
			return cachedRenderer;
		}
	}

	private void Awake()
	{
		SortingLayer = sortingLayerID;
		SortingOrder = sortingOrder;
	}
}
