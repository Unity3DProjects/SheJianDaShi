using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Prime31
{
	public class GoogleIABManager : AbstractManager
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_billingSupportedEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<string> m_billingNotSupportedEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<List<GooglePurchase>, List<GoogleSkuInfo>> m_queryInventorySucceededEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<string> m_queryInventoryFailedEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<string, string> m_purchaseCompleteAwaitingVerificationEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<GooglePurchase> m_purchaseSucceededEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<string, int> m_purchaseFailedEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<GooglePurchase> m_consumePurchaseSucceededEvent;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<string> m_consumePurchaseFailedEvent;

		public static event Action billingSupportedEvent
		{
			add
			{
				Action action = GoogleIABManager.m_billingSupportedEvent;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_billingSupportedEvent, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = GoogleIABManager.m_billingSupportedEvent;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_billingSupportedEvent, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<string> billingNotSupportedEvent
		{
			add
			{
				Action<string> action = GoogleIABManager.m_billingNotSupportedEvent;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_billingNotSupportedEvent, (Action<string>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<string> action = GoogleIABManager.m_billingNotSupportedEvent;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_billingNotSupportedEvent, (Action<string>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<List<GooglePurchase>, List<GoogleSkuInfo>> queryInventorySucceededEvent
		{
			add
			{
				Action<List<GooglePurchase>, List<GoogleSkuInfo>> action = GoogleIABManager.m_queryInventorySucceededEvent;
				Action<List<GooglePurchase>, List<GoogleSkuInfo>> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_queryInventorySucceededEvent, (Action<List<GooglePurchase>, List<GoogleSkuInfo>>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<List<GooglePurchase>, List<GoogleSkuInfo>> action = GoogleIABManager.m_queryInventorySucceededEvent;
				Action<List<GooglePurchase>, List<GoogleSkuInfo>> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_queryInventorySucceededEvent, (Action<List<GooglePurchase>, List<GoogleSkuInfo>>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<string> queryInventoryFailedEvent
		{
			add
			{
				Action<string> action = GoogleIABManager.m_queryInventoryFailedEvent;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_queryInventoryFailedEvent, (Action<string>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<string> action = GoogleIABManager.m_queryInventoryFailedEvent;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_queryInventoryFailedEvent, (Action<string>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<string, string> purchaseCompleteAwaitingVerificationEvent
		{
			add
			{
				Action<string, string> action = GoogleIABManager.m_purchaseCompleteAwaitingVerificationEvent;
				Action<string, string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_purchaseCompleteAwaitingVerificationEvent, (Action<string, string>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<string, string> action = GoogleIABManager.m_purchaseCompleteAwaitingVerificationEvent;
				Action<string, string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_purchaseCompleteAwaitingVerificationEvent, (Action<string, string>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<GooglePurchase> purchaseSucceededEvent
		{
			add
			{
				Action<GooglePurchase> action = GoogleIABManager.m_purchaseSucceededEvent;
				Action<GooglePurchase> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_purchaseSucceededEvent, (Action<GooglePurchase>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<GooglePurchase> action = GoogleIABManager.m_purchaseSucceededEvent;
				Action<GooglePurchase> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_purchaseSucceededEvent, (Action<GooglePurchase>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<string, int> purchaseFailedEvent
		{
			add
			{
				Action<string, int> action = GoogleIABManager.m_purchaseFailedEvent;
				Action<string, int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_purchaseFailedEvent, (Action<string, int>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<string, int> action = GoogleIABManager.m_purchaseFailedEvent;
				Action<string, int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_purchaseFailedEvent, (Action<string, int>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<GooglePurchase> consumePurchaseSucceededEvent
		{
			add
			{
				Action<GooglePurchase> action = GoogleIABManager.m_consumePurchaseSucceededEvent;
				Action<GooglePurchase> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_consumePurchaseSucceededEvent, (Action<GooglePurchase>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<GooglePurchase> action = GoogleIABManager.m_consumePurchaseSucceededEvent;
				Action<GooglePurchase> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_consumePurchaseSucceededEvent, (Action<GooglePurchase>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<string> consumePurchaseFailedEvent
		{
			add
			{
				Action<string> action = GoogleIABManager.m_consumePurchaseFailedEvent;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_consumePurchaseFailedEvent, (Action<string>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<string> action = GoogleIABManager.m_consumePurchaseFailedEvent;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GoogleIABManager.m_consumePurchaseFailedEvent, (Action<string>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		static GoogleIABManager()
		{
			AbstractManager.initialize(typeof(GoogleIABManager));
		}

		public void billingSupported(string empty)
		{
			GoogleIABManager.billingSupportedEvent.fire();
		}

		public void billingNotSupported(string error)
		{
			GoogleIABManager.billingNotSupportedEvent.fire(error);
		}

		public void queryInventorySucceeded(string json)
		{
			if (GoogleIABManager.queryInventorySucceededEvent != null)
			{
				Dictionary<string, object> dictionary = json.dictionaryFromJson();
				GoogleIABManager.queryInventorySucceededEvent(GooglePurchase.fromList(dictionary["purchases"] as List<object>), GoogleSkuInfo.fromList(dictionary["skus"] as List<object>));
			}
		}

		public void queryInventoryFailed(string error)
		{
			GoogleIABManager.queryInventoryFailedEvent.fire(error);
		}

		public void purchaseCompleteAwaitingVerification(string json)
		{
			if (GoogleIABManager.purchaseCompleteAwaitingVerificationEvent != null)
			{
				Dictionary<string, object> dictionary = json.dictionaryFromJson();
				string arg = dictionary["purchaseData"].ToString();
				string arg2 = dictionary["signature"].ToString();
				GoogleIABManager.purchaseCompleteAwaitingVerificationEvent(arg, arg2);
			}
		}

		public void purchaseSucceeded(string json)
		{
			GoogleIABManager.purchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
		}

		public void purchaseFailed(string json)
		{
			if (GoogleIABManager.purchaseFailedEvent != null)
			{
				Dictionary<string, object> dictionary = Json.decode<Dictionary<string, object>>(json);
				GoogleIABManager.purchaseFailedEvent(dictionary["result"].ToString(), int.Parse(dictionary["response"].ToString()));
			}
		}

		public void consumePurchaseSucceeded(string json)
		{
			if (GoogleIABManager.consumePurchaseSucceededEvent != null)
			{
				GoogleIABManager.consumePurchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
			}
		}

		public void consumePurchaseFailed(string error)
		{
			GoogleIABManager.consumePurchaseFailedEvent.fire(error);
		}
	}
}
