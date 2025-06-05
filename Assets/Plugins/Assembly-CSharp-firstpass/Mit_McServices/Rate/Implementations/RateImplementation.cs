using Mit_McServices.Rate.ManagerDelegate;

namespace Mit_McServices.Rate.Implementations
{
	public interface RateImplementation
	{
		void StartWithDelegate(RateManagerDeleagate managerDelegate);

		bool CanShowRatePopup();

		bool ShowRatePopup();

		bool ShowRatePopupWithId(string identifier);

		void Disable();

		void Enable();

		void ExperienceCompleted();

		bool RequestIgnoreAppOnBackgroundOnce();
	}
}
