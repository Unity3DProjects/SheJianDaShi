using System;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.Analytics;

public class GUIChest : MonoBehaviour
{
	public Action OnSpawnEnd;

	public Action OnOpenLock;

	[SerializeField]
	private SkeletonAnimation chestAnimation;

	[SpineSlot("", "chestAnimation", false)]
	[SerializeField]
	private string slotLockedLeft;

	[SpineSlot("", "chestAnimation", false)]
	[SerializeField]
	private string slotLockedCenter;

	[SpineSlot("", "chestAnimation", false)]
	[SerializeField]
	private string slotLockedRight;

	[SerializeField]
	private SkeletonUtilityBone boneLockedLeft;

	[SerializeField]
	private SkeletonUtilityBone boneLockedCenter;

	[SerializeField]
	private SkeletonUtilityBone boneLockedRight;

	[SerializeField]
	private SkeletonUtilityBone boneCenterChest;

	[SerializeField]
	private GUIControl controlMoveChest;

	[SerializeField]
	private GUIControl controlFade;

	[SerializeField]
	private GUIControl controlHideControls;

	[SerializeField]
	private Transform beginRootTransform;

	[SerializeField]
	private Transform endRootTransform;

	[SerializeField]
	private float timeDelayFade;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationSpawnUnlocked;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationSpawnOneLock;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationSpawnThreeLocks;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationOpen;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationUnlockLeft;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationUnlockCenter;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationUnlockOneLock;

	[SpineAnimation("", "chestAnimation")]
	[SerializeField]
	private string animationUnlockRight;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataSpawnEnd;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataUnlockEnd;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataUnlockLeft;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataUnlockRight;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataUnlockCenter;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataOpenStart;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataOpenEnd;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataJumpEnd;

	[SpineEventData("", "chestAnimation")]
	[SerializeField]
	private string eventDataJumpStart;

	private Slot cachedSlotLockedLeft;

	private Slot cachedSlotLockedCenter;

	private Slot cachedSlotLockedRight;

	private Spine.Animation cachedAnimationSpawn;

	private Spine.Animation cachedAnimationOpen;

	private Spine.Animation cachedAnimationUnlockLeft;

	private Spine.Animation cachedAnimationUnlockCenter;

	private Spine.Animation cachedAnimationUnlockRight;

	private EventData cachedEventDataSpawnEnd;

	private EventData cachedEventDataUnlockEnd;

	private EventData cachedEventDataUnlockLeft;

	private EventData cachedEventDataUnlockRight;

	private EventData cachedEventDataUnlockCenter;

	private EventData cachedEventDataOpenStart;

	private EventData cachedEventDataOpenEnd;

	private EventData cachedEventDataJumpEnd;

	private EventData cachedEventDataJumpStart;

	private Transform cachedTransform;

	private List<GameObject> currentSystems = new List<GameObject>();

	private ParticleSystem idleSystem;

	private Managers cachedManagers;

	private Slot CachedSlotLockedLeft
	{
		get
		{
			if (cachedSlotLockedLeft == null)
			{
				cachedSlotLockedLeft = chestAnimation.Skeleton.FindSlot(slotLockedLeft);
			}
			return cachedSlotLockedLeft;
		}
	}

	private Slot CachedSlotLockedCenter
	{
		get
		{
			if (cachedSlotLockedCenter == null)
			{
				cachedSlotLockedCenter = chestAnimation.Skeleton.FindSlot(slotLockedCenter);
			}
			return cachedSlotLockedCenter;
		}
	}

	private Slot CachedSlotLockedRight
	{
		get
		{
			if (cachedSlotLockedRight == null)
			{
				cachedSlotLockedRight = chestAnimation.Skeleton.FindSlot(slotLockedRight);
			}
			return cachedSlotLockedRight;
		}
	}

