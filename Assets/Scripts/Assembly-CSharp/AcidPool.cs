using System;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem rootSystem;

	[SerializeField]
	private ParticleSystem[] rotateSystems;

	[SerializeField]
	private float lifeTime;

	private float time;

	public ParticleSystem RootSystem
	{
		get
		{
			return rootSystem;
		}
	}

	public float LifeTime
	{
		get
		{
			return lifeTime;
		}
		set
		{
			lifeTime = value;
		}
	}

	private void Update()
	{
		if (lifeTime >= 0f)
		{
			time += Time.deltaTime;
			if (time >= lifeTime)
			{
				rootSystem.Stop();
			}
			if (!rootSystem.IsAlive())
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public static Vector3 PositionOnFloor(Vector3 position, Collider groundCollider)
	{
		Ground componentInParent = groundCollider.GetComponentInParent<Ground>();
		Vector3 result = componentInParent.GroundPosition(position.x);
		if (componentInParent.Type != FloorPartType.Straight && componentInParent.Type != FloorPartType.None)
		{
			float num = 70f;
			Vector3 hightChangePosition = componentInParent.GetHightChangePosition();
			if (Vector2.Distance(hightChangePosition, position) <= num)
			{
				Vector3 zero = Vector3.zero;
				float f = groundCollider.transform.rotation.eulerAngles.z * ((float)Math.PI / 180f);
				zero = ((componentInParent.Type == FloorPartType.SkewLeft || componentInParent.Type == FloorPartType.SkewLeftSmall) ? ((!(position.x <= hightChangePosition.x)) ? new Vector3(num * Mathf.Cos(f), (0f - num) * Mathf.Sin(f)) : new Vector3((0f - num) * Mathf.Cos(f), (0f - num) * Mathf.Sin(f))) : ((!(position.x <= hightChangePosition.x)) ? new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f)) : new Vector3((0f - num) * Mathf.Cos(f), num * Mathf.Sin(f))));
				result = hightChangePosition + zero;
			}
		}
		return result;
	}

	public void RotateToAngle(Quaternion rotation)
	{
		ParticleSystem[] array = rotateSystems;
		foreach (ParticleSystem particleSystem in array)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			main.startRotation = new ParticleSystem.MinMaxCurve((0f - rotation.eulerAngles.z) * ((float)Math.PI / 180f));
			particleSystem.Clear();
			particleSystem.Play();
		}
		base.transform.rotation = rotation;
	}
}
