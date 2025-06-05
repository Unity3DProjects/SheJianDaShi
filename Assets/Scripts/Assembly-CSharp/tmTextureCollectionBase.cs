using System.Collections.Generic;
using UnityEngine;

public class tmTextureCollectionBase : MonoBehaviour
{
	[SerializeField]
	public List<tmTextureDefenition> textureDefenitions = new List<tmTextureDefenition>();

	[SerializeField]
	public string assetGuid;

	[SerializeField]
	public string collectionGuid;

	[SerializeField]
	public tmAtlasImportSettings importSettings;

	[SerializeField]
	public float forcedScale = -1f;

	[SerializeField]
	public bool useStreamingAssets;

	public tmTextureDefenition GetTextureDefenitionByID(string textureID)
	{
		return textureDefenitions.Find((tmTextureDefenition f) => f.textureGuid.Equals(textureID));
	}

	public tmTextureDefenition GetTextureDefenitionByName(string textureName)
	{
		return textureDefenitions.Find((tmTextureDefenition f) => f.textureName.Equals(textureName));
	}
}
