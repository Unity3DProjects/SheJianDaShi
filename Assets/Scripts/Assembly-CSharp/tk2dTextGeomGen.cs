using System;
using UnityEngine;

public static class tk2dTextGeomGen
{
	public class GeomData
	{
		private tk2dTextMeshData textMeshData;

		private int currentAllocatedCharacters;

		public int RequiredAllocatedCharacters
		{
			get
			{
				return Math.Max(currentAllocatedCharacters, textMeshData.RequiredCharactersCount);
			}
		}

		public bool ReallocRequired
		{
			get
			{
				return RequiredAllocatedCharacters != currentAllocatedCharacters;
			}
		}

		public tk2dTextMeshData TextMeshData
		{
			get
			{
				return textMeshData;
			}
		}

		public int CurrentAllocatedCharacters
		{
			get
			{
				return currentAllocatedCharacters;
			}
		}

		public GeomData(tk2dTextMeshData _textMeshData, int _currentAllocatedCharacters)
		{
			textMeshData = _textMeshData;
			currentAllocatedCharacters = _currentAllocatedCharacters;
		}
	}

	public class InlineStyler
	{
		public Color32 meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public Color32 meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public float meshGradientTexU;

		public int curGradientCount = 1;
	}

	private static readonly Color32[] channelSelectColors = new Color32[4]
	{
		new Color32(0, 0, byte.MaxValue, 0),
		new Color(0f, 255f, 0f, 0f),
		new Color(255f, 0f, 0f, 0f),
		new Color(0f, 0f, 0f, 255f)
	};

	public static Vector2 GetMeshDimensionsForString(this tk2dTextMeshData geomData, tk2dColoredText inputText, int startIdx, int count)
	{
		tk2dFontData fontInst = geomData.FontInst;
		float b = 0f;
		float num = 0f;
		float num2 = 0f;
		for (int i = startIdx; i < inputText.Length && i < startIdx + count; i++)
		{
			int num3 = inputText[i];
			if (num3 == 10)
			{
				b = Mathf.Max(num, b);
				num = 0f;
				num2 -= geomData.ActualLineSpaceHeight;
				continue;
			}
			tk2dFontChar charForIndex = fontInst.GetCharForIndex(num3, 0);
			num += (charForIndex.advance + geomData.Spacing) * geomData.TotalScale.x;
			if (!geomData.kerning || i >= inputText.Length - 1)
			{
				continue;
			}
			tk2dFontKerning[] kerning = fontInst.kerning;
			foreach (tk2dFontKerning tk2dFontKerning2 in kerning)
			{
				if (tk2dFontKerning2.c0 == inputText[i] && tk2dFontKerning2.c1 == inputText[i + 1])
				{
					num += tk2dFontKerning2.amount * geomData.TotalScale.x;
					break;
				}
			}
		}
		b = Mathf.Max(num, b);
		num2 -= geomData.ActualLineHeight;
		return new Vector2(b, num2);
	}

	public static Vector2 GetMeshDimensionsForString(this tk2dTextMeshData geomData, tk2dColoredText inputText)
	{
		return geomData.GetMeshDimensionsForString(inputText, 0, inputText.Length);
	}

	public static Vector2 GetMeshDimensionsForString(this tk2dTextMeshData geomData, string inputString)
	{
		return geomData.GetMeshDimensionsForString(new tk2dColoredText(inputString, Color.white));
	}

