using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class FatalityMedallion : FatalitySatelite
{
	private const string START_MUSIC_EVENT = "music";

	[SerializeField]
	[ResourceLink]
	private AssetLink soundEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink bloodEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink medallionDisappearEffecctPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink moneyRainEffectPrefab;

	[SerializeField]
	private float delay;

	[SerializeField]
	private float delayBreakUpCharacter;

	[SerializeField]
	private int womansCount;

	[SerializeField]
	private TwerkingWoman[] womansPrefabs;

	[SerializeField]
	private float womenOffsetX;

	[SerializeField]
	private float womenOffsetY;

	[SerializeField]
	private string songName;

	[SerializeField]
	private string columnDropSoundName;

	private GameObject soundEffect;

	private EventData cachedEvent;

	private MusicColumn column;

	private TwerkingWoman[] womans;

	private bool isWomenCreated;

	private bool isButtonShow;

	public override void StartFatality(CollisionInfo info)
	{
		if (!base.IsActive)
		{
			base.StartFatality(info);
			womans = new TwerkingWoman[womansCount];
			if (medallionDisappearEffecctPrefab != null && !string.IsNullOrEmpty(medallionDisappearEffecctPrefab.assetGUID))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate((GameObject)medallionDisappearEffecctPrefab.GetAsset());
				gameObject.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
				gameObject.transform.position = info.position;
				if (info.collider != null)
				{
					info.collider.gameObject.SetActive(false);
				}
				SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.CurrentWeapon.RemoveFromCharacter();
			}
		}
		base.CachedManager.CurrentCharacter.SetAnimation(0, "finish_him", true);
		if (!isWomenCreated)
		{
			List<TwerkingWoman> list = new List<TwerkingWoman>(womansPrefabs);
			for (int i = 0; i < womansCount; i++)
			{
				TwerkingWoman twerkingWoman = list.RandomObject();
				bool flag = i % 2 == 0;
				Vector3 position = base.CachedManager.CurrentCharacter.transform.position;
				Vector3 vector = ((!flag) ? (Vector3.right * womenOffsetX + Vector3.left * 150f * (i / 2)) : (Vector3.left * womenOffsetX + Vector3.right * 150f * (i / 2)));
				Vector3 position2 = position + Vector3.up * womenOffsetY + Vector3.back * 5f + vector;
				womans[i] = UnityEngine.Object.Instantiate(twerkingWoman, position2, Quaternion.identity, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
				Vector3 localScale = womans[i].gameObject.transform.localScale;
				womans[i].gameObject.transform.localScale = new Vector3(localScale.x * ((!flag) ? 1f : (-1f)), localScale.y, localScale.z);
				list.Remove(twerkingWoman);
			}
			list.Clear();
			isWomenCreated = true;
		}
	}

	public override void StopFatality()
	{
		base.StopFatality();
		GameScreen gameScreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.GameScreen;
		if (gameScreen != null)
		{
			gameScreen.ShowPartyMastersButton();
		}
	}

	protected override void SetSateliteVelocity()
	{
		if (moneyRainEffectPrefab != null && !string.IsNullOrEmpty(moneyRainEffectPrefab.assetGUID))
		{
			tk2dCamera cachedCamera = SingletonMonoBehaviour<GameManager>.Instance.CameraController.CachedCamera;
			Vector3 position = cachedCamera.transform.position;
			Vector3 position2 = new Vector3(position.x, position.y + Mathf.Max(cachedCamera.NativeResolution.y, cachedCamera.NativeResolution.x) * 0.5f + 5f, 0f);
			GameObject gameObject = UnityEngine.Object.Instantiate((GameObject)moneyRainEffectPrefab.GetAsset(), SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform);
			gameObject.transform.position = position2;
		}
		Rigidbody component = currentSatelite.GetComponent<Rigidbody>();
		if ((bool)component)
		{
			component.velocity = Vector2.up * startVerticalSpeed;
		}
		currentSatelite.transform.rotation = Quaternion.identity;
		column = currentSatelite.GetComponent<MusicColumn>();
		column.SkeletonAnimation.Skeleton.Data.FindEvent("music");
		column.SkeletonAnimation.State.Event += Column_SkeletonAnimation_State_Event;
		cachedEvent = column.SkeletonAnimation.Skeleton.Data.FindEvent("music");
	}

	protected override void ObserverPhysics_EventTriggerEnter(Collider coll)
	{
		base.ObserverPhysics_EventTriggerEnter(coll);
		AudioManager.PlayAudioWithKey(columnDropSoundName);
		StartCoroutine(StartAnimationMusic());
	}

	private void BreakUpCharacter()
	{
		base.CachedManager.CurrentCharacter.StartDefaultAnimation();
		base.CachedManager.CurrentCharacter.ApplyCharacterRagdoll();
		SkeletonUtilityBone utilityBoneBody = base.CachedManager.GetUtilityBoneBody();
		if ((bool)utilityBoneBody)
		{
			Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(utilityBoneBody.boneName);
			Rigidbody[] rigidbodyArray = base.CachedManager.CharacterRagdoll.GetRigidbodyArray();
			if ((bool)rigidbody && rigidbodyArray != null && rigidbodyArray.Length > 0)
			{
				rigidbody.velocity = Vector3.up * 0f;
				rigidbody.angularVelocity = Vector3.forward * 30f;
				for (int i = 0; i < rigidbodyArray.Length; i++)
				{
					if (rigidbodyArray[i] != rigidbody && rigidbodyArray[i] != null)
					{
						rigidbodyArray[i].velocity = Vector3.zero;
						rigidbodyArray[i].constraints = RigidbodyConstraints.FreezeRotationX;
					}
					if (rigidbodyArray[i] != null)
					{
						rigidbodyArray[i].constraints = (RigidbodyConstraints)56;
					}
					BreakAllJoint(rigidbodyArray[i]);
				}
			}
		}
		base.CachedMask.Activate(startFatalityCollider, startFatalityUtilityBone);
	}

	private void ApplyBloodEffects()
	{
		if (bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
		{
			int num = 10;
			float num2 = (base.CachedManager.CurrentCharacter.HeadColliderBody.transform.position.y - base.CachedManager.CurrentCharacter.transform.position.y) / (float)num;
			for (int i = 0; i < num * 2; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate((GameObject)bloodEffectPrefab.GetAsset());
				gameObject.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
				gameObject.transform.position = base.CachedManager.CurrentCharacter.transform.position + Vector3.up * ((float)i * num2) + Vector3.right * (50f * Mathf.Cos(num - i) / (float)num * (float)Math.PI);
				gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
			}
		}
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		base.ExplodeCharacter(false);
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration();
	}

	private IEnumerator StartAnimationMusic()
	{
		yield return new WaitForSeconds(delay);
		if (currentSatelite != null)
		{
			MusicColumn component = currentSatelite.GetComponent<MusicColumn>();
			if (component != null)
			{
				component.ApplyMusic();
			}
		}
	}

	private IEnumerator StartCharacterAnimation()
	{
		base.CachedManager.CurrentCharacter.SkeletonAnimation.state.SetAnimation(0, base.CachedManager.AnimationGuillotinePose, true);
		ApplyBloodEffects();
		AudioManager.PlayAudioWithKey(songName);
		TwerkingWoman[] array = womans;
		foreach (TwerkingWoman twerkingWoman in array)
		{
			if (twerkingWoman != null)
			{
				twerkingWoman.ApplyTwerking();
			}
		}
		yield return new WaitForSeconds(delayBreakUpCharacter);
		ExplodeCharacter();
	}

	private void Column_SkeletonAnimation_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == cachedEvent)
		{
			if (soundEffectPrefab != null && !string.IsNullOrEmpty(soundEffectPrefab.assetGUID))
			{
				soundEffect = UnityEngine.Object.Instantiate((GameObject)soundEffectPrefab.GetAsset());
				soundEffect.transform.parent = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform;
				soundEffect.transform.position = currentSatelite.transform.position;
			}
			StartCoroutine(StartCharacterAnimation());
			column.SkeletonAnimation.State.Event -= Column_SkeletonAnimation_State_Event;
		}
	}
}
