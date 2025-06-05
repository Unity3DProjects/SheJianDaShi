using System;
using Spine;
using UnityEngine;

public class CharacterForAdsTimerPopup : GUIPopup
{
	private const string DANCE_ANIMATION = "dance";

	private const string DANCE_ANIMATION_CHINESE = "dance_ch";

	[SerializeField]
	private GUILayouter okLayouter;

	[SerializeField]
	private GUILayouter yesNoLayouter;

	[SerializeField]
	private tk2dTextMesh notificationLabel;

	[SerializeField]
	private tk2dUIItem yesButton;

	[SerializeField]
	private tk2dUIItem noButton;

	[SerializeField]
	private tk2dUIItem okButton;

	[SerializeField]
	private tk2dTextMesh timerLabel;

	[SerializeField]
	private SkeletonAnimation adsManAnimation;

	private ShopItemCharacter character;

	private GUITimer timer = new GUITimer();

	private Spine.Animation cachedAnimationDance;

	private Spine.Animation cachedAnimationDanceChinese;

	public ShopItemCharacter Character
	{
		set
		{
			if (value != null)
			{
				character = value;
				DateTime dateTime = DateTime.Now.Subtract(SingletonMonoBehaviour<StatisticsManager>.Instance.CheatedTime);
				DateTime dateTime2 = CustomPlayerPrefs.GetDateTime("character_timer_started" + character.ShopItemID, dateTime);
				if ((dateTime2 - dateTime).TotalSeconds > 0.0 && !timer.IsTimerActive)
				{
					timer.TimerInSeconds = (float)(dateTime2 - dateTime).TotalSeconds;
					timer.IsTimerActive = true;
					timer.UpdateTimer(Time.unscaledDeltaTime);
				}
			}
		}
	}

	private Spine.Animation CachedAnimationDance
	{
		get
		{
			if (cachedAnimationDance == null)
			{
				cachedAnimationDance = adsManAnimation.Skeleton.Data.FindAnimation("dance");
			}
			return cachedAnimationDance;
		}
	}

	private Spine.Animation CachedAnimationDanceChinese
	{
		get
		{
			if (cachedAnimationDanceChinese == null)
			{
				cachedAnimationDanceChinese = adsManAnimation.Skeleton.Data.FindAnimation("dance_ch");
			}
			return cachedAnimationDanceChinese;
		}
	}

	private void Awake()
	{
		SystemLanguage currentLanguage = SingletonMonoBehaviour<StatisticsManager>.Instance.CurrentLanguage;
		Spine.Animation animation = ((currentLanguage != SystemLanguage.Chinese && currentLanguage != SystemLanguage.ChineseSimplified && currentLanguage != SystemLanguage.ChineseTraditional) ? CachedAnimationDance : CachedAnimationDanceChinese);
		adsManAnimation.State.SetAnimation(0, animation, true);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		yesButton.OnClick += YesButton_OnClick;
		noButton.OnClick += CloseButton_OnClick;
		okButton.OnClick += CloseButton_OnClick;
		timer.OnTimerChanged += Timer_OnTimerChanged;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		yesButton.OnClick -= YesButton_OnClick;
		noButton.OnClick -= CloseButton_OnClick;
		okButton.OnClick -= CloseButton_OnClick;
		timer.OnTimerChanged -= Timer_OnTimerChanged;
	}

	protected override void Update()
	{
		base.Update();
		timer.UpdateTimer(Time.unscaledDeltaTime);
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Show(onFinished, immediately, delay, callInvokes);
		bool flag = CustomPlayerPrefs.GetBool("character_timer_notifications");
		okLayouter.gameObject.SetActive(flag);
		yesNoLayouter.gameObject.SetActive(!flag);
		notificationLabel.gameObject.SetActive(!flag);
		if (!flag)
		{
			CustomPlayerPrefs.SetBool("character_timer_notifications", true);
		}
	}

	private void YesButton_OnClick()
	{
		CloseButton_OnClick();
		base.CachedManagers.CachedNotifyManager.IsAdsPopupNotificationEnabled = true;
		base.CachedManagers.CachedNotifyManager.UpdateNotifications(timer.TimerInSeconds, base.ObjectsSpawner.GetCharacterLocalizedName(character.Infos[0].characterNameType));
	}

	private void Timer_OnTimerChanged(int time)
	{
		timerLabel.text = string.Format("{0}:{1}:{2}", time / 3600, (time / 60 % 60).ToString("00"), (time % 60).ToString("00"));
		if (time == 0)
		{
			CloseButton_OnClick();
		}
	}
}
