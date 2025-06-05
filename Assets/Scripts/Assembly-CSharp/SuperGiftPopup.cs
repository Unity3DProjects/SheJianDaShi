using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class SuperGiftPopup : GUIPopup
{
	private const string CLAIMED_DESCRIPTION = "ui.popup.super.gift.claim";

	private const string RECEIVED_DESCRIPTION = "ui.popup.super.gift.receive";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnAllRewardsReceived;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnRewardReceived;

	[SerializeField]
	private float hideDelay = 0.5f;

	[SerializeField]
	private tk2dUIItem[] firstRewardButtons;

	[SerializeField]
	private GUILayouter[] firstButtonLayouters;

	[SerializeField]
	private GUILayouter[] firstNonButtonLayouters;

	[SerializeField]
	private tk2dTextMesh[] firstNonButtonLabels;

	[SerializeField]
	private tk2dTextMesh[] firstRewardLabels;

	[SerializeField]
	private Transform[] firstRewardEffectTransforms;

	[SerializeField]
	private int firstDayReward;

	[SerializeField]
	private bool isFirstRewardInCoins = true;

	[SerializeField]
	private tk2dUIItem[] secondRewardButtons;

	[SerializeField]
	private GUILayouter[] secondButtonLayouters;

	[SerializeField]
	private GUILayouter[] secondNonButtonLayouters;

	[SerializeField]
	private tk2dTextMesh[] secondNonButtonLabels;

	[SerializeField]
	private tk2dTextMesh[] secondRewardLabels;

	[SerializeField]
	private Transform[] secondRewardEffectTransforms;

	[SerializeField]
	private int secondDayReward;

	[SerializeField]
	private bool isSecondRewardInCoins = true;

	[SerializeField]
	private tk2dUIItem[] thirdRewardButtons;

	[SerializeField]
	private GUILayouter[] thirdButtonLayouters;

	[SerializeField]
	private GUILayouter[] thirdNonButtonLayouters;

	[SerializeField]
	private tk2dTextMesh[] thirdNonButtonLabels;

	[SerializeField]
	private tk2dTextMesh[] thirdRewardLabels;

	[SerializeField]
	private Transform[] thirdRewardEffectTransforms;

	[SerializeField]
	private int thirdDayReward;

	[SerializeField]
	private bool isThirdRewardInCoins;

	[SerializeField]
	private tk2dUIItem closeButtonAdditional;

	[SerializeField]
	[ResourceLink]
	private AssetLink idleEffectLink;

	[SerializeField]
	private Transform[] idleEffectTransforms;

	public static event Action OnAllRewardsReceived
	{
		add
		{
			Action action = SuperGiftPopup.m_OnAllRewardsReceived;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SuperGiftPopup.m_OnAllRewardsReceived, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = SuperGiftPopup.m_OnAllRewardsReceived;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SuperGiftPopup.m_OnAllRewardsReceived, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<bool> OnRewardReceived
	{
		add
		{
			Action<bool> action = SuperGiftPopup.m_OnRewardReceived;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SuperGiftPopup.m_OnRewardReceived, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = SuperGiftPopup.m_OnRewardReceived;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SuperGiftPopup.m_OnRewardReceived, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void OnEnable()
	{
		if (idleEffectLink != null && !string.IsNullOrEmpty(idleEffectLink.assetGUID))
		{
			Transform[] array = idleEffectTransforms;
			foreach (Transform parent in array)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate((GameObject)idleEffectLink.GetAsset());
				gameObject.transform.parent = parent;
				gameObject.transform.localPosition = Vector3.zero;
			}
		}
		tk2dUIItem[] array2 = firstRewardButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array2)
		{
			if (tk2dUIItem2 != null)
			{
				tk2dUIItem2.OnClick += RewardButton_OnClick;
			}
		}
		tk2dUIItem[] array3 = secondRewardButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array3)
		{
			if (tk2dUIItem3 != null)
			{
				tk2dUIItem3.OnClick += RewardButton_OnClick;
			}
		}
		tk2dUIItem[] array4 = thirdRewardButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array4)
		{
			if (tk2dUIItem4 != null)
			{
				tk2dUIItem4.OnClick += RewardButton_OnClick;
			}
		}
		if (closeButtonAdditional != null)
		{
			closeButtonAdditional.OnClick += CloseButton_OnClick;
		}
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		tk2dUIItem[] array = firstRewardButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			if (tk2dUIItem2 != null)
			{
				tk2dUIItem2.OnClick -= RewardButton_OnClick;
			}
		}
		tk2dUIItem[] array2 = secondRewardButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			if (tk2dUIItem3 != null)
			{
				tk2dUIItem3.OnClick -= RewardButton_OnClick;
			}
		}
		tk2dUIItem[] array3 = thirdRewardButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			if (tk2dUIItem4 != null)
			{
				tk2dUIItem4.OnClick -= RewardButton_OnClick;
			}
		}
		if (closeButtonAdditional != null)
		{
			closeButtonAdditional.OnClick -= CloseButton_OnClick;
		}
		base.OnDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
		tk2dTextMesh[] array = firstRewardLabels;
		foreach (tk2dTextMesh tk2dTextMesh2 in array)
		{
			if (tk2dTextMesh2 != null)
			{
				tk2dTextMesh2.text = string.Format("+{0}", firstDayReward.ToString());
			}
		}
		tk2dTextMesh[] array2 = secondRewardLabels;
		foreach (tk2dTextMesh tk2dTextMesh3 in array2)
		{
			if (tk2dTextMesh3 != null)
			{
				tk2dTextMesh3.text = string.Format("+{0}", secondDayReward.ToString());
			}
		}
		tk2dTextMesh[] array3 = thirdRewardLabels;
		foreach (tk2dTextMesh tk2dTextMesh4 in array3)
		{
			if (tk2dTextMesh4 != null)
			{
				tk2dTextMesh4.text = string.Format("+{0}", thirdDayReward.ToString());
			}
		}
		switch (base.CachedManagers.CachedProposalManager.SuperGiftNumber)
		{
		case 0:
			SetRewardActivity(firstButtonLayouters, firstNonButtonLayouters, firstNonButtonLabels, SuperGiftRewardState.Current);
			SetRewardActivity(secondButtonLayouters, secondNonButtonLayouters, secondNonButtonLabels, SuperGiftRewardState.Next);
			SetRewardActivity(thirdButtonLayouters, thirdNonButtonLayouters, thirdNonButtonLabels, SuperGiftRewardState.Next);
			break;
		case 1:
			SetRewardActivity(firstButtonLayouters, firstNonButtonLayouters, firstNonButtonLabels, SuperGiftRewardState.Received);
			SetRewardActivity(secondButtonLayouters, secondNonButtonLayouters, secondNonButtonLabels, SuperGiftRewardState.Current);
			SetRewardActivity(thirdButtonLayouters, thirdNonButtonLayouters, thirdNonButtonLabels, SuperGiftRewardState.Next);
			break;
		case 2:
			SetRewardActivity(firstButtonLayouters, firstNonButtonLayouters, firstNonButtonLabels, SuperGiftRewardState.Received);
			SetRewardActivity(secondButtonLayouters, secondNonButtonLayouters, secondNonButtonLabels, SuperGiftRewardState.Received);
			SetRewardActivity(thirdButtonLayouters, thirdNonButtonLayouters, thirdNonButtonLabels, SuperGiftRewardState.Current);
			break;
		}
	}

	private void SetRewardActivity(GUILayouter[] buttonLayouters, GUILayouter[] nonButtonLayouters, tk2dTextMesh[] meshes, SuperGiftRewardState rewardState)
	{
		switch (rewardState)
		{
		case SuperGiftRewardState.Received:
		case SuperGiftRewardState.Next:
		{
			foreach (GUILayouter gUILayouter3 in buttonLayouters)
			{
				if (gUILayouter3 != null)
				{
					gUILayouter3.gameObject.SetActive(false);
				}
			}
			foreach (GUILayouter gUILayouter4 in nonButtonLayouters)
			{
				if (gUILayouter4 != null)
				{
					gUILayouter4.gameObject.SetActive(true);
				}
			}
			string text = ((rewardState != SuperGiftRewardState.Next) ? "ui.popup.super.gift.receive" : "ui.popup.super.gift.claim");
			foreach (tk2dTextMesh tk2dTextMesh2 in meshes)
			{
				if (tk2dTextMesh2 != null)
				{
					tk2dTextMesh2.text = text;
				}
			}
			break;
		}
		case SuperGiftRewardState.Current:
			foreach (GUILayouter gUILayouter in buttonLayouters)
			{
				if (gUILayouter != null)
				{
					gUILayouter.gameObject.SetActive(true);
				}
			}
			foreach (GUILayouter gUILayouter2 in nonButtonLayouters)
			{
				if (gUILayouter2 != null)
				{
					gUILayouter2.gameObject.SetActive(false);
				}
			}
			break;
		}
	}

	private void AddReward(bool isRewardOnCoins, int reward, Transform[] rewardTransforms)
	{
		if (isRewardOnCoins)
		{
			base.CachedManagers.CachedStatisticsManager.AddCoins(reward);
		}
		else
		{
			base.CachedManagers.CachedStatisticsManager.AddGems(reward);
		}
		EffectType effectType = ((!isRewardOnCoins) ? EffectType.GUIPopupGemsAdd : EffectType.GUIPopupCoinsAdd);
		foreach (Transform parent in rewardTransforms)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(effectType));
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
		}
		base.CachedManagers.CachedProposalManager.SuperGiftNumber++;
	}

	private void RewardButton_OnClick()
	{
		base.CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoAvailable)
		{
			if (isVideoAvailable)
			{
				switch (base.CachedManagers.CachedProposalManager.SuperGiftNumber)
				{
				case 0:
					if (SuperGiftPopup.OnRewardReceived != null)
					{
						SuperGiftPopup.OnRewardReceived(isFirstRewardInCoins);
					}
					SetRewardActivity(firstButtonLayouters, firstNonButtonLayouters, firstNonButtonLabels, SuperGiftRewardState.Received);
					SetRewardActivity(secondButtonLayouters, secondNonButtonLayouters, secondNonButtonLabels, SuperGiftRewardState.Current);
					AddReward(isFirstRewardInCoins, firstDayReward, firstRewardEffectTransforms);
					base.CachedManagers.CachedGiftManager.DateLastSuperGift = DateTime.Now;
					break;
				case 1:
					if (SuperGiftPopup.OnRewardReceived != null)
					{
						SuperGiftPopup.OnRewardReceived(isSecondRewardInCoins);
					}
					SetRewardActivity(secondButtonLayouters, secondNonButtonLayouters, secondNonButtonLabels, SuperGiftRewardState.Received);
					SetRewardActivity(thirdButtonLayouters, thirdNonButtonLayouters, thirdNonButtonLabels, SuperGiftRewardState.Current);
					AddReward(isSecondRewardInCoins, secondDayReward, secondRewardEffectTransforms);
					break;
				case 2:
				{
					if (SuperGiftPopup.OnRewardReceived != null)
					{
						SuperGiftPopup.OnRewardReceived(isThirdRewardInCoins);
					}
					SetRewardActivity(thirdButtonLayouters, thirdNonButtonLayouters, thirdNonButtonLabels, SuperGiftRewardState.Received);
					AddReward(isThirdRewardInCoins, thirdDayReward, thirdRewardEffectTransforms);
					if (SuperGiftPopup.OnAllRewardsReceived != null)
					{
						SuperGiftPopup.OnAllRewardsReceived();
					}
					tk2dUIItem[] array = thirdRewardButtons;
					foreach (tk2dUIItem tk2dUIItem2 in array)
					{
						if (tk2dUIItem2 != null)
						{
							tk2dUIItem2.enabled = false;
						}
					}
					base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
					{
						Hide();
					}, hideDelay);
					break;
				}
				}
			}
		});
	}
}
