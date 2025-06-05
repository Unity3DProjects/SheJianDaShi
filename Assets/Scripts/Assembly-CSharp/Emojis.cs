using Spine;
using UnityEngine;

public class Emojis : MonoBehaviour
{
	[SerializeField]
	private SkeletonAnimation emojiDockAnimation;

	[SerializeField]
	private SkeletonAnimation leftEmojiBubbleAnimation;

	[SerializeField]
	private SkeletonAnimation leftEmojiAnimation;

	[SerializeField]
	private SkeletonAnimation rightEmojiBubbleAnimation;

	[SerializeField]
	private SkeletonAnimation rightEmojiAnimation;

	[SerializeField]
	private tk2dUIItem dockButton;

	[SerializeField]
	private tk2dUIItem emojiButtonHello;

	[SerializeField]
	private tk2dUIItem emojiButtonScared;

	[SerializeField]
	private tk2dUIItem emojiButtonHappy;

	[SerializeField]
	private tk2dUIItem emojiButtonBye;

	[SerializeField]
	private BoxCollider dockContentTouchBlocker;

	[SerializeField]
	private BoxCollider dockTouchBlocker;

	[SerializeField]
	private float dockInactiveTimeAfterChoosingEmoji;

	[SerializeField]
	private GUILayoutCell iPhoneXCell;

	private Spine.Animation cachedAnimationEmojiDockShow;

	private Spine.Animation cachedAnimationEmojiDockShown;

	private Spine.Animation cachedAnimationEmojiDockHide;

	private Spine.Animation cachedAnimationEmojiDockHidden;

	private Spine.Animation cachedAnimationLeftBubbleHide;

	private Spine.Animation cachedAnimationLeftBubbleIdle;

	private Spine.Animation cachedAnimationLeftBubbleShow;

	private Spine.Animation cachedAnimationRightBubbleHide;

	private Spine.Animation cachedAnimationRightBubbleIdle;

	private Spine.Animation cachedAnimationRightBubbleShow;

	private Spine.Animation cachedAnimationLeftEmojiAnimationBye;

	private Spine.Animation cachedAnimationLeftEmojiAnimationHappy;

	private Spine.Animation cachedAnimationLeftEmojiAnimationHello;

	private Spine.Animation cachedAnimationLeftEmojiAnimationScared;

	private Spine.Animation cachedAnimationRightEmojiAnimationBye;

	private Spine.Animation cachedAnimationRightEmojiAnimationHappy;

	private Spine.Animation cachedAnimationRightEmojiAnimationHello;

	private Spine.Animation cachedAnimationRightEmojiAnimationScared;

	private EventData cachedEventDataHideLeftBubble;

	private EventData cachedEventDataHideRightBubble;

	private bool isDockShown;

	private GUILayouterRotationType currentRotation;

	public GUILayoutCell IPhoneXCell
	{
		get
		{
			return iPhoneXCell;
		}
	}

	private Spine.Animation CachedAnimationEmojiDockShow
	{
		get
		{
			if (cachedAnimationEmojiDockShow == null)
			{
				cachedAnimationEmojiDockShow = emojiDockAnimation.Skeleton.Data.FindAnimation("show");
			}
			return cachedAnimationEmojiDockShow;
		}
	}

	private Spine.Animation CachedAnimationEmojiDockShown
	{
		get
		{
			if (cachedAnimationEmojiDockShown == null)
			{
				cachedAnimationEmojiDockShown = emojiDockAnimation.Skeleton.Data.FindAnimation("shown");
			}
			return cachedAnimationEmojiDockShown;
		}
	}

	private Spine.Animation CachedAnimationEmojiDockHide
	{
		get
		{
			if (cachedAnimationEmojiDockHide == null)
			{
				cachedAnimationEmojiDockHide = emojiDockAnimation.Skeleton.Data.FindAnimation("hide");
			}
			return cachedAnimationEmojiDockHide;
		}
	}

	private Spine.Animation CachedAnimationEmojiDockHidden
	{
		get
		{
			if (cachedAnimationEmojiDockHidden == null)
			{
				cachedAnimationEmojiDockHidden = emojiDockAnimation.Skeleton.Data.FindAnimation("hidden");
			}
			return cachedAnimationEmojiDockHidden;
		}
	}

	private Spine.Animation CachedAnimationLeftBubbleHide
	{
		get
		{
			if (cachedAnimationLeftBubbleHide == null)
			{
				cachedAnimationLeftBubbleHide = leftEmojiBubbleAnimation.Skeleton.Data.FindAnimation("hide");
			}
			return cachedAnimationLeftBubbleHide;
		}
	}

