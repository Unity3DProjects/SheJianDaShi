using System;
using UnityEngine;

public class UnityLogsReceiptValidatorImplementation : ReceiptValidatorImplementation
{
	private bool LOGS_ENABLED = true;

	public UnityLogsReceiptValidatorImplementation()
	{
		Log("InitReceiptValidatorDelegate");
	}

	public IntPtr CreateReceipt(string receiptData, string signature)
	{
		Log("CreateReceipt(" + receiptData + ", " + signature + ")");
		return default(IntPtr);
	}

	public void ValidateReceipt(IntPtr receipt)
	{
		Log("ValidateReceipt(" + receipt + ")");
	}

	public Receipt NativeReceiptToCSharp(IntPtr receipt)
	{
		Log("NativeReceiptToCSharp(" + receipt + ")");
		return new Receipt(string.Empty, string.Empty);
	}

	public void SetSandboxMode(bool sandboxMode)
	{
		Log("SetSandboxMode(" + ((!sandboxMode) ? "false" : "true") + ")");
	}

	private void Log(string message)
	{
		if (LOGS_ENABLED)
		{
			Debug.Log("### UnityLogsReceiptValidatorImplementation: " + message);
		}
	}

	public void Update()
	{
	}
}
