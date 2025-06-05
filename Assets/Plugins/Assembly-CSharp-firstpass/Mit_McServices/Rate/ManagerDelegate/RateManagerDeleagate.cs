using System;
using Mit_McServices.Rate.Settings;

namespace Mit_McServices.Rate.ManagerDelegate
{
	public interface RateManagerDeleagate
	{
		RateSettings GetSettings();

		void PopupWillAppearBehaviour();

		void PopupDidAppearBehaviour();

		void PopupWillDisappearBehaviour();

		void PopupDidDisappearBehaviour();

		void ShowRatePopupBehaviour(Action rateBehaviour, Action cancelBehaviour);
	}
}
