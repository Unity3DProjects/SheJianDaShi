using System.Collections;
using Spine;
using UnityEngine;

public class FatalityTrashCar : FatalityBase
{
	private const string ON_BIKE_ANIMATION = "fatality_loop";

	private const string ON_BIKE_PUNCH_ANIMATION = "fatality_punch";

	private const string IDLE_ANIMATION = "idle";

	private const string UNDER_CHARACTER_SORTING_LAYER = "FX_Weapon_Front";

	[SerializeField]
	private TrashCar bikePrefab;

	[SerializeField]
	private TrashCar bikeRightPrefab;

	[SerializeField]
	private float delayBike;

	[SerializeField]
	private float bikeSpeed;

	[SerializeField]
	private float startShakeAmplitude;

	[SerializeField]
	private float collisionShakeAmplitude;

	[SerializeField]
	private float collisionShakeDuration;

	[SerializeField]
	private float collisionShakeOneSideDuration;

	[SerializeField]
	private float leftXPosition;

	[SerializeField]
	private float explodeCharacterForce;

	[SerializeField]
	private float bikeSoundDelay;

	[SerializeField]
	private float brutalityEffectDelay;

	private Character character;

	private TrashCar trashCar;

	private float timer;

	private bool isBikeMove;

	private bool isRight;

	private bool isFatalityPlaying;

	private float xStartPosition;

	private float xFrontWheelCurrentPosition;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private Spine.Animation cachedFatalityRideAnimation;

	private Spine.Animation cachedFatalityStrikeAnimation;

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
		TrashCar.OnFrontCollide += TrashCar_OnFrontCollide;
		TrashCar.OnBackCollide += TrashCar_OnBackCollide;
		if (!isFatalityPlaying)
		{
			base.StartFatality(infoCollision);
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			isRight = base.CachedManager.CurrentCharacter.transform.position.x < 0f;
			StartCoroutine(StartBike());
			isFatalityPlaying = true;
			ShowBrutalityEffect(brutalityEffectDelay);
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (isBikeMove)
		{
			timer += deltaTime;
			if (isRight)
			{
				xFrontWheelCurrentPosition = xStartPosition - timer * bikeSpeed;
			}
			else
			{
				xFrontWheelCurrentPosition = xStartPosition + timer * bikeSpeed;
			}
			trashCar.SetBikePosition(xFrontWheelCurrentPosition, deltaTime);
		}
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		base.CachedEffect.EmitGUICameraEffects(BrutalityEffectType.Finished);
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
				rigidbody.velocity = Vector3.up;
				rigidbody.velocity *= explodeCharacterForce;
				rigidbody.angularVelocity = Vector3.up * 30f;
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

	private IEnumerator StartBike()
	{
		yield return new WaitForSeconds(bikeSoundDelay);
		AudioManager.PlayAudioWithKey("trashcan_upgraded_car_rolling");
		yield return new WaitForSeconds(delayBike - bikeSoundDelay);
		character = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter;
		character.SkeletonAnimation.State.ClearTracks();
		character.SkeletonAnimation.Reset();
		character.SkeletonAnimation.State.SetAnimation(0, "fatality_loop", false);
		if (isRight)
		{
			trashCar = Object.Instantiate(bikeRightPrefab, character.transform.position, Quaternion.identity, base.CachedManager.CurrentCharacter.transform.parent);
			trashCar.IsRight = true;
		}
		else
		{
			trashCar = Object.Instantiate(bikePrefab, character.transform.position, Quaternion.identity, base.CachedManager.CurrentCharacter.transform.parent);
		}
		trashCar.SetFatalityManager(base.CachedManager.CurrentCharacter.FatalityManager);
		isBikeMove = true;
		if (isRight)
		{
			xStartPosition = base.CachedManager.CurrentCharacter.BodyTransform.position.x + bikeSpeed / trashCar.AnimationRideDuration;
		}
		else
		{
			xStartPosition = base.CachedManager.CurrentCharacter.BodyTransform.position.x - bikeSpeed / trashCar.AnimationRideDuration;
		}
		character.transform.parent = trashCar.transform;
		character.transform.position = Vector3.zero;
		character.transform.localPosition = trashCar.SeatPlace;
		character.SkeletonAnimation.MeshRenderer.sortingLayerName = "FX_Weapon_Front";
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(10f, startShakeAmplitude);
	}

	private void TrashCar_OnBackCollide()
	{
		isBikeMove = false;
		TrashCar.OnBackCollide -= TrashCar_OnBackCollide;
	}

	private void TrashCar_OnFrontCollide()
	{
		ExplodeCharacter();
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(collisionShakeDuration, collisionShakeAmplitude, false, true, false, collisionShakeOneSideDuration);
		TrashCar.OnFrontCollide -= TrashCar_OnFrontCollide;
		character = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter;
		character.SkeletonAnimation.State.ClearTracks();
		character.SkeletonAnimation.Reset();
		character.SkeletonAnimation.State.SetAnimation(0, "fatality_punch", false);
		character.AddAnimation(0, "idle", true);
		AudioManager.PlayAudioWithKey("trashcan_upgraded_car_collision");
	}
}
