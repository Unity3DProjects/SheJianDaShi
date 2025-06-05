using UnityEngine;

public class RadialAutoActivation : AutoActivation
{
	[SerializeField]
	private float maxRadius = 500f;

	[SerializeField]
	private float minRadius = 300f;

	private float activationRadius;

	public override void CalculateActivationTime(WeaponGeneral weapon, Level level, Vector3 startPosition, Vector3 enemyPosition)
	{
		activationRadius = Random.Range(minRadius, maxRadius);
	}

	public override bool IsNeedActivate(Vector3 position, Vector3 enemyPosition, float deltaTime)
	{
		bool result = false;
		if (isActivationEnabled)
		{
			result = Vector3.Distance(enemyPosition, position) < activationRadius;
		}
		return result;
	}
}
