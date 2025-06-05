using System;
using UnityEngine;

public class UpgradeArrowPart : MonoBehaviour
{
	public static Action OnArrowInterrupted;

	[SerializeField]
	private TweenTransform tweener;

	[SerializeField]
	private float hitForce = 100000f;

	[SerializeField]
	[ResourceLink]
	protected AssetLink bloodEffectPrefab;

	[SerializeField]
	[ResourceLink]
	protected AssetLink trailEffectPrefab;

	[SerializeField]
	private DecalType decalType;

	[SerializeField]
	private BoxCollider arrowTipCollider;

	[SerializeField]
	private BoxCollider arrowBodyCollider;

	private Transform parentTransform;

	private GameObject trail;

	public bool IsForceNeeded { get; set; }

	private void OnTriggerEnter(Collider collider)
	{
		SkeletonUtilityBone component = collider.transform.parent.GetComponent<SkeletonUtilityBone>();
		if (!component)
		{
			return;
		}
		CharacterBase componentInParent = component.GetComponentInParent<CharacterBase>();
		for (int i = 0; i < componentInParent.RagdollRigidbodies.Length; i++)
		{
			if (componentInParent.RagdollRigidbodies[i].name == component.boneName)
			{
				parentTransform = componentInParent.RagdollRigidbodies[i].transform;
				break;
			}
		}
		if (parentTransform != null)
		{
			if (arrowTipCollider.enabled)
			{
				UnityEngine.Object.Destroy(trail);
				arrowTipCollider.enabled = false;
				arrowBodyCollider.enabled = true;
				if (IsForceNeeded)
				{
					componentInParent.RootRigidBody.AddForce(base.transform.position * hitForce);
				}
				if (bloodEffectPrefab != null && !string.IsNullOrEmpty(bloodEffectPrefab.assetGUID))
				{
					UnityEngine.Object.Instantiate((GameObject)bloodEffectPrefab.GetAsset(), base.transform.position, Quaternion.identity, parentTransform.transform);
				}
				MaskBase mask = componentInParent.masksManager.GetMask(component.boneName);
				if (mask != null)
				{
					componentInParent.masksManager.AddDecal(mask, decalType, component.transform.position.ToVector2());
				}
			}
			else if (arrowBodyCollider.enabled)
			{
				if (OnArrowInterrupted != null)
				{
					OnArrowInterrupted();
				}
				arrowBodyCollider.enabled = false;
				tweener.enabled = false;
				base.transform.parent = parentTransform;
				base.transform.localPosition = Vector3.zero;
			}
		}
		AudioManager.PlayAudioWithKey("green_arrow_collision");
	}

	public void ShotArrow(Transform beginTransform, Transform endTransform, float time)
	{
		tweener.enabled = true;
		tweener.duration = time;
		tweener.beginTransform = beginTransform;
		tweener.endTransform = endTransform;
		if (trailEffectPrefab != null && !string.IsNullOrEmpty(trailEffectPrefab.assetGUID))
		{
			trail = UnityEngine.Object.Instantiate((GameObject)trailEffectPrefab.GetAsset(), base.transform.position, base.transform.rotation, base.transform);
		}
	}
}
