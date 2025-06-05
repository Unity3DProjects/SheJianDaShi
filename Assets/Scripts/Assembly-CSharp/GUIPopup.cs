using System;
using GooglePlayGames;
using UnityEngine;

public class GUIPopup : GUIControl
{
	public Action<GUIPopup> OnPopupHide;

	[SerializeField]
	private PopupType typePopup;

	[SerializeField]
	private BoxCollider touchBlocker;

	[SerializeField]
	private tk2dUIItem closeButton;

	[SerializeField]
	private bool useRealTimer;

	[SerializeField]
	private bool isNeedToReorderObjects = true;

	protected bool isRealTimerEnable;

	protected float realTimerTime;

	private float realTime;

	private float lastRealTime;

	private float actual;

	private Managers cachedManagers;

	private ObjectsSpawner objectsSpawner;

	public bool ForceOnTop;

	public PopupType TypePopup
	{
		get
		{
			return typePopup;
		}
	}

	protected Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	protected ObjectsSpawner ObjectsSpawner
	{
		get
		{
			if (objectsSpawner == null)
			{
				objectsSpawner = CachedManagers.CachedLevelsManager.ObjectsSpawner;
			}
			return objectsSpawner;
		}
	}

	public int MaxOrderInLayer { get; protected set; }

	public bool IsNeedToReorderObjects
	{
		get
		{
			return isNeedToReorderObjects;
		}
	}

	protected virtual void OnEnable()
	{
		if (closeButton != null)
		{
			closeButton.OnClick += CloseButton_OnClick;
		}
	}

	protected virtual void Update()
	{
		if (isRealTimerEnable)
		{
			realTime = Time.realtimeSinceStartup;
			float num = realTime - lastRealTime;
			actual += ((!(num < 0f)) ? num : 0f);
			num = 1E-05f * (float)(int)(actual * 100000f);
			actual -= num;
			if (num > 1f)
			{
				num = 1f;
			}
			lastRealTime = realTime;
			realTimerTime += num;
		}
	}

	protected virtual void OnDisable()
	{
		if (closeButton != null)
		{
			closeButton.OnClick -= CloseButton_OnClick;
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		if (touchBlocker != null)
		{
			touchBlocker.enabled = true;
		}
		if (useRealTimer)
		{
			isRealTimerEnable = true;
			lastRealTime = Time.realtimeSinceStartup;
			realTimerTime = 0f;
		}
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public virtual void SetOrderInLayer(int order)
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		Array.Sort(componentsInChildren, (Renderer renderer1, Renderer renderer2) => renderer1.sortingOrder.CompareTo(renderer2.sortingOrder));
		int num = 0;
		for (int num2 = 0; num2 < componentsInChildren.Length; num2++)
		{
			componentsInChildren[num2].sortingLayerName = "Overlayer";
			if (isNeedToReorderObjects)
			{
				if (num2 == 0)
				{
					num = componentsInChildren[num2].sortingOrder;
					componentsInChildren[num2].sortingOrder = order;
				}
				else if (componentsInChildren[num2].sortingOrder == num)
				{
					componentsInChildren[num2].sortingOrder = componentsInChildren[num2 - 1].sortingOrder;
				}
				else
				{
					num = componentsInChildren[num2].sortingOrder;
					componentsInChildren[num2].sortingOrder = componentsInChildren[num2 - 1].sortingOrder + 1;
				}
			}
			MaxOrderInLayer = componentsInChildren[num2].sortingOrder;
		}
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		tk2dUIManager.Instance.InputEnabled = false;
		if (touchBlocker != null)
		{
			touchBlocker.enabled = false;
		}
		Action<GUIControl> onFinished2 = delegate(GUIControl c)
		{
			if (onFinished != null)
			{
				onFinished(c);
			}
			tk2dUIManager.Instance.InputEnabled = true;
			base.gameObject.SetActive(false);
			if (OnPopupHide != null)
			{
				OnPopupHide(this);
			}
		};
		if (useRealTimer)
		{
			isRealTimerEnable = false;
			realTimerTime = 0f;
		}
		base.Hide(onFinished2, immediately, delay, callInvokes);
	}

	protected virtual void CloseButton_OnClick()
	{
		Hide();
	}

	public override void BackButtonPressed()
	{
		CloseButton_OnClick();
		if (TypePopup == PopupType.MatchmakingWaiting)
		{
			PlayGamesPlatform.Instance.RealTime.LeaveRoom();
		}
	}
}
