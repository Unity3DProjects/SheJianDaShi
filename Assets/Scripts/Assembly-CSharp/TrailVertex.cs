using System.Collections.Generic;
using UnityEngine;

public class TrailVertex : MonoBehaviour
{
	private List<Vertex> vertices = new List<Vertex>();

	[SerializeField]
	private LineRenderer trail;

	[SerializeField]
	private float trailTime = 0.1f;

	[SerializeField]
	private Vector3 offsetTrail = Vector3.zero;

	[SerializeField]
	private string sortingLayerName;

	private Transform cachedTransform;

	private Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public void InitTrail(Transform parent)
	{
		InitTrail(parent, offsetTrail);
	}

	public void InitTrail(Transform parent, Vector3 offset)
	{
		CachedTransform.parent = parent;
		CachedTransform.localPosition = offset;
		if (trail != null && !string.IsNullOrEmpty(sortingLayerName))
		{
			trail.sortingLayerName = sortingLayerName;
		}
	}

	public void UpdateTrail(float deltaTime, bool isAddVertex)
	{
		if (!(trail != null))
		{
			return;
		}
		float time = Time.time;
		for (int num = vertices.Count - 1; num >= 0; num--)
		{
			if (time - vertices[num].time > trailTime)
			{
				vertices.RemoveAt(num);
			}
		}
		if (isAddVertex)
		{
			Vertex item = default(Vertex);
			item.time = time;
			item.point = CachedTransform.TransformPoint(Vector3.zero);
			vertices.Insert(0, item);
		}
		int i = 1;
		for (int num2 = vertices.Count - 1; i < num2; i++)
		{
			Vertex vertex = vertices[i - 1];
			Vertex value = vertices[i];
			Vertex vertex2 = vertices[i + 1];
			value.point = (vertex.point + vertex2.point) * 0.5f;
			vertices[i] = value;
		}
		trail.positionCount = vertices.Count;
		int j = 0;
		for (int count = vertices.Count; j < count; j++)
		{
			if (trail.useWorldSpace)
			{
				trail.SetPosition(j, vertices[j].point);
			}
			else
			{
				trail.SetPosition(j, CachedTransform.InverseTransformPoint(vertices[j].point));
			}
		}
	}

	public void SetWidthTrail(float start, float end)
	{
		trail.startWidth = start;
		trail.endWidth = end;
		trail.widthCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	}
}
