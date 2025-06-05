using System;
using UnityEngine;

public class LeavingTournamentOnResultScreenPopup : GUIPopup
{
	private const string SAVE_FOR_GEMS_TEXT = "ui.main.tournament_mode.save.for.gems.label";

	[SerializeField]
	private tk2dUIItem leaveButton;

	[SerializeField]
	private tk2dUIItem saveForVideoButton;

	[SerializeField]
	private tk2dUIItem saveForGemsButton;

	[SerializeField]
	private tk2dTextMesh priceGemsLabel;

	private bool isOnVideoFinishEventSubscribed;

	public SavingState SavingState { get; private set; }

	private bool IsOnVideoFinishEventSubscribed
	{
		get
		{
			return isOnVideoFinishEventSubscribed;
		}
		set
		{
			if (isOnVideoFinishEventSubscribed != value)
			{
				isOnVideoFinishEventSubscribed = value;
				if (isOnVideoFinishEventSubscribed)
				{
					MCAdsManager.OnVideoTryLoad += MCAdsManager_OnVideoTryLoad;
					AdvertisingManager.OnVideoAdFinish += AdvertisingManager_OnVideoAdFinish;
				}
				else
				{
					MCAdsManager.OnVideoTryLoad -= MCAdsManager_OnVideoTryLoad;
					AdvertisingManager.OnVideoAdFinish -= AdvertisingManager_OnVideoAdFinish;
				}
			}
		}
	}

	protected override void OnEnable()
	{
		leaveButton.OnClick += LeaveButton_OnClick;
		saveForVideoButton.OnClick += SaveForVideoButton_OnClick;
		saveForGemsButton.OnClick += SaveForGemsButton_OnClick;
		IsOnVideoFinishEventSubscribed = false;
		SavingState = SavingState.None;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		leaveButton.OnClick -= LeaveButton_OnClick;
		saveForVideoButton.OnClick -= SaveForVideoButton_OnClick;
		saveForGemsButton.OnClick -= SaveForGemsButton_OnClick;
		base.OnDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		bool isSubscriptionActive = base.CachedManagers.CachedSubscriptionManager.IsSubscriptionActive;
		saveForGemsButton.gameObject.SetActive(isSubscriptionActive);
		saveForVideoButton.gameObject.SetActive(!isSubscriptionActive);
		priceGemsLabel.text = base.CachedManagers.CachedLocalisationManager.GetTextByKey("ui.main.tournament_mode.save.for.gems.label") + base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice;
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	private void LeaveButton_OnClick()
	{
		SavingState = SavingState.ExitWithoutSave;
		Hide();
	}

	private void SaveForVideoButton_OnClick()
	{
		IsOnVideoFinishEventSubscribed = true;
		base.CachedManagers.CachedAdvertisingManager.ShowVideo();
	}

	private void SaveForGemsButton_OnClick()
	{
		if (base.CachedManagers.CachedStatisticsManager.Gems >= base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice)
		{
			base.CachedManagers.CachedStatisticsManager.DecreaseGems(base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice);
			SavingState = SavingState.ExitWithSave;
			Hide();
		}
		else
		{
			GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MiniBank, true, null, true);
			(gUIPopup as MiniBankPopup).InitPopup(base.CachedManagers.CachedProposalManager.GemsForSaveTournamentPrice);
		}
	}

	private void MCAdsManager_OnVideoTryLoad(bool result)
	{
		if (!result)
		{
			Hide();
		}
	}

	private void AdvertisingManager_OnVideoAdFinish(bool isVideoAvailable)
	{
		IsOnVideoFinishEventSubscribed = false;
		if (isVideoAvailable)
		{
			SavingState = SavingState.ExitWithSave;
			Hide();
		}
	}

	protected override void CloseButton_OnClick()
	{
		SavingState = SavingState.None;
		Hide();
	}
}
