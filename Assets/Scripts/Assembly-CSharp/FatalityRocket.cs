using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

public class FatalityRocket : FatalityBrick
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnExplode;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action m_OnShellCollide;

	[SerializeField]
	private float explodeForce;

	private bool isExplosionAvailable = true;

	public static event Action OnExplode
	{
		add
		{
			Action action = FatalityRocket.m_OnExplode;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref FatalityRocket.m_OnExplode, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = FatalityRocket.m_OnExplode;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref FatalityRocket.m_OnExplode, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action OnShellCollide
	{
		add
		{
			Action action = this.m_OnShellCollide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnShellCollide, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnShellCollide;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnShellCollide, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	protected override void Start()
	{
		base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles + Vector3.forward * 90f);
		if (brickTrailEffectPrefab != null && !string.IsNullOrEmpty(brickTrailEffectPrefab.assetGUID))
		{
			trail = (GameObject)brickTrailEffectPrefab.GetInstance();
			trail.transform.parent = base.transform;
			trail.transform.localPosition = Vector3.zero;
			trail.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		AudioManager.PlayAudioWithKey("rocket_activation");
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (trail != null)
		{
			UnityEngine.Object.Destroy(trail.gameObject);
			trail = null;
		}
		if (brickCollisionEffectPrefab != null && !string.IsNullOrEmpty(brickCollisionEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)brickCollisionEffectPrefab.GetInstance();
			gameObject.transform.parent = base.transform.parent;
			gameObject.transform.position = base.transform.position;
		}
		SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(collision.collider);
		AudioManager.StopAudioWithKey("rocket_activation");
		OnCollision(collision.collider, utilityBone);
		AudioManager.PlayAudioWithKey("rocket_explosion");
		this.m_OnShellCollide?.Invoke();
	}

	public void ExplodeRocket()
	{
		isExplosionAvailable = false;
		base.CachedManager.CharacterAnimation.state.SetAnimation(0, base.CachedManager.AnimationGuillotinePose, true);
	}

	protected void OnCollision(Collider partCollider, SkeletonUtilityBone utilityBone)
	{
		if (partCollider.GetComponentInParent<Character>() != null && isExplosionAvailable)
		{
			base.CachedManager.CurrentCharacter.StartDefaultAnimation();
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
			if ((bool)utilityBoneBody)
			{
				Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
				Rigidbody[] array = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
				if ((bool)rigidbody && array != null && array.Length > 0)
				{
					rigidbody.velocity = new Vector3(0f, explodeForce, 0f);
					rigidbody.constraints = (RigidbodyConstraints)56;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != rigidbody)
						{
							Quaternion quaternion = Quaternion.LookRotation(array[i].transform.position - rigidbody.transform.position, Vector3.forward);
							array[i].velocity = quaternion * new Vector3(explodeForce, 0f, 0f);
							array[i].constraints = RigidbodyConstraints.FreezeRotationX;
							if (utilityBoneBody.bone.children.Exists((Bone bone) => bone.Data.Name == array[i].name))
							{
								base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, utilityBoneBody.transform, array[i].transform.position, new Vector3(0f, 0f, Mathf.Atan2(array[i].transform.position.y - rigidbody.transform.position.y, array[i].transform.position.x - rigidbody.transform.position.x) * 57.29578f));
							}
						}
						BreakAllJoint(array[i]);
					}
				}
			}
			List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>();
			list.Add(base.CachedManager.GetUtilityBoneBody());
			list.Add(base.CachedManager.GetUtilityBoneHead());
			if (list.Count > 0)
			{
				foreach (SkeletonUtilityBone item in list)
				{
					if (item == base.CachedHead)
					{
						base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, item.transform.position, item.transform.eulerAngles);
						base.CachedEffect.EmitBrain(base.transform, item.transform.position, Vector3.up);
					}
					else if (item == base.CachedBody)
					{
						base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, item.transform.position, item.transform.eulerAngles);
						base.CachedEffect.EmitGuts(base.transform, item.transform.position, Vector3.up);
					}
				}
			}
			base.CachedMask.Activate(partCollider, utilityBone);
			if (FatalityRocket.m_OnExplode != null)
			{
				FatalityRocket.m_OnExplode();
			}
		}
		base.gameObject.SetActive(false);
	}
}
