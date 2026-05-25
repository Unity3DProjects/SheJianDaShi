using System.Collections;
using Spine;
using UnityEngine;

public class FatalitySkateboard : FatalityBase
{
	private const string ANIMATION_NAME = "fatality_skateboard_final";

	private const string SKATE_HIT = "Skate_Hit";

	private const string FOOTBALL_KICK = "Football_Kick";

	[SerializeField]
	private float delayBeforeStart;

	[SerializeField]
	private float checkingGroundDistance;

	[SerializeField]
	private float burialDepth;

	[SerializeField]
	private float oneKickBuringDuration;

	[SerializeField]
	private Vector2 headDirection;

	private Character skater;

	private EventData cachedSkateHitEvent;

	private EventData cachedFootballKickEvent;

	private TweenPosition tweenPosition;

	private bool isRight;

	private Vector3 currentPosition;

	private EventData CachedSkateHitEvent
	{
		get
		{
			if (cachedSkateHitEvent == null)
			{
				cachedSkateHitEvent = skater.SkeletonAnimation.Skeleton.Data.FindEvent("Skate_Hit");
			}
			return cachedSkateHitEvent;
		}
	}

	private EventData CachedFootballKickEvent
	{
		get
		{
			if (cachedFootballKickEvent == null)
			{
				cachedFootballKickEvent = skater.SkeletonAnimation.Skeleton.Data.FindEvent("Football_Kick");
			}
			return cachedFootballKickEvent;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		base.StartFatality(infoCollision);
		skater = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter;
		base.CachedManager.CurrentCharacter.SetAnimation(0, "finish_him", true);
		StartCoroutine(StartFatalityAnimation());
		isRight = IsRightSide();
		if (isRight)
		{
			Vector3 localScale = skater.transform.localScale;
			localScale.x = -1f;
			skater.transform.localScale = localScale;
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		skater.SkeletonAnimation.state.Event -= Skater_SkeletonAnimation_state_Event;
	}

	private void BuryInGround()
	{
		tweenPosition.ResetTween();
		tweenPosition.beginPosition = currentPosition;
		tweenPosition.endPosition = currentPosition + Vector3.down * burialDepth;
		tweenPosition.duration = oneKickBuringDuration;
		tweenPosition.enabled = true;
		currentPosition = tweenPosition.endPosition;
	}

	private bool IsRightSide()
	{
		float y = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(base.CachedManager.CurrentCharacter.transform.position.x).y;
		float y2 = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(base.CachedManager.CurrentCharacter.transform.position.x - checkingGroundDistance).y;
		float y3 = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(base.CachedManager.CurrentCharacter.transform.position.x + checkingGroundDistance).y;
		if (y2 == y3)
		{
			return !IsSkaterLeft();
		}
		if (y == y3)
		{
			return true;
		}
		if (y == y2)
		{
			return false;
		}
		return y3 < y2;
	}

	private bool IsSkaterLeft()
	{
		return SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter.transform.position.x < base.CachedManager.CurrentCharacter.transform.position.x;
	}

	private void DoFreekick()
	{
		SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetComponent<Floor>().RemoveColliderAtX(base.CachedManager.CurrentCharacter.transform.position.x);
		MeshCollider[] componentsInChildren = skater.GetComponentsInChildren<MeshCollider>();
		MeshCollider[] array = componentsInChildren;
		foreach (MeshCollider meshCollider in array)
		{
			meshCollider.enabled = false;
		}
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		SkeletonUtilityBone utilityBoneHead = base.CachedManager.GetUtilityBoneHead();
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if (utilityBoneHead != null)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneHead.boneName);
			Rigidbody rigidbody2 = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Vector3 vector = new Vector3(utilityBoneHead.transform.position.x - utilityBoneBody.transform.position.x, utilityBoneHead.transform.position.y - utilityBoneBody.transform.position.y, 0f);
			Vector3 eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f);
			if (rigidbody != null)
			{
				Vector3 vector2 = ((!isRight) ? Vector3.left : Vector3.right);
				rigidbody.velocity = vector2 * headDirection.x + Vector3.up * headDirection.y;
				rigidbody.angularVelocity = new Vector3(0f, 0f, Random.Range(5f, 15f));
				rigidbody.maxAngularVelocity = 15f;
				rigidbody.constraints = (RigidbodyConstraints)56;
			}
			if (rigidbody2 != null)
			{
				rigidbody2.constraints = (RigidbodyConstraints)20;
			}
			base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, base.CachedManager.GetUtilityBone(utilityBoneHead.bone.Parent.Data.Name).transform, utilityBoneHead.transform.position, eulerAngles);
			BreakAllJoint(rigidbody);
		}
	}

	private IEnumerator StartFatalityAnimation()
	{
		yield return new WaitForSeconds(delayBeforeStart);
		skater.SkeletonAnimation.State.ClearTracks();
		skater.SkeletonAnimation.State.SetAnimation(0, "fatality_skateboard_final", false);
		skater.transform.position = base.CachedManager.CurrentCharacter.transform.position;
		skater.SkeletonAnimation.Update(0f);
		skater.SkeletonAnimation.LateUpdate();
		skater.SkeletonAnimation.state.Event += Skater_SkeletonAnimation_state_Event;
		tweenPosition = base.CachedManager.CurrentCharacter.gameObject.AddComponent<TweenPosition>();
		tweenPosition.beginPosition = base.CachedManager.CurrentCharacter.transform.localPosition;
		currentPosition = base.CachedManager.CurrentCharacter.transform.localPosition;
		yield return new WaitForSeconds(base.CachedManager.AnimationGuillotine.Duration);
	}

	private void Skater_SkeletonAnimation_state_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedSkateHitEvent)
		{
			BuryInGround();
		}
		if (e.Data == CachedFootballKickEvent)
		{
			DoFreekick();
		}
	}
}
