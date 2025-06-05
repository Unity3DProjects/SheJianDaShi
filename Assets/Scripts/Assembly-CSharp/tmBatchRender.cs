using System.Collections.Generic;
using UnityEngine;

public class tmBatchRender : tmTextureRenderBase
{
	private static Dictionary<Mesh, int> meshRetainCount = new Dictionary<Mesh, int>();

	public override void Rebuild()
	{
		UpdateMaterial();
		base.ModifiedFlag = (ModifiedFlags)0;
	}

	private static void RetainMesh(Mesh mesh)
	{
		if (!meshRetainCount.ContainsKey(mesh))
		{
			meshRetainCount.Add(mesh, 0);
		}
		meshRetainCount[mesh]++;
	}

	private static void ReleaseMesh(Mesh mesh)
	{
		if (meshRetainCount.ContainsKey(mesh) && --meshRetainCount[mesh] == 0)
		{
			Resources.UnloadAsset(mesh);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Mesh renderSharedMesh = base.RenderSharedMesh;
		if (renderSharedMesh != null && !renderSharedMesh.isReadable)
		{
			RetainMesh(renderSharedMesh);
		}
		base.MainTexCollection += (tmTextureRenderBase)this;
		base.LightmapCollection += (tmTextureRenderBase)this;
		base.ModifiedFlag |= ModifiedFlags.ModifiedMaterial;
	}

	protected override void OnDisable()
	{
		Mesh renderSharedMesh = base.RenderSharedMesh;
		if (renderSharedMesh != null && !renderSharedMesh.isReadable)
		{
			ReleaseMesh(renderSharedMesh);
		}
		base.MainTexCollection -= (tmTextureRenderBase)this;
		base.LightmapCollection -= (tmTextureRenderBase)this;
		base.OnDisable();
	}
}
