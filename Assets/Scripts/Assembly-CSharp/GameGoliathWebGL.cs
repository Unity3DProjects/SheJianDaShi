using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mit_Goliath_Native;
using Mit_ServicesUtils;
using UnityEngine;

public class GameGoliathWebGL : GoliathManager
{
	protected static GameGoliathWebGL instance;

	private const bool LOGS_ARE_ENABLED = false;

	private string user_id;

	private bool sessionGuard;

	private float ticksAtSessionBegin;

	private string sessionId;

	private static int userNum;

	public static GameGoliathWebGL Instance
	{
		get
		{
			return instance;
		}
	}

	private new void Awake()
	{
		base.Awake();
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		sessionId = "session_" + DateTime.Now.Ticks;
		user_id = PluginsUtils.GetUserId();
		SetUserId(user_id);
		SetSessionId(sessionId);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public void SendClientInit(bool isFacebookUser)
	{
		string facebookID = string.Empty;
		string tokenForbusiness = string.Empty;
		if (isFacebookUser)
		{
			facebookID = "facebookID";
			tokenForbusiness = "facebookIDforBusiness";
		}
		PostClientInitEvent(facebookID, tokenForbusiness, user_id, sessionId);
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 50f, 200f, 50f), "StartGoliath"))
		{
			SetUserId(user_id + "_" + ++userNum);
			GoliathStart();
		}
		if (GUI.Button(new Rect(300f, 50f, 200f, 50f), "StopGoliath"))
		{
			GoliathStop();
		}
		if (GUI.Button(new Rect(10f, 150f, 200f, 50f), "AddEvent"))
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("country", "IT");
			dictionary.Add("wallet_nuggets", 12);
			dictionary.Add("wallet_passes", 34);
			PostEvent("init", dictionary);
		}
	}

	[Conditional("UNITY_EDITOR")]
	[Conditional("MC_DEBUG")]
	private static void Log(string logStr)
	{
	}
}
