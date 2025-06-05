public class Receipt
{
	public string data = string.Empty;

	public string signature = string.Empty;

	public Receipt()
	{
	}

	public Receipt(string data, string signature)
	{
		this.data = data;
		this.signature = signature;
	}
}
