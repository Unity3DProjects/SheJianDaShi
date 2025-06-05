using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Spine;
using UnityEngine;

public class FatalityFlagUpgrade : FatalitySatelite
{
	private const float CHARACTER_GROUND_DOWN_MOVEMENT = 5000f;

	private const string CHARACTER_FLAG_EVENT_NAME = "Flag";

	private const string CHARACTER_FLAG_ANIMATION_NAME = "fatality_flag_loop";

	[SerializeField]
	[ResourceLink]
	private AssetLink groundEffectPrefab;

	[SerializeField]
	private int groundEffectsCount;

	[SerializeField]
	[ResourceLink]
	private AssetLink bloodEffectPrefab;

	[SerializeField]
	private float characterTweenerDuration;

	[SerializeField]
	private float moonStartShakeAmplitude;

	[SerializeField]
	private float moonMovementDuration;

	[SerializeField]
	private float movementGroundEffectsDuration;

	[SerializeField]
	private Vector2 moonVelocityDeviders;

	[SerializeField]
	private float brutalityEffectDelay;

	private Level currentLevel;

	private GameObject bloodEffect;

	private GameObject groundEffect;

	private float currentMoonMovementDuration;

	private float currentMovementGroundEffectDuration;

	private Moon moon;

	private void Awake()
	{
		currentLevel = SingletonMonoBehaviour<Managers>.Instance.CachedLevelsManager.CurrentLevel;
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		base.StartFatality(infoCollision);
		ShowBrutalityEffect(brutalityEffectDelay);
		currentLevel.ActiveCharacter.SkeletonAnimation.State.Event += CurrentLevel_ActiveArcher_SkeletonAnimation_State_Event;
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (!base.IsActive || !(currentMoonMovementDuration > 0f) || !(moon != null))
		{
			return;
		}
		currentMoonMovementDuration -= deltaTime;
		moon.UpdateMoon(deltaTime, currentMoonMovementDuration / moonMovementDuration);
		if (currentMovementGroundEffectDuration <= 0f)
		{
			if (groundEffectPrefab != null && !string.IsNullOrEmpty(groundEffectPrefab.assetGUID))
			{
				groundEffect = (GameObject)groundEffectPrefab.GetInstance();
				groundEffect.transform.parent = currentLevel.FXsTransform;
				float cornerPositionX = moon.CornerPositionX;
				groundEffect.transform.position = currentLevel.Location.GetPositionOnFloor(cornerPositionX);
			}
			currentMovementGroundEffectDuration = movementGroundEffectsDuration;
		}
		else
		{
			currentMovementGroundEffectDuration -= deltaTime;
		}
	}

	protected override void GroundCollided(Collider collider)
	{
		collider.enabled = false;
		if (groundEffectPrefab != null && !string.IsNullOrEmpty(groundEffectPrefab.assetGUID))
		{
			for (int i = 0; i < groundEffectsCount; i++)
			{
				groundEffect = (GameObject)groundEffectPrefab.GetInstance();
				GroundEffect component = groundEffect.GetComponent<GroundEffect>();
				groundEffect.transform.parent = currentLevel.FXsTransform;
				float position = currentSatelite.transform.position.x + (float)(i - groundEffectsCount / 2) * component.EffectLength;
				groundEffect.transform.position = currentLevel.Location.GetPositionOnFloor(position);
			}
		}
		if (bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
		{
			bloodEffect = (GameObject)bloodEffectPrefab.GetInstance();
			bloodEffect.transform.parent = currentLevel.FXsTransform;
			bloodEffect.transform.position = currentLevel.Location.GetPositionOnFloor(currentSatelite.transform.position.x);
		}
		base.GroundCollided(collider);
		currentMoonMovementDuration = moonMovementDuration;
	}

	protected override void CharacterCollided(Collider collider, bool isSoundNeed = true)
	{
		base.CachedEffect.EmitGUICameraEffects(BrutalityEffectType.Finished);
		Character character = collider.GetComponentInParent<Character>();
		if (character != null)
		{
			character.RemoveAllDriveIntoWeapon();
			for (int i = 0; i < character.PartsColliders.Count; i++)
			{
				if (character.PartsColliders[i] != null)
				{
					character.PartsColliders[i].gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
				}
			}
			SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsNeedFollowArcherOrWeapon = false;
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.SetTarget(currentSatelite.transform, Vector2.zero, Vector2.zero);
			TweenPosition tweenPosition = character.gameObject.AddComponent<TweenPosition>();
			tweenPosition.beginPosition = collider.transform.position;
			tweenPosition.endPosition = collider.transform.position + Vector3.down * 5000f;
			tweenPosition.duration = characterTweenerDuration;
			tweenPosition.SetOnFinishedDelegate(delegate
			{
				character.HeadRigidBody.useGravity = false;
				character.RootRigidBody.useGravity = false;
				for (int j = 0; j < character.RagdollRigidbodies.Length; j++)
				{
					character.RagdollRigidbodies[j].useGravity = false;
				}
			});
			tweenPosition.enabled = true;
		}
		base.CharacterCollided(collider, false);
		AudioManager.PlayAudioWithKey("flag_upgraded_moon_collision");
		if (moon != null)
		{
			moon.DestroyTrailEffect();
		}
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	private void CurrentLevel_ActiveArcher_SkeletonAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data.Name == "Flag")
		{
			currentLevel.ActiveCharacter.SkeletonAnimation.State.SetAnimation(7, "fatality_flag_loop", true);
			currentLevel.ActiveCharacter.SkeletonAnimation.State.Event -= CurrentLevel_ActiveArcher_SkeletonAnimation_State_Event;
		}
	}

	protected override IEnumerator CreateSatellite()
	{
		yield return _003CCreateSatellite_003E__BaseCallProxy0();
		Character cosmonaut = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.ActiveCharacter;
		cosmonaut.CachedTransform.parent = currentSatelite.GetComponent<Moon>().CosmonautGameObject;
		cosmonaut.CachedTransform.localPosition = Vector3.zero;
		cosmonaut.CachedTransform.localRotation = Quaternion.identity;
		cosmonaut.SkeletonAnimation.MeshRenderer.sortingLayerName = "Default";
		cosmonaut.SkeletonAnimation.MeshRenderer.sortingOrder = 10;
		currentSatelite.transform.localRotation = Quaternion.identity;
		moon = currentSatelite.GetComponent<Moon>();
		if (moon != null)
		{
			Rigidbody componentInChildren = currentSatelite.GetComponentInChildren<Rigidbody>();
			if (componentInChildren != null)
			{
				Vector3 velocity = componentInChildren.velocity;
				moon.Velocity = new Vector3(velocity.x / moonVelocityDeviders.x, velocity.y / moonVelocityDeviders.y, velocity.z);
			}
		}
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(10f, moonStartShakeAmplitude);
		AudioManager.PlayAudioWithKey("flag_upgraded_moon_flying");
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerator _003CCreateSatellite_003E__BaseCallProxy0()
	{
		return base.CreateSatellite();
	}
}
