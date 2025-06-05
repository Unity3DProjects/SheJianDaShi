using System;
using Spine;
using UnityEngine;

public class SpriteCollectionAttachmentLoader : AttachmentLoader
{
	private tk2dSpriteCollectionData[] collections;

	private float u;

	private float v;

	private float u2;

	private float v2;

	private bool regionRotated;

	private float regionOriginalWidth;

	private float regionOriginalHeight;

	private float regionWidth;

	private float regionHeight;

	private float regionOffsetX;

	private float regionOffsetY;

	private Material material;

	public SpriteCollectionAttachmentLoader(tk2dSpriteCollectionData[] collections)
	{
		if (collections == null)
		{
			throw new ArgumentNullException("sprites cannot be null.");
		}
		this.collections = collections;
	}

	private void ProcessSpriteDefinition(string name)
	{
		int num = name.LastIndexOfAny(new char[2] { '/', '\\' });
		if (num != -1)
		{
			name = name.Substring(num + 1);
		}
		tk2dSpriteDefinition tk2dSpriteDefinition2 = FindSpriteDefinition(name);
		if (tk2dSpriteDefinition2 == null)
		{
			Debug.Log("Sprite not found : " + name);
			throw new Exception("Sprite not found in atlas: " + name);
		}
		if (tk2dSpriteDefinition2.complexGeometry)
		{
			throw new NotImplementedException("Complex geometry is not supported: " + name);
		}
		if (tk2dSpriteDefinition2.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
		{
			throw new NotImplementedException("Only 2D Toolkit atlases are supported: " + name);
		}
		Vector2 lhs = Vector2.one;
		Vector2 lhs2 = Vector2.zero;
		for (int i = 0; i < tk2dSpriteDefinition2.uvs.Length; i++)
		{
			Vector2 rhs = tk2dSpriteDefinition2.uvs[i];
			lhs = Vector2.Min(lhs, rhs);
			lhs2 = Vector2.Max(lhs2, rhs);
		}
		regionRotated = tk2dSpriteDefinition2.flipped == tk2dSpriteDefinition.FlipMode.Tk2d;
		if (regionRotated)
		{
			float x = lhs.x;
			lhs.x = lhs2.x;
			lhs2.x = x;
		}
		u = lhs.x;
		v = lhs2.y;
		u2 = lhs2.x;
		v2 = lhs.y;
		regionOriginalWidth = (int)(tk2dSpriteDefinition2.untrimmedBoundsData[1].x / tk2dSpriteDefinition2.texelSize.x);
		regionOriginalHeight = (int)(tk2dSpriteDefinition2.untrimmedBoundsData[1].y / tk2dSpriteDefinition2.texelSize.y);
		regionWidth = (int)(tk2dSpriteDefinition2.boundsData[1].x / tk2dSpriteDefinition2.texelSize.x);
		regionHeight = (int)(tk2dSpriteDefinition2.boundsData[1].y / tk2dSpriteDefinition2.texelSize.y);
		float num2 = tk2dSpriteDefinition2.untrimmedBoundsData[0].x - tk2dSpriteDefinition2.untrimmedBoundsData[1].x / 2f;
		float num3 = tk2dSpriteDefinition2.boundsData[0].x - tk2dSpriteDefinition2.boundsData[1].x / 2f;
		regionOffsetX = (int)((num3 - num2) / tk2dSpriteDefinition2.texelSize.x);
		float num4 = tk2dSpriteDefinition2.untrimmedBoundsData[0].y - tk2dSpriteDefinition2.untrimmedBoundsData[1].y / 2f;
		float num5 = tk2dSpriteDefinition2.boundsData[0].y - tk2dSpriteDefinition2.boundsData[1].y / 2f;
		regionOffsetY = (int)((num5 - num4) / tk2dSpriteDefinition2.texelSize.y);
		material = tk2dSpriteDefinition2.materialInst;
	}

	public RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
	{
		ProcessSpriteDefinition(path);
		RegionAttachment regionAttachment = new RegionAttachment(name);
		regionAttachment.Path = path;
		regionAttachment.RendererObject = material;
		regionAttachment.SetUVs(u, v, u2, v2, regionRotated);
		regionAttachment.RegionRotate = regionRotated;
		regionAttachment.RegionOriginalWidth = regionOriginalWidth;
		regionAttachment.RegionOriginalHeight = regionOriginalHeight;
		regionAttachment.RegionWidth = regionWidth;
		regionAttachment.RegionHeight = regionHeight;
		regionAttachment.RegionOffsetX = regionOffsetX;
		regionAttachment.RegionOffsetY = regionOffsetY;
		return regionAttachment;
	}

	public MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
	{
		ProcessSpriteDefinition(path);
		MeshAttachment meshAttachment = new MeshAttachment(name);
		meshAttachment.Path = path;
		meshAttachment.RendererObject = material;
		meshAttachment.RegionU = u;
		meshAttachment.RegionV = v;
		meshAttachment.RegionU2 = u2;
		meshAttachment.RegionV2 = v2;
		meshAttachment.RegionRotate = regionRotated;
		meshAttachment.RegionOriginalWidth = regionOriginalWidth;
		meshAttachment.RegionOriginalHeight = regionOriginalHeight;
		meshAttachment.RegionWidth = regionWidth;
		meshAttachment.RegionHeight = regionHeight;
		meshAttachment.RegionOffsetX = regionOffsetX;
		meshAttachment.RegionOffsetY = regionOffsetY;
		return meshAttachment;
	}

	public SkinnedMeshAttachment NewSkinnedMeshAttachment(Skin skin, string name, string path)
	{
		ProcessSpriteDefinition(path);
		SkinnedMeshAttachment skinnedMeshAttachment = new SkinnedMeshAttachment(name);
		skinnedMeshAttachment.Path = path;
		skinnedMeshAttachment.RendererObject = material;
		skinnedMeshAttachment.RegionU = u;
		skinnedMeshAttachment.RegionV = v;
		skinnedMeshAttachment.RegionU2 = u2;
		skinnedMeshAttachment.RegionV2 = v2;
		skinnedMeshAttachment.RegionRotate = regionRotated;
		skinnedMeshAttachment.RegionOriginalWidth = regionOriginalWidth;
		skinnedMeshAttachment.RegionOriginalHeight = regionOriginalHeight;
		skinnedMeshAttachment.RegionWidth = regionWidth;
		skinnedMeshAttachment.RegionHeight = regionHeight;
		skinnedMeshAttachment.RegionOffsetX = regionOffsetX;
		skinnedMeshAttachment.RegionOffsetY = regionOffsetY;
		return skinnedMeshAttachment;
	}

	public BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name)
	{
		return new BoundingBoxAttachment(name);
	}

	public tk2dSpriteDefinition FindSpriteDefinition(string name)
	{
		for (int i = 0; i < collections.Length; i++)
		{
			tk2dSpriteDefinition spriteDefinition = collections[i].inst.GetSpriteDefinition(name);
			if (spriteDefinition != null)
			{
				return spriteDefinition;
			}
		}
		return null;
	}
}
