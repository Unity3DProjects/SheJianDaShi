using Spine;
using UnityEngine;

public class CharacterAnimationCache
{
	public const string IDLE_ANIMATION_NAME = "idle";

	public const string DEFAULT_ANIMATION_NAME = "default";

	public const string FACE_KO_ANIMATION_NAME = "face_ko";

	public const string FACE_SCREAM_ANIMATION_NAME = "face_scream";

	public const string FACE_HIT_ANIMATION_NAME = "face_hit";

	public const string FACE_DEFAULT_ANIMATION_NAME = "face_default";

	public const string SPAWN_ANIMATION_NAME = "spawn";

	public const string STAND_UP_ANIMATION_NAME = "stand_up";

	public const string LOSE_ANIMATION_NAME = "lose";

	public const string ANGER_ANIMATION_NAME = "anger";

	public const string SCARE_ANIMATION_NAME = "scare";

	public const string FINISH_HIM_ANIMATION_NAME = "finish_him";

	public const string TAUNT_ANIMATION_NAME = "taunt";

	public const string WIN_ANIMATION_NAME = "win";

	public const string AIM_ANIMATION_NAME = "aim";

	public const string SHOCK_ANIMATION_NAME = "shock";

	public const string SHOCK_IDLE_ANIMATION_NAME = "shock_idle";

	public const string FIRE_ANIMATION_NAME = "fire";

	public const string DEATH_SHOCK_ANIMATION_NAME = "death_by_shock";

	public const string DEATH_FIRE_ANIMATION_NAME = "death_by_fire";

	public const string DEATH_SHARK_ANIMATION_NAME = "fatality_small_shark";

	private string idleAnimationName = "idle";

	private SkeletonAnimation skeletonAnimation;

	private Spine.Animation idleAnimation;

	private Spine.Animation defaultAnimation;

	private Spine.Animation faceKOAnimation;

	private Spine.Animation faceScreamAnimation;

	private Spine.Animation faceHitAnimation;

	private Spine.Animation faceDefaultAnimation;

	private Spine.Animation spawnAnimation;

	private Spine.Animation standUpAnimation;

	private Spine.Animation loseAnimation;

	private Spine.Animation angerAnimation;

	private Spine.Animation scareAnimation;

	private Spine.Animation tauntAnimation;

	private Spine.Animation winAnimation;

	private Spine.Animation aimAngleAnimation;

	private Spine.Animation drawBowAnimation;

	private Spine.Animation drawGrenadeAnimation;

	private Spine.Animation drawTwitchSlowAnimation;

	private Spine.Animation drawTwitchFastAnimation;

	private Spine.Animation shotBowAnimation;

	private Spine.Animation shotGrenadeAnimation;

	private Spine.Animation fireAnimation;

	private Spine.Animation shockAnimation;

	private Spine.Animation shockIdleAnimation;

	private Spine.Animation deathFireAnimation;

	private Spine.Animation deathShockAnimation;

	private Spine.Animation deathSharkAnimation;

	public string IdleAnimationName
	{
		get
		{
			return idleAnimationName;
		}
		set
		{
			idleAnimationName = value;
		}
	}

	public SkeletonAnimation SkeletonAnimation
	{
		get
		{
			return skeletonAnimation;
		}
		set
		{
			if (value != skeletonAnimation)
			{
				skeletonAnimation = value;
			}
		}
	}

	public Skeleton AnimationSkeleton
	{
		get
		{
			return skeletonAnimation.Skeleton;
		}
	}

	public Spine.AnimationState AnimationState
	{
		get
		{
			return skeletonAnimation.State;
		}
	}

	public virtual Spine.Animation IdleAnimation
	{
		get
		{
			if (idleAnimation == null)
			{
				idleAnimation = AnimationState.Data.SkeletonData.FindAnimation(idleAnimationName);
			}
			return idleAnimation;
		}
	}

	public Spine.Animation DefaultAnimation
	{
		get
		{
			if (defaultAnimation == null)
			{
				defaultAnimation = AnimationState.Data.SkeletonData.FindAnimation("default");
			}
			return defaultAnimation;
		}
	}

	public Spine.Animation FaceKOAnimation
	{
		get
		{
			if (faceKOAnimation == null)
			{
				faceKOAnimation = AnimationState.Data.SkeletonData.FindAnimation("face_ko");
			}
			return faceKOAnimation;
		}
	}

	public Spine.Animation FaceScreamAnimation
	{
		get
		{
			if (faceScreamAnimation == null)
			{
				faceScreamAnimation = AnimationState.Data.SkeletonData.FindAnimation("face_scream");
			}
			return faceScreamAnimation;
		}
	}

