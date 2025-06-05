using System;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour, ICollidable
{
	[SerializeField]
	private FloorPartType type;

	private List<Vector3> floorHeights;

	public FloorPartType Type
	{
		get
		{
			return type;
		}
	}

	public List<Vector3> FloorHeights
	{
		get
		{
			if (floorHeights == null)
			{
				floorHeights = new List<Vector3>();
				tk2dSprite component = GetComponent<tk2dSprite>();
				float x = component.GetBounds().size.x;
				switch (type)
				{
				case FloorPartType.SkewRight:
				case FloorPartType.SkewLeft:
				case FloorPartType.SkewRightSmall:
				case FloorPartType.SkewLeftSmall:
				{
					float num = base.transform.position.x - component.GetBounds().size.x * 0.5f;
					floorHeights.Add(GroundPosition(num + x));
					floorHeights.Add(GroundPosition(num));
					break;
				}
				case FloorPartType.Straight:
					floorHeights.Add(GroundPosition(base.transform.position.x));
					break;
				}
			}
			return floorHeights;
		}
	}

	public bool IsWallPart { get; set; }

	public Vector3 GroundPosition(float position)
	{
		tk2dSprite component = GetComponent<tk2dSprite>();
		float x = component.GetBounds().size.x;
		float y = component.GetBounds().size.y;
		float y2 = base.transform.position.y + y;
		float num = base.transform.position.x - x * 0.5f;
		switch (type)
		{
		case FloorPartType.SkewLeftSmall:
		{
			float num3 = x / 3f;
			if (position <= num + num3)
			{
				y2 = base.transform.position.y + y * 0.5f;
			}
			else if (position <= num + num3 * 2f)
			{
				float num4 = num - position;
				y2 = base.transform.position.y - num4 * Mathf.Tan((float)Math.PI / 4f);
			}
			else
			{
				y2 = base.transform.position.y + y;
			}
			break;
		}
		case FloorPartType.SkewLeft:
			if (position <= num + x * 2f / 7f)
			{
				y2 = base.transform.position.y + y / 3f;
			}
			else if (position <= num + x * 5f / 7f)
			{
				float num7 = num - position;
				y2 = base.transform.position.y - num7 * Mathf.Tan((float)Math.PI / 4f);
			}
			else
			{
				y2 = base.transform.position.y + y;
			}
			break;
		case FloorPartType.SkewRightSmall:
		{
			float num5 = x / 3f;
			if (position <= num + num5)
			{
				y2 = base.transform.position.y + y;
			}
			else if (position <= num + num5 * 2f)
			{
				float num6 = position - num - x;
				y2 = base.transform.position.y - num6 * Mathf.Tan((float)Math.PI / 4f);
			}
			else
			{
				y2 = base.transform.position.y + y * 0.5f;
			}
			break;
		}
		case FloorPartType.SkewRight:
			if (position <= num + x * 2f / 7f)
			{
				y2 = base.transform.position.y + y;
			}
			else if (position <= num + x * 5f / 7f)
			{
				float num2 = position - num - x;
				y2 = base.transform.position.y - num2 * Mathf.Tan((float)Math.PI / 4f);
			}
			else
			{
				y2 = base.transform.position.y + y / 3f;
			}
			break;
		}
		return new Vector3(position, y2);
	}

	public Vector3 GetHightChangePosition()
	{
		Vector3 result = Vector3.zero;
		tk2dSprite component = GetComponent<tk2dSprite>();
		float x = component.GetBounds().size.x;
		switch (type)
		{
		case FloorPartType.SkewLeft:
		case FloorPartType.SkewLeftSmall:
		{
			float num2 = base.transform.position.x - component.GetBounds().size.x * 0.5f;
			result = new Vector3(num2 + component.GetBounds().size.x - x, base.transform.position.y + component.GetBounds().size.y);
			break;
		}
		case FloorPartType.SkewRight:
		case FloorPartType.SkewRightSmall:
		{
			float num = base.transform.position.x - component.GetBounds().size.x * 0.5f;
			result = new Vector3(num + x, base.transform.position.y + component.GetBounds().size.y);
			break;
		}
		}
		return result;
	}

	public bool OnCollision(CollisionInfo collisionInfo)
	{
		return true;
	}

	public void ApplyDamage(CollisionInfo collisionInfo)
	{
	}

	public void ApplyForce(float force, ForceMode forceMode, CollisionInfo collisionInfo)
	{
	}

	public bool IsAffectedInRadius(Vector3 position, float radius)
	{
		return false;
	}

	public List<Collider> AffectedObjects(Vector3 position, float radius)
	{
		return null;
	}
}
