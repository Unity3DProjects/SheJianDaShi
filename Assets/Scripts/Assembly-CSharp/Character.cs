using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class Character : CharacterBase
{
	protected const float HITTING_OFFSET = 800f;

	public const float FINISH_HIM_CAMERA_DELAY = 2.5f;

	[SerializeField]
	private Transform handAnchorTransform;

	[SerializeField]
	private Transform arrowAnchorTransform;

	[SerializeField]
	private HittingPobability[] hittingProbabilities;

	[SerializeField]
	private CharacterNameType characterNameType;

	[SerializeField]
	private WeaponType[] availableWeapons;

	[SerializeField]
	private string winningAnimationName = string.Empty;

	[SerializeField]
	private float endLevelDelay = 5f;

	private Vector3 weaponReleasePosition;

	private bool hasGreated;

	private bool isActive;

	private int shotsCount;

	private int missesCount;

	private Rect shootBounds = new Rect(-300f, 0f, 600f, 320f);

	private EventData spawnEnd;

	private EventData shotEnd;

	private EventData weaponVisible;

	private bool isKnockoutSoundPlayed;

	private bool isAI;

	public bool IsActive
	{
		get
		{
			return isActive;
		}
		set
		{
			if (value != isActive)
			{
				isActive = value;
				if (isActive)
				{
					RestoreArcher();
				}
			}
		}
	}

	public Transform WeaponAnchorTransform
	{
		get
		{
			return (level.CurrentWeapon.LaunchType != WeaponLaunchType.Bow) ? handAnchorTransform : arrowAnchorTransform;
		}
	}

	public bool IsAI
	{
		get
		{
			return isAI;
		}
		set
		{
			if (isAI == value)
			{
				return;
			}
			isAI = value;
			if (isAI)
			{
				TutorialManager cachedTutorialManager = base.CachedManagers.CachedTutorialManager;
				if (base.CachedManagers.CachedStatisticsManager.MathesCount < cachedTutorialManager.MatchesWithLowHealthEnemies)
				{
					health *= cachedTutorialManager.LowHealthEnemiesCoeff;
					initialHealth *= cachedTutorialManager.LowHealthEnemiesCoeff;
				}
			}
		}
	}

	public Rect ShootBounds
	{
		get
		{
			Rect result = shootBounds;
			result.x = base.CachedTransform.position.x + result.x;
			result.y = base.CachedTransform.position.y + result.y;
			return result;
		}
	}

	public WeaponType[] AvailableWeapons
	{
		get
		{
			return availableWeapons;
		}
	}

	public int MissesCount
	{
		get
		{
			return missesCount;
		}
		set
		{
			missesCount = value;
		}
	}

	public int ShotsCount
	{
		get
		{
			return shotsCount;
		}
	}

	public HittingPobability[] HittingProbabilities
	{
		get
		{
			return hittingProbabilities;
		}
		set
		{
			hittingProbabilities = value;
		}
	}

	public int ShotsCountWithIndividualHittingAfterRevieve { get; set; }

	protected bool IsKnockoutSoundPlayed
	{
		get
		{
			return isKnockoutSoundPlayed;
		}
		set
		{
			if (isKnockoutSoundPlayed != value)
			{
				isKnockoutSoundPlayed = value;
				if (isKnockoutSoundPlayed)
				{
					AudioManager.PlayAudioWithKey("knockout");
				}
			}
		}
	}

	public bool IsMultiplayerEnemy { get; set; }

	private bool IsAnyAnimationRunning
	{
		get
		{
			return animationCache.CurrentAnimation != null && animationCache.CurrentAnimation != animationCache.IdleAnimation;
		}
	}

	public CharacterNameType CharacterNameType
	{
		get
		{
			return characterNameType;
		}
	}

	public float EndLevelDelay
	{
		get
		{
			return endLevelDelay;
		}
	}

	private EventData SpawnEnd
	{
		get
		{
			if (spawnEnd == null)
			{
				spawnEnd = animationCache.AnimationState.Data.SkeletonData.FindEvent("Spawn_End");
			}
			return spawnEnd;
		}
	}

	private EventData ShotEnd
	{
		get
		{
			if (shotEnd == null)
			{
				shotEnd = animationCache.AnimationState.Data.SkeletonData.FindEvent("Shot_End");
			}
			return shotEnd;
		}
	}

	private EventData WeaponVisible
	{
		get
		{
			if (weaponVisible == null)
			{
				weaponVisible = animationCache.AnimationState.Data.SkeletonData.FindEvent("Weapon_Visible");
			}
			return weaponVisible;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		UpdateWeaponReleasePosition();
	}

	public void SetCurrentAngleAndVelocity(ShootingInfo info, float stretchRatio, bool isAnimated = false)
	{
		WeaponGeneral currentWeapon = level.CurrentWeapon;
		if (!isAnimated && level.GameType == GameType.Multiplayer)
		{
			(level as LevelMultiplayer).WeaponAiming(currentWeapon);
		}
		if (info.angle > 90f)
		{
			info.angle -= 360f;
		}
		base.IsFlipped = info.angle < -90f;
		if (base.IsFlipped)
		{
			info.angle = 0f - info.angle - 180f;
		}
		UpdateAimingAnimationWithRatioAndAngle(stretchRatio, info.angle, isAnimated);
		if (currentWeapon != null)
		{
			currentWeapon.SetShootingInfo(WeaponAnchorTransform.position, info);
		}
	}

	public void ResetHittingSettings(int forcedShotsCount = 0)
	{
		shotsCount = forcedShotsCount;
	}

	public void PlayHappyAnimation()
	{
		if (!string.IsNullOrEmpty(winningAnimationName))
		{
			base.SkeletonAnimation.State.SetAnimation(0, winningAnimationName, false);
			AddAnimation(0, "win", true);
		}
		else
		{
			SetAnimation(0, "win", true);
		}
	}

	public void PlayLoseAnimation()
	{
		SetAnimation(0, "lose", true);
	}

	public void PlayReleaseAnimation()
	{
		animationCache.AnimationState.SetAnimation(0, animationCache.ShotAnimation(level.CurrentWeapon.LaunchType), false);
		animationCache.AnimationState.AddAnimation(0, animationCache.IdleAnimation, true, 0f);
	}

	public void PlayTwitchAnimation()
	{
		animationCache.AnimationState.SetAnimation(0, animationCache.TwitchingAnimation(), true);
	}

	public ShootingInfo GetShootingInfo(bool isPlayerLoses, bool isIndividualProbability = false)
	{
		float hittingProbability = 0f;
		bool autoActivate = false;
		if (base.CachedManagers.CachedStatisticsManager.WinsCount < base.CachedManagers.CachedTutorialManager.WinsToDisableEasyMatches && isPlayerLoses)
		{
			hittingProbability = UnityEngine.Random.Range(0.45f, 0.55f);
		}
		else if (ShotsCountWithIndividualHittingAfterRevieve > 0)
		{
			ShotsCountWithIndividualHittingAfterRevieve--;
			ProposalManager cachedProposalManager = base.CachedManagers.CachedProposalManager;
			hittingProbability = UnityEngine.Random.Range(cachedProposalManager.MinProbabilityAfterMoreLife, cachedProposalManager.MaxProbabilityAfterMoreLife);
		}
		else
		{
			shotsCount++;
			for (int i = 0; i < hittingProbabilities.Length; i++)
			{
				if (i != hittingProbabilities.Length - 1)
				{
					if (shotsCount <= hittingProbabilities[i].shotCount)
					{
						hittingProbability = UnityEngine.Random.Range(hittingProbabilities[i].minProbability, hittingProbabilities[i].maxProbability);
						break;
					}
				}
				else if (shotsCount >= hittingProbabilities[i].shotCount)
				{
					hittingProbability = UnityEngine.Random.Range(hittingProbabilities[i].minProbability, hittingProbabilities[i].maxProbability);
					break;
				}
			}
			autoActivate = true;
		}
		if (isIndividualProbability || level.InactiveArcher.IsFinishHim)
		{
			hittingProbability = 1f;
		}
		return GetShootingInfo(hittingProbability, autoActivate);
	}

	public float ShootingAngle(Vector3 enemyPosition)
	{
		Vector3 vector = WeaponReleasePosition();
		List<Vector3> highChangePositionsForRange = level.Location.GetHighChangePositionsForRange(enemyPosition.x, vector.x);
		highChangePositionsForRange.Add(enemyPosition);
		float num = float.MaxValue;
		foreach (Vector3 item in highChangePositionsForRange)
		{
			float num2 = Mathf.Atan2(item.y - vector.y, enemyPosition.x - vector.x);
			if (vector.x > enemyPosition.x && num2 > 0f)
			{
				num2 -= (float)Math.PI * 2f;
			}
			if (num == float.MaxValue || Mathf.Abs(num2) > Mathf.Abs(num))
			{
				num = num2;
			}
		}
		num *= 57.29578f;
		if (vector.x > enemyPosition.x)
		{
			return UnityEngine.Random.Range(num - 10f, num - 50f);
		}
		return UnityEngine.Random.Range(num + 10f, num + 50f);
	}

	public ShootingInfo GetShootingInfo(float hittingProbability, bool autoActivate)
	{
		float num = ((UnityEngine.Random.Range(0, 2) != 0) ? (-1f) : 1f);
		if (hittingProbability >= 0.85f && level.GameType == GameType.Tournament)
		{
			TournamentManager cachedTournamentManager = base.CachedManagers.CachedTournamentManager;
			TournamentDifficultType difficultType = cachedTournamentManager.CurrentTournament.settings.difficultType;
			if ((difficultType == TournamentDifficultType.Medium || difficultType == TournamentDifficultType.Hard) && UnityEngine.Random.Range(0f, 1f) <= cachedTournamentManager.HittingOffsetProbability)
			{
				num = ((difficultType != TournamentDifficultType.Medium) ? 1f : (-1f));
				hittingProbability = cachedTournamentManager.HittingOffset;
			}
		}
		float num2 = num * 800f * (1f - hittingProbability);
		Vector3 vector;
		if (level.GameType == GameType.AppleShoot)
		{
			LevelAppleShoot levelAppleShoot = level as LevelAppleShoot;
			vector = new Vector3(levelAppleShoot.CurrentApple.transform.position.x, levelAppleShoot.CurrentApple.transform.position.y);
		}
		else
		{
			vector = new Vector3(level.InactiveArcher.BodyTransform.position.x, level.InactiveArcher.BodyTransform.position.y + num2);
		}
		float angle = ShootingAngle(vector);
		Vector3 startPosition = WeaponReleasePosition(angle);
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		ShootingInfo result = level.CurrentWeapon.GetShootingInfo(startPosition, vector, angle);
		if (level.GameType == GameType.VSComputer && (cachedStatisticsManager.MathesCount == 0 || SingletonMonoBehaviour<TutorialManager>.Instance.IsVSCompTutorialFinished) && !IsAI)
		{
			result = level.CurrentWeapon.GetShootingInfoForMaxForce(startPosition, vector, level.CurrentWeapon.MaxSpeed);
		}
		else if (level.GameType == GameType.AppleShoot && cachedStatisticsManager.FruitsSmashed == 0)
		{
			result = level.CurrentWeapon.GetShootingInfoForApple(startPosition, vector, level.CurrentWeapon.MaxSpeed);
		}
		if (hittingProbability >= 0.9f)
		{
			result.type = ShootType.ShootAtTarget;
		}
		else if (num > 0f)
		{
			result.type = ShootType.Overshoot;
		}
		else
		{
			result.type = ShootType.Undershoot;
		}
		result.autoActivation = autoActivate && IsAI;
		return result;
	}

	public Vector3 WeaponReleasePosition()
	{
		if (level.CurrentWeapon.LaunchType == WeaponLaunchType.Hand)
		{
			Vector3 vector = weaponReleasePosition;
			if (base.IsFlipped)
			{
				vector.Scale(new Vector3(-1f, 1f, 1f));
			}
			return base.CachedTransform.position + vector;
		}
		return WeaponAnchorTransform.position;
	}

	public Vector3 WeaponReleasePosition(float angle)
	{
		SetCurrentAngleAndVelocity(new ShootingInfo(angle, 0f), 0f);
		return WeaponReleasePosition();
	}

	public void UserAimingActionDetected()
	{
		Spine.Animation animation = animationCache.DrawAnimation(level.CurrentWeapon.LaunchType);
		TrackEntry trackEntry = animationCache.AnimationState.SetAnimation(0, animation, false);
		if (trackEntry != null)
		{
			trackEntry.Time = 0.9f;
		}
	}

	public void StartInactivityAnimation()
	{
		CharacterAnimationType[] array = new CharacterAnimationType[3]
		{
			CharacterAnimationType.EmoteTaunt,
			CharacterAnimationType.EmoteAnger,
			CharacterAnimationType.Win
		};
		CharacterAnimationType characterAnimationType = array[UnityEngine.Random.Range(0, array.Length)];
		if (animationCache.SetAnimation(characterAnimationType))
		{
			BeginCharge(true);
			level.CurrentWeapon.IsWeaponVisible = false;
		}
	}

	public void SetAnimationWithType(CharacterAnimationType type, float probabilty = 100f)
	{
		if (!base.RootRigidBody && !IsAnyAnimationRunning)
		{
			animationCache.SetAnimation(type, probabilty, CharacterAnimationType.Idle);
		}
	}

	public void KillCharacter()
	{
		ApplyDamageAnimation();
		ApplyCharacterRagdoll();
		Vector3 force = new Vector3(100f, 0f, 0f);
		base.RootRigidBody.AddForce(force, ForceMode.VelocityChange);
		if (IsAI || IsMultiplayerEnemy || level.GameType == GameType.VSHuman)
		{
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
			{
				AudioManager.PlayAudioWithKey("crowd_glee");
				AudioManager.PlayAudioWithKey("crowd_loop");
			}, 1.5f + level.CameraMoveToWinnerDuration / 2f);
		}
		level.OnCharacterDead(this);
		IsKnockoutSoundPlayed = true;
		base.CachedManagers.CachedGameManager.CameraController.RemoveBlur();
	}

	public void Revive(int percents)
	{
		isFinishHim = false;
		AddLifePercent((float)percents / 100f);
		RestoreArcher();
		base.IsSuicide = false;
	}

	private void UpdateAimingAnimationWithRatioAndAngle(float ratio, float angle, bool isAnimated)
	{
		float num = (angle + 57f) / 146f;
		float num2 = num * animationCache.AimAngleAnimation.Duration;
		TrackEntry trackEntry = animationCache.AnimationState.SetAnimation(1, animationCache.AimAngleAnimation, false);
		if (trackEntry != null)
		{
			if (isAnimated)
			{
				trackEntry.Time = 0.2f;
				trackEntry.LastTime = 0.2f;
			}
			else
			{
				trackEntry.Time = num2;
			}
			trackEntry.EndTime = num2;
		}
	}

	public void BeginCharge(bool isNeedToAdd = false)
	{
		Spine.Animation animation = animationCache.DrawAnimation(level.CurrentWeapon.LaunchType);
		if (isNeedToAdd)
		{
			animationCache.AnimationState.AddAnimation(0, animation, false, 0f);
		}
		else
		{
			animationCache.AnimationState.SetAnimation(0, animation, false);
		}
		level.EnableDragging();
		level.SpawnMisteryBox();
	}

	public void RestoreArcher()
	{
		if (health <= 0f)
		{
			health = -0.5f;
			if ((level.GameType == GameType.VSComputer || level.GameType == GameType.Tournament) && this == level.CharacterFirst && base.CachedManagers.CachedProposalManager.IsAvalaibleMoreLife)
			{
				((LevelVSComputer)level).ShowRevivePopup();
				base.CachedManagers.CachedGameManager.IsGamePaused = true;
				return;
			}
			level.ForceEmittedObjectsCollidersDisable();
			base.IsFinishHim = true;
		}
		if (isRestoreNeeded && (bool)base.RootRigidBody)
		{
			ragdoll.mix = 0f;
			ragdoll.Remove();
		}
		if (base.IsFinishHim)
		{
			if (!base.IsSuicide)
			{
				level.TurnFinished(2.5f);
			}
		}
		else if (!hasGreated && level.IsTurnable)
		{
			StartGreetingAnimation();
			hasGreated = true;
		}
		else if (!level.IsLevelFinished)
		{
			StartOnTurnBeginAnimations();
		}
	}

	public void TimeOutWithPrepareShooting()
	{
		if (level.IsCharacterOnScreen(this))
		{
			animationCache.SetAnimation(CharacterAnimationType.EmoteAnger, 100f, CharacterAnimationType.Idle);
		}
	}

	private void UpdateWeaponReleasePosition()
	{
		weaponReleasePosition = new Vector3(37f, 155f, 0f);
		if (characterType == CharacterType.Beefy)
		{
			weaponReleasePosition = new Vector3(77f, 194f, 0f);
		}
		else if (characterType == CharacterType.Plumpy)
		{
			weaponReleasePosition = new Vector3(49f, 149f, 0f);
		}
	}

	private void StartOnTurnBeginAnimations()
	{
		CharacterAnimationType characterAnimationType = ((!(lastTurnDamageAmount > 1f)) ? CharacterAnimationType.EmoteTaunt : CharacterAnimationType.EmoteAnger);
		if (base.Health < initialHealth * 0.3f)
		{
			characterAnimationType = CharacterAnimationType.EmoteScare;
		}
		bool isNeedToAdd = animationCache.SetAnimation(characterAnimationType, 0f);
		if (!base.IsSuicide)
		{
			BeginCharge(isNeedToAdd);
		}
		else
		{
			animationCache.AnimationState.SetAnimation(0, animationCache.IdleAnimation, true);
		}
		lastTurnDamageAmount = 0f;
	}

	private void StartRandomAfterShootingAnimation()
	{
		if (level.IsCharacterOnScreen(this))
		{
			CharacterAnimationType[] array = new CharacterAnimationType[3]
			{
				CharacterAnimationType.EmoteTaunt,
				CharacterAnimationType.EmoteAnger,
				CharacterAnimationType.Win
			};
			CharacterAnimationType characterAnimationType = array[UnityEngine.Random.Range(0, array.Length)];
			animationCache.SetAnimation(characterAnimationType, 30f, CharacterAnimationType.Idle);
		}
	}

	protected override void AnimationState_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		base.AnimationState_Event(state, trackIndex, e);
		if (e.Data == SpawnEnd)
		{
			if (level.IsTurnable)
			{
				BeginCharge();
			}
		}
		else if (e.Data == ShotEnd)
		{
			level.IsActiveForcedSwitchPlayerTimer = false;
			shotsCount++;
			WeaponGeneral currentWeapon = level.CurrentWeapon;
			if (currentWeapon != null)
			{
				level.CurrentWeapon.PrepareBeforeShoot(this);
				level.CurrentWeapon.Shoot();
			}
			if (level.GameType != GameType.Multiplayer)
			{
				StartRandomAfterShootingAnimation();
			}
		}
		else if (e.Data == WeaponVisible)
		{
			WeaponGeneral currentWeapon2 = level.CurrentWeapon;
			if (currentWeapon2 != null)
			{
				level.CurrentWeapon.IsWeaponVisible = true;
			}
		}
	}

	protected override void FatalityManager_OnFatalityEnd(WeaponType type)
	{
		base.FatalityManager_OnFatalityEnd(type);
		level.OnCharacterDead(this);
	}

	public override bool OnCollision(CollisionInfo collisionInfo)
	{
		bool result = base.OnCollision(collisionInfo);
		if ((level.GameType == GameType.VSComputer || level.GameType == GameType.Tournament) && level.ActiveCharacter == level.CharacterFirst)
		{
			BodyPartsHelper.BodyPartsType bodyPartsType = BodyPartsHelper.TypeByName(collisionInfo.boneName);
			if (bodyPartsType != BodyPartsHelper.BodyPartsType.BodyPartsTypeNone)
			{
				level.IncreaseShotStatisticsForBodyType(bodyPartsType);
			}
		}
		LevelVSComputer levelVSComputer = level as LevelVSComputer;
		if (levelVSComputer != null)
		{
			((LevelVSComputer)level).ResetShotSettings();
		}
		return result;
	}
}
