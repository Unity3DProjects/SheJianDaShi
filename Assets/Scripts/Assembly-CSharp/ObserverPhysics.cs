using System;
using UnityEngine;

public class ObserverPhysics : MonoBehaviour
{
	public Action<Collision> EventCollisionEnter { private get; set; }

	public Action<Collision> EventCollisionStay { private get; set; }

	public Action<Collision> EventCollisionExit { private get; set; }

	public Action<Collider> EventTriggerEnter { private get; set; }

	private void OnCollisionEnter(Collision collision)
	{
		if (EventCollisionEnter != null)
		{
			EventCollisionEnter(collision);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (EventCollisionStay != null)
		{
			EventCollisionStay(collision);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (EventCollisionExit != null)
		{
			EventCollisionExit(collision);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (EventTriggerEnter != null)
		{
			EventTriggerEnter(collider);
		}
	}
}
