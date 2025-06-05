using UnityEngine;

public class BigKnife : Axe
{
	[SerializeField]
	private Collider handleCollider;

	protected override void SetCollidersEnabled(bool isEnabled)
	{
		if ((bool)level && (bool)level.InactiveArcher && level.InactiveArcher.IsFinishHim)
		{
			AudioManager.PlayAudioWithKey(characterHitSound);
			{
				foreach (Collider collider in base.Colliders)
				{
					if (collider != handleCollider)
					{
						collider.enabled = true;
						collider.isTrigger = true;
					}
					else
					{
						collider.gameObject.layer = LayerMask.NameToLayer("IgnoreCharacterCollisions");
					}
				}
				return;
			}
		}
		base.SetCollidersEnabled(isEnabled);
	}

	protected override void OnTapActivation()
	{
		base.CachedTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
		base.CachedRigidbody.velocity = new Vector3(0f, -1000f);
		base.CachedRigidbody.angularVelocity = Vector3.zero;
		foreach (Collider collider in base.Colliders)
		{
			collider.isTrigger = true;
		}
	}
}
