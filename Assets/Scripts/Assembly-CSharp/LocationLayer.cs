using UnityEngine;

public class LocationLayer : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	private float paralaxMultiplier;

	[SerializeField]
	private GameObject layerPrefab;

	protected LocationMovingObject[] movingObjects;

	public float ParallaxMultiplier
	{
		get
		{
			return paralaxMultiplier;
		}
	}

	public virtual void Init(float locationWidth, float locationHeight)
	{
		if ((bool)layerPrefab)
		{
			GameObject gameObject = CreateLayerPart(0);
			float num = LayerWidth(gameObject);
			int num2 = Mathf.CeilToInt(locationWidth / num);
			gameObject.transform.localPosition = new Vector3((float)(1 - num2) * 0.5f * num, 0f, 0f);
			for (int i = 1; i < num2; i++)
			{
				GameObject gameObject2 = CreateLayerPart(i);
				gameObject2.transform.localPosition = new Vector3(((float)(1 - num2) * 0.5f + (float)i) * num, 0f, 0f);
			}
			movingObjects = GetComponentsInChildren<LocationMovingObject>();
			LocationMovingObject[] array = movingObjects;
			foreach (LocationMovingObject locationMovingObject in array)
			{
				locationMovingObject.Init(num * (float)num2);
			}
		}
	}

	public virtual void UpdateLayer(float deltaTime)
	{
		if (movingObjects != null)
		{
			int i = 0;
			for (int num = movingObjects.Length; i < num; i++)
			{
				movingObjects[i].UpdateObject(deltaTime);
			}
		}
	}

	protected virtual float LayerWidth(GameObject part)
	{
		SkeletonAnimation[] componentsInChildren = part.GetComponentsInChildren<SkeletonAnimation>();
		SkeletonAnimation[] array = componentsInChildren;
		foreach (SkeletonAnimation skeletonAnimation in array)
		{
			skeletonAnimation.Reset();
			skeletonAnimation.Initialize();
		}
		float num = float.MaxValue;
		float num2 = float.MinValue;
		Renderer[] componentsInChildren2 = part.GetComponentsInChildren<Renderer>();
		Renderer[] array2 = componentsInChildren2;
		foreach (Renderer renderer in array2)
		{
			float num3 = renderer.bounds.center.x - renderer.bounds.extents.x;
			float num4 = renderer.bounds.center.x + renderer.bounds.extents.x;
			if (num3 < num)
			{
				num = num3;
			}
			if (num4 > num2)
			{
				num2 = num4;
			}
		}
		return num2 - num;
	}

	private GameObject CreateLayerPart(int index)
	{
		GameObject gameObject = Object.Instantiate(layerPrefab);
		gameObject.transform.parent = base.transform;
		gameObject.name = "Part_" + index;
		return gameObject;
	}
}
