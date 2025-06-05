using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;

public class LLDebugManager : SingletonMonoBehaviour<LLDebugManager>
{
	private const string SUPERSONIC_CLASS_NAME = "LLSupersonicManager";

	private const string SUPERSONIC_SHOW_VIDEO_METHOD_NAME = "ShowVideo";

	private const string SUPERSONIC_SHOW_VIDEO_LABEL = "SV";

	private const string SUPERSONIC_SHOW_INTERSTITIAL_METHOD_NAME = "ShowInterstitial";

	private const string SUPERSONIC_SHOW_INTERSTITIAL_LABEL = "SI";

	private const string APPLOVIN_CLASS_NAME = "LLAppLovinManager";

	private const string APPLOVIN_SHOW_VIDEO_METHOD_NAME = "ShowVideo";

	private const string APPLOVIN_SHOW_VIDEO_LABEL = "AV";

	private const string APPLOVIN_SHOW_INTERSTITIAL_METHOD_NAME = "ShowInterstitial";

	private const string APPLOVIN_SHOW_INTERSTITIAL_LABEL = "AI";

	private const string TAPDAQ_CLASS_NAME = "LLTapdaqManager";

	private const string TAPDAQ_SHOW_INTERSTITIAL_METHOD_NAME = "ShowInterstitial";

	private const string TAPDAQ_SHOW_INTERSTITIAL_LABEL = "TI";

	private const string ON_OFF_LABEL = "ON/OFF";

	private bool isNeedToShow = true;

	private uint mb;

	private uint free;

	private float buttonsOffset;

	private float buttonSize;

	private string bundleVersion;

	private string bundleShortVersion;

	private GUIStyle labelsStyle;

	private Rect needToShowRect;

	private Rect fpsRect;

	private Rect usedMemoryRect;

	private Rect drawCallsRect;

	private Rect bundleVersionRect;

	private object[] emptyStringParameter;

	protected override void Awake()
	{
		base.Awake();
		buttonSize = (float)Mathf.Min(ScreenDimentions.Height, ScreenDimentions.Width) / 12.8f;
		labelsStyle = new GUIStyle();
		labelsStyle.fontSize = 30;
		labelsStyle.fontStyle = FontStyle.Bold;
		labelsStyle.normal.textColor = Color.magenta;
		labelsStyle.alignment = TextAnchor.UpperLeft;
		needToShowRect = new Rect(0f, (float)Screen.height - buttonSize, buttonSize, buttonSize);
		fpsRect = new Rect(70f, 30f, 200f, 30f);
		usedMemoryRect = new Rect(70f, 60f, 200f, 30f);
		drawCallsRect = new Rect(70f, 90f, 200f, 30f);
		bundleVersionRect = new Rect(70f, 120f, 200f, 30f);
		emptyStringParameter = new object[1] { string.Empty };
	}

	private void OnEnable()
	{
		CustomGUI.OnDebugGUI += CustomGUI_OnDebugGUI;
	}

	private void OnDisable()
	{
		CustomGUI.OnDebugGUI -= CustomGUI_OnDebugGUI;
	}

	private void CustomGUI_OnDebugGUI()
	{
		if (GUI.Button(needToShowRect, "ON/OFF"))
		{
			isNeedToShow = !isNeedToShow;
		}
		if (isNeedToShow)
		{
			float num = 1f / Time.deltaTime;
			GUI.Label(fpsRect, string.Format("{0:0.} FPS", num), labelsStyle);
			mb = (uint)(Profiler.GetTotalReservedMemoryLong() / 1048576);
			free = 0u;
			GUI.Label(usedMemoryRect, string.Format("{0:0} MB USED / {1:0} MB FREE", mb, free), labelsStyle);
			bundleVersion = "0";
			bundleShortVersion = Application.version;
			GUI.Label(bundleVersionRect, string.Format("{0}({1})", bundleShortVersion, bundleVersion), labelsStyle);
			buttonsOffset = buttonSize;
			ShowGUIButton("LLSupersonicManager", "ShowVideo", "SV");
			ShowGUIButton("LLSupersonicManager", "ShowInterstitial", "SI");
			ShowGUIButton("LLAppLovinManager", "ShowVideo", "AV", emptyStringParameter);
			ShowGUIButton("LLAppLovinManager", "ShowInterstitial", "AI", emptyStringParameter);
			ShowGUIButton("LLTapdaqManager", "ShowInterstitial", "TI");
		}
	}

	private void ShowGUIButton(string className, string methodName, string buttonLabel, object[] parameters = null)
	{
		if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(methodName))
		{
			return;
		}
		Type type = Type.GetType(className);
		if (type == null)
		{
			return;
		}
		MethodInfo method = type.GetMethod(methodName);
		if (method != null)
		{
			if (GUI.Button(new Rect(buttonsOffset, (float)Screen.height - buttonSize, buttonSize, buttonSize), buttonLabel))
			{
				method.Invoke(type, parameters);
			}
			buttonsOffset += buttonSize;
		}
	}
}
