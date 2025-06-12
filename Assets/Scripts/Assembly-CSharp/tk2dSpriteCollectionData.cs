using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using tk2dRuntime;
using Debug = UnityEngine.Debug;

[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollectionData")]
public class tk2dSpriteCollectionData : MonoBehaviour
{
	public const int CURRENT_VERSION = 3;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<tk2dSpriteCollectionData> m_OnSpriteCollectionInitialize;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<tk2dSpriteCollectionData> m_OnSpriteCollectionReset;

	public int version;

	public bool materialIdsValid;

	public bool needMaterialInstance;

	public tk2dSpriteDefinition[] spriteDefinitions;

	[SerializeField]
	private Dictionary<string, int> spriteNameLookupDict;

	public bool premultipliedAlpha;

	public Material material;

	public Material[] materials;

	[NonSerialized]
	public Material[] materialInsts;

	// [NonSerialized]
	public Texture2D[] textureInsts = new Texture2D[0];

	public Texture[] textures;

	public TextAsset[] pngTextures = new TextAsset[0];

	public string[] streamingTextures = new string[0];

	public int[] widthTextures = new int[0];

	public int[] heightTextures = new int[0];

	public int[] materialPngTextureId = new int[0];

	public FilterMode textureFilterMode = FilterMode.Bilinear;

	public bool textureMipMaps;

	public bool textureLoadAsync;

	public bool allowMultipleAtlases;

	public string spriteCollectionGUID;

	public string spriteCollectionName;

	public string assetName = string.Empty;

	public bool loadable;

	public float invOrthoSize = 1f;

	public float halfTargetHeight = 1f;

	public int buildKey;

	public string dataGuid = string.Empty;

	public bool managedSpriteCollection;

	public bool hasPlatformData;

	public string[] spriteCollectionPlatforms;

	public string[] spriteCollectionPlatformGUIDs;

	private tk2dSpriteCollectionData platformSpecificData;

	public static readonly string internalResourcePrefix = "tk2dInternal$.";

	private int retainCount;

	private HashSet<MonoBehaviour> renderRefs = new HashSet<MonoBehaviour>();

	public bool Transient { get; set; }

	public int Count
	{
		get
		{
			return inst.spriteDefinitions.Length;
		}
	}

	public tk2dSpriteDefinition FirstValidDefinition
	{
		get
		{
			tk2dSpriteDefinition[] array = inst.spriteDefinitions;
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition2 in array)
			{
				if (tk2dSpriteDefinition2.Valid)
				{
					return tk2dSpriteDefinition2;
				}
			}
			return null;
		}
	}

	public int FirstValidDefinitionIndex
	{
		get
		{
			tk2dSpriteCollectionData tk2dSpriteCollectionData2 = inst;
			for (int i = 0; i < tk2dSpriteCollectionData2.spriteDefinitions.Length; i++)
			{
				if (tk2dSpriteCollectionData2.spriteDefinitions[i].Valid)
				{
					return i;
				}
			}
			return -1;
		}
	}

	public tk2dSpriteCollectionData inst
	{
		get
		{
			if (platformSpecificData == null)
			{
				if (hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < spriteCollectionPlatforms.Length; i++)
					{
						if (spriteCollectionPlatforms[i] == currentPlatform)
						{
							text = spriteCollectionPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						CustomDebug.LogWarning("failed to find platform by guid!");
						text = spriteCollectionPlatformGUIDs[0];
					}
					platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dSpriteCollectionData>(text);
				}
				else
				{
					platformSpecificData = this;
				}
			}
			if (platformSpecificData != null)
			{
				platformSpecificData.Init();
			}
			return platformSpecificData;
		}
	}

	public int RetainCount
	{
		get
		{
			return retainCount;
		}
		private set
		{
			if (retainCount != 0 || value > 0)
			{
			}
			if (retainCount <= 0 || value == 0)
			{
			}
			retainCount = value;
		}
	}

	public static event Action<tk2dSpriteCollectionData> OnSpriteCollectionInitialize
	{
		add
		{
			Action<tk2dSpriteCollectionData> action = tk2dSpriteCollectionData.m_OnSpriteCollectionInitialize;
			Action<tk2dSpriteCollectionData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tk2dSpriteCollectionData.m_OnSpriteCollectionInitialize, (Action<tk2dSpriteCollectionData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dSpriteCollectionData> action = tk2dSpriteCollectionData.m_OnSpriteCollectionInitialize;
			Action<tk2dSpriteCollectionData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tk2dSpriteCollectionData.m_OnSpriteCollectionInitialize, (Action<tk2dSpriteCollectionData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<tk2dSpriteCollectionData> OnSpriteCollectionReset
	{
		add
		{
			Action<tk2dSpriteCollectionData> action = tk2dSpriteCollectionData.m_OnSpriteCollectionReset;
			Action<tk2dSpriteCollectionData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tk2dSpriteCollectionData.m_OnSpriteCollectionReset, (Action<tk2dSpriteCollectionData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<tk2dSpriteCollectionData> action = tk2dSpriteCollectionData.m_OnSpriteCollectionReset;
			Action<tk2dSpriteCollectionData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref tk2dSpriteCollectionData.m_OnSpriteCollectionReset, (Action<tk2dSpriteCollectionData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public int GetSpriteIdByName(string name)
	{
		return GetSpriteIdByName(name, 0);
	}

	public int GetSpriteIdByName(string name, int defaultValue)
	{
		inst.InitDictionary();
		int value = defaultValue;
		if (!inst.spriteNameLookupDict.TryGetValue(name, out value))
		{
			return defaultValue;
		}
		return value;
	}

	public tk2dSpriteDefinition GetSpriteDefinition(string name)
	{
		int spriteIdByName = GetSpriteIdByName(name, -1);
		if (spriteIdByName == -1)
		{
			return null;
		}
		return spriteDefinitions[spriteIdByName];
	}

	public tk2dSliceBorders GetSliceBordersForSrite(int spriteID)
	{
		return ScriptableSingleton<tk2dSliceBordersCollector>.Instance.GetBordersForID(spriteCollectionName + "+" + GetNameForSpriteID(spriteID));
	}

	public string GetNameForSpriteID(int spriteID)
	{
		return inst.spriteDefinitions[spriteID].name;
	}

	public void InitDictionary()
	{
		if (spriteNameLookupDict == null)
		{
			spriteNameLookupDict = new Dictionary<string, int>(spriteDefinitions.Length);
			for (int i = 0; i < spriteDefinitions.Length; i++)
			{
				spriteNameLookupDict[spriteDefinitions[i].name] = i;
			}
		}
	}

	public bool IsValidSpriteId(int id)
	{
		if (id < 0 || id >= inst.spriteDefinitions.Length)
		{
			return false;
		}
		return inst.spriteDefinitions[id].Valid;
	}

	public void InitMaterialIds()
	{
		if (inst.materialIdsValid)
		{
			return;
		}
		int num = -1;
		Dictionary<Material, int> dictionary = new Dictionary<Material, int>();
		for (int i = 0; i < inst.materials.Length; i++)
		{
			if (num == -1 && inst.materials[i] != null)
			{
				num = i;
			}
			dictionary[materials[i]] = i;
		}
		if (num == -1)
		{
			CustomDebug.LogError("Init material ids failed.");
			return;
		}
		tk2dSpriteDefinition[] array = inst.spriteDefinitions;
		foreach (tk2dSpriteDefinition tk2dSpriteDefinition2 in array)
		{
			if (!dictionary.TryGetValue(tk2dSpriteDefinition2.material, out tk2dSpriteDefinition2.materialId))
			{
				tk2dSpriteDefinition2.materialId = num;
			}
		}
		inst.materialIdsValid = true;
	}

	public void Init()
	{
		if (materialInsts != null)
		{
			return;
		}
		if (spriteDefinitions == null)
		{
			spriteDefinitions = new tk2dSpriteDefinition[0];
		}
		if (materials == null)
		{
			materials = new Material[0];
		}
		materialInsts = new Material[materials.Length];
		if (needMaterialInstance)
		{
			if (tk2dSystem.OverrideBuildMaterial)
			{
				for (int i = 0; i < materials.Length; i++)
				{
					materialInsts[i] = new Material(Shader.Find("tk2d/BlendVertexColor"));
				}
			}
			else
			{
				bool flag = false;
				// Debug.LogError($"尼玛{pngTextures.Length}");
				// foreach (var VARIABLE in pngTextures)
				// {
				// 	Debug.LogError($"尼玛pp:{VARIABLE.name}");
				// 	
				// }
				// if (gameObject.name == "GUIAtlas_A8@2x")
				// {
			    	// Debug.LogError(pngTextures.Length);
					
				// }
				if (pngTextures.Length >0)
				{
					
					flag = true;
					textureInsts = new Texture2D[pngTextures.Length];
					for (int j = 0; j < pngTextures.Length; j++)
					{
						if (pngTextures[j] == null)
						{
							// Debug.LogError($"TextAsset引用为空：{gameObject.name}");
							continue;
							
						}
						 
						// Debug.LogError($"PngTexture:Load_{pngTextures[j].name}");
						Texture2D texture2D = new Texture2D(4, 4, TextureFormat.ARGB32, textureMipMaps);
						texture2D.name = string.Format("{0}PNG_{1}_{2}", internalResourcePrefix, base.name, j);
						texture2D.LoadImage(pngTextures[j].bytes);
						if (texture2D.LoadImage(pngTextures[j].bytes))
						{
							// Debug.LogError($"$成功读取：{pngTextures[j].name}");
						}
						else
						{
							// Debug.LogError($"$读取失败了：{pngTextures[j].name}");
						}
					
						texture2D.filterMode = textureFilterMode;
						texture2D.Apply(textureMipMaps, true);
						textureInsts[j] = texture2D;
						TextAsset assetToUnload = pngTextures[j];
						// pngTextures[j] = null;
						// Resources.UnloadAsset(assetToUnload);
					}
				}
				if (streamingTextures.Length > 0)
				{
					flag = true;
					textureInsts = new Texture2D[streamingTextures.Length];
					for (int k = 0; k < streamingTextures.Length; k++)
					{
						string imagePath = streamingTextures[k];
						int preloadWidth = ((widthTextures.Length != streamingTextures.Length) ? 4 : widthTextures[k]);
						int preloadHeight = ((heightTextures.Length != streamingTextures.Length) ? 4 : heightTextures[k]);
						Texture2D texture2D2 = ((!textureLoadAsync) ? TextureManager.LoadTexture(imagePath, textureMipMaps) : TextureManager.LoadTextureAsync(imagePath, textureMipMaps, preloadWidth, preloadHeight));
						texture2D2.name = string.Format("{0}PNG_{1}_{2}", internalResourcePrefix, base.name, k);
						// texture2D2.filterMode = textureFilterMode;
						texture2D2.filterMode = FilterMode.Bilinear;
						
						textureInsts[k] = texture2D2;
					}
				}
				for (int l = 0; l < materials.Length; l++)
				{
					materialInsts[l] = UnityEngine.Object.Instantiate(materials[l]);
					if (flag)
					{
						int num = ((materialPngTextureId.Length != 0) ? materialPngTextureId[l] : 0);
						materialInsts[l].mainTexture = textureInsts[num];
					}
				}
			}
			for (int m = 0; m < spriteDefinitions.Length; m++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition2 = spriteDefinitions[m];
				tk2dSpriteDefinition2.materialInst = materialInsts[tk2dSpriteDefinition2.materialId];
			}
		}
		else
		{
			for (int n = 0; n < materials.Length; n++)
			{
				materialInsts[n] = materials[n];
			}
			for (int num2 = 0; num2 < spriteDefinitions.Length; num2++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition3 = spriteDefinitions[num2];
				tk2dSpriteDefinition3.materialInst = tk2dSpriteDefinition3.material;
			}
		}
		if (CustomDebug.Enable && tk2dSpriteCollectionData.m_OnSpriteCollectionInitialize != null)
		{
			tk2dSpriteCollectionData.m_OnSpriteCollectionInitialize(this);
		}
		tk2dEditorSpriteDataUnloader.Register(this);
	}

	public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, string[] names, Rect[] regions, Vector2[] anchors)
	{
		return SpriteCollectionGenerator.CreateFromTexture(texture, size, names, regions, anchors);
	}

	public static tk2dSpriteCollectionData CreateFromTexturePacker(tk2dSpriteCollectionSize size, string texturePackerData, Texture texture)
	{
		return SpriteCollectionGenerator.CreateFromTexturePacker(size, texturePackerData, texture);
	}

	public void ResetPlatformData()
	{
		if (CustomDebug.Enable && tk2dSpriteCollectionData.m_OnSpriteCollectionReset != null)
		{
			tk2dSpriteCollectionData.m_OnSpriteCollectionReset(this);
		}
		tk2dEditorSpriteDataUnloader.Unregister(this);
		if (platformSpecificData != null)
		{
			platformSpecificData.DestroyTextureInsts();
			platformSpecificData.DestroyMaterialInsts();
		}
		DestroyTextureInsts();
		DestroyMaterialInsts();
		if ((bool)platformSpecificData)
		{
			platformSpecificData = null;
		}
		materialInsts = null;
	}

	private void DestroyTextureInsts()
	{
		if (textureInsts != null)
		{
			for (int i = 0; i < textureInsts.Length; i++)
			{
				Texture2D texture2D = textureInsts[i];
				if (texture2D != null)
				{
					//TODO:销毁--------关闭销毁
					Debug.LogWarning("卸载资源已注释");
					// TextureManager.UnloadTexture(texture2D);
					// UnityEngine.Object.Destroy(texture2D);
				}
				textureInsts[i] = null;
			}
		}
		textureInsts = new Texture2D[0];
	}

	private void DestroyMaterialInsts()
	{
		if (materialInsts != null)
		{
			for (int i = 0; i < materialInsts.Length; i++)
			{
				Material material = materialInsts[i];
				if (material != null)
				{
					UnityEngine.Object.Destroy(material);
				}
				materialInsts[i] = null;
			}
			materialInsts = null;
		}
		if (spriteDefinitions == null)
		{
			return;
		}
		for (int j = 0; j < spriteDefinitions.Length; j++)
		{
			tk2dSpriteDefinition tk2dSpriteDefinition2 = spriteDefinitions[j];
			if (tk2dSpriteDefinition2 != null)
			{
				tk2dSpriteDefinition2.materialInst = null;
			}
		}
	}

	private void OnDestroy()
	{
		ResetPlatformData();
	}

	public void AddRetainCount(MonoBehaviour sender)
	{
		if (renderRefs.Add(sender))
		{
			RetainCount++;
		}
	}

	public void RemoveRetainCount(MonoBehaviour sender)
	{
		if (renderRefs.Remove(sender))
		{
			RetainCount--;
		}
	}
}
