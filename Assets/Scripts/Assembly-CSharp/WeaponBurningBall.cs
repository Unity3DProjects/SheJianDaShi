using System.Collections.Generic;
using UnityEngine;

public class WeaponBurningBall : WeaponGeneral
{
	[SerializeField]
	private GameObject meshObj;

	[SerializeField]
	private EmittedObject emittedFirePrefab;

	[SerializeField]
	private float minSpeedForSpawnFire;

	private List<Vector3> spawnFirePositions = new List<Vector3>();

	private float ballRotation;

	protected override void Awake()
	{
		base.Awake();
		ballRotation = meshObj.transform.localRotation.x;
		disableSqrVelocityBorder = 70f;
		disableAngularVelocityBorder = 0.2f;
	}

	public override void UpdateWeapon(float deltaTime)
	{
		ballRotation += 1000f * deltaTime;
		base.UpdateWeapon(deltaTime);
		if (isFlying && isRotateFromDirection)
		{
			meshObj.transform.localRotation = Quaternion.Euler(ballRotation, meshObj.transform.localRotation.y, meshObj.transform.localRotation.z);
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			if ((collidable is Ground || collidable is Character || collidable is Bird) && (bool)base.IdleEffect)
			{
				base.IdleEffect.SetActive(base.enabled);
			}
			if ((bool)base.CachedEmission)
			{
				base.CachedEmission.StopTrails();
				if (base.IsActive && collidable is Ground && base.Velocity.magnitude > minSpeedForSpawnFire)
				{
					Vector3 collidePosition = level.Location.GetPositionOnFloor(collisionInfo.position.x);
					if (!spawnFirePositions.Exists((Vector3 position) => Mathf.Abs(collidePosition.x - position.x) < 20f))
					{
						EmittedObject emittedObject = Object.Instantiate(emittedFirePrefab);
						level.AddEmittedObject(emittedObject);
						emittedObject.transform.position = collidePosition;
						emittedObject.Init(level, Vector3.zero);
						spawnFirePositions.Add(collidePosition);
						AudioManager.PlayAudioWithKey("fire_start");
						AudioManager.PlayAudioWithKey("burning_ball_ground_hit");
					}
				}
			}
		}
		base.CachedRigidbody.maxAngularVelocity = float.PositiveInfinity;
		isRotateFromDirection = false;
		return collidable;
	}

	protected override void SetIdleEffectEnabled(bool enabled)
	{
	}
}
