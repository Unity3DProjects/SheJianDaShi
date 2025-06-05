using System;
using UnityEngine;

[Serializable]
public class tk2dTextMeshData
{
	public enum LineBreakMode
	{
		ScaleDown = 0,
		LineBreak = 1,
		ThreeDots = 2,
		DoNothing = 3
	}

	[SerializeField]
	private tk2dFontData font;

	[SerializeField]
	private string text = string.Empty;

	[SerializeField]
	private Vector3 scale = Vector3.one;

	[SerializeField]
	private float spacing;

	private float maxTextLength;

	private float maxTextHeight;

	[SerializeField]
	private LineBreakMode breakMode;

	[SerializeField]
	private float lineSpacing;

	[SerializeField]
	private bool disableNumberGrouping;

	[SerializeField]
	private bool useShortNumbers;

	[SerializeField]
	public Color color = Color.white;

	[SerializeField]
	public Color color2 = Color.white;

	[SerializeField]
	public Color colorS = Color.white;

	[SerializeField]
	public bool useGradient;

	[SerializeField]
	public int textureGradient;

	[SerializeField]
	public TextAnchor anchor = TextAnchor.LowerLeft;

	[SerializeField]
	public int renderLayer;

	[SerializeField]
	public bool kerning;

	[SerializeField]
	public int prewarmedCharactersCount;

	[SerializeField]
	private bool shouldScaleInCellSize;

	private Vector3 lineBreakScale = Vector3.one;

	private tk2dColoredText formattedTextCache;