	public Spine.Animation FaceHitAnimation
	{
		get
		{
			if (faceHitAnimation == null)
			{
				faceHitAnimation = AnimationState.Data.SkeletonData.FindAnimation("face_hit");
			}
			return faceHitAnimation;
		}
	}

	public Spine.Animation FaceDefaultAnimation
	{
		get
		{
			if (faceDefaultAnimation == null)
			{
				faceDefaultAnimation = AnimationState.Data.SkeletonData.FindAnimation("face_default");
			}
			return faceDefaultAnimation;
		}
	}

	public Spine.Animation SpawnAnimation
	{
		get
		{
			if (spawnAnimation == null)
			{
				spawnAnimation = AnimationState.Data.SkeletonData.FindAnimation("spawn");
			}
			return spawnAnimation;
		}
	}

	public Spine.Animation StandUpAnimation
	{
		get
		{
			if (standUpAnimation == null)
			{
				standUpAnimation = AnimationState.Data.SkeletonData.FindAnimation("stand_up");
			}
			return standUpAnimation;
		}
	}

	public Spine.Animation LoseAnimation
	{
		get
		{
			if (loseAnimation == null)
			{
				loseAnimation = AnimationState.Data.SkeletonData.FindAnimation("lose");
			}
			return loseAnimation;
		}
	}

	public Spine.Animation AngerAnimation
	{
		get
		{
			if (angerAnimation == null)
			{
				angerAnimation = AnimationState.Data.SkeletonData.FindAnimation("anger");
			}
			return angerAnimation;
		}
	}

	public Spine.Animation ScareAnimation
	{
		get
		{
			if (scareAnimation == null)
			{
				scareAnimation = AnimationState.Data.SkeletonData.FindAnimation("scare");
			}
			return scareAnimation;
		}
	}

	public Spine.Animation FinishHimAnimation
	{
		get
		{
			return AnimationState.Data.SkeletonData.FindAnimation("finish_him");
		}
	}

	public Spine.Animation TauntAnimation
	{
		get
		{
			if (tauntAnimation == null)
			{
				tauntAnimation = AnimationState.Data.SkeletonData.FindAnimation("taunt");
			}
			return tauntAnimation;
		}
	}

	public Spine.Animation WinAnimation
	{
		get
		{
			if (winAnimation == null)
			{
				winAnimation = AnimationState.Data.SkeletonData.FindAnimation("win");
			}
			return winAnimation;
		}
	}

	public Spine.Animation AimAngleAnimation
	{
		get
		{
			if (aimAngleAnimation == null)
			{
				aimAngleAnimation = AnimationState.Data.SkeletonData.FindAnimation("aim");
			}
			return aimAngleAnimation;
		}
	}

	public Spine.Animation ShockAnimation
	{
		get
		{
			if (shockAnimation == null)
			{
				shockAnimation = AnimationState.Data.SkeletonData.FindAnimation("shock");
			}
			return shockAnimation;
		}
	}

	public Spine.Animation ShockIdleAnimation
	{
		get
		{
			if (shockIdleAnimation == null)
			{
				shockIdleAnimation = AnimationState.Data.SkeletonData.FindAnimation("shock_idle");
			}
			return shockIdleAnimation;
		}
	}

	public Spine.Animation FireAnimation
	{
		get
		{
			if (fireAnimation == null)
			{
				fireAnimation = AnimationState.Data.SkeletonData.FindAnimation("fire");
			}
			return fireAnimation;
		}
	}

	public Spine.Animation DeathShockAnimation
	{
		get
		{
			if (deathShockAnimation == null)
			{
				deathShockAnimation = AnimationState.Data.SkeletonData.FindAnimation("death_by_shock");
			}
			return deathShockAnimation;
		}
	}

	public Spine.Animation DeathFireAnimation
	{
		get
		{
			if (deathFireAnimation == null)
			{
				deathFireAnimation = AnimationState.Data.SkeletonData.FindAnimation("death_by_fire");
			}
			return deathFireAnimation;
		}
	}

	public Spine.Animation DeathSharkAnimation
	{
		get
		{
			if (deathSharkAnimation == null)
			{
				deathSharkAnimation = AnimationState.Data.SkeletonData.FindAnimation("fatality_small_shark");
			}
			return deathSharkAnimation;
		}
	}

	public Spine.Animation CurrentAnimation
	{
		get
		{
			TrackEntry current = AnimationState.GetCurrent(0);
			if (current != null)
			{
				return current.Animation;
			}
			return null;
		}
	}

