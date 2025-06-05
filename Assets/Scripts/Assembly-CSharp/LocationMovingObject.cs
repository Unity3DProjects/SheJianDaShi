using UnityEngine;

public class LocationMovingObject : MonoBehaviour
{
	[SerializeField]
	private Vector3 minVelocity;

	[SerializeField]
	private Vector3 maxVelocity;

	private Vector3 velocity = Vector3.zero;

	private float leftPosition;

	private float rightPosition;

	private bool isMoving;

	private SkeletonAnimation objectAnimation;

	public void Init(float locationWidth)
	{
		isMoving = true;
		RecalculateVelocity();
		leftPosition = -0.5f * locationWidth;
		rightPosition = 0.5f * locationWidth;
		objectAnimation = GetComponent<SkeletonAnimation>();
	}

	public void UpdateObject(float deltaTime)
	{
		if (isMoving)
		{
			base.transform.position += velocity * deltaTime;
			if (velocity.x > 0f && base.transform.position.x > rightPosition)
			{
				base.transform.position = new Vector3(leftPosition, base.transform.position.y, base.transform.position.z);
				RecalculateVelocity();
			}
			else if (velocity.x < 0f && base.transform.position.x < leftPosition)
			{
				base.transform.position = new Vector3(rightPosition, base.transform.position.y, base.transform.position.z);
				RecalculateVelocity();
			}
			objectAnimation.Update(deltaTime);
		}
	}

	private void RecalculateVelocity()
	{
		velocity = new Vector3(Random.Range(minVelocity.x, maxVelocity.x), Random.Range(minVelocity.y, maxVelocity.y), Random.Range(minVelocity.z, maxVelocity.z));
	}
}
