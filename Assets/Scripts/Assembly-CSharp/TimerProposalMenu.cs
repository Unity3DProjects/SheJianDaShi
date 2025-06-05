using UnityEngine;

public class TimerProposalMenu : GUIProposalMenu
{
	[SerializeField]
	private tk2dTextMesh timerLabel;

	[SerializeField]
	protected tk2dBaseSprite iconProposal;

	private bool isDoubled;

	protected override void Awake()
	{
		base.Awake();
		if (tk2dSystem.IsRetina)
		{
			iconProposal.transform.localPosition *= 2f;
			isDoubled = true;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GUITimer proposalTimer = SingletonMonoBehaviour<ProposalManager>.Instance.ProposalTimer;
		proposalTimer.OnTimerChanged += ProposalManager_OnProposalTimerChanged;
		ProposalManager_OnProposalTimerChanged((int)proposalTimer.TimerInSeconds);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		ProposalManager instanceIfExist = SingletonMonoBehaviour<ProposalManager>.InstanceIfExist;
		if (instanceIfExist != null)
		{
			instanceIfExist.ProposalTimer.OnTimerChanged -= ProposalManager_OnProposalTimerChanged;
		}
	}

	public override void Activate()
	{
		base.Activate();
		if (currentEffect != null)
		{
			float num = ((!tk2dSystem.IsRetina || isDoubled) ? 1f : 2f);
			currentEffect.transform.localPosition = iconProposal.transform.localPosition * num;
		}
	}

	private void ProposalManager_OnProposalTimerChanged(int time)
	{
		timerLabel.text = string.Format("{0}:{1}:{2}", time / 3600, (time / 60 % 60).ToString("00"), (time % 60).ToString("00"));
	}
}