	public static float GetYAnchorForHeight(this tk2dTextMeshData geomData, float textHeight)
	{
		tk2dFontData fontInst = geomData.FontInst;
		float actualLineHeight = geomData.ActualLineHeight;
		switch (geomData.anchor)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.UpperCenter:
		case TextAnchor.UpperRight:
			return 0f - actualLineHeight;
		case TextAnchor.MiddleLeft:
		case TextAnchor.MiddleCenter:
		case TextAnchor.MiddleRight:
		{
			float num = (0f - textHeight) / 2f - actualLineHeight;
			if (fontInst.version >= 2)
			{
				float num2 = fontInst.texelSize.y * geomData.TotalScale.y;
				return Mathf.Floor(num / num2) * num2;
			}
			return num;
		}
		case TextAnchor.LowerLeft:
		case TextAnchor.LowerCenter:
		case TextAnchor.LowerRight:
			return 0f - textHeight - actualLineHeight;
		default:
			return 0f - actualLineHeight;
		}
	}

	public static float GetXAnchorForWidth(this tk2dTextMeshData geomData, float lineWidth)
	{
		tk2dFontData fontInst = geomData.FontInst;
		switch ((int)geomData.anchor % 3)
		{
		case 0:
			return 0f;
		case 1:
		{
			float num = (0f - lineWidth) / 2f;
			if (fontInst.version >= 2)
			{
				float num2 = fontInst.texelSize.x * geomData.TotalScale.x;
				return Mathf.Floor(num / num2) * num2;
			}
			return num;
		}
		case 2:
			return 0f - lineWidth;
		default:
			return 0f;
		}
	}

	private static void PostAlignTextData(Vector3[] pos, int offset, int targetStart, int targetEnd, float offsetX)
	{
		for (int i = targetStart * 4; i < targetEnd * 4; i++)
		{
			Vector3 vector = pos[offset + i];
			vector.x += offsetX;
			pos[offset + i] = vector;
		}
	}

	public static void GetTextMeshGeomDesc(this GeomData geomData, out int numVertices, out int numIndices)
	{
		numVertices = geomData.RequiredAllocatedCharacters * 4;
		numIndices = geomData.RequiredAllocatedCharacters * 6;
	}

	public static void SetTextMeshGeom(this GeomData geomData, Vector3[] pos, Vector2[] uv, Vector2[] uv2, Color32[] color, int offset)
	{
		tk2dTextMeshData textMeshData = geomData.TextMeshData;
		tk2dFontData fontInst = textMeshData.FontInst;
		tk2dColoredText formattedText = textMeshData.FormattedText;
		InlineStyler inlineStyler = new InlineStyler();
		inlineStyler.meshTopColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		inlineStyler.meshBottomColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		inlineStyler.meshGradientTexU = (float)textMeshData.textureGradient / (float)((fontInst.gradientCount <= 0) ? 1 : fontInst.gradientCount);
		inlineStyler.curGradientCount = fontInst.gradientCount;
		float yAnchorForHeight = textMeshData.GetYAnchorForHeight(textMeshData.GetMeshDimensionsForString(formattedText).y);
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < formattedText.Length; i++)
		{
			if (num3 >= geomData.CurrentAllocatedCharacters)
			{
				break;
			}
			formattedText.ApplyColorCommand(inlineStyler, i);
			int num5 = formattedText[i];
			tk2dFontChar charForIndex = fontInst.GetCharForIndex(num5, 0);
			if (num5 == 10)
			{
				float lineWidth = num;
				int targetEnd = num3;
				if (num4 != num3)
				{
					float xAnchorForWidth = textMeshData.GetXAnchorForWidth(lineWidth);
					PostAlignTextData(pos, offset, num4, targetEnd, xAnchorForWidth);
				}
				num4 = num3;
				num = 0f;
				num2 -= textMeshData.ActualLineSpaceHeight;
				continue;
			}
			pos[offset + num3 * 4] = new Vector3(num + charForIndex.p0.x * textMeshData.TotalScale.x, yAnchorForHeight + num2 + charForIndex.p0.y * textMeshData.TotalScale.y, 0f);
			pos[offset + num3 * 4 + 1] = new Vector3(num + charForIndex.p1.x * textMeshData.TotalScale.x, yAnchorForHeight + num2 + charForIndex.p0.y * textMeshData.TotalScale.y, 0f);
			pos[offset + num3 * 4 + 2] = new Vector3(num + charForIndex.p0.x * textMeshData.TotalScale.x, yAnchorForHeight + num2 + charForIndex.p1.y * textMeshData.TotalScale.y, 0f);
			pos[offset + num3 * 4 + 3] = new Vector3(num + charForIndex.p1.x * textMeshData.TotalScale.x, yAnchorForHeight + num2 + charForIndex.p1.y * textMeshData.TotalScale.y, 0f);
			if (charForIndex.flipped)
			{
				uv[offset + num3 * 4] = new Vector2(charForIndex.uv1.x, charForIndex.uv1.y);
				uv[offset + num3 * 4 + 1] = new Vector2(charForIndex.uv1.x, charForIndex.uv0.y);
				uv[offset + num3 * 4 + 2] = new Vector2(charForIndex.uv0.x, charForIndex.uv1.y);
				uv[offset + num3 * 4 + 3] = new Vector2(charForIndex.uv0.x, charForIndex.uv0.y);
			}
			else
			{
				uv[offset + num3 * 4] = new Vector2(charForIndex.uv0.x, charForIndex.uv0.y);
				uv[offset + num3 * 4 + 1] = new Vector2(charForIndex.uv1.x, charForIndex.uv0.y);
				uv[offset + num3 * 4 + 2] = new Vector2(charForIndex.uv0.x, charForIndex.uv1.y);
				uv[offset + num3 * 4 + 3] = new Vector2(charForIndex.uv1.x, charForIndex.uv1.y);
			}
			if (fontInst.textureGradients)
			{
				uv2[offset + num3 * 4] = charForIndex.gradientUv[0] + new Vector2(inlineStyler.meshGradientTexU, 0f);
				uv2[offset + num3 * 4 + 1] = charForIndex.gradientUv[1] + new Vector2(inlineStyler.meshGradientTexU, 0f);
				uv2[offset + num3 * 4 + 2] = charForIndex.gradientUv[2] + new Vector2(inlineStyler.meshGradientTexU, 0f);
				uv2[offset + num3 * 4 + 3] = charForIndex.gradientUv[3] + new Vector2(inlineStyler.meshGradientTexU, 0f);
			}
			if (fontInst.isPacked)
			{
				Color32 color2 = channelSelectColors[charForIndex.channel];
				color[offset + num3 * 4] = color2;
				color[offset + num3 * 4 + 1] = color2;
				color[offset + num3 * 4 + 2] = color2;
				color[offset + num3 * 4 + 3] = color2;
			}
			else
			{
				color[offset + num3 * 4] = inlineStyler.meshTopColor;
				color[offset + num3 * 4 + 1] = inlineStyler.meshTopColor;
				color[offset + num3 * 4 + 2] = inlineStyler.meshBottomColor;
				color[offset + num3 * 4 + 3] = inlineStyler.meshBottomColor;
			}
			num += (charForIndex.advance + textMeshData.Spacing) * textMeshData.TotalScale.x;
			if (textMeshData.kerning && i < formattedText.Length - 1)
			{
				tk2dFontKerning[] kerning = fontInst.kerning;
				foreach (tk2dFontKerning tk2dFontKerning2 in kerning)
				{
					if (tk2dFontKerning2.c0 == formattedText[i] && tk2dFontKerning2.c1 == formattedText[i + 1])
					{
						num += tk2dFontKerning2.amount * textMeshData.TotalScale.x;
						break;
					}
				}
			}
			num3++;
		}
		if (num4 != num3)
		{
			float lineWidth2 = num;
			int targetEnd2 = num3;
			float xAnchorForWidth2 = textMeshData.GetXAnchorForWidth(lineWidth2);
			PostAlignTextData(pos, offset, num4, targetEnd2, xAnchorForWidth2);
		}
		for (int k = num3; k < geomData.CurrentAllocatedCharacters; k++)
		{
			pos[offset + k * 4] = (pos[offset + k * 4 + 1] = (pos[offset + k * 4 + 2] = (pos[offset + k * 4 + 3] = Vector3.zero)));
			uv[offset + k * 4] = (uv[offset + k * 4 + 1] = (uv[offset + k * 4 + 2] = (uv[offset + k * 4 + 3] = Vector2.zero)));
			if (fontInst.textureGradients)
			{
				uv2[offset + k * 4] = (uv2[offset + k * 4 + 1] = (uv2[offset + k * 4 + 2] = (uv2[offset + k * 4 + 3] = Vector2.zero)));
			}
			color[offset + k * 4] = (color[offset + k * 4 + 1] = (color[offset + k * 4 + 2] = (color[offset + k * 4 + 3] = Color.clear)));
		}
	}

	public static void SetTextMeshIndices(this GeomData geomData, int[] indices, int offset, int vStart)
	{
		for (int i = 0; i < geomData.CurrentAllocatedCharacters; i++)
		{
			indices[offset + i * 6] = vStart + i * 4;
			indices[offset + i * 6 + 1] = vStart + i * 4 + 1;
			indices[offset + i * 6 + 2] = vStart + i * 4 + 3;
			indices[offset + i * 6 + 3] = vStart + i * 4 + 2;
			indices[offset + i * 6 + 4] = vStart + i * 4;
			indices[offset + i * 6 + 5] = vStart + i * 4 + 3;
		}
	}
}
