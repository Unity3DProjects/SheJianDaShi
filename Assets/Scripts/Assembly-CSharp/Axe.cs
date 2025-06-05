using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class Axe : WeaponGeneral
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<Axe> m_OnFloor;

	[SerializeField]
	private Collider headCollider;

	private bool isFatalityWeapon;

	public static event Action<Axe> OnFloor
	{
		add
		{
			Action<Axe> action = Axe.m_OnFloor;
			Action<Axe> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Axe.m_OnFloor, (Action<Axe>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<Axe> action = Axe.m_OnFloor;
			Action<Axe> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref Axe.m_OnFloor, (Action<Axe>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void OnTriggerEnter(Collider collider)
	{
		if (!isCollideEnabled)
		{
			return;
		}
		base.OnTriggerEnter(collider);
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = headCollider;
		collisionInfo.direction = Vector3.down;
		collisionInfo.otherCollider = collider;
		collisionInfo.position = collider.transform.position;
		collisionInfo.decalType = decalType;
		collisionInfo.fatality = base.WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
		ICollidable[] componentsInParent = collider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Character)
			{
				SkeletonUtilityBone component = collider.transform.parent.GetComponent<SkeletonUtilityBone>();
				if ((bool)component)
				{
					collisionInfo.boneName = component.boneName;
				}
				isCollideEnabled = true;
				if (!(collidable as Character).IsFinishHim)
				{
					break;
				}
				AudioManager.PlayAudioWithKey(characterHitSound);
				FatalityBase activeFatality = (collidable as Character).FatalityManager.ActiveFatality;
				if (!activeFatality)
				{
					(collidable as Character).FatalityManager.StartFatality(collisionInfo);
					SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
					{
						if (!isFatalityWeapon)
						{
							isFatalityWeapon = true;
							base.CachedRigidbody.velocity = Vector3.zero;
							if (Axe.m_OnFloor != null)
							{
								Axe.m_OnFloor(this);
							}
						}
					}, 0.5f);
				}
				else if (activeFatality is FatalityHatchetBrutality)
				{
					(activeFatality as FatalityHatchetBrutality).TryBreakPart(collisionInfo);
				}
				else if (activeFatality is FatalityBreak)
				{
					(activeFatality as FatalityBreak).BreakPart(collisionInfo);
				}
				break;
			}
			if (!(collidable is Ground))
			{
				continue;
			}
			if (!isFatalityWeapon)
			{
				isFatalityWeapon = true;
				Collided(collisionInfo);
				if (Axe.m_OnFloor != null)
				{
					Axe.m_OnFloor(this);
				}
			}
			break;
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable is Ground || collidable is CharacterBase || collidable is Bird)
		{
			AudioManager.StopAudioWithKey(idleSound, 0.1f);
		}
		return collidable;
	}

	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		if (!isFatalityWeapon)
		{
			base.DriveInto(collidedObject, collisionInfo, depth);
		}
		else
		{
			base.CachedRigidbody.isKinematic = true;
		}
	}

	public void ColliderEnable(bool isEnabled)
	{
		isCollideEnabled = isEnabled;
		SetCollidersEnabled(isEnabled);
	}

	protected override void SetCollidersEnabled(bool isEnabled)
	{
		if ((bool)level && (bool)level.InactiveArcher && level.InactiveArcher.IsFinishHim)
		{
			foreach (Collider collider in base.Colliders)
			{
				if (collider == headCollider)
				{
					collider.enabled = true;
					collider.isTrigger = true;
				}
				else
				{
					collider.gameObject.layer = LayerMask.NameToLayer("IgnoreCharacterCollisions");
				}
			}
			return;
		}
		base.SetCollidersEnabled(isEnabled);
	}
}
