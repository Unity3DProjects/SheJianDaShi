using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIMultiStateToggleButton")]
public class tk2dUIMultiStateToggleButton : tk2dUIBaseItemControl
{
	public GameObject[] states;

	public bool activateOnPress;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIMultiStateToggleButton> m_OnStateToggle;

	private int index;

	public string SendMessageOnStateToggleMethodName = string.Empty;

	public int Index
	{
		get
		{
			return index;
		}
		set
		{
			if (value >= states.Length)
			{
				value = states.Length;
			}
			if (value < 0)
			{
				value = 0;
			}
			if (index != value)
			{
				index = value;
				SetState();
				if (this.OnStateToggle != null)
				{
					this.OnStateToggle(this);
				}
				DoSendMessage(SendMessageOnStateToggleMethodName, this);
			}
		}
	}

	public event Action<tk2dUIMultiStateToggleButton> OnStateToggle
	{
		add
		{
			Action<tk2dUIMultiStateToggleButton> action = this.m_OnStateToggle;
			Action<tk2dUIMultiStateToggleButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnStateToggle, (Action<tk2dUIMultiStateToggleButton>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIMultiStateToggleButton> action = this.m_OnStateToggle;
			Action<tk2dUIMultiStateToggleButton> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnStateToggle, (Action<tk2dUIMultiStateToggleButton>)Delegate.Remove(action2, value), action);
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
		if (Index + 1 >= states.Length)
		{
			Index = 0;
		}
		else
		{
			Index++;
		}
	}

	private void SetState()
	{
		for (int i = 0; i < states.Length; i++)
		{
			GameObject gameObject = states[i];
			if (!(gameObject != null))
			{
				continue;
			}
			if (i != index)
			{
				if (states[i].activeInHierarchy)
				{
					states[i].SetActive(false);
				}
			}
			else if (!states[i].activeInHierarchy)
			{
				states[i].SetActive(true);
			}
		}
	}
}
