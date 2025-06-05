using UnityEngine;

public class EmissionCharacter : MonoBehaviour
{
	public enum IdleEffectPositionType
	{
		Background = 0,
		RightArm = 1,
		LeftArm = 2,
		LeftArmByEvent = 3,
		LeftArmByEventSecond = 4,
		Broom = 5,
		BroomSplash = 6,
		HammerFirst = 7,
		HammerSecond = 8,
		Skull = 9
	}

	[SerializeField]
	[ResourceLink]
	private AssetLink backgroundIdleEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink rightArmEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink rightArmByEventEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink rightArmByEventSecondEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink broomEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink broomSplashEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink hammerIdleEffect;

	[SerializeField]
	[ResourceLink]
	private AssetLink skullIdleEffect;

	[SerializeField]
	private bool isUnflippedWithScale;

	[SerializeField]
	private bool isBodyUnavailable;

	public bool IsUnflippedWithScale
	{
		get
		{
			return isUnflippedWithScale;
		}
	}

	public GameObject StartIdleEffect(Transform parent, Vector3 position, Vector2 velocity, EmitType emitType = EmitType.Shoot, IdleEffectPositionType effect = IdleEffectPositionType.Background)
	{
		GameObject gameObject = null;
		switch (effect)
		{
		case IdleEffectPositionType.Background:
			if (backgroundIdleEffect != null && !string.IsNullOrEmpty(backgroundIdleEffect.assetGUID))
			{
				GameObject gameObject6 = backgroundIdleEffect.GetAsset() as GameObject;
				EmittedSystemInfo component5 = gameObject6.GetComponent<EmittedSystemInfo>();
				if (!component5 || component5.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject6);
					AddObject(gameObject, parent, position, velocity);
					gameObject.transform.localScale = gameObject6.transform.localScale;
					gameObject.transform.localPosition = gameObject6.transform.localPosition;
				}
			}
			break;
		case IdleEffectPositionType.LeftArm:
			if (rightArmEffect != null && !string.IsNullOrEmpty(rightArmEffect.assetGUID))
			{
				GameObject gameObject9 = rightArmEffect.GetAsset() as GameObject;
				EmittedSystemInfo component8 = gameObject9.GetComponent<EmittedSystemInfo>();
				if (!component8 || component8.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject9);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "arm_l/forearm_l/hand_l");
					gameObject.transform.localScale = gameObject9.transform.localScale;
					gameObject.transform.localPosition = gameObject9.transform.localPosition;
				}
			}
			break;
		case IdleEffectPositionType.LeftArmByEvent:
			if (rightArmByEventEffect != null && !string.IsNullOrEmpty(rightArmByEventEffect.assetGUID))
			{
				GameObject gameObject5 = rightArmByEventEffect.GetAsset() as GameObject;
				EmittedSystemInfo component4 = gameObject5.GetComponent<EmittedSystemInfo>();
				if (!component4 || component4.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject5);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "arm_l/forearm_l/hand_l");
					gameObject.transform.localScale = gameObject5.transform.localScale;
					gameObject.transform.localPosition = gameObject5.transform.localPosition;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.rotation = Quaternion.identity;
				}
			}
			break;
		case IdleEffectPositionType.LeftArmByEventSecond:
			if (rightArmByEventSecondEffect != null && !string.IsNullOrEmpty(rightArmByEventSecondEffect.assetGUID))
			{
				GameObject gameObject10 = rightArmByEventSecondEffect.GetAsset() as GameObject;
				EmittedSystemInfo component9 = gameObject10.GetComponent<EmittedSystemInfo>();
				if (!component9 || component9.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject10);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "arm_l/forearm_l/hand_l");
					gameObject.transform.localScale = gameObject10.transform.localScale;
					gameObject.transform.localPosition = gameObject10.transform.localPosition;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.rotation = Quaternion.identity;
				}
			}
			break;
		case IdleEffectPositionType.Broom:
			if (broomEffect != null && !string.IsNullOrEmpty(broomEffect.assetGUID))
			{
				GameObject gameObject8 = broomEffect.GetAsset() as GameObject;
				EmittedSystemInfo component7 = gameObject8.GetComponent<EmittedSystemInfo>();
				if (!component7 || component7.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject8);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "arm_l/forearm_l/broomstick/idle_effect");
					gameObject.transform.localScale = gameObject8.transform.localScale;
					gameObject.transform.localPosition = gameObject8.transform.localPosition;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.rotation = Quaternion.identity;
				}
			}
			break;
		case IdleEffectPositionType.BroomSplash:
			if (broomSplashEffect != null && !string.IsNullOrEmpty(broomSplashEffect.assetGUID))
			{
				GameObject gameObject3 = broomSplashEffect.GetAsset() as GameObject;
				EmittedSystemInfo component2 = gameObject3.GetComponent<EmittedSystemInfo>();
				if (!component2 || component2.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject3);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "arm_l/forearm_l/broomstick/idle_effect");
					gameObject.transform.localScale = gameObject3.transform.localScale;
					gameObject.transform.localPosition = gameObject3.transform.localPosition;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.rotation = Quaternion.identity;
				}
			}
			break;
		case IdleEffectPositionType.HammerFirst:
			if (hammerIdleEffect != null && !string.IsNullOrEmpty(hammerIdleEffect.assetGUID))
			{
				GameObject gameObject4 = hammerIdleEffect.GetAsset() as GameObject;
				EmittedSystemInfo component3 = gameObject4.GetComponent<EmittedSystemInfo>();
				if (!component3 || component3.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject4);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "body/arm_l/forearm_l/hand_l/hammer/effect_hammer");
					gameObject.transform.localScale = gameObject4.transform.localScale;
					gameObject.transform.localPosition = gameObject4.transform.localPosition;
					gameObject.transform.localRotation = Quaternion.identity;
				}
			}
			break;
		case IdleEffectPositionType.HammerSecond:
			if (hammerIdleEffect != null && !string.IsNullOrEmpty(hammerIdleEffect.assetGUID))
			{
				GameObject gameObject7 = hammerIdleEffect.GetAsset() as GameObject;
				EmittedSystemInfo component6 = gameObject7.GetComponent<EmittedSystemInfo>();
				if (!component6 || component6.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject7);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "hammer2/effect_hammer2");
					gameObject.transform.localScale = gameObject7.transform.localScale;
					gameObject.transform.localPosition = gameObject7.transform.localPosition;
					gameObject.transform.localRotation = Quaternion.identity;
				}
			}
			break;
		case IdleEffectPositionType.Skull:
			if (skullIdleEffect != null && !string.IsNullOrEmpty(skullIdleEffect.assetGUID))
			{
				GameObject gameObject2 = skullIdleEffect.GetAsset() as GameObject;
				EmittedSystemInfo component = gameObject2.GetComponent<EmittedSystemInfo>();
				if (!component || component.EmitType == emitType)
				{
					gameObject = Object.Instantiate(gameObject2);
					AddObjectToPartOfBody(gameObject, parent, position, velocity, "steam");
					gameObject.transform.localScale = gameObject2.transform.localScale;
					gameObject.transform.localPosition = gameObject2.transform.localPosition;
					gameObject.transform.localRotation = Quaternion.identity;
				}
			}
			break;
		}
		return gameObject;
	}

	private void AddObject(GameObject obj, Transform parent, Vector3 position, Vector2 velocity)
	{
		if ((bool)obj)
		{
			obj.transform.parent = parent;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.position = position;
		}
	}

	private void AddObjectToPartOfBody(GameObject obj, Transform parent, Vector3 position, Vector2 velocity, string bodyPartName)
	{
		if ((bool)obj)
		{
			Character component = parent.gameObject.GetComponent<Character>();
			GUIEffectCreator component2 = base.gameObject.GetComponent<GUIEffectCreator>();
			if ((bool)component && !isBodyUnavailable)
			{
				obj.transform.parent = component.BodyTransform.Find(bodyPartName);
			}
			else
			{
				obj.transform.parent = component2.BodyTransform.Find(bodyPartName);
			}
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.position = position;
		}
	}
}
