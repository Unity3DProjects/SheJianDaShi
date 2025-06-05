public class Molotov : WeaponGeneral
{
	protected override void OnHitActivation()
	{
		isCollideEnabled = false;
		RemoveAllActivations();
		EmitObjects();
		ExplodeComponent component = GetComponent<ExplodeComponent>();
		if (component != null)
		{
			component.Explode(this, level);
		}
	}
}
