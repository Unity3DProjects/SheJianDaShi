using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButtonGroup")]
public class tk2dUIToggleButtonGroup : MonoBehaviour
{
	[SerializeField]
	private tk2dUIToggleButton[] toggleBtns;

	public GameObject sendMessageTarget;

	[SerializeField]
	private int selectedIndex;

	private tk2dUIToggleButton selectedToggleButton;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIToggleButtonGroup> m_OnChange;

	public string SendMessageOnChangeMethodName = string.Empty;

	public tk2dUIToggleButton[] ToggleBtns
	{
		get
		{
			return toggleBtns;
		}
	}

	public int SelectedIndex
	{
		get
		{
			return selectedIndex;
		}
		set
		{
			if (selectedIndex != value)
			{
				selectedIndex = value;
				SetToggleButtonUsingSelectedIndex();
			}
		}
	}

	public tk2dUIToggleButton SelectedToggleButton
	{
		get
		{
			return selectedToggleButton;
		}
		set
		{
			ButtonToggle(value);
		}
	}

	public event Action<tk2dUIToggleButtonGroup> OnChange
	{
		add
		{
			Action<tk2dUIToggleButtonGroup> action = this.m_OnChange;
			Action<tk2dUIToggleButtonGroup> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnChange, (Action<tk2dUIToggleButtonGroup>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIToggleButtonGroup> action = this.m_OnChange;
			Action<tk2dUIToggleButtonGroup> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnChange, (Action<tk2dUIToggleButtonGroup>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected virtual void Awake()
	{
		Setup();
	}

	protected void Setup()
	{
		tk2dUIToggleButton[] array = toggleBtns;
		foreach (tk2dUIToggleButton tk2dUIToggleButton2 in array)
		{
			if (tk2dUIToggleButton2 != null)
			{
				tk2dUIToggleButton2.IsInToggleGroup = true;
				tk2dUIToggleButton2.IsOn = false;
				tk2dUIToggleButton2.OnToggle += ButtonToggle;
			}
		}
		SetToggleButtonUsingSelectedIndex();
	}

	public void AddNewToggleButtons(tk2dUIToggleButton[] newToggleBtns)
	{
		ClearExistingToggleBtns();
		toggleBtns = newToggleBtns;
		Setup();
	}

	private void ClearExistingToggleBtns()
	{
		if (toggleBtns != null && toggleBtns.Length > 0)
		{
			tk2dUIToggleButton[] array = toggleBtns;
			foreach (tk2dUIToggleButton tk2dUIToggleButton2 in array)
			{
				tk2dUIToggleButton2.IsInToggleGroup = false;
				tk2dUIToggleButton2.OnToggle -= ButtonToggle;
				tk2dUIToggleButton2.IsOn = false;
			}
		}
	}

	private void SetToggleButtonUsingSelectedIndex()
	{
		tk2dUIToggleButton tk2dUIToggleButton2 = null;
		if (selectedIndex >= 0 && selectedIndex < toggleBtns.Length)
		{
			tk2dUIToggleButton2 = toggleBtns[selectedIndex];
			tk2dUIToggleButton2.IsOn = true;
		}
		else
		{
			tk2dUIToggleButton2 = null;
			selectedIndex = -1;
			ButtonToggle(tk2dUIToggleButton2);
		}
	}

	private void ButtonToggle(tk2dUIToggleButton toggleButton)
	{
		if (!(toggleButton == null) && !toggleButton.IsOn)
		{
			return;
		}
		tk2dUIToggleButton[] array = toggleBtns;
		foreach (tk2dUIToggleButton tk2dUIToggleButton2 in array)
		{
			if (tk2dUIToggleButton2 != toggleButton)
			{
				tk2dUIToggleButton2.IsOn = false;
			}
		}
		if (toggleButton != selectedToggleButton)
		{
			selectedToggleButton = toggleButton;
			SetSelectedIndexFromSelectedToggleButton();
			this.m_OnChange?.Invoke(this);
			if (sendMessageTarget != null && SendMessageOnChangeMethodName.Length > 0)
			{
				sendMessageTarget.SendMessage(SendMessageOnChangeMethodName, this, SendMessageOptions.RequireReceiver);
			}
		}
	}

	private void SetSelectedIndexFromSelectedToggleButton()
	{
		selectedIndex = -1;
		for (int i = 0; i < toggleBtns.Length; i++)
		{
			tk2dUIToggleButton tk2dUIToggleButton2 = toggleBtns[i];
			if (tk2dUIToggleButton2 == selectedToggleButton)
			{
				selectedIndex = i;
				break;
			}
		}
	}
}
