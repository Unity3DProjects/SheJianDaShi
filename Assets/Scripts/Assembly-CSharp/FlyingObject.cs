using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : LevelObject, ICollidable
{
	[SerializeField]
	protected float edgeWidth = 1800f;

	[SerializeField]
	protected float velocityXMin = 280f;

	[SerializeField]
	protected float velocityXMax = 400f;

	[SerializeField]
	protected float velocityIncreaseByWave = 100f;

	[SerializeField]
	protected float velocityLimit = 1000f;

	[SerializeField]
	protected float spotPositionXMin;

	[SerializeField]
	protected float spotPositionXMax;

	[SerializeField]
	protected float spotPositionYMin;

	[SerializeField]
	protected float spotPositionYMax;

	[SerializeField]
	protected float amplitudeMin;

	[SerializeField]
	protected float amplitudeMax;

	[SerializeField]
	protected float periodMin;

	[SerializeField]
	protected float periodMax;

	[SerializeField]
	protected int goldMin;

	[SerializeField]
	protected int goldMax;

	[SerializeField]
	protected Collider bodyCollider;

	protected float currentPeriod;

	protected float currentAmplitude;

	protected float flyTime;

	protected int currentGold;

	protected Vector2 startPosition;

	private LevelBirdsHunt level;

	private float speedMultiplier = 1f;

	private bool isFlying;

	private bool isFlipped;

	private float currentVelocityX;

	private Transform cachedTransform;

	private List<Collider> affectedObjects = new List<Collider>();

	internal LevelBirdsHunt Level
	{
		get
		{
			return level;
		}
	}

	internal Transform CachedTransform
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

	public bool IsFlipped
	{
		get
		{
			return isFlipped;
		}
		set
		{
			if (value != isFlipped)
			{
				isFlipped = value;
				CachedTransform.localEulerAngles = new Vector3(0f, isFlipped ? 180 : 0, 0f);
			}
		}
	}

	internal float VelocityX
	{
		get
		{
			return currentVelocityX;
		}
		set
		{
			currentVelocityX = value;
		}
	}

	public float SpeedMultiplier
	{
		get
		{
			return speedMultiplier;
		}
		set
		{
			if (value != speedMultiplier)
			{
				speedMultiplier = value;
			}
		}
	}

	public bool IsFlying
	{
		get
		{
			return isFlying;
		}
		set
		{
			isFlying = value;
		}
	}

	public virtual void Init(LevelBirdsHunt level)
	{
		this.level = level;
		speedMultiplier = 1f;
		isFlying = true;
		currentVelocityX = UnityEngine.Random.Range(velocityXMin, velocityXMax) + velocityIncreaseByWave * (float)level.WavesCount;
		currentVelocityX = Mathf.Min(currentVelocityX, velocityLimit);
		IsFlipped = UnityEngine.Random.value > 0.5f;
		startPosition = new Vector2(UnityEngine.Random.Range(spotPositionXMin, spotPositionXMax), UnityEngine.Random.Range(spotPositionYMin, spotPositionYMax));
		if (IsFlipped)
		{
			startPosition.x *= -1f;
		}
		startPosition += new Vector2(level.CharacterFirst.transform.position.x, 0f);
		CachedTransform.position = startPosition;
		currentPeriod = Mathf.Max(UnityEngine.Random.Range(periodMin, periodMax), 0.1f);
		currentAmplitude = UnityEngine.Random.Range(amplitudeMin, amplitudeMax);
		currentGold = UnityEngine.Random.Range(goldMin, goldMax);
		flyTime = 0f;
	}

	public virtual void Disabled()
	{
		isFlying = false;
	}

	public virtual void UpdateObject(float deltaTime)
	{
		float value = startPosition.y + currentAmplitude * Mathf.Sin((float)Math.PI * 2f * flyTime / currentPeriod);
		value = Mathf.Clamp(value, spotPositionYMin, spotPositionYMax);
		CachedTransform.position = new Vector3(CachedTransform.position.x, value, CachedTransform.position.z);
		CachedTransform.position += (IsFlipped ? 1 : (-1)) * new Vector3(currentVelocityX * deltaTime, 0f, 0f);
		if (IsOverTheEdge(CachedTransform.position.x))
		{
			OverTheEdge();
		}
	}

	public virtual void Remove()
	{
		Level.RemoveFlyObject(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public virtual void ApplyExlosionForce(ExplosionInfo explosionInfo)
	{
		DamageApplied(FlyingObjectDamageType.Fire, new Vector2(0f, 1f));
	}

	public virtual void DamageApplied(FlyingObjectDamageType type, Vector2 direction)
	{
	}

	public virtual bool OnCollision(CollisionInfo collisionInfo)
	{
		return true;
	}

	public virtual void ApplyDamage(CollisionInfo collisionInfo)
	{
	}

	public void ApplyForce(float force, ForceMode forceMode, CollisionInfo collisionInfo)
	{
	}

	public virtual bool IsAffectedInRadius(Vector3 position, float radius)
	{
		return AffectedObjects(position, radius).Count > 0;
	}

	public virtual List<Collider> AffectedObjects(Vector3 position, float radius)
	{
		affectedObjects.Clear();
		if (bodyCollider != null)
		{
			Vector2 a = bodyCollider.bounds.ClosestPoint(position);
			if (Vector2.Distance(a, position) <= radius || Vector2.Distance(bodyCollider.transform.position, position) <= radius)
			{
				affectedObjects.Add(bodyCollider);
			}
		}
		else
		{
			Debug.Log("bodyCollider is NULL");
		}
		return affectedObjects;
	}

	private bool IsOverTheEdge(float xPosition)
	{
		float x = level.CharacterFirst.transform.position.x;
		float num = x - edgeWidth;
		float num2 = x + edgeWidth;
		return xPosition < num || xPosition > num2;
	}

	private void OverTheEdge()
	{
		Disabled();
		Remove();
	}
}
