using System;
using System.Runtime.InteropServices;
using AOT;
using Mit_ServicesUtils;
using UnityEngine;

public class CnativeReceiptValidatorImplementation : ReceiptValidatorImplementation
{
	public delegate void InvalidReceiptDelegate(int error, IntPtr receipt);

	public delegate void UnableToValidateDelegate(int error, IntPtr receipt);

	public delegate void SuccessDelegate(IntPtr receipt);

	private bool LOGS_ENABLED = true;

	private IntPtr currentReceipt;

	private static CallbackEvents callbackEvents = new CallbackEvents();

	public CnativeReceiptValidatorImplementation()
	{
		mc_rv_setInvalidReceiptCallback(InvalidReceiptCallback);
		mc_rv_setUnableToValidateCallback(UnableToValidateCallback);
		mc_rv_setSuccessCallback(SuccessCallback);
	}

	[DllImport("mcservices")]
	private static extern IntPtr mc_rv_createReceipt(string receiptData, string signature);

	[DllImport("mcservices")]
	private static extern void mc_rv_destroyReceipt(IntPtr receipt);

	[DllImport("mcservices")]
	private static extern void mc_rv_validateReceipt(IntPtr receipt);

	[DllImport("mcservices")]
	private static extern void mc_rv_setInvalidReceiptCallback(InvalidReceiptDelegate callback);

	[DllImport("mcservices")]
	private static extern void mc_rv_setUnableToValidateCallback(UnableToValidateDelegate callback);

	[DllImport("mcservices")]
	private static extern void mc_rv_setSuccessCallback(SuccessDelegate callback);

	[DllImport("mcservices")]
	private static extern void mc_rv_setSandboxMode(bool sandboxMode);

	[DllImport("mcservices")]
	private static extern IntPtr mc_rv_getReceiptData(IntPtr receipt);

	[DllImport("mcservices")]
	private static extern IntPtr mc_rv_getReceiptSignature(IntPtr receipt);

	public IntPtr CreateReceipt(string receiptData, string signature)
	{
		currentReceipt = mc_rv_createReceipt(receiptData, signature);
		return currentReceipt;
	}

	public void ValidateReceipt(IntPtr receipt)
	{
		mc_rv_validateReceipt(receipt);
	}

	public void SetSandboxMode(bool sandboxMode)
	{
		mc_rv_setSandboxMode(sandboxMode);
	}

	public Receipt NativeReceiptToCSharp(IntPtr receipt)
	{
		string data = Marshal.PtrToStringAnsi(mc_rv_getReceiptData(receipt));
		string signature = Marshal.PtrToStringAnsi(mc_rv_getReceiptSignature(receipt));
		return new Receipt(data, signature);
	}

	[MonoPInvokeCallback(typeof(InvalidReceiptDelegate))]
	private static void InvalidReceiptCallback(int error, IntPtr receipt)
	{
		callbackEvents.AddEvent(delegate
		{
			SingletonMonoBehaviour<MCInnAppsmanager>.Instance.OnInvalidReceiptCallback(error, receipt);
			mc_rv_destroyReceipt(receipt);
		});
	}

	[MonoPInvokeCallback(typeof(UnableToValidateDelegate))]
	private static void UnableToValidateCallback(int error, IntPtr receipt)
	{
		callbackEvents.AddEvent(delegate
		{
			SingletonMonoBehaviour<MCInnAppsmanager>.Instance.OnUnableToValidateCallback(error, receipt);
			mc_rv_destroyReceipt(receipt);
		});
	}

	[MonoPInvokeCallback(typeof(SuccessDelegate))]
	private static void SuccessCallback(IntPtr receipt)
	{
		Debug.Log("### CnativeReceiptValidatorImplementation SuccessCallback");
		callbackEvents.AddEvent(delegate
		{
			SingletonMonoBehaviour<MCInnAppsmanager>.Instance.OnSetSuccessCallback(receipt);
			mc_rv_destroyReceipt(receipt);
		});
	}

	public void Update()
	{
		callbackEvents.TryInvokeEvents();
	}
}
