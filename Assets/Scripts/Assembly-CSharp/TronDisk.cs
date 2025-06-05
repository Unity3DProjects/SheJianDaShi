using UnityEngine;

public class TronDisk : WeaponGeneral
{
	[SerializeField]
	private Collider bodyCollider;

	protected override void OnTriggerEnter(Collider collider)
	{
		base.OnTriggerEnter(collider);
		if (!isCollideEnabled || !level.InactiveArcher || !level.InactiveArcher.IsFinishHim)
		{
			return;
		}
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = bodyCollider;
		collisionInfo.direction = Vector3.down;
		collisionInfo.otherCollider = collider;
		collisionInfo.position = collider.transform.position;
		collisionInfo.decalType = decalType;
		collisionInfo.fatality = base.WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
		ICollidable[] componentsInParent = collider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		int num = 0;
		if (num >= array.Length)
		{
			return;
		}
		ICollidable collidable = array[num];
		if (collidable is Character)
		{
			SkeletonUtilityBone component = collider.transform.parent.GetComponent<SkeletonUtilityBone>();
			if ((bool)component)
			{
				collisionInfo.boneName = component.boneName;
			}
			isCollideEnabled = true;
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
		else
		{
			Collided(collisionInfo);
		}
	}

	public override void Shoot()
	{
		base.Shoot();
		AudioManager.StopAudioWithKey(idleSound);
	}

	protected override void SetCollidersEnabled(bool isEnabled)
	{
		if ((bool)level && (bool)level.InactiveArcher && level.InactiveArcher.IsFinishHim)
		{
			foreach (Collider collider in base.Colliders)
			{
				if (collider == bodyCollider)
				{
					collider.enabled = true;
					collider.isTrigger = true;
				}
				else
				{
					collider.enabled = false;
				}
			}
			return;
		}
		base.SetCollidersEnabled(isEnabled);
	}
}
