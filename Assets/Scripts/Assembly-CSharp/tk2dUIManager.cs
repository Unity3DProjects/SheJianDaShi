using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIManager")]
public class tk2dUIManager : MonoBehaviour
{
	public static double version = 1.0;

	public static int releaseId = 0;

	private static tk2dUIManager instance;

	[SerializeField]
	private Camera uiCamera;

	private static List<tk2dUICamera> allCameras = new List<tk2dUICamera>();

	private List<tk2dUICamera> sortedCameras = new List<tk2dUICamera>();

	public LayerMask raycastLayerMask = -1;

	private bool inputEnabled = true;

	private bool lockedForTransition;

	private bool lockedForPopupAnimations;

	private bool lockedForPlayAnimation;

	private tk2dUIItem forcedUIItem;

	public bool areHoverEventsTracked = true;

	private tk2dUIItem pressedUIItem;

	private tk2dUIItem overUIItem;

	private tk2dUITouch firstPressedUIItemTouch;

	private bool checkForHovers = true;

	[SerializeField]
	private bool useMultiTouch;

	private bool claimTouches;

	private const int MAX_MULTI_TOUCH_COUNT = 5;

	private tk2dUITouch[] allTouches = new tk2dUITouch[5];

	private List<tk2dUIItem> prevPressedUIItemList = new List<tk2dUIItem>();

	private tk2dUIItem[] pressedUIItems = new tk2dUIItem[5];

	private int touchCounter;

	private Vector2 mouseDownFirstPos = Vector2.zero;

	private const float SWIPE_THRESHOLD_PERCENT = 0.05f;

	private const string MOUSE_WHEEL_AXES_NAME = "Mouse ScrollWheel";

	private tk2dUITouch primaryTouch = default(tk2dUITouch);

	private tk2dUITouch secondaryTouch = default(tk2dUITouch);

	private tk2dUITouch resultTouch = default(tk2dUITouch);

	private tk2dUIItem hitUIItem;

	private RaycastHit hit;

	private Ray ray;

	private tk2dUITouch currTouch;

	private tk2dUIItem currPressedItem;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnAnyPress;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnInputUpdate;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<float> m_OnScrollWheelChange;