	public tk2dFontData Font
	{
		get
		{
			return font;
		}
		set
		{
			font = value;
			formattedTextCache = null;
		}
	}

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			if (text != value)
			{
				text = value;
				formattedTextCache = null;
			}
		}
	}

	public Vector3 UserScale
	{
		get
		{
			return scale;
		}
		set
		{
			if (scale != value)
			{
				scale = value;
				formattedTextCache = null;
			}
		}
	}

	public float LineSpacing
	{
		get
		{
			return lineSpacing;
		}
		set
		{
			lineSpacing = value;
		}
	}

	public int TextureGradient
	{
		get
		{
			return textureGradient;
		}
		set
		{
			textureGradient = value % Font.gradientCount;
		}
	}

	public float Spacing
	{
		get
		{
			return spacing * (float)((!tk2dSystem.IsRetina) ? 1 : 2);
		}
		set
		{
			if (spacing != value)
			{
				spacing = value;
				formattedTextCache = null;
			}
		}
	}

	public float MaxTextLength
	{
		get
		{
			return maxTextLength;
		}
		set
		{
			if (maxTextLength != value)
			{
				maxTextLength = value;
				formattedTextCache = null;
			}
		}
	}

	public float MaxTextHeight
	{
		get
		{
			return maxTextHeight;
		}
		set
		{
			if (maxTextHeight != value)
			{
				maxTextHeight = value;
				formattedTextCache = null;
			}
		}
	}

	public LineBreakMode BreakMode
	{
		get
		{
			return breakMode;
		}
		set
		{
			if (breakMode != value)
			{
				breakMode = value;
				formattedTextCache = null;
			}
		}
	}

	public bool ShouldScaleInCellSize
	{
		get
		{
			return shouldScaleInCellSize;
		}
		set
		{
			if (shouldScaleInCellSize != value)
			{
				shouldScaleInCellSize = value;
				formattedTextCache = null;
			}
		}
	}

	public bool DisableNumberGrouping
	{
		get
		{
			return disableNumberGrouping;
		}
		set
		{
			if (disableNumberGrouping != value)
			{
				disableNumberGrouping = value;
				formattedTextCache = null;
			}
		}
	}

	public bool UseShortNumbers
	{
		get
		{
			return useShortNumbers;
		}
		set
		{
			if (useShortNumbers != value)
			{
				useShortNumbers = value;
				formattedTextCache = null;
			}
		}
	}

	public tk2dFontData FontInst
	{
		get
		{
			if (Font != null)
			{
				return font.inst;
			}
			return null;
		}
	}

	public tk2dColoredText FormattedText
	{
		get
		{
			if (formattedTextCache == null)
			{
				formattedTextCache = FormatTextImpl(text, this);
			}
			return formattedTextCache;
		}
	}

	public int VisibleCharactersCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < FormattedText.Length; i++)
			{
				int num2 = FormattedText[i];
				if (FontInst.useDictionary)
				{
					if (!FontInst.CharDict.ContainsKey(num2))
					{
						num2 = 0;
					}
				}
				else if (num2 >= FontInst.chars.Length)
				{
					num2 = 0;
				}
				if (num2 != 10)
				{
					num++;
				}
			}
			return num;
		}
	}

	public int RequiredCharactersCount
	{
		get
		{
			return Math.Max(VisibleCharactersCount, prewarmedCharactersCount);
		}
	}

	public Bounds EstimatedMeshBounds
	{
		get
		{
			return GetEstimatedMeshBoundsForString(FormattedText);
		}
	}

	public float ActualLineSpaceHeight
	{
		get
		{
			return ActualLineHeight * (1f + lineSpacing / 100f);
		}
	}

	public float ActualLineHeight
	{
		get
		{
			return FontInst.lineHeight * TotalScale.y;
		}
	}

	public Vector3 TotalScale
	{
		get
		{
			Vector3 userScale = UserScale;
			userScale.Scale(lineBreakScale);
			return userScale;
		}
	}

	public Bounds GetEstimatedMeshBoundsForString(tk2dColoredText formattedText)
	{
		Vector2 meshDimensionsForString = this.GetMeshDimensionsForString(formattedText);
		float yAnchorForHeight = this.GetYAnchorForHeight(meshDimensionsForString.y);
		float xAnchorForWidth = this.GetXAnchorForWidth(meshDimensionsForString.x);
		float num = (FontInst.lineHeight + lineSpacing) * TotalScale.y;
		return new Bounds(new Vector3(xAnchorForWidth + meshDimensionsForString.x * 0.5f, yAnchorForHeight + meshDimensionsForString.y * 0.5f + num, 0f), Vector3.Scale(meshDimensionsForString, new Vector3(1f, -1f, 1f)));
	}

	public Bounds GetEstimatedMeshBoundsForString(string str)
	{
		return GetEstimatedMeshBoundsForString(new tk2dColoredText(str, Color.white));
	}

	private static tk2dColoredText FormatTextImpl(string txt, tk2dTextMeshData meshData)
	{
		string source = LocalisationManager.LocalizedStringOrSource(txt);
		tk2dColoredText tk2dColoredText2 = new tk2dColoredText(source, meshData.colorS);
		ConvertString(tk2dColoredText2, meshData);
		return tk2dColoredText2;
	}

	private static void ConvertString(tk2dColoredText formattedText, tk2dTextMeshData meshData)
	{
		if (meshData.UseShortNumbers)
		{
			formattedText.ApplyShortNumber();
		}
		if (!meshData.DisableNumberGrouping)
		{
			int num = 0;
			for (int num2 = formattedText.Length - 1; num2 >= 0; num2--)
			{
				char c = formattedText[num2];
				num = ((c >= '0' && c <= '9') ? (num + 1) : 0);
				if (num == 4)
				{
					formattedText.Insert(num2 + 1, new string('\u0011', 1));
					num = 1;
				}
			}
		}
		meshData.lineBreakScale = Vector3.one;
		Vector2 meshDimensionsForString = meshData.GetMeshDimensionsForString(formattedText);
		if (meshDimensionsForString.x > meshData.maxTextLength && meshData.maxTextLength > 0f)
		{
			switch (meshData.BreakMode)
			{
			case LineBreakMode.ScaleDown:
			{
				float num8 = meshData.maxTextLength / meshDimensionsForString.x;
				meshData.lineBreakScale.Set(num8, num8, num8);
				break;
			}
			case LineBreakMode.ThreeDots:
			{
				formattedText.Append("...");
				int num9 = formattedText.Length - 4;
				while (num9 >= 0 && meshData.GetMeshDimensionsForString(formattedText).x > meshData.MaxTextLength)
				{
					formattedText.Remove(num9);
					num9--;
				}
				break;
			}
			case LineBreakMode.LineBreak:
			{
				int num3 = 0;
				int num4 = 0;
				while (num4 < formattedText.Length - 1)
				{
					int num5 = formattedText.IndexOf(' ', num4 + 1);
					int num6 = ((num5 == -1) ? (formattedText.Length - 1) : num5);
					if (meshData.GetMeshDimensionsForString(formattedText, num3, num6 - num3 + 1).x > meshData.maxTextLength)
					{
						int num7 = ((num4 <= num3) ? num6 : num4);
						formattedText[num7] = '\n';
						num4 = (num3 = num7 + 1);
					}
					else
					{
						num4 = num6;
					}
				}
				break;
			}
			}
		}
		float num10 = Mathf.Abs(meshData.GetMeshDimensionsForString(formattedText).y);
		if (num10 > meshData.maxTextHeight && meshData.maxTextHeight > 0f)
		{
			float x = meshData.lineBreakScale.x;
			float val = meshData.maxTextHeight / num10;
			x = Math.Min(x, val);
			meshData.lineBreakScale.Set(x, x, x);
		}
		if (meshData.ShouldScaleInCellSize)
		{
			float num11 = Mathf.Min(meshData.maxTextLength / meshDimensionsForString.x, meshData.MaxTextHeight / Mathf.Abs(meshDimensionsForString.y));
			meshData.lineBreakScale.Set(num11, num11, num11);
		}
		formattedText.Replace('\u0011', ' ');
	}
}