	private Spine.Animation CachedAnimationLeftBubbleIdle
	{
		get
		{
			if (cachedAnimationLeftBubbleIdle == null)
			{
				cachedAnimationLeftBubbleIdle = leftEmojiBubbleAnimation.Skeleton.Data.FindAnimation("idle");
			}
			return cachedAnimationLeftBubbleIdle;
		}
	}

	private Spine.Animation CachedAnimationLeftBubbleShow
	{
		get
		{
			if (cachedAnimationLeftBubbleShow == null)
			{
				cachedAnimationLeftBubbleShow = leftEmojiBubbleAnimation.Skeleton.Data.FindAnimation("show");
			}
			return cachedAnimationLeftBubbleShow;
		}
	}

	private Spine.Animation CachedAnimationRightBubbleHide
	{
		get
		{
			if (cachedAnimationRightBubbleHide == null)
			{
				cachedAnimationRightBubbleHide = rightEmojiBubbleAnimation.Skeleton.Data.FindAnimation("hide");
			}
			return cachedAnimationRightBubbleHide;
		}
	}

	private Spine.Animation CachedAnimationRightBubbleIdle
	{
		get
		{
			if (cachedAnimationRightBubbleIdle == null)
			{
				cachedAnimationRightBubbleIdle = rightEmojiBubbleAnimation.Skeleton.Data.FindAnimation("idle");
			}
			return cachedAnimationRightBubbleIdle;
		}
	}

	private Spine.Animation CachedAnimationRightBubbleShow
	{
		get
		{
			if (cachedAnimationRightBubbleShow == null)
			{
				cachedAnimationRightBubbleShow = rightEmojiBubbleAnimation.Skeleton.Data.FindAnimation("show");
			}
			return cachedAnimationRightBubbleShow;
		}
	}

	private Spine.Animation CachedAnimationLeftEmojiAnimationBye
	{
		get
		{
			if (cachedAnimationLeftEmojiAnimationBye == null)
			{
				cachedAnimationLeftEmojiAnimationBye = leftEmojiAnimation.Skeleton.Data.FindAnimation("bye");
			}
			return cachedAnimationLeftEmojiAnimationBye;
		}
	}

	private Spine.Animation CachedAnimationLeftEmojiAnimationHappy
	{
		get
		{
			if (cachedAnimationLeftEmojiAnimationHappy == null)
			{
				cachedAnimationLeftEmojiAnimationHappy = leftEmojiAnimation.Skeleton.Data.FindAnimation("happy");
			}
			return cachedAnimationLeftEmojiAnimationHappy;
		}
	}

	private Spine.Animation CachedAnimationLeftEmojiAnimationHello
	{
		get
		{
			if (cachedAnimationLeftEmojiAnimationHello == null)
			{
				cachedAnimationLeftEmojiAnimationHello = leftEmojiAnimation.Skeleton.Data.FindAnimation("hello");
			}
			return cachedAnimationLeftEmojiAnimationHello;
		}
	}

	private Spine.Animation CachedAnimationLeftEmojiAnimationScared
	{
		get
		{
			if (cachedAnimationLeftEmojiAnimationScared == null)
			{
				cachedAnimationLeftEmojiAnimationScared = leftEmojiAnimation.Skeleton.Data.FindAnimation("scared");
			}
			return cachedAnimationLeftEmojiAnimationScared;
		}
	}

	private EventData CachedEventDataHideLeftBubble
	{
		get
		{
			if (cachedEventDataHideLeftBubble == null)
			{
				cachedEventDataHideLeftBubble = leftEmojiAnimation.Skeleton.Data.FindEvent("Hide_Bubble");
			}
			return cachedEventDataHideLeftBubble;
		}
	}

	private Spine.Animation CachedAnimationRightEmojiAnimationBye
	{
		get
		{
			if (cachedAnimationRightEmojiAnimationBye == null)
			{
				cachedAnimationRightEmojiAnimationBye = rightEmojiAnimation.Skeleton.Data.FindAnimation("bye_flipped");
			}
			return cachedAnimationRightEmojiAnimationBye;
		}
	}

	private Spine.Animation CachedAnimationRightEmojiAnimationHappy
	{
		get
		{
			if (cachedAnimationRightEmojiAnimationHappy == null)
			{
				cachedAnimationRightEmojiAnimationHappy = rightEmojiAnimation.Skeleton.Data.FindAnimation("happy");
			}
			return cachedAnimationRightEmojiAnimationHappy;
		}
	}

