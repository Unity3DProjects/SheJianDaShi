using UnityEngine;

public class FatalityDumpedSign : FatalitySatelite
{
	[SerializeField]
	private FatalityBricks fatalityBricksPrefab;

	[SerializeField]
	private bool isDumpedSignVerticalSpeedNeeded = true;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			if (fatalityBricksPrefab != null)
			{
				FatalityBricks fatalityBricks = Object.Instantiate(fatalityBricksPrefab);
				fatalityBricks.transform.parent = base.CachedManager.transform;
				fatalityBricks.transform.localEulerAngles = Vector3.zero;
				fatalityBricks.transform.localPosition = Vector3.zero;
				fatalityBricks.StartFatalityBrick(base.gameObject);
			}
		}
	}

	protected override void SetSateliteVelocity()
	{
		if (isDumpedSignVerticalSpeedNeeded)
		{
			Rigidbody component = currentSatelite.GetComponent<Rigidbody>();
			if ((bool)component)
			{
				float x = 0f;
				float y = startVerticalSpeed;
				component.velocity = new Vector2(x, y);
			}
		}
		else
		{
			base.SetSateliteVelocity();
		}
	}
}
