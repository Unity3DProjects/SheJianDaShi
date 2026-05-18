using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("2D Toolkit/Backend/tk2dFontData")]
public class tk2dFontData : MonoBehaviour
{
	public const int CURRENT_VERSION = 2;

	[HideInInspector]
	public int version;

	public float lineHeight;

	public tk2dFontChar[] chars;

	[SerializeField]
	private List<int> charDictKeys;

	[SerializeField]
	private List<tk2dFontChar> charDictValues;

	public string[] fontPlatforms;

	public string[] fontPlatformGUIDs;

	private tk2dFontData platformSpecificData;

	public bool hasPlatformData;

	public bool managedFont;

	public bool needMaterialInstance;

	public bool isPacked;

	public bool premultipliedAlpha;

	public Vector3 resultMeshCustomOffset = Vector3.zero;

	public tk2dSpriteCollectionData spriteCollection;

	private Dictionary<int, tk2dFontChar> charDict;

	public bool useDictionary;

	public tk2dFontKerning[] kerning;

	public float largestWidth;

	public Material material;

	[NonSerialized]
	public Material materialInst;

	public Texture2D gradientTexture;

	public bool textureGradients;

	public int gradientCount = 1;

	public Vector2 texelSize;

	private static readonly tk2dFontChar emptyChar = new tk2dFontChar
	{
		p0 = Vector3.zero,
		p1 = Vector3.zero,
		uv0 = Vector3.zero,
		uv1 = Vector3.zero,
		gradientUv = new Vector2[4],
		advance = 0f,
		channel = 0
	};

	[HideInInspector]
	public float invOrthoSize = 1f;

	[HideInInspector]
	public float halfTargetHeight = 1f;

	public tk2dFontData inst
	{
		get
		{
			if (platformSpecificData == null || platformSpecificData.materialInst == null)
			{
				if (hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < fontPlatforms.Length; i++)
					{
						if (fontPlatforms[i] == currentPlatform)
						{
							text = fontPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						text = fontPlatformGUIDs[0];
					}
					platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dFontData>(text);
				}
				else
				{
					platformSpecificData = this;
				}
				platformSpecificData.Init();
			}
			return platformSpecificData;
		}
	}

	public Dictionary<int, tk2dFontChar> CharDict
	{
		get
		{
			if (useDictionary && charDict == null)
			{
				charDict = new Dictionary<int, tk2dFontChar>(charDictKeys.Count);
				for (int i = 0; i < charDictKeys.Count; i++)
				{
					charDict[charDictKeys[i]] = charDictValues[i];
				}
			}
			return charDict;
		}
	}

	private void Init()
	{
		if (needMaterialInstance)
		{
			if ((bool)spriteCollection)
			{
				tk2dSpriteCollectionData tk2dSpriteCollectionData2 = spriteCollection.inst;
				for (int i = 0; i < tk2dSpriteCollectionData2.materials.Length; i++)
				{
					if (tk2dSpriteCollectionData2.materials[i] == material)
					{
						materialInst = tk2dSpriteCollectionData2.materialInsts[i];
						break;
					}
				}
				if (materialInst == null)
				{
					CustomDebug.LogError("Fatal error - font from sprite collection is has an invalid material");
				}
			}
			else
			{
				materialInst = UnityEngine.Object.Instantiate(material);
				materialInst.hideFlags = HideFlags.DontSave;
			}
		}
		else
		{
			materialInst = material;
		}
	}

	public void ResetPlatformData()
	{
		if (hasPlatformData && (bool)platformSpecificData)
		{
			platformSpecificData = null;
		}
		materialInst = null;
	}

	private void OnDestroy()
	{
		if (needMaterialInstance && spriteCollection == null)
		{
			UnityEngine.Object.DestroyImmediate(materialInst);
		}
	}

	public tk2dFontChar GetCharForIndex(int charIndex, int defaultIndex)
	{
		int num = charIndex;
		if (useDictionary)
		{
			if (!CharDict.ContainsKey(num))
			{
				num = defaultIndex;
			}
			if (!CharDict.ContainsKey(num))
			{
				num = 32;
			}
			if (!CharDict.ContainsKey(num))
			{
				return emptyChar;
			}
			return CharDict[num];
		}
		if (chars == null || chars.Length == 0)
		{
			return emptyChar;
		}
		if (num < 0 || num >= chars.Length || chars[num] == null)
		{
			num = defaultIndex;
		}
		if (num < 0 || num >= chars.Length || chars[num] == null)
		{
			num = 32;
		}
		if (num < 0 || num >= chars.Length || chars[num] == null)
		{
			return emptyChar;
		}
		return chars[num];
	}

	public bool HasCharForIndex(int charIndex)
	{
		if (charIndex == 10 || charIndex == 13 || charIndex == 9)
		{
			return true;
		}
		if (useDictionary)
		{
			return CharDict != null && CharDict.ContainsKey(charIndex);
		}
		return chars != null && charIndex >= 0 && charIndex < chars.Length && chars[charIndex] != null;
	}

	public void SetDictionary(Dictionary<int, tk2dFontChar> dict)
	{
		chars = null;
		charDict = null;
		charDictKeys = new List<int>(dict.Keys);
		charDictValues = new List<tk2dFontChar>();
		for (int i = 0; i < charDictKeys.Count; i++)
		{
			charDictValues.Add(dict[charDictKeys[i]]);
		}
		useDictionary = true;
	}

	public void SetArray(tk2dFontChar[] arr)
	{
		chars = arr;
		charDict = null;
		useDictionary = false;
	}
}
