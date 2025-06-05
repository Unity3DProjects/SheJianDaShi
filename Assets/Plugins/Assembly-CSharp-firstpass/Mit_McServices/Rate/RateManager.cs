using System;
using System.Diagnostics;
using Mit_McServices.Rate.Implementations;
using Mit_McServices.Rate.ManagerDelegate;
using Mit_McServices.Rate.Settings;
using UnityEngine;

namespace Mit_McServices.Rate
{
	public abstract class RateManager : MonoBehaviour, RateManagerDeleagate
	{
		protected const bool LOGS_ARE_ENABLED = false;

		private const string keyNumShownInVersion = "rate_num_shown_version_";

		protected static RateManager instance;

		protected int numPopupsShownInCurrentVersion;

		private RateImplementation implementation;

		private bool isShowingPopup;

		public bool IsShowingPopup
		{
			get
			{
				return isShowingPopup;
			}
		}

		public abstract RateSettings GetSettings();

		protected abstract void CustomRatePopupBehaviour(Action rateBehaviour, Action cancelBehaviour);

		protected abstract bool RateIsEnabled();

		protected abstract bool ConditionsAreMet();

		protected abstract string GetVersionString();

		protected abstract void OnPopupShown();

		protected abstract void OnPopupDismissed();

		protected void Awake()
		{
			instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			isShowingPopup = false;
			SetImplementation();
			numPopupsShownInCurrentVersion = PlayerPrefs.GetInt("rate_num_shown_version_" + GetVersionString(), 0);
		}

		protected void OnDestroy()
		{
			instance = null;
		}

		private void SetImplementation()
		{
			implementation = base.gameObject.AddComponent<CApiRateImplementation>();
		}

		public void Initialize()
		{
			implementation.StartWithDelegate(this);
		}

		public void ShowPopupOnConditions()
		{
			if (RateIsEnabled() && ConditionsAreMet())
			{
				isShowingPopup = implementation.CanShowRatePopup();
				implementation.ShowRatePopup();
			}
		}

		public void ShowPopupOnConditions(string identifier)
		{
			if (RateIsEnabled() && ConditionsAreMet())
			{
				isShowingPopup = implementation.CanShowRatePopup();
				implementation.ShowRatePopupWithId(identifier);
			}
		}

		public void Disable()
		{
			implementation.Disable();
		}

		public void Enable()
		{
			implementation.Enable();
		}

		public void ExperienceCompleted()
		{
			implementation.ExperienceCompleted();
		}

		public bool RequestIgnoreAppOnBackgroundOnce()
		{
			return implementation.RequestIgnoreAppOnBackgroundOnce();
		}

		void RateManagerDeleagate.PopupWillAppearBehaviour()
		{
			isShowingPopup = true;
		}

		void RateManagerDeleagate.PopupDidAppearBehaviour()
		{
			isShowingPopup = true;
			numPopupsShownInCurrentVersion++;
			PlayerPrefs.SetInt("rate_num_shown_version_" + GetVersionString(), numPopupsShownInCurrentVersion);
			OnPopupShown();
		}

		void RateManagerDeleagate.PopupWillDisappearBehaviour()
		{
		}

		void RateManagerDeleagate.PopupDidDisappearBehaviour()
		{
			isShowingPopup = false;
			OnPopupDismissed();
		}

		void RateManagerDeleagate.ShowRatePopupBehaviour(Action rateBehaviour, Action cancelBehaviour)
		{
			CustomRatePopupBehaviour(rateBehaviour, cancelBehaviour);
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
