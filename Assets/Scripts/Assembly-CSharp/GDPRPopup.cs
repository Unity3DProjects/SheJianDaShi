using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public abstract class GDPRPopup : GUIPopup
{
	public static float GDPR_POPUP_Z_ORDER = 160f;

	public static int GDPR_POPUP_ORDER_IN_LAYER = 100;

	[SerializeField]
	private tk2dUIItem AgreeButton;

	[SerializeField]
	public tk2dTextMesh AgreeText;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnAgreedClicked;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnRefusedClicked;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnCheckPrivacyClicked;

	public event Action OnAgreedClicked
	{
		add
		{
			Action action = this.m_OnAgreedClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnAgreedClicked, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnAgreedClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnAgreedClicked, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnRefusedClicked
	{
		add
		{
			Action action = this.m_OnRefusedClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRefusedClicked, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnRefusedClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnRefusedClicked, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnCheckPrivacyClicked
	{
		add
		{
			Action action = this.m_OnCheckPrivacyClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCheckPrivacyClicked, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnCheckPrivacyClicked;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCheckPrivacyClicked, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		AgreeButton.OnClick += OnButtonAgreedClicked;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		AgreeButton.OnClick -= OnButtonAgreedClicked;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.CachedManagers.CachedTK2DUIManager.InputEnabled = true;
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	public override void SetOrderInLayer(int order)
	{
		base.SetOrderInLayer(GDPR_POPUP_ORDER_IN_LAYER);
	}

	public virtual void OnButtonAgreedClicked()
	{
		this.OnAgreedClicked();
	}

	public virtual void OnButtonRefusedClicked()
	{
		this.OnRefusedClicked();
	}

	public virtual void OnButtonCheckPrivacyClicked()
	{
		this.OnCheckPrivacyClicked();
	}

	public override void BackButtonPressed()
	{
	}
}
