using Mit_McServices.Newsfeed.ManagerDelegate;

namespace Mit_McServices.Newsfeed.Implementations
{
	public interface NewsfeedImplementation
	{
		void StartNewsfeed(NewsfeedManagerDeleagate managerDelegate);

		bool ShowBoard();

		bool ShowUrgentBoard();

		void DismissBoard();

		void SetPayingUser(uint isPayingUser);

		int GetUnreadMessagesCount();

		int GetUnreadUrgentMessagesCount();

		int GetMessagesCount();

		int GetMinimumSessionDuration();

		void SetMinimumSessionDuration(int duration);

		int GetUrgentMessagesDelay();

		void SetUrgentMessagesDelay(int duration);

		void SetSandBox(uint sandBoxMode);

		void SetLogsEnabled(bool isEnabled);

		void SetShowBadge(uint showBadge);

		bool CanShowUrgentBoard();

		bool IsRunning();

		void Reset();
	}
}
