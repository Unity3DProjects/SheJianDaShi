using System.Collections;
using UnityEngine;

public class FatalitySlingshot : FatalityBase
{
	private const string HEAD_BONE = "head";

	[SerializeField]
	private Locker lockerPrefab;

	[SerializeField]
	private float lockerAppearDelay = 2f;

	[SerializeField]
	private float lockerFallingHeight = 700f;

	[SerializeField]
	private float lockerFallDuration = 0.5f;

	[SerializeField]
	private Vector2 lockerOffset;

	[SerializeField]
	private float delayBeforeHeadSeparationNormal = 1f;

	private Locker locker;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			base.CachedManager.CurrentCharacter.SetAnimation(0, "finish_him", true);
			StartCoroutine(CoroutineAnimationFinished(lockerAppearDelay));
		}
	}

	public override void StopFatality()
	{
		locker.OnGroundStuck -= Locker_OnGroundStuck;
		locker.OnOpenDoor -= Locker_OnOpenDoor;
		base.StopFatality();
	}

	private IEnumerator CoroutineAnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		locker = Object.Instantiate(lockerPrefab, base.CachedManager.CurrentCharacter.HeadColliderBody.transform.position + new Vector3(lockerOffset.x, lockerOffset.y + lockerFallingHeight, 0f), Quaternion.identity, base.transform);
		locker.OnCharacterHit += Locker_OnCharacterHit;
		locker.OnGroundStuck += Locker_OnGroundStuck;
		locker.OnOpenDoor += Locker_OnOpenDoor;
		float multiplier = (base.CachedManager.CurrentCharacter.IsFlipped ? 1 : (-1));
		locker.SetTweenPositionParams(new Vector3(lockerOffset.x * multiplier, lockerOffset.y + lockerFallingHeight, 0f), new Vector3(lockerOffset.x * multiplier, lockerOffset.y, 0f), lockerFallDuration);
	}

	private IEnumerator CoroutineOnOpenDoor()
	{
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		yield return new WaitForSeconds(delayBeforeHeadSeparationNormal);
		Rigidbody rgHead = base.CachedManager.CharacterRagdoll.GetRigidbody("head");
		if (rgHead != null)
		{
			if (rgHead.gameObject.GetComponent<MeshCollider>() != null)
			{
				SphereCollider sphereCollider = rgHead.gameObject.AddComponent<SphereCollider>();
				sphereCollider.radius = 35f;
				sphereCollider.center = new Vector3(base.CachedManager.CurrentCharacter.AppleHeightOffset / 2f, 0f, 0f);
				rgHead.gameObject.GetComponent<MeshCollider>().enabled = false;
				rgHead.velocity = ((!base.CachedManager.CurrentCharacter.IsFlipped) ? (Vector3.left * 100f) : (Vector3.right * 100f));
			}
			rgHead.constraints = (RigidbodyConstraints)56;
		}
		BreakAllJoint(rgHead);
	}

	private void Locker_OnCharacterHit()
	{
		base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, base.CachedManager.CurrentCharacter.transform, base.CachedManager.CurrentCharacter.HeadCollider.transform.position, Vector3.zero);
	}

	private void Locker_OnGroundStuck()
	{
		base.CachedManager.CharacterAnimation.state.SetAnimation(0, base.CachedManager.AnimationGuillotinePose, true);
		locker.OnCharacterHit -= Locker_OnCharacterHit;
	}

	private void Locker_OnOpenDoor()
	{
		StartCoroutine(CoroutineOnOpenDoor());
	}
}
