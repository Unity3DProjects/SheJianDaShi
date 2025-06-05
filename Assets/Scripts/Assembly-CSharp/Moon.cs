using UnityEngine;

public class Moon : MonoBehaviour
{
	[SerializeField]
	private Transform cosmonautGameObject;

	[SerializeField]
	[ResourceLink]
	private AssetLink trailPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink idlePrefab;

	[SerializeField]
	private tk2dSprite sprite;

	[SerializeField]
	private float minPositionY;

	[SerializeField]
	private float maxPositionY;

	private GameObject trail;

	private GameObject idle;

	private Vector3 preliminaryPosition;

	private Managers cachedManagers;

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	public Transform CosmonautGameObject
	{
		get
		{
			return cosmonautGameObject;
		}
	}

	public float CornerPositionX
	{
		get
		{
			return base.transform.position.x + (float)((Velocity.x > 0f) ? 1 : (-1)) * sprite.GetBounds().size.x / 2f;
		}
	}

	public Vector3 Velocity { get; set; }

	private void Awake()
	{
		if (trailPrefab != null && !string.IsNullOrEmpty(trailPrefab.assetGUID))
		{
			trail = (GameObject)trailPrefab.GetInstance();
			trail.transform.parent = base.transform;
			trail.transform.localPosition = Vector3.zero;
		}
		if (idlePrefab != null && !string.IsNullOrEmpty(idlePrefab.assetGUID))
		{
			idle = (GameObject)idlePrefab.GetInstance();
			idle.transform.parent = base.transform;
			idle.transform.localPosition = Vector3.zero;
		}
	}

	public void UpdateMoon(float deltaTime, float velocityPercent)
	{
		preliminaryPosition = base.transform.position + Vector3.right * deltaTime * velocityPercent * Velocity.x + deltaTime * velocityPercent * Velocity.y * Vector3.up;
		Vector3 positionOnFloor = CachedManagers.CachedLevelsManager.CurrentLevel.Location.GetPositionOnFloor(preliminaryPosition.x);
		base.transform.position = new Vector3(preliminaryPosition.x, Mathf.Clamp(preliminaryPosition.y, positionOnFloor.y - minPositionY, positionOnFloor.y + maxPositionY), preliminaryPosition.z);
	}

	public void DestroyTrailEffect()
	{
		if (trail != null)
		{
			Object.Destroy(trail.gameObject);
			trail = null;
		}
	}
}
