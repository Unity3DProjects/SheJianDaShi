using System.Collections;
using Spine;
using UnityEngine;

public class FatalityJokerGrenade : FatalityBase
{
	private const string MOUTH_BONE = "mouth";

	[SerializeField]
	private float timeMoveGrenade = 1f;

	[SerializeField]
	private float delayDropLegs;

	[SerializeField]
	private float explodeCharacterForce;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private SkeletonUtilityBone cachedMouth;

	private SkeletonUtilityBone[] cachedLegs;

	private bool isExplode;

	private bool isMoveGrenade;

	private bool isAnimation;

	private float currentTimeMoveGrenade;

	private JokerGrenade grenade;

	private Vector3 grenadeStartPosition;

	private Vector3 grenadeStartScale;

	protected SkeletonUtilityBone CachedHead
	{
		get
		{
			if (cachedHead == null)
			{
				cachedHead = base.CachedManager.GetUtilityBoneHead();
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
				cachedBody = base.CachedManager.GetUtilityBoneBody();
			}
			return cachedBody;
		}
	}

	private SkeletonUtilityBone CachedMouth
	{
		get
		{
			if (cachedMouth == null)
			{
				cachedMouth = base.CachedManager.GetUtilityBone("mouth");
			}
			return cachedMouth;
		}
	}

	private SkeletonUtilityBone[] CachedLegs
	{
		get
		{
			if (cachedLegs == null)
			{
				cachedLegs = base.CachedManager.GetUtilityBonesLegs();
			}
			return cachedLegs;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (base.IsActive)
		{
			return;
		}
		base.StartFatality(infoCollision);
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(infoCollision.otherCollider);
		if (utilityBone == CachedHead)
		{
			if (infoCollision.collider != null)
			{
				grenade = infoCollision.collider.GetComponentInParent<JokerGrenade>();
				if (grenade != null)
				{
					grenade.Disabled(true);
					grenade.DisableExplode = true;
					grenade.CachedTransform.parent = CachedMouth.CachedTransform;
					isMoveGrenade = true;
					currentTimeMoveGrenade = 0f;
					grenadeStartPosition = grenade.CachedTransform.localPosition;
					grenadeStartScale = grenade.CachedTransform.localScale;
					AudioManager.PlayAudioWithKey("joker_grenade_fatality_eat");
				}
				else
				{
					ExplodeCharacter();
				}
			}
			else
			{
				ExplodeCharacter();
			}
		}
		else
		{
			ExplodeCharacter();
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		if (!base.IsActive)
		{
			return;
		}
		if (isExplode)
		{
			base.UpdateFatality(deltaTime);
		}
		if (isMoveGrenade)
		{
			currentTimeMoveGrenade += deltaTime;
			if (currentTimeMoveGrenade < timeMoveGrenade)
			{
				float t = currentTimeMoveGrenade / timeMoveGrenade;
				grenade.CachedTransform.localPosition = Vector3.Lerp(grenadeStartPosition, Vector3.zero, t);
				grenade.CachedTransform.localScale = Vector3.Lerp(grenadeStartScale, Vector3.zero, t);
				return;
			}
			isAnimation = true;
			grenade.CachedTransform.localPosition = Vector3.zero;
			grenade.CachedTransform.localScale = Vector3.zero;
			isMoveGrenade = false;
			StartAnimation(base.CachedManager.AnimationGrenade);
			StartCoroutine(CoroutineAnimationFinished(base.CachedManager.AnimationGrenade.Duration - 0.2f));
			AudioManager.PlayAudioWithKey("joker_grenade_fatality_eat_finished");
			AudioManager.PlayAudioWithKey("fatality_pulse");
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		if (isAnimation)
		{
			StopAnimation(base.CachedManager.AnimationGrenade);
		}
	}

	protected override void PlayCrowdHappySound()
	{
	}

	protected override void ExplodeCharacter(bool withLegs = true)
	{
		base.PlayCrowdHappySound();
		AudioManager.PlayAudioWithKey("joker_grenade_fatality_explode");
		isExplode = true;
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if ((bool)CachedBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(CachedBody.boneName);
			Rigidbody[] array = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			Rigidbody[] array2 = null;
			if (!withLegs)
			{
				SkeletonUtilityBone[] utilityBonesLegs = base.CachedManager.GetUtilityBonesLegs();
				if (utilityBonesLegs != null)
				{
					array2 = new Rigidbody[utilityBonesLegs.Length];
					for (int i = 0; i < utilityBonesLegs.Length; i++)
					{
						array2[i] = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBonesLegs[i].boneName);
					}
				}
			}
			if ((bool)rigidbody && array != null && array.Length > 0)
			{
				if (withLegs)
				{
					rigidbody.velocity = new Vector3(0f, explodeCharacterForce, 0f);
				}
				rigidbody.constraints = (RigidbodyConstraints)56;
				BreakSelfJoint(rigidbody);
				for (int j = 0; j < array.Length; j++)
				{
					if (!(array[j] != rigidbody))
					{
						continue;
					}
					bool flag = true;
					if (!withLegs)
					{
						for (int k = 0; k < array2.Length; k++)
						{
							if (array2[k] == array[j])
							{
								flag = false;
								break;
							}
						}
					}
					if (!flag)
					{
						continue;
					}
					if (array[j] != null)
					{
						Quaternion quaternion = Quaternion.LookRotation(array[j].transform.position - rigidbody.transform.position, Vector3.forward);
						array[j].velocity = quaternion * new Vector3(explodeCharacterForce, 0f, 0f);
						array[j].constraints = (RigidbodyConstraints)56;
						if (CachedBody.bone.children.Exists((Bone bone) => bone.Data.Name == array[j].name))
						{
							Vector3 eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(array[j].transform.position.y - rigidbody.transform.position.y, array[j].transform.position.x - rigidbody.transform.position.x) * 57.29578f);
							base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, CachedBody.transform, array[j].transform.position, eulerAngles);
							base.CachedEffect.EmitAcidEffect(base.CachedManager.CurrentCharacter.CharacterType, CachedBody.transform, array[j].transform.position, eulerAngles);
						}
					}
					BreakSelfJoint(array[j]);
				}
				if (!withLegs)
				{
					base.CachedMask.Activate(startFatalityCollider, CachedBody);
					base.CachedEffect.EmitFireEffect(base.CachedManager.CurrentCharacter.CharacterType, rigidbody.transform, rigidbody.transform.position, Vector3.up);
					base.CachedEffect.EmitExplodeEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
				}
			}
		}
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, CachedHead.transform.eulerAngles);
		base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
		base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
	}

	private IEnumerator CoroutineAnimationFinished(float delay)
	{
		yield return new WaitForSeconds(delay);
		ExplodeCharacter(false);
		base.CachedManager.CharacterRagdoll.GetRigidbody(CachedBody.boneName).isKinematic = true;
		yield return new WaitForSeconds(delayDropLegs);
		base.CachedManager.CharacterRagdoll.GetRigidbody(CachedBody.boneName).isKinematic = false;
		isAnimation = false;
	}

	protected override void AnimationEnd(Spine.Animation anim)
	{
	}
}
