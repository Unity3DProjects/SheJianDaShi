using UnityEngine;

public class EmittedCard : WeaponGeneral
{
	[SerializeField]
	private GameObject[] possibleCards;

	protected override void Awake()
	{
		base.Awake();
		if (possibleCards != null && possibleCards.Length != 0)
		{
			possibleCards[Random.Range(0, possibleCards.Length)].gameObject.SetActive(true);
		}
	}

	public override void Shoot()
	{
		base.Shoot();
		base.CachedRigidbody.useGravity = false;
	}

	public override void Disabled(bool forced = false)
	{
		base.Disabled(true);
	}

	protected override void OnHitActivation()
	{
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null)
		{
			component.Explode(this, level);
		}
	}
}