	public static tk2dUIManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(tk2dUIManager)) as tk2dUIManager;
				if (instance == null)
				{
					GameObject gameObject = new GameObject("tk2dUIManager");
					instance = gameObject.AddComponent<tk2dUIManager>();
				}
			}
			return instance;
		}
	}

	public static tk2dUIManager Instance__NoCreate
	{
		get
		{
			return instance;
		}
	}

	public Camera UICamera
	{
		get
		{
			return uiCamera;
		}
		set
		{
			uiCamera = value;
		}
	}

	public bool InputEnabled
	{
		get
		{
			return inputEnabled;
		}
		set
		{
			if ((bool)SingletonMonoBehaviour<Scheduler>.InstanceIfExist)
			{
				SingletonMonoBehaviour<Scheduler>.Instance.UnscheduleAllMethodForTarget(this);
			}
			if (inputEnabled && !value)
			{
				SortCameras();
				inputEnabled = value;
				if (useMultiTouch)
				{
					ClearTouches();
				}
				else
				{
					CheckInputs();
				}
			}
			else
			{
				inputEnabled = value;
			}
		}
	}

	public bool LockedForTransition
	{
		get
		{
			return lockedForTransition;
		}
		set
		{
			lockedForTransition = value;
		}
	}

	public bool LockedForPopupAnimations
	{
		get
		{
			return lockedForPopupAnimations;
		}
		set
		{
			lockedForPopupAnimations = value;
		}
	}

	public bool LockedForPlayAnimation
	{
		get
		{
			return lockedForPlayAnimation;
		}
		set
		{
			lockedForPlayAnimation = value;
		}
	}

	private bool IsEnabledInputTouches
	{
		get
		{
			return InputEnabled && !LockedForTransition && !LockedForPopupAnimations && !LockedForPlayAnimation;
		}
	}

	public tk2dUIItem PressedUIItem
	{
		get
		{
			if (useMultiTouch)
			{
				if (pressedUIItems.Length > 0)
				{
					return pressedUIItems[pressedUIItems.Length - 1];
				}
				return null;
			}
			return pressedUIItem;
		}
	}

	public tk2dUIItem[] PressedUIItems
	{
		get
		{
			return pressedUIItems;
		}
	}

	public bool UseMultiTouch
	{
		get
		{
			return useMultiTouch;
		}
		set
		{
			if (useMultiTouch != value && inputEnabled)
			{
				InputEnabled = false;
				useMultiTouch = value;
				InputEnabled = true;
			}
			else
			{
				useMultiTouch = value;
			}
		}
	}

	public bool ClaimTouches
	{
		get
		{
			return claimTouches;
		}
	}

	public float SwipeThreshold
	{
		get
		{
			return (float)Mathf.Min(Screen.height, Screen.width) * 0.05f;
		}
	}

	public event Action OnAnyPress
	{
		add
		{
			Action action = this.m_OnAnyPress;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnAnyPress, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnAnyPress;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnAnyPress, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnInputUpdate
	{
		add
		{
			Action action = this.m_OnInputUpdate;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnInputUpdate, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnInputUpdate;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnInputUpdate, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<float> OnScrollWheelChange
	{
		add
		{
			Action<float> action = this.m_OnScrollWheelChange;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnScrollWheelChange, (Action<float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float> action = this.m_OnScrollWheelChange;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnScrollWheelChange, (Action<float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public Camera GetUICameraForControl(GameObject go)
	{
		int num = 1 << go.layer;
		int count = allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera2 = allCameras[i];
			if (((int)tk2dUICamera2.FilteredMask & num) != 0)
			{
				return tk2dUICamera2.HostCamera;
			}
		}
		CustomDebug.LogError("Unable to find UI camera for " + go.name);
		return null;
	}

	public static void RegisterCamera(tk2dUICamera cam)
	{
		allCameras.Add(cam);
	}

	public static void UnregisterCamera(tk2dUICamera cam)
	{
		allCameras.Remove(cam);
	}

	private void SortCameras()
	{
		sortedCameras.Clear();
		int count = allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera2 = allCameras[i];
			if (tk2dUICamera2 != null)
			{
				sortedCameras.Add(tk2dUICamera2);
			}
		}
		sortedCameras.Sort((tk2dUICamera a, tk2dUICamera b) => b.GetComponent<Camera>().depth.CompareTo(a.GetComponent<Camera>().depth));
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}
		else if (instance != this)
		{
			CustomDebug.Log("Discarding unnecessary tk2dUIManager instance.");
			if (uiCamera != null)
			{
				HookUpLegacyCamera(uiCamera);
				uiCamera = null;
			}
			UnityEngine.Object.Destroy(this);
			return;
		}
		tk2dUITime.Init();
		Setup();
	}

	private void HookUpLegacyCamera(Camera cam)
	{
		if (cam.GetComponent<tk2dUICamera>() == null)
		{
			tk2dUICamera tk2dUICamera2 = cam.gameObject.AddComponent<tk2dUICamera>();
			tk2dUICamera2.AssignRaycastLayerMask(raycastLayerMask);
		}
	}

	private void Start()
	{
		if (uiCamera != null)
		{
			CustomDebug.Log("It is no longer necessary to hook up a camera to the tk2dUIManager. You can simply attach a tk2dUICamera script to the cameras that interact with UI.");
			HookUpLegacyCamera(uiCamera);
			uiCamera = null;
		}
		if (allCameras.Count == 0)
		{
			CustomDebug.LogError("Unable to find any tk2dUICameras, and no cameras are connected to the tk2dUIManager. You will not be able to interact with the UI.");
		}
	}

	private void Setup()
	{
		if (!areHoverEventsTracked)
		{
			checkForHovers = false;
		}
	}

	private void Update()
	{
		tk2dUITime.Update();
		this.m_OnInputUpdate?.Invoke();
		if (!IsEnabledInputTouches)
		{
			return;
		}
		SortCameras();
		if (useMultiTouch)
		{
			claimTouches = CheckMultiTouchInputs();
		}
		else
		{
			claimTouches = CheckInputs();
		}
		if (this.m_OnScrollWheelChange != null)
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis != 0f)
			{
				this.m_OnScrollWheelChange(axis);
			}
		}
	}

	public void DisableTouchesForScreenTransition()
	{
		LockedForTransition = true;
	}

	public tk2dUIItem GetForcedItem()
	{
		return forcedUIItem;
	}

	public void ForceUIItem(tk2dUIItem item)
	{
		forcedUIItem = item;
	}

	public void ResetForcedUIItem()
	{
		forcedUIItem = null;
	}

	private bool CheckInputs()
	{
		bool result = false;
		if (GUIUtility.hotControl != 0)
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		primaryTouch = default(tk2dUITouch);
		secondaryTouch = default(tk2dUITouch);
		resultTouch = default(tk2dUITouch);
		hitUIItem = null;
		if (IsEnabledInputTouches)
		{
			if (Input.touchCount > 0)
			{
				Touch[] touches = Input.touches;
				for (int i = 0; i < touches.Length; i++)
				{
					Touch touch = touches[i];
					if (touch.phase == TouchPhase.Began)
					{
						primaryTouch = new tk2dUITouch(touch);
						flag = true;
						flag3 = true;
					}
					else if (pressedUIItem != null && touch.fingerId == firstPressedUIItemTouch.fingerId)
					{
						secondaryTouch = new tk2dUITouch(touch);
						flag2 = true;
					}
				}
				checkForHovers = false;
			}
			else if (Input.GetMouseButtonDown(0))
			{
				primaryTouch = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				flag = true;
				flag3 = true;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = Vector2.zero;
				TouchPhase phase = TouchPhase.Moved;
				if (pressedUIItem != null)
				{
					vector = firstPressedUIItemTouch.position - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				}
				if (Input.GetMouseButtonUp(0))
				{
					phase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					phase = TouchPhase.Stationary;
				}
				secondaryTouch = new tk2dUITouch(phase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				flag2 = true;
			}
		}
		if (flag)
		{
			resultTouch = primaryTouch;
		}
		else if (flag2)
		{
			resultTouch = secondaryTouch;
		}
		if (flag || flag2)
		{
			hitUIItem = RaycastForUIItem(resultTouch.position);
			if (resultTouch.phase == TouchPhase.Began)
			{
				if (pressedUIItem != null)
				{
					pressedUIItem.CurrentOverUIItem(hitUIItem);
					if (pressedUIItem != hitUIItem && (forcedUIItem == null || pressedUIItem == forcedUIItem))
					{
						pressedUIItem.Release();
						pressedUIItem = null;
					}
					else
					{
						firstPressedUIItemTouch = resultTouch;
					}
				}
				if (hitUIItem != null && (forcedUIItem == null || hitUIItem == forcedUIItem))
				{
					hitUIItem.Press(resultTouch);
					result = true;
				}
				pressedUIItem = hitUIItem;
				firstPressedUIItemTouch = resultTouch;
			}
			else if (resultTouch.phase == TouchPhase.Ended)
			{
				if (pressedUIItem != null && (forcedUIItem == null || pressedUIItem == forcedUIItem))
				{
					pressedUIItem.CurrentOverUIItem(hitUIItem);
					if (pressedUIItem != null)
					{
						pressedUIItem.UpdateTouch(resultTouch);
					}
					if (pressedUIItem != null)
					{
						pressedUIItem.Release();
					}
					pressedUIItem = null;
				}
			}
			else if (pressedUIItem != null && (forcedUIItem == null || pressedUIItem == forcedUIItem) && hitUIItem != null)
			{
				if (pressedUIItem != null)
				{
					pressedUIItem.CurrentOverUIItem(hitUIItem);
				}
				if (pressedUIItem != null)
				{
					pressedUIItem.UpdateTouch(resultTouch);
				}
			}
		}
		else if (pressedUIItem != null && (forcedUIItem == null || pressedUIItem == forcedUIItem))
		{
			if (pressedUIItem != null)
			{
				pressedUIItem.CurrentOverUIItem(null);
			}
			if (pressedUIItem != null)
			{
				pressedUIItem.Release();
			}
			pressedUIItem = null;
		}
		if (checkForHovers)
		{
			if (IsEnabledInputTouches)
			{
				if (!flag && !flag2 && hitUIItem == null && !Input.GetMouseButton(0))
				{
					hitUIItem = RaycastForUIItem(Input.mousePosition);
				}
				else if (Input.GetMouseButton(0))
				{
					hitUIItem = null;
				}
			}
			if (hitUIItem != null)
			{
				if (hitUIItem.isHoverEnabled)
				{
					if (!hitUIItem.HoverOver(overUIItem) && overUIItem != null)
					{
						overUIItem.HoverOut(hitUIItem);
					}
					overUIItem = hitUIItem;
				}
				else if (overUIItem != null)
				{
					overUIItem.HoverOut(null);
				}
			}
			else if (overUIItem != null)
			{
				overUIItem.HoverOut(null);
			}
		}
		if (flag3 && this.m_OnAnyPress != null)
		{
			this.m_OnAnyPress?.Invoke();
		}
		return result;
	}

	private bool CheckMultiTouchInputs()
	{
		bool result = false;
		bool flag = false;
		int num = -1;
		bool flag2 = false;
		bool flag3 = false;
		touchCounter = 0;
		if (IsEnabledInputTouches)
		{
			if (Input.touchCount > 0)
			{
				Touch[] touches = Input.touches;
				foreach (Touch touch in touches)
				{
					if (touchCounter < 5)
					{
						allTouches[touchCounter] = new tk2dUITouch(touch);
						touchCounter++;
						continue;
					}
					break;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				allTouches[touchCounter] = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				mouseDownFirstPos = Input.mousePosition;
				touchCounter++;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = mouseDownFirstPos - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				TouchPhase phase = TouchPhase.Moved;
				if (Input.GetMouseButtonUp(0))
				{
					phase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					phase = TouchPhase.Stationary;
				}
				allTouches[touchCounter] = new tk2dUITouch(phase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				touchCounter++;
			}
		}
		for (int j = 0; j < touchCounter; j++)
		{
			tk2dUITouch tk2dUITouch2 = allTouches[j];
			pressedUIItems[j] = RaycastForUIItem(tk2dUITouch2.position);
		}
		for (int num2 = prevPressedUIItemList.Count - 1; num2 >= 0; num2--)
		{
			tk2dUIItem tk2dUIItem2 = prevPressedUIItemList[num2];
			if (tk2dUIItem2 != null && tk2dUIItem2.isActiveAndEnabled)
			{
				num = tk2dUIItem2.Touch.fingerId;
				flag2 = false;
				for (int k = 0; k < touchCounter; k++)
				{
					currTouch = allTouches[k];
					if (currTouch.fingerId != num)
					{
						continue;
					}
					flag2 = true;
					currPressedItem = pressedUIItems[k];
					if (!(currPressedItem != null) || !currPressedItem.isActiveAndEnabled || !(tk2dUIItem2 != null))
					{
						break;
					}
					if (currTouch.phase == TouchPhase.Ended)
					{
						tk2dUIItem2.CurrentOverUIItem(currPressedItem);
						tk2dUIItem2.UpdateTouch(currTouch);
						tk2dUIItem2.Release();
						if (prevPressedUIItemList.Contains(tk2dUIItem2))
						{
							prevPressedUIItemList.Remove(tk2dUIItem2);
						}
					}
					else if (currTouch.phase == TouchPhase.Canceled)
					{
						tk2dUIItem2.CurrentOverUIItem(currPressedItem);
						tk2dUIItem2.UpdateTouch(currTouch);
						tk2dUIItem2.Release();
						if (prevPressedUIItemList.Contains(tk2dUIItem2))
						{
							prevPressedUIItemList.Remove(tk2dUIItem2);
						}
					}
					else
					{
						tk2dUIItem2.CurrentOverUIItem(currPressedItem);
						tk2dUIItem2.UpdateTouch(currTouch);
					}
					break;
				}
				if (!flag2 && tk2dUIItem2 != null)
				{
					tk2dUIItem2.CurrentOverUIItem(null);
					tk2dUIItem2.Release();
					if (prevPressedUIItemList.Contains(tk2dUIItem2))
					{
						prevPressedUIItemList.Remove(tk2dUIItem2);
					}
				}
			}
			else
			{
				prevPressedUIItemList.RemoveAt(num2);
			}
		}
		for (int l = 0; l < touchCounter; l++)
		{
			currPressedItem = pressedUIItems[l];
			currTouch = allTouches[l];
			if (currTouch.phase != TouchPhase.Began)
			{
				continue;
			}
			if (currPressedItem != null && currPressedItem.isActiveAndEnabled)
			{
				result = true;
				if (currPressedItem.Press(currTouch) && !prevPressedUIItemList.Contains(currPressedItem))
				{
					prevPressedUIItemList.Add(currPressedItem);
				}
			}
			flag = true;
		}
		if (flag && this.m_OnAnyPress != null)
		{
			this.m_OnAnyPress?.Invoke();
		}
		return result;
	}

	private void ClearTouches()
	{
		for (int i = 0; i < prevPressedUIItemList.Count; i++)
		{
			if (prevPressedUIItemList[i] != null)
			{
				prevPressedUIItemList[i].Release();
				prevPressedUIItemList[i] = null;
			}
		}
		for (int j = 0; j < pressedUIItems.Length; j++)
		{
			currPressedItem = pressedUIItems[j];
			if (currPressedItem != null)
			{
				currPressedItem.Exit();
				tk2dUIItem parentUIItem = currPressedItem.ParentUIItem;
				while (parentUIItem != null)
				{
					parentUIItem.Exit();
					parentUIItem = parentUIItem.ParentUIItem;
				}
				pressedUIItems[j] = null;
			}
		}
	}

	private tk2dUIItem RaycastForUIItem(Vector2 screenPos)
	{
		int count = sortedCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera2 = sortedCameras[i];
			if (tk2dUICamera2.RaycastType == tk2dUICamera.tk2dRaycastType.Physics3D)
			{
				ray = tk2dUICamera2.HostCamera.ScreenPointToRay(screenPos);
				if (Physics.Raycast(ray, out hit, tk2dUICamera2.HostCamera.farClipPlane - tk2dUICamera2.HostCamera.nearClipPlane, tk2dUICamera2.FilteredMask))
				{
					return hit.collider.GetComponent<tk2dUIItem>();
				}
			}
			else if (tk2dUICamera2.RaycastType == tk2dUICamera.tk2dRaycastType.Physics2D)
			{
				Collider2D collider2D = Physics2D.OverlapPoint(tk2dUICamera2.HostCamera.ScreenToWorldPoint(screenPos), tk2dUICamera2.FilteredMask);
				if (collider2D != null)
				{
					return collider2D.GetComponent<tk2dUIItem>();
				}
			}
		}
		return null;
	}

	public void OverrideClearAllChildrenPresses(tk2dUIItem item)
	{
		if (useMultiTouch)
		{
			for (int i = 0; i < pressedUIItems.Length; i++)
			{
				tk2dUIItem tk2dUIItem2 = pressedUIItems[i];
				if (tk2dUIItem2 != null && item.CheckIsUIItemChildOfMe(tk2dUIItem2))
				{
					tk2dUIItem2.CurrentOverUIItem(item);
				}
			}
		}
		else if (pressedUIItem != null && item.CheckIsUIItemChildOfMe(pressedUIItem))
		{
			pressedUIItem.CurrentOverUIItem(item);
		}
	}

	public bool ReplacePressedUIItem(tk2dUIItem source, tk2dUIItem target)
	{
		if (useMultiTouch)
		{
			for (int i = 0; i < pressedUIItems.Length; i++)
			{
				tk2dUIItem tk2dUIItem2 = pressedUIItems[i];
				if (tk2dUIItem2 != null && tk2dUIItem2 == source)
				{
					tk2dUIItem2.Release();
					pressedUIItems[i] = target;
					return true;
				}
			}
		}
		else if (pressedUIItem != null && pressedUIItem == source)
		{
			pressedUIItem.Release();
			pressedUIItem = target;
			return true;
		}
		return false;
	}
}
