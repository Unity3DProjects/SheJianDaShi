using System.Collections;
using Spine;
using UnityEngine;

public class FatalityShotgun : FatalityBase
{
	private const string ON_BIKE_ANIMATION = "fatality_archi_idle";

	private const string RELEASE_ANIMATION = "fatality_archi_action";

	private const string UNDER_CHARACTER_SORTING_LAYER = "FX_Weapon_Front";

	private const string BLOCK_RELEASE_NAME = "Block_Release";

	[SerializeField]
	private FatalityRocket terminatorBlockPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink blockEffectPrefab;

	[SerializeField]
	private Bike bikePrefab;

	[SerializeField]
	private Bike bikeRightPrefab;

	[SerializeField]
	private float delayBike;

	[SerializeField]
	private float bikeSpeed;

	[SerializeField]
	private float delayBeforeBlockRelease;

	[SerializeField]
	private float rideDuration;

	[SerializeField]
	private float leftXPosition;

	[SerializeField]
	private float blockReleaseForce;

	[SerializeField]
	private float explodeCharacterForce;

	[SerializeField]
	private float distanceToBlockRelease;

	[SerializeField]
	private Vector3 blockReleaseOffset;

	[SerializeField]
	private float bikeSoundDelay;

	private Character archi;

	private Bike bike;

	private FatalityRocket block;

	private float timer;

	private bool isBikeMove;

	private float xStartPosition;

	private float xFrontWheelCurrentPosition;

	private float distanceBetweenWheels;

	private bool isFatalityPlaying;

	private bool isArchiApproach;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private EventData cachedBlockReleaseEvent;

	private bool isRight;

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

