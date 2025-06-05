using UnityEngine;

public class EnvironmentDeer : EnvironmentObject
{
	[SerializeField]
	private float delayBetweenAnimationsMin = 5f;

	[SerializeField]
	private float delayBetweenAnimationsMax = 10f;

	private float currentDelay;

	public override void StartEnvironment()
	{
		base.EnvironmentState.SetAnimation(0, base.AnimationIdle, false);
		currentDelay = Random.Range(delayBetweenAnimationsMin, delayBetweenAnimationsMax);
	}

	public override void UpdateEnvironment(float deltaTime)
	{
		base.UpdateEnvironment(deltaTime);
		currentDelay -= deltaTime;
		if (currentDelay < 0f)
		{
			base.EnvironmentState.SetAnimation(0, base.AnimationIdle, false);
			currentDelay = Random.Range(delayBetweenAnimationsMin, delayBetweenAnimationsMax);
		}
	}
}
