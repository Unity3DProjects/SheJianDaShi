using UnityEngine;

public struct LayoutCellInfo
{
	public GUILayouterType type;

	public AnchorType anchor;

	public Rect cellRect;

	public LayoutCellInfo(GUILayouterType t, AnchorType a, Rect r)
	{
		type = t;
		anchor = a;
		cellRect = r;
	}
}
