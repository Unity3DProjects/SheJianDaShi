using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mit_ServicesUtils
{
	public class CallbackEvents
	{
		private const bool LOGS_ARE_ENABLED = false;

		private object thisLock = new object();

		private Queue<Action> eventsQueue = new Queue<Action>();

		public void AddEvent(Action evt)
		{
			lock (thisLock)
			{
				if (evt != null)
				{
					eventsQueue.Enqueue(evt);
				}
			}
		}

		public void TryInvokeEvents()
		{
			lock (thisLock)
			{
				while (eventsQueue.Count > 0)
				{
					eventsQueue.Dequeue()();
				}
			}
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
