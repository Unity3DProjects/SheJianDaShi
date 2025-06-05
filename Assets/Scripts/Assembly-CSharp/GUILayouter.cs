using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILayouter : MonoBehaviour, ILayoutCellHandler
{
	private bool isDirty;

	[HideInInspector]
	[SerializeField]
	private GUILayouterRotationType rotationType;

	[SerializeField]
	private GUILayouterType type;

	private GUILayoutCell[] cells;

	[SerializeField]
	private bool isRootLayouter = true;

	[SerializeField]
	private bool isInversed;

	private Rect? occupiedPixels;

	private Transform cachedTransform;

	public static float DEFAULT_OFFSET
	{
		get
		{
			int num = ((!tk2dSystem.IsRetina) ? 1 : 2);
			return 20 * num;
		}
	}

	public GUILayouterRotationType RotationType
	{
		get
		{
			return rotationType;
		}
		set
		{
			if (!Application.isPlaying)
			{
				rotationType = value;
			}
		}
	}

	public GUILayouterType Type
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

	public bool IsRootLayouter
	{
		get
		{
			return isRootLayouter;
		}
		set
		{
			isRootLayouter = value;
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

	public Rect? OccupiedPixels
	{
		get
		{
			return occupiedPixels;
		}
		set
		{
			occupiedPixels = value;
			if (cells == null || cells.Length == 0)
			{
				cells = GetComponentsInChildren<GUILayoutCell>(true);
				List<GUILayoutCell> list = new List<GUILayoutCell>();
				GUILayoutCell[] array = cells;
				foreach (GUILayoutCell gUILayoutCell in array)
				{
					if (gUILayoutCell.transform.parent == CachedTransform)
					{
						list.Add(gUILayoutCell);
					}
				}
				cells = list.ToArray();
			}
			UpdateLayout();
		}
	}

	protected virtual void Awake()
	{
		if (IsRootLayouter)
		{
			GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
		}
		ResetLayouter();
	}

	protected virtual void Start()
	{
		if (rotationType != GUILayouterRotationType.Both && IsRootLayouter)
		{
			base.gameObject.SetActive(rotationType == SingletonMonoBehaviour<GUIOrientationManger>.Instance.CurrentRotationType);
		}
		Initialize();
	}

	protected virtual void OnDestroy()
	{
		if (IsRootLayouter)
		{
			GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
		}
	}

	protected virtual void OnEnable()
	{
		if (isDirty)
		{
			StartCoroutine(CoroutineInitialize());
			isDirty = false;
		}
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType rt)
	{
		if (this.IsNull())
		{
			GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
			return;
		}
		if (rotationType != GUILayouterRotationType.Both)
		{
			bool active = rotationType == rt;
			base.gameObject.SetActive(active);
		}
		ResetLayouter();
		if (base.isActiveAndEnabled)
		{
			StartCoroutine(CoroutineInitialize());
		}
		else
		{
			isDirty = true;
		}
	}

	private IEnumerator CoroutineInitialize()
	{
		yield return new WaitForEndOfFrame();
		Initialize();
	}

	public void RepositionForCell(LayoutCellInfo info)
	{
		if (info.type == GUILayouterType.Horizontal)
		{
			OccupiedPixels = info.cellRect;
		}
		else if (info.type == GUILayouterType.Vertical)
		{
			OccupiedPixels = info.cellRect;
		}
		if (info.anchor == AnchorType.Left)
		{
			CachedTransform.localPosition = new Vector3((0f - info.cellRect.width) * 0.5f, 0f, CachedTransform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Right)
		{
			CachedTransform.localPosition = new Vector3(info.cellRect.width * 0.5f, 0f, CachedTransform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Top)
		{
			CachedTransform.localPosition = new Vector3(0f, info.cellRect.height * 0.5f, CachedTransform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Bottom)
		{
			CachedTransform.localPosition = new Vector3(0f, (0f - info.cellRect.height) * 0.5f, CachedTransform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Center)
		{
			CachedTransform.localPosition = new Vector3(0f, 0f, CachedTransform.localPosition.z);
		}
	}

	public void ResetLayouter()
	{
		occupiedPixels = null;
		cells = null;
	}

	public void Initialize()
	{
		Rect? rect = occupiedPixels;
		if (!rect.HasValue)
		{
			int width = ScreenDimentions.Width;
			int height = ScreenDimentions.Height;
			int num = width;
			int num2 = height;
			bool flag = ((rotationType != GUILayouterRotationType.Both) ? (rotationType == GUILayouterRotationType.Landscape) : (num > num2));
			num = ((!flag) ? Mathf.Min(width, height) : Mathf.Max(width, height));
			num2 = ((!flag) ? Mathf.Max(width, height) : Mathf.Min(width, height));
			OccupiedPixels = new Rect(0f, 0f, num, num2);
		}
	}

	public void UpdateLayout()
	{
		if (cells == null || cells.Length == 0 || cells[0] == null)
		{
			return;
		}
		int num = 0;
		float num2 = 0f;
		bool flag = false;
		for (int i = 0; i < cells.Length; i++)
		{
			GUILayoutCell gUILayoutCell = cells[i];
			if (gUILayoutCell.Type == GUILayoutCellType.FixedSize)
			{
				num += (int)gUILayoutCell.SizeValue;
			}
			else if (gUILayoutCell.Type == GUILayoutCellType.RelativeFixedSize)
			{
				num += (int)(gUILayoutCell.SizeValue * ((type != GUILayouterType.Horizontal) ? occupiedPixels.GetValueOrDefault().height : occupiedPixels.GetValueOrDefault().width));
			}
			else if (gUILayoutCell.Type == GUILayoutCellType.Flexible)
			{
				flag = true;
				num2 += gUILayoutCell.SizeValue;
			}
		}
		int num3 = (int)((type != GUILayouterType.Horizontal) ? occupiedPixels.GetValueOrDefault().height : occupiedPixels.GetValueOrDefault().width);
		int num4 = num3 - num;
		if (num4 < 0)
		{
		}
		if (num4 > 0 && num2 < float.Epsilon && flag)
		{
			CustomDebug.LogError(base.gameObject.name + ": Weights for flexible cells is 0!");
		}
		int num5 = (isInversed ? num3 : 0);
		for (int j = 0; j < cells.Length; j++)
		{
			GUILayoutCell gUILayoutCell2 = cells[j];
			Rect rect = default(Rect);
			if (type == GUILayouterType.Horizontal)
			{
				if (gUILayoutCell2.Type == GUILayoutCellType.FixedSize)
				{
					rect = new Rect(0f, 0f, gUILayoutCell2.SizeValue, occupiedPixels.GetValueOrDefault().height);
				}
				else if (gUILayoutCell2.Type == GUILayoutCellType.RelativeFixedSize)
				{
					rect = new Rect(0f, 0f, gUILayoutCell2.SizeValue * (float)((type != GUILayouterType.Horizontal) ? ScreenDimentions.Height : ScreenDimentions.Width), occupiedPixels.GetValueOrDefault().height);
				}
				else if (gUILayoutCell2.Type == GUILayoutCellType.Flexible)
				{
					rect = new Rect(0f, 0f, gUILayoutCell2.SizeValue * (float)num4 / num2, occupiedPixels.GetValueOrDefault().height);
				}
				if (isInversed)
				{
					rect.center = new Vector3((float)num5 - rect.width * 0.5f, 0f, 0f);
					num5 -= (int)rect.width;
				}
				else
				{
					rect.center = new Vector3((float)num5 + rect.width * 0.5f, 0f, 0f);
					num5 += (int)rect.width;
				}
			}
			else if (type == GUILayouterType.Vertical)
			{
				if (gUILayoutCell2.Type == GUILayoutCellType.FixedSize)
				{
					rect = new Rect(0f, gUILayoutCell2.SizeValue * 0.5f, occupiedPixels.GetValueOrDefault().width, gUILayoutCell2.SizeValue);
				}
				else if (gUILayoutCell2.Type == GUILayoutCellType.RelativeFixedSize)
				{
					rect = new Rect(0f, 0f, occupiedPixels.GetValueOrDefault().width, gUILayoutCell2.SizeValue * (float)((type != GUILayouterType.Horizontal) ? ScreenDimentions.Height : ScreenDimentions.Width));
				}
				else if (gUILayoutCell2.Type == GUILayoutCellType.Flexible)
				{
					rect = new Rect(0f, 0f, occupiedPixels.GetValueOrDefault().width, gUILayoutCell2.SizeValue * (float)num4 / num2);
				}
				if (isInversed)
				{
					rect.center = new Vector3(0f, (float)num5 - rect.height * 0.5f, 0f);
					num5 -= (int)rect.height;
				}
				else
				{
					rect.center = new Vector3(0f, (float)num5 + rect.height * 0.5f, 0f);
					num5 += (int)rect.height;
				}
			}
			gUILayoutCell2.Reposition(type, rect);
		}
	}
}
