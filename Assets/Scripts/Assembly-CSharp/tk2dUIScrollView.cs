using System;
using UnityEngine;

public class tk2dUIScrollView : MonoBehaviour
{
	[SerializeField]
	private bool isInverse;

	[SerializeField]
	private bool isMapScroll;

	[SerializeField]
	private float speedCoof;

	public float contentLength = 1f;

	public float leftLimitFactor = 1f;

	public float rightLimitFactor = 1f;

	public Transform contentContainer;

	[SerializeField]
	protected tk2dUIItem backgroundBtn;

	public ScrollDirection scrollDirection = ScrollDirection.Vertical;

	public bool momentum = true;

	public bool snap;

	public bool snapFirstLast;

	public int pageSize = -1;

	public float scrollSpeed = 40f;

	public float smoothTime = 0.1f;

	public bool limitedMaxVelocity;

	public float maxVelocity = 50f;

	public float SWIPE_THRESHOLD = 0.02f;

	public float INPUT_CHANGE_APPLY_FACTOR = 0.5f;

	public float SWIPE_VELOCITY_FACTOR = 1f;

	public float TIME_FACTOR_FOR_MOVE_CONTENT_INSIDE_LIMITS = 5f;

	public float TIME_FACTOR_FOR_MOVE_CONTENT_OUTSIDE_LIMITS = 30f;

	public float VELOCITY_FOR_END_MOVE_CONTENT_INSIDE_LIMITS = 1f;

	public float VELOCITY_FOR_END_MOVE_CONTENT_OUTSIDE_LIMITS = 1f;

	public float VELOCITY_FOR_END_MOVE_CONTENT_TO_ONE_OF_LIMITS = 5f;

	public Action OnContentPositionChange;

	public Action<Transform> OnStartSnap;

	public Action<Transform> OnFinishedSnap;

	protected bool isDown;

	protected bool resetChild;

	protected bool isInProgress;

	protected bool isSnap;

	protected float swipeVelocity;

	protected float snapToLimitsVelocity;

	protected float startTouchValue;

	protected float previousTouchWorldValue;

	protected float cachedTouchWorldValue;

	protected float beginContentOffsetValue;

	protected float downOffsetValue;

	protected float fromSnapValue;

	protected float toSnapValue;

	protected float startSnapVelocity;

	protected float currentSnapVelocity;

	protected Transform snapItem;

	private Transform cachedTransform;

