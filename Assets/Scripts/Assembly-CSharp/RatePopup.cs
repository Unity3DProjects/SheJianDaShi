using System;
using UnityEngine;

public class RatePopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem buttonNoThanks;

	[SerializeField]
	private tk2dUIItem buttonRateIt;

	private MCRateCNativeImplementation.RateBehaviorDelegate rateDelegate;

	protected override void OnEnable()
	{
		base.OnEnable();
		buttonNoThanks.OnClick += ButtonNoThanks_OnClick;
		buttonRateIt.OnClick += ButtonRateIt_OnClick;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		buttonNoThanks.OnClick -= ButtonNoThanks_OnClick;
		buttonRateIt.OnClick -= ButtonRateIt_OnClick;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public void ButtonNoThanks_OnClick()
	{
		Hide();
	}

	public void ButtonRateIt_OnClick()
	{
		rateDelegate();
		Hide();
	}

	public void setRateDelegate(MCRateCNativeImplementation.RateBehaviorDelegate rateCallback)
	{
		rateDelegate = rateCallback;
	}

	public override void BackButtonPressed()
	{
		ButtonNoThanks_OnClick();
	}
}
