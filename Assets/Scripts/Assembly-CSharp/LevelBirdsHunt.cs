using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBirdsHunt : Level
{
	[SerializeField]
	private GameObject birdsParent;

	[SerializeField]
	private GameObject powerUpParent;

	[SerializeField]
	private float timeNextWaveMin;

	[SerializeField]
	private float timeNextWaveMax;

	[SerializeField]
	private WaveParametrs[] waves = new WaveParametrs[1] { WaveParametrs.WaveDefault };

	[SerializeField]
	private WeaponType[] availableWeapons;

	[SerializeField]
	private int birdsForTrickShot = 15;

	private LevelTimer cachedTimer;

	private List<FlyingObject> flyObjects = new List<FlyingObject>();

	private List<FlyingObject> removedFlyObjects = new List<FlyingObject>();

	private int currentWave;

	private int wavesCount;

	private bool isCameraZooming;

	private bool isTimeCounting;

	public LevelTimer CachedTimer
	{
		get
		{
			if (cachedTimer == null)
			{
				cachedTimer = GetComponent<LevelTimer>();
			}
			return cachedTimer;
		}
	}

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

	public int WavesCount
	{
		get
		{
			return wavesCount;
		}
	}

	public override void Init(ObjectsSpawner objectsSpawner, CameraController cameraController)
	{
		base.Init(objectsSpawner, cameraController);
		base.ActiveCharacter.IsInvincible = true;
		base.ActiveCharacter.SetCollidersEnabled(false, true);
		isCameraZooming = true;
		characterFirst.StartGreetingAnimation();
		ZoomToCharacter(true);
	}

	public override void UpdateLevel(float deltaTime)
	{
		base.UpdateLevel(deltaTime);
		foreach (FlyingObject flyObject in flyObjects)
		{
			flyObject.UpdateObject(deltaTime);
		}
		if (removedFlyObjects.Count > 0)
		{
			foreach (FlyingObject removedFlyObject in removedFlyObjects)
			{
				flyObjects.Remove(removedFlyObject);
			}
			removedFlyObjects.Clear();
		}
		if (isTimeCounting && !base.CachedManagers.CachedTutorialManager.IsBirdsGameTutorialInProgress)
		{
			CachedTimer.UpdateTimer(deltaTime, this);
		}
	}

	public void RemoveFlyObject(FlyingObject flyObject)
	{
		removedFlyObjects.Add(flyObject);
	}

	protected override WeaponType CreateWeapon(WeaponType type = WeaponType.None)
	{
		if (gameScreen != null)
		{
			gameScreen.ShowWeapons();
		}
		WeaponType weaponType = base.CreateWeapon(type);
		base.ActiveCharacter.SetCollidersEnabled(false);
		if (weaponType != WeaponType.None && base.ActiveCharacter != null && gameScreen != null)
		{
			gameScreen.SetFirstWeapon(weaponType);
		}
		return weaponType;
	}

	public override void WeaponShoot(WeaponGeneral weapon, Character characterShoot)
	{
		base.WeaponShoot(weapon, characterShoot);
		TurnFinished(1f);
	}

	public override void Explode(ExplosionInfo explosionInfo)
	{
		base.Explode(explosionInfo);
		foreach (FlyingObject flyObject in flyObjects)
		{
			if (flyObject.IsAffectedInRadius(explosionInfo.position, explosionInfo.radius))
			{
				flyObject.ApplyExlosionForce(explosionInfo);
			}
		}
	}

	public override void ApplyPeriodicalDamage(PeriodicalDamageInfo damageInfo)
	{
		foreach (FlyingObject flyObject in flyObjects)
		{
			if (flyObject.IsAffectedInRadius(damageInfo.position, damageInfo.radius))
			{
				FlyingObjectDamageType type = FlyingObjectDamageType.None;
				if (damageInfo.type == PeriodicalDamageType.Fire)
				{
					type = FlyingObjectDamageType.Fire;
				}
				else if (damageInfo.type == PeriodicalDamageType.Electricity)
				{
					type = FlyingObjectDamageType.Electricity;
				}
				flyObject.DamageApplied(type, Vector2.zero);
			}
		}
	}

	public void TimeIsOver()
	{
		if (base.CachedManagers.CachedProposalManager.IsAvalaibleMoreTime)
		{
			GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Time);
			gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(Popup_OnPopupHide));
			base.CachedManagers.CachedGameManager.IsGamePaused = true;
			return;
		}
		isLevelFinished = true;
		arrowDragger.IsDraggingEnabled = false;
		activeCharacter.PlayIdleAnimation();
		ZoomToCharacter(false);
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				EndLevel(base.ActiveCharacter);
			}
		}, 2f);
	}

	public void SkipCameraZooming()
	{
		CameraZoomActionOnStartEnded();
	}

	protected override void EndLevel(Character loseCharacter)
	{
		base.EndLevel(loseCharacter);
		StatisticsManager cachedStatisticsManager = base.CachedManagers.CachedStatisticsManager;
		if (cachedStatisticsManager.Score > cachedStatisticsManager.HighScoreBirdsKilled)
		{
			cachedStatisticsManager.HighScoreBirdsKilled = cachedStatisticsManager.Score;
		}
	}

	public override void IncreaseScore(int value = 1)
	{
		base.IncreaseScore(value);
		base.CachedManagers.CachedStatisticsManager.BirdsKilled += value;
		if (base.CachedManagers.CachedStatisticsManager.Score >= birdsForTrickShot)
		{
			base.CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.TrickShot);
		}
		base.CachedManagers.CachedMicroachievementsManager.CheckLevelAchievementReaching(MicroachievementType.GreatHunt, base.CachedManagers.CachedStatisticsManager.BirdsKilled);
	}

	public override void EnableDragging()
	{
		if (!isLevelFinished)
		{
			arrowDragger.IsDraggingEnabled = true;
		}
	}

	protected override WeaponType CurrentWeaponType()
	{
		WeaponType result = AvailableWeapons.RandomObject();
		TutorialManager cachedTutorialManager = base.CachedManagers.CachedTutorialManager;
		if (cachedTutorialManager.IsBirdsGameTutorialInProgress && base.GameType == GameType.BirdsHunt)
		{
			result = cachedTutorialManager.TutorialWeaponBirds;
		}
		return result;
	}

	protected override Vector3 LeftArcherPosition()
	{
		return location.GetCharacterPositionForRange(-3000f, 3000f);
	}

	private void StartGame()
	{
		NavigateToCharacter(characterFirst, 1f);
		cameraController.SetScaleWithDuration(cameraSettings.size, 1f);
		StartSpawning();
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				isTimeCounting = true;
			}
		}, 2f);
	}

	private T CreateFlyingObject<T>(LevelObjectType type, GameObject parentObject) where T : FlyingObject
	{
		T result = objectsSpawner.CreateObject(type) as T;
		result.Init(this);
		result.CachedTransform.parent = parentObject.transform;
		result.CachedTransform.localPosition = (Vector2)result.CachedTransform.localPosition;
		return result;
	}

	private IEnumerator SpawnBird()
	{
		while (true)
		{
			int birdsCount = UnityEngine.Random.Range(waves[currentWave].birdsCountMin, waves[currentWave].birdsCountMax);
			for (int i = 0; i < birdsCount; i++)
			{
				Bird newBird = objectsSpawner.CreateBird();
				newBird.Init(this);
				newBird.CachedTransform.parent = birdsParent.transform;
				newBird.CachedTransform.localPosition = (Vector2)newBird.CachedTransform.localPosition;
				flyObjects.Add(newBird);
				yield return new WaitForSeconds(UnityEngine.Random.Range(waves[currentWave].birdsDelayMin, waves[currentWave].birdsDelayMax));
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(timeNextWaveMin, timeNextWaveMin));
			currentWave = Mathf.Min(currentWave + 1, waves.Length - 1);
			wavesCount++;
		}
	}

	private IEnumerator SpawnPowerUp()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(timeNextWaveMin, timeNextWaveMin));
		yield return new WaitForSeconds(UnityEngine.Random.Range(waves[currentWave].timeSpawnPowerUpMin, waves[currentWave].timeSpawnPowerUpMax));
		while (true)
		{
			PowerUp newPowerUp = CreateFlyingObject<PowerUp>(LevelObjectType.PowerUp, powerUpParent);
			newPowerUp.TypePowerUp = ((!(UnityEngine.Random.value > 0.5f)) ? PowerUpType.Time : PowerUpType.Gold);
			flyObjects.Add(newPowerUp);
			yield return new WaitForSeconds(UnityEngine.Random.Range(waves[currentWave].timeSpawnPowerUpMin, waves[currentWave].timeSpawnPowerUpMax));
		}
	}

	private void StartSpawning()
	{
		currentWave = 0;
		StartCoroutine(SpawnBird());
		StartCoroutine(SpawnPowerUp());
	}

	private void ZoomToCharacter(bool isOnStart)
	{
		cameraController.SetScaleWithDuration(cameraSettings.size * 0.5f, 3f);
		Vector2 targetPos = (Vector2)base.ActiveCharacter.transform.position + new Vector2(0f, 200f);
		if (isOnStart)
		{
			cameraController.TweenMoveToPosition(targetPos, 3f, CameraZoomActionOnStartEnded);
		}
		else
		{
			cameraController.TweenMoveToPosition(targetPos, 3f);
		}
	}

	private void CameraZoomActionOnStartEnded()
	{
		if (isCameraZooming)
		{
			float delay = 0f;
			if (!base.CachedManagers.CachedTutorialManager.IsBirdsGameTutorialInProgress)
			{
				base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Timer);
				delay = 2f;
			}
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, StartGame, delay);
			isCameraZooming = false;
		}
	}

	private void Popup_OnPopupHide(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(Popup_OnPopupHide));
		if (CachedTimer.Time <= 0f)
		{
			base.CachedManagers.CachedGameManager.IsGamePaused = false;
			TimeIsOver();
			return;
		}
		base.CachedManagers.CachedGUIPopupManager.ShowPopup(PopupType.Timer);
		base.CachedManagers.CachedGameManager.IsGamePaused = true;
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				base.CachedManagers.CachedGameManager.IsGamePaused = false;
			}
		}, 2f);
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
}
