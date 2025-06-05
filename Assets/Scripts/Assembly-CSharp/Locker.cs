using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class Locker : MonoBehaviour
{
	private const string SPRITE_NAME_TO_CHANGE = "locker_door_opened";

	private const string KEY_DELEGATE_NAME = "Door";

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnCharacterHit;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnGroundStuck;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnOpenDoor;

	[SerializeField]
	private TweenRotation tweenRotation;

	[SerializeField]
	private TweenPosition tweenPosition;

	[SerializeField]
	private MeshRenderer closedDoorMesh;

	[SerializeField]
	private tk2dSprite closedDoorSprite;

	[SerializeField]
	private string layerAfterDoorOpen = "UnderCharacter";

	private FatalityManager cachedManager;

	public FatalityManager CachedManager
	{
		get
		{
			if (cachedManager == null)
			{
				cachedManager = GetComponentInParent<FatalityManager>();
			}
			return cachedManager;
		}
	}

	public event Action OnCharacterHit
	{
		add
		{
			Action action = this.m_OnCharacterHit;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCharacterHit, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnCharacterHit;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnCharacterHit, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnGroundStuck
	{
		add
		{
			Action action = this.m_OnGroundStuck;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnGroundStuck, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnGroundStuck;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnGroundStuck, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnOpenDoor
	{
		add
		{
			Action action = this.m_OnOpenDoor;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnOpenDoor, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnOpenDoor;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnOpenDoor, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		ObserverPhysics observerPhysics = base.gameObject.AddComponent<ObserverPhysics>();
		if ((bool)observerPhysics)
		{
			observerPhysics.EventTriggerEnter = ObserverPhysics_EventTriggerEnter;
		}
	}

	public void SetTweenPositionParams(Vector3 beginPosition, Vector3 endPosition, float duration)
	{
		tweenPosition.beginPosition = beginPosition;
		tweenPosition.endPosition = endPosition;
		tweenPosition.duration = duration;
		tweenPosition.enabled = true;
		tweenPosition.SetOnFinishedDelegate(OnFinishMoving);
		AudioManager.PlayAudioWithKey("satellite_flying");
	}

	private IEnumerator CoroutineOnStuck()
	{
		yield return new WaitForSeconds(1f);
		tweenRotation.enabled = true;
		tweenRotation.AddTimedCallback("Door", 0.5f, delegate
		{
			closedDoorSprite.SetSprite("locker_door_opened");
		});
		AudioManager.PlayAudioWithKey("slingshot_fatality_door_creak");
		tweenRotation.SetOnFinishedDelegate(OnOpenDoorFinished);
	}

	private void RemoveEventHandlers()
	{
		ObserverPhysics component = base.gameObject.GetComponent<ObserverPhysics>();
		if ((bool)component)
		{
			component.EventCollisionEnter = null;
			component.EventTriggerEnter = null;
		}
	}

	private void ObserverPhysics_EventTriggerEnter(Collider coll)
	{
		if (CachedManager.WithCharacterCollision(coll) && this.OnCharacterHit != null)
		{
			this.OnCharacterHit();
		}
	}

	private void OnFinishMoving(ITweener itw)
	{
		if (this.OnGroundStuck != null)
		{
			this.OnGroundStuck();
		}
		AudioManager.PlayAudioWithKey("satellite_character_collision");
		StartCoroutine(CoroutineOnStuck());
	}

	private void OnOpenDoorFinished(ITweener itw)
	{
		closedDoorMesh.sortingLayerName = layerAfterDoorOpen;
		if (this.OnOpenDoor != null)
		{
			this.OnOpenDoor();
		}
	}
}
