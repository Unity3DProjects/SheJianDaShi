using UnityEngine;

public class PeakAutoActivation : AutoActivation
{
	[SerializeField]
	private float minTimeAfterPeak = 0.3f;

	[SerializeField]
	private float maxTimeAfterPeak = 0.5f;

	private float activationTime;

	public override void CalculateActivationTime(WeaponGeneral weapon, Level level, Vector3 startPosition, Vector3 enemyPosition)
	{
		float timeForTrajectoryPeak = weapon.GetTimeForTrajectoryPeak();
		if (timeForTrajectoryPeak > 0f)
		{
			activationTime = timeForTrajectoryPeak + Random.Range(minTimeAfterPeak, maxTimeAfterPeak);
		}
		else
		{
			isActivationEnabled = false;
		}
	}

	public override bool IsNeedActivate(Vector3 position, Vector3 enemyPosition, float deltaTime)
	{
		bool result = false;
		if (isActivationEnabled)
		{
			activationTime -= deltaTime;
			if (activationTime < 0f)
			{
				result = true;
			}
		}
		return result;
	}
}
