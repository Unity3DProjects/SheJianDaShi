using UnityEngine;

public class tmParticleSystemRender : tmTextureRenderBase
{
	[SerializeField]
	private bool useRenderQueue;

	[SerializeField]
	private int renderQueue;

	public bool UseRenderQueue
	{
		get
		{
			return useRenderQueue;
		}
	}

	public int RenderQueue
	{
		get
		{
			return renderQueue;
		}
	}

	protected override void Awake()
	{
		base.MainTexCollection -= (tmTextureRenderBase)this;
		base.MainTexCollection += (tmTextureRenderBase)this;
		base.Awake();
	}

	protected override Material MaterialInstance(Material original, tmTextureCollectionPlatform mainCollection, tmTextureCollectionPlatform lightmapCollection)
	{
		string text = "mat" + original.GetHashCode();
		string text2 = string.Empty;
		if (mainCollection != null)
		{
			text += mainCollection.collectionGuid;
			text2 += mainCollection.name;
		}
		if (lightmapCollection != null)
		{
			text += lightmapCollection.collectionGuid;
			text2 += lightmapCollection.name;
		}
		text += ((!UseRenderQueue) ? base.Material.renderQueue : RenderQueue);
		Vector2 mainTextureOffset = Vector2.zero;
		Vector2 mainTextureScale = Vector2.zero;
		if (base.MainTextureDefenition != null)
		{
			Rect uv = base.MainTextureDefenition.uv;
			Rect offset = base.MainTextureDefenition.offset;
			uv.center += offset.center;
			uv.size += offset.size;
			mainTextureOffset = new Vector2(uv.x, uv.y);
			mainTextureScale = new Vector2(uv.width, uv.height);
		}
		string text3 = text;
		text = text3 + string.Empty + mainTextureOffset.x + mainTextureOffset.y + mainTextureScale.x + mainTextureScale.y;
		Material copy;
		if (tmManager.Instance.GetSharedMaterial(original, mainCollection, lightmapCollection, text, out copy))
		{
			copy.name = original.name + "_" + text2;
			copy.hideFlags = HideFlags.HideAndDontSave;
			copy.mainTextureOffset = mainTextureOffset;
			copy.mainTextureScale = mainTextureScale;
			if (UseRenderQueue)
			{
				copy.renderQueue = RenderQueue;
			}
		}
		return copy;
	}
}
