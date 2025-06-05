using UnityEngine;

public class EmittedAcidPart : EmittedObject
{
	[SerializeField]
	protected AcidPool acidPoolPrefab;

	private AcidPool acidPool;

	public override void RemoveObject()
	{
		if (!isNeedDestroy && (bool)acidPool)
		{
			acidPool.RootSystem.Stop();
		}
		base.RemoveObject();
	}

	protected override void Activate(CharacterBase character, Collider collider)
	{
		if (isActivated && !isNeedDestroy && (bool)acidPool)
		{
			acidPool.RootSystem.Stop();
		}
		base.Activate(character, collider);
	}

	protected override void OnGroundCollided(Collider collider, Vector3 position)
	{
		base.OnGroundCollided(collider, position);
		emitEffect.Stop();
		if (!isNeedDestroy)
		{
			acidPool = Object.Instantiate(acidPoolPrefab);
			acidPool.transform.parent = base.transform;
			acidPool.transform.position = AcidPool.PositionOnFloor(position, collider);
			acidPool.RotateToAngle(collider.transform.rotation);
			acidPool.LifeTime = -1f;
		}
	}
}
