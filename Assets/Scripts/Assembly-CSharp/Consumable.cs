public class Consumable
{
	private string productId;

	private Receipt receipt;

	private int consumeAttempts;

	public int ConsumeAttempts
	{
		get
		{
			return consumeAttempts;
		}
		set
		{
			consumeAttempts = value;
		}
	}

	public Receipt Receipt
	{
		get
		{
			return receipt;
		}
	}

	public string ProductId
	{
		get
		{
			return productId;
		}
	}

	public Consumable(string productId, Receipt receipt, int consumeAttempts)
	{
		this.productId = productId;
		this.receipt = receipt;
		this.consumeAttempts = consumeAttempts;
	}
}
