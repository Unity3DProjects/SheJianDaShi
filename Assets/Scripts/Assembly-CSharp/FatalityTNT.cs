using UnityEngine;

public class FatalityTNT : FatalityBrick
{
	private const float burningDuration = 4.5f;

	private ObserverPhysics observerPhysics;

	protected override void Start()
	{
		base.Start();
		AudioManager.PlayLoopAudioWithKey("arrow_firework_idle");
		observerPhysics = base.gameObject.AddComponent<ObserverPhysics>();
		observerPhysics.EventCollisionEnter = ObserverPhysics_EventCollisionEnter;
	}

	private void ObserverPhysics_EventCollisionEnter(Collision coll)
	{
		AudioManager.PlayAudioWithKey("mjolnir_ground_collision");
		observerPhysics.EventCollisionEnter = null;
	}
}
