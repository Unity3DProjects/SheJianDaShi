using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class GUIDynamicCounter : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnCounterValueChanged;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnCounterValueChangeEnded;

	[SerializeField]
	private tk2dTextMesh counterLabel;

	[SerializeField]
	private float filterValue = 0.1f;

	[SerializeField]
	private bool isRoundCounter;

	[SerializeField]
	private int roundValue = 5;

	private StringBuilder sb = new StringBuilder();

	private string postfixString = string.Empty;

	private string prefixString = string.Empty;

	private float currentValue;

	private float finalValue;

	public tk2dTextMesh CounterLabel
	{
		get
		{
			return counterLabel;
		}
	}

	private string CurrentPrefixString
	{
		get
		{
			return prefixString;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && prefixString != value)
			{
				prefixString = value;
			}
		}
	}

	private string CurrentPostfixString
	{
		get
		{
			return postfixString;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && postfixString != value)
			{
				postfixString = value;
			}
		}
	}

	private string CurrentValueString
	{
		get
		{
			if (sb.Length > 0)
			{
				sb.Remove(0, sb.Length);
			}
			sb.Append(CurrentPrefixString);
			sb.Append(currentValue.ToString("F0"));
			sb.Append(CurrentPostfixString);
			return sb.ToString();
		}
	}

	public event Action OnCounterValueChanged
	{
		add
		{
			Action action = this.m_OnCounterValueChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCounterValueChanged, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnCounterValueChanged;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCounterValueChanged, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnCounterValueChangeEnded
	{
		add
		{
			Action action = this.m_OnCounterValueChangeEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCounterValueChangeEnded, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnCounterValueChangeEnded;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCounterValueChangeEnded, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Update()
	{
		if (!(Mathf.Abs(finalValue - currentValue) > float.Epsilon))
		{
			return;
		}
		int num = Mathf.FloorToInt(currentValue);
		currentValue += filterValue * Mathf.Abs(finalValue - (float)num);
		currentValue = Mathf.Min(currentValue, finalValue);
		if (num != Mathf.FloorToInt(currentValue))
		{
			counterLabel.text = CurrentValueString;
			if (this.OnCounterValueChanged != null)
			{
				this.OnCounterValueChanged();
			}
		}
		else if (this.OnCounterValueChangeEnded != null)
		{
			this.OnCounterValueChangeEnded();
		}
	}

	public void SetValue(float v, bool immediately = false, string prefix = null, string postfix = null)
	{
		if (isRoundCounter)
		{
			float num = v % (float)roundValue;
			if (!(num < 1f))
			{
				v += (float)roundValue - num;
			}
		}
		CurrentPostfixString = postfix;
		CurrentPrefixString = prefix;
		if (immediately)
		{
			currentValue = (finalValue = v);
			counterLabel.text = CurrentValueString;
		}
		else
		{
			finalValue = v;
		}
	}
}