	private Spine.Animation CachedAnimationSpawn
	{
		get
		{
			if (cachedAnimationSpawn == null)
			{
				cachedAnimationSpawn = chestAnimation.Skeleton.Data.FindAnimation(animationSpawnThreeLocks);
			}
			Spine.Animation result = cachedAnimationSpawn;
			if (ChestType == GUIChestType.Unlocked)
			{
				result = chestAnimation.Skeleton.Data.FindAnimation(animationSpawnUnlocked);
			}
			else if (ChestType == GUIChestType.OneLock)
			{
				result = chestAnimation.Skeleton.Data.FindAnimation(animationSpawnOneLock);
			}
			return result;
		}
	}

	private Spine.Animation CachedAnimationOpen
	{
		get
		{
			if (cachedAnimationOpen == null)
			{
				cachedAnimationOpen = chestAnimation.Skeleton.Data.FindAnimation(animationOpen);
			}
			return cachedAnimationOpen;
		}
	}

	private Spine.Animation CachedAnimationUnlockLeft
	{
		get
		{
			if (cachedAnimationUnlockLeft == null)
			{
				cachedAnimationUnlockLeft = chestAnimation.Skeleton.Data.FindAnimation(animationUnlockLeft);
			}
			return cachedAnimationUnlockLeft;
		}
	}

	private Spine.Animation CachedAnimationUnlockCenter
	{
		get
		{
			if (ChestType == GUIChestType.OneLock)
			{
				return chestAnimation.Skeleton.Data.FindAnimation(animationUnlockOneLock);
			}
			if (cachedAnimationUnlockCenter == null)
			{
				cachedAnimationUnlockCenter = chestAnimation.Skeleton.Data.FindAnimation(animationUnlockCenter);
			}
			return cachedAnimationUnlockCenter;
		}
	}

	private Spine.Animation CachedAnimationUnlockRight
	{
		get
		{
			if (cachedAnimationUnlockRight == null)
			{
				cachedAnimationUnlockRight = chestAnimation.Skeleton.Data.FindAnimation(animationUnlockRight);
			}
			return cachedAnimationUnlockRight;
		}
	}

	private EventData CachedEventDataSpawnEnd
	{
		get
		{
			if (cachedEventDataSpawnEnd == null)
			{
				cachedEventDataSpawnEnd = chestAnimation.Skeleton.Data.FindEvent(eventDataSpawnEnd);
			}
			return cachedEventDataSpawnEnd;
		}
	}

	private EventData CachedEventDataOpenEnd
	{
		get
		{
			if (cachedEventDataOpenEnd == null)
			{
				cachedEventDataOpenEnd = chestAnimation.Skeleton.Data.FindEvent(eventDataOpenEnd);
			}
			return cachedEventDataOpenEnd;
		}
	}

	private EventData CachedEventDataOpenStart
	{
		get
		{
			if (cachedEventDataOpenStart == null)
			{
				cachedEventDataOpenStart = chestAnimation.Skeleton.Data.FindEvent(eventDataOpenStart);
			}
			return cachedEventDataOpenStart;
		}
	}

	private EventData CachedEventDataJumpStart
	{
		get
		{
			if (cachedEventDataJumpStart == null)
			{
				cachedEventDataJumpStart = chestAnimation.Skeleton.Data.FindEvent(eventDataJumpStart);
			}
			return cachedEventDataJumpStart;
		}
	}

	private EventData CachedEventDataJumpEnd
	{
		get
		{
			if (cachedEventDataJumpEnd == null)
			{
				cachedEventDataJumpEnd = chestAnimation.Skeleton.Data.FindEvent(eventDataJumpEnd);
			}
			return cachedEventDataJumpEnd;
		}
	}

	private EventData CachedEventDataUnlockEnd
	{
		get
		{
			if (cachedEventDataUnlockEnd == null)
			{
				cachedEventDataUnlockEnd = chestAnimation.Skeleton.Data.FindEvent(eventDataUnlockEnd);
			}
			return cachedEventDataUnlockEnd;
		}
	}

	private EventData CachedEventDataUnlockLeft
	{
		get
		{
			if (cachedEventDataUnlockLeft == null)
			{
				cachedEventDataUnlockLeft = chestAnimation.Skeleton.Data.FindEvent(eventDataUnlockLeft);
			}
			return cachedEventDataUnlockLeft;
		}
	}

