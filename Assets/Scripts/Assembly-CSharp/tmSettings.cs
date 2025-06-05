using UnityEngine;

public class tmSettings : ScriptableSingleton<tmSettings>
{
	public tmPlatform[] texturePlatforms = new tmPlatform[3]
	{
		new tmPlatform("Android HD", "@hd", 1.5f, tmUtility.NewGUID()),
		new tmPlatform("Android FullHD", "@fhd", 2.25f, tmUtility.NewGUID()),
		new tmPlatform("Android QuadHD", "@qhd", 3f, tmUtility.NewGUID())
	};

	public const string ResourceFolder = "tmResources/";

	public const string AssetsFolder = "Assets/Resources/tmResources/";

	public const string ResourceLinkPrefix = "tm";

	private tmPlatform currentPlatform;

	private tmPlatform lightmapPlatform;

	[SerializeField]
	private tmPlatform targetPlatform;

	[SerializeField]
	[Tooltip("used for autorescaling textures")]
	private tmPlatform defaultPlatform;

	public bool autoRebuild;

	[SerializeField]
	private bool forceStaticGeometry;

	public bool rebuildMesh;

	public bool batching;

	public bool isImmediateTextureLoadEnabled;

	public bool isAtlasesPreloadEnabled;

	public bool isAtlasesUnloadEnabled = true;

	public bool ForceStaticGeometry
	{
		get
		{
			return true;
		}
		set
		{
			forceStaticGeometry = value;
		}
	}

	public tmPlatform CurrentPlatform
	{
		get
		{
			if (currentPlatform == null || string.IsNullOrEmpty(currentPlatform.name))
			{
				currentPlatform = GetPlatformWithName("Android FullHD");
				CustomDebug.Log("texture platform : " + currentPlatform.name);
			}
			return currentPlatform;
		}
	}

	public tmPlatform LightmapPlatform
	{
		get
		{
			if (lightmapPlatform == null || string.IsNullOrEmpty(lightmapPlatform.name))
			{
				if (Screen.height < 1000)
				{
					lightmapPlatform = GetPlatformWithName("Android HD");
				}
				else if (Screen.height < 1400)
				{
					lightmapPlatform = GetPlatformWithName("Android FullHD");
				}
				else
				{
					lightmapPlatform = GetPlatformWithName("Android QuadHD");
				}
				CustomDebug.Log("lightmap platform : " + lightmapPlatform.name);
			}
			return lightmapPlatform;
		}
	}

	public tmPlatform TargetPlatform
	{
		get
		{
			return targetPlatform;
		}
		set
		{
			targetPlatform = value;
		}
	}

	public tmPlatform DefaultPlatform
	{
		get
		{
			return defaultPlatform;
		}
		set
		{
			defaultPlatform = value;
		}
	}

	public static tmPlatform[] allPlatfrorms
	{
		get
		{
			return ScriptableSingleton<tmSettings>.Instance.texturePlatforms;
		}
	}

	public static tmPlatform GetPlatformWithName(string platformName)
	{
		tmPlatform[] array = ScriptableSingleton<tmSettings>.Instance.texturePlatforms;
		foreach (tmPlatform tmPlatform2 in array)
		{
			if (tmPlatform2.name.Equals(platformName))
			{
				return tmPlatform2;
			}
		}
		return null;
	}
}
