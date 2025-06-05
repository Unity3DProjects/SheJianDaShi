using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIItem")]
public class tk2dUIItem : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<UISoundType> m_OnStaticClick;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnDown;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnUp;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnClick;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnRelease;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnHoverOver;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnHoverOut;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> m_OnDownUIItem;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> m_OnUpUIItem;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> m_OnClickUIItem;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> m_OnReleaseUIItem;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> m_OnHoverOverUIItem;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<tk2dUIItem> m_OnHoverOutUIItem;

	public UISoundType soundType = UISoundType.Button;

	public GameObject sendMessageTarget;

	public string SendMessageOnDownMethodName = string.Empty;

	public string SendMessageOnUpMethodName = string.Empty;

	public string SendMessageOnClickMethodName = string.Empty;

	public string SendMessageOnReleaseMethodName = string.Empty;

	[SerializeField]
	private bool isChildOfAnotherUIItem;

	public bool registerPressFromChildren;

	public bool isHoverEnabled;

	public bool isOverTouchDisabled;

	public Transform[] editorExtraBounds = new Transform[0];

	public Transform[] editorIgnoreBounds = new Transform[0];

	private bool isPressed;

	private bool isHoverOver;

	private tk2dUITouch touch;

	private tk2dUIItem parentUIItem;

	public bool IsPressed
	{
		get
		{
			return isPressed;
		}
	}

	public tk2dUITouch Touch
	{
		get
		{
			return touch;
		}
	}

	public tk2dUIItem ParentUIItem
	{
		get
		{
			return parentUIItem;
		}
	}

	public static event Action<UISoundType> OnStaticClick
	{
		add
		{
			Action<UISoundType> action = tk2dUIItem.m_OnStaticClick;
			Action<UISoundType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tk2dUIItem.m_OnStaticClick, (Action<UISoundType>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<UISoundType> action = tk2dUIItem.m_OnStaticClick;
			Action<UISoundType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tk2dUIItem.m_OnStaticClick, (Action<UISoundType>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnDown
	{
		add
		{
			Action action = this.m_OnDown;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnDown, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnDown;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnDown, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnUp
	{
		add
		{
			Action action = this.m_OnUp;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnUp, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnUp;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnUp, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnClick
	{
		add
		{
			Action action = this.m_OnClick;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClick, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnClick;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClick, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnRelease
	{
		add
		{
			Action action = this.m_OnRelease;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRelease, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnRelease;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRelease, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnHoverOver
	{
		add
		{
			Action action = this.m_OnHoverOver;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOver, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnHoverOver;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOver, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnHoverOut
	{
		add
		{
			Action action = this.m_OnHoverOut;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOut, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnHoverOut;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOut, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<tk2dUIItem> OnDownUIItem
	{
		add
		{
			Action<tk2dUIItem> action = this.m_OnDownUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnDownUIItem, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = this.m_OnDownUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnDownUIItem, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<tk2dUIItem> OnUpUIItem
	{
		add
		{
			Action<tk2dUIItem> action = this.m_OnUpUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnUpUIItem, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = this.m_OnUpUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnUpUIItem, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<tk2dUIItem> OnClickUIItem
	{
		add
		{
			Action<tk2dUIItem> action = this.m_OnClickUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClickUIItem, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = this.m_OnClickUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClickUIItem, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<tk2dUIItem> OnReleaseUIItem
	{
		add
		{
			Action<tk2dUIItem> action = this.m_OnReleaseUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnReleaseUIItem, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = this.m_OnReleaseUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnReleaseUIItem, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<tk2dUIItem> OnHoverOverUIItem
	{
		add
		{
			Action<tk2dUIItem> action = this.m_OnHoverOverUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOverUIItem, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = this.m_OnHoverOverUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOverUIItem, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<tk2dUIItem> OnHoverOutUIItem
	{
		add
		{
			Action<tk2dUIItem> action = this.m_OnHoverOutUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOutUIItem, (Action<tk2dUIItem>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dUIItem> action = this.m_OnHoverOutUIItem;
			Action<tk2dUIItem> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnHoverOutUIItem, (Action<tk2dUIItem>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected virtual void Awake()
	{
		if (isChildOfAnotherUIItem)
		{
			UpdateParent();
		}
	}

	private void Start()
	{
		if (tk2dUIManager.Instance == null)
		{
			CustomDebug.LogError("Unable to find tk2dUIManager. Please create a tk2dUIManager in the scene before proceeding.");
		}
		if (isChildOfAnotherUIItem && parentUIItem == null)
		{
			UpdateParent();
		}
	}

	public void UpdateParent()
	{
		parentUIItem = GetParentUIItem();
	}

	public void ManuallySetParent(tk2dUIItem newParentUIItem)
	{
		parentUIItem = newParentUIItem;
	}

	public void RemoveParent()
	{
		parentUIItem = null;
	}

	public virtual bool Press(tk2dUITouch touch)
	{
		return Press(touch, null);
	}

	public bool Press(tk2dUITouch touch, tk2dUIItem sentFromChild)
	{
		if (isPressed)
		{
			return false;
		}
		if (!isPressed)
		{
			this.touch = touch;
			if ((registerPressFromChildren || sentFromChild == null) && base.enabled)
			{
				isPressed = true;
				this.m_OnDown?.Invoke();
				this.m_OnDownUIItem?.Invoke(this);
				DoSendMessage(SendMessageOnDownMethodName);
			}
			if (parentUIItem != null)
			{
				parentUIItem.Press(touch, this);
			}
		}
		return true;
	}

	public virtual void UpdateTouch(tk2dUITouch touch)
	{
		this.touch = touch;
		if (parentUIItem != null)
		{
			parentUIItem.UpdateTouch(touch);
		}
	}

	private void DoSendMessage(string methodName)
	{
		if (sendMessageTarget != null && methodName.Length > 0)
		{
			sendMessageTarget.SendMessage(methodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	public void Release()
	{
		if (isPressed)
		{
			isPressed = false;
			this.m_OnUp?.Invoke();
			this.m_OnUpUIItem?.Invoke(this);
			DoSendMessage(SendMessageOnUpMethodName);
			this.m_OnClick?.Invoke();
			tk2dUIItem.m_OnStaticClick?.Invoke(soundType);
			this.m_OnClickUIItem?.Invoke(this);
			DoSendMessage(SendMessageOnClickMethodName);
		}
		this.m_OnRelease?.Invoke();
		this.m_OnReleaseUIItem?.Invoke(this);
		DoSendMessage(SendMessageOnReleaseMethodName);
		if (parentUIItem != null)
		{
			parentUIItem.Release();
		}
	}

	public void CurrentOverUIItem(tk2dUIItem overUIItem)
	{
		if (!(overUIItem != this))
		{
			return;
		}
		if (isPressed)
		{
			if (!isOverTouchDisabled && !CheckIsUIItemChildOfMe(overUIItem))
			{
				Exit();
				if (parentUIItem != null)
				{
					parentUIItem.CurrentOverUIItem(overUIItem);
				}
			}
		}
		else if (parentUIItem != null)
		{
			parentUIItem.CurrentOverUIItem(overUIItem);
		}
	}

	public bool CheckIsUIItemChildOfMe(tk2dUIItem uiItem)
	{
		tk2dUIItem tk2dUIItem2 = null;
		bool result = false;
		if (uiItem != null)
		{
			tk2dUIItem2 = uiItem.parentUIItem;
		}
		while (tk2dUIItem2 != null)
		{
			if (tk2dUIItem2 == this)
			{
				result = true;
				break;
			}
			tk2dUIItem2 = tk2dUIItem2.parentUIItem;
		}
		return result;
	}

	public void Exit()
	{
		if (isPressed)
		{
			isPressed = false;
			this.m_OnUp?.Invoke();
			this.m_OnUpUIItem?.Invoke(this);
			DoSendMessage(SendMessageOnUpMethodName);
		}
	}

	public bool HoverOver(tk2dUIItem prevHover)
	{
		bool flag = false;
		tk2dUIItem tk2dUIItem2 = null;
		if (!isHoverOver)
		{
			this.m_OnHoverOver?.Invoke();
			this.m_OnHoverOverUIItem?.Invoke(this);
			isHoverOver = true;
		}
		if (prevHover == this)
		{
			flag = true;
		}
		if (parentUIItem != null && parentUIItem.isHoverEnabled)
		{
			tk2dUIItem2 = parentUIItem;
		}
		if (tk2dUIItem2 == null)
		{
			return flag;
		}
		return tk2dUIItem2.HoverOver(prevHover) || flag;
	}

	public void HoverOut(tk2dUIItem currHoverButton)
	{
		if (isHoverOver)
		{
			this.m_OnHoverOut?.Invoke();
			this.m_OnHoverOutUIItem?.Invoke(this);
			isHoverOver = false;
		}
		if (parentUIItem != null && parentUIItem.isHoverEnabled)
		{
			if (currHoverButton == null)
			{
				parentUIItem.HoverOut(currHoverButton);
			}
			else if (!parentUIItem.CheckIsUIItemChildOfMe(currHoverButton) && currHoverButton != parentUIItem)
			{
				parentUIItem.HoverOut(currHoverButton);
			}
		}
	}

	private tk2dUIItem GetParentUIItem()
	{
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			tk2dUIItem component = parent.GetComponent<tk2dUIItem>();
			if (component != null)
			{
				return component;
			}
			parent = parent.parent;
		}
		return null;
	}

	public void SimulateClick()
	{
		this.m_OnDown?.Invoke();
		this.m_OnDownUIItem?.Invoke(this);
		DoSendMessage(SendMessageOnDownMethodName);
		this.m_OnUp?.Invoke();
		this.m_OnUpUIItem?.Invoke(this);
		DoSendMessage(SendMessageOnUpMethodName);
		this.m_OnClick?.Invoke();
		tk2dUIItem.m_OnStaticClick?.Invoke(soundType);
		this.m_OnClickUIItem?.Invoke(this);
		DoSendMessage(SendMessageOnClickMethodName);
		this.m_OnRelease?.Invoke();
		this.m_OnReleaseUIItem?.Invoke(this);
		DoSendMessage(SendMessageOnReleaseMethodName);
	}

	public void InternalSetIsChildOfAnotherUIItem(bool state)
	{
		isChildOfAnotherUIItem = state;
	}

	public bool InternalGetIsChildOfAnotherUIItem()
	{
		return isChildOfAnotherUIItem;
	}
}
