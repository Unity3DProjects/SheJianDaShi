using System;

public interface ReceiptValidatorImplementation
{
	IntPtr CreateReceipt(string receiptData, string signature);

	void ValidateReceipt(IntPtr receipt);

	Receipt NativeReceiptToCSharp(IntPtr receipt);

	void SetSandboxMode(bool sandboxMode);

	void Update();
}
