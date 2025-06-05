using System;
using UnityEngine;

[Serializable]
public class BatchedMesh
{
	public enum Type
	{
		EmptyGameObject = 0,
		Sprite = 1,
		TiledSprite = 2,
		FBXMesh = 3
	}

	public Type type;

	public string name = string.Empty;

	public int parentId = -1;

	public tk2dSpriteCollectionData spriteCollection;

	public int spriteId;

	public Material fbxMaterial;

	public Mesh fbxMesh;

	public Quaternion rotation = Quaternion.identity;

	public Vector3 position = Vector3.zero;

	public Vector3 localScale = Vector3.one;

	public Color color = Color.white;

	public Vector3 baseScale = Vector3.one;

	public int renderLayer;

	public Vector2 dimensions;

	public tk2dBaseSprite.Anchor anchor;

	public Matrix4x4 relativeMatrix = Matrix4x4.identity;

	public bool IsDrawn
	{
		get
		{
			return type != Type.EmptyGameObject;
		}
	}

	public BatchedMesh()
	{
		parentId = -1;
	}

	public tk2dSpriteDefinition GetSpriteDefinition()
	{
		if (spriteCollection != null && spriteId != -1)
		{
			return spriteCollection.inst.spriteDefinitions[spriteId];
		}
		return null;
	}
}
