using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class GUIOrientationManger : SingletonMonoBehaviour<GUIOrientationManger>
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<GUILayouterRotationType> m_OnDeviceOrientationChanged;

	private GUILayouterRotationType currentRotationType;

	private DeviceOrientation deviceOrientation;

	private ScreenOrientation screenOrientation;

	public GUILayouterRotationType CurrentRotationType
	{
		get
		{
			return currentRotationType;
		}
	}

	public static event Action<GUILayouterRotationType> OnDeviceOrientationChanged
	{
		add
		{
			Action<GUILayouterRotationType> action = GUIOrientationManger.m_OnDeviceOrientationChanged;
			Action<GUILayouterRotationType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GUIOrientationManger.m_OnDeviceOrientationChanged, (Action<GUILayouterRotationType>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<GUILayouterRotationType> action = GUIOrientationManger.m_OnDeviceOrientationChanged;
			Action<GUILayouterRotationType> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref GUIOrientationManger.m_OnDeviceOrientationChanged, (Action<GUILayouterRotationType>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		// if (Screen.height > Screen.width)
		// {
		// 	RotateToPortrait();
		// }
		// else
		// {
			RotateToLandscape();
		// }
	}

	private void Update()
	{
		UpdateGame();
	}

	private void UpdateEditor()
	{
		if (Screen.height > Screen.width)
		{
			if (screenOrientation != ScreenOrientation.Portrait && screenOrientation != ScreenOrientation.PortraitUpsideDown)
			{
				screenOrientation = ScreenOrientation.Portrait;
				RotateToPortrait();
			}
		}
		else if (screenOrientation != ScreenOrientation.LandscapeLeft && screenOrientation != ScreenOrientation.LandscapeRight)
		{
			screenOrientation = ScreenOrientation.LandscapeRight;
			RotateToLandscape();
		}
	}

	private void UpdateGame()
	{
		// if (Screen.width > Screen.height && currentRotationType != GUILayouterRotationType.Landscape)
		// {
			RotateToLandscape();
			currentRotationType = GUILayouterRotationType.Landscape;
		// }
		// else if (Screen.height > Screen.width && currentRotationType != GUILayouterRotationType.Portrait)
		// {
		// 	RotateToPortrait();
		// 	currentRotationType = GUILayouterRotationType.Portrait;
		// }
	}

	public void RotateToLandscape()
	{
		int baseHeight = ScriptableSingleton<SizeHelperSettings>.Instance.baseHeight;
		int baseWidth = ScriptableSingleton<SizeHelperSettings>.Instance.baseWidth;
		ScriptableSingleton<SizeHelperSettings>.Instance.baseHeight = ((baseHeight <= baseWidth) ? baseHeight : baseWidth);
		ScriptableSingleton<SizeHelperSettings>.Instance.baseWidth = ((baseHeight <= baseWidth) ? baseWidth : baseHeight);
		currentRotationType = GUILayouterRotationType.Landscape;
		ScreenDimentions.UpdateUICamera();
		SingletonMonoBehaviour<GUIScreenManager>.Instance.ForceGUIScaleRecalculation();
		GUIOrientationManger.m_OnDeviceOrientationChanged?.Invoke(currentRotationType);
	}

	public void RotateToPortrait()
	{
		int baseHeight = ScriptableSingleton<SizeHelperSettings>.Instance.baseHeight;
		int baseWidth = ScriptableSingleton<SizeHelperSettings>.Instance.baseWidth;
		ScriptableSingleton<SizeHelperSettings>.Instance.baseWidth = ((baseHeight <= baseWidth) ? baseHeight : baseWidth);
		ScriptableSingleton<SizeHelperSettings>.Instance.baseHeight = ((baseHeight <= baseWidth) ? baseWidth : baseHeight);
		currentRotationType = GUILayouterRotationType.Portrait;
		ScreenDimentions.UpdateUICamera();
		SingletonMonoBehaviour<GUIScreenManager>.Instance.ForceGUIScaleRecalculation();
		GUIOrientationManger.m_OnDeviceOrientationChanged?.Invoke(currentRotationType);
	}
}
