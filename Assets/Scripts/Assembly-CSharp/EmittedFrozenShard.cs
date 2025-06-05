using UnityEngine;

public class EmittedFrozenShard : WeaponGeneral
{
	[SerializeField]
	private float minSpeedRelativeDeviation;

	[SerializeField]
	private float maxSpeedRelativeDeviation;

	[SerializeField]
	private GameObject[] possibleShards;

	protected override void Awake()
	{
		base.Awake();
		int num = Random.Range(0, possibleShards.Length);
		possibleShards[num].SetActive(true);
		weaponMesh = possibleShards[num].GetComponent<MeshFilter>();
	}

	public override void Shoot()
	{
		base.Shoot();
		float num = Random.Range(minSpeedRelativeDeviation, maxSpeedRelativeDeviation);
		base.CachedRigidbody.velocity = Vector3.Scale(base.CachedRigidbody.velocity, new Vector3(num, num));
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
