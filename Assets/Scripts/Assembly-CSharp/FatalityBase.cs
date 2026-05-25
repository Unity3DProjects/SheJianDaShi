using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class FatalityBase : MonoBehaviour
{
	public Action<FatalityBase> OnFatalityFinish;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float> m_OnBrutalityShowed;

	[SerializeField]
	private WeaponType typeWeapon;

	[SerializeField]
	private FatalityActivationType activationType = FatalityActivationType.AnyType;

	[SerializeField]
	protected float time;

	[SerializeField]
	protected float explodeForce;

	protected FatalityManager cachedManager;

	protected FatalityEffect cachedEffect;

	protected FatalityMask cachedMask;

	private bool isActive;

	protected float currentTime;

	private Spine.Animation currentAnimation;

	protected Collider startFatalityCollider;

	protected SkeletonUtilityBone startFatalityUtilityBone;

	private bool isBrutalityShowed;

	public WeaponType TypeWeapon
	{
		get
		{
			return typeWeapon;
		}
	}

	public FatalityActivationType ActivationType
	{
		get
		{
			return activationType;
		}
	}

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}

	public FatalityManager CachedManager
	{
		get
		{
			if (cachedManager == null)
			{
				cachedManager = GetComponentInParent<FatalityManager>();
			}
			return cachedManager;
		}
	}

	public FatalityEffect CachedEffect
	{
		get
		{
			if (cachedEffect == null)
			{
				cachedEffect = GetComponent<FatalityEffect>();
			}
			return cachedEffect;
		}
	}

	public FatalityMask CachedMask
	{
		get
		{
			if (cachedMask == null)
			{
				cachedMask = GetComponent<FatalityMask>();
			}
			return cachedMask;
		}
	}

	public static event Action<float> OnBrutalityShowed
	{
		add
		{
			Action<float> action = FatalityBase.m_OnBrutalityShowed;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref FatalityBase.m_OnBrutalityShowed, (Action<float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float> action = FatalityBase.m_OnBrutalityShowed;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref FatalityBase.m_OnBrutalityShowed, (Action<float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public virtual void InitFatality(FatalityManager manager)
	{
		cachedManager = manager;
	}

	public virtual void StartFatality(CollisionInfo infoCollision)
	{
		if (!isActive)
		{
			isActive = true;
			currentTime = time;
			CachedManager.CharacterAnimation.state.SetAnimation(0, CachedManager.AnimationFace, true);
			startFatalityCollider = infoCollision.otherCollider;
			startFatalityUtilityBone = CachedManager.GetUtilityBone(startFatalityCollider);
			PlayFatalityStartSound();
		}
	}

	public virtual void UpdateFatality(float deltaTime)
	{
		if (!isActive)
		{
			return;
		}
		currentTime -= deltaTime;
		if (currentTime < 0f)
		{
			isActive = false;
			if (OnFatalityFinish != null)
			{
				OnFatalityFinish(this);
			}
		}
	}

	public virtual void StopFatality()
	{
		if (isActive)
		{
			isActive = false;
		}
	}

	protected virtual void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		CachedManager.CurrentCharacter.StartDefaultAnimation();
		CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if (!GoreContentFilter.AllowDismemberment)
		{
			SkeletonUtilityBone utilityBoneBodyForSafeRagdoll = CachedManager.GetUtilityBoneBody();
			if ((bool)utilityBoneBodyForSafeRagdoll)
			{
				Rigidbody rigidbodyForSafeRagdoll = CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBodyForSafeRagdoll.boneName);
				if ((bool)rigidbodyForSafeRagdoll)
				{
					rigidbodyForSafeRagdoll.velocity = new Vector3(0f, explodeForce * 0.25f, 0f);
					rigidbodyForSafeRagdoll.constraints = (RigidbodyConstraints)56;
				}
			}
			return;
		}
		SkeletonUtilityBone utilityBoneBody = CachedManager.GetUtilityBoneBody();
		if (!utilityBoneBody)
		{
			return;
		}
		Rigidbody rigidbody = CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
		Rigidbody[] array = CachedManager.CharacterRagdoll.GetRigidbodyArray();
		if (!rigidbody || array == null || array.Length <= 0)
		{
			return;
		}
		rigidbody.velocity = new Vector3(0f, explodeForce, 0f);
		rigidbody.constraints = (RigidbodyConstraints)56;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != rigidbody && array[i] != null)
			{
				Quaternion quaternion = Quaternion.LookRotation(array[i].transform.position - rigidbody.transform.position, Vector3.forward);
				array[i].velocity = quaternion * new Vector3(explodeForce, 0f, 0f);
				array[i].constraints = (RigidbodyConstraints)56;
				if (isNeedBloodEffects && utilityBoneBody.bone.children.Exists((Bone bone) => bone.Data.Name == array[i].name))
				{
					CachedEffect.EmitBloodEffect(CachedManager.CurrentCharacter.CharacterType, utilityBoneBody.transform, array[i].transform.position, new Vector3(0f, 0f, Mathf.Atan2(array[i].transform.position.y - rigidbody.transform.position.y, array[i].transform.position.x - rigidbody.transform.position.x) * 57.29578f));
				}
			}
			BreakSelfJoint(array[i]);
		}
	}

	protected virtual void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
		PlayCrowdHappySound();
	}

	protected virtual void PlayCrowdHappySound()
	{
		Character character = CachedManager.CurrentCharacter as Character;
		if (character.IsAI || character.IsMultiplayerEnemy || SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType == GameType.VSHuman)
		{
			AudioManager.PlayAudioWithKey("crowd_glee");
			AudioManager.PlayAudioWithKey("crowd_loop");
		}
	}

	protected virtual void BreakAllJoint(Rigidbody rgBody)
	{
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		if ((bool)rgBody)
		{
			BreakSelfJoint(rgBody);
			BreakRelativeJoint(rgBody);
		}
	}

	protected void BreakSelfJoint(Rigidbody rgBody)
	{
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		if ((bool)rgBody)
		{
			HingeJoint component = rgBody.GetComponent<HingeJoint>();
			if ((bool)component)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	protected void BreakRelativeJoint(Rigidbody rgBody)
	{
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		if (!rgBody || !CachedManager.CharacterRagdoll.IsActive)
		{
			return;
		}
		Rigidbody[] rigidbodyArray = CachedManager.CharacterRagdoll.GetRigidbodyArray();
		if (rigidbodyArray == null)
		{
			return;
		}
		for (int i = 0; i < rigidbodyArray.Length; i++)
		{
			HingeJoint component = rigidbodyArray[i].GetComponent<HingeJoint>();
			if (component != null && component.connectedBody == rgBody)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	protected HingeJoint GetRelativeJoint(Rigidbody rgBody)
	{
		HingeJoint result = null;
		if ((bool)rgBody && CachedManager.CharacterRagdoll.IsActive)
		{
			Rigidbody[] rigidbodyArray = CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if (rigidbodyArray != null)
			{
				for (int i = 0; i < rigidbodyArray.Length; i++)
				{
					HingeJoint component = rigidbodyArray[i].GetComponent<HingeJoint>();
					if (component != null && component.connectedBody == rgBody)
					{
						result = component;
						break;
					}
				}
			}
		}
		return result;
	}

	protected void StartAnimation(Spine.Animation anim)
	{
		if (currentAnimation == null && anim != null)
		{
			CachedManager.CharacterAnimation.state.Event += CurrentManager_CharacterAnimation_state_Event;
			CachedManager.CharacterAnimation.state.SetAnimation(0, anim, false);
			currentAnimation = anim;
		}
	}

	protected void StopAnimation(Spine.Animation anim)
	{
		if (anim == currentAnimation)
		{
			CachedManager.CharacterAnimation.state.Event -= CurrentManager_CharacterAnimation_state_Event;
			currentAnimation = null;
		}
	}

	protected virtual void AnimationEnd(Spine.Animation anim)
	{
		MeshRenderer component = CachedManager.CharacterAnimation.GetComponent<MeshRenderer>();
		component.enabled = false;
	}

	protected void ShowBrutalityEffect(float delay)
	{
		if (!isBrutalityShowed && FatalityBase.m_OnBrutalityShowed != null)
		{
			isBrutalityShowed = true;
			FatalityBase.m_OnBrutalityShowed(delay);
		}
	}

	private void CurrentManager_CharacterAnimation_state_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedManager.AnimationDeathEnd)
		{
			AnimationEnd(currentAnimation);
		}
	}
}
