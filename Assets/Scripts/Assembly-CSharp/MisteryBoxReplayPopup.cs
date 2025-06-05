using System;
using UnityEngine;

public class MisteryBoxReplayPopup : GUIPopup
{
	[SerializeField]
	private tk2dUIItem watchButton;

	[SerializeField]
	private Transform characterEffectTransform;

	private GameObject characterEffect;

	private ParticleSystem[] particles;

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
		watchButton.OnClick += WatchButton_OnClick;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		watchButton.OnClick -= WatchButton_OnClick;
		base.OnDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			characterEffect = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<EffectsManager>.Instance.EffectPrefabByType(EffectType.GUIPopupInviteSecond));
			characterEffect.transform.parent = characterEffectTransform;
			characterEffect.transform.localPosition = Vector3.zero;
			particles = characterEffect.GetComponentsInChildren<ParticleSystem>();
			if (particles != null && particles.Length > 0)
			{
				for (int i = 0; i < particles.Length; i++)
				{
					particles[i].Play(true);
				}
			}
		}, 0f);
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		if (characterEffect != null)
		{
			UnityEngine.Object.Destroy(characterEffect);
			characterEffect = null;
		}
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void WatchButton_OnClick()
	{
		base.CachedManagers.CachedGameManager.IsGamePaused = true;
		base.CachedManagers.CachedAdvertisingManager.ShowVideo(delegate(bool isVideoShown)
		{
			base.CachedManagers.CachedGameManager.IsGamePaused = false;
			Hide(delegate
			{
				if (isVideoShown)
				{
					base.CachedManagers.CachedGiftManager.GenerateMisteryBoxAward();
				}
				else
				{
					base.CachedManagers.CachedGameManager.IsGamePaused = false;
				}
			});
		});
	}

	protected override void CloseButton_OnClick()
	{
		base.CachedManagers.CachedGameManager.IsGamePaused = false;
		base.CloseButton_OnClick();
	}
}
