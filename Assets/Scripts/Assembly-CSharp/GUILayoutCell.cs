using System.Collections.Generic;
using UnityEngine;

public class GUILayoutCell : MonoBehaviour
{
	[SerializeField]
	private GUILayoutCellType type;

	[SerializeField]
	private List<GameObject> layoutHandlerObjects = new List<GameObject>();

	[SerializeField]
	protected float sizeValue;

	[SerializeField]
	private AnchorType cellContentAnchor = AnchorType.Center;

	[SerializeField]
	protected Rect recievedRect;

	protected GUILayouterType recievedType;

	private List<ILayoutCellHandler> layoutHandlers = new List<ILayoutCellHandler>();

	private string cachedName;

	private Transform cachedTransform;

	public List<GameObject> LayoutHandlerObjects
	{
		get
		{
			return layoutHandlerObjects;
		}
	}

	public Transform CachedTransform
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

	public GUILayoutCellType Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public Rect RecievedRect
	{
		get
		{
			return recievedRect;
		}
	}

	public GUILayouterType RecievedType
	{
		get
		{
			return recievedType;
		}
	}

	public float SizeValue
	{
		get
		{
			return sizeValue;
		}
		set
		{
			sizeValue = value;
			if (type == GUILayoutCellType.FixedSize && tk2dSystem.IsRetina)
			{
				sizeValue *= 2f;
			}
		}
	}

	public float FixedSizeValue
	{
		set
		{
			sizeValue = value;
		}
	}

	public AnchorType CellContentAnchor
	{
		get
		{
			return cellContentAnchor;
		}
	}

	public string CachedName
	{
		get
		{
			if (cachedName == null)
			{
				cachedName = base.gameObject.name;
			}
			return cachedName;
		}
	}

	private void InitHandlerObjects()
	{
		foreach (GameObject layoutHandlerObject in layoutHandlerObjects)
		{
			if (layoutHandlerObject != null)
			{
				ILayoutCellHandler component = layoutHandlerObject.GetComponent<ILayoutCellHandler>();
				if (component != null)
				{
					if (!layoutHandlers.Contains(component))
					{
						layoutHandlers.Add(component);
					}
				}
				else
				{
					base.gameObject.name = CachedName + "(NOT_FOUND_HANDLERS)";
					CustomDebug.LogWarning("no handlers found in GUILayoutCell references, gameObject name = " + CachedName, this);
				}
			}
			else
			{
				base.gameObject.name = CachedName + "(NOT_FOUND_HANDLERS)";
				CustomDebug.LogWarning("no handlers found in GUILayoutCell references, gameObject name = " + CachedName, this);
			}
		}
	}

	protected virtual void Awake()
	{
		InitHandlerObjects();
		if (type == GUILayoutCellType.FixedSize && tk2dSystem.IsRetina)
		{
			sizeValue *= 2f;
		}
	}

	public void Reposition(GUILayouterType t, Rect rect)
	{
		recievedRect = rect;
		recievedType = t;
		if (recievedType == GUILayouterType.Horizontal)
		{
			CachedTransform.localPosition = new Vector3(recievedRect.center.x, CachedTransform.localPosition.y, CachedTransform.localPosition.z);
		}
		else if (recievedType == GUILayouterType.Vertical)
		{
			CachedTransform.localPosition = new Vector3(CachedTransform.localPosition.x, recievedRect.center.y, CachedTransform.localPosition.z);
		}
		if (layoutHandlers.Count == 0 && layoutHandlerObjects.Count > 0)
		{
			InitHandlerObjects();
		}
		foreach (ILayoutCellHandler layoutHandler in layoutHandlers)
		{
			if (layoutHandler != null)
			{
				layoutHandler.RepositionForCell(new LayoutCellInfo(recievedType, cellContentAnchor, recievedRect));
			}
		}
	}
}
