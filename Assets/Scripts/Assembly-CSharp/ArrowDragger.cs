using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ArrowDragger : MonoBehaviour
{
	private const float depthGizmos = -400f;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnTwitchingApply;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnTwitchingDisable;

	[SerializeField]
	private TrajectoryDrawer trajectoryDrawerPrefab;

	[SerializeField]
	private TrajectoryDrawer trajectoryDrawerApplePrefab;

	[SerializeField]
	private tk2dBaseSprite gizmoSprite;

	[SerializeField]
	private float gizmoVerticalOffset = 250f;

	[SerializeField]
	private float maxCameraScale = 1.2f;

	[SerializeField]
	private float maxCameraScaleAppleLevel = 1.2f;

	[SerializeField]
	private Color greenColor;

	private float minStretch = 50f;

	private float maxStretch = 350f;

	private const float inactivityLimit = 6f;

	private Vector2 beginPosition;

	private int reservedTouchId = -1;

	private bool isStretchedEnough;

	private bool isDraggingEnabled;

	private ShootingInfo draggingShootingInfo = default(ShootingInfo);

	private float initialCameraScale;

	private CameraController cachedCameraController;

	private LevelsManager cachedLevelsManager;

	private Managers cachedManagers;

	private DraggerGizmos cachedGizmos;

	private Rigidbody cachedHeadRigidbody;

	private TrajectoryDrawer trajectoryDrawer;

	private TrajectoryDrawer tutorialTrajectoryDrawer;

	private float inactivityTime;

	private float tutorialTrajectoryDrawerAlpha = 0.6f;

	private bool isGizmosEnabled;

	private ShootingInfo playerShootingInfo;

	private bool isTwitchingAvailable;

	public bool IsDraggingEnabled
	{
		get
		{
			return isDraggingEnabled;
		}
		set
		{
			if (isDraggingEnabled != value)
			{
				isDraggingEnabled = value;
				if (!isDraggingEnabled)
				{
					TrajectoryDrawer.SetTrajectoryActive(false);
				}
			}
		}
	}

	public bool IsPlayerDragging { get; set; }

	private DraggerGizmos CachedGizmos
	{
		get
		{
			if (cachedGizmos == null)
			{
				GameScreen gameScreen = CachedManagers.CachedGUIScreenManager.GameScreen;
				if (gameScreen != null)
				{
					cachedGizmos = gameScreen.DraggerGizmos;
				}
			}
			return cachedGizmos;
		}
	}

	private CameraController CachedCameraController
	{
		get
		{
			if (cachedCameraController == null)
			{
				cachedCameraController = CachedManagers.CachedGameManager.CameraController;
			}
			return cachedCameraController;
		}
	}

	private LevelsManager CachedLevelsManager
	{
		get
		{
			if (cachedLevelsManager == null)
			{
				cachedLevelsManager = CachedManagers.CachedLevelsManager;
			}
			return cachedLevelsManager;
		}
	}

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	private TrajectoryDrawer TrajectoryDrawer
	{
		get
		{
			if (trajectoryDrawer == null)
			{
				TrajectoryDrawer original = ((CachedLevelsManager.CurrentLevel.GameType != GameType.AppleShoot) ? trajectoryDrawerPrefab : trajectoryDrawerApplePrefab);
				trajectoryDrawer = UnityEngine.Object.Instantiate(original);
				trajectoryDrawer.transform.parent = base.transform;
				trajectoryDrawer.Init();
			}
			return trajectoryDrawer;
		}
	}

	private TrajectoryDrawer TutorialTrajectoryDrawer
	{
		get
		{
			if (tutorialTrajectoryDrawer == null)
			{
				TrajectoryDrawer original = ((CachedLevelsManager.CurrentLevel.GameType != GameType.AppleShoot) ? trajectoryDrawerPrefab : trajectoryDrawerApplePrefab);
				tutorialTrajectoryDrawer = UnityEngine.Object.Instantiate(original);
				tutorialTrajectoryDrawer.transform.parent = base.transform;
				tutorialTrajectoryDrawer.IsTutorialDrawer = true;
				tutorialTrajectoryDrawer.Init();
				Color color = new Color(1f, 1f, 1f, tutorialTrajectoryDrawerAlpha);
				tutorialTrajectoryDrawer.PaintDots(color);
			}
			return tutorialTrajectoryDrawer;
		}
	}

	private bool IsGizmosEnabled
	{
		get
		{
			return isGizmosEnabled;
		}
		set
		{
			if (isGizmosEnabled == value)
			{
				return;
			}
			isGizmosEnabled = value;
			Level currentLevel = CachedLevelsManager.CurrentLevel;
			if (!(currentLevel != null))
			{
				return;
			}
			bool flag = currentLevel.GameType == GameType.BirdsHunt;
			if (CachedGizmos != null)
			{
				CachedGizmos.gameObject.SetActive(IsGizmosEnabled && !flag);
			}
			if (!isGizmosEnabled)
			{
				if (TrajectoryDrawer != null)
				{
					TrajectoryDrawer.SetTrajectoryActive(false);
				}
				if (TutorialTrajectoryDrawer != null)
				{
					TutorialTrajectoryDrawer.SetTrajectoryActive(false);
				}
			}
		}
	}

	private bool IsTutorialTrajectoryDrawerEnabled
	{
		get
		{
			GameType gameType = CachedLevelsManager.CurrentLevel.GameType;
			if ((gameType == GameType.VSComputer && CachedManagers.CachedStatisticsManager.MathesCount == 0) || (gameType == GameType.AppleShoot && CachedManagers.CachedStatisticsManager.FruitsSmashed == 0) || !SingletonMonoBehaviour<TutorialManager>.Instance.IsVSCompTutorialFinished)
			{
				return true;
			}
			return false;
		}
	}

	private bool IsTwitchingAvailable
	{
		get
		{
			return isTwitchingAvailable;
		}
		set
		{
			if (isTwitchingAvailable != value)
			{
				isTwitchingAvailable = value;
			}
		}
	}

	public event Action OnTwitchingApply
	{
		add
		{
			Action action = this.m_OnTwitchingApply;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnTwitchingApply, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnTwitchingApply;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnTwitchingApply, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnTwitchingDisable
	{
		add
		{
			Action action = this.m_OnTwitchingDisable;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnTwitchingDisable, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnTwitchingDisable;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnTwitchingDisable, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		gizmoVerticalOffset = 350f;
	}

	private void OnEnable()
	{
		TouchManager.TouchBeganEvent += OnTouchBegan;
		TouchManager.TouchMoveEvent += OnTouchMoved;
		TouchManager.TouchEndedEvent += OnTouchEnded;
	}

	private void OnDisable()
	{
		TouchManager.TouchBeganEvent -= OnTouchBegan;
		TouchManager.TouchMoveEvent -= OnTouchMoved;
		TouchManager.TouchEndedEvent -= OnTouchEnded;
	}

	private Vector2 ConvertedTouchPosition(Vector2 position)
	{
		return CachedCameraController.CachedCamera.ScreenCamera.ScreenToWorldPoint(position);
	}

	private void OnTouchBegan(TouchInfo touch)
	{
		Level currentLevel = CachedLevelsManager.CurrentLevel;
		TrajectoryDrawer.PaintDots(Color.white);
		CachedGizmos.SetColor(new Color(0.4745098f, 0.23529412f, 44f / 85f, 1f));
		if (currentLevel.ActiveCharacter.IsAI || currentLevel.ActiveCharacter.IsMultiplayerEnemy || currentLevel.IsLevelFinished)
		{
			return;
		}
		GameScreen gameScreen = CachedManagers.CachedGUIScreenManager.GameScreen;
		if (gameScreen != null && gameScreen.IsTouchOnGameField(touch))
		{
			WeaponGeneral currentWeapon = currentLevel.CurrentWeapon;
			if ((bool)currentWeapon)
			{
				if (isDraggingEnabled)
				{
					inactivityTime = 0f;
					reservedTouchId = touch.id;
					isStretchedEnough = false;
					beginPosition = touch.position;
					Character activeCharacter = currentLevel.ActiveCharacter;
					IsPlayerDragging = true;
					Vector3 position = activeCharacter.BodyTransform.TransformPoint(gizmoVerticalOffset, 0f, 0f);
					position = CachedCameraController.CachedCamera.ScreenCamera.WorldToScreenPoint(position);
					CachedGizmos.SetPosition(new Vector3(position.x, position.y, -401f));
					CachedGizmos.SetParametrs(0, 0);
					initialCameraScale = CachedCameraController.Scale;
					if ((bool)activeCharacter && !currentWeapon.IsActive)
					{
						activeCharacter.UserAimingActionDetected();
						activeCharacter.UpdateCharacter(0f);
					}
					if (IsTutorialTrajectoryDrawerEnabled)
					{
						playerShootingInfo = currentLevel.ActiveCharacter.GetShootingInfo(false, true);
					}
				}
				else if (currentWeapon.IsActive)
				{
					currentWeapon.Activate(WeaponActivationType.Tap, null);
				}
			}
		}
		currentLevel.TouchOnMisteryBox(touch);
	}

	private void OnTouchMoved(TouchInfo touch)
	{
		if (reservedTouchId == touch.id)
		{
			OnDragging(touch);
		}
	}

	private void OnTouchEnded(TouchInfo touch)
	{
		if (isDraggingEnabled)
		{
			if (reservedTouchId == touch.id)
			{
				reservedTouchId = -1;
				if (isStretchedEnough)
				{
					ReleaseArrow();
				}
			}
			IsGizmosEnabled = false;
			IsPlayerDragging = false;
		}
		if (!CachedManagers.CachedGameManager.IsGamePaused)
		{
			Level currentLevel = CachedLevelsManager.CurrentLevel;
			if (currentLevel is LevelBirdsHunt)
			{
				(currentLevel as LevelBirdsHunt).SkipCameraZooming();
			}
		}
	}

	public void CancelTouch()
	{
		reservedTouchId = -1;
		isStretchedEnough = false;
		IsGizmosEnabled = false;
	}

	public void CustomUpdate(float deltaTime)
	{
		Character activeCharacter = CachedLevelsManager.CurrentLevel.ActiveCharacter;
		if (activeCharacter != null && !activeCharacter.IsAI && !activeCharacter.IsMultiplayerEnemy && isDraggingEnabled && reservedTouchId == -1)
		{
			inactivityTime += deltaTime;
			if (inactivityTime >= 6f)
			{
				inactivityTime = 0f;
				activeCharacter.StartInactivityAnimation();
			}
		}
		TrajectoryDrawer.CustomUpdate(deltaTime);
		if (IsTutorialTrajectoryDrawerEnabled)
		{
			TutorialTrajectoryDrawer.CustomUpdate(deltaTime);
		}
	}

	private void OnDragging(TouchInfo touch)
	{
		if (!CachedManagers.CachedGameManager.IsGameRunning)
		{
			return;
		}
		Level currentLevel = CachedLevelsManager.CurrentLevel;
		if (!currentLevel)
		{
			return;
		}
		WeaponGeneral currentWeapon = currentLevel.CurrentWeapon;
		if (!currentWeapon || !isDraggingEnabled)
		{
			return;
		}
		Vector2 vector = touch.position - beginPosition;
		float num = 90f + Mathf.Atan2(vector.x, vector.y) * 57.29578f;
		float num2 = Mathf.Min(vector.magnitude, maxStretch);
		isStretchedEnough = num2 >= minStretch;
		float num3 = num2 / maxStretch;
		float num4 = num3 * currentWeapon.MaxSpeed;
		draggingShootingInfo.velocity = num4;
		draggingShootingInfo.angle = 0f - num;
		Character activeCharacter = currentLevel.ActiveCharacter;
		if (isStretchedEnough)
		{
			float num5 = 1f;
			if (currentLevel.GameType != GameType.BirdsHunt)
			{
				float num6 = ((currentLevel.GameType != GameType.AppleShoot) ? maxCameraScale : maxCameraScaleAppleLevel);
				float num7 = ((num2 != minStretch) ? ((num2 - minStretch) / (maxStretch - minStretch)) : num3);
				num5 = initialCameraScale * (1f + (num6 - 1f) * num7);
				CachedCameraController.SetScaleImmediately(num5);
			}
			IsGizmosEnabled = true;
			float emissionAngle = currentWeapon.emissionAngle;
			TrajectoryDrawer.UpdateTrajectory(activeCharacter.WeaponReleasePosition(), emissionAngle * ((float)Math.PI / 180f), num4, num3);
			if (IsTutorialTrajectoryDrawerEnabled)
			{
				float ratio = playerShootingInfo.velocity / currentWeapon.MaxSpeed;
				TutorialTrajectoryDrawer.UpdateTrajectory(activeCharacter.WeaponReleasePosition(), playerShootingInfo.angle * ((float)Math.PI / 180f), playerShootingInfo.velocity, ratio);
			}
			float num8 = Mathf.Atan2(vector.y, vector.x) * 57.29578f + 180f;
			CachedGizmos.SetPosition(GizmoPosition(num5, num8));
			CachedGizmos.SetParametrs((int)(Mathf.Clamp(num3, 0f, 1f) * 100f), (int)num8);
			if (currentLevel.CurrentWeapon.LaunchType == WeaponLaunchType.Bow)
			{
				IsTwitchingAvailable = true;
			}
		}
		else
		{
			IsGizmosEnabled = false;
			if (currentLevel.CurrentWeapon.LaunchType == WeaponLaunchType.Bow)
			{
				IsTwitchingAvailable = false;
			}
		}
		if (IsTutorialTrajectoryDrawerEnabled)
		{
			if (playerShootingInfo.angle + 1f > draggingShootingInfo.angle && playerShootingInfo.angle - 1f < draggingShootingInfo.angle && playerShootingInfo.velocity + 50f > draggingShootingInfo.velocity && playerShootingInfo.velocity - 50f < draggingShootingInfo.velocity)
			{
				TrajectoryDrawer.PaintDots(greenColor);
				TutorialTrajectoryDrawer.gameObject.SetActive(false);
				CachedGizmos.SetColor(greenColor);
			}
			else
			{
				TrajectoryDrawer.PaintDots(Color.white);
				TutorialTrajectoryDrawer.gameObject.SetActive(true);
				CachedGizmos.SetColor(new Color(0.4745098f, 0.23529412f, 44f / 85f, 1f));
			}
		}
		activeCharacter.SetCurrentAngleAndVelocity(draggingShootingInfo, num3);
	}

	private void ReleaseArrow()
	{
		isStretchedEnough = false;
		isDraggingEnabled = false;
		CachedLevelsManager.CurrentLevel.ReleaseWeapon();
	}

	private Vector3 GizmoPosition(float cameraScale, float angle)
	{
		Vector3 size = gizmoSprite.GetBounds().size;
		angle = ((!(angle > 90f) || !(angle < 270f)) ? (angle + 90f) : (angle - 90f));
		angle *= (float)Math.PI / 180f;
		float num = size.x * Mathf.Cos(angle);
		float num2 = size.y * Mathf.Sin(angle);
		Vector3 position = new Vector3((gizmoVerticalOffset - Mathf.Sqrt(num * num + num2 * num2)) / cameraScale, 0f, 0f);
		position = CachedLevelsManager.CurrentLevel.ActiveCharacter.BodyTransform.TransformPoint(position);
		position = CachedCameraController.CachedCamera.ScreenCamera.WorldToScreenPoint(position);
		position.x += num;
		position.y += num2;
		position.z = -401f;
		return position;
	}
}
