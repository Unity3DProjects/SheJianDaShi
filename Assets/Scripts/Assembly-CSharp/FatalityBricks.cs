using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalityBricks : MonoBehaviour
{
	[SerializeField]
	private FatalityBrick brickPrefab;

	[SerializeField]
	private int numberOfBricks;

	[SerializeField]
	protected Vector2 brickSpawnPoint;

	[SerializeField]
	protected float brickStartVerticalSpeed = -1000f;

	[SerializeField]
	private float brickDelayBeforeStart;

	[SerializeField]
	private float delayBetweenBricks = 0.1f;

	[SerializeField]
	protected float brickLeftBorder;

	[SerializeField]
	protected float brickRightBorder;

	[SerializeField]
	private bool isBricksVerticalSpeedNeeded;

	[SerializeField]
	private string soundName;

	protected List<string> breakBonesNames = new List<string>();

	protected List<FatalityBrick> bricks = new List<FatalityBrick>();

	private FatalityManager cachedFatalityManager;

	private FatalityEffect cachedEffect;

	private FatalityMask cachedMask;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private bool isExplosionAvailable = true;

	private bool isCollided;

	public FatalityManager CachedManager
	{
		get
		{
			if (cachedFatalityManager == null)
			{
				cachedFatalityManager = GetComponentInParent<FatalityManager>();
			}
			return cachedFatalityManager;
		}
	}

	protected SkeletonUtilityBone CachedHead
	{
		get
		{
			if (cachedHead == null)
			{
				cachedHead = CachedManager.GetUtilityBoneHead();
			}
			return cachedHead;
		}
	}

	protected SkeletonUtilityBone CachedBody
	{
		get
		{
			if (cachedBody == null)
			{
				cachedBody = CachedManager.GetUtilityBoneBody();
			}
			return cachedBody;
		}
	}

	private void OnEnable()
	{
		FatalityRocket.OnExplode += FatalityRocket_OnExplode;
	}

	private void OnDisable()
	{
		FatalityRocket.OnExplode -= FatalityRocket_OnExplode;
	}

	public virtual void StartFatalityBrick(GameObject bricksParent)
	{
		StartCoroutine(CreateBricks(bricksParent));
	}

	private void CreateBrick(GameObject brickParent)
	{
		float min = brickSpawnPoint.x - brickLeftBorder;
		float max = brickSpawnPoint.x + brickRightBorder;
		Vector2 vector = brickSpawnPoint;
		vector.x = Random.Range(min, max);
		FatalityBrick fatalityBrick = Object.Instantiate(brickPrefab);
		fatalityBrick.transform.parent = brickParent.transform;
		fatalityBrick.transform.position = CachedBody.transform.position + (Vector3)vector;
		fatalityBrick.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f + 90f);
		Rigidbody componentInChildren = fatalityBrick.GetComponentInChildren<Rigidbody>();
		if ((bool)componentInChildren)
		{
			float num = 0f;
			float x = 0f;
			float num2 = brickStartVerticalSpeed;
			if (!isBricksVerticalSpeedNeeded && !Mathf.Approximately(Physics.gravity.y, 0f))
			{
				float num3 = Mathf.Pow(num2, 2f) - 2f * Physics.gravity.y * vector.y;
				if (num3 >= 0f)
				{
					float num4 = (0f - num2 + Mathf.Sqrt(num3)) / Physics.gravity.y;
					float num5 = (0f - num2 - Mathf.Sqrt(num3)) / Physics.gravity.y;
					if (num4 > 0f)
					{
						num = ((!(num5 > 0f) || !(num4 > num5)) ? num4 : num5);
					}
					else if (num5 > 0f)
					{
						num = num5;
					}
					x = (0f - vector.x) / num;
				}
			}
			componentInChildren.velocity = new Vector2(x, num2);
		}
		if (!string.IsNullOrEmpty(soundName))
		{
			fatalityBrick.gameObject.AddComponent<ObserverPhysics>();
			fatalityBrick.GetComponent<ObserverPhysics>().EventCollisionEnter = OnBrickCollision;
		}
		bricks.Add(fatalityBrick);
	}

	private void FatalityRocket_OnExplode()
	{
		if (!isExplosionAvailable)
		{
			return;
		}
		isExplosionAvailable = false;
		for (int i = 0; i < bricks.Count; i++)
		{
			if (bricks[i] != null && bricks[i] is FatalityRocket)
			{
				(bricks[i] as FatalityRocket).ExplodeRocket();
			}
		}
	}

	private void OnBrickCollision(Collision coll)
	{
		if (!string.IsNullOrEmpty(soundName) && !isCollided)
		{
			isCollided = true;
			AudioManager.PlayAudioWithKey(soundName);
		}
	}

	private IEnumerator CreateBricks(GameObject bricksParent)
	{
		yield return new WaitForSeconds(brickDelayBeforeStart);
		for (int i = 0; i < numberOfBricks; i++)
		{
			CreateBrick(bricksParent);
			yield return new WaitForSeconds(delayBetweenBricks);
		}
	}
}
