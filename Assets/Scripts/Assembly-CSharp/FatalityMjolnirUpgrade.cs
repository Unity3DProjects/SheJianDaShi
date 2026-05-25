using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityMjolnirUpgrade : FatalityBase
{
	private const string SHOCK_EFFECT = "shock";

	[SerializeField]
	[ResourceLink]
	protected AssetLink lightning;

	[SerializeField]
	private float delayBeforeStart = 2f;

	[SerializeField]
	private List<LightningParameters> lightningParameters;

	[SerializeField]
	private BoneParticleSystem[] flyingBoneSystems;

	[SerializeField]
	private BoneParticleSystem[] collidedBoneSystems;

	[SerializeField]
	private float brutalityEffectDelay;

	private List<GameObject> particleSystems = new List<GameObject>();

	private float lightningTime;

	private int currentLightningCount;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

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

	private void OnEnable()
	{
		BoneEffectClass.OnCollided += BoneEffectClass_OnCollided;
	}

	private void OnDisable()
	{
		BoneEffectClass.OnCollided -= BoneEffectClass_OnCollided;
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (!base.IsActive)
		{
			base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
			base.StartFatality(infoCollision);
			lightningTime = lightningParameters[currentLightningCount].delay;
			for (int i = 0; i < lightningParameters.Count; i++)
			{
				currentTime += lightningParameters[i].delay;
			}
			currentTime += delayBeforeStart;
			ShowBrutalityEffect(brutalityEffectDelay);
		}
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (!base.IsActive)
		{
			return;
		}
		lightningTime -= deltaTime;
		if (lightningTime <= 0f)
		{
			currentLightningCount++;
			if (currentLightningCount < lightningParameters.Count)
			{
				ApplyEfect();
				lightningTime = lightningParameters[currentLightningCount].delay;
			}
			else if (currentLightningCount == lightningParameters.Count)
			{
				ApplyEfect();
				DestroyCharacter();
			}
		}
	}

	public override void StopFatality()
	{
		foreach (GameObject particleSystem in particleSystems)
		{
			particleSystem.StopFX(true);
		}
		base.StopFatality();
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	private void ApplyEfect()
	{
		AudioManager.PlayAudioWithKey("mjolnir_upgraded_brutality_lightning");
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			rigidbody.AddForce(Vector3.up * lightningParameters[currentLightningCount - 1].jumpForce, ForceMode.VelocityChange);
		}
		if (lightning != null)
		{
			GameObject gameObject = Object.Instantiate((GameObject)lightning.GetAsset());
			gameObject.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.FXsTransform;
			gameObject.transform.position = base.CachedManager.CurrentCharacter.BodyTransform.position;
		}
		base.CachedManager.CurrentCharacter.StartElectricityEffect();
		base.CachedManager.CurrentCharacter.StopElectricityEffect(lightningParameters[currentLightningCount - 1].delay - 0.1f);
		base.CachedManager.CurrentCharacter.SkeletonAnimation.State.SetAnimation(0, "shock", false);
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(0.5f, lightningParameters[currentLightningCount - 1].shakeAmplitude);
	}

	private void DestroyCharacter()
	{
		base.CachedEffect.EmitGUICameraEffects(BrutalityEffectType.Finished);
		ApplyFatalityEffects();
		if (!GoreContentFilter.AllowDismemberment)
		{
			SkeletonUtilityBone utilityBoneBodyForSafeRagdoll = base.CachedManager.GetUtilityBoneBody();
			if ((bool)utilityBoneBodyForSafeRagdoll)
			{
				Rigidbody rigidbodyForSafeRagdoll = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBodyForSafeRagdoll.boneName);
				if ((bool)rigidbodyForSafeRagdoll)
				{
					rigidbodyForSafeRagdoll.velocity = Vector3.zero;
					rigidbodyForSafeRagdoll.angularVelocity = Vector3.zero;
					rigidbodyForSafeRagdoll.constraints = (RigidbodyConstraints)56;
				}
			}
			AudioManager.PlayAudioWithKey("green_arrow_brutality_explode");
			return;
		}
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Rigidbody[] array = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].velocity = Vector3.zero;
				array[i].angularVelocity = Vector3.zero;
				BreakAllJoint(array[i]);
				if (utilityBoneBody.bone.children.Exists((Bone bone) => bone.Data.Name == array[i].name))
				{
					base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, utilityBoneBody.transform, array[i].transform.position, new Vector3(0f, 0f, Mathf.Atan2(array[i].transform.position.y - rigidbody.transform.position.y, array[i].transform.position.x - rigidbody.transform.position.x) * 57.29578f));
				}
			}
		}
		BoneParticleSystem[] array2 = flyingBoneSystems;
		for (int num = 0; num < array2.Length; num++)
		{
			BoneParticleSystem boneParticleSystem = array2[num];
			if (boneParticleSystem.particleEffectPrefab == null || string.IsNullOrEmpty(boneParticleSystem.particleEffectPrefab.assetGUID))
			{
				continue;
			}
			SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(boneParticleSystem.boneName);
			BoneEffectClass boneEffectClass = new BoneEffectClass();
			Character character = base.CachedManager.CurrentCharacter as Character;
			for (int num2 = 0; num2 < character.RagdollRigidbodies.Length; num2++)
			{
				if (character.RagdollRigidbodies[num2].name == utilityBone.boneName)
				{
					boneEffectClass = character.RagdollRigidbodies[num2].gameObject.AddComponent<BoneEffectClass>();
					GameObject gameObject = (GameObject)boneParticleSystem.particleEffectPrefab.GetInstance();
					Transform transform = character.RagdollRigidbodies[num2].transform;
					gameObject.transform.parent = character.RagdollRigidbodies[num2].transform;
					gameObject.transform.position = transform.position;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.PlayFX();
					particleSystems.Add(gameObject);
					boneEffectClass.SetFlyingEffect(gameObject);
					break;
				}
			}
		}
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
		AudioManager.PlayAudioWithKey("green_arrow_brutality_explode");
	}

	private void ApplyFatalityEffects()
	{
		List<SkeletonUtilityBone> list = new List<SkeletonUtilityBone>();
		list.Add(base.CachedManager.GetUtilityBoneBody());
		list.Add(base.CachedManager.GetUtilityBoneHead());
		if (list.Count > 0)
		{
			foreach (SkeletonUtilityBone item in list)
			{
				if (item == CachedHead)
				{
					base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, item.transform.position, item.transform.eulerAngles);
					base.CachedEffect.EmitBrain(base.transform, item.transform.position, Vector3.up);
				}
				else if (item == CachedBody)
				{
					base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, item.transform.position, item.transform.eulerAngles);
					base.CachedEffect.EmitGuts(base.transform, item.transform.position, Vector3.up);
				}
			}
		}
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
	}

	private void BoneEffectClass_OnCollided(GameObject go)
	{
		BoneParticleSystem[] array = collidedBoneSystems;
		for (int i = 0; i < array.Length; i++)
		{
			BoneParticleSystem boneParticleSystem = array[i];
			if (boneParticleSystem.particleEffectPrefab == null || string.IsNullOrEmpty(boneParticleSystem.particleEffectPrefab.assetGUID))
			{
				continue;
			}
			SkeletonUtilityBone utilityBone = base.CachedManager.GetUtilityBone(boneParticleSystem.boneName);
			Character character = base.CachedManager.CurrentCharacter as Character;
			for (int j = 0; j < character.RagdollRigidbodies.Length; j++)
			{
				if (character.RagdollRigidbodies[j].name == utilityBone.boneName)
				{
					GameObject gameObject = (GameObject)boneParticleSystem.particleEffectPrefab.GetInstance();
					Transform transform = character.RagdollRigidbodies[j].transform;
					gameObject.transform.parent = transform;
					gameObject.transform.position = transform.position;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.PlayFX();
					particleSystems.Add(gameObject);
					break;
				}
			}
		}
	}
}