	private Spine.Animation CachedAnimationRightEmojiAnimationHello
	{
		get
		{
			if (cachedAnimationRightEmojiAnimationHello == null)
			{
				cachedAnimationRightEmojiAnimationHello = rightEmojiAnimation.Skeleton.Data.FindAnimation("hello");
			}
			return cachedAnimationRightEmojiAnimationHello;
		}
	}

	private Spine.Animation CachedAnimationRightEmojiAnimationScared
	{
		get
		{
			if (cachedAnimationRightEmojiAnimationScared == null)
			{
				cachedAnimationRightEmojiAnimationScared = rightEmojiAnimation.Skeleton.Data.FindAnimation("scared");
			}
			return cachedAnimationRightEmojiAnimationScared;
		}
	}

	private EventData CachedEventDataHideRightBubble
	{
		get
		{
			if (cachedEventDataHideRightBubble == null)
			{
				cachedEventDataHideRightBubble = rightEmojiAnimation.Skeleton.Data.FindEvent("Hide_Bubble");
			}
			return cachedEventDataHideRightBubble;
		}
	}

	private void Awake()
	{
		emojiDockAnimation.State.SetAnimation(0, CachedAnimationEmojiDockHidden, true);
		dockContentTouchBlocker.enabled = false;
		dockTouchBlocker.enabled = false;
		isDockShown = false;
		if (tk2dSystem.IsRetina)
		{
			dockButton.GetComponent<BoxCollider>().size *= 2f;
			emojiButtonBye.GetComponent<BoxCollider>().size *= 2f;
			emojiButtonHappy.GetComponent<BoxCollider>().size *= 2f;
			emojiButtonHello.GetComponent<BoxCollider>().size *= 2f;
			emojiButtonScared.GetComponent<BoxCollider>().size *= 2f;
			leftEmojiAnimation.gameObject.transform.localPosition *= 2f;
			rightEmojiAnimation.gameObject.transform.localPosition *= 2f;
		}
	}

	private void OnEnable()
	{
		dockButton.OnClick += DockButton_OnClick;
		emojiButtonHello.OnClick += EmojiButtonHello_OnClick;
		emojiButtonScared.OnClick += EmojiButtonScared_OnClick;
		emojiButtonHappy.OnClick += EmojiButtonHappy_OnClick;
		emojiButtonBye.OnClick += EmojiButtonBye_OnClick;
		leftEmojiAnimation.State.Event += LeftEmojiAnimation_State_Event;
		rightEmojiAnimation.State.Event += RightEmojiAnimation_State_Event;
		MultiplayerManager.OnEmojiReceived += MultiplayerManager_OnEmojiReceived;
	}

	private void OnDisable()
	{
		dockButton.OnClick -= DockButton_OnClick;
		emojiButtonHello.OnClick -= EmojiButtonHello_OnClick;
		emojiButtonScared.OnClick -= EmojiButtonHappy_OnClick;
		emojiButtonHappy.OnClick -= EmojiButtonScared_OnClick;
		emojiButtonBye.OnClick -= EmojiButtonBye_OnClick;
		leftEmojiAnimation.State.Event -= LeftEmojiAnimation_State_Event;
		rightEmojiAnimation.State.Event -= RightEmojiAnimation_State_Event;
		MultiplayerManager.OnEmojiReceived -= MultiplayerManager_OnEmojiReceived;
	}

