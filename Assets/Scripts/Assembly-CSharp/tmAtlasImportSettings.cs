using System;
using UnityEngine;

[Serializable]
public class tmAtlasImportSettings
{
	[SerializeField]
	public int border;

	[SerializeField]
	public bool generateMipMaps;

	[SerializeField]
	public TextureFormat format = TextureFormat.RGBA32;
}
