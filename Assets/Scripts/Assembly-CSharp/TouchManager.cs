using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class TouchManager : SingletonMonoBehaviour<TouchManager>
{
	public delegate void TouchManagerEventHandler(TouchInfo touch);

	public delegate void TouchManagerGestureHandler();

	[SerializeField]
	private float touchStationaryDelta;

	private static TouchInfo touchInfoPrimary;

	private static TouchInfo touchInfoSecondary;

	private static TouchInfo touchInfoResult;

	private static TouchInfo mainTouchInfo;

	private static bool mouseButtonIsDown;

	private static bool quadraTouchFired;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerEventHandler m_TouchBeganEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerEventHandler m_TouchEndedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerEventHandler m_TouchMoveEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerEventHandler m_TouchHoldEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerGestureHandler m_MoveRightEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerGestureHandler m_MoveLeftEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerGestureHandler m_MoveUpEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static TouchManagerGestureHandler m_MoveDownEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_QuadraTouchEvent;

	public static event TouchManagerEventHandler TouchBeganEvent
	{
		add
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchBeganEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchBeganEvent, (TouchManagerEventHandler)Delegate.Combine(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
		remove
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchBeganEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchBeganEvent, (TouchManagerEventHandler)Delegate.Remove(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
	}

	public static event TouchManagerEventHandler TouchEndedEvent
	{
		add
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchEndedEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchEndedEvent, (TouchManagerEventHandler)Delegate.Combine(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
		remove
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchEndedEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchEndedEvent, (TouchManagerEventHandler)Delegate.Remove(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
	}

	public static event TouchManagerEventHandler TouchMoveEvent
	{
		add
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchMoveEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchMoveEvent, (TouchManagerEventHandler)Delegate.Combine(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
		remove
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchMoveEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchMoveEvent, (TouchManagerEventHandler)Delegate.Remove(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
	}

	public static event TouchManagerEventHandler TouchHoldEvent
	{
		add
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchHoldEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchHoldEvent, (TouchManagerEventHandler)Delegate.Combine(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
		remove
		{
			TouchManagerEventHandler touchManagerEventHandler = TouchManager.m_TouchHoldEvent;
			TouchManagerEventHandler touchManagerEventHandler2;
			do
			{
				touchManagerEventHandler2 = touchManagerEventHandler;
				touchManagerEventHandler = Interlocked.CompareExchange(ref TouchManager.m_TouchHoldEvent, (TouchManagerEventHandler)Delegate.Remove(touchManagerEventHandler2, value), touchManagerEventHandler);
			}
			while (touchManagerEventHandler != touchManagerEventHandler2);
		}
	}

	public static event TouchManagerGestureHandler MoveRightEvent
	{
		add
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveRightEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveRightEvent, (TouchManagerGestureHandler)Delegate.Combine(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
		remove
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveRightEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveRightEvent, (TouchManagerGestureHandler)Delegate.Remove(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
	}

	public static event TouchManagerGestureHandler MoveLeftEvent
	{
		add
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveLeftEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveLeftEvent, (TouchManagerGestureHandler)Delegate.Combine(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
		remove
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveLeftEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveLeftEvent, (TouchManagerGestureHandler)Delegate.Remove(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
	}

	public static event TouchManagerGestureHandler MoveUpEvent
	{
		add
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveUpEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveUpEvent, (TouchManagerGestureHandler)Delegate.Combine(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
		remove
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveUpEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveUpEvent, (TouchManagerGestureHandler)Delegate.Remove(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
	}

	public static event TouchManagerGestureHandler MoveDownEvent
	{
		add
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveDownEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveDownEvent, (TouchManagerGestureHandler)Delegate.Combine(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
		remove
		{
			TouchManagerGestureHandler touchManagerGestureHandler = TouchManager.m_MoveDownEvent;
			TouchManagerGestureHandler touchManagerGestureHandler2;
			do
			{
				touchManagerGestureHandler2 = touchManagerGestureHandler;
				touchManagerGestureHandler = Interlocked.CompareExchange(ref TouchManager.m_MoveDownEvent, (TouchManagerGestureHandler)Delegate.Remove(touchManagerGestureHandler2, value), touchManagerGestureHandler);
			}
			while (touchManagerGestureHandler != touchManagerGestureHandler2);
		}
	}

	public static event Action QuadraTouchEvent
	{
		add
		{
			Action action = TouchManager.m_QuadraTouchEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TouchManager.m_QuadraTouchEvent, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = TouchManager.m_QuadraTouchEvent;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TouchManager.m_QuadraTouchEvent, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		TouchInfo.StationaryDelta = touchStationaryDelta;
	}

	public static void UpdateTouches()
	{
		ProcessTouches();
	}

	private static void ProcessTouches()
	{
		if (GUIUtility.hotControl == 0)
		{
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				ProcessRealTouches();
				return;
			}
			ProcessMouseEvents();
			ProcessKeyboardInput();
		}
	}

	private static void ProcessKeyboardInput()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (TouchManager.m_MoveUpEvent != null)
			{
				TouchManager.m_MoveUpEvent();
			}
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (TouchManager.m_MoveDownEvent != null)
			{
				TouchManager.m_MoveDownEvent();
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (TouchManager.m_MoveLeftEvent != null)
			{
				TouchManager.m_MoveLeftEvent();
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) && TouchManager.m_MoveRightEvent != null)
		{
			TouchManager.m_MoveRightEvent();
		}
	}

	private static void ProcessMouseEvents()
	{
		if (Input.GetMouseButtonDown(0))
		{
			mainTouchInfo = new TouchInfo();
			mouseButtonIsDown = true;
			mainTouchInfo.id = 0;
			mainTouchInfo.startPosition = Input.mousePosition;
			mainTouchInfo.position = Input.mousePosition;
			mainTouchInfo.phase = TouchPhase.Began;
			if (TouchManager.m_TouchBeganEvent != null)
			{
				TouchManager.m_TouchBeganEvent(mainTouchInfo);
			}
		}
		if (Input.GetMouseButtonUp(0) && mouseButtonIsDown)
		{
			mouseButtonIsDown = false;
			if (mainTouchInfo != null)
			{
				mainTouchInfo.UpdatePosition(Input.mousePosition);
				mainTouchInfo.phase = TouchPhase.Ended;
				if (TouchManager.m_TouchEndedEvent != null)
				{
					TouchManager.m_TouchEndedEvent(mainTouchInfo);
				}
				mainTouchInfo = null;
			}
		}
		if (!mouseButtonIsDown || mainTouchInfo == null)
		{
			return;
		}
		mainTouchInfo.UpdatePosition(Input.mousePosition);
		if (mainTouchInfo.IsStationary)
		{
			mainTouchInfo.phase = TouchPhase.Stationary;
			if (TouchManager.m_TouchHoldEvent != null)
			{
				TouchManager.m_TouchHoldEvent(mainTouchInfo);
			}
		}
		else
		{
			mainTouchInfo.phase = TouchPhase.Moved;
			if (TouchManager.m_TouchMoveEvent != null)
			{
				TouchManager.m_TouchMoveEvent(mainTouchInfo);
			}
		}
	}

	private static void ProcessRealTouches()
	{
		touchInfoPrimary = null;
		touchInfoSecondary = null;
		touchInfoResult = null;
		if (Input.touchCount > 0)
		{
			if (Input.touchCount == 4)
			{
				if (!quadraTouchFired && TouchManager.m_QuadraTouchEvent != null)
				{
					quadraTouchFired = true;
					TouchManager.m_QuadraTouchEvent();
				}
			}
			else
			{
				quadraTouchFired = false;
			}
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (touch.phase == TouchPhase.Began)
				{
					touchInfoPrimary = new TouchInfo(touch);
				}
				else if (mainTouchInfo != null && touch.fingerId == mainTouchInfo.id)
				{
					touchInfoSecondary = new TouchInfo(touch);
				}
			}
		}
		if (touchInfoSecondary != null)
		{
			touchInfoResult = touchInfoSecondary;
		}
		else if (touchInfoPrimary != null)
		{
			touchInfoResult = touchInfoPrimary;
		}
		if (touchInfoPrimary != null || touchInfoSecondary != null)
		{
			if (touchInfoResult.phase == TouchPhase.Began)
			{
				if (mainTouchInfo != null)
				{
					mainTouchInfo.phase = TouchPhase.Ended;
					if (TouchManager.m_TouchEndedEvent != null)
					{
						TouchManager.m_TouchEndedEvent(mainTouchInfo);
					}
					mainTouchInfo = null;
				}
				mainTouchInfo = new TouchInfo(touchInfoResult);
				if (TouchManager.m_TouchBeganEvent != null)
				{
					TouchManager.m_TouchBeganEvent(mainTouchInfo);
				}
			}
			else if (touchInfoResult.phase == TouchPhase.Ended && mainTouchInfo != null)
			{
				mainTouchInfo.phase = TouchPhase.Ended;
				mainTouchInfo.UpdatePosition(touchInfoResult.position);
				if (TouchManager.m_TouchEndedEvent != null)
				{
					TouchManager.m_TouchEndedEvent(mainTouchInfo);
				}
				mainTouchInfo = null;
			}
			else if (touchInfoResult.phase == TouchPhase.Moved && mainTouchInfo != null)
			{
				mainTouchInfo.phase = TouchPhase.Moved;
				mainTouchInfo.UpdatePosition(touchInfoResult.position);
				if (TouchManager.m_TouchMoveEvent != null)
				{
					TouchManager.m_TouchMoveEvent(mainTouchInfo);
				}
			}
			else if (mainTouchInfo != null)
			{
				mainTouchInfo.phase = TouchPhase.Stationary;
				mainTouchInfo.UpdatePosition(touchInfoResult.position);
				if (TouchManager.m_TouchHoldEvent != null)
				{
					TouchManager.m_TouchHoldEvent(mainTouchInfo);
				}
			}
		}
		else if (mainTouchInfo != null)
		{
			mainTouchInfo.phase = TouchPhase.Ended;
			if (TouchManager.m_TouchEndedEvent != null)
			{
				TouchManager.m_TouchEndedEvent(mainTouchInfo);
			}
			mainTouchInfo = null;
		}
	}
}
