using System;
using UnityEngine;

[Serializable]
public class tmTextureDefenition
{
	public string textureName;

	public string textureGuid;

	public string assetGuid;

	public ulong originalTimestamp;

	public ulong platformTimestamp;

	public Rect uv;

	public Rect offset;

	public tmTextureDefenition Copy()
	{
		tmTextureDefenition tmTextureDefenition2 = new tmTextureDefenition();
		tmTextureDefenition2.textureName = textureName;
		tmTextureDefenition2.textureGuid = textureGuid;
		tmTextureDefenition2.assetGuid = assetGuid;
		tmTextureDefenition2.originalTimestamp = originalTimestamp;
		tmTextureDefenition2.platformTimestamp = platformTimestamp;
		tmTextureDefenition2.offset = offset;
		return tmTextureDefenition2;
	}
}
