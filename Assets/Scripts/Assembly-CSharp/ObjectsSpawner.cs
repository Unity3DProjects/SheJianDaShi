using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour
{
	private const string LAST_FISRT_CHARACTER = "First_Character";

	private const string LAST_SECOND_CHARACTER = "Second_Character";

	private const string LAST_TRY_CHARACTER = "Try_Character";

	private const string LAST_FISRT_CHARACTER_NAME_TYPE = "First_Character_NameType";

	private const string LAST_SECOND_CHARACTER_NAME_TYPE = "Second_Character_NameType";

	private const string LAST_TRY_CHARACTER_NAME_TYPE = "Try_Character_NameType";

	[SerializeField]
	private LevelObject[] objectsPrefabs;

	[SerializeField]
	private WeaponGeneral[] weaponsMainPrefabs;

	[SerializeField]
	private WeaponGeneral[] weaponsSecondaryPrefabs;

	[SerializeField]
	private CharacterBase[] charactersPrefabs;

	[SerializeField]
	private GUICharacter[] guiCharactersPrefabs;

	[SerializeField]
	private ShopItemCharacter[] shopItemCharactersPrefabs;

	[SerializeField]
	private Bird[] birdsPrefabs;

	[SerializeField]
	private GameObject[] groundDecals;

	[SerializeField]
	[Range(0f, 1f)]
	private float specialEnemyChance;

	[SerializeField]
	private List<CharacterNameType> specialEnemyCharacters;

	private Dictionary<LevelObjectType, LevelObject> objectsMap = new Dictionary<LevelObjectType, LevelObject>();

	private Dictionary<WeaponType, WeaponGeneral> weaponsMainMap = new Dictionary<WeaponType, WeaponGeneral>();

	private Dictionary<WeaponType, WeaponGeneral> weaponsSecondaryMap = new Dictionary<WeaponType, WeaponGeneral>();

	private List<ShopItemCharacter> characters;

	private List<CharacterNameType> charactersNameTypes;

	private Dictionary<int, Character> allCharacters;

	private Dictionary<int, GUICharacter> allGUICharacters;

	private Dictionary<int, ShopItemCharacter> allShopItemCharacters;

	private Dictionary<int, string> allCharactersLocalizedNames;

	private Dictionary<int, string> allCharactersLocalizedDescriptions;

	private Dictionary<int, string> allCharactersLocalizedWeaponDescriptions;

	private Managers cachedManagers;

	private bool isFirstcharacterWin;

	public CharacterBase FirstSelectedCharacter { get; private set; }

	public CharacterBase SecondSelectedCharacter { get; private set; }

	public CharacterNameType FirstSelectedCharacterNameType
	{
		get
		{
			CharacterNameType characterNameType = (CharacterNameType)CustomPlayerPrefs.GetInt("First_Character_NameType", 0);
			if (characterNameType == CharacterNameType.None)
			{
				ShopItemCharacter shopItemCharacter = CachedManagers.CachedShopManager.GetShopItemCharacter(FirstSelectedCharacterID);
				if (shopItemCharacter == null)
				{
					shopItemCharacter = CachedManagers.CachedShopManager.AvailableCharacters[0];
				}
				characterNameType = ((shopItemCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? shopItemCharacter.Infos[0].characterNameType : shopItemCharacter.CharacterUpgradeInfo.characterNameType);
			}
			return characterNameType;
		}
		private set
		{
			CustomPlayerPrefs.SetInt("First_Character_NameType", (int)value);
		}
	}

	public CharacterNameType SecondSelectedCharacterNameType
	{
		get
		{
			CharacterNameType characterNameType = (CharacterNameType)CustomPlayerPrefs.GetInt("Second_Character_NameType", 0);
			if (characterNameType == CharacterNameType.None)
			{
				ShopItemCharacter shopItemCharacter = CachedManagers.CachedShopManager.GetShopItemCharacter(SecondSelectedCharacterID);
				if (shopItemCharacter == null)
				{
					shopItemCharacter = CachedManagers.CachedShopManager.AvailableCharacters[0];
				}
				characterNameType = ((shopItemCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? shopItemCharacter.Infos[0].characterNameType : shopItemCharacter.CharacterUpgradeInfo.characterNameType);
			}
			return characterNameType;
		}
		private set
		{
			CustomPlayerPrefs.SetInt("Second_Character_NameType", (int)value);
		}
	}

	public CharacterNameType BeforeTrySelectedCharacterNameType
	{
		get
		{
			CharacterNameType characterNameType = (CharacterNameType)CustomPlayerPrefs.GetInt("Try_Character", 0);
			if (characterNameType == CharacterNameType.None)
			{
				ShopItemCharacter shopItemCharacter = CachedManagers.CachedShopManager.GetShopItemCharacter(BeforeTrySelectedCharacterID);
				if (shopItemCharacter == null)
				{
					shopItemCharacter = CachedManagers.CachedShopManager.AvailableCharacters[0];
				}
				characterNameType = ((shopItemCharacter.CharacterUpgradeInfo.upgradeLevel <= 0) ? shopItemCharacter.Infos[0].characterNameType : shopItemCharacter.CharacterUpgradeInfo.characterNameType);
			}
			return characterNameType;
		}
		set
		{
			CustomPlayerPrefs.SetInt("Try_Character_NameType", (int)value);
		}
	}

	public string FirstSelectedCharacterID
	{
		get
		{
			return CustomPlayerPrefs.GetString("First_Character");
		}
		private set
		{
			CustomPlayerPrefs.SetString("First_Character", value);
		}
	}

	public string SecondSelectedCharacterID
	{
		get
		{
			return CustomPlayerPrefs.GetString("Second_Character");
		}
		private set
		{
			CustomPlayerPrefs.SetString("Second_Character", value);
		}
	}

	public string BeforeTrySelectedCharacterID
	{
		get
		{
			return CustomPlayerPrefs.GetString("Try_Character");
		}
		set
		{
			CustomPlayerPrefs.SetString("Try_Character", value);
		}
	}

	private List<ShopItemCharacter> Characters
	{
		get
		{
			if (characters == null)
			{
				characters = SingletonMonoBehaviour<ShopManager>.Instance.AvailableCharacters;
			}
			return characters;
		}
	}

	private Managers CachedManagers
	{
		get
		{
			if (cachedManagers == null)
			{
				cachedManagers = SingletonMonoBehaviour<Managers>.Instance;
			}
			return cachedManagers;
		}
	}

	public bool IsFirstcharacterWin
	{
		get
		{
			return isFirstcharacterWin;
		}
		set
		{
			if (isFirstcharacterWin != value)
			{
				isFirstcharacterWin = value;
			}
		}
	}

	public CharacterNameType CharacterWinnerDoubleActiveType { get; set; }

	public CharacterNameType FirstCharacterDoubleType { get; set; }

	public CharacterNameType SecondCharacterDoubleType { get; set; }

	public List<CharacterNameType> CharactersNameTypes
	{
		get
		{
			if (charactersNameTypes == null)
			{
				charactersNameTypes = new List<CharacterNameType>();
				for (int i = 0; i < charactersPrefabs.Length; i++)
				{
					charactersNameTypes.Add(((Character)charactersPrefabs[i]).CharacterNameType);
				}
			}
			return charactersNameTypes;
		}
	}

	public Dictionary<int, Character> AllCharacters
	{
		get
		{
			if (allCharacters == null)
			{
				allCharacters = new Dictionary<int, Character>();
				for (int i = 0; i < charactersPrefabs.Length; i++)
				{
					allCharacters.Add((int)((Character)charactersPrefabs[i]).CharacterNameType, (Character)charactersPrefabs[i]);
				}
			}
			return allCharacters;
		}
	}

	public Dictionary<int, GUICharacter> AllGUICharacters
	{
		get
		{
			if (allGUICharacters == null)
			{
				allGUICharacters = new Dictionary<int, GUICharacter>();
				for (int i = 0; i < guiCharactersPrefabs.Length; i++)
				{
					for (int j = 0; j < guiCharactersPrefabs[i].CharacterNameTypes.Count; j++)
					{
						allGUICharacters.Add((int)guiCharactersPrefabs[i].CharacterNameTypes[j], guiCharactersPrefabs[i]);
					}
				}
			}
			return allGUICharacters;
		}
	}

	private Dictionary<int, ShopItemCharacter> AllShopItemCharacters
	{
		get
		{
			if (allShopItemCharacters == null)
			{
				allShopItemCharacters = new Dictionary<int, ShopItemCharacter>();
				for (int i = 0; i < shopItemCharactersPrefabs.Length; i++)
				{
					for (int j = 0; j < shopItemCharactersPrefabs[i].Infos.Count; j++)
					{
						allShopItemCharacters.Add((int)shopItemCharactersPrefabs[i].Infos[j].characterNameType, shopItemCharactersPrefabs[i]);
					}
				}
			}
			return allShopItemCharacters;
		}
	}

	private Dictionary<int, string> AllCharacterLocalizedNames
	{
		get
		{
			if (allCharactersLocalizedNames == null)
			{
				allCharactersLocalizedNames = new Dictionary<int, string>();
				for (int i = 0; i < shopItemCharactersPrefabs.Length; i++)
				{
					for (int j = 0; j < shopItemCharactersPrefabs[i].Infos.Count; j++)
					{
						allCharactersLocalizedNames.Add((int)shopItemCharactersPrefabs[i].Infos[j].characterNameType, shopItemCharactersPrefabs[i].Infos[j].localizedName);
					}
				}
			}
			return allCharactersLocalizedNames;
		}
	}

	private Dictionary<int, string> AllCharactersLocalizedDescriptions
	{
		get
		{
			if (allCharactersLocalizedDescriptions == null)
			{
				allCharactersLocalizedDescriptions = new Dictionary<int, string>();
				for (int i = 0; i < shopItemCharactersPrefabs.Length; i++)
				{
					for (int j = 0; j < shopItemCharactersPrefabs[i].Infos.Count; j++)
					{
						allCharactersLocalizedDescriptions.Add((int)shopItemCharactersPrefabs[i].Infos[j].characterNameType, shopItemCharactersPrefabs[i].Infos[j].localizedDescription);
					}
				}
			}
			return allCharactersLocalizedDescriptions;
		}
	}

	private Dictionary<int, string> AllCharactersLocalizedWeaponDescriptions
	{
		get
		{
			if (allCharactersLocalizedWeaponDescriptions == null)
			{
				allCharactersLocalizedWeaponDescriptions = new Dictionary<int, string>();
				for (int i = 0; i < shopItemCharactersPrefabs.Length; i++)
				{
					for (int j = 0; j < shopItemCharactersPrefabs[i].Infos.Count; j++)
					{
						allCharactersLocalizedWeaponDescriptions.Add((int)shopItemCharactersPrefabs[i].Infos[j].characterNameType, shopItemCharactersPrefabs[i].Infos[j].localizedWeaponDescription);
					}
				}
			}
			return allCharactersLocalizedWeaponDescriptions;
		}
	}

	public float SpecialEnemyChance
	{
		get
		{
			return specialEnemyChance;
		}
	}

	public List<CharacterNameType> SpecialEnemyCharacters
	{
		get
		{
			return specialEnemyCharacters;
		}
	}

	private LevelObject GetObject(LevelObjectType type)
	{
		LevelObject value;
		if (objectsMap.TryGetValue(type, out value))
		{
			return value;
		}
		throw new Exception("Missing object with TYPE : " + type);
	}

	private Bird GetBird()
	{
		return birdsPrefabs.RandomObject();
	}

	public void Init()
	{
		if (objectsPrefabs.Length != objectsMap.Count)
		{
			LevelObject[] array = objectsPrefabs;
			foreach (LevelObject levelObject in array)
			{
				if (!objectsMap.ContainsKey(levelObject.Type))
				{
					objectsMap.Add(levelObject.Type, levelObject);
				}
				else
				{
					Debug.LogError("Duplicate object type");
				}
			}
		}
		if (weaponsMainPrefabs.Length != weaponsMainMap.Count)
		{
			WeaponGeneral[] array2 = weaponsMainPrefabs;
			foreach (WeaponGeneral weaponGeneral in array2)
			{
				if (!weaponsMainMap.ContainsKey(weaponGeneral.WeaponType))
				{
					weaponsMainMap.Add(weaponGeneral.WeaponType, weaponGeneral);
				}
				else
				{
					Debug.LogError("Duplicate weapon type");
				}
			}
		}
		if (weaponsSecondaryPrefabs.Length == weaponsSecondaryMap.Count)
		{
			return;
		}
		WeaponGeneral[] array3 = weaponsSecondaryPrefabs;
		foreach (WeaponGeneral weaponGeneral2 in array3)
		{
			if (!weaponsSecondaryMap.ContainsKey(weaponGeneral2.WeaponType))
			{
				weaponsSecondaryMap.Add(weaponGeneral2.WeaponType, weaponGeneral2);
			}
			else
			{
				Debug.LogError("Duplicate weapon type");
			}
		}
	}

	public LevelObject CreateObject(LevelObjectType type)
	{
		GameObject original = GetObject(type).gameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
		return gameObject.GetComponent<LevelObject>();
	}

	public WeaponGeneral WeaponByType(WeaponType type)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(GetWeapon(type).gameObject);
		return gameObject.GetComponent<WeaponGeneral>();
	}

	public Bird CreateBird()
	{
		GameObject original = GetBird().gameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original);
		return gameObject.GetComponent<Bird>();
	}

	public CharacterBase RandomAppleBoyCharacter()
	{
		CharacterBase characterBase = null;
		do
		{
			characterBase = charactersPrefabs.RandomObject();
		}
		while (!characterBase.IsAvailableAsAppleBoy);
		return UnityEngine.Object.Instantiate(characterBase);
	}

	public void SetFirstSelectedCharacter(CharacterNameType characterNameType)
	{
		GUICharacter gUICharacter = GetGUICharacter(characterNameType);
		Character character = GetCharacter(characterNameType);
		ShopItemCharacter shopItemCharacter = GetShopItemCharacter(characterNameType);
		if (!character || !gUICharacter)
		{
			return;
		}
		if (gUICharacter.CharacterMode == CharacterMode.Random)
		{
			for (int i = 0; i < charactersPrefabs.Length; i++)
			{
				if (charactersPrefabs[i] == GetCharacter(gUICharacter.CurrentMultipleCharacterInfo.multipleCharacterType))
				{
					FirstSelectedCharacter = charactersPrefabs[i];
					FirstSelectedCharacterNameType = characterNameType;
					FirstSelectedCharacterID = shopItemCharacter.ProductID;
					FirstCharacterDoubleType = (charactersPrefabs[i] as Character).CharacterNameType;
					break;
				}
			}
			return;
		}
		for (int j = 0; j < charactersPrefabs.Length; j++)
		{
			if (charactersPrefabs[j] == GetCharacter(characterNameType))
			{
				FirstSelectedCharacter = charactersPrefabs[j];
				FirstSelectedCharacterNameType = characterNameType;
				FirstSelectedCharacterID = shopItemCharacter.ProductID;
				FirstCharacterDoubleType = (charactersPrefabs[j] as Character).CharacterNameType;
				break;
			}
		}
	}

	public void SetSecondSelectedCharacter(CharacterNameType characterNameType)
	{
		GUICharacter gUICharacter = GetGUICharacter(characterNameType);
		Character character = GetCharacter(characterNameType);
		if (!character || !gUICharacter)
		{
			return;
		}
		if (gUICharacter.CharacterMode == CharacterMode.Random)
		{
			for (int i = 0; i < charactersPrefabs.Length; i++)
			{
				if (charactersPrefabs[i] == GetCharacter(gUICharacter.CurrentMultipleCharacterInfo.multipleCharacterType))
				{
					SecondSelectedCharacter = charactersPrefabs[i];
					SecondSelectedCharacterNameType = characterNameType;
					SecondCharacterDoubleType = (charactersPrefabs[i] as Character).CharacterNameType;
					break;
				}
			}
			return;
		}
		for (int j = 0; j < charactersPrefabs.Length; j++)
		{
			if (charactersPrefabs[j] == GetCharacter(characterNameType))
			{
				SecondSelectedCharacter = charactersPrefabs[j];
				SecondSelectedCharacterNameType = characterNameType;
				SecondCharacterDoubleType = (charactersPrefabs[j] as Character).CharacterNameType;
				break;
			}
		}
	}

	public GameObject RandomGroundDecal()
	{
		GameObject original = groundDecals.RandomObject();
		return UnityEngine.Object.Instantiate(original);
	}

	public WeaponGeneral GetWeapon(WeaponType type)
	{
		GameType gameType = CachedManagers.CachedLevelsManager.CurrentLevel.GameType;
		WeaponGeneral value;
		if (gameType == GameType.BirdsHunt || gameType == GameType.AppleShoot)
		{
			if (weaponsSecondaryMap.TryGetValue(type, out value))
			{
				return value;
			}
		}
		else if (weaponsMainMap.TryGetValue(type, out value))
		{
			return value;
		}
		throw new Exception("Missing weapon with TYPE : " + type);
	}

	public ShopItemCharacter GetShopItemCharacter(CharacterNameType characterNameType)
	{
		if (AllShopItemCharacters.ContainsKey((int)characterNameType))
		{
			return AllShopItemCharacters[(int)characterNameType];
		}
		return null;
	}

	public Character GetCharacter(CharacterNameType characterNameType)
	{
		if (AllCharacters.ContainsKey((int)characterNameType))
		{
			return AllCharacters[(int)characterNameType];
		}
		return null;
	}

	public CharacterNameType CharacterNameTypeByProductID(string ID)
	{
		ShopItemCharacter shopItemCharacter = Characters.Find((ShopItemCharacter character) => character.ProductID == ID);
		CharacterNameType result = CharacterNameType.None;
		if (shopItemCharacter == null)
		{
			shopItemCharacter = Characters.Find((ShopItemCharacter character) => character.ProductID == CachedManagers.CachedShopManager.DefaultCharacterProductID);
		}
		if (shopItemCharacter.UpgradeLevel < shopItemCharacter.Infos.Count)
		{
			result = shopItemCharacter.Infos[shopItemCharacter.UpgradeLevel].characterNameType;
		}
		return result;
	}

	public GUICharacter GetGUICharacter(CharacterNameType characterNameType)
	{
		if (AllGUICharacters.ContainsKey((int)characterNameType))
		{
			return AllGUICharacters[(int)characterNameType];
		}
		return null;
	}

	public string GetCharacterLocalizedName(CharacterNameType characterNameType)
	{
		if (AllCharacterLocalizedNames.ContainsKey((int)characterNameType))
		{
			return CachedManagers.CachedLocalisationManager.GetTextByKey(AllCharacterLocalizedNames[(int)characterNameType]);
		}
		return string.Empty;
	}

	public string GetCharacterLocalizedDescription(CharacterNameType characterNameType)
	{
		if (AllCharactersLocalizedDescriptions.ContainsKey((int)characterNameType))
		{
			return CachedManagers.CachedLocalisationManager.GetTextByKey(AllCharactersLocalizedDescriptions[(int)characterNameType]);
		}
		return string.Empty;
	}

	public string GetCharacterLocalizedWeaponDescription(CharacterNameType characterNameType)
	{
		if (AllCharactersLocalizedWeaponDescriptions.ContainsKey((int)characterNameType))
		{
			return CachedManagers.CachedLocalisationManager.GetTextByKey(AllCharactersLocalizedWeaponDescriptions[(int)characterNameType]);
		}
		return string.Empty;
	}
}