	private EventData CachedEventDataUnlockRight
	{
		get
		{
			if (cachedEventDataUnlockRight == null)
			{
				cachedEventDataUnlockRight = chestAnimation.Skeleton.Data.FindEvent(eventDataUnlockRight);
			}
			return cachedEventDataUnlockRight;
		}
	}

	private EventData CachedEventDataUnlockCenter
	{
		get
		{
			if (cachedEventDataUnlockCenter == null)
			{
				cachedEventDataUnlockCenter = chestAnimation.Skeleton.Data.FindEvent(eventDataUnlockCenter);
			}
			return cachedEventDataUnlockCenter;
		}
	}

	private Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public bool IsChestOpening { get; private set; }

	public GUIChestType ChestType { get; set; }

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

	private void Awake()
	{
		chestAnimation.State.Event += ChestAnimation_State_Event;
	}

	private void OnEnable()
	{
		chestAnimation.ForceUpdate();
	}

	private void OnDisable()
	{
		chestAnimation.ForceUpdate();
		IsChestOpening = false;
	}

	private void OnDestroy()
	{
		chestAnimation.State.Event -= ChestAnimation_State_Event;
	}

	public void InitChest()
	{
		controlMoveChest.HideImmediately();
		controlFade.HideImmediately();
		controlHideControls.HideImmediately();
		CachedTransform.parent = beginRootTransform;
		CachedTransform.localPosition = Vector3.zero;
		chestAnimation.Skeleton.SetToSetupPose();
		chestAnimation.ForceUpdate();
		for (int i = 0; i < currentSystems.Count; i++)
		{
			UnityEngine.Object.Destroy(currentSystems[i]);
		}
		currentSystems.Clear();
		idleSystem = null;
		int keysCount = CachedManagers.CachedRouletteManager.KeysCount;
		CachedSlotLockedLeft.SetColor((keysCount >= 1) ? Color.clear : Color.white);
		CachedSlotLockedCenter.SetColor((keysCount >= 2) ? Color.clear : Color.white);
		CachedSlotLockedRight.SetColor(Color.white);
	}

