using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class MisteryBoxRewardPopup : GUIPopup
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<bool> m_OnRewardReceived;

	[SerializeField]
	private tk2dSprite coinsSprite;

	[SerializeField]
	private tk2dTextMesh coinsLabel;

	[SerializeField]
	private GUILayouter coinsLayouter;

	[SerializeField]
	private tk2dSprite gemsSprite;

	[SerializeField]
	private tk2dTextMesh gemsLabel;

	[SerializeField]
	private GUILayouter gemsLayouter;

	[SerializeField]
	private tk2dUIItem claimButton;

	[SerializeField]
	[ResourceLink]
	private AssetLink idleEffectLink;

	private bool isRepalyPopupNeeded;

	private bool isCurrentCoinsReward;

	private int currentReward;

	private ParticleSystem[] particles;

	public static event Action<bool> OnRewardReceived
	{
		add
		{
			Action<bool> action = MisteryBoxRewardPopup.m_OnRewardReceived;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MisteryBoxRewardPopup.m_OnRewardReceived, (Action<bool>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<bool> action = MisteryBoxRewardPopup.m_OnRewardReceived;
			Action<bool> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MisteryBoxRewardPopup.m_OnRewardReceived, (Action<bool>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Update()
	{
		if (particles != null && particles.Length > 0)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Simulate(Time.unscaledDeltaTime, true, false);
			}
		}
		base.Update();
	}

	protected override void OnEnable()
	{
		claimButton.OnClick += ClaimButton_OnClick;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		claimButton.OnClick -= ClaimButton_OnClick;
		base.OnDisable();
	}

	public void Init(bool isCoinsReward, int reward, bool isOriginalMisteryBox)
	{
		isCurrentCoinsReward = isCoinsReward;
		currentReward = reward;
		coinsLayouter.gameObject.SetActive(isCoinsReward);
		gemsLayouter.gameObject.SetActive(!isCoinsReward);
		if (isCoinsReward)
		{
			gemsSprite.gameObject.SetActive(false);
			coinsLabel.text = string.Format("+{0}", reward);
		}
		else
		{
			coinsSprite.gameObject.SetActive(false);
			gemsLabel.text = string.Format("+{0}", reward);
		}
		if (idleEffectLink != null && !string.IsNullOrEmpty(idleEffectLink.assetGUID))
		{
			Transform parent = ((!isCoinsReward) ? gemsSprite.CachedTransform : coinsSprite.CachedTransform);
			GameObject gameObject = UnityEngine.Object.Instantiate((GameObject)idleEffectLink.GetAsset());
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			particles = gameObject.GetComponentsInChildren<ParticleSystem>();
			if (particles != null && particles.Length > 0)
			{
				for (int i = 0; i < particles.Length; i++)
				{
					particles[i].Play(true);
				}
			}
		}
		isRepalyPopupNeeded = isOriginalMisteryBox;
	}

	private void ClaimButton_OnClick()
	{
		EffectType effectType = ((!isCurrentCoinsReward) ? EffectType.GUIPopupGemsAdd : EffectType.GUIPopupCoinsAdd);
		Transform parent = ((!isCurrentCoinsReward) ? gemsSprite.CachedTransform : coinsSprite.CachedTransform);
		GameObject gameObject = UnityEngine.Object.Instantiate(base.CachedManagers.CachedEffectsManager.EffectPrefabByType(effectType));
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		if (isCurrentCoinsReward)
		{
			base.CachedManagers.CachedStatisticsManager.AddCoins(currentReward);
		}
		else
		{
			base.CachedManagers.CachedStatisticsManager.AddGems(currentReward);
		}
		if (MisteryBoxRewardPopup.OnRewardReceived != null)
		{
			MisteryBoxRewardPopup.OnRewardReceived(isCurrentCoinsReward);
		}
		Hide(delegate
		{
			if (isRepalyPopupNeeded && base.CachedManagers.CachedRemoteManager.GetRemoteValueForKey("IsMisteryBoxReplayEnabled", true))
			{
				base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.MisteryBoxReplay);
			}
			else
			{
				base.CachedManagers.CachedGameManager.IsGamePaused = false;
			}
		});
	}
}
