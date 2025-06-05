using System.Collections.Generic;
using UnityEngine;

public class VerticalAutoActivation : AutoActivation
{
	[SerializeField]
	private float activationDistance = 400f;

	[SerializeField]
	private float activationHeight = 300f;

	[SerializeField]
	private float activationHeightOffset;

	private float activationPositionY;

	public override void CalculateActivationTime(WeaponGeneral weapon, Level level, Vector3 startPosition, Vector3 enemyPosition)
	{
		float timeForTrajectoryPeak = weapon.GetTimeForTrajectoryPeak();
		float num = enemyPosition.y + activationHeightOffset;
		float num2 = num + activationHeight;
		if (timeForTrajectoryPeak > 0f)
		{
			float positionYForTime = weapon.GetPositionYForTime(timeForTrajectoryPeak, startPosition.y);
			if (positionYForTime < num2)
			{
				num2 = positionYForTime;
			}
		}
		List<float> floorHeightsForRange = level.Location.GetFloorHeightsForRange(enemyPosition.x, startPosition.x);
		foreach (float item in floorHeightsForRange)
		{
			float num3 = item;
			float[] timeToPositionY = weapon.GetTimeToPositionY(startPosition.y, num3);
			if (timeToPositionY != null)
			{
				float minPositiveValue = weapon.GetMinPositiveValue(timeToPositionY);
				float positionXForTime = weapon.GetPositionXForTime(minPositiveValue, startPosition.x);
				if (level.Location.GetPositionOnFloor(positionXForTime).y == num3)
				{
					num = num3;
				}
			}
		}
		num = Mathf.Min(num + 50f, num2);
		activationPositionY = Random.Range(num, num2);
	}

	public override bool IsNeedActivate(Vector3 position, Vector3 enemyPosition, float deltaTime)
	{
		bool result = false;
		if (isActivationEnabled && Mathf.Abs(enemyPosition.x - position.x) <= activationDistance && position.y < activationPositionY)
		{
			result = true;
		}
		return result;
	}
}
