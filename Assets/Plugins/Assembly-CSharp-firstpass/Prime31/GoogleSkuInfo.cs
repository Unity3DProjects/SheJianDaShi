using System.Collections.Generic;

namespace Prime31
{
	public class GoogleSkuInfo
	{
		public string title;

		public string price;

		public string type;

		public string description;

		public string productId;

		public string priceCurrencyCode;

		public long priceAmountMicros;

		public string subscriptionPeriod;

		public string freeTrialPeriod;

		public GoogleSkuInfo()
		{
		}

		public GoogleSkuInfo(Dictionary<string, object> dict)
		{
			if (dict.ContainsKey("title"))
			{
				title = dict["title"] as string;
			}
			if (dict.ContainsKey("price"))
			{
				price = dict["price"] as string;
			}
			if (dict.ContainsKey("type"))
			{
				type = dict["type"] as string;
			}
			if (dict.ContainsKey("description"))
			{
				description = dict["description"] as string;
			}
			if (dict.ContainsKey("productId"))
			{
				productId = dict["productId"] as string;
			}
			if (dict.ContainsKey("price_currency_code"))
			{
				priceCurrencyCode = dict["price_currency_code"] as string;
			}
			if (dict.ContainsKey("price_amount_micros"))
			{
				long? num = dict["price_amount_micros"] as long?;
				if (num.HasValue)
				{
					priceAmountMicros = num.Value;
				}
			}
			if (dict.ContainsKey("subscriptionPeriod"))
			{
				subscriptionPeriod = dict["subscriptionPeriod"] as string;
			}
			if (dict.ContainsKey("freeTrialPeriod"))
			{
				freeTrialPeriod = dict["freeTrialPeriod"] as string;
			}
		}

		public static List<GoogleSkuInfo> fromList(List<object> items)
		{
			List<GoogleSkuInfo> list = new List<GoogleSkuInfo>();
			foreach (Dictionary<string, object> item in items)
			{
				list.Add(new GoogleSkuInfo(item));
			}
			return list;
		}

		public override string ToString()
		{
			return string.Format("<GoogleSkuInfo> title: {0}, price: {1}, type: {2}, description: {3}, productId: {4}, priceCurrencyCode: {5}", title, price, type, description, productId, priceCurrencyCode);
		}
	}
}
