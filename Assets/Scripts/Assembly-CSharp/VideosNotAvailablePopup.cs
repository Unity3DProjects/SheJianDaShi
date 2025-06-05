using System;

public class VideosNotAvailablePopup : GUIPopup
{
	private const float SECONDS_TO_WAIT_TILL_HIDE = 5f;

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected override void Update()
	{
		base.Update();
		if (realTimerTime > 5f)
		{
			Hide();
		}
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Hide(onFinished, immediately, delay, callInvokes);
	}
}
