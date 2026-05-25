using Spine;
using UnityEngine;

public class FatalityPlasmaGrenade : FatalityBase
{
	private const string HEAD_BONE = "head";

	private const string LEFT_ARM_BONE = "arm_l";

	private const string RIGHT_ARM_BONE = "arm_r";

	private const string LEFT_FOREARM_BONE = "forearm_l";

	private const string RIGHT_FOREARM_BONE = "forearm_r";

	private const string LEFT_HAND_BONE = "hand_l";

	private const string RIGHT_HAND_BONE = "hand_r";

	private const string BODY_BONE = "body";

	private const string LEFT_THIGH_BONE = "thigh_l";

	private const string RIGHT_THIGH_BONE = "thigh_r";

	private const string LEFT_SHIN_BONE = "shin_l";

	private const string RIGHT_SHIN_BONE = "shin_r";

	[SerializeField]
	private float explodeCharacterForce;

	private SkeletonUtilityBone utility;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private Vector3 bodyPosition;

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

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.StartFatality(infoCollision);
			base.CachedManager.CharacterAnimation.state.SetAnimation(0, base.CachedManager.AnimationGuillotinePose, true);
			Explode();
			base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
		}
	}

	private void Explode()
	{
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		utility = base.CachedManager.GetUtilityBoneBody();
		if (!GoreContentFilter.AllowDismemberment)
		{
			if ((bool)utility)
			{
				Rigidbody rigidbodyForSafeRagdoll = base.CachedManager.CharacterRagdoll.GetRigidbody(utility.boneName);
				if ((bool)rigidbodyForSafeRagdoll)
				{
					rigidbodyForSafeRagdoll.velocity = new Vector3(explodeCharacterForce * 0.1f, explodeCharacterForce * 0.5f, 0f);
					rigidbodyForSafeRagdoll.constraints = (RigidbodyConstraints)56;
				}
			}
			return;
		}
		if ((bool)utility)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utility.boneName);
			bodyPosition = rigidbody.transform.position;
			Rigidbody[] array = new Rigidbody[1] { base.CachedManager.CharacterRagdoll.GetRigidbody("head") };
			Rigidbody[] array2 = new Rigidbody[3]
			{
				base.CachedManager.CharacterRagdoll.GetRigidbody("arm_l"),
				base.CachedManager.CharacterRagdoll.GetRigidbody("forearm_l"),
				base.CachedManager.CharacterRagdoll.GetRigidbody("hand_l")
			};
			Rigidbody[] array3 = new Rigidbody[3]
			{
				base.CachedManager.CharacterRagdoll.GetRigidbody("arm_r"),
				base.CachedManager.CharacterRagdoll.GetRigidbody("forearm_r"),
				base.CachedManager.CharacterRagdoll.GetRigidbody("hand_r")
			};
			Rigidbody[] array4 = new Rigidbody[2]
			{
				base.CachedManager.CharacterRagdoll.GetRigidbody("shin_l"),
				base.CachedManager.CharacterRagdoll.GetRigidbody("thigh_l")
			};
			Rigidbody[] array5 = new Rigidbody[2]
			{
				base.CachedManager.CharacterRagdoll.GetRigidbody("shin_r"),
				base.CachedManager.CharacterRagdoll.GetRigidbody("thigh_r")
			};
			BreakeParts(array, new Vector3(0.1f, 1f, 0f) * explodeCharacterForce);
			BreakeParts(array2, new Vector3(-1f, 1f, 0f) * explodeCharacterForce);
			BreakeParts(array3, new Vector3(1f, 1f, 0f) * explodeCharacterForce);
			FreezeParts(array4);
			FreezeParts(array5);
			if ((bool)rigidbody)
			{
				rigidbody.velocity = new Vector3(explodeCharacterForce * 0.1f, explodeCharacterForce * 0.5f, 0f);
				rigidbody.constraints = (RigidbodyConstraints)56;
			}
			BreakAllJoint(rigidbody);
			base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, CachedHead.transform.eulerAngles);
			base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
			base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
			base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, CachedBody.transform.eulerAngles);
		}
	}

	private void BreakeParts(Rigidbody[] array, Vector3 velocity)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				array[i].velocity = velocity;
				array[i].constraints = (RigidbodyConstraints)56;
				BreakAllJoint(array[i]);
				if (utility.bone.children.Exists((Bone bone) => bone.Data.Name == array[i].name))
				{
					base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, utility.transform, array[i].transform.position, new Vector3(0f, 0f, Mathf.Atan2(array[i].transform.position.y - bodyPosition.y, array[i].transform.position.x - bodyPosition.x) * 57.29578f));
				}
			}
		}
	}

	private void FreezeParts(Rigidbody[] array)
	{
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				array[i].constraints = RigidbodyConstraints.FreezeAll;
				array[i].gameObject.transform.parent = base.transform.parent;
			}
		}
	}
}
