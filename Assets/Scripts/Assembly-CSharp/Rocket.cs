using System;
using UnityEngine;

public class Rocket : WeaponGeneral
{
	[SerializeField]
	private float waveFrequency;

	[SerializeField]
	private float waveAmplitude;

	[SerializeField]
	private float activatedSpeedMultiplier;

	private bool isBeingActivated;

	private Vector3 previousWavePosition = Vector3.zero;

	private float currentAngle;

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			ExplodeComponent component = GetComponent<ExplodeComponent>();
			if (component != null)
			{
				component.Explode(this, level);
			}
		}
		return collidable;
	}

	public override void Shoot()
	{
		base.Shoot();
		previousWavePosition = previousPosition;
	}

	protected override void UpdateNonPhysicalWeapon(float deltaTime)
	{
		if (!isBeingActivated)
		{
			if (isFlying)
			{
				velocity = new Vector3(velocity.x, velocity.y - base.Gravity * deltaTime);
				Vector3 vector = new Vector3(velocity.x * deltaTime, velocity.y * deltaTime);
				previousPosition.x += vector.x;
				previousPosition.y += vector.y;
				currentAngle += deltaTime * (float)Math.PI * waveFrequency;
				float num = Mathf.Cos(currentAngle) * waveAmplitude;
				float num2 = (float)Math.PI / 2f - Mathf.Atan2(velocity.x, velocity.y);
				previousWavePosition.x = previousPosition.x + Mathf.Cos(num2 + (float)Math.PI / 2f) * num;
				previousWavePosition.y = previousPosition.y + Mathf.Sin(num2 + (float)Math.PI / 2f) * num;
			}
		}
		else
		{
			base.UpdateNonPhysicalWeapon(deltaTime);
		}
		if (!isFlying)
		{
			return;
		}
		Vector3 vector2 = previousWavePosition - base.CachedTransform.position;
		float z = 90f - Mathf.Atan2(vector2.x, vector2.y) * 57.29578f;
		BoxCollider boxCollider = (BoxCollider)base.Colliders[0];
		RaycastHit hitInfo;
		if (!Physics.BoxCast(base.CachedTransform.position, boxCollider.bounds.extents, vector2.normalized, out hitInfo, Quaternion.Euler(new Vector3(0f, 0f, z)), vector2.magnitude) || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("MisteryBox"))
		{
			if (!isBeingActivated)
			{
				base.CachedTransform.rotation = Quaternion.Euler(0f, 0f, z);
				base.CachedTransform.position = previousWavePosition;
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y);
			}
			return;
		}
		previousPosition = base.CachedTransform.position + (hitInfo.point - base.CachedTransform.position).normalized * (Vector3.Distance(base.CachedTransform.position, hitInfo.point) - boxCollider.bounds.size.x * 0.5f);
		CollisionInfo collisionInfo = new CollisionInfo();
		collisionInfo.collider = base.Colliders[0];
		collisionInfo.otherCollider = hitInfo.collider;
		collisionInfo.position = hitInfo.point;
		collisionInfo.direction = vector2.normalized;
		collisionInfo.decalType = decalType;
		collisionInfo.fatality = base.WeaponType;
		collisionInfo.fatalityActvationType = fatalityActivationType;
		collisionInfo.speedRatio = base.Velocity.magnitude / maxSpeed;
		SkeletonUtilityBone component = collisionInfo.otherCollider.transform.parent.GetComponent<SkeletonUtilityBone>();
		if ((bool)component)
		{
			collisionInfo.boneName = component.boneName;
		}
		Collided(collisionInfo);
	}

	protected override void UpdatePreviousPosition()
	{
	}

	protected override void OnTapActivation()
	{
		isBeingActivated = true;
		isRotateFromDirection = true;
		float num = base.CachedRigidbody.rotation.eulerAngles.z * ((float)Math.PI / 180f);
		if (num != 0f)
		{
			velocity = new Vector3(velocity.magnitude * Mathf.Cos(num), velocity.magnitude * Mathf.Sin(num), velocity.z);
		}
		velocity *= activatedSpeedMultiplier;
	}
}
