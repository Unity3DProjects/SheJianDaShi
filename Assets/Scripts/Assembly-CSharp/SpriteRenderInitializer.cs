using System.Collections;
using UnityEngine;
using tk2dRuntime;

[ExecuteInEditMode]
public class SpriteRenderInitializer : MonoBehaviour, ISpriteCollectionForceBuild
{
	private static Hashtable spritesCache = new Hashtable();

	private RectTransform cachedRectTransform;

	private Transform cachedTransfrom;

	private SpriteRenderer spriteRenderer;

	[SerializeField]
	protected int _spriteId;

	[SerializeField]
	private tk2dSpriteCollectionData collection;

	private tk2dSpriteCollectionData collectionInst;

	[SerializeField]
	private tk2dBaseSprite.Anchor anchorPoint = tk2dBaseSprite.Anchor.MiddleCenter;

	[SerializeField]
	private int targetPixelsPerMeter = 100;

	public RectTransform CachedRectTransform
	{
		get
		{
			if (cachedRectTransform == null)
			{
				cachedRectTransform = GetComponent<RectTransform>();
			}
			return cachedRectTransform;
		}
	}

	public Transform CachedTransfrom
	{
		get
		{
			if (cachedTransfrom == null)
			{
				cachedTransfrom = GetComponent<Transform>();
			}
			return cachedTransfrom;
		}
	}

	public SpriteRenderer SpriteRenderer
	{
		get
		{
			if (spriteRenderer == null)
			{
				spriteRenderer = GetComponent<SpriteRenderer>();
			}
			return spriteRenderer;
		}
	}

	public int SpriteID
	{
		get
		{
			return _spriteId;
		}
		set
		{
			if (value != _spriteId)
			{
				_spriteId = Mathf.Clamp(value, 0, CollectionInst.spriteDefinitions.Length - 1);
			}
		}
	}

	public string SpriteName
	{
		get
		{
			return CollectionInst.GetNameForSpriteID(SpriteID);
		}
	}

	public tk2dSpriteCollectionData Collection
	{
		get
		{
			return collection;
		}
		set
		{
			collection = value;
			collectionInst = collection.inst;
		}
	}

	protected tk2dSpriteCollectionData CollectionInst
	{
		get
		{
			if (collectionInst == null && collection != null)
			{
				collectionInst = collection.inst;
			}
			return collectionInst;
		}
	}

	public tk2dSpriteDefinition CurrentSprite
	{
		get
		{
			return (!(CollectionInst == null)) ? CollectionInst.spriteDefinitions[_spriteId] : null;
		}
	}

	public tk2dBaseSprite.Anchor AnchorPoint
	{
		get
		{
			return anchorPoint;
		}
		set
		{
			anchorPoint = value;
		}
	}

	public Vector2 AnchorPointVect2
	{
		get
		{
			Vector2 result = Vector2.zero;
			switch (anchorPoint)
			{
			case tk2dBaseSprite.Anchor.LowerCenter:
				result = new Vector2(0.5f, 0f);
				break;
			case tk2dBaseSprite.Anchor.LowerLeft:
				result = new Vector2(0f, 0f);
				break;
			case tk2dBaseSprite.Anchor.LowerRight:
				result = new Vector2(1f, 0f);
				break;
			case tk2dBaseSprite.Anchor.MiddleCenter:
				result = new Vector2(0.5f, 0.5f);
				break;
			case tk2dBaseSprite.Anchor.MiddleLeft:
				result = new Vector2(0f, 0.5f);
				break;
			case tk2dBaseSprite.Anchor.MiddleRight:
				result = new Vector2(1f, 0.5f);
				break;
			case tk2dBaseSprite.Anchor.UpperCenter:
				result = new Vector2(0.5f, 1f);
				break;
			case tk2dBaseSprite.Anchor.UpperLeft:
				result = new Vector2(0f, 1f);
				break;
			case tk2dBaseSprite.Anchor.UpperRight:
				result = new Vector2(1f, 1f);
				break;
			}
			return result;
		}
	}

