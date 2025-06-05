using System.Runtime.InteropServices;
using AOT;

public class MCRateCNativeImplementation
{
	public delegate void EmptyCallbackDelegate();

	public delegate void ShowRatePopupBehaviourDelegate(RateBehaviorDelegate RateBehaviorDelegate, CancelBehaviorDelegate cancelBehavior);

	public delegate void RateBehaviorDelegate();

	public delegate void CancelBehaviorDelegate();

	[DllImport("mcservices")]
	public static extern void mc_rate_startWithDelegate(uint questionLimitPerVersion, double minimumActiveTimePerSession, uint minimumNumberOfSessionsStarted, double minimumDaysBetweenRatePopups, uint minimumPostiveExperiences, string titleText, string messageText, string cancelText, string rateText, EmptyCallbackDelegate ratePopupWillAppearBehaviour, EmptyCallbackDelegate ratePopupDidAppearBehaviour, EmptyCallbackDelegate ratePopupWillDisappearBehaviour, EmptyCallbackDelegate ratePopupDidDisappearBehaviour, ShowRatePopupBehaviourDelegate showRatePopupBehaviour, uint shouldUseIOSNativePopup);

	public static void InitRatePopup()
	{
		mc_rate_startWithDelegate(3u, 20.0, 2u, 0.0, 0u, string.Empty, string.Empty, string.Empty, string.Empty, null, null, null, null, ShowRatePopupBehaviourCallback, 0u);
	}

	[MonoPInvokeCallback(typeof(ShowRatePopupBehaviourDelegate))]
	private static void ShowRatePopupBehaviourCallback(RateBehaviorDelegate rateCallback, CancelBehaviorDelegate cancelCallback)
	{
		MiniclipManager.Instance.AddCallbackToQueue(delegate
		{
			RatePopup ratePopup = SingletonMonoBehaviour<GUIPopupManager>.Instance.ShowPopup(PopupType.Rate) as RatePopup;
			ratePopup.setRateDelegate(rateCallback);
		});
	}
}
