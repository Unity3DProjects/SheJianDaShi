using System;
using UnityEngine;

public class TimerPopup : GUIPopup
{
	private const string ICON_SECOND1 = "1";

	private const string ICON_SECOND2 = "2";

	private const string ICON_SECOND3 = "3";

	[SerializeField]
	private tk2dSprite timerSprite;

	protected override void Update()
	{
		base.Update();
		if (isRealTimerEnable)
		{
			if (realTimerTime < 1f)
			{
				timerSprite.SetSprite("3");
				return;
			}
			if (realTimerTime < 2f)
			{
				timerSprite.SetSprite("2");
				return;
			}
			if (realTimerTime < 3f)
			{
				timerSprite.SetSprite("1");
				return;
			}
			timerSprite.SetSprite("3");
			isRealTimerEnable = false;
			realTimerTime = 0f;
			Hide();
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SingletonMonoBehaviour<GameManager>.Instance.IsGamePaused = false;
		base.Hide(onFinished, immediately, delay, callInvokes);
	}
}