	public void SpawnChest()
	{
		chestAnimation.State.SetAnimation(0, CachedAnimationSpawn, false);
		chestAnimation.Update(0f);
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIChestSpawn));
			gameObject.transform.parent = boneCenterChest.transform;
			gameObject.transform.localPosition = Vector3.zero;
			currentSystems.Add(gameObject);
		}, 0f);
	}

	public void OpenChest()
	{
		ProposalResultType currentProposalResult = CachedManagers.CachedProposalManager.CurrentProposalResult;
		if (currentProposalResult == ProposalResultType.Chest || ChestType == GUIChestType.OneLock)
		{
			if ((bool)idleSystem)
			{
				idleSystem.Stop(true);
			}
			chestAnimation.State.SetAnimation(0, CachedAnimationOpen, false);
			IsChestOpening = true;
			controlFade.Show(null, false, CachedAnimationOpen.Duration - timeDelayFade, true);
			StatisticsManager cachedStatisticsManager = CachedManagers.CachedStatisticsManager;
			Analytics.CustomEvent("Chest_Opened_Result_Screen", new Dictionary<string, object>
			{
				{ "ChestOpened", cachedStatisticsManager.OpenedChestsCount },
				{
					"GameTimeMinutes",
					cachedStatisticsManager.TotalGameTime.Minutes
				}
			});
		}
	}

	public void OpenLock()
	{
		SingletonMonoBehaviour<GUIScreenManager>.Instance.HideCurrentScreen(true, delegate
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.white;
			SingletonMonoBehaviour<GUIScreenManager>.Instance.ShowScreen(GUIScreenType.Roulette, delegate
			{
				SingletonMonoBehaviour<GameManager>.Instance.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.black;
			});
		}, false);
	}

	public void UpdateChest(float deltaTime)
	{
		chestAnimation.Update(deltaTime);
	}

	private void ChestAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataSpawnEnd)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIChestIdle));
				gameObject.transform.parent = boneCenterChest.transform;
				gameObject.transform.localPosition = Vector3.zero;
				currentSystems.Add(gameObject);
				idleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
				if (OnSpawnEnd != null)
				{
					OnSpawnEnd();
				}
			}, 0f);
		}
		else if (e.Data == CachedEventDataUnlockLeft)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIChestOpenLock));
				gameObject.transform.parent = boneLockedLeft.CachedTransform;
				gameObject.transform.localPosition = Vector3.zero;
				currentSystems.Add(gameObject);
			}, 0f);
		}
		else if (e.Data == CachedEventDataUnlockRight)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIChestOpenLock));
				gameObject.transform.parent = boneLockedRight.CachedTransform;
				gameObject.transform.localPosition = Vector3.zero;
				currentSystems.Add(gameObject);
			}, 0f);
			AudioManager.PlayAudioWithKey("chest_open");
		}
		else if (e.Data == CachedEventDataUnlockCenter)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIChestOpenLock));
				gameObject.transform.parent = boneLockedCenter.CachedTransform;
				gameObject.transform.localPosition = Vector3.zero;
				currentSystems.Add(gameObject);
			}, 0f);
			if (ChestType == GUIChestType.OneLock)
			{
				AudioManager.PlayAudioWithKey("chest_open");
			}
		}
		else if (e.Data == CachedEventDataUnlockEnd)
		{
			if (CachedManagers.CachedRouletteManager.KeysCount == SingletonMonoBehaviour<RouletteManager>.Instance.LocksCount || ChestType == GUIChestType.OneLock)
			{
				OpenChest();
			}
			else if (OnOpenLock != null)
			{
				OnOpenLock();
			}
		}
		else if (e.Data == CachedEventDataJumpStart)
		{
			CachedTransform.parent = endRootTransform;
			controlMoveChest.Show();
			controlHideControls.Show();
		}
		else if (e.Data == CachedEventDataJumpEnd)
		{
			AudioManager.PlayAudioWithKey("chest_jump_finish");
		}
		else if (e.Data == CachedEventDataOpenStart)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(CachedManagers.CachedEffectsManager.EffectPrefabByType(EffectType.GUIChestOpen));
				gameObject.transform.parent = boneCenterChest.CachedTransform;
				gameObject.transform.localPosition = Vector3.zero;
				currentSystems.Add(gameObject);
			}, 0f);
			CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
			{
				if (this != null && CachedManagers.CachedStatisticsManager.OpenedChestsCount == 1)
				{
					NotifyManager cachedNotifyManager = CachedManagers.CachedNotifyManager;
					cachedNotifyManager.TryToRegisterForLocalNotifications();
					DateTime value = DateTime.Now - (TimeSpan.FromDays(1.0) - TimeSpan.FromSeconds(CachedManagers.CachedProposalManager.SecondsBeforeFirstGift));
					CustomPlayerPrefs.SetDateTime("GiftDate", value);
					CustomPlayerPrefs.SetDateTime("last_super_gift_date", DateTime.Now - TimeSpan.FromDays(1.0));
					CustomPlayerPrefs.SetDateTime("last_roulette_played_day", DateTime.Now - TimeSpan.FromDays(1.0));
					CustomPlayerPrefs.SetDateTime("last_free_chest_accelerator", DateTime.Now - TimeSpan.FromDays(1.0));
					cachedNotifyManager.IsDailyGiftNotificationEnabled = true;
					cachedNotifyManager.UpdateNotifications(0f, string.Empty);
				}
			}, 0.4f);
		}
		else
		{
			if (e.Data != CachedEventDataOpenEnd)
			{
				return;
			}
			CachedManagers.CachedGUIScreenManager.HideCurrentScreen(true, delegate
			{
				CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.white;
				CachedManagers.CachedGUIScreenManager.ShowScreen(GUIScreenType.Roulette, delegate
				{
					CachedManagers.CachedGameManager.CameraController.CachedCamera.ScreenCamera.backgroundColor = Color.black;
				});
			}, false);
		}
	}
}
