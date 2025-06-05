using UnityEngine;

[RequireComponent(typeof(tk2dSprite))]
public class MaskedSpine : MonoBehaviour
{
	private Material cachedMaterial;

	private MeshRenderer cachedRenderer;

	private tk2dSprite cachedSprite;

	private Transform cachedTransform;

	private MeshFilter cachedFilter;

	protected MaskSpine currentMask;

	public Transform CachedTransform
	{
		get
		{
			if (!cachedTransform)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public MeshRenderer CachedRenderer
	{
		get
		{
			if (!cachedRenderer)
			{
				cachedRenderer = GetComponent<MeshRenderer>();
			}
			return cachedRenderer;
		}
	}

	public Material CachedMaterial
	{
		get
		{
			if ((bool)CachedRenderer)
			{
				if (Application.isPlaying)
				{
					cachedMaterial = CachedRenderer.material;
				}
				else
				{
					cachedMaterial = CachedRenderer.sharedMaterial;
				}
			}
			return cachedMaterial;
		}
	}

	public MeshFilter CachedFilter
	{
		get
		{
			if (!cachedFilter)
			{
				cachedFilter = GetComponent<MeshFilter>();
			}
			return cachedFilter;
		}
	}

	public tk2dSprite CachedSprite
	{
		get
		{
			if (!cachedSprite)
			{
				cachedSprite = GetComponent<tk2dSprite>();
			}
			return cachedSprite;
		}
	}

	public Vector4 GetUVRect()
	{
		Texture mainTexture = CachedMaterial.mainTexture;
		Vector4 result = new Vector4(CachedSprite.CurrentSprite.uvs[0].x, CachedSprite.CurrentSprite.uvs[0].y, CachedSprite.CurrentSprite.boundsData[1].x / (float)mainTexture.width, CachedSprite.CurrentSprite.boundsData[1].y / (float)mainTexture.height);
		if (CachedSprite.CurrentSprite.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
		{
			result = new Vector4(CachedSprite.CurrentSprite.uvs[0].x, CachedSprite.CurrentSprite.uvs[0].y, CachedSprite.CurrentSprite.boundsData[1].y / (float)mainTexture.width, CachedSprite.CurrentSprite.boundsData[1].x / (float)mainTexture.height);
		}
		else if (CachedSprite.CurrentSprite.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
		{
			Debug.LogError(" not implemented ");
		}
		return result;
	}

	public Vector2[] GetVertices()
	{
		Bounds bounds = CachedSprite.GetBounds();
		Vector2 vector = new Vector2(bounds.min.x, bounds.min.y);
		Vector2 vector2 = new Vector2(bounds.min.x + bounds.size.x, bounds.min.y);
		Vector2 vector3 = new Vector2(bounds.min.x, bounds.min.y + bounds.size.y);
		Vector2[] array = new Vector2[3] { vector, vector2, vector3 };
		if (CachedSprite.CurrentSprite.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
		{
			Vector2 vector4 = array[1];
			array[1] = array[2];
			array[2] = vector4;
		}
		return array;
	}

	public void SetDepth(float maskDepth)
	{
		Vector3[] vertices = CachedFilter.mesh.vertices;
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i].z = maskDepth;
		}
		CachedFilter.mesh.vertices = vertices;
		CachedFilter.mesh.RecalculateBounds();
	}

	public Bounds GetBounds()
	{
		Bounds bounds = CachedSprite.GetBounds();
		return new Bounds
		{
			min = CachedTransform.TransformPoint(bounds.min),
			max = CachedTransform.TransformPoint(bounds.max)
		};
	}
}
