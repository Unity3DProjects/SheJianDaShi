using Mit_McServices.Rate.ManagerDelegate;
using UnityEngine;

namespace Mit_McServices.Rate.Implementations
{
	public class FakeRateImplementation : MonoBehaviour, RateImplementation
	{
		void RateImplementation.StartWithDelegate(RateManagerDeleagate managerDelegate)
		{
		}

		bool RateImplementation.CanShowRatePopup()
		{
			return false;
		}

		bool RateImplementation.ShowRatePopup()
		{
			return false;
		}

		bool RateImplementation.ShowRatePopupWithId(string identifier)
		{
			return false;
		}

		void RateImplementation.Disable()
		{
		}

		void RateImplementation.Enable()
		{
		}

		void RateImplementation.ExperienceCompleted()
		{
		}

		bool RateImplementation.RequestIgnoreAppOnBackgroundOnce()
		{
			return false;
		}
	}
}
