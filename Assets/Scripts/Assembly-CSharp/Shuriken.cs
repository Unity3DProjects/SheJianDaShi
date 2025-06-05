using UnityEngine;

public class Shuriken : WeaponGeneral
{
	[SerializeField]
	private int maxTapCount = 6;

	[SerializeField]
	private float angleBetweenShurikens = 60f;

	private int currentTapCount;

	protected override void UpdatePhysicalWeapon(float deltaTime)
	{
		base.UpdatePhysicalWeapon(deltaTime);
		Rigidbody component = base.gameObject.GetComponent<Rigidbody>();
		component.angularDrag = 0f;
	}

	protected override void EmitObjects()
	{
		Emission component = base.gameObject.GetComponent<Emission>();
		if ((bool)component)
		{
			if (velocity.x < 0f)
			{
				component.EmitObjects(level, base.CachedTransform.position, (float)currentTapCount * angleBetweenShurikens + 180f, base.CachedRigidbody.velocity.magnitude);
			}
			else
			{
				component.EmitObjects(level, base.CachedTransform.position, (float)(-currentTapCount) * angleBetweenShurikens, base.CachedRigidbody.velocity.magnitude);
			}
		}
	}

	private void OnTap()
	{
		if (currentTapCount < maxTapCount)
		{
			EmitObjects();
			currentTapCount++;
		}
	}
}
