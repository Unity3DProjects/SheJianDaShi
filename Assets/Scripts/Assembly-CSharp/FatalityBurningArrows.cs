using System.Collections;
using UnityEngine;

public class FatalityBurningArrows : FatalitySatelite
{
	[SerializeField]
	private FatalityBricks fatalityArrowsPrefab;

	[SerializeField]
	private FatalityBricks fatalityExplosiveArrowsPrefab;

	[SerializeField]
	private float startFatalityDelay = 2f;

	[SerializeField]
	private float lastArrowDelay = 2f;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			StartCoroutine(ApplyFatalityArrows(startFatalityDelay));
		}
	}

	protected override void SetSateliteVelocity()
	{
	}

	protected IEnumerator ApplyFatalityArrows(float delay)
	{
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		yield return new WaitForSeconds(delay);
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		base.CachedMask.Activate(base.CachedManager.CurrentCharacter.HeadCollider, base.CachedManager.GetUtilityBoneHead());
		if ((bool)fatalityArrowsPrefab)
		{
			FatalityBricks fatalityBricks = Object.Instantiate(fatalityArrowsPrefab);
			fatalityBricks.transform.parent = base.CachedManager.transform;
			fatalityBricks.transform.localEulerAngles = Vector3.zero;
			fatalityBricks.transform.localPosition = Vector3.zero;
			fatalityBricks.StartFatalityBrick(base.gameObject);
		}
		StartCoroutine(AnimationFinished());
	}

	protected IEnumerator AnimationFinished()
	{
		yield return new WaitForSeconds(lastArrowDelay);
		if (fatalityExplosiveArrowsPrefab != null)
		{
			FatalityBricks fatalityBricks = Object.Instantiate(fatalityExplosiveArrowsPrefab);
			fatalityBricks.transform.parent = base.CachedManager.transform;
			fatalityBricks.transform.localEulerAngles = Vector3.zero;
			fatalityBricks.transform.localPosition = Vector3.zero;
			fatalityBricks.StartFatalityBrick(base.gameObject);
		}
	}
}
