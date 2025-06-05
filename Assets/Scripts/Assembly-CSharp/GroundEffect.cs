using System;
using UnityEngine;

public class GroundEffect : MonoBehaviour
{
	[SerializeField]
	private Transform effectRightEnd;

	[SerializeField]
	private Transform effectLeftEnd;

	public float EffectLength
	{
		get
		{
			return effectLeftEnd.position.x - effectRightEnd.position.x;
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		GetComponentInChildren<Collider>().enabled = false;
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSystem in array)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			main.startRotation = new ParticleSystem.MinMaxCurve((0f - collider.transform.rotation.eulerAngles.z) * ((float)Math.PI / 180f));
			particleSystem.Clear();
			particleSystem.Play();
		}
	}
}