	private Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public bool BackgroundBtnEnabled
	{
		get
		{
			return backgroundBtn;
		}
		set
		{
			backgroundBtn.enabled = value;
			BoxCollider component = backgroundBtn.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.enabled = backgroundBtn.enabled;
			}
		}
	}

	public bool IsInverse
	{
		get
		{
			return isInverse;
		}
	}

	public int Axis
	{
		get
		{
			return (int)scrollDirection;
		}
	}

	public int AxisZ
	{
		get
		{
			return (scrollDirection == ScrollDirection.ByZ) ? 1 : Axis;
		}
	}

	protected float ContentOffsetValue
	{
		get
		{
			if (isInverse)
			{
				return 0f - contentContainer.localPosition[Axis];
			}
			return contentContainer.localPosition[Axis];
		}
		set
		{
			Vector3 localPosition = contentContainer.localPosition;
			localPosition[Axis] = ((!isInverse) ? value : (0f - value));
			contentContainer.localPosition = localPosition;
			FireOnContentPositionChangedEvent();
		}
	}

	public void ScrollTo(Transform scrollToItem, float velocity = 20f)
	{
		snapItem = scrollToItem;
		velocity = Abs(velocity);
		if (velocity < 1f)
		{
			velocity = 1f;
		}
		startSnapVelocity = (currentSnapVelocity = velocity);
		fromSnapValue = ContentOffsetValue;
		toSnapValue = 0f - scrollToItem.localPosition[AxisZ];
		swipeVelocity = 0f;
		snapToLimitsVelocity = 0f;
		isDown = (isInProgress = false);
		isSnap = true;
		if (OnStartSnap != null)
		{
			OnStartSnap(snapItem);
		}
	}

	public void ResetContentPosition()
	{
		ContentOffsetValue = 0f;
	}

	public void ContentPositionChangedManually()
	{
		FireOnContentPositionChangedEvent();
	}

	protected virtual void OnEnable()
	{
		if (backgroundBtn != null)
		{
			backgroundBtn.OnDownUIItem += Down;
			backgroundBtn.OnUpUIItem += Up;
		}
		if (tk2dUIManager.Instance != null)
		{
			tk2dUIManager.Instance.OnInputUpdate += OverUpdate;
		}
	}

	protected virtual void OnDisable()
	{
		if (backgroundBtn != null)
		{
			backgroundBtn.OnDownUIItem -= Down;
			backgroundBtn.OnUpUIItem -= Up;
		}
		if (tk2dUIManager.Instance__NoCreate != null)
		{
			tk2dUIManager.Instance.OnInputUpdate -= OverUpdate;
		}
		isDown = false;
		isInProgress = false;
		swipeVelocity = 0f;
		snapToLimitsVelocity = 0f;
	}

	protected virtual void Down(tk2dUIItem btn)
	{
		if (contentLength > float.Epsilon)
		{
			startTouchValue = btn.Touch.position[AxisZ];
			cachedTouchWorldValue = (previousTouchWorldValue = CurrentTouchWorldPosition(btn)[AxisZ]);
			downOffsetValue = (beginContentOffsetValue = ContentOffsetValue);
			swipeVelocity = 0f;
			snapToLimitsVelocity = 0f;
			resetChild = (isInProgress = false);
			isDown = true;
		}
	}

	protected virtual void Up(tk2dUIItem btn)
	{
		if (!isDown)
		{
			return;
		}
		isDown = false;
		isInProgress = true;
		swipeVelocity *= SWIPE_VELOCITY_FACTOR;
		float value = btn.Touch.position[AxisZ] - startTouchValue;
		if (Abs(value) > SwipeThreshold())
		{
			if (!snap)
			{
				return;
			}
			float num = ((scrollDirection != ScrollDirection.Horizontal) ? 0f : (0f - contentLength));
			float num2 = ((scrollDirection != ScrollDirection.Horizontal) ? contentLength : 0f);
			float num3 = CalculateDestinationValue(swipeVelocity, num, num2);
			snapItem = GetChildForSnap(0f - num3);
			if (!OutsideLimits(num, num2, num3) || snapFirstLast)
			{
				swipeVelocity = Abs(swipeVelocity);
				if (swipeVelocity < 1f)
				{
					swipeVelocity = 1f;
				}
				startSnapVelocity = (currentSnapVelocity = swipeVelocity);
				fromSnapValue = ContentOffsetValue;
				toSnapValue = 0f - snapItem.localPosition[AxisZ];
				swipeVelocity = 0f;
				snapToLimitsVelocity = 0f;
				isInProgress = false;
				isSnap = true;
				if (OutsideLimits(num, num2, num3) && snapFirstLast)
				{
					if (num3 < num)
					{
						startSnapVelocity = (currentSnapVelocity = Mathf.Abs(num3 - num));
					}
					else if (num3 > num2)
					{
						startSnapVelocity = (currentSnapVelocity = Mathf.Abs(num3 - num2));
					}
				}
			}
			if (OnStartSnap != null)
			{
				OnStartSnap(snapItem);
			}
		}
		else
		{
			snapItem = GetChildForSnap(0f);
			if (OnFinishedSnap != null)
			{
				OnFinishedSnap(null);
			}
		}
	}

	protected virtual void OverUpdate()
	{
		float num = ContentOffsetValue;
		bool flag = false;
		float num2 = ((scrollDirection != ScrollDirection.Horizontal) ? 0f : (0f - contentLength));
		float num3 = ((scrollDirection != ScrollDirection.Horizontal) ? contentLength : 0f);
		if (isDown)
		{
			num = AddChangeInputValue(num);
			flag = isInProgress;
			if (isInProgress)
			{
				if (OutsideLimits(num2, num, num))
				{
					num -= (num - num2) / leftLimitFactor / 2f;
					flag = true;
					swipeVelocity = 0f;
					if (num >= num2)
					{
						num = num2;
					}
				}
				else if (OutsideLimits(num, num3, num))
				{
					num -= (num - num3) / rightLimitFactor / 2f;
					flag = true;
					swipeVelocity = 0f;
				}
			}
		}
		else if (isSnap)
		{
			currentSnapVelocity = LerpToZero(currentSnapVelocity, tk2dUITime.deltaTime * TIME_FACTOR_FOR_MOVE_CONTENT_INSIDE_LIMITS);
			if (Abs(currentSnapVelocity) < VELOCITY_FOR_END_MOVE_CONTENT_INSIDE_LIMITS)
			{
				currentSnapVelocity = 0f;
			}
			float num4 = 1f - currentSnapVelocity / startSnapVelocity;
			num = Lerp(fromSnapValue, toSnapValue, num4);
			flag = true;
			if (num4 >= 1f)
			{
				isSnap = false;
				swipeVelocity = 0f;
				isInProgress = false;
				if (OnFinishedSnap != null)
				{
					OnFinishedSnap(snapItem);
				}
			}
		}
		else if (isInProgress)
		{
			if (!OutsideLimits(num2, num3, num))
			{
				num += swipeVelocity * tk2dUITime.deltaTime * scrollSpeed;
				flag = true;
				swipeVelocity = LerpToZero(swipeVelocity, tk2dUITime.deltaTime * TIME_FACTOR_FOR_MOVE_CONTENT_INSIDE_LIMITS);
				if (Abs(swipeVelocity) < VELOCITY_FOR_END_MOVE_CONTENT_INSIDE_LIMITS)
				{
					swipeVelocity = 0f;
					isInProgress = false;
				}
			}
			else if (Abs(swipeVelocity) > 0f)
			{
				num += swipeVelocity * tk2dUITime.deltaTime * scrollSpeed;
				flag = true;
				swipeVelocity = LerpToZero(swipeVelocity, tk2dUITime.deltaTime * TIME_FACTOR_FOR_MOVE_CONTENT_OUTSIDE_LIMITS);
				if (Abs(swipeVelocity) < VELOCITY_FOR_END_MOVE_CONTENT_OUTSIDE_LIMITS)
				{
					swipeVelocity = 0f;
				}
			}
			else
			{
				float num5 = ((!(num < num2)) ? num3 : num2);
				num = Mathf.SmoothDamp(num, num5, ref snapToLimitsVelocity, smoothTime, float.PositiveInfinity, tk2dUITime.deltaTime);
				flag = true;
				if (Abs(snapToLimitsVelocity) < VELOCITY_FOR_END_MOVE_CONTENT_TO_ONE_OF_LIMITS)
				{
					num = num5;
					snapToLimitsVelocity = 0f;
					isInProgress = false;
					if (snap && !snapFirstLast && OnFinishedSnap != null)
					{
						OnFinishedSnap(snapItem);
					}
				}
			}
		}
		if (flag)
		{
			ContentOffsetValue = num;
		}
	}

	protected Vector3 CurrentTouchWorldPosition(tk2dUIItem btn)
	{
		Vector3 position = btn.Touch.position;
		Camera uICameraForControl = tk2dUIManager.Instance.GetUICameraForControl(base.gameObject);
		position.z = btn.transform.position.z - uICameraForControl.transform.position.z;
		Vector3 position2 = uICameraForControl.ScreenToWorldPoint(position);
		position2.z = btn.transform.position.z;
		return CachedTransform.InverseTransformPoint(position2);
	}

	protected bool OutsideLimits(float min, float max, float value)
	{
		return value < min || value > max;
	}

	private float Clamp01(float value)
	{
		return (value < 0f) ? 0f : ((!(value > 1f)) ? value : 1f);
	}

	private float Abs(float value)
	{
		return (!(value < 0f)) ? value : (0f - value);
	}

	private float LerpToZero(float from, float factor)
	{
		return from * (1f - Clamp01(factor));
	}

	private float Lerp(float from, float to, float factor)
	{
		factor = Clamp01(factor);
		return from * (1f - factor) + to * factor;
	}

	private float AddChangeInputValue(float source)
	{
		if (momentum)
		{
			float num = CurrentTouchWorldPosition(backgroundBtn)[AxisZ];
			swipeVelocity = num - previousTouchWorldValue;
			if (swipeVelocity == 0f)
			{
				swipeVelocity = num - cachedTouchWorldValue;
			}
			if (limitedMaxVelocity && Abs(swipeVelocity) > maxVelocity)
			{
				swipeVelocity = ((!(swipeVelocity < 0f)) ? maxVelocity : (0f - maxVelocity));
			}
			if (previousTouchWorldValue != num)
			{
				cachedTouchWorldValue = previousTouchWorldValue;
			}
			previousTouchWorldValue = num;
		}
		float num2 = backgroundBtn.Touch.position[AxisZ] - startTouchValue;
		if (!resetChild && Abs(num2) > SwipeThreshold())
		{
			tk2dUIManager.Instance.OverrideClearAllChildrenPresses(backgroundBtn);
			resetChild = true;
		}
		num2 = ((!isMapScroll) ? num2 : (num2 * speedCoof));
		if (!isInProgress && Abs(num2) > SWIPE_THRESHOLD)
		{
			isInProgress = true;
			beginContentOffsetValue -= num2;
		}
		if (isInProgress)
		{
			source = Lerp(source, beginContentOffsetValue + num2, INPUT_CHANGE_APPLY_FACTOR);
		}
		return source;
	}

	protected float CalculateDestinationValue(float velocity, float min, float max)
	{
		float num = ContentOffsetValue;
		float deltaTime = tk2dUITime.deltaTime;
		while (true)
		{
			bool flag = OutsideLimits(min, max, num);
			num += velocity * deltaTime * scrollSpeed;
			velocity = LerpToZero(velocity, deltaTime * ((!flag) ? TIME_FACTOR_FOR_MOVE_CONTENT_INSIDE_LIMITS : TIME_FACTOR_FOR_MOVE_CONTENT_OUTSIDE_LIMITS));
			if (Abs(velocity) < ((!flag) ? VELOCITY_FOR_END_MOVE_CONTENT_INSIDE_LIMITS : VELOCITY_FOR_END_MOVE_CONTENT_OUTSIDE_LIMITS))
			{
				break;
			}
			if (pageSize > 0 && (Mathf.Abs(num - downOffsetValue) > (float)pageSize || flag))
			{
				num -= velocity * deltaTime * scrollSpeed;
				break;
			}
		}
		return num;
	}

	protected virtual float SwipeThreshold()
	{
		return tk2dUIManager.Instance.SwipeThreshold;
	}

	protected virtual Transform GetChildForSnap(float destination)
	{
		Transform transform = null;
		float num = 0f;
		foreach (Transform item in contentContainer)
		{
			if (item.gameObject.activeSelf)
			{
				float num2 = Abs(item.localPosition[AxisZ] - destination);
				if (transform == null || num > num2)
				{
					num = num2;
					transform = item;
				}
			}
		}
		return transform;
	}

	protected virtual void FireOnContentPositionChangedEvent()
	{
		if (OnContentPositionChange != null)
		{
			OnContentPositionChange();
		}
	}

	public void Stop()
	{
		isInProgress = false;
		isDown = false;
		isSnap = false;
	}
}
