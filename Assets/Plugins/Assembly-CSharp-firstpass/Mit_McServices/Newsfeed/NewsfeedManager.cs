using System.Diagnostics;
using Mit_McServices.Newsfeed.Implementations;
using Mit_McServices.Newsfeed.ManagerDelegate;
using Mit_McServices.Newsfeed.Settings;
using UnityEngine;

namespace Mit_McServices.Newsfeed
{
	public abstract class NewsfeedManager : MonoBehaviour, NewsfeedManagerDeleagate
	{
		private const bool LOGS_ARE_ENABLED = false;

		protected static NewsfeedManager instance;

		private NewsfeedImplementation implementation;

		private bool isShowingNewsfeed;

		public int UnreadNews
		{
			get
			{
				return implementation.GetUnreadMessagesCount();
			}
		}

		public int UnreadUrgents
		{
			get
			{
				return implementation.GetUnreadUrgentMessagesCount();
			}
		}

		public int TotalNews
		{
			get
			{
				return implementation.GetMessagesCount();
			}
		}

		public bool IsShowingNewsfeed
		{
			get
			{
				return isShowingNewsfeed;
			}
		}

		protected void Awake()
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			SetImplementation();
			implementation.StartNewsfeed(this);
		}

		protected void OnDestroy()
		{
			instance = null;
		}

		private void SetImplementation()
		{
			implementation = base.gameObject.AddComponent<CApiNewsfeedImplementation>();
		}

		public bool ShowBoard()
		{
			return implementation.ShowBoard();
		}

		public bool ShowUrgentBoard()
		{
			return implementation.ShowUrgentBoard();
		}

		public void DismissBoard()
		{
			implementation.DismissBoard();
		}

		public void SetPayingUser(uint isPayingUser)
		{
			implementation.SetPayingUser(isPayingUser);
		}

		public int GetUnreadMessagesCount()
		{
			return implementation.GetUnreadMessagesCount();
		}

		public int GetUnreadUrgentMessagesCount()
		{
			return implementation.GetUnreadUrgentMessagesCount();
		}

		public int GetMessagesCount()
		{
			return implementation.GetMessagesCount();
		}

		public int GetMinimumSessionDuration()
		{
			return implementation.GetMinimumSessionDuration();
		}

		public void SetMinimumSessionDuration(int duration)
		{
			implementation.SetMinimumSessionDuration(duration);
		}

		public int GetUrgentMessagesDelay()
		{
			return implementation.GetUrgentMessagesDelay();
		}

		public void SetUrgentMessagesDelay(int duration)
		{
			implementation.SetUrgentMessagesDelay(duration);
		}

		public void SetSandBox(uint sandBoxMode)
		{
			implementation.SetSandBox(sandBoxMode);
		}

		public void SetNativeLogsEnabled(bool isEnabled)
		{
			implementation.SetLogsEnabled(isEnabled);
		}

		public void SetShowBadge(uint showBadge)
		{
			implementation.SetShowBadge(showBadge);
		}

		public bool CanShowUrgentBoard()
		{
			return implementation.CanShowUrgentBoard();
		}

		public bool IsRunning()
		{
			return implementation.IsRunning();
		}

		public void Reset()
		{
			implementation.Reset();
		}

		public NewsfeedSettings GetSettings()
		{
			return Game_GetSettings();
		}

		public void OnBoardWillAppear()
		{
			Game_OnBoardWillAppear();
		}

		public void OnBoardDidAppear()
		{
			isShowingNewsfeed = true;
			Game_OnBoardDidAppear();
		}

		public void OnBoardWillDisappear()
		{
			Game_OnBoardWillDisappear();
		}

		public void OnBoardDidDisappear()
		{
			isShowingNewsfeed = false;
			Game_OnBoardDidDisappear();
		}

		public void OnAvailabilityChanged(uint availability)
		{
			Game_OnAvailabilityChanged(availability);
		}

		public void OnUnreadMessagesCountChanged(uint currentUnreadMessagesCount)
		{
			Game_OnUnreadMessagesCountChanged(currentUnreadMessagesCount);
		}

		public abstract NewsfeedSettings Game_GetSettings();

		public abstract void Game_OnBoardWillAppear();

		public abstract void Game_OnBoardDidAppear();

		public abstract void Game_OnBoardWillDisappear();

		public abstract void Game_OnBoardDidDisappear();

		public abstract void Game_OnAvailabilityChanged(uint availability);

		public abstract void Game_OnUnreadMessagesCountChanged(uint currentUnreadMessagesCount);

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
