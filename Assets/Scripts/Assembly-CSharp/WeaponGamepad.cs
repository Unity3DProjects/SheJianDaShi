using UnityEngine;

public class WeaponGamepad : WeaponGeneral
{
	[SerializeField]
	private float activationDuration;

	[SerializeField]
	private Animator activationAnimation;

	private float time;

	private Vector3 angVelocity = Vector3.zero;

	private Vector3 angAcceleration = Vector3.zero;

	private bool isRotationWillFinish;

	public override void Init(Level level)
	{
		base.Init(level);
		emitOnCollideMinVelocity = 0f;
	}

	public override void UpdateWeapon(float deltaTime)
	{
		base.UpdateWeapon(deltaTime);
		if (!isActivated)
		{
			return;
		}
		time += deltaTime;
		if (time >= periodicalDamageInfo.duration)
		{
			activationAnimation.enabled = false;
		}
		if (time >= activationDuration)
		{
			base.CachedRigidbody.isKinematic = false;
			AudioManager.StopAudioWithKey(activationSound);
			AudioManager.StopAudioWithKey(activationFlyingSound);
		}
		if (isRotationWillFinish)
		{
			float z = base.CachedRigidbody.transform.eulerAngles.z;
			angVelocity += angAcceleration * deltaTime;
			base.CachedRigidbody.transform.rotation = Quaternion.Euler(0f, 0f, base.CachedRigidbody.transform.eulerAngles.z + angVelocity.z * deltaTime);
			if (z < base.CachedRigidbody.transform.eulerAngles.z)
			{
				isRotationWillFinish = false;
				base.CachedRigidbody.transform.rotation = Quaternion.identity;
			}
		}
	}

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (collidable != null)
		{
			Disabled();
			level.RemoveWeapon(this);
		}
		return collidable;
	}

	protected override void StopAllSounds()
	{
		base.StopAllSounds();
		AudioManager.StopAudioWithKey(activationSound);
	}

	protected override void OnTapActivation()
	{
		float z = base.CachedRigidbody.transform.eulerAngles.z;
		float num = base.CachedRigidbody.angularVelocity.z / Time.deltaTime;
		float num2 = z / Mathf.Abs(num) + 0.2f;
		float z2 = (0f - z - num * num2) * 2f / Mathf.Pow(num2, 2f);
		angVelocity = new Vector3(0f, 0f, num);
		angAcceleration = new Vector3(0f, 0f, z2);
		isRotationWillFinish = true;
		base.CachedRigidbody.isKinematic = true;
		base.CachedRigidbody.velocity = Vector3.zero;
		base.CachedRigidbody.angularVelocity = Vector3.zero;
		activationAnimation.enabled = true;
		periodicalDamageInfo.position = base.transform.position;
		level.ApplyPeriodicalDamage(periodicalDamageInfo);
	}

	protected override void CheckDisablePhysicalWeapon(float deltaTime, bool flying = false)
	{
	}
}
