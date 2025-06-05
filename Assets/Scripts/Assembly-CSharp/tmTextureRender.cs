using UnityEngine;

[RequireComponent(typeof(tmBatchObject))]
public class tmTextureRender : tmTextureRenderBase
{
	private tmBatchObject batchObject;

	public tmBatchObject BatchObject
	{
		get
		{
			if (batchObject == null)
			{
				batchObject = GetComponent(typeof(tmBatchObject)) as tmBatchObject;
				if (batchObject == null)
				{
					batchObject = base.gameObject.AddComponent<tmBatchObject>();
				}
			}
			return batchObject;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		base.MainTexCollection += (tmTextureRenderBase)this;
		base.LightmapCollection += (tmTextureRenderBase)this;
		base.ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
	}

	protected override void OnDisable()
	{
		BatchObject.Unbatch();
		base.MainTexCollection -= (tmTextureRenderBase)this;
		base.LightmapCollection -= (tmTextureRenderBase)this;
		base.OnDisable();
	}

	public override void Rebuild()
	{
		base.Rebuild();
		BatchObject.Rebuild();
	}
}
