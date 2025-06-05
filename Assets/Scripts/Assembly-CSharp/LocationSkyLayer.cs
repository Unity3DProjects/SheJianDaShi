using System.Collections.Generic;
using UnityEngine;

public class LocationSkyLayer : LocationLayer
{
	[SerializeField]
	private tk2dTiledSprite sky;

	[SerializeField]
	private tk2dTiledSprite skyPart;

	[SerializeField]
	private Transform cloudsParent;

	[SerializeField]
	private List<GameObject> cloudsPrefabs;

	[SerializeField]
	private float minSpeedCloud;

	[SerializeField]
	private float maxSpeedCloud;

	[SerializeField]
	private float minRelativeCloudsHeight = 0.35f;

	[SerializeField]
	private float maxRelativeCloudsHeight = 1f;

	[SerializeField]
	private float minPositionOffset = 50f;

	[SerializeField]
	private float maxPositionOffset = 200f;

	private float leftCloudsPosition;

	private float rightCloudsPosition;

	private int cloudKeysCount;

	private Dictionary<Transform, float> clouds = new Dictionary<Transform, float>();

	private List<Transform> cloudsKeys = new List<Transform>();

	public override void Init(float locationWidth, float locationHeight)
	{
		base.Init(locationWidth, locationHeight);
		sky.scale = new Vector3(locationWidth, locationHeight, 1f);
		sky.transform.localPosition = new Vector3((0f - locationWidth) * 0.5f, sky.transform.localPosition.y, sky.transform.localPosition.z);
		if (skyPart != null)
		{
			skyPart.scale = new Vector3(locationWidth, skyPart.scale.y, 1f);
			skyPart.transform.localPosition = new Vector3((0f - locationWidth) * 0.5f, skyPart.transform.localPosition.y, skyPart.transform.localPosition.z);
		}
		GenerateClouds(locationWidth, locationHeight);
	}

	public override void UpdateLayer(float deltaTime)
	{
		for (int i = 0; i < cloudKeysCount; i++)
		{
			Transform transform = cloudsKeys[i];
			Vector3 position = transform.position;
			if (position.x < leftCloudsPosition)
			{
				transform.position = new Vector3(rightCloudsPosition, position.y, position.z);
			}
			transform.localPosition += Vector3.left * clouds[transform] * deltaTime;
		}
	}

	private void GenerateClouds(float locationWidth, float locationHeight)
	{
		if ((bool)cloudsParent)
		{
			leftCloudsPosition = (0f - locationWidth) * 0.5f;
			rightCloudsPosition = locationWidth * 0.5f;
			float min = locationHeight * minRelativeCloudsHeight;
			float max = locationHeight * maxRelativeCloudsHeight;
			for (float num = leftCloudsPosition; num <= rightCloudsPosition; num += Random.Range(minPositionOffset, maxPositionOffset))
			{
				GameObject gameObject = Object.Instantiate(cloudsPrefabs.RandomObject());
				gameObject.transform.parent = cloudsParent;
				gameObject.transform.position = new Vector3(num, sky.transform.position.y + Random.Range(min, max));
				clouds.Add(gameObject.transform, Random.Range(minSpeedCloud, maxSpeedCloud));
			}
			cloudsKeys = new List<Transform>(clouds.Keys);
			cloudKeysCount = cloudsKeys.Count;
		}
	}
}
