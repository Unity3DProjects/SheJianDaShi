using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownHoverButton")]
public class tk2dUIUpDownHoverButton : tk2dUIBaseItemControl
{
	public GameObject upStateGO;

	public GameObject downStateGO;

	public GameObject hoverOverStateGO;

	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	private bool isDown;

	private bool isHover;

	public string SendMessageOnToggleOverMethodName = string.Empty;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIUpDownHoverButton> m_OnToggleOver;

	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return useOnReleaseInsteadOfOnUp;
		}
	}

	public bool IsOver
	{
		get
		{
			return isDown || isHover;
		}
		set
		{
			if (value == isDown && !isHover)
			{
				return;
			}
			if (value)
			{
				isHover = true;
				SetState();
				if (this.OnToggleOver != null)
				{
					this.OnToggleOver(this);
				}
			}
			else if (isDown && isHover)
			{
				isDown = false;
				isHover = false;
				SetState();
				if (this.OnToggleOver != null)
				{
					this.OnToggleOver(this);
				}
			}
			else if (isDown)
			{
				isDown = false;
				SetState();
				if (this.OnToggleOver != null)
				{
					this.OnToggleOver(this);
				}
			}
			else
			{
				isHover = false;
				SetState();
				if (this.OnToggleOver != null)
				{
					this.OnToggleOver(this);
				}
			}
			DoSendMessage(SendMessageOnToggleOverMethodName, this);
		}
	}

	public event Action<tk2dUIUpDownHoverButton> OnToggleOver
	{
		add
		{
			Action<tk2dUIUpDownHoverButton> action = this.m_OnToggleOver;
			Action<tk2dUIUpDownHoverButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnToggleOver, (Action<tk2dUIUpDownHoverButton>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIUpDownHoverButton> action = this.m_OnToggleOver;
			Action<tk2dUIUpDownHoverButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnToggleOver, (Action<tk2dUIUpDownHoverButton>)Delegate.Remove(action2, value), action);
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
			uiItem.OnDown += ButtonDown;
			if (useOnReleaseInsteadOfOnUp)
			{
				uiItem.OnRelease += ButtonUp;
			}
			else
			{
				uiItem.OnUp += ButtonUp;
			}
			uiItem.OnHoverOver += ButtonHoverOver;
			uiItem.OnHoverOut += ButtonHoverOut;
		}
	}

	private void OnDisable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnDown -= ButtonDown;
			if (useOnReleaseInsteadOfOnUp)
			{
				uiItem.OnRelease -= ButtonUp;
			}
			else
			{
				uiItem.OnUp -= ButtonUp;
			}
			uiItem.OnHoverOver -= ButtonHoverOver;
			uiItem.OnHoverOut -= ButtonHoverOut;
		}
	}

	private void ButtonUp()
	{
		if (isDown)
		{
			isDown = false;
			SetState();
			if (!isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	private void ButtonDown()
	{
		if (!isDown)
		{
			isDown = true;
			SetState();
			if (!isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	private void ButtonHoverOver()
	{
		if (!isHover)
		{
			isHover = true;
			SetState();
			if (!isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	private void ButtonHoverOut()
	{
		if (isHover)
		{
			isHover = false;
			SetState();
			if (!isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(upStateGO, !isDown && !isHover);
		if (downStateGO == hoverOverStateGO)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(downStateGO, isDown || isHover);
			return;
		}
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(downStateGO, isDown);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(hoverOverStateGO, isHover);
	}

	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		useOnReleaseInsteadOfOnUp = state;
	}
}
