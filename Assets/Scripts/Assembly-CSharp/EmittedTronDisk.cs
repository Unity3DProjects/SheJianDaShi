using System;
using UnityEngine;

public class EmittedTronDisk : WeaponGeneral
{
	[SerializeField]
	private bool isTragetForCamera;

	[SerializeField]
	private bool isInverted;

	[SerializeField]
	private float waveFrequency;

	[SerializeField]
	private float waveAmplitude;

	private Vector3 previousWavePosition = Vector3.zero;

	private float currentAngle;

	private GameObject cameraTarget;

	public override void Shoot()
	{
		base.Shoot();
		cameraTarget = new GameObject();
		cameraTarget.transform.parent = base.transform;
		previousWavePosition = previousPosition;
		if (isTragetForCamera && base.Level.IsTurnable)
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.SetTarget(cameraTarget.transform, Vector2.zero, Vector2.zero);
		}
	}

	protected override void UpdateNonPhysicalWeapon(float deltaTime)
	{
		if (!isFlying)
		{
			return;
		}
		if (isAccelerationEnabled)
		{
			velocity = new Vector3(velocity.x + velocity.normalized.x * acceleration * deltaTime, velocity.y + velocity.normalized.y * acceleration * deltaTime);
		}
		Vector3 vector = new Vector3(velocity.x * deltaTime, velocity.y * deltaTime);
		previousPosition.x += vector.x;
		previousPosition.y += vector.y;
		currentAngle += deltaTime * (float)Math.PI * waveFrequency;
		float num = Mathf.Sin(currentAngle) * waveAmplitude;
		float num2 = (float)Math.PI / 2f - Mathf.Atan2(velocity.x, velocity.y);
		float f = num2 + (float)Math.PI / 2f;
		Vector2 vector2 = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * num;
		if (!isInverted)
		{
			previousWavePosition.x = previousPosition.x + vector2.x;
			previousWavePosition.y = previousPosition.y + vector2.y;
		}
		else
		{
			previousWavePosition.x = previousPosition.x - vector2.x;
			previousWavePosition.y = previousPosition.y - vector2.y;
		}
		Vector3 vector3 = previousWavePosition - base.CachedTransform.position;
		float z = 90f - Mathf.Atan2(vector3.x, vector3.y) * 57.29578f;
		SphereCollider sphereCollider = (SphereCollider)base.Colliders[0];
		RaycastHit hitInfo;
		if (!Physics.SphereCast(base.CachedTransform.position, sphereCollider.radius, vector3.normalized, out hitInfo, vector3.magnitude) || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("MisteryBox"))
		{
			base.CachedTransform.rotation = Quaternion.Euler(0f, 0f, z);
			base.CachedTransform.position = previousWavePosition;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y);
		}
		else
		{
			ICollidable[] componentsInParent = hitInfo.collider.gameObject.GetComponentsInParent<ICollidable>();
			if (!hitInfo.collider.isTrigger)
			{
				previousPosition = base.CachedTransform.position + (hitInfo.point - base.CachedTransform.position).normalized * (Vector3.Distance(base.CachedTransform.position, hitInfo.point) - sphereCollider.radius);
				CollisionInfo collisionInfo = new CollisionInfo();
				collisionInfo.collider = base.Colliders[0];
				collisionInfo.otherCollider = hitInfo.collider;
				collisionInfo.position = hitInfo.point;
				collisionInfo.direction = velocity.normalized;
				collisionInfo.decalType = decalType;
				collisionInfo.fatality = base.WeaponType;
				collisionInfo.fatalityActvationType = fatalityActivationType;
				collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
				ICollidable[] array = componentsInParent;
				int num3 = 0;
				if (num3 < array.Length)
				{
					ICollidable collidable = array[num3];
					if (collidable is Character && !collisionInfo.otherCollider.isTrigger)
					{
						SkeletonUtilityBone component = hitInfo.collider.transform.parent.GetComponent<SkeletonUtilityBone>();
						if ((bool)component)
						{
							collisionInfo.boneName = component.boneName;
						}
						if ((collidable as Character).IsFinishHim)
						{
							AudioManager.PlayAudioWithKey(characterHitSound);
							isCollideEnabled = true;
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
					else
					{
						Collided(collisionInfo);
					}
				}
			}
			else
			{
				ICollidable[] array2 = componentsInParent;
				foreach (ICollidable collidable2 in array2)
				{
					if (collidable2 is PowerUp)
					{
						(collidable2 as PowerUp).Hit();
					}
				}
			}
		}
		cameraTarget.transform.position = previousPosition;
	}

	protected override void UpdatePreviousPosition()
	{
	}
}
