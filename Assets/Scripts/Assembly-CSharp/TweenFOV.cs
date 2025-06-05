using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TweenFOV : Tweener
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnFovUpdate;

	public float beginFOV;

	public float endFOV;

	private Camera targetCamera;

	public event Action OnFovUpdate
	{
		add
		{
			Action action = this.m_OnFovUpdate;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnFovUpdate, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnFovUpdate;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnFovUpdate, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Awake()
	{
		targetCamera = GetComponent<Camera>();
		base.Awake();
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		targetCamera.fieldOfView = beginFOV * (1f - factor) + endFOV * factor;
	}

	public override void Update()
	{
		this.m_OnFovUpdate?.Invoke();
		base.Update();
	}
}
