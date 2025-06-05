using UnityEngine;

public class CustomRenderQueue : MonoBehaviour
{
	private const string CUSTOM_RENDER_QUEUE_MATERIAL_SUBNAME = "Skeleton";

	private const int CUSTOM_RENDER_QUEUE = 2900;

	private void Start()
	{
		MeshRenderer component = GetComponent<MeshRenderer>();
		if (!(component != null))
		{
			return;
		}
		Material[] sharedMaterials = component.sharedMaterials;
		if (sharedMaterials == null)
		{
			return;
		}
		int i = 0;
		for (int num = sharedMaterials.Length; i < num; i++)
		{
			Material material = sharedMaterials[i];
			if (material != null && material.mainTexture.name.Contains("Skeleton"))
			{
				material.renderQueue = 2900;
			}
		}
	}
}
