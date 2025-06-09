using System;
using System.Collections.Generic;
using System.Linq;
using Spine;
using UnityEngine;

public class GUICharacter : MonoBehaviour
{
	private const byte DISABLE_R = 90;

	private const byte DISABLE_G = 0;

	private const byte DISABLE_B = 100;

	private const float DISABLE_GRAYSCALE = 1f;

	private const string DISABLE_SHADER = "tk2d/BlendVertexColorGrayscaleColorize";

	private const string DISABLE_GRAYSCALE_PROPERTY = "_GrayscaleFactor";

	private const string ANIMATION_CHOOSE_HIDDEN = "hidden";

	private const string ANIMATION_CHOOSE_PICK = "pick";

	private const string ANIMATION_IDLE_POSE = "idle_fight";

	private const string ANIMATION_HIT = "hit";

	private const string ANIMATION_HIT_REACTION = "hit_reaction";

	private const string ANIMATION_IDLE_SHOP = "idle_shop";

	private const string ANIMATION_SKELETON_NAME = "Skeleton";

	private const string ANIMATION_SKELETON_BEEFY_NAME = "SkeletonBeefy";

	private const string ANIMATION_SKELETON_PLUMPY_NAME = "SkeletonPlumpy";

	public Action schedulAnimation;

	[SerializeField]
	private List<CharacterNameType> characterNameTypes;

	[SerializeField]
	private tk2dSprite crownSpritePrefab;

	[SerializeField]
	private tk2dSprite upgradeCrownSpritePrefab;

	[SerializeField]
	private tk2dSprite iconShop;

	[SerializeField]
	private string iconShopName;

	[SerializeField]
	private string iconPopupName;

	[SerializeField]
	private tk2dSprite checkSprite;

	[SerializeField]
	private tk2dSprite circleSprite;

	[SerializeField]
	private tk2dSprite newSprite;

	[SerializeField]
	private tk2dSprite freeSprite;

	[SerializeField]
	private tk2dSprite vipSprite;

	[SerializeField]
	private tk2dSprite choiceFirstSprite;

	[SerializeField]
	private tk2dSprite choiceSecondSprite;

	[SerializeField]
	private tk2dSprite choiceFirstAndSecondSprite;

	[SerializeField]
	private AnimationCurve scaleCircle;

	[SerializeField]
	private string iconAvatarName;

	[SerializeField]
	private string weaponIconName;

	[SerializeField]
	private SkeletonAnimation animationSkeleton;

	[SerializeField]
	private SkeletonDataAsset dataAssetSkeleton;

	[SerializeField]
	private CharacterMode characterMode;

	[SerializeField]
	private SkeletonAnimation animationIconChoose;

	[SerializeField]
	private tk2dSpriteCollectionData[] spriteCollectionsData;

	[SerializeField]
	private List<MultipleCharacterInfo> multipleCharacterData;

	[SerializeField]
	private tk2dSpriteCollectionData rouletteSpriteCollectionsData;

	[SerializeField]
	private string rouletteSpriteName;

	private Dictionary<ShopScrollView, tk2dSprite> shopIcon = new Dictionary<ShopScrollView, tk2dSprite>();

	private Dictionary<ShopScrollView, Transform> shopIconTransform = new Dictionary<ShopScrollView, Transform>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconCheck = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconNew = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconFree = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconVip = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, Transform> shopIconNewTransform = new Dictionary<tk2dSprite, Transform>();

	private Dictionary<tk2dSprite, Transform> shopIconFreeTransform = new Dictionary<tk2dSprite, Transform>();

	private Dictionary<tk2dSprite, Transform> shopIconVipTransform = new Dictionary<tk2dSprite, Transform>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconCircle = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, Transform> shopIconCircleTransform = new Dictionary<tk2dSprite, Transform>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconChoiceFirst = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, Transform> shopIconChoiceFirstTransform = new Dictionary<tk2dSprite, Transform>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconChoiceSecond = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, Transform> shopIconChoiceSecondTransform = new Dictionary<tk2dSprite, Transform>();

	private Dictionary<tk2dSprite, tk2dSprite> shopIconChoiceFirstAndSecond = new Dictionary<tk2dSprite, tk2dSprite>();

	private Dictionary<tk2dSprite, Transform> shopIconChoiceFirstAndSecondTransform = new Dictionary<tk2dSprite, Transform>();

	private Dictionary<tk2dSprite, SkeletonAnimation> shopIconChoose = new Dictionary<tk2dSprite, SkeletonAnimation>();

	private List<SkeletonAnimation> doubleAnimationAvatar = new List<SkeletonAnimation>();

	private Spine.Animation animationIdle;

	private Spine.Animation animationAnger;

	private Spine.Animation animationWin;

	private Spine.Animation animationLose;

	private Spine.Animation animationScare;

	private Spine.Animation animationDefault;

	private Spine.Animation animationStandUp;

	private Spine.Animation animationSpawn;

	private SkeletonData cachedSkeletonData;

	private bool isRunningRandomAnimations;

	private List<GameObject> currentEffects = new List<GameObject>();

	private Managers cachedManagers;

	private bool isAnimationNeededReset;

	private CharacterMenuAnimation currentCharacterMenuAnimation;

	private MultipleCharacterInfo currentMultipleCharacterInfo;

	private Character selectedMultipleCharacter;

	private GUIScreenManager cachedScreenManager;

	private ShopItemCharacter shopItem;

	public Transform[] ShopIconCheckTransform
	{
		get
		{
			Transform[] array = new Transform[shopIconCheck.Values.Count];
			int num = 0;
			foreach (tk2dSprite value in shopIconCheck.Values)
			{
				array[num] = value.transform;
				num++;
			}
			return array;
		}
	}

