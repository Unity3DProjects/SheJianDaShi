using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
	[SerializeField]
	private FloorPartInfo[] partsInfo;

	[SerializeField]
	private tk2dSprite[] floorTextures;

	[SerializeField]
	private tk2dTiledSprite floorTiled;

	[Range(0f, 100f)]
	[SerializeField]
	private float randomPossibility = 20f;

	[SerializeField]
	private bool isDefaultFloorFlipped;

	[SerializeField]
	private bool isFloorFairRandomed;

	private FloorSettings settings;

	private float spriteScale = 1f;

	private float multiplier = 1f;

	private Vector2 floorTextureSize;

	private int currentHeight;

	private int currentStraightParts;

	private Dictionary<FloorPartType, tk2dSprite[]> parts;

	private FloorPartInfo previousPartInfo;

	private List<FloorPartInfo> possibleTypes = new List<FloorPartInfo>();

	private List<Rect> straightAreas = new List<Rect>();

	private List<Ground> floorParts = new List<Ground>();

	private List<Vector2> generatedFloor = new List<Vector2>();

	public List<Vector2> GeneratedFloor
	{
		get
		{
			return generatedFloor;
		}
	}

	protected Dictionary<FloorPartType, tk2dSprite[]> Parts
	{
		get
		{
			if (parts == null)
			{
				parts = new Dictionary<FloorPartType, tk2dSprite[]>();
				for (int i = 0; i < partsInfo.Length; i++)
				{
					parts.Add(partsInfo[i].type, partsInfo[i].parts);
				}
			}
			return parts;
		}
	}

	private void Awake()
	{
		multiplier = ((!tk2dSystem.IsRetina) ? 1f : 2f);
		if (tk2dSystem.IsRetina)
		{
			multiplier = 2f;
			spriteScale = 0.5f;
		}
		floorTextureSize = floorTextures[0].GetBounds().size * spriteScale;
	}

	public List<Vector2> GenerateFloorHeights(FloorSettings settings, float width)
	{
		if (tk2dSystem.IsRetina)
		{
			spriteScale = 0.5f;
		}
		this.settings = settings;
		currentHeight = Random.Range(this.settings.minHeight, this.settings.maxHeight + 1);
		if (generatedFloor == null)
		{
			generatedFloor = new List<Vector2>();
		}
		generatedFloor.Clear();
		generatedFloor.Add(new Vector2(currentHeight, 0f));
		float currentWidth = 0f;
		GenerateNextFloorPart(ref currentHeight, width, ref currentWidth);
		return generatedFloor;
	}

	public void GenerateFloor(Transform ground, FloorSettings settings, float width)
	{
		if (SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType != GameType.Multiplayer)
		{
			GenerateFloorHeights(settings, width);
		}
		else
		{
			this.settings = settings;
			generatedFloor = SingletonMonoBehaviour<MultiplayerManager>.Instance.MatchSettings.levelSettings.floorHeights;
		}
		currentHeight = (int)generatedFloor[0].x;
		float currentWidth = 0f;
		currentStraightParts = 0;
		CreateNextFloorPart(ground, ref currentHeight, width, ref currentWidth);
		float num = floorTextureSize.y * (float)this.settings.minHeight + 2f;
		int num2 = Mathf.CeilToInt(width / floorTextureSize.x);
		int num3 = 1;
		CreateWall(new Vector3(0.5f * floorTextureSize.x, num + floorTextureSize.y, 0f), ground);
		CreateWall(new Vector3((float)num2 * floorTextureSize.x, num + floorTextureSize.y, 0f), ground);
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num3; j++)
			{
				tk2dSprite tk2dSprite2 = Object.Instantiate(RandomFloorPart(floorTextures, randomPossibility));
				tk2dSprite2.scale *= spriteScale;
				tk2dSprite2.transform.parent = ground;
				tk2dSprite2.transform.localPosition = new Vector3(((float)i + 0.5f) * floorTextureSize.x, num - ((float)j + 1f) * floorTextureSize.y, 0f);
			}
		}
		if (floorTiled != null)
		{
			tk2dTiledSprite tk2dTiledSprite2 = Object.Instantiate(floorTiled);
			tk2dTiledSprite2.scale *= spriteScale;
			tk2dTiledSprite2.transform.parent = ground;
			tk2dTiledSprite2.transform.localPosition = new Vector3(0f, num - (float)num3 * floorTextureSize.y, 0f);
			tk2dTiledSprite2.dimensions = new Vector2(width / spriteScale, 12f * floorTextureSize.y * multiplier / spriteScale);
		}
	}

	public tk2dSprite RandomFloorPart(tk2dSprite[] possibleParts, float possibility)
	{
		tk2dSprite tk2dSprite2 = null;
		if (possibleParts.Length > 1 && possibility >= Random.Range(0f, 100f))
		{
			return possibleParts[Random.Range(1, possibleParts.Length)];
		}
		return (!isDefaultFloorFlipped) ? possibleParts[0] : possibleParts[Random.Range(0, 2)];
	}

	public Vector3 GroundPositionAtX(float position)
	{
		Vector3 result = Vector3.zero;
		foreach (Ground floorPart in floorParts)
		{
			float num = floorPart.GetComponent<tk2dSprite>().GetBounds().size.x * 0.5f;
			if (position >= floorPart.transform.position.x - num && position <= floorPart.transform.position.x + num)
			{
				result = floorPart.GroundPosition(position);
				break;
			}
		}
		return result;
	}

	public void RemoveColliderAtX(float position)
	{
		Vector3 zero = Vector3.zero;
		foreach (Ground floorPart in floorParts)
		{
			float num = floorPart.GetComponent<tk2dSprite>().GetBounds().size.x * 0.5f;
			if (position >= floorPart.transform.position.x - num && position <= floorPart.transform.position.x + num)
			{
				Collider[] components = floorPart.GetComponents<Collider>();
				Collider[] array = components;
				foreach (Collider collider in array)
				{
					collider.enabled = false;
				}
				break;
			}
		}
	}

	public List<float> GetFloorHeightsForRange(float startPosition, float endPosition)
	{
		List<Ground> list = floorParts.FindAll(delegate(Ground part)
		{
			float num = part.GetComponent<tk2dSprite>().GetBounds().size.x * 0.5f;
			return startPosition <= part.transform.position.x + num && endPosition >= part.transform.position.x - num;
		});
		List<float> list2 = new List<float>();
		foreach (Ground item in list)
		{
			foreach (Vector3 floorHeight in item.FloorHeights)
			{
				if (!list2.Contains(floorHeight.y))
				{
					list2.Add(floorHeight.y);
				}
			}
		}
		return list2;
	}

	public List<Vector3> GetHighChangePositionsForRange(float startPosition, float endPosition)
	{
		List<Ground> list = floorParts.FindAll(delegate(Ground part)
		{
			float num = part.GetComponent<tk2dSprite>().GetBounds().size.x * 0.5f;
			return startPosition <= part.transform.position.x + num && endPosition >= part.transform.position.x - num;
		});
		List<Vector3> list2 = new List<Vector3>();
		foreach (Ground item in list)
		{
			if (item.Type != FloorPartType.None && item.Type != FloorPartType.Straight)
			{
				Vector3 hightChangePosition = item.GetHightChangePosition();
				if (hightChangePosition.x > startPosition && hightChangePosition.x < endPosition)
				{
					list2.Add(hightChangePosition);
				}
			}
		}
		return list2;
	}

	public Vector3 GetCharacterPositionForRange(float startXPosition, float endXPosition)
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < floorParts.Count; i++)
		{
			Ground ground = floorParts[i];
			if (ground.Type == FloorPartType.Straight)
			{
				tk2dSprite component = ground.GetComponent<tk2dSprite>();
				float num = ((!(component == null)) ? (component.GetBounds().size.x * 0.5f) : 0f);
				float num2 = ground.transform.position.x - num;
				float num3 = ground.transform.position.x - num;
				if (startXPosition <= num2 && endXPosition >= num3)
				{
					list.Add(new Vector2(num2, num3));
				}
			}
		}
		Vector3 zero = Vector3.zero;
		if (list.Count > 0)
		{
			Vector2 vector = list.RandomObject();
			return GroundPositionAtX(Random.Range(vector.x, vector.y));
		}
		return GroundPositionAtX(Random.Range(startXPosition, endXPosition));
	}

	protected void CreateNextFloorPart(Transform ground, ref int height, float width, ref float currentWidth)
	{
		for (int i = 1; i < generatedFloor.Count; i++)
		{
			int num = height;
			int num2 = (int)(generatedFloor[i].x - generatedFloor[i - 1].x);
			height += num2;
			previousPartInfo = PartInfoByHeight(num2);
			if (previousPartInfo == null)
			{
				continue;
			}
			if (previousPartInfo.type != FloorPartType.SkewLeft && previousPartInfo.type != FloorPartType.SkewLeftSmall)
			{
				num = height;
			}
			tk2dSprite tk2dSprite2 = Object.Instantiate(Parts[previousPartInfo.type][(int)generatedFloor[i].y]);
			tk2dSprite2.scale *= spriteScale;
			tk2dSprite2.transform.parent = ground;
			tk2dSprite2.transform.localPosition = new Vector3(currentWidth + tk2dSprite2.GetBounds().size.x * 0.5f, floorTextureSize.y * (float)num);
			floorParts.Add(tk2dSprite2.GetComponent<Ground>());
			if (previousPartInfo.type == FloorPartType.Straight)
			{
				if (currentStraightParts <= 1)
				{
					straightAreas.Add(new Rect(tk2dSprite2.transform.position.x - tk2dSprite2.GetBounds().size.x * 0.5f, tk2dSprite2.transform.position.y + tk2dSprite2.GetBounds().size.y, tk2dSprite2.GetBounds().size.x, 0f));
				}
				else
				{
					Rect value = straightAreas[straightAreas.Count - 1];
					value.width += tk2dSprite2.GetBounds().size.x;
					straightAreas[straightAreas.Count - 1] = value;
				}
			}
			if (previousPartInfo.type == FloorPartType.SkewLeft || previousPartInfo.type == FloorPartType.SkewRight || previousPartInfo.type == FloorPartType.SkewLeftSmall || previousPartInfo.type == FloorPartType.SkewRightSmall)
			{
				int num3 = ((previousPartInfo.type != FloorPartType.SkewLeft && previousPartInfo.type != FloorPartType.SkewRight) ? 3 : 4);
				for (int j = 0; j < num - settings.minHeight; j++)
				{
					for (int k = 0; k < num3; k++)
					{
						tk2dSprite tk2dSprite3 = Object.Instantiate(RandomFloorPart(floorTextures, randomPossibility));
						tk2dSprite3.scale *= spriteScale;
						tk2dSprite3.transform.parent = ground;
						tk2dSprite3.transform.localPosition = new Vector3(currentWidth + tk2dSprite3.GetBounds().size.x * (0.5f + (float)k), tk2dSprite3.GetBounds().size.y * (float)(num - j - 1));
					}
				}
			}
			else if (previousPartInfo.type == FloorPartType.Straight)
			{
				for (int l = 0; l < num - settings.minHeight; l++)
				{
					tk2dSprite tk2dSprite4 = Object.Instantiate(RandomFloorPart(floorTextures, randomPossibility));
					tk2dSprite4.scale *= spriteScale;
					tk2dSprite4.transform.parent = ground;
					tk2dSprite4.transform.localPosition = new Vector3(currentWidth + tk2dSprite2.GetBounds().size.x * 0.5f, tk2dSprite4.GetBounds().size.y * (float)(num - l - 1));
				}
			}
			currentWidth += tk2dSprite2.GetBounds().size.x - 2f;
		}
	}

	protected void GenerateNextFloorPart(ref int height, float width, ref float currentWidth)
	{
		previousPartInfo = PossibleTypeInfo(ref height);
		int num = Parts[previousPartInfo.type].Length;
		int num2 = 0;
		if (isFloorFairRandomed)
		{
			num2 = Random.Range(0, num);
		}
		else if (num > 1 && randomPossibility >= Random.Range(0f, 100f))
		{
			num2 = Random.Range(1, num);
		}
		generatedFloor.Add(new Vector2(height, num2));
		currentWidth += Parts[previousPartInfo.type][num2].GetBounds().size.x * spriteScale - 2f;
		if (currentWidth < width)
		{
			GenerateNextFloorPart(ref height, width, ref currentWidth);
		}
	}

	protected FloorPartInfo PossibleTypeInfo(ref int height)
	{
		if (possibleTypes == null)
		{
			possibleTypes = new List<FloorPartInfo>();
		}
		possibleTypes.Clear();
		for (int i = 0; i < partsInfo.Length; i++)
		{
			int num = height + partsInfo[i].heightOffset;
			if (num > settings.maxHeight || num < settings.minHeight)
			{
				continue;
			}
			if (partsInfo[i].type == FloorPartType.Straight)
			{
				if (currentStraightParts < settings.maxStraightParts || (settings.minHeight == 0 && settings.maxHeight == 0))
				{
					possibleTypes.Add(partsInfo[i]);
				}
			}
			else if (previousPartInfo == null)
			{
				possibleTypes.Add(partsInfo[i]);
			}
			else if (currentStraightParts >= settings.minStraightParts && (previousPartInfo.type == FloorPartType.None || previousPartInfo.type == FloorPartType.Straight))
			{
				possibleTypes.Add(partsInfo[i]);
			}
		}
		FloorPartInfo floorPartInfo = possibleTypes.RandomObject();
		height += floorPartInfo.heightOffset;
		if (floorPartInfo.type == FloorPartType.Straight)
		{
			currentStraightParts++;
		}
		else
		{
			currentStraightParts = 0;
		}
		return floorPartInfo;
	}

	protected FloorPartInfo PartInfoByHeight(int height)
	{
		FloorPartInfo floorPartInfo = null;
		for (int i = 0; i < partsInfo.Length; i++)
		{
			if (partsInfo[i].heightOffset == height)
			{
				floorPartInfo = partsInfo[i];
				break;
			}
		}
		if (floorPartInfo.type == FloorPartType.Straight)
		{
			currentStraightParts++;
		}
		else
		{
			currentStraightParts = 0;
		}
		return floorPartInfo;
	}

	protected void CreateWall(Vector3 localPosition, Transform parent)
	{
		tk2dSprite tk2dSprite2 = Object.Instantiate(Parts[FloorPartType.Straight][0]);
		tk2dSprite2.scale *= spriteScale;
		tk2dSprite2.transform.parent = parent;
		tk2dSprite2.transform.localScale = new Vector3(1f, 50f, 1f);
		tk2dSprite2.transform.localPosition = localPosition;
		tk2dSprite2.GetComponent<MeshRenderer>().enabled = false;
		Ground component = tk2dSprite2.GetComponent<Ground>();
		if (component != null)
		{
			component.IsWallPart = true;
		}
	}
}
