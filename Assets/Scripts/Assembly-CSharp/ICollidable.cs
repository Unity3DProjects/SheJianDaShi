using System.Collections.Generic;
using UnityEngine;

public interface ICollidable
{
	bool OnCollision(CollisionInfo collisionInfo);

	void ApplyDamage(CollisionInfo collisionInfo);

	void ApplyForce(float force, ForceMode forceMode, CollisionInfo collisionInfo);

	bool IsAffectedInRadius(Vector3 position, float radius);

	List<Collider> AffectedObjects(Vector3 position, float radius);
}
