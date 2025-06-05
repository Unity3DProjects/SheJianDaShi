using UnityEngine;
using tk2dRuntime;

public class tk2dMySpriteFromTexture : MonoBehaviour
{
	public tk2dSpriteCollectionSize spriteCollectionSize = new tk2dSpriteCollectionSize();

	public tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.MiddleCenter;

	public bool rebuild = true;

	[SerializeField]
	private tk2dSpriteCollectionData spriteCollection;

	[SerializeField]
	private Texture texture;

	private tk2dBaseSprite sprite;

	public Material CollectionMaterial
	{
		get
		{
			if (spriteCollection != null)
			{
				return spriteCollection.spriteDefinitions[0].material;
			}
			return null;
		}
	}

	public Texture Texture
	{
		get
		{
			return texture;
		}
		set
		{
			if (texture != value)
			{
				texture = value;
				Build();
			}
		}
	}

	public tk2dBaseSprite Sprite
	{
		get
		{
			if (sprite == null)
			{
				sprite = GetComponent<tk2dBaseSprite>();
			}
			if (sprite == null)
			{
				CustomDebug.Log("tk2dSpriteFromTexture - Missing sprite object. Creating.");
				sprite = base.gameObject.AddComponent<tk2dSprite>();
			}
			return sprite;
		}
	}

	public bool IsSpriteCollectionExist
	{
		get
		{
			return spriteCollection != null;
		}
	}

	public tk2dSpriteCollectionData SpriteCollection
	{
		get
		{
			return spriteCollection;
		}
	}

	private void Awake()
	{
		CheckSprite();
	}

	private void Build()
	{
		if (texture != null)
		{
			DestroyInternal();
			GameObject gameObject = new GameObject("tk2dSpriteFromTexture - " + texture.name);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			Vector2 anchorOffset = tk2dSpriteGeomGen.GetAnchorOffset(anchor, texture.width, texture.height);
			spriteCollection = SpriteCollectionGenerator.CreateFromTexture(gameObject, texture, spriteCollectionSize, new Vector2(texture.width, texture.height), new string[1] { "unnamed" }, new Rect[1]
			{
				new Rect(0f, 0f, texture.width, texture.height)
			}, null, new Vector2[1] { anchorOffset }, new bool[1]);
			string spriteCollectionName = "SpriteFromTexture " + texture.name;
			spriteCollection.spriteCollectionName = spriteCollectionName;
			spriteCollection.spriteDefinitions[0].material.name = spriteCollectionName;
			spriteCollection.spriteDefinitions[0].material.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector;
			Sprite.SetSprite(spriteCollection, 0);
		}
	}

	private void DestroyInternal()
	{
		if (IsSpriteCollectionExist)
		{
			if (GetComponent<Renderer>() != null)
			{
				GetComponent<Renderer>().material = null;
			}
			Object.DestroyImmediate(spriteCollection.spriteDefinitions[0].material);
			Object.DestroyImmediate(spriteCollection.gameObject);
			spriteCollection = null;
		}
	}

	private void CheckSprite()
	{
		if (!IsSpriteCollectionExist)
		{
			Build();
		}
		if (Sprite.Collection == null)
		{
			Sprite.SetSprite(spriteCollection, 0);
		}
	}

	public void ForceBuild()
	{
		Build();
	}

	public void Clear()
	{
		DestroyInternal();
	}
}
