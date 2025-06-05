using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class StoreWarningPopup : GUIPopup
{
	private const string popupTitleText = "ui.popup.settings.storeprogresswarning.header";

	private const string popupBodyText = "ui.popup.settings.storeprogresswarning.middlelabel";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnStoreProgressResumed;

	[SerializeField]
	private tk2dUIItem yesButton;

	[SerializeField]
	private tk2dUIItem noButton;

	public static event Action OnStoreProgressResumed
	{
		add
		{
			Action action = StoreWarningPopup.m_OnStoreProgressResumed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StoreWarningPopup.m_OnStoreProgressResumed, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = StoreWarningPopup.m_OnStoreProgressResumed;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref StoreWarningPopup.m_OnStoreProgressResumed, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public void Start()
	{
		tk2dTextMesh[] componentsInChildren = GetComponentsInChildren<tk2dTextMesh>();
		tk2dTextMesh[] array = componentsInChildren;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			if (tk2dTextMesh2.text == "ui.popup.settings.storeprogresswarning.header")
			{
				tk2dTextMesh2.text = "ui.popup.settings.googleplay.storeprogresswarning.header";
			}
			else if (tk2dTextMesh2.text == "ui.popup.settings.storeprogresswarning.middlelabel")
			{
				tk2dTextMesh2.text = "ui.popup.settings.googleplay.storeprogresswarning.middlelabel";
			}
		}
	}

	protected override void OnEnable()
	{
		yesButton.OnClick += YesButton_OnClick;
		noButton.OnClick += NoButton_OnClick;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		yesButton.OnClick -= YesButton_OnClick;
		noButton.OnClick -= NoButton_OnClick;
		base.OnDisable();
	}

	private void YesButton_OnClick()
	{
		SingletonMonoBehaviour<StatisticsManager>.Instance.ICloudStore = ICLoudSaveType.Needed;
		if (StoreWarningPopup.OnStoreProgressResumed != null)
		{
			StoreWarningPopup.OnStoreProgressResumed();
		}
		HideImmediately();
	}

	private void NoButton_OnClick()
	{
		SingletonMonoBehaviour<StatisticsManager>.Instance.ICloudStore = ICLoudSaveType.UnNeeded;
		if (StoreWarningPopup.OnStoreProgressResumed != null)
		{
			StoreWarningPopup.OnStoreProgressResumed();
		}
		HideImmediately();
	}
}
