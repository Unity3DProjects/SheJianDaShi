using System;
using UnityEngine;

public class CustomGUI : SingletonMonoBehaviour<CustomGUI>
{
	private class GUIDebug : MonoBehaviour
	{
		private void OnGUI()
		{
			if (NeedShowDebugGUI && onDebugGUI != null)
			{
				// onDebugGUI();
			}
		}
	}

	private static bool needShowDebugGUI;

	private static bool needShowDebugSwitch;

	private static Action onDebugGUI;

	private GUIDebug guiDebug;

	private static bool NeedShowDebugGUI
	{
		get
		{
			return CustomDebug.Enable && needShowDebugGUI;
		}
	}

	public static event Action OnDebugGUI
	{
		add
		{
			if ((bool)SingletonMonoBehaviour<CustomGUI>.Instance)
			{
				onDebugGUI = (Action)Delegate.Combine(onDebugGUI, value);
			}
		}
		remove
		{
			onDebugGUI = (Action)Delegate.Remove(onDebugGUI, value);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		guiDebug = base.gameObject.AddComponent<GUIDebug>();
		guiDebug.enabled = needShowDebugGUI;
	}

	private void Update()
	{
		if (!CustomDebug.Enable)
		{
			return;
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touchCount == 4)
			{
				if (!needShowDebugSwitch)
				{
					needShowDebugSwitch = true;
					needShowDebugGUI = !needShowDebugGUI;
				}
			}
			else if (needShowDebugSwitch)
			{
				needShowDebugSwitch = false;
			}
		}
		else if (Input.GetMouseButtonDown(1))
		{
			if (!needShowDebugSwitch)
			{
				needShowDebugSwitch = true;
				needShowDebugGUI = !needShowDebugGUI;
			}
		}
		else if (needShowDebugSwitch)
		{
			needShowDebugSwitch = false;
		}
		if ((bool)guiDebug)
		{
			guiDebug.enabled = NeedShowDebugGUI;
		}
	}
}
