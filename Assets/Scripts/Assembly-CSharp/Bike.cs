using UnityEngine;

public class Bike : Vehicle
{
	[SerializeField]
	private GameObject frontWheelGameObject;

	[SerializeField]
	private float frontWheelAngleSpeed;

	[SerializeField]
	private Transform seatPlace;

	public Vector3 SeatPlace
	{
		get
		{
			if (seatPlace == null)
			{
				return Vector3.zero;
			}
			return seatPlace.position;
		}
	}

	public void SetBikePosition(float frontWheelXPosition, float deltaTime)
	{
		SetVehiclePosition(frontWheelXPosition);
		frontWheelGameObject.transform.Rotate(Vector3.forward, frontWheelAngleSpeed * deltaTime);
	}
}
