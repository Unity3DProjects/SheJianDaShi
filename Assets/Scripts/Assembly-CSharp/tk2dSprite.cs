using UnityEngine;

[AddComponentMenu("2D Toolkit/Sprite/tk2dSprite")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class tk2dSprite : tk2dBaseSprite, ILayoutCellHandler
{
	private Vector3[] meshVertices;

	private Vector3[] meshNormals;

	private Vector4[] meshTangents;

	private Color32[] meshColors;

	public void RepositionForCell(LayoutCellInfo info)
	{
		Vector3 size = GetUntrimmedBounds().size;
		if (isLayoutSizeLocked)
		{
			if (info.anchor == AnchorType.Center)
			{
				base.CachedTransform.localPosition = new Vector3(0f, base.CachedTransform.localPosition.y, base.CachedTransform.localPosition.z);
			}
			else if (info.anchor == AnchorType.Left)
			{
				base.CachedTransform.localPosition = new Vector3((0f - info.cellRect.width + size.x) * 0.5f, base.CachedTransform.localPosition.y, base.CachedTransform.localPosition.z);
			}
			else if (info.anchor == AnchorType.Right)
			{
				base.CachedTransform.localPosition = new Vector3((info.cellRect.width - size.x) * 0.5f, base.CachedTransform.localPosition.y, base.CachedTransform.localPosition.z);
			}
			else if (info.anchor == AnchorType.Center)
			{
				base.CachedTransform.localPosition = new Vector3(base.CachedTransform.localPosition.x, 0f, base.CachedTransform.localPosition.z);
			}
			else if (info.anchor == AnchorType.Bottom)
			{
				base.CachedTransform.localPosition = new Vector3(base.CachedTransform.localPosition.x, (0f - info.cellRect.height + size.y) * 0.5f, base.CachedTransform.localPosition.z);
			}
			else if (info.anchor == AnchorType.Top)
			{
				base.CachedTransform.localPosition = new Vector3(base.CachedTransform.localPosition.x, (info.cellRect.height - size.y) * 0.5f, base.CachedTransform.localPosition.z);
			}
		}
		else
		{
			base.scale = Vector3.one;
			size = GetUntrimmedBounds().size;
			base.scale = new Vector3(_scale.x * info.cellRect.width / size.x, _scale.y * info.cellRect.height / size.y, _scale.z);
			base.CachedTransform.localPosition = new Vector3(0f, 0f, base.CachedTransform.localPosition.z);
		}
	}

	private new void Awake()
	{
		base.Awake();
		if ((bool)base.Collection)
		{
			if (_spriteId < 0 || _spriteId >= base.Collection.Count)
			{
				_spriteId = 0;
			}
			Build();
		}
	}

	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteId);
	}

	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteName);
	}

	public static GameObject CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
	{
		return tk2dBaseSprite.CreateFromTexture<tk2dSprite>(texture, size, region, anchor);
	}

	public static void PlaySpriteToFaceAnimation(tk2dSprite sprite)
	{
		PlaySpriteToFaceAnimation(sprite, Color.white);
	}

	public static void PlaySpriteToFaceAnimation(tk2dSprite sprite, Color c)
	{
		tk2dSprite spriteCopy = Object.Instantiate(sprite);
		spriteCopy.transform.parent = sprite.transform.parent;
		spriteCopy.transform.localPosition = new Vector3(0f, 0f, -5f);
		spriteCopy.transform.localScale = Vector3.one;
		spriteCopy.color = c;
		TweenScale tweenScale = TweenScale.SetScale(spriteCopy.gameObject, Vector3.one * 5f, 0.3f);
		tweenScale.BeginScale = Vector3.one;
		TweenColor tweenColor = TweenColor.SetColor(spriteCopy.gameObject, Color.clear, 0.3f);
		tweenColor.beginColor = Color.white;
		bool[] useChanelMask = tweenColor.useChanelMask;
		bool flag;
		tweenColor.useChanelMask[1] = (flag = (tweenColor.useChanelMask[2] = false));
		useChanelMask[0] = flag;
		tweenColor.SetOnFinishedDelegate(delegate
		{
			Object.Destroy(spriteCopy.gameObject);
		});
	}

	protected override void UpdateGeometry()
	{
		UpdateGeometryImpl();
	}

	protected override void UpdateColors()
	{
		UpdateColorsImpl();
	}

	protected override void UpdateVertices()
	{
		UpdateVerticesImpl();
	}

	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		meshVertices = new Vector3[currentSprite.positions.Length];
		meshColors = new Color32[currentSprite.positions.Length];
		meshNormals = new Vector3[0];
		meshTangents = new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			meshNormals = new Vector3[currentSprite.normals.Length];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			meshTangents = new Vector4[currentSprite.tangents.Length];
		}
		MeshClear();
		UpdateVerticesImpl();
		UpdateColorsImpl();
		base.MeshTriangles = currentSprite.indices;
		UpdateMaterial();
		CreateCollider();
	}

	protected void UpdateColorsImpl()
	{
		if (meshColors != null && meshColors.Length != 0)
		{
			SetColors(meshColors);
			base.MeshColors32 = meshColors;
		}
	}

	protected void UpdateVerticesImpl()
	{
		if (meshVertices != null && meshVertices.Length != 0 && !(base.CollectionInst == null))
		{
			tk2dSpriteDefinition currentSprite = base.CurrentSprite;
			if (currentSprite.normals.Length != meshNormals.Length)
			{
				meshNormals = ((currentSprite.normals == null || currentSprite.normals.Length <= 0) ? new Vector3[0] : new Vector3[currentSprite.normals.Length]);
			}
			if (currentSprite.tangents.Length != meshTangents.Length)
			{
				meshTangents = ((currentSprite.tangents == null || currentSprite.tangents.Length <= 0) ? new Vector4[0] : new Vector4[currentSprite.tangents.Length]);
			}
			SetPositions(meshVertices, meshNormals, meshTangents);
			base.MeshVertices = meshVertices;
			base.MeshNormals = meshNormals;
			base.MeshTangents = meshTangents;
			base.MeshUV = currentSprite.uvs;
			base.MeshTriangles = currentSprite.indices;
			base.MeshBounds = tk2dBaseSprite.AdjustedMeshBounds(GetBounds(), renderLayer);
		}
	}

	protected void UpdateGeometryImpl()
	{
		if (!(base.CollectionInst == null))
		{
			tk2dSpriteDefinition currentSprite = base.CurrentSprite;
			if (meshVertices == null || meshVertices.Length != currentSprite.positions.Length)
			{
				meshVertices = new Vector3[currentSprite.positions.Length];
				meshNormals = ((currentSprite.normals == null || currentSprite.normals.Length <= 0) ? new Vector3[0] : new Vector3[currentSprite.normals.Length]);
				meshTangents = ((currentSprite.tangents == null || currentSprite.tangents.Length <= 0) ? new Vector4[0] : new Vector4[currentSprite.tangents.Length]);
				meshColors = new Color32[currentSprite.positions.Length];
			}
			MeshClear();
			UpdateVerticesImpl();
			UpdateColorsImpl();
		}
	}

	protected override void UpdateMaterial()
	{
		base.CurrentMaterial = base.CurrentSprite.materialInst;
	}

	protected override int GetCurrentVertexCount()
	{
		if (meshVertices == null)
		{
			return 0;
		}
		return meshVertices.Length;
	}

	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		float num = 0.1f;
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3 b = new Vector3(Mathf.Abs(_scale.x), Mathf.Abs(_scale.y), Mathf.Abs(_scale.z));
		Vector3 vector = Vector3.Scale(currentSprite.untrimmedBoundsData[0], _scale) - 0.5f * Vector3.Scale(currentSprite.untrimmedBoundsData[1], b);
		Vector3 vector2 = Vector3.Scale(currentSprite.untrimmedBoundsData[1], b);
		Vector3 vector3 = vector2 + dMax - dMin;
		vector3.x /= currentSprite.untrimmedBoundsData[1].x;
		vector3.y /= currentSprite.untrimmedBoundsData[1].y;
		if (currentSprite.untrimmedBoundsData[1].x * vector3.x < currentSprite.texelSize.x * num && vector3.x < b.x)
		{
			dMin.x = 0f;
			vector3.x = b.x;
		}
		if (currentSprite.untrimmedBoundsData[1].y * vector3.y < currentSprite.texelSize.y * num && vector3.y < b.y)
		{
			dMin.y = 0f;
			vector3.y = b.y;
		}
		Vector2 vector4 = new Vector3((!Mathf.Approximately(b.x, 0f)) ? (vector3.x / b.x) : 0f, (!Mathf.Approximately(b.y, 0f)) ? (vector3.y / b.y) : 0f);
		Vector3 vector5 = new Vector3(vector.x * vector4.x, vector.y * vector4.y);
		Vector3 position = dMin + vector - vector5;
		position.z = 0f;
		base.transform.position = base.transform.TransformPoint(position);
		base.scale = new Vector3(_scale.x * vector4.x, _scale.y * vector4.y, _scale.z);
	}
}
