using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIHoverItem")]
public class tk2dUIHoverItem : tk2dUIBaseItemControl
{
	public GameObject outStateGO;

	public GameObject overStateGO;

	private bool isOver;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIHoverItem> m_OnToggleHover;

	public string SendMessageOnToggleHoverMethodName = string.Empty;

	public bool IsOver
	{
		get
		{
			return isOver;
		}
		set
		{
			if (isOver != value)
			{
				isOver = value;
				SetState();
				this.m_OnToggleHover?.Invoke(this);
				DoSendMessage(SendMessageOnToggleHoverMethodName, this);
			}
		}
	}

	public event Action<tk2dUIHoverItem> OnToggleHover
	{
		add
		{
			Action<tk2dUIHoverItem> action = this.m_OnToggleHover;
			Action<tk2dUIHoverItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnToggleHover, (Action<tk2dUIHoverItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIHoverItem> action = this.m_OnToggleHover;
			Action<tk2dUIHoverItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnToggleHover, (Action<tk2dUIHoverItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		SetState();
	}

	private void OnEnable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnHoverOver += HoverOver;
			uiItem.OnHoverOut += HoverOut;
		}
	}

	private void OnDisable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnHoverOver -= HoverOver;
			uiItem.OnHoverOut -= HoverOut;
		}
	}

	private void HoverOver()
	{
		IsOver = true;
	}

	private void HoverOut()
	{
		IsOver = false;
	}

	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(overStateGO, isOver);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(outStateGO, !isOver);
	}
}
