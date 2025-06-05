using Mit_McServices.Newsfeed.Settings;

namespace Mit_McServices.Newsfeed.ManagerDelegate
{
	public interface NewsfeedManagerDeleagate
	{
		NewsfeedSettings GetSettings();

		void OnBoardWillAppear();

		void OnBoardDidAppear();

		void OnBoardWillDisappear();

		void OnBoardDidDisappear();

		void OnAvailabilityChanged(uint availability);

		void OnUnreadMessagesCountChanged(uint currentUnreadMessagesCount);
	}
}