	public int TargetPixelsPerMeter
	{
		get
		{
			return targetPixelsPerMeter;
		}
		set
		{
			targetPixelsPerMeter = value;
		}
	}

	public int PixelsPerMeter
	{
		get
		{
			int num = TargetPixelsPerMeter;
			if (tk2dSystem.IsRetina)
			{
				num *= 2;
			}
			return num;
		}
	}

	private void UpdateSpriteRenderer()
	{
		if (CurrentSprite != null && CurrentSprite.uvs.Length > 0 && CurrentSprite.boundsData.Length > 1)
		{
			Sprite sprite = null;
			if (spritesCache.ContainsKey(CurrentSprite) && spritesCache[CurrentSprite] != null && (bool)(Sprite)spritesCache[CurrentSprite] && Application.isPlaying)
			{
				sprite = spritesCache[CurrentSprite] as Sprite;
			}
			else
			{
				Texture2D texture2D = CurrentSprite.materialInst.mainTexture as Texture2D;
				Rect rect = new Rect((float)texture2D.width * CurrentSprite.uvs[0].x, (float)texture2D.height * CurrentSprite.uvs[0].y, CurrentSprite.boundsData[1].x, CurrentSprite.boundsData[1].y);
				Bounds untrimmedBounds = CurrentSprite.GetUntrimmedBounds();
				Bounds bounds = CurrentSprite.GetBounds();
				float x = untrimmedBounds.size.x * 0.5f - untrimmedBounds.center.x + bounds.center.x - bounds.size.x * 0.5f;
				float y = untrimmedBounds.size.y * 0.5f - untrimmedBounds.center.y + bounds.center.y - bounds.size.y * 0.5f;
				Vector2 vector = new Vector2(x, y);
				Vector2 vector2 = new Vector2(AnchorPointVect2.x * untrimmedBounds.size.x, AnchorPointVect2.y * untrimmedBounds.size.y) - vector;
				Vector2 pivot = new Vector2(vector2.x / bounds.size.x, vector2.y / bounds.size.y);
				sprite = Sprite.Create(texture2D, rect, pivot, PixelsPerMeter, 0u, SpriteMeshType.FullRect, Vector4.zero);
				spritesCache.Remove(CurrentSprite);
				spritesCache.Add(CurrentSprite, sprite);
			}
			if (SpriteRenderer != null)
			{
				SpriteRenderer.sprite = sprite;
			}
		}
		else
		{
			SpriteRenderer.sprite = null;
		}
	}

	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return Collection == spriteCollection;
	}

	public virtual void ForceBuild()
	{
		if (!(Collection == null))
		{
			collectionInst = null;
			UpdateSpriteRenderer();
		}
	}

	private void Awake()
	{
		if ((bool)Collection)
		{
			if (_spriteId < 0 || _spriteId >= Collection.Count)
			{
				_spriteId = 0;
			}
			ForceBuild();
		}
	}

	public void SetSprite(tk2dSpriteCollectionData newCollection, int newSpriteId, tk2dBaseSprite.Anchor anchor)
	{
		if (Collection != newCollection)
		{
			Collection = newCollection;
			_spriteId = -1;
		}
		SpriteID = newSpriteId;
		AnchorPoint = anchor;
		UpdateSpriteRenderer();
	}

	public void SetSprite(tk2dSpriteCollectionData newCollection, int newSpriteId)
	{
		SetSprite(newCollection, newSpriteId, AnchorPoint);
	}

	public bool SetSprite(tk2dSpriteCollectionData newCollection, string spriteName, tk2dBaseSprite.Anchor anchor)
	{
		int spriteIdByName = newCollection.GetSpriteIdByName(spriteName, -1);
		if (spriteIdByName != -1)
		{
			SetSprite(newCollection, spriteIdByName, anchor);
		}
		else
		{
			CustomDebug.LogError("SetSprite - Sprite not found in collection: " + spriteName);
		}
		return spriteIdByName != -1;
	}

	public bool SetSprite(tk2dSpriteCollectionData newCollection, string spriteName)
	{
		return SetSprite(newCollection, spriteName, AnchorPoint);
	}
}
