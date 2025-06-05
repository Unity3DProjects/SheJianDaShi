using UnityEngine;

public class Flag : WeaponGeneral
{
	[SerializeField]
	private SkinnedMeshRenderer flagMesh;

	public override bool IsWeaponVisible
	{
		get
		{
			return base.IsWeaponVisible;
		}
		set
		{
			base.IsWeaponVisible = value;
			flagMesh.enabled = isWeaponVisible;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		flagMesh.enabled = false;
	}
}
