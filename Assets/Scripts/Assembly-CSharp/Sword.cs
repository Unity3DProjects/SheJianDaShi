using UnityEngine;

public class Sword : Axe
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
}
