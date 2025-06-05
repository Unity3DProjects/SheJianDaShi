using UnityEngine;

public class BodyPartVelocitySetter : MonoBehaviour
{
	[SerializeField]
	private float velocityMinX = -500f;

	[SerializeField]
	private float velocityMaxX = 500f;

	[SerializeField]
	private float velocityMinY = 400f;

	[SerializeField]
	private float velocityMaxY = 500f;

	[SerializeField]
	private float angleVelocity = 20f;

	private void Start()
	{
		Rigidbody component = GetComponent<Rigidbody>();
		float x = Random.value * (velocityMaxX - velocityMinX) + velocityMinX;
		float y = Random.value * (velocityMaxY - velocityMinY) + velocityMinY;
		component.velocity = new Vector3(x, y, component.velocity.z);
		component.angularVelocity = Vector3.forward * angleVelocity;
	}
}
