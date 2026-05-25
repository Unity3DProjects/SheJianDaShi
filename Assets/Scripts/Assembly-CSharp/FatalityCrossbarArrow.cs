using System.Collections;
using Spine;
using UnityEngine;

public class FatalityCrossbarArrow : FatalityBase
{
	private const string FATALITY = "fatality";

	private const string FATALITY_APPEAR = "fatality_appear";

	private const string FATALITY_FIRST_HIT_EVENT_NAME = "Fatallity_Down";

	private const string FATALITY_SWORD_HIT_EVENT_NAME = "Fatallity_Sword_Hit";

	private const float HANGING_SPEED = 10f;

	[SerializeField]
	private float delayBeforeStart;

	[SerializeField]
	private float breakUpDelay;

	[SerializeField]
	private Vector3 throwUpDirection;

	[SerializeField]
	[ResourceLink]
	private AssetLink smokeEffectPrefab;

	[SerializeField]
	private string firstHitSound;

	[SerializeField]
	private string swordHitSound;

	private Character witcher;

	private Spine.Animation firstHitAnimation;

	private Spine.Animation mainAnimation;

	private EventData cachedEventEarthShake;

	private EventData cachedSwordHitEvent;

	private bool isCharacterFly;

	private bool isHanging;

	private Character Witcher
	{
		get
		{
			if (witcher == null)
			{
				witcher = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter;
			}
			return witcher;
		}
	}

	private EventData CachedEventEarthShake
	{
		get
		{
			if (cachedEventEarthShake == null)
			{
				cachedEventEarthShake = Witcher.SkeletonAnimation.Skeleton.Data.FindEvent("Fatallity_Down");
			}
			return cachedEventEarthShake;
		}
	}

	private EventData CachedSwordHitEvent
	{
		get
		{
			if (cachedSwordHitEvent == null)
			{
				cachedSwordHitEvent = Witcher.SkeletonAnimation.Skeleton.Data.FindEvent("Fatallity_Sword_Hit");
			}
			return cachedSwordHitEvent;
		}
	}

	private Spine.Animation FirstHitAnimation
	{
		get
		{
			if (firstHitAnimation == null)
			{
				firstHitAnimation = Witcher.SkeletonAnimation.Skeleton.Data.FindAnimation("fatality_appear");
			}
			return firstHitAnimation;
		}
	}

	private Spine.Animation MainAnimation
	{
		get
		{
			if (mainAnimation == null)
			{
				mainAnimation = Witcher.SkeletonAnimation.Skeleton.Data.FindAnimation("fatality");
			}
			return mainAnimation;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		base.StartFatality(infoCollision);
		base.CachedManager.CurrentCharacter.SetAnimation(0, "finish_him", true);
		StartCoroutine(StartFirstHitAnimation());
	}

	public override void StopFatality()
	{
		base.StopFatality();
		Witcher.SkeletonAnimation.state.Event -= Witcher_SkeletonAnimation_state_Event;
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (!isCharacterFly || isHanging || !(base.CachedManager.CurrentCharacter.RootRigidBody.velocity.y < 10f))
		{
			return;
		}
		isHanging = true;
		Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
		Rigidbody[] array = rigidbodyArray;
		foreach (Rigidbody rigidbody in array)
		{
			if (rigidbody != null)
			{
				rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}
		}
		StartCoroutine(StartWitcherMainAnimation());
	}

	private void ThrowUpCharacter()
	{
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
		Rigidbody[] array = rigidbodyArray;
		foreach (Rigidbody rigidbody in array)
		{
			if (rigidbody != null)
			{
				rigidbody.velocity = throwUpDirection;
			}
		}
		isCharacterFly = true;
	}

	private void BreakUpCharacter()
	{
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Rigidbody[] array = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if ((bool)rigidbody && array != null && array.Length > 0)
			{
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.forward * 30f;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != rigidbody && array[i] != null)
					{
						Quaternion quaternion = Quaternion.LookRotation(array[i].transform.position - rigidbody.transform.position, Vector3.forward);
						array[i].velocity = quaternion * Vector3.left * 600f;
						array[i].constraints = RigidbodyConstraints.FreezeRotationX;
					}
					if (array[i] != null)
					{
						array[i].constraints = (RigidbodyConstraints)56;
					}
					if (utilityBoneBody.bone.children.Exists((Bone bone) => bone.Data.Name == array[i].name))
					{
						base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, utilityBoneBody.transform, array[i].transform.position, new Vector3(0f, 0f, Mathf.Atan2(array[i].transform.position.y - rigidbody.transform.position.y, array[i].transform.position.x - rigidbody.transform.position.x) * 57.29578f));
					}
					BreakAllJoint(array[i]);
				}
			}
		}
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
	}

	private IEnumerator StartFirstHitAnimation()
	{
		yield return new WaitForSeconds(delayBeforeStart);
		Witcher.transform.position = base.CachedManager.CurrentCharacter.transform.position;
		Witcher.SkeletonAnimation.state.Event += Witcher_SkeletonAnimation_state_Event;
		Witcher.SkeletonAnimation.State.SetAnimation(0, FirstHitAnimation, false);
	}

	private IEnumerator StartWitcherMainAnimation()
	{
		Witcher.transform.position = base.CachedManager.CurrentCharacter.transform.position;
		Witcher.SkeletonAnimation.State.ClearTracks();
		Witcher.SkeletonAnimation.State.SetAnimation(0, MainAnimation, false);
		yield return new WaitForSeconds(MainAnimation.Duration + breakUpDelay);
		BreakUpCharacter();
		Witcher.SkeletonAnimation.state.ClearTracks();
		Witcher.SetAnimation(0, "win", true);
	}

	private void Witcher_SkeletonAnimation_state_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventEarthShake)
		{
			if (smokeEffectPrefab != null && !string.IsNullOrEmpty(smokeEffectPrefab.assetGUID))
			{
				Object.Instantiate((GameObject)smokeEffectPrefab.GetInstance(), base.CachedManager.CurrentCharacter.transform.position, Quaternion.identity, base.transform);
			}
			ThrowUpCharacter();
		}
		if (e.Data != CachedSwordHitEvent)
		{
		}
	}
}
