using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelAppleShoot : Level
{
	private const int MISSES_FOR_LOSE = 3;

	[SerializeField]
	private GameObject applesParent;

	[SerializeField]
	private WeaponType[] availableWeapons;

	[SerializeField]
	private float characterStepValue;

	[SerializeField]
	private float characterDistanceMinBorder;

	[SerializeField]
	private float characterDistanceMaxBorder;

	[SerializeField]
	private float characterWithinBorderMinStep;

	[SerializeField]
	private float characterWithinBorderMaxStep;

	[SerializeField]
	private int scoreForSkillShotAchievement;

	[SerializeField]
	private LevelObjectType[] appleTypes;

	[SerializeField]
	private LevelObjectType[] forcedAppleTypes;

	[SerializeField]
	private bool isForcedAppleTypesUse;

	private CharacterBase targetCharacter;

	private Apple currentApple;

	private GameObject cameraTarget;

	private int appleMissesCount = -1;

	private bool wasAppleDestroyed;

	private bool isFirstTurn = true;

	public WeaponType[] AvailableWeapons
	{
		get
		{
			List<WeaponType> list = new List<WeaponType>();
			List<WeaponType> list2 = base.CachedManagers.CachedShopManager.AvailableWeapons;
			for (int i = 0; i < availableWeapons.Length; i++)
			{
				WeaponType item = availableWeapons[i];
				if (list2.Contains(item))
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}
	}

	private float CurrentCameraSize
	{
		get
		{
			return cameraSettings.size;
		}
	}

	protected override WeaponUnfollowBounds WeaponTargetUnfollowBounds
	{
		get
		{
			WeaponUnfollowBounds result = default(WeaponUnfollowBounds);
			bool flag = targetCharacter.transform.position.x > characterFirst.transform.position.x;
			CharacterBase characterBase = ((!flag) ? targetCharacter : characterFirst);
			CharacterBase characterBase2 = ((!flag) ? characterFirst : targetCharacter);
			result.min = characterBase.transform.position.x - weaponDeactivationDistance;
			result.max = characterBase2.transform.position.x + weaponDeactivationDistance;
			return result;
		}
	}

	public int AppleMissesCount
	{
		get
		{
			return appleMissesCount;
		}
		set
		{
			if (appleMissesCount != value)
			{
				appleMissesCount = value;
				if (gameScreen != null)
				{
					gameScreen.UpdateAppleMisses(appleMissesCount);
				}
			}
		}
	}

	public bool IsAppleBoyWasDamaged { get; set; }

	public int HighScoreFruitsSmashed { get; set; }

	public Apple CurrentApple
	{
		get
		{
			return currentApple;
		}
	}

	protected override int MatchesForFloorSettings
	{
		get
		{
			return (base.CachedManagers.CachedStatisticsManager.FruitsSmashed != 0) ? 1 : 0;
		}
	}

	public override void Init(ObjectsSpawner objectsSpawner, CameraController cameraController)
	{
		base.Init(objectsSpawner, cameraController);
		base.ActiveCharacter.IsInvincible = true;
		base.ActiveCharacter.SetCollidersEnabled(false, true);
		multiplier = ((!tk2dSystem.IsRetina) ? 1f : 2f);
		cameraTarget = new GameObject();
		cameraTarget.transform.parent = base.transform;
		targetCharacter = CreateAppleBoy();
		targetCharacter.PlayIdleAnimation();
		NavigateToCharacter(targetCharacter, 0f);
		characterDistanceMaxBorder = 3700f;
		characterWithinBorderMinStep = 500f;
		characterWithinBorderMaxStep = 1000f;
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				NextTurn();
			}
		}, 2f);
	}

	public override void UpdateLevel(float deltaTime)
	{
		base.UpdateLevel(deltaTime);
		if ((bool)targetCharacter)
		{
			targetCharacter.UpdateCharacter(deltaTime);
		}
		if ((bool)targetCharacter && (bool)targetCharacter.RootRigidBody)
		{
			UnfollowWeaponWithVelocity(Vector2.zero);
		}
	}

	public override void Explode(ExplosionInfo explosionInfo)
	{
		base.Explode(explosionInfo);
		if ((bool)targetCharacter && !targetCharacter.IsInvincible && targetCharacter.IsAffectedInRadius(explosionInfo.position, explosionInfo.radius))
		{
			targetCharacter.ApplyExplosionForce(explosionInfo);
		}
		if (currentApple.IsAffectedInRadius(explosionInfo.position, explosionInfo.radius))
		{
			currentApple.ApplyDamage();
		}
	}

	public void DestroyAppleJoint()
	{
		if (currentApple != null)
		{
			currentApple.DestroyJoint();
		}
	}

	public override void UnfollowWeaponWithVelocity(Vector2 velocity, float duration = 3f)
	{
		Transform bodyTransform = cameraTarget.transform;
		Vector2 offset = Vector2.zero;
		if ((bool)targetCharacter.RootRigidBody)
		{
			bodyTransform = targetCharacter.BodyTransform;
			offset = base.CharacterOffset;
			cameraController.SetScaleWithDuration(1f, 1f);
		}
		else
		{
			bodyTransform.position = NewCameraPosition();
			cameraController.SetScaleWithDuration(NewCameraScale(), 1f);
		}
		if (cameraController.Target != bodyTransform)
		{
			cameraController.SetTarget(bodyTransform, velocity, offset, duration);
		}
		cameraTargetWeapon = null;
	}

	protected override void CreateObjectsForTurn()
	{
		base.CreateObjectsForTurn();
		if (!isFirstTurn && wasAppleDestroyed && (!currentApple || !currentApple.OnHead))
		{
			base.CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching((!IsAppleBoyWasDamaged) ? MicroachievementType.AppleHitPerfect : MicroachievementType.AppleHitUsual);
		}
		isFirstTurn = false;
		IsAppleBoyWasDamaged = false;
		CreateApple();
	}

	protected override WeaponType CreateWeapon(WeaponType type = WeaponType.None)
	{
		if (gameScreen != null)
		{
			gameScreen.ShowWeapons();
		}
		if (wasAppleDestroyed)
		{
			wasAppleDestroyed = false;
		}
		else if (!base.CachedManagers.CachedTutorialManager.IsAppleGameTutorialInProgress)
		{
			AppleMissesCount++;
		}
		else
		{
			AppleMissesCount = 0;
		}
		WeaponType weaponType = ((type != WeaponType.None) ? type : CurrentWeaponType());
		if (weaponType == WeaponType.None)
		{
			if (base.CachedManagers.CachedProposalManager.IsAvalaibleMoreAmmo)
			{
				GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Ammo);
				gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(Popup_OnPopupHide));
				base.CachedManagers.CachedGameManager.IsGamePaused = true;
			}
			else
			{
				isLevelFinished = true;
				arrowDragger.IsDraggingEnabled = false;
				cameraController.SetScaleWithDuration(1f, 3f);
				Vector2 targetPos = (Vector2)base.ActiveCharacter.transform.position + new Vector2(0f, 200f);
				cameraController.TweenMoveToPosition(targetPos, 3f);
				characterFirst.PlayIdleAnimation();
				base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
				{
					if (this != null)
					{
						_003CCreateWeapon_003E__BaseCallProxy0(WeaponType.None);
					}
				}, 4f);
			}
		}
		else
		{
			if (gameScreen != null)
			{
				gameScreen.SetFirstWeapon(weaponType);
			}
			base.CreateWeapon(weaponType);
		}
		return weaponType;
	}

	protected override WeaponType CurrentWeaponType()
	{
		WeaponType weaponType = WeaponType.None;
		if (base.CachedManagers.CachedTutorialManager.IsAppleGameTutorialInProgress)
		{
			return base.CachedManagers.CachedTutorialManager.TutorialWeaponApple;
		}
		return (AppleMissesCount < 3) ? AvailableWeapons.RandomObject() : WeaponType.None;
	}

	private void CreateApple()
	{
		if (targetCharacter != null && (!currentApple || !currentApple.OnHead))
		{
			targetCharacter.BodyTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
			SkeletonUtilityBone componentInParent = targetCharacter.HeadCollider.GetComponentInParent<SkeletonUtilityBone>();
			if ((bool)componentInParent)
			{
				componentInParent.gameObject.transform.localRotation = Quaternion.identity;
			}
			LevelObjectType type = ((!isForcedAppleTypesUse) ? appleTypes.RandomObject() : forcedAppleTypes.RandomObject());
			Apple apple = objectsSpawner.CreateObject(type) as Apple;
			apple.Init(this);
			apple.transform.parent = applesParent.transform;
			Vector3 position = targetCharacter.HeadColliderBody.transform.position;
			apple.transform.position = new Vector3(position.x, position.y + targetCharacter.AppleHeightOffset, position.z);
			apple.AttachedToBody(targetCharacter.HeadColliderBody);
			apple.OnHead = true;
			if (currentApple != null)
			{
				UnityEngine.Object.Destroy(currentApple.gameObject);
			}
			currentApple = apple;
		}
	}

	protected CharacterBase CreateAppleBoy()
	{
		CharacterBase characterBase = objectsSpawner.RandomAppleBoyCharacter();
		characterBase.IsAppleBoy = true;
		characterBase.AnimationChache.IdleAnimationName = "idle_apple";
		characterBase.transform.position = RightArcherPosition();
		characterBase.transform.parent = charactersTransfrom;
		characterBase.DrivenIntoWeaponsCacheSize = drivenIntoWeaponsCacheSize;
		characterBase.transform.localPosition = new Vector3(characterBase.transform.localPosition.x, characterBase.transform.localPosition.y, 0f);
		characterBase.IsFlipped = true;
		characterBase.Init(this);
		characterBase.PlayIdleAnimation();
		return characterBase;
	}

	protected override Vector3 RightArcherPosition()
	{
		return location.GetCharacterPositionForRange(base.CharacterFirst.transform.position.x + minEnemySpotDistance, base.CharacterFirst.transform.position.x + maxEnemySpotDistance);
	}

	protected override void UpdateCameraPosition()
	{
		Vector2 targetPos = NewCameraPosition();
		float targetScale = NewCameraScale();
		cameraController.SetScaleWithDuration(targetScale, 1f);
		cameraController.TweenMoveToPosition(targetPos, 1f, UpdateCameraFinished);
	}

	protected override void EndLevel(Character loseCharacter)
	{
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		if (cachedStatisticsManager.Score > cachedStatisticsManager.HighScoreFruitsSmashed)
		{
			cachedStatisticsManager.HighScoreFruitsSmashed = cachedStatisticsManager.Score;
		}
		base.EndLevel(loseCharacter);
	}

	public override void IncreaseScore(int value = 1)
	{
		base.IncreaseScore(value);
		wasAppleDestroyed = true;
		MovePlayer();
		base.CachedManagers.CachedStatisticsManager.FruitsSmashed += value;
		if (base.CachedManagers.CachedStatisticsManager.Score == scoreForSkillShotAchievement)
		{
			base.CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.SkillShot);
		}
		base.CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.Fruitslayer, base.CachedManagers.CachedStatisticsManager.FruitsSmashed);
	}

	private float NewCameraScale()
	{
		float num = ScreenDimentions.Width;
		float num2 = Mathf.Max(CurrentDistance() * multiplier / num, 1f);
		if (base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType == GUILayouterRotationType.Portrait)
		{
			num2 *= 1.2f;
		}
		return num2;
	}

	private float CurrentDistance()
	{
		return Mathf.Abs(targetCharacter.BoyPosition.x - base.CharacterFirst.transform.position.x) + 500f;
	}

	private Vector2 NewCameraPosition()
	{
		float num = -250f + CurrentDistance() * 0.5f;
		Vector2 vector = DefaultCameraPositionForCharacter(characterFirst);
		float num2 = NewCameraScale();
		float num3 = ScreenDimentions.Height;
		float num4 = num3 * (num2 - 1f) / 4f;
		return new Vector2(vector.x + num, vector.y + num4);
	}

	private void MovePlayer()
	{
		float x = activeCharacter.transform.position.x;
		float x2 = activeCharacter.transform.position.x;
		float x3 = targetCharacter.transform.position.x;
		if (x3 - x2 > characterDistanceMinBorder)
		{
			float num = UnityEngine.Random.Range(characterWithinBorderMinStep, characterWithinBorderMaxStep);
			num *= (float)(UnityEngine.Random.Range(0, 2) * 2 - 1);
			x += num;
			if (x > x3 - characterDistanceMinBorder || x < x3 - characterDistanceMaxBorder)
			{
				num *= -1f;
				x += 2f * num;
			}
		}
		else
		{
			x -= characterStepValue;
		}
		activeCharacter.transform.position = location.GetCharacterPositionForRange(x, x);
	}

	private void Popup_OnPopupHide(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(Popup_OnPopupHide));
		wasAppleDestroyed = true;
		CreateWeapon();
		if (!isLevelFinished)
		{
			base.ActiveCharacter.BeginCharge();
		}
		base.CachedManagers.CachedGameManager.IsGamePaused = false;
	}

	protected override void Tutorial_OnTutorialWillAppear()
	{
		base.Tutorial_OnTutorialWillAppear();
		if (currentWeapon.TutorialType != TutorialType.None && currentWeapon.TutorialType != TutorialType.Aiming)
		{
			CharacterNameType characterNameTypeForWeapon = GetCharacterNameTypeForWeapon(currentWeapon.WeaponType);
			charactersWithLoadedSpriteCollections.Remove(characterNameTypeForWeapon);
			charactersWithLoadedSpriteCollections.Add(characterNameTypeForWeapon);
			while (charactersWithLoadedSpriteCollections.Count > 5)
			{
				base.CachedManagers.CachedResourceManager.UnloadCharacterSpriteCollections(charactersWithLoadedSpriteCollections[0], 0f);
				charactersWithLoadedSpriteCollections.RemoveAt(0);
			}
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private WeaponType _003CCreateWeapon_003E__BaseCallProxy0(WeaponType type)
	{
		return base.CreateWeapon(type);
	}
}
