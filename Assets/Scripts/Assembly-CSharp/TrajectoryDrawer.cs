using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
	[SerializeField]
	private float dotsTimeInterval = 0.05f;

	[SerializeField]
	private int minDotsCount;

	[SerializeField]
	private int maxDotsCount;

	[SerializeField]
	private float dotsMoveSpeed = 1f;

	[SerializeField]
	private tk2dSprite dotPrefab;

	private int dotsCount;

	private float moveTimeOffset;

	private Vector3 startPosition;

	private float angle;

	private float strength;

	private float timeOffset;

	private tk2dSprite[] dots;

	private LevelsManager cachedLevelsManager;

	protected tk2dSprite[] Dots
	{
		get
		{
			if (dots == null)
			{
				dots = new tk2dSprite[maxDotsCount];
			}
			return dots;
		}
	}

	public bool IsTutorialDrawer { get; set; }

	private LevelsManager CachedLevelsManager
	{
		get
		{
			if (cachedLevelsManager == null)
			{
				cachedLevelsManager = SingletonMonoBehaviour<LevelsManager>.Instance;
			}
			return cachedLevelsManager;
		}
	}

	public void Init()
	{
		for (int i = 0; i < maxDotsCount; i++)
		{
			Dots[i] = Object.Instantiate(dotPrefab);
			tk2dSprite tk2dSprite2 = Dots[i];
			tk2dSprite2.transform.parent = base.transform;
			tk2dSprite2.transform.position = new Vector3(50f * (float)i, 0f, 0f);
			tk2dSprite2.gameObject.SetActive(false);
		}
	}

	public void UpdateTrajectory(Vector3 startPosition, float angle, float strength, float ratio)
	{
		this.startPosition = startPosition;
		this.angle = angle;
		this.strength = strength;
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		float num = ratio * (float)(maxDotsCount - minDotsCount);
		dotsCount = minDotsCount + (int)num;
		timeOffset = num - Mathf.Floor(num);
	}

	public void SetTrajectoryActive(bool active)
	{
		base.gameObject.SetActive(active);
	}

	public void CustomUpdate(float deltaTime)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		moveTimeOffset += deltaTime * dotsMoveSpeed;
		if (moveTimeOffset >= 1f)
		{
			moveTimeOffset -= 1f;
		}
		float num = Mathf.Cos(angle);
		float num2 = Mathf.Sin(angle);
		for (int i = 0; i < maxDotsCount; i++)
		{
			tk2dSprite tk2dSprite2 = Dots[i];
			if (i < dotsCount)
			{
				float num3 = ((float)i + timeOffset + moveTimeOffset + 1f) * dotsTimeInterval;
				float x = startPosition.x + strength * num * num3;
				float gravity = CachedLevelsManager.CurrentLevel.CurrentWeapon.Gravity;
				float y = startPosition.y + strength * num2 * num3 - 0.5f * gravity * num3 * num3;
				tk2dSprite2.transform.position = new Vector3(x, y, 0f);
				Vector3 localPosition = tk2dSprite2.transform.localPosition;
				localPosition.z = 0f;
				tk2dSprite2.transform.localPosition = localPosition;
				float num4 = 1f - 0.03f * ((float)i + timeOffset + moveTimeOffset);
				tk2dSprite2.transform.localScale = new Vector3(num4, num4);
				if (IsTutorialDrawer)
				{
					tk2dSprite2.transform.localScale *= 1.2f;
				}
				tk2dSprite2.gameObject.SetActive(true);
			}
			else
			{
				tk2dSprite2.gameObject.SetActive(false);
			}
		}
	}

	public void PaintDots(Color color)
	{
		tk2dSprite[] array = Dots;
		foreach (tk2dSprite tk2dSprite2 in array)
		{
			tk2dSprite2.color = color;
		}
	}
}