	public Transform[] ShopIconTransform
	{
		get
		{
			Transform[] array = new Transform[shopIcon.Values.Count];
			int num = 0;
			foreach (tk2dSprite value in shopIcon.Values)
			{
				array[num] = value.transform;
				num++;
			}
			return array;
		}
	}

	public tk2dSpriteCollectionData[] SpriteCollectionsData
	{
		get
		{
			return spriteCollectionsData;
		}
	}

	public string IconShopName
	{
		get
		{
			return iconShopName;
		}
	}

	public string IconPopupName
	{
		get
		{
			return iconPopupName;
		}
	}

	public string IconAvatarName
	{
		get
		{
			return iconAvatarName;
		}
	}

	public string WeaponIconName
	{
		get
		{
			return weaponIconName;
		}
	}

	public SkeletonDataAsset SkeletonAsset
	{
		get
		{
			if (dataAssetSkeleton != null)
			{
				return dataAssetSkeleton;
			}
			return animationSkeleton.SkeletonDataAsset;
		}
	}

	public Spine.Animation AnimationIdle
	{
		get
		{
			if (animationIdle == null && CachedSkeletonData != null)
			{
				animationIdle = CachedSkeletonData.FindAnimation("idle");
			}
			return animationIdle;
		}
	}

	public Spine.Animation AnimationAnger
	{
		get
		{
			if (animationAnger == null && CachedSkeletonData != null)
			{
				animationAnger = CachedSkeletonData.FindAnimation("anger");
			}
			return animationAnger;
		}
	}

	public Spine.Animation AnimationWin
	{
		get
		{
			if (animationWin == null && CachedSkeletonData != null)
			{
				animationWin = CachedSkeletonData.FindAnimation("win");
			}
			return animationWin;
		}
	}

	public Spine.Animation AnimationLose
	{
		get
		{
			if (animationLose == null && CachedSkeletonData != null)
			{
				animationLose = CachedSkeletonData.FindAnimation("lose");
			}
			return animationLose;
		}
	}

	public Spine.Animation AnimationScare
	{
		get
		{
			if (animationScare == null && CachedSkeletonData != null)
			{
				animationScare = CachedSkeletonData.FindAnimation("scare");
			}
			return animationScare;
		}
	}

	private Spine.Animation AnimationStandUp
	{
		get
		{
			if (animationStandUp == null && CachedSkeletonData != null)
			{
				animationStandUp = CachedSkeletonData.FindAnimation("stand_up");
			}
			return animationStandUp;
		}
	}

	private Spine.Animation AnimationSpawn
	{
		get
		{
			if (animationSpawn == null && CachedSkeletonData != null)
			{
				animationSpawn = CachedSkeletonData.FindAnimation("spawn");
			}
			return animationSpawn;
		}
	}

	private Spine.Animation AnimationDefault
	{
		get
		{
			if (animationDefault == null && CachedSkeletonData != null)
			{
				animationDefault = CachedSkeletonData.FindAnimation("default");
			}
			return animationDefault;
		}
	}

	private SkeletonData CachedSkeletonData
	{
		get
		{
			if (cachedSkeletonData == null)
			{
				if (CachedManagers.CachedLevelsManager.CurrentLevel != null && CharacterMode == CharacterMode.Random)
				{
					if (CurrentMultipleCharacterInfo.multipleDataAssetSkeleton != null)
					{
						cachedSkeletonData = CurrentMultipleCharacterInfo.multipleDataAssetSkeleton.GetSkeletonData(true);
					}
					else if (animationSkeleton != null && animationSkeleton.SkeletonDataAsset != null)
					{
						cachedSkeletonData = CurrentMultipleCharacterInfo.multipleAnimationSkeleton.SkeletonDataAsset.GetSkeletonData(true);
					}
				}
				else if (dataAssetSkeleton != null)
				{
					cachedSkeletonData = dataAssetSkeleton.GetSkeletonData(true);
				}
				else if (animationSkeleton != null && animationSkeleton.SkeletonDataAsset != null)
				{
					cachedSkeletonData = animationSkeleton.SkeletonDataAsset.GetSkeletonData(true);
				}
			}
			return cachedSkeletonData;
		}
	}

	public bool IsChoicedFirstPlayer { get; set; }

