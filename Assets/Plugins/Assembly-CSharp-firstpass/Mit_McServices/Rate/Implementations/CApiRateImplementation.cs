using System.Diagnostics;
using System.Runtime.InteropServices;
using AOT;
using Mit_McServices.Rate.ManagerDelegate;
using Mit_McServices.Rate.Settings;
using Mit_ServicesUtils;
using UnityEngine;

namespace Mit_McServices.Rate.Implementations
{
	public class CApiRateImplementation : MonoBehaviour, RateImplementation
	{
		public delegate void RateItPopupFlowDelegate();

		public delegate void RateItPopupBehaviour(RateItPopupFlowDelegate rateBehaviour, RateItPopupFlowDelegate cancelBehaviour);

		private const bool LOGS_ARE_ENABLED = false;

		private static RateManagerDeleagate manager;

		private static CallbackEvents callbackEvents = new CallbackEvents();

		[DllImport("mcprime")]
		public static extern void mc_connection_load_helper();

		[DllImport("mcservices")]
		public static extern void mc_rate_startWithDelegate(uint questionLimitPerVersion, double minimumActiveTimePerSession, uint minimumNumberOfSessionsStarted, double minimumDaysBetweenRatePopups, uint minimumPostiveExperiences, string titleText, string messageText, string cancelText, string rateText, RateItPopupFlowDelegate ratePopupWillAppearBehaviour, RateItPopupFlowDelegate ratePopupDidAppearBehaviour, RateItPopupFlowDelegate ratePopupWillDisappearBehaviour, RateItPopupFlowDelegate ratePopupDidDisappearBehaviour, RateItPopupBehaviour showRatePopupBehaviour, uint shouldUseIOSNativePopup);

		[DllImport("mcservices")]
		public static extern bool mc_rate_canShowRatePopup();

		[DllImport("mcservices")]
		public static extern bool mc_rate_showRatePopup();

		[DllImport("mcservices")]
		public static extern bool mc_rate_showRatePopupWithId(string identifier);

		[DllImport("mcservices")]
		public static extern void mc_rate_disable();

		[DllImport("mcservices")]
		public static extern void mc_rate_enable();

		[DllImport("mcservices")]
		public static extern void mc_rate_experienceCompleted();

		[DllImport("mcservices")]
		public static extern bool mc_rate_requestIgnoreAppOnBackgroundOnce();

		void RateImplementation.StartWithDelegate(RateManagerDeleagate managerDelegate)
		{
			manager = managerDelegate;
			mc_connection_load_helper();
			RateSettings settings = manager.GetSettings();
			RateItPopupBehaviour showRatePopupBehaviour = null;
			if (!settings.WillUseNativePopup)
			{
				showRatePopupBehaviour = RateItPopupBehaviourCallback;
			}
			mc_rate_startWithDelegate((uint)settings.QuestionLimitPerVersion, settings.MinimumActiveTimePerSession, (uint)settings.MinimumSessionStarts, settings.MinimumDaysInterval, (uint)settings.MinimumPositiveExperiences, settings.Texts.Title, settings.Texts.Message, settings.Texts.LaterButton, settings.Texts.OkButton, RateItPopupWillAppearCallback, RateItPopupDidAppearCallback, RatePopupWillDisappearCallback, RateItPopupDidDisappearCallback, showRatePopupBehaviour, settings.ShouldUseIOSNativePopup ? 1u : 0u);
		}

		bool RateImplementation.CanShowRatePopup()
		{
			return mc_rate_canShowRatePopup();
		}

		bool RateImplementation.ShowRatePopup()
		{
			return mc_rate_showRatePopup();
		}

		bool RateImplementation.ShowRatePopupWithId(string identifier)
		{
			return mc_rate_showRatePopupWithId(identifier);
		}

		void RateImplementation.Disable()
		{
			mc_rate_disable();
		}

		void RateImplementation.Enable()
		{
			mc_rate_enable();
		}

		void RateImplementation.ExperienceCompleted()
		{
			mc_rate_experienceCompleted();
		}

		bool RateImplementation.RequestIgnoreAppOnBackgroundOnce()
		{
			return mc_rate_requestIgnoreAppOnBackgroundOnce();
		}

		[MonoPInvokeCallback(typeof(RateItPopupFlowDelegate))]
		private static void RateItPopupWillAppearCallback()
		{
			if (callbackEvents != null && manager != null)
			{
				callbackEvents.AddEvent(manager.PopupWillAppearBehaviour);
			}
		}

		[MonoPInvokeCallback(typeof(RateItPopupFlowDelegate))]
		private static void RateItPopupDidAppearCallback()
		{
			if (callbackEvents != null && manager != null)
			{
				callbackEvents.AddEvent(manager.PopupDidAppearBehaviour);
			}
		}

		[MonoPInvokeCallback(typeof(RateItPopupFlowDelegate))]
		private static void RatePopupWillDisappearCallback()
		{
			if (callbackEvents != null && manager != null)
			{
				callbackEvents.AddEvent(manager.PopupWillDisappearBehaviour);
			}
		}

		[MonoPInvokeCallback(typeof(RateItPopupFlowDelegate))]
		private static void RateItPopupDidDisappearCallback()
		{
			if (callbackEvents != null && manager != null)
			{
				callbackEvents.AddEvent(manager.PopupDidDisappearBehaviour);
			}
		}

		[MonoPInvokeCallback(typeof(RateItPopupBehaviour))]
		private static void RateItPopupBehaviourCallback(RateItPopupFlowDelegate rateBehaviour, RateItPopupFlowDelegate cancelBehaviour)
		{
			if (callbackEvents == null || manager == null)
			{
				return;
			}
			callbackEvents.AddEvent(delegate
			{
				manager.ShowRatePopupBehaviour(delegate
				{
					rateBehaviour();
				}, delegate
				{
					cancelBehaviour();
				});
			});
		}

		private void Update()
		{
			if (callbackEvents != null)
			{
				callbackEvents.TryInvokeEvents();
			}
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
