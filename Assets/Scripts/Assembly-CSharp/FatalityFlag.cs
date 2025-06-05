using UnityEngine;

public class FatalityFlag : FatalityBase
{
	[SerializeField]
	private GameObject prefabSatelite;

	[SerializeField]
	private float spawnHeight;

	[SerializeField]
	private string bodyName;

	private GameObject currentSatelite;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			currentSatelite = Object.Instantiate(prefabSatelite);
			currentSatelite.transform.position = base.transform.TransformPoint(new Vector2(0f, spawnHeight));
			ObserverPhysics observerPhysics = currentSatelite.AddComponent<ObserverPhysics>();
			if ((bool)observerPhysics)
			{
				observerPhysics.EventCollisionEnter = ObserverPhysics_EventCollisionEnter;
			}
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		if ((bool)currentSatelite)
		{
			Object.Destroy(currentSatelite.gameObject);
		}
	}

	private void ObserverPhysics_EventCollisionEnter(Collision coll)
	{
		if (base.CachedManager.WithCharacterCollision(coll.collider))
		{
			ObserverPhysics component = currentSatelite.GetComponent<ObserverPhysics>();
			if ((bool)component)
			{
				component.EventCollisionEnter = null;
			}
			if (!base.CachedManager.CharacterRagdoll.IsActive)
			{
				base.CachedManager.CharacterRagdoll.Apply();
			}
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(bodyName);
			if ((bool)rigidbody)
			{
				rigidbody.constraints = (RigidbodyConstraints)56;
				BreakAllJoint(rigidbody);
			}
		}
	}
}
