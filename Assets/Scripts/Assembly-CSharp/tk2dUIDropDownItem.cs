using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIDropDownItem")]
public class tk2dUIDropDownItem : tk2dUIBaseItemControl
{
	public tk2dTextMesh label;

	public float height;

	public tk2dUIUpDownHoverButton upDownHoverBtn;

	private int index;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIDropDownItem> m_OnItemSelected;

	public int Index
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
		}
	}

	public string LabelText
	{
		get
		{
			return label.text;
		}
		set
		{
			label.text = value;
		}
	}

	public event Action<tk2dUIDropDownItem> OnItemSelected
	{
		add
		{
			Action<tk2dUIDropDownItem> action = this.m_OnItemSelected;
			Action<tk2dUIDropDownItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnItemSelected, (Action<tk2dUIDropDownItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIDropDownItem> action = this.m_OnItemSelected;
			Action<tk2dUIDropDownItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnItemSelected, (Action<tk2dUIDropDownItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void OnEnable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnClick += ItemSelected;
		}
	}

	private void OnDisable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnClick -= ItemSelected;
		}
	}

	private void ItemSelected()
	{
		if (this.OnItemSelected != null)
		{
			this.OnItemSelected(this);
		}
	}
}
