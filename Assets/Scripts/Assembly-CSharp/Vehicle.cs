using UnityEngine;

public class Vehicle : MonoBehaviour
{
	[SerializeField]
	private Transform frontWheelPoint;

	[SerializeField]
	private Transform backWheelPoint;

	[SerializeField]
	private Vector2 bikePositionOffset;

	private float distanceBetweenWheels = -1f;

	private float angle;

	public float DistanceBetweenWheels
	{
		get
		{
			if (distanceBetweenWheels <= 0f)
			{
				distanceBetweenWheels = Vector2.Distance(frontWheelPoint.position, backWheelPoint.position);
			}
			return distanceBetweenWheels;
		}
	}

	public virtual void SetVehiclePosition(float frontWheelXPosition)
	{
		float y = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(frontWheelXPosition).y;
		float num = frontWheelXPosition - DistanceBetweenWheels;
		float y2 = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(num).y;
		if (DistanceBetweenWheels > Mathf.Abs(y - y2))
		{
			float num2 = Mathf.Sqrt(Mathf.Pow(DistanceBetweenWheels, 2f) - Mathf.Pow(y - y2, 2f));
			float num3 = num - num2;
			float y3 = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(num3).y;
			Vector2 vector = new Vector2(frontWheelXPosition - num3, y - y3);
			float num4 = Mathf.Sqrt(Mathf.Pow(vector.x, 2f) + Mathf.Pow(vector.y, 2f));
			angle = Mathf.Acos((frontWheelXPosition - num3) / num4) * 57.29578f;
			if (y3 > y)
			{
				angle *= -1f;
			}
		}
		SetBikeAngle(angle);
		base.transform.position = new Vector3(frontWheelXPosition + bikePositionOffset.x, y + bikePositionOffset.y, base.transform.position.z);
	}

	private void SetBikeAngle(float angle)
	{
		base.transform.Rotate(base.transform.forward, 0f - base.transform.rotation.eulerAngles.z + angle);
	}
}
