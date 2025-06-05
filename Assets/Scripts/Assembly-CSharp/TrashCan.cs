using System.Collections;
using UnityEngine;

public class TrashCan : WeaponGeneral
{
	[SerializeField]
	private float secondsCameraMove = 1f;

	[SerializeField]
	private float weaponsEmissionPosition = 30f;

	private bool isFirstCollision;

	protected override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		if (IsGroundCollision(collision))
		{
			if (!isFirstCollision)
			{
				EmitObjects();
				RemoveAllActivations();
				StartCoroutine(CoroutineCameraMove(secondsCameraMove));
			}
			AudioManager.PlayAudioWithKey("trash_can_ground_hit_2");
		}
		isFirstCollision = true;
	}

	protected override void EmitObjects()
	{
		EmissionTrashCan component = base.gameObject.GetComponent<EmissionTrashCan>();
		if ((bool)component)
		{
			component.EmitObjects(level, base.CachedTransform.position + Vector3.up * weaponsEmissionPosition, Mathf.Atan2(base.CachedRigidbody.velocity.y, base.CachedRigidbody.velocity.x) * 57.29578f, base.CachedRigidbody.velocity.magnitude);
		}
	}

	private IEnumerator CoroutineCameraMove(float delay)
	{
		Character activeArcher = level.ActiveCharacter;
		yield return new WaitForSeconds(delay);
		if (activeArcher == level.ActiveCharacter)
		{
			level.UnfollowWeaponWithVelocity(Vector2.zero);
		}
	}

	protected bool IsGroundCollision(Collision coll)
	{
		ICollidable[] componentsInParent = coll.collider.gameObject.GetComponentsInParent<ICollidable>();
		ICollidable[] array = componentsInParent;
		foreach (ICollidable collidable in array)
		{
			if (collidable is Ground)
			{
				return true;
			}
		}
		return false;
	}
}
