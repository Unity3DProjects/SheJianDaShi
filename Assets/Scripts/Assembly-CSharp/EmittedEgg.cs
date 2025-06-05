using UnityEngine;

public class EmittedEgg : WeaponGeneral
{
	[SerializeField]
	[ResourceLink]
	protected AssetLink eggExplodeEffectPrefab;

	[SerializeField]
	private float fatalityDelay;

	private bool isFatalityWithDelay;

	public override ICollidable Collided(CollisionInfo collisionInfo)
	{
		ICollidable collidable = base.Collided(collisionInfo);
		if (eggExplodeEffectPrefab != null && !string.IsNullOrEmpty(eggExplodeEffectPrefab.assetGUID))
		{
			Object.Instantiate(eggExplodeEffectPrefab.GetAsset(), base.transform.position, Quaternion.identity, base.Level.FXsTransform);
		}
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		Character character = collidable as Character;
		if (character != null)
		{
			isFatalityWithDelay = character.IsFinishHim;
		}
		if (component != null)
		{
			component.Explode(this, level, true, true, isFatalityWithDelay, fatalityDelay);
		}
		base.CachedRigidbody.isKinematic = true;
		Disabled(true);
		return collidable;
	}
}
