using System;
using UnityEngine;

public class GUIProposalMenu : GUILayouter
{
	public Action<GUIProposalMenu> OnProposalClick;

	[SerializeField]
	private ProposalMenuType typeProposal;

	[SerializeField]
	protected tk2dUIItem mainButton;

	[SerializeField]
	protected Transform iconCellTransform;

	[SerializeField]
	protected EffectType effectType;

	protected GameObject currentEffect;

	public ProposalMenuType TypeProposal
	{
		get
		{
			return typeProposal;
		}
	}

	public bool IsActive
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if ((bool)mainButton)
		{
			mainButton.OnClick += MainButton_OnClick;
		}
	}

	protected virtual void OnDisable()
	{
		if ((bool)mainButton)
		{
			mainButton.OnClick -= MainButton_OnClick;
		}
	}

	public virtual void Activate()
	{
		base.gameObject.SetActive(true);
		Initialize();
		if (iconCellTransform != null)
		{
			if (currentEffect != null)
			{
				UnityEngine.Object.Destroy(currentEffect);
				currentEffect = null;
			}
			if (effectType != EffectType.None)
			{
				currentEffect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(effectType));
				currentEffect.transform.parent = iconCellTransform;
				currentEffect.transform.localPosition = Vector3.zero;
			}
		}
	}

	public virtual void Deactivate()
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				base.gameObject.SetActive(false);
			}
		}, 0f);
		if ((bool)currentEffect)
		{
			UnityEngine.Object.Destroy(currentEffect);
		}
		currentEffect = null;
	}

	protected virtual void MainButton_OnClick()
	{
		if (OnProposalClick != null)
		{
			OnProposalClick(this);
		}
	}
}
