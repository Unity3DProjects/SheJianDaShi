using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class WarningRestoreProgress : GUIPopup
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnButtonClicked;

	private const string TOP_LABEL_TEXT = "ui.popup.settings.restoreprogresswarning.toplabel";

	private const string BOT_LABEL_TEXT = "ui.popup.settings.restoreprogresswarning.botlabel";

	private const string LOAD_TEXT = "ui.popup.settings.restoreprogresswarning.load";

	private const string SAVE_TEXT = "ui.popup.settings.restoreprogresswarning.save";

	[SerializeField]
	private tk2dUIItem yesButton;

	[SerializeField]
	private tk2dUIItem noButton;

	[SerializeField]
	private tk2dTextMesh topLabelDescription;

	[SerializeField]
	private tk2dTextMesh botLabelDescription;

	public static event Action<bool> OnButtonClicked
	{
		add
		{
			Action<bool> action = WarningRestoreProgress.m_OnButtonClicked;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref WarningRestoreProgress.m_OnButtonClicked, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = WarningRestoreProgress.m_OnButtonClicked;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref WarningRestoreProgress.m_OnButtonClicked, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void OnEnable()
	{
		yesButton.OnClick += YesButton_OnClick;
		noButton.OnClick += NoButton_OnClick;
		string textByKey = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.popup.settings.restoreprogresswarning.botlabel");
		string textByKey2 = base.CachedManagers.CachedLocalisationManager.GetTextByKey((SingletonMonoBehaviour<ICloudManager>.Instance.BadChoiceType == ChoiceType.Upload) ? "ui.popup.settings.restoreprogresswarning.save" : "ui.popup.settings.restoreprogresswarning.load");
		botLabelDescription.text = textByKey.Replace("%", textByKey2);
		topLabelDescription.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.popup.settings.restoreprogresswarning.toplabel");
	}

	protected override void OnDisable()
	{
		yesButton.OnClick -= YesButton_OnClick;
		noButton.OnClick -= NoButton_OnClick;
	}

	private void YesButton_OnClick()
	{
		WarningRestoreProgress.m_OnButtonClicked?.Invoke(true);
		base.Hide();
	}

	private void NoButton_OnClick()
	{
		WarningRestoreProgress.m_OnButtonClicked?.Invoke(false);
		base.Hide();
	}
}
