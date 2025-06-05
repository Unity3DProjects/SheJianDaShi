using UnityEngine;

public class SwordFish : WeaponGeneral
{
	private const string RAGDOLL_NAME = "ragdoll";

	private const string APPLE_TAG = "Apple";

	[SerializeField]
	private float deltaAngle;

	[SerializeField]
	private float minYVelosity;

	[SerializeField]
	private float minXVelosity;

	private bool isCollided;

	protected override void OnCollisionEnter(Collision collision)
	{
		if (isCollided)
		{
			return;
		}
		if (collision.contacts[0].otherCollider.name.Contains("ragdoll") || (270f - deltaAngle < base.transform.eulerAngles.z && base.transform.eulerAngles.z < 270f + deltaAngle) || collision.collider.CompareTag("Apple"))
		{
			base.OnCollisionEnter(collision);
		}
		else
		{
			if (!base.IsActive)
			{
				return;
			}
			isCollideEnabled = true;
			if (Mathf.Abs(velocity.y) > minYVelosity && Mathf.Abs(velocity.x) > minXVelosity)
			{
				if (velocity.y < 0f)
				{
					isCollided = true;
					SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(0.2f);
					base.CachedRigidbody.velocity = (velocity = new Vector3(velocity.normalized.x * velocity.magnitude * bounciness, (0f - velocity.normalized.y) * velocity.magnitude * bounciness));
					if ((bool)base.CachedEmission)
					{
						base.CachedEmission.EmitOnCollision(collision.contacts[0].point);
					}
				}
			}
			else
			{
				Disabled();
			}
			AudioManager.PlayAudioWithKey(groundHitSound);
		}
	}

	protected void OnCollisionExit(Collision collision)
	{
		isCollided = false;
	}
}
