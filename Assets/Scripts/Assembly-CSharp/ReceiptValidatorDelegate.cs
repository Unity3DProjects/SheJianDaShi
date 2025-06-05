using System;

public interface ReceiptValidatorDelegate
{
	void OnInvalidReceiptCallback(int error, IntPtr receipt);

	void OnUnableToValidateCallback(int error, IntPtr receipt);

	void OnSetSuccessCallback(IntPtr receipt);
}
