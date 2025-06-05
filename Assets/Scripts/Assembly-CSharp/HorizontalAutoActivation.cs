using UnityEngine;

public class HorizontalAutoActivation : AutoActivation
{
	[SerializeField]
	private float activationWidth = 400f;

	private Vector3 startPosition = Vector3.zero;

	private float activationPositionX;

	public override void CalculateActivationTime(WeaponGeneral weapon, Level level, Vector3 startPosition, Vector3 enemyPosition)
	{
		this.startPosition = startPosition;
		activationPositionX = Random.Range(enemyPosition.x - activationWidth * 0.5f, enemyPosition.x + activationWidth * 0.5f);
	}

	public override bool IsNeedActivate(Vector3 position, Vector3 enemyPosition, float deltaTime)
	{
		bool result = false;
		if (isActivationEnabled)
		{
			if (startPosition.x > activationPositionX && activationPositionX >= position.x)
			{
				result = true;
			}
			else if (startPosition.x < activationPositionX && activationPositionX <= position.x)
			{
				result = true;
			}
		}
		return result;
	}
}
