using System;
using Mit_ServicesUtils;
using UnityEngine;

public class MiniclipManager : MonoBehaviour
{
	private static MiniclipManager _instance;

	private CallbackEvents _callbackEvents;

	public GameObject SchedulerObjTag;

	public static MiniclipManager Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		_callbackEvents = new CallbackEvents();
	}

	private void Start()
	{
		MCRateCNativeImplementation.InitRatePopup();
		SchedulerObjTag = new GameObject();
	}

	private void Update()
	{
		_callbackEvents.TryInvokeEvents();
	}

	public void AddCallbackToQueue(Action evt)
	{
		_callbackEvents.AddEvent(evt);
	}
}
