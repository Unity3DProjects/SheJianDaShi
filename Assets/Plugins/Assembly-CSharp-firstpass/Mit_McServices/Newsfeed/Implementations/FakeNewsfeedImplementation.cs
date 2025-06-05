using Mit_McServices.Newsfeed.ManagerDelegate;
using UnityEngine;

namespace Mit_McServices.Newsfeed.Implementations
{
	public class FakeNewsfeedImplementation : MonoBehaviour, NewsfeedImplementation
	{
		public void StartNewsfeed(NewsfeedManagerDeleagate managerDelegate)
		{
		}

		public bool ShowBoard()
		{
			return false;
		}

		public bool ShowUrgentBoard()
		{
			return false;
		}

		public void DismissBoard()
		{
		}

		public void SetPayingUser(uint isPayingUser)
		{
		}

		public int GetUnreadMessagesCount()
		{
			return 0;
		}

		public int GetUnreadUrgentMessagesCount()
		{
			return 0;
		}

		public int GetMessagesCount()
		{
			return 0;
		}

		public int GetMinimumSessionDuration()
		{
			return 0;
		}

		public void SetMinimumSessionDuration(int duration)
		{
		}

		public int GetUrgentMessagesDelay()
		{
			return 0;
		}

		public void SetUrgentMessagesDelay(int duration)
		{
		}

		public void SetSandBox(uint sandBoxMode)
		{
		}

		public void SetLogsEnabled(bool isEnabled)
		{
		}

		public void SetShowBadge(uint showBadge)
		{
		}

		public bool CanShowUrgentBoard()
		{
			return false;
		}

		public bool IsRunning()
		{
			return false;
		}

		public void Reset()
		{
		}
	}
}