	private EventData CachedBlockReleaseEvent
	{
		get
		{
			if (cachedBlockReleaseEvent == null)
			{
				cachedBlockReleaseEvent = archi.SkeletonAnimation.Skeleton.Data.FindEvent("Block_Release");
			}
			return cachedBlockReleaseEvent;
		}
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!isFatalityPlaying)
		{
			base.StartFatality(infoCollision);
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			isRight = base.CachedManager.CurrentCharacter.transform.position.x < 0f;
			StartCoroutine(StartBike());
			isFatalityPlaying = true;
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (isBikeMove)
		{
			timer += deltaTime;
			if (timer > rideDuration)
			{
				isBikeMove = false;
			}
			if (isRight)
			{
				xFrontWheelCurrentPosition = xStartPosition - timer * bikeSpeed;
			}
			else
			{
				xFrontWheelCurrentPosition = xStartPosition + timer * bikeSpeed;
			}
			bike.SetBikePosition(xFrontWheelCurrentPosition, deltaTime);
			if (!isArchiApproach && Mathf.Abs(archi.transform.position.x - base.CachedManager.CurrentCharacter.RootRigidBody.transform.position.x) < distanceToBlockRelease)
			{
				isArchiApproach = true;
				archi.SkeletonAnimation.State.ClearTracks();
				archi.SkeletonAnimation.Reset();
				archi.SkeletonAnimation.State.SetAnimation(0, "fatality_archi_action", false);
				archi.SkeletonAnimation.State.Event += Archi_SkeletonAnimation_State_Event;
			}
		}
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		base.CachedManager.CurrentCharacter.RemoveAllDriveIntoWeapon();
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		if (!GoreContentFilter.AllowDismemberment)
		{
			SkeletonUtilityBone utilityBoneBodyForSafeRagdoll = base.CachedManager.GetUtilityBoneBody();
			if ((bool)utilityBoneBodyForSafeRagdoll)
			{
				Rigidbody rigidbodyForSafeRagdoll = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBodyForSafeRagdoll.boneName);
				if ((bool)rigidbodyForSafeRagdoll)
				{
					rigidbodyForSafeRagdoll.velocity = Vector3.up * explodeCharacterForce * 0.25f;
					rigidbodyForSafeRagdoll.constraints = (RigidbodyConstraints)56;
				}
			}
			base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
			return;
		}
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if ((bool)rigidbody && rigidbodyArray != null && rigidbodyArray.Length > 0)
			{
				rigidbody.velocity = Vector3.up * explodeCharacterForce;
				rigidbody.angularVelocity = Vector3.forward * 30f;
				rigidbody.gameObject.GetComponent<MeshCollider>().enabled = true;
				for (int i = 0; i < rigidbodyArray.Length; i++)
				{
					if (rigidbodyArray[i] != rigidbody && rigidbodyArray[i] != null)
					{
						Quaternion quaternion = Quaternion.LookRotation(rigidbodyArray[i].transform.position - rigidbody.transform.position, Vector3.forward);
						rigidbodyArray[i].velocity = quaternion * new Vector3(explodeCharacterForce, 0f, 0f);
						rigidbodyArray[i].constraints = (RigidbodyConstraints)56;
					}
					if (rigidbodyArray[i] != null)
					{
						rigidbodyArray[i].constraints = (RigidbodyConstraints)56;
					}
					BreakAllJoint(rigidbodyArray[i]);
				}
			}
		}
		base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, Vector3.zero);
		base.CachedEffect.EmitBrain(base.transform, CachedHead.transform.position, Vector3.up);
		base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, Vector3.zero);
		base.CachedEffect.EmitGuts(base.transform, CachedBody.transform.position, Vector3.up);
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
	}

	public override void StopFatality()
	{
		base.StopFatality();
		base.PlayCrowdHappySound();
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	private void ReleaseWeapon()
	{
		if (terminatorBlockPrefab != null)
		{
			if (isRight)
			{
				blockReleaseOffset *= -1f;
			}
			block = Object.Instantiate(terminatorBlockPrefab, archi.WeaponAnchorTransform.position + blockReleaseOffset, Quaternion.identity, base.CachedManager.CurrentCharacter.FatalityManager.transform);
			if (blockEffectPrefab != null && !string.IsNullOrEmpty(blockEffectPrefab.assetGUID))
			{
				Object.Instantiate((GameObject)blockEffectPrefab.GetAsset(), block.transform.position, Quaternion.identity, block.transform);
			}
			block.OnShellCollide += Block_OnShellCollide;
			Rigidbody component = block.gameObject.GetComponent<Rigidbody>();
			component.velocity = Vector3.up * blockReleaseForce;
			component.angularVelocity = Vector3.forward * 300f;
			Collider blockCollider = block.gameObject.GetComponent<Collider>();
			blockCollider.enabled = false;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				blockCollider.enabled = true;
			}, 0.5f);
		}
	}

	private IEnumerator StartBike()
	{
		yield return new WaitForSeconds(delayBike);
		archi = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter;
		archi.SkeletonAnimation.State.ClearTracks();
		archi.SkeletonAnimation.Reset();
		archi.SetAnimation(0, "fatality_archi_idle", true, true);
		archi.SkeletonAnimation.AnimationName = "fatality_archi_idle";
		if (isRight)
		{
			bike = Object.Instantiate(bikeRightPrefab, archi.transform.position, Quaternion.identity, base.CachedManager.CurrentCharacter.transform.parent);
		}
		else
		{
			bike = Object.Instantiate(bikePrefab, archi.transform.position, Quaternion.identity, base.CachedManager.CurrentCharacter.transform.parent);
		}
		isBikeMove = true;
		if (isRight)
		{
			xStartPosition = base.CachedManager.CurrentCharacter.BodyTransform.position.x + leftXPosition;
		}
		else
		{
			xStartPosition = base.CachedManager.CurrentCharacter.BodyTransform.position.x - leftXPosition;
		}
		archi.transform.parent = bike.transform;
		archi.transform.position = bike.SeatPlace;
		archi.SkeletonAnimation.MeshRenderer.sortingLayerName = "FX_Weapon_Front";
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			AudioManager.PlayAudioWithKey("fatality_shotgun_bike_drive_sound");
		}, bikeSoundDelay);
	}

	private void Archi_SkeletonAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedBlockReleaseEvent)
		{
			ReleaseWeapon();
			archi.SkeletonAnimation.State.Event += Archi_SkeletonAnimation_State_Event;
		}
	}

	private void Block_OnShellCollide()
	{
		ExplodeCharacter();
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(4f, 10f);
		block.OnShellCollide -= Block_OnShellCollide;
	}
}
