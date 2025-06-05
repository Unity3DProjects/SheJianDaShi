using UnityEngine;

public class GUIEffectsScale : MonoBehaviour
{
	[SerializeField]
	private bool isZScale = true;

	[SerializeField]
	private bool isChildrenPositionScale = true;

	private void Awake()
	{
		Scale();
	}

	private void Scale()
	{
		float num = ((!tk2dSystem.IsRetina) ? 1f : 2f) / GUIScreenManager.GuiScale;
		if (base.gameObject.name.Contains("FX_Brutality_Title"))
		{
			num = 1f;
		}
		if (isZScale)
		{
			base.transform.localScale *= num;
		}
		else
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x * num, base.transform.localScale.y * num, base.transform.localScale.z);
		}
		ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			particleSystem.startSpeed *= num;
			particleSystem.startSize *= num;
			if (!isChildrenPositionScale)
			{
				particleSystem.transform.localPosition /= num;
			}
			if (particleSystem.isPlaying)
			{
				particleSystem.Stop();
				particleSystem.Play();
			}
		}
		TrailRenderer[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<TrailRenderer>(true);
		foreach (TrailRenderer trailRenderer in componentsInChildren2)
		{
			trailRenderer.startWidth *= num;
			trailRenderer.endWidth *= num;
		}
	}
}
