using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButton")]
public class tk2dUIToggleButton : tk2dUIBaseItemControl
{
	public GameObject offStateGO;

	public GameObject onStateGO;

	public bool activateOnPress;

	[SerializeField]
	private bool isOn = true;

	private bool isInToggleGroup;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIToggleButton> m_OnToggle;

	public string SendMessageOnToggleMethodName = string.Empty;

	public bool IsOn
	{
		get
		{
			return isOn;
		}
		set
		{
			if (isOn != value)
			{
				isOn = value;
				SetState();
				this.m_OnToggle?.Invoke(this);
			}
		}
	}

	public bool IsInToggleGroup
	{
		get
		{
			return isInToggleGroup;
		}
		set
		{
			isInToggleGroup = value;
		}
	}

	public event Action<tk2dUIToggleButton> OnToggle
	{
		add
		{
			Action<tk2dUIToggleButton> action = this.m_OnToggle;
			Action<tk2dUIToggleButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnToggle, (Action<tk2dUIToggleButton>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIToggleButton> action = this.m_OnToggle;
			Action<tk2dUIToggleButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnToggle, (Action<tk2dUIToggleButton>)Delegate.Remove(action2, value), action);
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
			uiItem.OnClick += ButtonClick;
			uiItem.OnDown += ButtonDown;
		}
	}

	private void OnDisable()
	{
		if ((bool)uiItem)
		{
			uiItem.OnClick -= ButtonClick;
			uiItem.OnDown -= ButtonDown;
		}
	}

	private void ButtonClick()
	{
		if (!activateOnPress)
		{
			ButtonToggle();
		}
	}

	private void ButtonDown()
	{
		if (activateOnPress)
		{
			ButtonToggle();
		}
	}

	private void ButtonToggle()
	{
		if (!isOn || !isInToggleGroup)
		{
			isOn = !isOn;
			SetState();
			this.m_OnToggle?.Invoke(this);
			DoSendMessage(SendMessageOnToggleMethodName, this);
		}
	}

	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(offStateGO, !isOn);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(onStateGO, isOn);
	}
}
