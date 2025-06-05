using System;
using UnityEngine;
using tk2dRuntime;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("2D Toolkit/Text/tk2dTextMesh")]
public class tk2dTextMesh : tk2dBaseMesh, ISpriteCollectionForceBuild, ILayoutCellHandler
{
	[Flags]
	private enum UpdateFlags
	{
		UpdateNone = 0,
		UpdateAll = 1
	}

	[SerializeField]
	private tk2dTextMeshData data = new tk2dTextMeshData();

	private int currentNumberOfCharacters;

	private Vector3[] vertices = new Vector3[0];

	private Vector2[] uvs = new Vector2[0];

	private Vector2[] uv2 = new Vector2[0];

	private Color32[] colors = new Color32[0];

	private Color32[] untintedColors = new Color32[0];

	private int[] triangles = new int[0];

	private tk2dFontData defaultFontData;

	private UpdateFlags updateFlags;

	public tk2dTextMeshData Data
	{
		get
		{
			return data;
		}
	}

	public tk2dColoredText FormattedText
	{
		get
		{
			return data.FormattedText;
		}
	}

	public tk2dFontData font
	{
		get
		{
			return data.Font;
		}
		set
		{
			data.Font = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
			UpdateMaterial();
		}
	}

	public tk2dFontData FontInst
	{
		get
		{
			return data.FontInst;
		}
	}

