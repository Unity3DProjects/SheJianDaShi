using System.Diagnostics;
using System.Runtime.InteropServices;
using AOT;
using Mit_McServices.Newsfeed.ManagerDelegate;
using Mit_McServices.Newsfeed.Settings;
using Mit_ServicesUtils;
using UnityEngine;

namespace Mit_McServices.Newsfeed.Implementations
{
	public class CApiNewsfeedImplementation : MonoBehaviour, NewsfeedImplementation
	{
		public delegate void BoardFlowDelegate();

		public delegate void AvailabilityChangedDelegate(uint availability);

		public delegate void UnreadMessagesCountChangedDelegate(uint currentUnreadMessagesCount);

		private const bool LOGS_ARE_ENABLED = false;

		private static NewsfeedManagerDeleagate manager;

		private static CallbackEvents callbackEvents = new CallbackEvents();

		[DllImport("mcprime")]
		public static extern void mc_connection_load_helper();

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_start(string applicationID);

		[DllImport("mcservices")]
		public static extern uint mc_newsfeed_showBoard();

		[DllImport("mcservices")]
		public static extern uint mc_newsfeed_showUrgentBoard();

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_dismissBoard();

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setPayingUser(uint isPayingUser);

		[DllImport("mcservices")]
		public static extern uint mc_newsfeed_unreadMessagesCount();

		[DllImport("mcservices")]
		public static extern uint mc_newsfeed_unreadUrgentMessagesCount();

		[DllImport("mcservices")]
		public static extern uint mc_newsfeed_messagesCount();

		[DllImport("mcservices")]
		public static extern int mc_newsfeed_minimumSessionDuration();

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setMinimumSessionDuration(int duration);

		[DllImport("mcservices")]
		public static extern int mc_newsfeed_urgentMessagesDelay();

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setUrgentMessagesDelay(int duration);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setSandBox(uint sandBoxMode);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setLogsEnabled(uint enabled);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setShowBadge(uint showBadge);

		[DllImport("mcservices")]
		public static extern uint mc_newsfeed_canShowUrgentBoard();

		[DllImport("mcservices")]
		public static extern uint mc_newsfeed_isRunning();

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_reset();

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setNewsfeedBoardWillAppearCallback(BoardFlowDelegate callback);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setNewsfeedBoardDidAppearCallback(BoardFlowDelegate callback);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setNewsfeedBoardWillDisappearCallback(BoardFlowDelegate callback);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setNewsfeedBoardDidDisappearCallback(BoardFlowDelegate callback);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setNewsfeedAvailabilityChangedCallback(AvailabilityChangedDelegate callback);

		[DllImport("mcservices")]
		public static extern void mc_newsfeed_setNewsfeedUnreadMessagesCountChangedCallback(UnreadMessagesCountChangedDelegate callback);

		public void StartNewsfeed(NewsfeedManagerDeleagate managerDelegate)
		{
			mc_connection_load_helper();
			manager = managerDelegate;
			NewsfeedSettings settings = manager.GetSettings();
			mc_newsfeed_setNewsfeedBoardWillAppearCallback(BoardWillAppearCallback);
			mc_newsfeed_setNewsfeedBoardDidAppearCallback(BoardDidAppearCallback);
			mc_newsfeed_setNewsfeedBoardWillDisappearCallback(BoardWillDisappearCallback);
			mc_newsfeed_setNewsfeedBoardDidDisappearCallback(BoardDidDisappearCallback);
			mc_newsfeed_setNewsfeedAvailabilityChangedCallback(AvailabilityChangedCallback);
			mc_newsfeed_setNewsfeedUnreadMessagesCountChangedCallback(UnreadMessagesCountChangedCallback);
			mc_newsfeed_setSandBox(BoolToUint(settings.IsSandbox));
			mc_newsfeed_setShowBadge(BoolToUint(settings.HasToShowBadge));
			mc_newsfeed_setMinimumSessionDuration(settings.MinimumSessionDuration);
			mc_newsfeed_start(settings.ApplicationId);
		}

