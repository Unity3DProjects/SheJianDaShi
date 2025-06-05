using System.Collections.Generic;

namespace Prime31
{
	public class GooglePurchase
	{
		public enum GooglePurchaseState
		{
			Purchased = 0,
			Canceled = 1,
			Refunded = 2
		}

		public string packageName;

		public string orderId;

		public string productId;

		public string developerPayload;

		public string type;

		public long purchaseTime;

		public GooglePurchaseState purchaseState;

		public string purchaseToken;

		public string signature;

		public string originalJson;

		public GooglePurchase()
		{
		}

		public GooglePurchase(Dictionary<string, object> dict)
		{
			if (dict.ContainsKey("packageName"))
			{
				packageName = dict["packageName"].ToString();
			}
			if (dict.ContainsKey("orderId"))
			{
				orderId = dict["orderId"].ToString();
			}
			if (dict.ContainsKey("productId"))
			{
				productId = dict["productId"].ToString();
			}
			if (dict.ContainsKey("developerPayload"))
			{
				developerPayload = dict["developerPayload"].ToString();
			}
			if (dict.ContainsKey("type"))
			{
				type = dict["type"] as string;
			}
			if (dict.ContainsKey("purchaseTime"))
			{
				purchaseTime = long.Parse(dict["purchaseTime"].ToString());
			}
			if (dict.ContainsKey("purchaseState"))
			{
				purchaseState = (GooglePurchaseState)int.Parse(dict["purchaseState"].ToString());
			}
			if (dict.ContainsKey("purchaseToken"))
			{
				purchaseToken = dict["purchaseToken"].ToString();
			}
			if (dict.ContainsKey("signature"))
			{
				signature = dict["signature"].ToString();
			}
			if (dict.ContainsKey("originalJson"))
			{
				originalJson = dict["originalJson"].ToString();
			}
		}

		public static List<GooglePurchase> fromList(List<object> items)
		{
			List<GooglePurchase> list = new List<GooglePurchase>();
			foreach (Dictionary<string, object> item in items)
			{
				list.Add(new GooglePurchase(item));
			}
			return list;
		}

		public override string ToString()
		{
			return string.Format("<GooglePurchase> packageName: {0}, orderId: {1}, productId: {2}, developerPayload: {3}, purchaseToken: {4}, purchaseState: {5}, signature: {6}, type: {7}, json: {8}", packageName, orderId, productId, developerPayload, purchaseToken, purchaseState, signature, type, originalJson);
		}
	}
}
