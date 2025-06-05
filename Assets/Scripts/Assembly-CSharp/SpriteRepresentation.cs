using UnityEngine;

public class SpriteRepresentation : MonoBehaviour
{
	public Vector3 SpriteScale
	{
		get
		{
			return (!tk2dSystem.IsRetina) ? Vector3.one : (Vector3.one * 0.5f);
		}
	}

	private void Start()
	{
		tk2dSprite[] componentsInChildren = GetComponentsInChildren<tk2dSprite>(true);
		foreach (tk2dSprite tk2dSprite2 in componentsInChildren)
		{
			tk2dSprite2.scale = SpriteScale;
		}
	}
}
