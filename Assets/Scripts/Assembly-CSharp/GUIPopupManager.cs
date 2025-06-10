using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GUIPopupManager : SingletonMonoBehaviour<GUIPopupManager>
{
	private const float Z_ORDERING_OFFSET = 40f;

	[SerializeField]
	private GUIPopup[] popupsPrefabs;

	private Queue<GUIPopup> popupsQueue = new Queue<GUIPopup>();

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnOverlayedPopupHide;

	private GUIPopup activePopup;

	private GUIPopup oldActive;

	public GUIPopup ActivePopup
	{
		get
		{
			return activePopup;
		}
		private set
		{
			activePopup = value;
			if (activePopup != null)
			{
				AudioManager.PlayAudioWithKey("proposal_appear");
			}
		}
	}

	public static event Action OnOverlayedPopupHide
	{
		add
		{
			Action action = GUIPopupManager.m_OnOverlayedPopupHide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GUIPopupManager.m_OnOverlayedPopupHide, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = GUIPopupManager.m_OnOverlayedPopupHide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GUIPopupManager.m_OnOverlayedPopupHide, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		int num = popupsPrefabs.Length;
		int num2 = MiniclipPrefabs.Instance.miniclipPopupPrefabs.Length;
		Array.Resize(ref popupsPrefabs, num + num2);
		int num3 = num;
		int num4 = 0;
		while (num3 < popupsPrefabs.Length)
		{
			popupsPrefabs[num3] = MiniclipPrefabs.Instance.miniclipPopupPrefabs[num4];
			num3++;
			num4++;
		}
		AdjustGUIScale();
	}

	private void OnEnable()
	{
		LocalisationManager.OnLanguageChanged += LocalisationManager_OnLanguageChanged;
		GUIOrientationManger.OnDeviceOrientationChanged += GUIOrientationManger_OnDeviceOrientationChanged;
	}

	private void OnDisable()
	{
		LocalisationManager.OnLanguageChanged -= LocalisationManager_OnLanguageChanged;
		GUIOrientationManger.OnDeviceOrientationChanged -= GUIOrientationManger_OnDeviceOrientationChanged;
	}

	public GUIPopup ShowPopup(PopupType typePopup, bool force = true, GUIPopup popup = null, bool isOverlayedPopup = false, bool forceOnTop = false)
	{
		Debug.Log($"调用了{typePopup}");
		GUIPopup[] popups = GetComponentsInChildren<GUIPopup>(true);
		int maxOrder = 0;
		for (int i = 0; i < popups.Length; i++)
		{
			if (maxOrder < popups[i].MaxOrderInLayer && !popups[i].ForceOnTop)
			{
				maxOrder = popups[i].MaxOrderInLayer;
			}
		}
		GUIPopup result = ((!popup) ? UnityEngine.Object.Instantiate(PopupWithType(typePopup)) : popup);
		if (result == null)
		{
			CustomDebug.LogError("Can't find popup with id = " + typePopup);
		}
		else
		{
			Vector3 localPosition = result.transform.localPosition;
			result.transform.parent = base.transform;
			result.transform.localScale = Vector3.one;
			result.transform.localPosition = localPosition;
			GUIPopup gUIPopup = result;
			gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(GUIPopup_OnPopupHide));
			if (!force)
			{
				if (popupsQueue.Contains(result) || (ActivePopup != null && ActivePopup.TypePopup == result.TypePopup))
				{
					CustomDebug.LogError("Popup with id allready showed up or inside queue = " + typePopup);
				}
				else if (ActivePopup != null)
				{
					popupsQueue.Enqueue(result);
				}
				else
				{
					ActivePopup = result;
					ActivePopup.Show();
				}
				return result;
			}
			result.Show();
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				result.SetOrderInLayer(maxOrder + 1);
				popups = GetComponentsInChildren<GUIPopup>(true);
				Array.Sort(popups, (GUIPopup gUIPopup2, GUIPopup gUIPopup3) => gUIPopup2.MaxOrderInLayer.CompareTo(gUIPopup3.MaxOrderInLayer));
				for (int num = 0; num < popups.Length; num++)
				{
					if (popups[num].IsNeedToReorderObjects)
					{
						popups[num].CachedTransform.localPosition = Vector3.back * ((!forceOnTop) ? ((float)num * 40f) : 160f);
					}
				}
			}, 0f);
			if ((bool)ActivePopup)
			{
				oldActive = ActivePopup;
				ActivePopup = result;
				result.ForceOnTop = forceOnTop;
				if (!isOverlayedPopup && !oldActive.ForceOnTop)
				{
					oldActive.HideImmediately();
					oldActive = null;
				}
			}
			else
			{
				ActivePopup = result;
			}
		}
		return result;
	}

	public void HidePopup(PopupType typePopup, bool clearQueue = false)
	{
		if ((bool)ActivePopup && ActivePopup.TypePopup == typePopup)
		{
			HideActivePopup(clearQueue);
		}
	}

	public void HideActivePopup(bool clearQueue = false)
	{
		if (clearQueue)
		{
			popupsQueue.Clear();
		}
		if (ActivePopup != null)
		{
			ActivePopup.Hide();
		}
	}

	public void ClearAll()
	{
		popupsQueue.Clear();
		if (ActivePopup != null)
		{
			ActivePopup.HideImmediately();
		}
	}

	public bool IsPopupActive(PopupType type)
	{
		GUIPopup[] array = popupsQueue.ToArray();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].TypePopup == type)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void AdjustGUIScale()
	{
		Vector3 localScale = Vector3.one / GUIScreenManager.GuiScale;
		localScale.z = 1f;
		base.transform.localScale = localScale;
	}

	private GUIPopup PopupWithType(PopupType type)
	{
		for (int i = 0; i < popupsPrefabs.Length; i++)
		{
			GUIPopup gUIPopup = popupsPrefabs[i];
			if (gUIPopup.TypePopup == type)
			{
				return gUIPopup;
			}
		}
		return null;
	}

	private void GUIPopup_OnPopupHide(GUIPopup popup)
	{
		if (!(ActivePopup == popup))
		{
			return;
		}
		GUIPopup gUIPopup = ActivePopup;
		gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(gUIPopup.OnPopupHide, new Action<GUIPopup>(GUIPopup_OnPopupHide));
		GUIPopupManager.m_OnOverlayedPopupHide?.Invoke();
		UnityEngine.Object.Destroy(ActivePopup.gameObject);
		ActivePopup = null;
		if (oldActive != null)
		{
			if (oldActive.gameObject.activeSelf)
			{
				ActivePopup = oldActive;
			}
			else
			{
				UnityEngine.Object.Destroy(oldActive.gameObject);
			}
			oldActive = null;
		}
		else if (popupsQueue.Count > 0)
		{
			GUIPopup gUIPopup2 = popupsQueue.Dequeue();
			ShowPopup(gUIPopup2.TypePopup, false, gUIPopup2);
		}
	}

	private void LocalisationManager_OnLanguageChanged()
	{
		LocaleMeshChanger[] componentsInChildren = GetComponentsInChildren<LocaleMeshChanger>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].RefreshMeshFonts();
		}
		LocaleSpriteChanger[] componentsInChildren2 = GetComponentsInChildren<LocaleSpriteChanger>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].RefreshSpriteFonts();
		}
	}

	private void GUIOrientationManger_OnDeviceOrientationChanged(GUILayouterRotationType orientation)
	{
		AdjustGUIScale();
	}
}
