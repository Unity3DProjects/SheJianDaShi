using UnityEngine;

public class LevelObject : MonoBehaviour
{
	[SerializeField]
	private LevelObjectType type;

	public LevelObjectType Type
	{
		get
		{
			return type;
		}
	}
}
