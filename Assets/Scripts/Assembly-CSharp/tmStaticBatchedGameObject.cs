using UnityEngine;

public class tmStaticBatchedGameObject : MonoBehaviour
{
	[SerializeField]
	[ResourceLink]
	private AssetLink staticLink;

	public AssetLink StaticLink
	{
		get
		{
			return staticLink;
		}
	}
}