	public string text
	{
		get
		{
			return data.Text;
		}
		set
		{
			data.Text = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public Color color
	{
		get
		{
			return data.color;
		}
		set
		{
			data.color = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public Color color2
	{
		get
		{
			return data.color2;
		}
		set
		{
			data.color2 = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public Color colorS
	{
		get
		{
			return data.colorS;
		}
		set
		{
			data.colorS = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public bool useGradient
	{
		get
		{
			return data.useGradient;
		}
		set
		{
			data.useGradient = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public TextAnchor anchor
	{
		get
		{
			return data.anchor;
		}
		set
		{
			data.anchor = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public Vector3 scale
	{
		get
		{
			return data.UserScale;
		}
		set
		{
			data.UserScale = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public bool kerning
	{
		get
		{
			return data.kerning;
		}
		set
		{
			data.kerning = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public int textureGradient
	{
		get
		{
			return data.textureGradient;
		}
		set
		{
			data.textureGradient = value;
			SetNeedUpdate(UpdateFlags.UpdateAll);
		}
	}

	public float Spacing
	{
		get
		{
			return data.Spacing;
		}
		set
		{
			if (data.Spacing != value)
			{
				data.Spacing = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public float LineSpacing
	{
		get
		{
			return data.LineSpacing;
		}
		set
		{
			if (data.LineSpacing != value)
			{
				data.LineSpacing = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public float MaxTextLength
	{
		get
		{
			return data.MaxTextLength;
		}
		set
		{
			if (data.MaxTextLength != value)
			{
				data.MaxTextLength = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public float MaxTextHeight
	{
		get
		{
			return data.MaxTextHeight;
		}
		set
		{
			if (data.MaxTextHeight != value)
			{
				data.MaxTextHeight = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public tk2dTextMeshData.LineBreakMode BreakMode
	{
		get
		{
			return data.BreakMode;
		}
		set
		{
			if (data.BreakMode != value)
			{
				data.BreakMode = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public bool ShouldScaleInCellSize
	{
		get
		{
			return data.ShouldScaleInCellSize;
		}
		set
		{
			if (data.ShouldScaleInCellSize != value)
			{
				data.ShouldScaleInCellSize = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public bool DisableNumberGrouping
	{
		get
		{
			return data.DisableNumberGrouping;
		}
		set
		{
			if (DisableNumberGrouping != value)
			{
				data.DisableNumberGrouping = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public bool UseShortNumbers
	{
		get
		{
			return data.UseShortNumbers;
		}
		set
		{
			if (UseShortNumbers != value)
			{
				data.UseShortNumbers = value;
				SetNeedUpdate(UpdateFlags.UpdateAll);
			}
		}
	}

	public int SortingOrder
	{
		get
		{
			return base.CachedRenderer.sortingOrder;
		}
		set
		{
			if (base.CachedRenderer.sortingOrder != value)
			{
				data.renderLayer = value;
				base.CachedRenderer.sortingOrder = value;
			}
		}
	}

	public tk2dFontData DefaultFontData
	{
		get
		{
			return defaultFontData;
		}
	}

	public Bounds CurrentMeshBounds
	{
		get
		{
			return data.EstimatedMeshBounds;
		}
	}

	public void RepositionForCell(LayoutCellInfo info)
	{
		MaxTextLength = info.cellRect.width;
		MaxTextHeight = info.cellRect.height;
		if (info.anchor == AnchorType.Top)
		{
			anchor = TextAnchor.UpperCenter;
		}
		else if (info.anchor == AnchorType.Bottom)
		{
			anchor = TextAnchor.LowerCenter;
		}
		else if (info.anchor == AnchorType.Center)
		{
			anchor = TextAnchor.MiddleCenter;
		}
		else if (info.anchor == AnchorType.Left)
		{
			anchor = TextAnchor.MiddleLeft;
		}
		else if (info.anchor == AnchorType.Right)
		{
			anchor = TextAnchor.MiddleRight;
		}
		if (info.anchor == AnchorType.Center)
		{
			base.transform.localPosition = new Vector3(0f, base.transform.localPosition.y, base.transform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Left)
		{
			base.transform.localPosition = new Vector3((0f - info.cellRect.width) * 0.5f, base.transform.localPosition.y, base.transform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Right)
		{
			base.transform.localPosition = new Vector3(info.cellRect.width * 0.5f, base.transform.localPosition.y, base.transform.localPosition.z);
		}
		if (info.anchor == AnchorType.Center)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, 0f, base.transform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Bottom)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, (0f - info.cellRect.height) * 0.5f, base.transform.localPosition.z);
		}
		else if (info.anchor == AnchorType.Top)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, info.cellRect.height * 0.5f, base.transform.localPosition.z);
		}
	}

	private void SetNeedUpdate(UpdateFlags uf)
	{
		if (updateFlags == UpdateFlags.UpdateNone)
		{
			updateFlags |= uf;
			tk2dUpdateManager.QueueCommit(this);
		}
		else
		{
			updateFlags |= uf;
		}
	}

	private void RebuildAll()
	{
		UpdateMaterial();
		updateFlags = UpdateFlags.UpdateNone;
		SetNeedUpdate(UpdateFlags.UpdateAll);
	}

	private void Awake()
	{
		if (font != null)
		{
			defaultFontData = font;
		}
		RebuildAll();
	}

	public Bounds GetEstimatedMeshBoundsForString(string str)
	{
		return data.GetEstimatedMeshBoundsForString(str);
	}

	protected override void RoundShiftChanged()
	{
		SetNeedUpdate(UpdateFlags.UpdateAll);
	}

	private static void SetVertices(Vector3[] curVertices, Vector2[] curUVs, Vector2[] curUV2s, Color32[] untintedColors, Vector3 meshCustomOffset, bool isRoundShiftDisabled, Vector3 totalShift, tk2dTextGeomGen.GeomData curGeomData)
	{
		curGeomData.SetTextMeshGeom(curVertices, curUVs, curUV2s, untintedColors, 0);
		for (int i = 0; i < curVertices.Length; i++)
		{
			if (!isRoundShiftDisabled)
			{
				curVertices[i] = curVertices[i].FloorVector() - totalShift;
			}
			if (Mathf.Abs(meshCustomOffset.x) > float.Epsilon || Mathf.Abs(meshCustomOffset.y) > float.Epsilon)
			{
				curVertices[i] += meshCustomOffset;
			}
		}
	}

	private void SetColors(Color32[] dest)
	{
		Color32 color = data.color;
		Color32 color2 = ((!data.useGradient) ? data.color : data.color2);
		for (int i = 0; i < dest.Length; i++)
		{
			Color32 color3 = ((i % 4 >= 2) ? color2 : color);
			byte b = (byte)(untintedColors[i].r * color3.r / 255);
			byte b2 = (byte)(untintedColors[i].g * color3.g / 255);
			byte b3 = (byte)(untintedColors[i].b * color3.b / 255);
			byte b4 = (byte)(untintedColors[i].a * color3.a / 255);
			if (FontInst.premultipliedAlpha)
			{
				b = (byte)(b * b4 / 255);
				b2 = (byte)(b2 * b4 / 255);
				b3 = (byte)(b3 * b4 / 255);
			}
			dest[i].r = b;
			dest[i].g = b2;
			dest[i].b = b3;
			dest[i].a = b4;
		}
	}

	public bool DoNotUse__CommitInternal()
	{
		if (FontInst == null)
		{
			return false;
		}
		bool flag = false;
		tk2dTextGeomGen.GeomData geomData = new tk2dTextGeomGen.GeomData(data, currentNumberOfCharacters);
		if (geomData.ReallocRequired)
		{
			int numVertices;
			int numIndices;
			geomData.GetTextMeshGeomDesc(out numVertices, out numIndices);
			if (vertices.Length != numVertices)
			{
				vertices = new Vector3[numVertices];
				uvs = new Vector2[numVertices];
				colors = new Color32[numVertices];
				untintedColors = new Color32[numVertices];
				if (FontInst.textureGradients)
				{
					uv2 = new Vector2[numVertices];
				}
				triangles = new int[numIndices];
			}
			currentNumberOfCharacters = geomData.RequiredAllocatedCharacters;
			geomData = new tk2dTextGeomGen.GeomData(data, currentNumberOfCharacters);
			geomData.SetTextMeshIndices(triangles, 0, 0);
			MeshClear();
			flag = true;
		}
		if ((updateFlags & UpdateFlags.UpdateAll) != UpdateFlags.UpdateNone || flag)
		{
			SetVertices(vertices, uvs, uv2, untintedColors, (!Application.isPlaying) ? data.Font.resultMeshCustomOffset : FontInst.resultMeshCustomOffset, isRoundShiftDisabled, base.TotalShift, geomData);
			if (!FontInst.isPacked)
			{
				SetColors(colors);
			}
			else
			{
				colors = untintedColors;
			}
			base.MeshVertices = vertices;
			base.MeshUV = uvs;
			if (font.textureGradients)
			{
				base.MeshUV2 = uv2;
			}
			base.MeshColors32 = colors;
			if (flag)
			{
				base.MeshTriangles = triangles;
			}
			MeshRecalculateBounds();
			base.MeshBounds = tk2dBaseSprite.AdjustedMeshBounds(base.MeshBounds, data.renderLayer);
		}
		updateFlags = UpdateFlags.UpdateNone;
		return true;
	}

	public Vector3[] LastLetterVertices()
	{
		Vector3[] array = new Vector3[4];
		for (int i = 0; i < 4; i++)
		{
			array[i] = base.MeshVertices[data.RequiredCharactersCount * 4 - 4 + i];
		}
		return array;
	}

	public void MakePixelPerfect()
	{
		float num = 1f;
		tk2dCamera tk2dCamera2 = tk2dCamera.CameraForLayer(base.gameObject.layer);
		if (tk2dCamera2 != null)
		{
			if (FontInst.version < 1)
			{
				CustomDebug.LogError("Need to rebuild font.");
			}
			float distance = base.transform.position.z - tk2dCamera2.transform.position.z;
			float num2 = FontInst.invOrthoSize * FontInst.halfTargetHeight;
			num = tk2dCamera2.GetSizeAtDistance(distance) * num2;
		}
		else if ((bool)Camera.main)
		{
			if (Camera.main.orthographic)
			{
				num = Camera.main.orthographicSize;
			}
			else
			{
				float zdist = base.transform.position.z - Camera.main.transform.position.z;
				num = tk2dPixelPerfectHelper.CalculateScaleForPerspectiveCamera(Camera.main.fieldOfView, zdist);
			}
			num *= FontInst.invOrthoSize;
		}
		scale = new Vector3(Mathf.Sign(scale.x) * num, Mathf.Sign(scale.y) * num, Mathf.Sign(scale.z) * num);
	}

	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		if (data.Font != null && data.Font.spriteCollection != null)
		{
			return data.Font.spriteCollection == spriteCollection;
		}
		return true;
	}

	private void UpdateMaterial()
	{
		if (FontInst != null)
		{
			base.CurrentMaterial = FontInst.materialInst;
		}
	}

	public void ForceBuild()
	{
		RebuildAll();
	}
}