	private Spine.Animation AnimationByType(CharacterAnimationType type)
	{
		Spine.Animation result = null;
		switch (type)
		{
		case CharacterAnimationType.None:
			result = null;
			break;
		case CharacterAnimationType.Idle:
			result = IdleAnimation;
			break;
		case CharacterAnimationType.Spawn:
			result = SpawnAnimation;
			break;
		case CharacterAnimationType.EmoteTaunt:
			result = TauntAnimation;
			break;
		case CharacterAnimationType.EmoteScare:
			result = ScareAnimation;
			break;
		case CharacterAnimationType.EmoteAnger:
			result = AngerAnimation;
			break;
		case CharacterAnimationType.Lose:
			result = LoseAnimation;
			break;
		case CharacterAnimationType.Win:
			result = WinAnimation;
			break;
		}
		return result;
	}

	public Spine.Animation DrawAnimation(WeaponLaunchType launchType)
	{
		Spine.Animation result = null;
		switch (launchType)
		{
		case WeaponLaunchType.Bow:
			if (drawBowAnimation == null)
			{
				drawBowAnimation = AnimationState.Data.SkeletonData.FindAnimation("bow_draw");
			}
			result = drawBowAnimation;
			break;
		case WeaponLaunchType.Hand:
			if (drawGrenadeAnimation == null)
			{
				drawGrenadeAnimation = AnimationState.Data.SkeletonData.FindAnimation("grenade_draw");
			}
			result = drawGrenadeAnimation;
			break;
		}
		return result;
	}

	public Spine.Animation TwitchingAnimation()
	{
		Spine.Animation animation = null;
		if (drawTwitchFastAnimation == null)
		{
			drawTwitchFastAnimation = AnimationState.Data.SkeletonData.FindAnimation("bow_draw_shaking");
		}
		return drawTwitchFastAnimation;
	}

	public Spine.Animation ShotAnimation(WeaponLaunchType launchType)
	{
		Spine.Animation result = null;
		switch (launchType)
		{
		case WeaponLaunchType.Bow:
			if (shotBowAnimation == null)
			{
				shotBowAnimation = AnimationState.Data.SkeletonData.FindAnimation("bow_shot");
			}
			result = shotBowAnimation;
			break;
		case WeaponLaunchType.Hand:
			if (shotGrenadeAnimation == null)
			{
				shotGrenadeAnimation = AnimationState.Data.SkeletonData.FindAnimation("grenade_shot");
			}
			result = shotGrenadeAnimation;
			break;
		}
		return result;
	}

	public Spine.Animation FatalityAnimation()
	{
		Spine.Animation animation = null;
		if (Random.value > 0.5f)
		{
			return DeathFireAnimation;
		}
		return DeathShockAnimation;
	}

	public bool SetAnimation(CharacterAnimationType type, float probability = 100f, CharacterAnimationType nextAnimationtype = CharacterAnimationType.None)
	{
		bool result = false;
		float num = Random.Range(0, 100);
		if (num <= probability)
		{
			Spine.Animation animation = AnimationByType(type);
			if (animation != null)
			{
				result = true;
				AnimationState.SetAnimation(0, animation, false);
				switch (nextAnimationtype)
				{
				case CharacterAnimationType.Idle:
					AnimationState.AddAnimation(0, IdleAnimation, true, 0f);
					break;
				case CharacterAnimationType.None:
				{
					Spine.Animation animation2 = AnimationByType(nextAnimationtype);
					if (animation2 != null)
					{
						AnimationState.AddAnimation(0, animation2, false, 0f);
					}
					break;
				}
				}
			}
		}
		return result;
	}

	public Spine.Animation AnimationByName(string animationName)
	{
		Spine.Animation result = null;
		switch (animationName)
		{
		case "":
			result = null;
			break;
		case "idle":
			result = IdleAnimation;
			break;
		case "default":
			result = DefaultAnimation;
			break;
		case "face_ko":
			result = FaceKOAnimation;
			break;
		case "face_scream":
			result = FaceScreamAnimation;
			break;
		case "face_hit":
			result = FaceHitAnimation;
			break;
		case "spawn":
			result = SpawnAnimation;
			break;
		case "stand_up":
			result = StandUpAnimation;
			break;
		case "lose":
			result = LoseAnimation;
			break;
		case "anger":
			result = AngerAnimation;
			break;
		case "scare":
			result = ScareAnimation;
			break;
		case "finish_him":
			result = FinishHimAnimation;
			break;
		case "taunt":
			result = TauntAnimation;
			break;
		case "win":
			result = WinAnimation;
			break;
		case "aim":
			result = AimAngleAnimation;
			break;
		case "shock":
			result = ShockAnimation;
			break;
		case "shock_idle":
			result = ShockIdleAnimation;
			break;
		case "fire":
			result = FireAnimation;
			break;
		case "death_by_shock":
			result = DeathShockAnimation;
			break;
		case "death_by_fire":
			result = DeathFireAnimation;
			break;
		case "fatality_small_shark":
			result = DeathSharkAnimation;
			break;
		}
		return result;
	}
}