	private void ShowBubblesWithEmojiOfType(EmojiType value, bool isLeftBubble)
	{
		if (isLeftBubble)
		{
			leftEmojiBubbleAnimation.State.SetAnimation(0, CachedAnimationLeftBubbleShow, false);
			leftEmojiBubbleAnimation.State.AddAnimation(0, CachedAnimationLeftBubbleIdle, true, 0f);
			switch (value)
			{
			case EmojiType.Bye:
				leftEmojiAnimation.State.SetAnimation(0, CachedAnimationLeftEmojiAnimationBye, false);
				break;
			case EmojiType.Happy:
				leftEmojiAnimation.State.SetAnimation(0, CachedAnimationLeftEmojiAnimationHappy, false);
				break;
			case EmojiType.Hello:
				leftEmojiAnimation.State.SetAnimation(0, CachedAnimationLeftEmojiAnimationHello, false);
				break;
			case EmojiType.Scared:
				leftEmojiAnimation.State.SetAnimation(0, CachedAnimationLeftEmojiAnimationScared, false);
				break;
			}
		}
		else
		{
			rightEmojiBubbleAnimation.State.SetAnimation(0, CachedAnimationRightBubbleShow, false);
			rightEmojiBubbleAnimation.State.AddAnimation(0, CachedAnimationRightBubbleIdle, true, 0f);
			switch (value)
			{
			case EmojiType.Bye:
				rightEmojiAnimation.State.SetAnimation(0, CachedAnimationRightEmojiAnimationBye, false);
				break;
			case EmojiType.Happy:
				rightEmojiAnimation.State.SetAnimation(0, CachedAnimationRightEmojiAnimationHappy, false);
				break;
			case EmojiType.Hello:
				rightEmojiAnimation.State.SetAnimation(0, CachedAnimationRightEmojiAnimationHello, false);
				break;
			case EmojiType.Scared:
				rightEmojiAnimation.State.SetAnimation(0, CachedAnimationRightEmojiAnimationScared, false);
				break;
			}
		}
		LevelMultiplayer levelMultiplayer = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel as LevelMultiplayer;
		if ((levelMultiplayer.LocalPlayerCharacter == levelMultiplayer.CharacterFirst && isLeftBubble) || (levelMultiplayer.LocalPlayerCharacter == levelMultiplayer.CharacterSecond && !isLeftBubble))
		{
			DockButtonAction(CachedAnimationEmojiDockHide, CachedAnimationEmojiDockHidden);
			isDockShown = !isDockShown;
			dockContentTouchBlocker.enabled = !dockContentTouchBlocker.enabled;
			dockTouchBlocker.enabled = true;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				dockTouchBlocker.enabled = false;
			}, dockInactiveTimeAfterChoosingEmoji);
		}
	}

	private void DockButtonAction(Spine.Animation firstAnimation, Spine.Animation secondAnimation)
	{
		emojiDockAnimation.State.SetAnimation(0, firstAnimation, false);
		emojiDockAnimation.State.AddAnimation(0, secondAnimation, true, 0f);
		dockButton.enabled = false;
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			dockButton.enabled = true;
		}, firstAnimation.Duration);
	}

	private void LeftEmojiAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataHideLeftBubble)
		{
			leftEmojiBubbleAnimation.State.SetAnimation(0, CachedAnimationLeftBubbleHide, false);
		}
	}

	private void RightEmojiAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataHideRightBubble)
		{
			rightEmojiBubbleAnimation.State.SetAnimation(0, CachedAnimationRightBubbleHide, false);
		}
	}

	private void DockButton_OnClick()
	{
		if (!isDockShown)
		{
			DockButtonAction(CachedAnimationEmojiDockShow, CachedAnimationEmojiDockShown);
		}
		else
		{
			DockButtonAction(CachedAnimationEmojiDockHide, CachedAnimationEmojiDockHidden);
		}
		isDockShown = !isDockShown;
		dockContentTouchBlocker.enabled = !dockContentTouchBlocker.enabled;
	}

	private void EmojiButtonHello_OnClick()
	{
		ShowBubblesWithEmojiOfType(EmojiType.Hello, !SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsMultiplayerSecondaryPlayer);
		SingletonMonoBehaviour<MultiplayerManager>.Instance.SendEmojiToOpponent(EmojiType.Hello);
	}

	private void EmojiButtonScared_OnClick()
	{
		ShowBubblesWithEmojiOfType(EmojiType.Scared, !SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsMultiplayerSecondaryPlayer);
		SingletonMonoBehaviour<MultiplayerManager>.Instance.SendEmojiToOpponent(EmojiType.Scared);
	}

	private void EmojiButtonHappy_OnClick()
	{
		ShowBubblesWithEmojiOfType(EmojiType.Happy, !SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsMultiplayerSecondaryPlayer);
		SingletonMonoBehaviour<MultiplayerManager>.Instance.SendEmojiToOpponent(EmojiType.Happy);
	}

	private void EmojiButtonBye_OnClick()
	{
		ShowBubblesWithEmojiOfType(EmojiType.Bye, !SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsMultiplayerSecondaryPlayer);
		SingletonMonoBehaviour<MultiplayerManager>.Instance.SendEmojiToOpponent(EmojiType.Bye);
	}

	private void MultiplayerManager_OnEmojiReceived(EmojiType emojiType)
	{
		ShowBubblesWithEmojiOfType(emojiType, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsMultiplayerSecondaryPlayer);
	}
}