	public bool IsAnimationResetNeeded
	{
		get
		{
			return animationSkeleton != null || characterMode == CharacterMode.Random || characterMode == CharacterMode.Alternative;
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

	public CharacterMode CharacterMode
	{
		get
		{
			return characterMode;
		}
	}

	public bool IsMultipleCharacterActive { get; set; }

	public MultipleCharacterInfo CurrentMultipleCharacterInfo
	{
		get
		{
			return currentMultipleCharacterInfo;
		}
		set
		{
			currentMultipleCharacterInfo = value;
		}
	}

	public List<MultipleCharacterInfo> MultipleCharacterData
	{
		get
		{
			return multipleCharacterData;
		}
	}

	public CharacterNameType CurrentType { get; set; }

	private GUIScreenManager CachedScreenManager
	{
		get
		{
			if (cachedScreenManager == null)
			{
				cachedScreenManager = CachedManagers.CachedGUIScreenManager;
			}
			return cachedScreenManager;
		}
	}

	public tk2dSpriteCollectionData RouletteSpriteCollectionsData
	{
		get
		{
			return rouletteSpriteCollectionsData;
		}
	}

	public string RouletteSpriteName
	{
		get
		{
			return rouletteSpriteName;
		}
	}

	public List<CharacterNameType> CharacterNameTypes
	{
		get
		{
			return characterNameTypes;
		}
	}

	public void AddedIconShop(ShopScrollView scrollView, Vector3 cellPosition, bool isPortraitPositioning)
	{
		shopItem = CachedManagers.CachedLevelsManager.ObjectsSpawner.GetShopItemCharacter(characterNameTypes[0]);
		if (shopIcon.ContainsKey(scrollView))
		{
			return;
		}
		iconShop.SetSprite(iconShopName);
		tk2dSprite tk2dSprite2 = UnityEngine.Object.Instantiate(iconShop);
		tk2dSprite2.CachedTransform.parent = scrollView.contentContainer;
		tk2dSprite2.CachedTransform.localPosition = cellPosition;
		Material material = tk2dSprite2.CachedRenderer.material;
		Material material2 = new Material(Shader.Find("tk2d/BlendVertexColorGrayscaleColorize"));
		material2.CopyPropertiesFromMaterial(material);
		tk2dSprite2.CachedRenderer.material = material2;
		if (isPortraitPositioning)
		{
			tk2dSprite2.CachedRenderer.sortingOrder -= 11;
		}
		shopIcon.Add(scrollView, tk2dSprite2);
		shopIconTransform.Add(scrollView, tk2dSprite2.CachedTransform);
		tk2dSprite tk2dSprite3 = UnityEngine.Object.Instantiate(checkSprite);
		tk2dSprite3.CachedTransform.parent = tk2dSprite2.CachedTransform;
		Vector3 localPosition = checkSprite.CachedTransform.localPosition;
		localPosition *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
		tk2dSprite3.CachedTransform.localPosition = localPosition;
		if (isPortraitPositioning)
		{
			tk2dSprite3.CachedRenderer.sortingOrder -= 11;
		}
		shopIconCheck.Add(tk2dSprite2, tk2dSprite3);
		tk2dSprite tk2dSprite4 = ((shopItem.UpgradeInfo.Count == 0) ? null : upgradeCrownSpritePrefab);
		if (tk2dSprite4 != null)
		{
			tk2dSprite tk2dSprite5 = UnityEngine.Object.Instantiate(tk2dSprite4);
			tk2dSprite5.CachedTransform.parent = tk2dSprite2.CachedTransform;
			tk2dSprite5.CachedTransform.localPosition = tk2dSprite4.CachedTransform.localPosition * ((!tk2dSystem.IsRetina) ? 1f : 2f);
			Material material3 = new Material(Shader.Find("tk2d/BlendVertexColorGrayscaleColorize"));
			material3.mainTexture = tk2dSprite5.CurrentSprite.materialInst.mainTexture;
			tk2dSprite5.CachedRenderer.material = material3;
			if (isPortraitPositioning)
			{
				tk2dSprite5.CachedRenderer.sortingOrder -= 11;
			}
			if (shopItem.UpgradeLevel > 0)
			{
				Color black = Color.black;
				black.a = tk2dSprite5.color.a;
				tk2dSprite5.color = black;
				tk2dSprite5.CachedRenderer.material.SetFloat("_GrayscaleFactor", 0f);
			}
			else
			{
				Color color = new Color32(90, 0, 100, 0);
				color.a = tk2dSprite5.color.a;
				tk2dSprite5.color = color;
				tk2dSprite5.CachedRenderer.material.SetFloat("_GrayscaleFactor", 0.7f);
			}
		}
		tk2dSprite tk2dSprite6 = UnityEngine.Object.Instantiate(circleSprite);
		tk2dSprite6.CachedTransform.parent = tk2dSprite2.CachedTransform;
		tk2dSprite6.CachedTransform.localPosition = circleSprite.CachedTransform.localPosition;
		shopIconCircle.Add(tk2dSprite2, tk2dSprite6);
		if (isPortraitPositioning)
		{
			tk2dSprite6.CachedRenderer.sortingOrder -= 11;
		}
		shopIconCircleTransform.Add(tk2dSprite2, tk2dSprite6.CachedTransform);
		if (!shopItem.Bought)
		{
			if (CachedManagers.CachedShopManager.NewCharacters != null && CachedManagers.CachedShopManager.NewCharacters.Contains(shopItem))
			{
				tk2dSprite tk2dSprite7 = UnityEngine.Object.Instantiate(newSprite);
				tk2dSprite7.CachedTransform.parent = tk2dSprite2.transform;
				tk2dSprite7.CachedTransform.localPosition = new Vector3(45f, 45f, 0f);
				if (tk2dSystem.IsRetina)
				{
					tk2dSprite7.CachedTransform.localPosition *= 2f;
				}
				if (isPortraitPositioning)
				{
					tk2dSprite7.CachedRenderer.sortingOrder -= 11;
				}
				shopIconNew.Add(tk2dSprite2, tk2dSprite7);
				shopIconNewTransform.Add(tk2dSprite2, tk2dSprite7.CachedTransform);
			}
			else if (shopItem.IsInviteReward || shopItem.IsSubscribeReward || shopItem.IsOnlySubscribeReward || shopItem.LockedByAdsInfo.isLockedByAds)
			{
				tk2dSprite tk2dSprite8 = UnityEngine.Object.Instantiate(freeSprite);
				tk2dSprite8.CachedTransform.parent = tk2dSprite2.transform;
				tk2dSprite8.CachedTransform.localPosition = new Vector3(45f, 45f, 0f);
				if (tk2dSystem.IsRetina)
				{
					tk2dSprite8.CachedTransform.localPosition *= 2f;
				}
				if (isPortraitPositioning)
				{
					tk2dSprite8.CachedRenderer.sortingOrder -= 11;
				}
				shopIconFree.Add(tk2dSprite2, tk2dSprite8);
				shopIconFreeTransform.Add(tk2dSprite2, tk2dSprite8.CachedTransform);
			}
		}
		if (CachedManagers.CachedShopManager.VipPack.Characters.Contains(shopItem))
		{
			tk2dSprite tk2dSprite9 = UnityEngine.Object.Instantiate(vipSprite);
			tk2dSprite9.CachedTransform.parent = tk2dSprite2.transform;
			tk2dSprite9.CachedTransform.localPosition = new Vector3(-40f, 50f, 0f);
			if (tk2dSystem.IsRetina)
			{
				tk2dSprite9.CachedTransform.localPosition *= 2f;
			}
			if (isPortraitPositioning)
			{
				tk2dSprite9.CachedRenderer.sortingOrder -= 11;
			}
			shopIconVip.Add(tk2dSprite2, tk2dSprite9);
			shopIconVipTransform.Add(tk2dSprite2, tk2dSprite9.CachedTransform);
		}
		tk2dSprite tk2dSprite10 = UnityEngine.Object.Instantiate(choiceFirstSprite);
		tk2dSprite10.CachedTransform.parent = tk2dSprite2.transform;
		tk2dSprite10.CachedTransform.localPosition = choiceFirstSprite.CachedTransform.localPosition;
		if (isPortraitPositioning)
		{
			tk2dSprite10.CachedRenderer.sortingOrder -= 11;
		}
		shopIconChoiceFirst.Add(tk2dSprite2, tk2dSprite10);
		shopIconChoiceFirstTransform.Add(tk2dSprite2, tk2dSprite10.CachedTransform);
		tk2dSprite tk2dSprite11 = UnityEngine.Object.Instantiate(choiceSecondSprite);
		tk2dSprite11.CachedTransform.parent = tk2dSprite2.CachedTransform;
		tk2dSprite11.CachedTransform.localPosition = choiceSecondSprite.CachedTransform.localPosition;
		if (isPortraitPositioning)
		{
			tk2dSprite11.CachedRenderer.sortingOrder -= 11;
		}
		shopIconChoiceSecond.Add(tk2dSprite2, tk2dSprite11);
		shopIconChoiceSecondTransform.Add(tk2dSprite2, tk2dSprite11.CachedTransform);
		tk2dSprite tk2dSprite12 = UnityEngine.Object.Instantiate(choiceFirstAndSecondSprite);
		tk2dSprite12.CachedTransform.parent = tk2dSprite2.CachedTransform;
		tk2dSprite12.CachedTransform.localPosition = choiceFirstAndSecondSprite.CachedTransform.localPosition;
		if (isPortraitPositioning)
		{
			tk2dSprite12.CachedRenderer.sortingOrder -= 11;
		}
		shopIconChoiceFirstAndSecond.Add(tk2dSprite2, tk2dSprite12);
		shopIconChoiceFirstAndSecondTransform.Add(tk2dSprite2, tk2dSprite12.CachedTransform);
		SkeletonAnimation skeletonAnimation = UnityEngine.Object.Instantiate(animationIconChoose);
		skeletonAnimation.transform.parent = tk2dSprite2.CachedTransform;
		skeletonAnimation.transform.localPosition = animationIconChoose.transform.localPosition;
		skeletonAnimation.State.SetAnimation(0, "hidden", true);
		if (isPortraitPositioning)
		{
			skeletonAnimation.MeshRenderer.sortingOrder -= 11;
		}
		shopIconChoose.Add(tk2dSprite2, skeletonAnimation);
	}

	public void ClearAll()
	{
		shopIcon.Clear();
		shopIconTransform.Clear();
		shopIconCheck.Clear();
		shopIconCircle.Clear();
		shopIconCircleTransform.Clear();
		shopIconNew.Clear();
		shopIconNewTransform.Clear();
		shopIconVip.Clear();
		shopIconVipTransform.Clear();
		shopIconFree.Clear();
		shopIconFreeTransform.Clear();
		shopIconChoiceFirst.Clear();
		shopIconChoiceFirstTransform.Clear();
		shopIconChoiceSecond.Clear();
		shopIconChoiceSecondTransform.Clear();
		shopIconChoiceFirstAndSecond.Clear();
		shopIconChoiceFirstAndSecondTransform.Clear();
		shopIconChoose.Clear();
		ResetAnimationState();
	}

	public void AddedAvatar(bool isResetNeeded, SkeletonAnimation defaultAnimation)
	{
		GUIScreenType activeScreen = CachedScreenManager.ActiveScreen;
		StartSpecialIdleAnimations(false, activeScreen == GUIScreenType.Main);
		Dictionary<GUILayoutCell, SkeletonAnimation> dictionary = null;
		switch (activeScreen)
		{
		case GUIScreenType.Main:
			dictionary = CachedScreenManager.MenuScreen.MenuScreenScrollView.AnimationAvatar;
			doubleAnimationAvatar = CachedScreenManager.MenuScreen.MenuScreenScrollView.DoubleAnimationAvatar;
			break;
		case GUIScreenType.Result:
			dictionary = CachedScreenManager.ResultScreen.AnimationAvatar;
			break;
		}
		if (doubleAnimationAvatar.Count != 0)
		{
			for (int i = 0; i < doubleAnimationAvatar.Count; i++)
			{
				if (doubleAnimationAvatar[i] != null)
				{
					UnityEngine.Object.Destroy(doubleAnimationAvatar[i].gameObject);
					doubleAnimationAvatar[i] = null;
				}
			}
			doubleAnimationAvatar.Clear();
		}
		if (dictionary == null)
		{
			return;
		}
		List<GUILayoutCell> list = new List<GUILayoutCell>(dictionary.Keys);
		if (animationSkeleton != null || isResetNeeded)
		{
			foreach (GUILayoutCell item in list)
			{
				if (CharacterMode == CharacterMode.Random && activeScreen != GUIScreenType.Result)
				{
					List<SkeletonAnimation> list2 = new List<SkeletonAnimation>(AddDoubleCharacterAnimations(defaultAnimation, item.CachedTransform));
					for (int j = 0; j < list2.Count; j++)
					{
						doubleAnimationAvatar.Add(list2[j]);
						if (list2[j].SkeletonDataAsset == CurrentMultipleCharacterInfo.multipleDataAssetSkeleton)
						{
							list2[j].gameObject.SetActive(false);
							UnityEngine.Object.Destroy(dictionary[item].gameObject);
							dictionary[item] = list2[j];
							break;
						}
					}
				}
				else if (CharacterMode == CharacterMode.Random && activeScreen == GUIScreenType.Result)
				{
					for (int k = 0; k < MultipleCharacterData.Count; k++)
					{
						CharacterNameType characterWinnerDoubleActiveType = CachedManagers.CachedLevelsManager.ObjectsSpawner.CharacterWinnerDoubleActiveType;
						if (MultipleCharacterData[k].multipleCharacterType == characterWinnerDoubleActiveType)
						{
							SkeletonAnimation skeletonAnimation = CreateSkeletonAnimation(MultipleCharacterData[k].multipleAnimationSkeleton, defaultAnimation, MultipleCharacterData[k].multipleDataAssetSkeleton);
							skeletonAnimation.transform.parent = item.CachedTransform;
							skeletonAnimation.transform.localPosition = Vector3.zero;
							skeletonAnimation.transform.localScale = Vector3.one;
							skeletonAnimation.gameObject.SetActive(false);
							UnityEngine.Object.Destroy(dictionary[item].gameObject);
							dictionary[item] = skeletonAnimation;
							break;
						}
					}
				}
				else
				{
					SkeletonAnimation skeletonAnimation2 = AddCharacterAnimation(defaultAnimation, item.CachedTransform);
					skeletonAnimation2.transform.localScale = Vector2.one;
					skeletonAnimation2.gameObject.SetActive(false);
					UnityEngine.Object.Destroy(dictionary[item].gameObject);
					dictionary[item] = skeletonAnimation2;
				}
			}
		}
		else if (dataAssetSkeleton != null)
		{
			foreach (GUILayoutCell item2 in list)
			{
				SkeletonAnimation skeletonAnimation3 = dictionary[item2];
				skeletonAnimation3.SkeletonDataAsset = dataAssetSkeleton;
				skeletonAnimation3.transform.localScale = Vector3.one;
				skeletonAnimation3.SkeletonDataAsset.Reset();
				skeletonAnimation3.Reset();
				skeletonAnimation3.Initialize();
				FadeSkeletons(skeletonAnimation3);
				skeletonAnimation3.gameObject.SetActive(false);
				dictionary[item2] = skeletonAnimation3;
			}
		}
		switch (activeScreen)
		{
		case GUIScreenType.Main:
			CachedScreenManager.MenuScreen.MenuScreenScrollView.AnimationAvatar = dictionary;
			CachedScreenManager.MenuScreen.MenuScreenScrollView.DoubleAnimationAvatar = doubleAnimationAvatar;
			break;
		case GUIScreenType.Result:
			CachedScreenManager.ResultScreen.AnimationAvatar = dictionary;
			break;
		}
		ResetAnimationState();
	}

	public SkeletonAnimation AddCharacterAnimation(SkeletonAnimation defaultAnimation, Transform parent)
	{
		SkeletonAnimation skeletonAnimation = CreateSkeletonAnimation(animationSkeleton, defaultAnimation, dataAssetSkeleton);
		skeletonAnimation.transform.parent = parent;
		skeletonAnimation.transform.localPosition = Vector3.zero;
		return InitSkeletonAnimation(skeletonAnimation);
	}

	public List<SkeletonAnimation> AddDoubleCharacterAnimations(SkeletonAnimation defaultAnimation, Transform parent)
	{
		List<SkeletonAnimation> list = new List<SkeletonAnimation>();
		if (CharacterMode == CharacterMode.Random)
		{
			for (int i = 0; i < MultipleCharacterData.Count; i++)
			{
				SkeletonAnimation skeletonAnimation = null;
				skeletonAnimation = CreateSkeletonAnimation(MultipleCharacterData[i].multipleAnimationSkeleton, defaultAnimation, MultipleCharacterData[i].multipleDataAssetSkeleton);
				skeletonAnimation.transform.parent = parent;
				skeletonAnimation.transform.localPosition = Vector3.right * MultipleCharacterData[i].multipleCharacterOffset + Vector3.back * i;
				skeletonAnimation = InitSkeletonAnimation(skeletonAnimation);
				doubleAnimationAvatar.Add(skeletonAnimation);
				list.Add(skeletonAnimation);
				if (tk2dSystem.IsRetina)
				{
					skeletonAnimation.transform.localPosition *= 2f;
				}
			}
		}
		return list;
	}

	public void SetParameters(Vector3 position, Vector3 scale, Color newColor, bool seconPlayeMustChoose)
	{
		SetPosition(position);
		SetScale(scale, seconPlayeMustChoose);
		SetColor(newColor);
	}

	public void SetPosition(Vector3 position)
	{
		foreach (Transform value in shopIconTransform.Values)
		{
			value.localPosition = position;
		}
	}

	public void SetScale(Vector3 scale, bool secondPlayerMustChoose)
	{
		foreach (Transform value in shopIconTransform.Values)
		{
			value.localScale = scale;
		}
		foreach (Transform value2 in shopIconCircleTransform.Values)
		{
			value2.localScale = scale * scaleCircle.Evaluate(scale.x);
		}
		bool flag = CachedManagers.CachedGameManager.CurrentGameType == GameType.VSHuman;
		if (!secondPlayerMustChoose)
		{
			foreach (Transform value3 in shopIconChoiceFirstTransform.Values)
			{
				value3.localScale = ((!shopItem.Availible) ? Vector3.zero : (scale * scaleCircle.Evaluate(scale.x)));
			}
			foreach (Transform value4 in shopIconChoiceSecondTransform.Values)
			{
				value4.localScale = Vector3.zero;
			}
			{
				foreach (Transform value5 in shopIconChoiceFirstAndSecondTransform.Values)
				{
					value5.localScale = Vector3.zero;
				}
				return;
			}
		}
		if (!flag)
		{
			return;
		}
		foreach (Transform value6 in shopIconChoiceFirstTransform.Values)
		{
			value6.localScale = Vector3.zero;
		}
		foreach (Transform value7 in shopIconChoiceSecondTransform.Values)
		{
			value7.localScale = ((IsChoicedFirstPlayer || !shopItem.Availible) ? Vector3.zero : (scale * scaleCircle.Evaluate(scale.x)));
		}
		foreach (Transform value8 in shopIconChoiceFirstAndSecondTransform.Values)
		{
			value8.localScale = ((!IsChoicedFirstPlayer || !shopItem.Availible) ? Vector3.zero : (scale * scaleCircle.Evaluate(scale.x)));
		}
	}

	public void SetColor(Color newColor)
	{
		foreach (tk2dSprite value in shopIcon.Values)
		{
			Color color = value.color;
			color.a = newColor.a;
			value.color = color;
			tk2dSprite[] componentsInChildren = value.GetComponentsInChildren<tk2dSprite>();
			tk2dSprite[] array = componentsInChildren;
			foreach (tk2dSprite tk2dSprite2 in array)
			{
				color = tk2dSprite2.color;
				color.a = newColor.a;
				tk2dSprite2.color = color;
			}
		}
		foreach (tk2dSprite value2 in shopIconCheck.Values)
		{
			value2.color = newColor;
		}
		foreach (tk2dSprite value3 in shopIconCircle.Values)
		{
			value3.color = newColor;
		}
	}

	public void StartAnimation(Spine.Animation newAnimation = null)
	{
		Dictionary<GUILayoutCell, SkeletonAnimation> dictionary = null;
		ResultScreen resultScreen = CachedScreenManager.ResultScreen;
		if (!(resultScreen != null))
		{
			return;
		}
		dictionary = resultScreen.AnimationAvatar;
		foreach (SkeletonAnimation value in dictionary.Values)
		{
			value.gameObject.SetActive(true);
			if (newAnimation != null && value.state != null)
			{
				value.State.SetAnimation(0, newAnimation, true);
			}
		}
		resultScreen.AnimationAvatar = dictionary;
	}

	public void StartAnimationOnResultScreen(bool isWinner)
	{
		Spine.Animation[] array = new Spine.Animation[2] { AnimationSpawn, AnimationWin };
		Spine.Animation[] array2 = new Spine.Animation[2] { AnimationAnger, AnimationLose };
		Spine.Animation[] array3 = ((!isWinner) ? array2 : array);
		Dictionary<GUILayoutCell, SkeletonAnimation> dictionary = null;
		ResultScreen resultScreen = CachedScreenManager.ResultScreen;
		if (resultScreen != null)
		{
			dictionary = resultScreen.AnimationAvatar;
			foreach (SkeletonAnimation value in dictionary.Values)
			{
				if (value != null)
				{
					value.gameObject.SetActive(true);
					if (CharacterMode == CharacterMode.Random)
					{
						value.SkeletonDataAsset.Reset();
						value.Reset();
						value.LateUpdate();
					}
					if (value.state != null)
					{
						value.State.SetAnimation(0, array3[0], false);
						value.State.AddAnimation(0, array3[1], true, 0f);
					}
				}
			}
			resultScreen.AnimationAvatar = dictionary;
		}
		schedulAnimation = delegate
		{
			if (this != null)
			{
				bool flag = true;
				if (resultScreen != null)
				{
					Dictionary<GUILayoutCell, SkeletonAnimation>.ValueCollection values = resultScreen.AnimationAvatar.Values;
					foreach (SkeletonAnimation item in values)
					{
						if (item.SkeletonDataAsset != SkeletonAsset)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						ResetAnimationState();
						BeginRandomAnimations(false);
					}
				}
			}
		};
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, schedulAnimation, 4f);
	}

	public void BeginRandomAnimations(bool isSetActive = true)
	{
		if (isSetActive)
		{
			isRunningRandomAnimations = true;
		}
		Dictionary<GUILayoutCell, SkeletonAnimation> dictionary = null;
		MenuScreen menuScreen = CachedScreenManager.MenuScreen;
		if (menuScreen != null)
		{
			MenuScreenScrollView menuScreenScrollView = menuScreen.MenuScreenScrollView;
			dictionary = menuScreenScrollView.AnimationAvatar;
			doubleAnimationAvatar = menuScreenScrollView.DoubleAnimationAvatar;
			StartMenuAnimations(isSetActive, dictionary);
			StartMenuAnimations(isSetActive, doubleAnimationAvatar);
			menuScreenScrollView.AnimationAvatar = dictionary;
			menuScreenScrollView.DoubleAnimationAvatar = doubleAnimationAvatar;
			if (CharacterMode == CharacterMode.Random)
			{
				isRunningRandomAnimations = false;
			}
		}
		if (!isRunningRandomAnimations)
		{
			return;
		}
		CachedManagers.CachedScheduler.CallMethodWithDelay(this, delegate
		{
			if (this != null)
			{
				bool flag = true;
				if (menuScreen != null)
				{
					MenuScreenScrollView menuScreenScrollView2 = menuScreen.MenuScreenScrollView;
					Dictionary<GUILayoutCell, SkeletonAnimation>.ValueCollection values = menuScreenScrollView2.AnimationAvatar.Values;
					foreach (SkeletonAnimation item in values)
					{
						if (item.SkeletonDataAsset != SkeletonAsset)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						ResetAnimationState();
						BeginRandomAnimations(false);
					}
				}
			}
		}, 20f);
	}

	public void CheckBought()
	{
		ResizeIcon(shopIconCheck, false);
		ResizeIcon(shopIconNew, true);
		ResizeIcon(shopIconFree, true);
		foreach (tk2dSprite value in shopIcon.Values)
		{
			if (shopItem.Availible)
			{
				Color black = Color.black;
				black.a = value.color.a;
				value.color = black;
				value.CachedRenderer.material.SetFloat("_GrayscaleFactor", 0f);
			}
			else
			{
				Color color = new Color32(90, 0, 100, 0);
				color.a = value.color.a;
				value.color = color;
				value.CachedRenderer.material.SetFloat("_GrayscaleFactor", 0.7f);
			}
		}
	}

	public void PlayChooseEffect()
	{
		bool flag = true;
		foreach (SkeletonAnimation value in shopIconChoose.Values)
		{
			value.State.SetAnimation(0, "pick", false);
			value.State.AddAnimation(0, "hidden", true, 0f);
			if (!flag)
			{
				value.MeshRenderer.sortingOrder = -11;
			}
			flag = !flag;
		}
	}

	public void PlayItemPurchasedEffect(GameObject iconEffectPrefab, GameObject animationEffectPrefab)
	{
		if (iconEffectPrefab != null)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				bool flag = false;
				foreach (Transform value in shopIconCircleTransform.Values)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(iconEffectPrefab);
					gameObject.transform.parent = value;
					gameObject.transform.localPosition = Vector3.zero;
					ParticleSystemRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].sortingLayerName = "GUI_elements";
						componentsInChildren[i].sortingOrder = ((!flag) ? (-1) : (-12));
					}
					currentEffects.Add(gameObject);
					flag = !flag;
				}
			}, 0f);
		}
		Dictionary<GUILayoutCell, SkeletonAnimation> animationAvatar = null;
		MenuScreen menuScreen = CachedScreenManager.MenuScreen;
		if (!(menuScreen != null))
		{
			return;
		}
		MenuScreenScrollView menuScreenScrollView = menuScreen.MenuScreenScrollView;
		animationAvatar = menuScreenScrollView.AnimationAvatar;
		if (animationEffectPrefab != null)
		{
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				foreach (SkeletonAnimation value2 in animationAvatar.Values)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(animationEffectPrefab);
					gameObject.transform.parent = value2.transform;
					gameObject.transform.localPosition = Vector3.zero;
					if (CharacterMode == CharacterMode.Random)
					{
						int num = -1;
						float num2 = 0f;
						for (int i = 0; i < MultipleCharacterData.Count; i++)
						{
							num *= num;
							num2 += MultipleCharacterData[i].multipleCharacterOffset * (float)num;
						}
						gameObject.transform.localPosition += num2 * Vector3.right / MultipleCharacterData.Count;
					}
					ParticleSystemRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].sortingLayerName = "GUI_FX_back";
					}
					currentEffects.Add(gameObject);
				}
			}, 0f);
		}
		if (CharacterMode != CharacterMode.Random && animationAvatar != null)
		{
			foreach (SkeletonAnimation value3 in animationAvatar.Values)
			{
				if (value3 != null)
				{
					value3.State.SetAnimation(0, AnimationDefault, false);
					value3.State.AddAnimation(0, AnimationStandUp, false, 0f);
					value3.State.AddAnimation(0, AnimationSpawn, false, 0f);
					value3.State.AddAnimation(0, AnimationIdle, true, 0f);
				}
			}
		}
		menuScreenScrollView.AnimationAvatar = animationAvatar;
	}

	public void PlaySpecialIdleAnimations()
	{
		Dictionary<GUILayoutCell, SkeletonAnimation> dictionary = null;
		MenuScreen menuScreen = CachedScreenManager.MenuScreen;
		if (!(menuScreen != null))
		{
			return;
		}
		MenuScreenScrollView menuScreenScrollView = menuScreen.MenuScreenScrollView;
		isRunningRandomAnimations = true;
		dictionary = menuScreenScrollView.AnimationAvatar;
		doubleAnimationAvatar = menuScreenScrollView.DoubleAnimationAvatar;
		if (dictionary != null)
		{
			foreach (SkeletonAnimation value in dictionary.Values)
			{
				currentCharacterMenuAnimation = ((!(value.SkeletonDataAsset == CurrentMultipleCharacterInfo.multipleDataAssetSkeleton)) ? CharacterMenuAnimation.HitReaction : CharacterMenuAnimation.Hit);
				StartMenuAnimation(true, value);
			}
		}
		for (int i = 0; i < doubleAnimationAvatar.Count; i++)
		{
			currentCharacterMenuAnimation = ((!(doubleAnimationAvatar[i].SkeletonDataAsset == CurrentMultipleCharacterInfo.multipleDataAssetSkeleton)) ? CharacterMenuAnimation.HitReaction : CharacterMenuAnimation.Hit);
			StartMenuAnimation(true, doubleAnimationAvatar[i]);
		}
		currentCharacterMenuAnimation = CharacterMenuAnimation.Shop;
		menuScreenScrollView.AnimationAvatar = dictionary;
		menuScreenScrollView.DoubleAnimationAvatar = doubleAnimationAvatar;
	}

	public void StartSpecialIdleAnimations(bool isResetNeeded = false, bool isRandomNeeded = false)
	{
		if (CharacterMode == CharacterMode.Random)
		{
			currentCharacterMenuAnimation = CharacterMenuAnimation.Idle;
		}
		if (isResetNeeded)
		{
			BeginRandomAnimations();
		}
		if (isRandomNeeded)
		{
			RandomizeMultipleCharacter();
		}
	}

	public void RandomizeMultipleCharacter()
	{
		float num = 0f;
		for (int i = 0; i < multipleCharacterData.Count; i++)
		{
			num += (float)multipleCharacterData[i].multipleCharacterWeight;
		}
		if (!(num > 0f))
		{
			return;
		}
		double num2 = 0.0;
		double num3 = UnityEngine.Random.value;
		for (int j = 0; j < multipleCharacterData.Count; j++)
		{
			num2 += (double)multipleCharacterData[j].multipleCharacterWeight;
			if (num3 <= num2 / (double)num)
			{
				CurrentMultipleCharacterInfo = multipleCharacterData[j];
				CurrentType = multipleCharacterData[j].multipleCharacterType;
				break;
			}
		}
	}

	public void SetMultipleCharacter(CharacterNameType characterNameType)
	{
		for (int i = 0; i < multipleCharacterData.Count; i++)
		{
			if (characterNameType == multipleCharacterData[i].multipleCharacterType)
			{
				CurrentMultipleCharacterInfo = multipleCharacterData[i];
				CurrentType = multipleCharacterData[i].multipleCharacterType;
				break;
			}
		}
	}

	private void ResetAnimationState()
	{
		cachedSkeletonData = null;
		animationAnger = null;
		animationIdle = null;
		animationStandUp = null;
		animationScare = null;
		animationWin = null;
		animationLose = null;
		animationSpawn = null;
		animationDefault = null;
	}

	private void ResizeIcon(Dictionary<tk2dSprite, tk2dSprite> iconDictionary, bool isReversed)
	{
		foreach (tk2dSprite value in iconDictionary.Values)
		{
			if (shopItem.Availible)
			{
				value.transform.localScale = ((!isReversed) ? Vector3.one : Vector3.zero);
			}
			else
			{
				value.transform.localScale = ((!isReversed) ? Vector3.zero : Vector3.one);
			}
		}
	}

	private SkeletonAnimation CreateSkeletonAnimation(SkeletonAnimation animationSkeleton, SkeletonAnimation defaultAnimation, SkeletonDataAsset dataAssetSkeleton)
	{
		SkeletonAnimation skeletonAnimation;
		if (animationSkeleton != null)
		{
			skeletonAnimation = UnityEngine.Object.Instantiate(animationSkeleton);
		}
		else
		{
			skeletonAnimation = UnityEngine.Object.Instantiate(defaultAnimation);
			if (dataAssetSkeleton != null)
			{
				skeletonAnimation.SkeletonDataAsset = dataAssetSkeleton;
				skeletonAnimation.SkeletonDataAsset.Reset();
				skeletonAnimation.Reset();
				skeletonAnimation.LateUpdate();
			}
		}
		return skeletonAnimation;
	}

	private SkeletonAnimation InitSkeletonAnimation(SkeletonAnimation animationSkeleton)
	{
		if (tk2dSystem.IsRetina)
		{
			animationSkeleton.transform.localScale = Vector3.one * 2f;
		}
		animationSkeleton.Initialize();
		FadeSkeletons(animationSkeleton);
		animationSkeleton.gameObject.AddComponent<CustomRenderQueue>();
		return animationSkeleton;
	}

	private void StartMenuAnimations(bool isSetActive, List<SkeletonAnimation> animationAvatar)
	{
		if (animationAvatar == null)
		{
			return;
		}
		for (int i = 0; i < animationAvatar.Count; i++)
		{
			if (animationAvatar[i] != null)
			{
				animationAvatar[i] = StartMenuAnimation(isSetActive, animationAvatar[i]);
			}
		}
	}

	private void StartMenuAnimations(bool isSetActive, Dictionary<GUILayoutCell, SkeletonAnimation> animationAvatar)
	{
		if (animationAvatar == null)
		{
			return;
		}
		List<GUILayoutCell> list = new List<GUILayoutCell>(animationAvatar.Keys);
		foreach (GUILayoutCell item in list)
		{
			SkeletonAnimation anim = animationAvatar[item];
			anim = StartMenuAnimation(isSetActive, anim);
			animationAvatar[item] = anim;
		}
	}

	private SkeletonAnimation StartMenuAnimation(bool isSetActive, SkeletonAnimation anim)
	{
		if (isSetActive && anim != null)
		{
			anim.gameObject.SetActive(true);
		}
		if (anim != null && anim.gameObject.activeSelf && isRunningRandomAnimations)
		{
			Spine.Animation[] list = new Spine.Animation[3] { AnimationAnger, AnimationWin, AnimationScare };
			Spine.Animation animation = list.RandomObject();
			if (animation != null && anim.state != null)
			{
				anim.state.ClearTracks();
				if (CharacterMode == CharacterMode.Random)
				{
					if (currentCharacterMenuAnimation == CharacterMenuAnimation.Idle)
					{
						anim.State.SetAnimation(0, "idle_fight", true);
					}
					else if (currentCharacterMenuAnimation == CharacterMenuAnimation.Hit)
					{
						anim.State.SetAnimation(0, "hit", false);
						anim.State.AddAnimation(0, "idle_shop", true, 0f);
					}
					else if (currentCharacterMenuAnimation == CharacterMenuAnimation.HitReaction)
					{
						anim.State.SetAnimation(0, "hit_reaction", false);
					}
				}
				else
				{
					anim.State.SetAnimation(0, animation, true);
					anim.State.AddAnimation(0, AnimationIdle, true, 0f);
				}
			}
		}
		return anim;
	}

	private void FadeSkeletons(SkeletonAnimation currentAnimation)
	{
		GUIScreenType activeScreen = CachedScreenManager.ActiveScreen;
		if (activeScreen != GUIScreenType.Main && activeScreen != GUIScreenType.Roulette)
		{
			return;
		}
		tk2dSpriteCollectionData[] spriteCollections = currentAnimation.SkeletonDataAsset.spriteCollections;
		for (int i = 0; i < spriteCollections.Length; i++)
		{
			if (!(spriteCollections[i].spriteCollectionName == "Skeleton") && !(spriteCollections[i].spriteCollectionName == "SkeletonBeefy") && !(spriteCollections[i].spriteCollectionName == "SkeletonPlumpy"))
			{
				continue;
			}
			Material[] materialInsts = spriteCollections[i].inst.materialInsts;
			if (materialInsts != null)
			{
				for (int j = 0; j < materialInsts.Length; j++)
				{
					materialInsts[j].mainTexture = Texture2D.blackTexture;
				}
			}
		}
	}
}