		private static uint BoolToUint(bool inputVal)
		{
			return inputVal ? 1u : 0u;
		}

		private static bool UintToBool(uint inputVal)
		{
			return inputVal != 0;
		}

		private static int UintToInt(uint inputVal)
		{
			if (inputVal > uint.MaxValue)
			{
				return int.MaxValue;
			}
			return (int)inputVal;
		}

		public bool ShowBoard()
		{
			return UintToBool(mc_newsfeed_showBoard());
		}

		public bool ShowUrgentBoard()
		{
			return UintToBool(mc_newsfeed_showUrgentBoard());
		}

		public void DismissBoard()
		{
			mc_newsfeed_dismissBoard();
		}

		public void SetPayingUser(uint isPayingUser)
		{
			mc_newsfeed_setPayingUser(isPayingUser);
		}

		public int GetUnreadMessagesCount()
		{
			return UintToInt(mc_newsfeed_unreadMessagesCount());
		}

		public int GetUnreadUrgentMessagesCount()
		{
			return UintToInt(mc_newsfeed_unreadUrgentMessagesCount());
		}

		public int GetMessagesCount()
		{
			return UintToInt(mc_newsfeed_messagesCount());
		}

		public int GetMinimumSessionDuration()
		{
			return mc_newsfeed_minimumSessionDuration();
		}

		public void SetMinimumSessionDuration(int duration)
		{
			mc_newsfeed_setMinimumSessionDuration(duration);
		}

		public int GetUrgentMessagesDelay()
		{
			return mc_newsfeed_urgentMessagesDelay();
		}

		public void SetUrgentMessagesDelay(int duration)
		{
			mc_newsfeed_setUrgentMessagesDelay(duration);
		}

		public void SetSandBox(uint sandBoxMode)
		{
			mc_newsfeed_setSandBox(sandBoxMode);
		}

		public void SetLogsEnabled(bool isEnabled)
		{
			mc_newsfeed_setLogsEnabled(BoolToUint(isEnabled));
		}

		public void SetShowBadge(uint showBadge)
		{
			mc_newsfeed_setShowBadge(showBadge);
		}

		public bool CanShowUrgentBoard()
		{
			return UintToBool(mc_newsfeed_canShowUrgentBoard());
		}

		public bool IsRunning()
		{
			return UintToBool(mc_newsfeed_isRunning());
		}

		public void Reset()
		{
			mc_newsfeed_reset();
		}

		[MonoPInvokeCallback(typeof(BoardFlowDelegate))]
		private static void BoardWillAppearCallback()
		{
			callbackEvents.AddEvent(manager.OnBoardWillAppear);
		}

		[MonoPInvokeCallback(typeof(BoardFlowDelegate))]
		private static void BoardDidAppearCallback()
		{
			callbackEvents.AddEvent(manager.OnBoardDidAppear);
		}

		[MonoPInvokeCallback(typeof(BoardFlowDelegate))]
		private static void BoardWillDisappearCallback()
		{
			callbackEvents.AddEvent(manager.OnBoardWillDisappear);
		}

		[MonoPInvokeCallback(typeof(BoardFlowDelegate))]
		private static void BoardDidDisappearCallback()
		{
			callbackEvents.AddEvent(manager.OnBoardDidDisappear);
		}

		[MonoPInvokeCallback(typeof(AvailabilityChangedDelegate))]
		private static void AvailabilityChangedCallback(uint availability)
		{
			callbackEvents.AddEvent(delegate
			{
				manager.OnAvailabilityChanged(availability);
			});
		}

		[MonoPInvokeCallback(typeof(UnreadMessagesCountChangedDelegate))]
		private static void UnreadMessagesCountChangedCallback(uint currentUnreadMessagesCount)
		{
			callbackEvents.AddEvent(delegate
			{
				manager.OnUnreadMessagesCountChanged(currentUnreadMessagesCount);
			});
		}

		private void Update()
		{
			callbackEvents.TryInvokeEvents();
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
