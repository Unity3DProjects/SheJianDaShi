using Spine;
using UnityEngine;

public class TurnTimer : MonoBehaviour
{
	[SerializeField]
	private ProgressBar[] turnTimerProgressBars;

	[SerializeField]
	private tk2dTextMesh[] turnTimerLabels;

	[SerializeField]
	private SkeletonAnimation[] timerAnimations;

	[SerializeField]
	private SkeletonAnimation[] clockAnimations;

	private Spine.Animation cachedAnimationAppear;

	private Spine.Animation cachedAnimationDisappear;

	private Spine.Animation cachedAnimationIdle;

	public ProgressBar[] TurnTimerProgressBars
	{
		get
		{
			return turnTimerProgressBars;
		}
	}

	public SkeletonAnimation[] ClockAnimations
	{
		get
		{
			return clockAnimations;
		}
	}

	public SkeletonAnimation[] TimerAnimations
	{
		get
		{
			return timerAnimations;
		}
	}

	private Spine.Animation CachedAnimationAppear
	{
		get
		{
			if (cachedAnimationAppear == null)
			{
				cachedAnimationAppear = timerAnimations[0].Skeleton.Data.FindAnimation("appear");
			}
			return cachedAnimationAppear;
		}
	}

	public Spine.Animation CachedAnimationDisappear
	{
		get
		{
			if (cachedAnimationDisappear == null)
			{
				cachedAnimationDisappear = timerAnimations[0].Skeleton.Data.FindAnimation("disappear");
			}
			return cachedAnimationDisappear;
		}
	}

	private Spine.Animation CachedAnimationIdle
	{
		get
		{
			if (cachedAnimationIdle == null)
			{
				cachedAnimationIdle = timerAnimations[0].Skeleton.Data.FindAnimation("idle");
			}
			return cachedAnimationIdle;
		}
	}

	private void Awake()
	{
		if (tk2dSystem.IsRetina)
		{
			for (int i = 0; i < turnTimerLabels.Length; i++)
			{
				Vector3 localPosition = turnTimerLabels[i].transform.localPosition;
				turnTimerLabels[i].transform.localPosition = new Vector3(localPosition.x * 2f, localPosition.y, localPosition.z);
			}
		}
		for (int j = 0; j < timerAnimations.Length; j++)
		{
			timerAnimations[j].gameObject.SetActive(false);
		}
		for (int k = 0; k < ClockAnimations.Length; k++)
		{
			ClockAnimations[k].ignoreMonoUpdate = true;
		}
	}

	private void Start()
	{
		if (!SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsMultiplayerSecondaryPlayer)
		{
			for (int i = 0; i < timerAnimations.Length; i++)
			{
				timerAnimations[i].gameObject.SetActive(true);
				timerAnimations[i].State.SetAnimation(0, CachedAnimationIdle, true);
			}
		}
	}

	private void OnEnable()
	{
		Level.OnPlayerChanged += Level_OnPlayerChanged;
	}

	private void OnDisable()
	{
		Level.OnPlayerChanged -= Level_OnPlayerChanged;
	}

	private void Level_OnPlayerChanged(int player)
	{
		for (int i = 0; i < timerAnimations.Length; i++)
		{
			timerAnimations[i].gameObject.SetActive(true);
		}
		if (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType != GameType.Multiplayer)
		{
			return;
		}
		LevelMultiplayer levelMultiplayer = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel as LevelMultiplayer;
		GameScreen gameScreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.GameScreen;
		if (!levelMultiplayer.IsMainBattleFinished)
		{
			if (levelMultiplayer.LocalPlayerCharacter.Health > 0f && levelMultiplayer.EnemyCharacter.Health > 0f)
			{
				if (levelMultiplayer.ActiveCharacter == levelMultiplayer.LocalPlayerCharacter)
				{
					if (gameScreen != null)
					{
						gameScreen.SetTurnTimerFill(1f);
					}
					for (int j = 0; j < timerAnimations.Length; j++)
					{
						timerAnimations[j].State.SetAnimation(0, CachedAnimationAppear, false);
						timerAnimations[j].State.AddAnimation(0, CachedAnimationIdle, true, 0f);
					}
				}
				else
				{
					for (int k = 0; k < timerAnimations.Length; k++)
					{
						timerAnimations[k].State.SetAnimation(0, CachedAnimationDisappear, false);
					}
				}
				return;
			}
			for (int l = 0; l < timerAnimations.Length; l++)
			{
				if (timerAnimations[l].AnimationName == "idle")
				{
					timerAnimations[l].State.SetAnimation(0, CachedAnimationDisappear, false);
				}
			}
			return;
		}
		if (levelMultiplayer.CharacterFirst.IsFinishHim || levelMultiplayer.CharacterSecond.IsFinishHim)
		{
			if (levelMultiplayer.IsLevelFinished)
			{
				return;
			}
			if (levelMultiplayer.ActiveCharacter == levelMultiplayer.LocalPlayerCharacter && levelMultiplayer.LocalPlayerCharacter.Health > 0f)
			{
				if (gameScreen != null)
				{
					gameScreen.SetTurnTimerFill(1f);
				}
				for (int m = 0; m < timerAnimations.Length; m++)
				{
					timerAnimations[m].State.SetAnimation(0, CachedAnimationAppear, false);
					timerAnimations[m].State.AddAnimation(0, CachedAnimationIdle, true, 0f);
				}
				return;
			}
			for (int n = 0; n < timerAnimations.Length; n++)
			{
				if (timerAnimations[n].AnimationName == "idle")
				{
					timerAnimations[n].State.SetAnimation(0, CachedAnimationDisappear, false);
				}
			}
			return;
		}
		for (int num = 0; num < timerAnimations.Length; num++)
		{
			if (timerAnimations[num].AnimationName == "idle")
			{
				timerAnimations[num].State.SetAnimation(0, CachedAnimationDisappear, false);
			}
		}
	}
}
