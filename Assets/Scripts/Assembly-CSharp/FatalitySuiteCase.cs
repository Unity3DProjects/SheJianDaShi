using System.Collections;
using UnityEngine;

public class FatalitySuiteCase : FatalityDumpedSign
{
	private const string BLOOD_SOUND_KEY = "blood_fountain_loop";

	[SerializeField]
	private FatalityBricks[] fatalitiesBricksPrefabs;

	[SerializeField]
	[ResourceLink]
	private AssetLink moneyFlightEffectPrefab;

	[SerializeField]
	private float fatalityDelay;

	[SerializeField]
	private float delayBetweenBricks;

	[SerializeField]
	private float moneyEffectHeight;

	[SerializeField]
	private float effectMultiplier;

	public override void UpdateFatality(float deltaTime)
	{
		base.UpdateFatality(deltaTime);
		AudioManager.StopAudioWithKey("blood_fountain_loop");
	}

	public override void StartFatality(CollisionInfo infoCollision)
	{
		base.StartFatality(infoCollision);
		StartCoroutine(StartFatalityBricks());
	}

	protected override void ExplodeCharacter(bool isNeedBloodEffects = true)
	{
		SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration();
		base.ExplodeCharacter();
	}

	private IEnumerator StartFatalityBricks()
	{
		yield return new WaitForSeconds(fatalityDelay);
		if (moneyFlightEffectPrefab != null && !string.IsNullOrEmpty(moneyFlightEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)moneyFlightEffectPrefab.GetInstance();
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = base.CachedManager.CurrentCharacter.BodyTransform.position + Vector3.up * moneyEffectHeight;
		}
		for (int i = 0; i < fatalitiesBricksPrefabs.Length; i++)
		{
			yield return new WaitForSeconds(delayBetweenBricks);
			FatalityBricks fatalityBricks = Object.Instantiate(fatalitiesBricksPrefabs[i]);
			fatalityBricks.transform.parent = base.CachedManager.transform;
			fatalityBricks.transform.localEulerAngles = Vector3.zero;
			fatalityBricks.transform.localPosition = Vector3.zero;
			fatalityBricks.StartFatalityBrick(base.gameObject);
		}
	}

	protected override void ObserverPhysics_EventTriggerEnter(Collider coll)
	{
		if (!isCharacterDamaged)
		{
			isCharacterDamaged = true;
			ExplodeCharacter();
			for (int i = 0; (float)i < effectMultiplier; i++)
			{
				base.CachedEffect.EmitGutsEffect(base.CachedManager.CurrentCharacter.CharacterType, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.transform, base.CachedManager.CurrentCharacter.RootRigidBody.transform.position, Vector3.zero);
			}
			if (!string.IsNullOrEmpty(characterCollisionSound))
			{
				AudioManager.PlayAudioWithKey(characterCollisionSound);
			}
			SingletonMonoBehaviour<GameManager>.Instance.CameraController.ApplyShakeWithDuration(2f, 10f);
		}
	}
}
