using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatalityTurkeys : FatalityBase
{
	public static Action OnBrainEmitted;

	[SerializeField]
	private FatalityTurkey turkeyPrefab;

	[SerializeField]
	private int numberOfTurkeys;

	[SerializeField]
	[Range(0f, 1f)]
	private float breakProbability = 0.2f;

	[SerializeField]
	[Range(0f, 1f)]
	private float gutsProbability = 0.1f;

	[SerializeField]
	private Vector2 turkeySpawnPoint;

	[SerializeField]
	private float turkeyStartVerticalSpeed = -1000f;

	[SerializeField]
	private float delayBeforeStart;

	[SerializeField]
	private float maxDelayBetweenTurkeys = 0.1f;

	[SerializeField]
	private float leftBorder;

	[SerializeField]
	private float rightBorder;

	[SerializeField]
	private float turkeyFinishOffset = 20f;

	[SerializeField]
	private float biteShakeDuration = 0.05f;

	[SerializeField]
	private float biteShakeAmplitude = 0.1f;

	[SerializeField]
	private Vector3 brainLocalPosition;

	[SerializeField]
	private float effectMultiplier = 8f;

	private List<FatalityTurkey> turkeys = new List<FatalityTurkey>();

	private List<string> breakBonesNames = new List<string>();

	private FatalityManager cachedFatalityManager;

	private SkeletonUtilityBone cachedHead;

	private SkeletonUtilityBone cachedBody;

	private Character affectedCharacter;

	private int currentTurkeyOrder;

	private int leftTurkeysCount;

	private int rightTurkeysCount;

	private List<Rigidbody> bones = new List<Rigidbody>();

	private bool isNeedToFlyWithGuts = true;

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
		FatalityTurkey.OnTurkeyBite = (Action<Transform>)Delegate.Combine(FatalityTurkey.OnTurkeyBite, new Action<Transform>(FatalityTurkey_OnTurkeyBite));
	}

	private void OnDisable()
	{
		FatalityTurkey.OnTurkeyBite = (Action<Transform>)Delegate.Remove(FatalityTurkey.OnTurkeyBite, new Action<Transform>(FatalityTurkey_OnTurkeyBite));
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		if (base.IsActive)
		{
			return;
		}
		base.StartFatality(infoCollision);
		affectedCharacter = infoCollision.otherCollider.GetComponentInParent<Character>();
		affectedCharacter.ForceNotRestore = true;
		affectedCharacter.StartDefaultAnimation();
		affectedCharacter.SkeletonAnimation.State.ClearTrack(0);
		affectedCharacter.StartFaceDefaultAnimation();
		affectedCharacter.ApplyCharacterRagdoll();
		StartCoroutine(CreateTurkeys());
		SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
		{
			for (int i = 0; (float)i < effectMultiplier; i++)
			{
				base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform, base.CachedManager.CurrentCharacter.RootRigidBody.transform.position, Vector3.zero);
			}
			for (int j = 0; j < turkeys.Count; j++)
			{
				if (!turkeys[j].IsFlyingTurkey)
				{
					turkeys[j].Fly();
				}
			}
			PlayCrowdHappySound();
			turkeys.Clear();
		}, currentTime - 3f);
	}

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		if (!base.IsActive)
		{
			return;
		}
		for (int i = 0; i < bones.Count; i++)
		{
			if (bones[i] != null && UnityEngine.Random.value < 0.01f)
			{
				bones[i].AddForce(Vector3.up * bones[i].mass / 4f, ForceMode.VelocityChange);
			}
		}
		if (turkeys.Count == 0)
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(biteShakeDuration, biteShakeAmplitude);
		}
		else
		{
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(biteShakeDuration * (float)turkeys.Count, biteShakeAmplitude * (float)turkeys.Count);
		}
	}

	protected override void PlayFatalityStartSound()
	{
		AudioManager.PlayAudioWithKey("fatality_start");
	}

	private void CreateTurkey()
	{
		float min = turkeySpawnPoint.x + leftBorder;
		float max = turkeySpawnPoint.x + rightBorder;
		Vector2 vector = turkeySpawnPoint;
		vector.x = UnityEngine.Random.Range(min, max);
		if (vector.x < 0f && leftTurkeysCount > numberOfTurkeys / 2)
		{
			vector.x *= -1f;
			rightTurkeysCount++;
		}
		else if (vector.x > 0f && rightTurkeysCount > numberOfTurkeys / 2)
		{
			vector.x *= -1f;
			leftTurkeysCount++;
		}
		else if (vector.x < 0f)
		{
			leftTurkeysCount++;
		}
		else
		{
			rightTurkeysCount++;
		}
		FatalityTurkey fatalityTurkey = UnityEngine.Object.Instantiate(turkeyPrefab);
		fatalityTurkey.transform.parent = base.transform;
		fatalityTurkey.transform.position = CachedBody.transform.position + (Vector3)vector;
		fatalityTurkey.TurkeyAnimationLocalScale = ((!(vector.x > 0f)) ? Vector3.one : new Vector3(-1f, 1f, 1f));
		fatalityTurkey.transform.localPosition = new Vector3(fatalityTurkey.transform.localPosition.x, fatalityTurkey.transform.localPosition.y, currentTurkeyOrder);
		currentTurkeyOrder++;
		Collider[] componentsInChildren = affectedCharacter.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < fatalityTurkey.TurkeyColliders.Count; j++)
			{
				Physics.IgnoreCollision(componentsInChildren[i], fatalityTurkey.TurkeyColliders[j]);
			}
		}
		Rigidbody componentInChildren = fatalityTurkey.GetComponentInChildren<Rigidbody>();
		if ((bool)componentInChildren)
		{
			float num = 0f;
			float x = 0f;
			float num2 = turkeyStartVerticalSpeed;
			if (!Mathf.Approximately(Physics.gravity.y, 0f))
			{
				float num3 = Mathf.Pow(num2, 2f) - 2f * Physics.gravity.y * vector.y;
				if (num3 >= 0f)
				{
					float num4 = (0f - num2 + Mathf.Sqrt(num3)) / Physics.gravity.y;
					float num5 = (0f - num2 - Mathf.Sqrt(num3)) / Physics.gravity.y;
					if (num4 > 0f)
					{
						num = ((!(num5 > 0f) || !(num4 > num5)) ? num4 : num5);
					}
					else if (num5 > 0f)
					{
						num = num5;
					}
					float num6 = ((!(vector.x > 0f)) ? (0f - turkeyFinishOffset + UnityEngine.Random.Range(0f, 100f)) : (turkeyFinishOffset - UnityEngine.Random.Range(0f, 100f)));
					x = (num6 - vector.x) / num;
				}
			}
			componentInChildren.velocity = new Vector2(x, num2);
		}
		turkeys.Add(fatalityTurkey);
	}

	private IEnumerator CreateTurkeys()
	{
		yield return new WaitForSeconds(delayBeforeStart);
		AudioManager.PlayAudioWithKey("turkeys_fatality");
		for (int i = 0; i < numberOfTurkeys; i++)
		{
			CreateTurkey();
			yield return new WaitForSeconds(UnityEngine.Random.Range(0f, maxDelayBetweenTurkeys));
		}
	}

	private void FatalityTurkey_OnTurkeyBite(Transform beakTransform)
	{
		if (!(base.CachedManager.ActiveFatality == this))
		{
			return;
		}
		if (!GoreContentFilter.AllowDismemberment)
		{
			return;
		}
		SkeletonUtilityBone skeletonUtilityBone = ((!(UnityEngine.Random.value <= gutsProbability)) ? base.CachedManager.GetUtilityBoneByPosition(beakTransform.position) : CachedBody);
		if (!(skeletonUtilityBone != null) || breakBonesNames.Contains(skeletonUtilityBone.boneName) || !(UnityEngine.Random.value <= breakProbability))
		{
			return;
		}
		if (skeletonUtilityBone != CachedBody)
		{
			breakBonesNames.Add(skeletonUtilityBone.boneName);
		}
		Rigidbody rigidbody = base.CachedManager.CharacterRagdoll.GetRigidbody(skeletonUtilityBone.boneName);
		if (skeletonUtilityBone != CachedBody && skeletonUtilityBone != CachedHead)
		{
			bones.Add(rigidbody);
		}
		Vector3 vector = new Vector3(skeletonUtilityBone.transform.position.x - CachedBody.transform.position.x, skeletonUtilityBone.transform.position.y - CachedBody.transform.position.y, 0f);
		Vector3 eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vector.y, vector.x) * 57.29578f);
		if ((bool)rigidbody && (skeletonUtilityBone != CachedBody || (skeletonUtilityBone == CachedBody && UnityEngine.Random.value <= gutsProbability)))
		{
			rigidbody.velocity = vector.normalized * UnityEngine.Random.Range(500f, 700f);
			rigidbody.angularVelocity = new Vector3(0f, 0f, UnityEngine.Random.Range(5f, 15f));
			rigidbody.maxAngularVelocity = 15f;
			rigidbody.constraints = (RigidbodyConstraints)56;
		}
		base.CachedEffect.EmitBloodEffect(base.CachedManager.CurrentCharacter.CharacterType, base.CachedManager.GetUtilityBone(skeletonUtilityBone.bone.Parent.Data.Name).transform, skeletonUtilityBone.transform.position, eulerAngles);
		if (skeletonUtilityBone == CachedHead)
		{
			base.CachedEffect.EmitBrain(base.transform, skeletonUtilityBone.transform.position, Vector3.up + Vector3.right * UnityEngine.Random.Range(-1f, 1f));
			base.CachedEffect.EmitBrainEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedHead.transform.position, Vector3.zero);
			Rigidbody[] componentsInChildren = GetComponentsInChildren<Rigidbody>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name.Contains("Brain"))
				{
					Rigidbody rigidbody2 = componentsInChildren[i];
					rigidbody2.isKinematic = true;
					rigidbody2.transform.parent = beakTransform;
					rigidbody2.transform.localPosition = brainLocalPosition;
					rigidbody2.transform.rotation = Quaternion.identity;
					if (OnBrainEmitted != null)
					{
						OnBrainEmitted();
					}
					break;
				}
			}
		}
		else if (skeletonUtilityBone == CachedBody)
		{
			base.CachedEffect.EmitGuts(base.transform, skeletonUtilityBone.transform.position, Vector3.up + Vector3.right * UnityEngine.Random.Range(-0.5f, 0.5f));
			if (isNeedToFlyWithGuts)
			{
				base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, base.transform, CachedBody.transform.position, Vector3.zero);
				Rigidbody[] componentsInChildren2 = GetComponentsInChildren<Rigidbody>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					if (componentsInChildren2[j].name.Contains("Guts"))
					{
						Rigidbody rigidbody3 = componentsInChildren2[j];
						rigidbody3.isKinematic = true;
						rigidbody3.transform.parent = beakTransform;
						rigidbody3.transform.localPosition = brainLocalPosition;
						rigidbody3.transform.rotation = Quaternion.identity;
						if (OnBrainEmitted != null)
						{
							OnBrainEmitted();
						}
						break;
					}
				}
				isNeedToFlyWithGuts = true;
			}
		}
		base.CachedMask.Activate(cachedManager.GetCollider(skeletonUtilityBone), skeletonUtilityBone);
		BreakAllJoint(rigidbody);
	}
}
