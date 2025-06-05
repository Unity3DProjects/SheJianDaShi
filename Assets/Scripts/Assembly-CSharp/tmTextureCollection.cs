using System.Collections.Generic;
using UnityEngine;

public class tmTextureCollection : tmTextureCollectionBase
{
	[SerializeField]
	public List<tmTextureCollectionPlatformRef> platforms = new List<tmTextureCollectionPlatformRef>();
}
