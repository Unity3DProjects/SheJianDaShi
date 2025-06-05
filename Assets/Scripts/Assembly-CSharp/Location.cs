using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
	[SerializeField]
	private LocationType type;

	[SerializeField]
	private string locationSound;

	[SerializeField]
	private Transform objectsTransform;

	[SerializeField]
	private Transform ground;

	[SerializeField]
	private float width;

	[SerializeField]
	private float height;

	[SerializeField]
	private tk2dSpriteCollectionData[] dataSpriteCollections;

	private List<LocationLayer> layers = new List<LocationLayer>();

	private List<EnvironmentObject> environmentObjects = new List<EnvironmentObject>();

	private CameraController cachedCameraController;

	public LocationType Type
	{
		get
		{
			return type;
		}
	}

	public Transform ObjectsTransform
	{
		get
		{
			return objectsTransform;
		}
	}

	private CameraController CachedCameraController
	{
		get
		{
			if (cachedCameraController == null)
			{
				cachedCameraController = SingletonMonoBehaviour<GameManager>.Instance.CameraController;
			}
			return cachedCameraController;
		}
	}

	private void OnDestroy()
	{
		if (SingletonMonoBehaviour<AudioManager>.InstanceIfExist != null)
		{
			AudioManager.StopAudioWithKey(locationSound);
		}
	}

	public void Init(FloorSettings settings)
	{
		GetComponentsInChildren(layers);
		foreach (LocationLayer layer in layers)
		{
			layer.Init(width, height);
		}
		GetComponentsInChildren(environmentObjects);
		GenerateFloor(settings);
		AudioManager.PlayAudioWithKey(locationSound);
	}

	public virtual void GenerateFloor(FloorSettings settings)
	{
		Floor component = GetComponent<Floor>();
		if ((bool)component)
		{
			ground.transform.localPosition = new Vector3((0f - width) * 0.5f, ground.transform.localPosition.y, ground.transform.localPosition.z);
			component.GenerateFloor(ground, settings, width);
		}
	}

	public List<Vector2> GenerateFloorHeights(FloorSettings settings)
	{
		Floor component = GetComponent<Floor>();
		if ((bool)component)
		{
			return component.GenerateFloorHeights(settings, width);
		}
		return null;
	}

	public void UpdateLocation(float deltaTime)
	{
		float x = CachedCameraController.Position.x;
		int i = 0;
		for (int count = layers.Count; i < count; i++)
		{
			LocationLayer locationLayer = layers[i];
			Vector3 position = locationLayer.transform.position;
			position.x = x * locationLayer.ParallaxMultiplier;
			locationLayer.transform.position = position;
			locationLayer.UpdateLayer(deltaTime);
		}
		if (environmentObjects != null)
		{
			int j = 0;
			for (int count2 = environmentObjects.Count; j < count2; j++)
			{
				environmentObjects[j].UpdateEnvironment(deltaTime);
			}
		}
	}

	public Vector3 GetCharacterPositionForRange(float startXPosition, float endXPosition)
	{
		Vector3 result = Vector3.zero;
		Floor component = GetComponent<Floor>();
		if ((bool)component)
		{
			result = component.GetCharacterPositionForRange(startXPosition, endXPosition);
		}
		return result;
	}

	public Vector3 GetPositionOnFloor(float position)
	{
		Vector3 result = Vector3.zero;
		Floor component = GetComponent<Floor>();
		if ((bool)component)
		{
			result = component.GroundPositionAtX(position);
		}
		return result;
	}

	public List<Vector3> GetHighChangePositionsForRange(float startPosition, float endPosition)
	{
		List<Vector3> result = new List<Vector3>();
		Floor component = GetComponent<Floor>();
		if ((bool)component)
		{
			result = component.GetHighChangePositionsForRange(startPosition, endPosition);
		}
		return result;
	}

	public List<float> GetFloorHeightsForRange(float startPosition, float endPosition)
	{
		List<float> result = new List<float>();
		Floor component = GetComponent<Floor>();
		if ((bool)component)
		{
			result = component.GetFloorHeightsForRange(startPosition, endPosition);
		}
		return result;
	}
}
