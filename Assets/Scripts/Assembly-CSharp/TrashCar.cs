using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class TrashCar : Vehicle
{
	private const string TRASH_CAR_ANIMATION_LOOP = "fatality_loop";

	private const string TRASH_CAR_ANIMATION_PUNCH = "fatality_punch";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnFrontCollide;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnBackCollide;

	[SerializeField]
	private SkeletonAnimation trashCarAnimation;

	[SerializeField]
	private Collider frontCollider;

	[SerializeField]
	private Collider backCollider;

	[SerializeField]
	private Transform seatPlace;

	[SerializeField]
	private Transform effectCrashTransform;

	[SerializeField]
	[ResourceLink]
	public AssetLink crashEffectPrefab;

	[SerializeField]
	[ResourceLink]
	public AssetLink bloodEffectPrefab;

	private FatalityManager cachedFatalityManager;

	private bool isFirstCollided;

	private Spine.Animation cachedFatalityRideAnimation;

	private Spine.Animation cachedFatalityStrikeAnimation;

	private GameObject bloodEffect;

	private GameObject crashEffect;

	public Vector3 SeatPlace
	{
		get
		{
			if (seatPlace == null)
			{
				return Vector3.zero;
			}
			return seatPlace.localPosition;
		}
	}

	public FatalityManager CachedManager
	{
		get
		{
			return cachedFatalityManager;
		}
	}

	private Spine.Animation CachedFatalityRideAnimation
	{
		get
		{
			if (cachedFatalityRideAnimation == null)
			{
				cachedFatalityRideAnimation = trashCarAnimation.Skeleton.Data.FindAnimation("fatality_loop");
			}
			return cachedFatalityRideAnimation;
		}
	}

	private Spine.Animation CachedFatalityStrikeAnimation
	{
		get
		{
			if (cachedFatalityStrikeAnimation == null)
			{
				cachedFatalityStrikeAnimation = trashCarAnimation.Skeleton.Data.FindAnimation("fatality_punch");
			}
			return cachedFatalityStrikeAnimation;
		}
	}

	public float AnimationRideDuration
	{
		get
		{
			return CachedFatalityRideAnimation.Duration;
		}
	}

	public bool IsRight { get; set; }

	public static event Action OnFrontCollide
	{
		add
		{
			Action action = TrashCar.m_OnFrontCollide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TrashCar.m_OnFrontCollide, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = TrashCar.m_OnFrontCollide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TrashCar.m_OnFrontCollide, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action OnBackCollide
	{
		add
		{
			Action action = TrashCar.m_OnBackCollide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TrashCar.m_OnBackCollide, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = TrashCar.m_OnBackCollide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref TrashCar.m_OnBackCollide, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		trashCarAnimation.State.SetAnimation(0, CachedFatalityRideAnimation, false);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (!CachedManager.WithCharacterCollision(collider))
		{
			return;
		}
		if (!isFirstCollided)
		{
			isFirstCollided = true;
			frontCollider.enabled = false;
			backCollider.enabled = true;
			TrashCar.m_OnFrontCollide?.Invoke();
			trashCarAnimation.State.SetAnimation(0, CachedFatalityStrikeAnimation, false);
			if (bloodEffect == null && bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
			{
				bloodEffect = (GameObject)bloodEffectPrefab.GetInstance();
				bloodEffect.transform.parent = collider.transform;
				bloodEffect.transform.localPosition = Vector3.zero;
				bloodEffect.transform.localRotation = Quaternion.identity;
				if (IsRight)
				{
					bloodEffect.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				bloodEffect.PlayFX();
			}
		}
		else
		{
			frontCollider.enabled = false;
			backCollider.enabled = false;
			TrashCar.m_OnBackCollide?.Invoke();
			if (crashEffect == null && crashEffectPrefab != null && !string.IsNullOrEmpty(crashEffectPrefab.assetGUID))
			{
				crashEffect = (GameObject)crashEffectPrefab.GetInstance();
				crashEffect.transform.parent = effectCrashTransform;
				crashEffect.transform.localPosition = Vector3.zero;
				crashEffect.transform.localRotation = Quaternion.identity;
				crashEffect.PlayFX();
			}
		}
	}

	public void SetBikePosition(float frontWheelXPosition, float deltaTime)
	{
		SetVehiclePosition(frontWheelXPosition);
	}

	public void SetFatalityManager(FatalityManager fatalityManager)
	{
		if (cachedFatalityManager != fatalityManager)
		{
			cachedFatalityManager = fatalityManager;
		}
	}
}
