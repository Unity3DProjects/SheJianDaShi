using System.Collections;
using Spine;
using UnityEngine;

public class FatalityCandyTrident : FatalityTrident
{
	[SerializeField]
	private BigShark bigSharkPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink waterEffectPrefab;

	[SerializeField]
	private float gayserDealy;

	[SerializeField]
	private int gayserEffectsCount;

	[SerializeField]
	[ResourceLink]
	private AssetLink gayserEffectPrefab;

	[SerializeField]
	private float gayserShakeDuration;

	[SerializeField]
	private float gayserShakeAmplitude;

	[SerializeField]
	private float characterRestoreDelay;

	[SerializeField]
	private float brutalityEffectDelay;

	private BigShark bigShark;

	private int bitesCount;

	private CharacterBase character;

	public override void StartFatality(CollisionInfo infoCollision)
	{
		character = base.CachedManager.CurrentCharacter;
		base.StartFatality(infoCollision);
		Level currentLevel = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel;
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			if (gayserEffectPrefab != null && !string.IsNullOrEmpty(gayserEffectPrefab.assetGUID))
			{
				for (int i = 0; i < gayserEffectsCount; i++)
				{
					GameObject gameObject = (GameObject)gayserEffectPrefab.GetInstance();
					GroundEffect component = gameObject.GetComponent<GroundEffect>();
					gameObject.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.FXsTransform;
					gameObject.transform.rotation = Quaternion.identity;
					float position = character.CachedTransform.position.x + (float)(i - gayserEffectsCount / 2) * component.EffectLength;
					gameObject.transform.position = currentLevel.Location.GetPositionOnFloor(position);
				}
			}
			AudioManager.PlayLoopAudioWithKey("candy_trident_bubbles");
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(gayserShakeDuration, gayserShakeAmplitude);
		}, gayserDealy);
		ShowBrutalityEffect(brutalityEffectDelay);
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	protected override IEnumerator AnimationFinished(float delay)
	{
		yield return new WaitForSeconds(characterRestoreDelay);
		character.RestoreActionFatality();
		yield return new WaitForSeconds(delay - characterRestoreDelay);
		character.SkeletonAnimation.MeshRenderer.sortingLayerName = "Character";
		Spine.Animation fatalityAnimation = character.AnimationChache.DeathSharkAnimation;
		StartAnimation(fatalityAnimation);
		if (bigSharkPrefab != null)
		{
			bigShark = Object.Instantiate(bigSharkPrefab, character.transform.position, Quaternion.identity, character.CachedTransform.parent);
			SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsNeedFollowArcherOrWeapon = false;
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.SetTarget(character.BodyTransform, Vector2.zero, Vector2.zero);
			bigShark.OnBiten += BigShark_OnBiten;
			bigShark.OnContacted += BigShark_OnContacted;
		}
		if (waterEffectPrefab != null && !string.IsNullOrEmpty(waterEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)waterEffectPrefab.GetInstance();
			gameObject.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.FXsTransform;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.position = character.CachedTransform.position;
		}
		base.CachedEffect.EmitGUICameraEffects(BrutalityEffectType.Started);
	}

	private void BigShark_OnBiten()
	{
		bitesCount++;
		if (bitesCount == 1)
		{
			if (bigShark != null)
			{
				base.CachedEffect.EmitBloodEffect(character.CharacterType, character.CachedTransform, bigShark.JawBoneTransform.position, Vector3.up);
			}
		}
		else if (bitesCount == 2)
		{
			if (bigShark != null)
			{
				character.UpdatePosition(bigShark.JawBoneTransform.position);
				character.SkeletonAnimation.Reset();
				float num = ((!character.IsFlipped) ? 100f : 260f);
				character.CachedTransform.rotation = Quaternion.Euler(Vector3.forward * num);
			}
			SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.IsNeedFollowArcherOrWeapon = true;
			ExplodeCharacter();
			character.SkeletonAnimation.Update();
			ApplyFatalityEffects();
			base.CachedEffect.EmitGUICameraEffects(BrutalityEffectType.Finished);
			bigShark.OnBiten -= BigShark_OnBiten;
		}
	}

	private void BigShark_OnContacted()
	{
		bigShark.OnContacted -= BigShark_OnContacted;
		AudioManager.PlayAudioWithKey("candy_trident_shark_bite");
	}
}
