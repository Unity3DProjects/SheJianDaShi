public class LLStoreProduct
{
	public string id;

	public string title;

	public string localizedPrice;

	public long priceInMicros;

	public bool isSubscription;

	public bool IsFreeTrial
	{
		get
		{
			return priceInMicros == 0 && isSubscription;
		}
	}

	public LLStoreProduct(string id, string title, string localizedPrice, long priceInMicros, bool isSubscription)
	{
		this.id = id;
		this.title = title;
		this.localizedPrice = localizedPrice;
		this.priceInMicros = priceInMicros;
		this.isSubscription = isSubscription;
	}
}
