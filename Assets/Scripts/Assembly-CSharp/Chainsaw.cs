using System;
using System.Collections;
using UnityEngine;

public class Chainsaw : WeaponGeneral
{
	[SerializeField]
	private Collider bladeTrigger;

	[SerializeField]
	private Animator animator;

	private bool isCharacterDamaged;

	private SkeletonAnimation chainsawAnimation;

	protected SkeletonAnimation ChainsawAnimation
	{
		get
		{
			if (chainsawAnimation == null)
			{
				chainsawAnimation = GetComponentInChildren<SkeletonAnimation>();
			}
			return chainsawAnimation;
		}
	}

	private bool IsCharacterDamaged
	{
		set
		{
			if (isCharacterDamaged != value)
			{
				isCharacterDamaged = value;
				if (isCharacterDamaged)
				{
					AudioManager.PlayAudioWithKey("chainsaw_character_pierce");
				}
				else
				{
					StartCoroutine(FadeSound("chainsaw_character_pierce", 0.2f));
				}
			}
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
		collisionInfo.collider = bladeTrigger;
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
			if (!(collidable is Character) && !(collidable is Bird))
			{
				continue;
			}
			IsCharacterDamaged = true;
			SkeletonUtilityBone component = collider.transform.parent.GetComponent<SkeletonUtilityBone>();
			if ((bool)component)
			{
				collisionInfo.boneName = component.boneName;
			}
			WeaponDamage[] components = bladeTrigger.GetComponents<WeaponDamage>();
			WeaponDamage[] array2 = components;
			foreach (WeaponDamage weaponDamage in array2)
			{
				weaponDamage.OnCollide(this, collisionInfo, collidable);
			}
			if ((bool)base.CachedEmission)
			{
				base.CachedEmission.EmitOnHitDriveInto(collider.transform.position);
			}
			if (collidable is Character && (collidable as Character).IsFinishHim)
			{
				AudioManager.PlayAudioWithKey(characterHitSound);
				FatalityBase activeFatality = (collidable as Character).FatalityManager.ActiveFatality;
				if (!activeFatality)
				{
					(collidable as Character).FatalityManager.StartFatality(collisionInfo);
				}
				else if (activeFatality is FatalityBreak)
				{
					(activeFatality as FatalityBreak).BreakPart(collisionInfo);
				}
			}
			break;
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			if (collidable is Ground)
			{
				WeaponCollider[] componentsInChildren = collisionInfo.collider.GetComponentsInChildren<WeaponCollider>();
				WeaponCollider[] array = componentsInChildren;
				foreach (WeaponCollider weaponCollider in array)
				{
					if (weaponCollider is WeaponDriveIntoCollider)
					{
						StartCoroutine(FadeSound(groundPierceSound, 1f));
					}
					else
					{
						AudioManager.PlayAudioWithKey("chainsaw_ground_main_hit");
					}
				}
				IsCharacterDamaged = false;
			}
			else
			{
				isCollideEnabled = true;
				IsCharacterDamaged = true;
			}
		}
		return collidable;
	}

	public override void DriveInto(ICollidable collidedObject, CollisionInfo collisionInfo, float depth)
	{
		float f = base.transform.eulerAngles.z * ((float)Math.PI / 180f);
		if (Mathf.Sin(f) <= -0.5f && collidedObject is Ground)
		{
			base.DriveInto(collidedObject, collisionInfo, depth);
			ChainsawAnimation.state.ClearTracks();
			animator.enabled = true;
		}
	}

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		ChainsawAnimation.Update(deltaTime);
	}

	protected override void SetCollidersEnabled(bool isEnabled)
	{
		if ((bool)level && (bool)level.InactiveArcher && level.InactiveArcher.IsFinishHim)
		{
			foreach (Collider collider in base.Colliders)
			{
				collider.enabled = true;
				if (collider != bladeTrigger)
				{
					collider.gameObject.layer = LayerMask.NameToLayer("IgnoreCharacterCollisions");
				}
			}
			return;
		}
		base.SetCollidersEnabled(isEnabled);
	}

	private IEnumerator FadeSound(string soundName, float fadeDuration)
	{
		yield return null;
		AudioManager.StopAudioWithKey(soundName, fadeDuration);
	}
}
