using System;

public class ReceiptValidator
{
	private bool LOGS_ENABLED = true;

	private ReceiptValidatorImplementation implementation;

	public ReceiptValidator()
	{
		implementation = new CnativeReceiptValidatorImplementation();
	}

	public void ValidateReceipt(string receiptData, string signature)
	{
		IntPtr receipt = implementation.CreateReceipt(receiptData, signature);
		implementation.ValidateReceipt(receipt);
	}

	public void SetSandboxMode(bool sandboxMode)
	{
		implementation.SetSandboxMode(sandboxMode);
	}

	public Receipt NativeReceiptToCSharp(IntPtr receipt)
	{
		return implementation.NativeReceiptToCSharp(receipt);
	}

	public void Update()
	{
		implementation.Update();
	}
}
