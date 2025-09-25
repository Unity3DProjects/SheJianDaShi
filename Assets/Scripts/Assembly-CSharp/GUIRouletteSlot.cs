using System.Collections.Generic;
using UnityEngine;

public class GUIRouletteSlot : MonoBehaviour
{
	private const string LABEL_PREFIX_COINS = "+ ";

	private const string SPRITE_COIN = "coin_gui";

	private const string SPRITE_GEM = "gem";

	private const string ROULETTE_COINS_SMALL_SPRITE_NAME = "roulette_coins_small";

	private const string ROULETTE_COINS_SPRITE_NAME = "roulette_coins";

	private const string ROULETTE_COINS_LARGE_SPRITE_NAME = "roulette_coins_large";

	private const string ROULETTE_GEMS_SMALL_SPRITE_NAME = "gem_one";

	private const string ROULETTE_GEMS_SPRITE_NAME = "gems_2";

	private const string ROULETTE_GEMS_LARGE_SPRITE_NAME = "pinch_of_gems";

	private const string DEFAULT_ANIMATION = "default";

	private const string WIN_ANIMATION = "win";

	private const string IDLE_ANIMATION = "idle";

	[SerializeField]
	private tk2dBaseSprite spriteCoins;

	[SerializeField]
	private Transform spriteCoinsParentTransform;

	[SerializeField]
	private TweenColor spriteCoinsTweenColor;

	[SerializeField]
	private tk2dTextMesh labelCoins;

	[SerializeField]
	private Transform labelCoinsParentTransform;

	[SerializeField]
	private TweenColor labelCoinsTweenColor;

	[SerializeField]
	private float labelCoinsPositionXSmall = -20f;

	[SerializeField]
	private float labelCoinsPositionXLarge = -20f;

	[SerializeField]
	private float labelGemsPositionXLarge = -20f;

	[SerializeField]
	private tk2dSprite spriteSmallCoins;

	[SerializeField]
	private Transform spriteSmallParentTransform;

	[SerializeField]
	private TweenColor spriteSmallCoinsTweenColor;

	[SerializeField]
	private float spriteSmallCoinsPositionXSmall = 40f;

	[SerializeField]
	private float spriteSmallCoinsPositionXLarge = 40f;

	[SerializeField]
	private float spriteSmallGemsPositionXSmall = 15f;

	[SerializeField]
	private Transform characterAnimationTransform;

	[SerializeField]
	private TweenColor characterAnimationTweenColor;

	[SerializeField]
	private tk2dBaseSprite characterShadow;

	[SerializeField]
	private Transform characterShadowTransform;

	[SerializeField]
	private TweenColor characterShadowTweenColor;

	[SerializeField]
	private tk2dBaseSprite characterStars;

	[SerializeField]
	private Transform characterStarsTransform;

	[SerializeField]
	private TweenColor characterStarsTweenColor;

	[SerializeField]
	private tk2dSprite characterSprite;

	[SerializeField]
	private float gemsProbability = 0.4f;

	[SerializeField]
	private GUIControl hideControl;

	private RouletteRewardType type;

	private bool isWinnerSlot;

	private bool isWinnerNeighbourSlot;

	private Transform cachedTransform;

	private EffectsManager cachedEffectsManager;

	private List<GameObject> currentEffects = new List<GameObject>();

	private SkeletonAnimation slotCharacterAnimation;

	private tk2dSprite slotCharacterSprite;

	private tk2dBaseSprite slotSpriteCoins;

	private tk2dTextMesh slotLabelCoins;

	private tk2dSprite slotSmallCoinsSprite;

	private tk2dBaseSprite slotCharacterShadow;

	private tk2dBaseSprite slotCharacterStars;

	private List<SkeletonAnimation> slotDoubleCharacterAnimations = new List<SkeletonAnimation>();

	private Managers cachedManagers;

	private GUICharacter guiCharacter;

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	private EffectsManager CachedEffectsManager
	{
		get
		{
			if (cachedEffectsManager == null)
			{
				cachedEffectsManager = SingletonMonoBehaviour<EffectsManager>.Instance;
			}
			return cachedEffectsManager;
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

	public bool IsWinnerSlot
	{
		get
		{
			return isWinnerSlot;
		}
	}

	public RouletteRewardType RewardType
	{
		get
		{
			return type;
		}
	}

	public void InitSlot(RouletteSlotInfo slotInfo)
	{
		isWinnerSlot = slotInfo.isWinnerSlot;
		isWinnerNeighbourSlot = slotInfo.isWinnerNeighbourSlot;
		type = slotInfo.rewardType;
		if (isWinnerNeighbourSlot)
		{
			RouletteRewardType[] list = ((!(Random.value < 0.75f)) ? new RouletteRewardType[5]
			{
				RouletteRewardType.Ordinary,
				RouletteRewardType.Premium,
				RouletteRewardType.Gems1,
				RouletteRewardType.Gems2,
				RouletteRewardType.Gems3
			} : new RouletteRewardType[3]
			{
				RouletteRewardType.Coins1,
				RouletteRewardType.Coins2,
				RouletteRewardType.Coins3
			});
			type = list.RandomObject();
		}
	}

	public void InitReward(RouletteRewardInfo winInfo = null)
	{
		hideControl.HideImmediately();
		GameObject systemEffect = null;
		for (int i = 0; i < currentEffects.Count; i++)
		{
			systemEffect = currentEffects[i];
			if ((bool)systemEffect)
			{
				Object.Destroy(systemEffect);
			}
		}
		currentEffects.Clear();
		if (isWinnerSlot && winInfo != null)
		{
			type = winInfo.type;
		}
		bool flag = Random.value < gemsProbability;
		if (!isWinnerSlot && flag && (type == RouletteRewardType.Coins1 || type == RouletteRewardType.Coins2 || type == RouletteRewardType.Coins3))
		{
			switch (type)
			{
			case RouletteRewardType.Coins1:
				type = RouletteRewardType.Gems1;
				break;
			case RouletteRewardType.Coins2:
				type = RouletteRewardType.Gems2;
				break;
			case RouletteRewardType.Coins3:
				type = RouletteRewardType.Gems3;
				break;
			}
		}
		if (type == RouletteRewardType.Coins1 || type == RouletteRewardType.Coins2 || type == RouletteRewardType.Coins3 || type == RouletteRewardType.Gems1 || type == RouletteRewardType.Gems2 || type == RouletteRewardType.Gems3)
		{
			slotSpriteCoins = Object.Instantiate(spriteCoins);
			slotSpriteCoins.transform.parent = spriteCoinsParentTransform;
			slotSpriteCoins.transform.localPosition = spriteCoins.transform.localPosition;
			spriteCoinsTweenColor.Target = slotSpriteCoins.gameObject;
			float x = 0f;
			slotLabelCoins = Object.Instantiate(labelCoins);
			slotLabelCoins.transform.parent = labelCoinsParentTransform;
			switch (type)
			{
			case RouletteRewardType.Coins1:
				x = labelCoinsPositionXSmall;
				break;
			case RouletteRewardType.Coins2:
			case RouletteRewardType.Coins3:
				x = labelCoinsPositionXLarge;
				break;
			case RouletteRewardType.Gems1:
			case RouletteRewardType.Gems2:
			case RouletteRewardType.Gems3:
				x = labelGemsPositionXLarge;
				break;
			}
			slotLabelCoins.transform.localPosition = new Vector3(x, labelCoins.transform.localPosition.y, labelCoins.transform.localPosition.z);
			labelCoinsTweenColor.Target = slotLabelCoins.gameObject;
			slotSmallCoinsSprite = Object.Instantiate(spriteSmallCoins);
			slotSmallCoinsSprite.transform.parent = spriteSmallParentTransform;
			switch (type)
			{
			case RouletteRewardType.Coins1:
				x = spriteSmallCoinsPositionXSmall;
				break;
			case RouletteRewardType.Coins2:
			case RouletteRewardType.Coins3:
				x = spriteSmallCoinsPositionXLarge;
				break;
			case RouletteRewardType.Gems1:
			case RouletteRewardType.Gems2:
			case RouletteRewardType.Gems3:
				x = spriteSmallGemsPositionXSmall;
				break;
			}
			slotSmallCoinsSprite.transform.localPosition = new Vector3(x, spriteSmallCoins.transform.localPosition.y, spriteSmallCoins.transform.localPosition.z);
			spriteSmallCoinsTweenColor.Target = slotSmallCoinsSprite.gameObject;
			string sprite = ((type != RouletteRewardType.Gems1 && type != RouletteRewardType.Gems2 && type != RouletteRewardType.Gems3) ? "coin_gui" : "gem");
			slotSmallCoinsSprite.SetSprite(sprite);
			if (slotCharacterShadow != null)
			{
				slotCharacterShadow.gameObject.SetActive(false);
			}
			if (slotCharacterStars != null)
			{
				slotCharacterStars.gameObject.SetActive(false);
			}
		}
		if (type == RouletteRewardType.Ordinary || type == RouletteRewardType.Premium)
		{
			slotCharacterShadow = Object.Instantiate(characterShadow);
			slotCharacterShadow.transform.parent = characterShadowTransform;
			slotCharacterShadow.transform.localPosition = characterShadow.transform.localPosition;
			characterShadowTweenColor.Target = slotCharacterShadow.gameObject;
		}
		if (tk2dSystem.IsRetina)
		{
			if (slotSpriteCoins != null)
			{
				slotSpriteCoins.transform.localPosition *= 2f;
			}
			if (slotLabelCoins != null)
			{
				slotLabelCoins.transform.localPosition *= 2f;
			}
			if (slotSmallCoinsSprite != null)
			{
				slotSmallCoinsSprite.transform.localPosition *= 2f;
			}
			if (slotCharacterShadow != null)
			{
				slotCharacterShadow.transform.localPosition *= 2f;
			}
			if (slotCharacterStars != null)
			{
				slotCharacterStars.transform.localPosition *= 2f;
			}
		}
		switch (type)
		{
		case RouletteRewardType.Coins1:
			slotSpriteCoins.SetSprite("roulette_coins_small");
			slotSpriteCoins.gameObject.SetActive(true);
			slotLabelCoins.gameObject.SetActive(true);
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Coins1;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Coins2:
			slotSpriteCoins.SetSprite("roulette_coins");
			slotSpriteCoins.gameObject.SetActive(true);
			slotLabelCoins.gameObject.SetActive(true);
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Coins2;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Coins3:
			slotSpriteCoins.SetSprite("roulette_coins_large");
			slotSpriteCoins.gameObject.SetActive(true);
			slotLabelCoins.gameObject.SetActive(true);
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Coins3;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Gems1:
			slotSpriteCoins.SetSprite("gem_one");
			slotSpriteCoins.gameObject.SetActive(true);
			slotLabelCoins.gameObject.SetActive(true);
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Gems1;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Gems2:
			slotSpriteCoins.SetSprite("gems_2");
			slotSpriteCoins.gameObject.SetActive(true);
			slotLabelCoins.gameObject.SetActive(true);
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Gems2;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Gems3:
			slotSpriteCoins.SetSprite("pinch_of_gems");
			slotSpriteCoins.gameObject.SetActive(true);
			slotLabelCoins.gameObject.SetActive(true);
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Gems3;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Ordinary:
			slotCharacterShadow.gameObject.SetActive(true);
			if (!isWinnerSlot)
			{
				if (!SingletonMonoBehaviour<RouletteManager>.Instance.IsAvalaibleOrdinaryCharacters)
				{
					type = RouletteRewardType.Coins3;
					InitReward();
					break;
				}
				winInfo = SingletonMonoBehaviour<RouletteManager>.Instance.GetRandomOrdinaryCharacter();
			}
			if (winInfo == null)
			{
				break;
			}
			if (isWinnerSlot)
			{
				AddAnimations(winInfo);
			}
			else
			{
				AddCharacterSprite(winInfo);
			}
			if (winInfo.archer != null && winInfo.archer.UpgradeInfo.Count != 0)
			{
				slotCharacterStars = Object.Instantiate(characterStars);
				slotCharacterStars.transform.parent = characterStarsTransform;
				slotCharacterStars.transform.localPosition = characterStars.transform.localPosition;
				if (tk2dSystem.IsRetina && slotCharacterStars != null)
				{
					slotCharacterStars.transform.localPosition *= 2f;
				}
				characterStarsTweenColor.Target = slotCharacterStars.gameObject;
			}
			break;
		case RouletteRewardType.Premium:
			slotCharacterShadow.gameObject.SetActive(true);
			if (!isWinnerSlot)
			{
				if (!SingletonMonoBehaviour<RouletteManager>.Instance.IsAvalaiblePremiumCharacters)
				{
					if (SingletonMonoBehaviour<RouletteManager>.Instance.IsAvalaibleOrdinaryCharacters && SingletonMonoBehaviour<GameManager>.Instance.CurrentGameType == GameType.Tournament)
					{
						type = RouletteRewardType.Ordinary;
					}
					else
					{
						type = RouletteRewardType.Coins3;
					}
					InitReward();
					break;
				}
				winInfo = SingletonMonoBehaviour<RouletteManager>.Instance.GetRandomPremiumCharacter();
			}
			if (winInfo == null)
			{
				break;
			}
			if (isWinnerSlot)
			{
				AddAnimations(winInfo);
			}
			else
			{
				AddCharacterSprite(winInfo);
			}
			if (winInfo.archer != null && winInfo.archer.UpgradeInfo.Count != 0)
			{
				slotCharacterStars = Object.Instantiate(characterStars);
				slotCharacterStars.transform.parent = characterStarsTransform;
				slotCharacterStars.transform.localPosition = characterStars.transform.localPosition;
				if (tk2dSystem.IsRetina && slotCharacterStars != null)
				{
					slotCharacterStars.transform.localPosition *= 2f;
				}
				characterStarsTweenColor.Target = slotCharacterStars.gameObject;
			}
			break;
		}
		if (tk2dSystem.IsRetina)
		{
			if (slotCharacterAnimation != null)
			{
				slotCharacterAnimation.transform.localPosition *= 2f;
			}
			if (slotCharacterSprite != null)
			{
				slotCharacterSprite.transform.localPosition *= 2f;
			}
		}
		if (slotCharacterAnimation != null)
		{
			slotCharacterAnimation.Initialize();
		}
		RefreshMeshFonts();
	}

	public void UpdateSlot(float deltaTime)
	{
		if (slotCharacterAnimation != null)
		{
			slotCharacterAnimation.Update(deltaTime);
		}
	}

	public void PrepareSlot()
	{
		if (isWinnerSlot)
		{
			return;
		}
		for (int i = 0; i < currentEffects.Count; i++)
		{
			if ((bool)currentEffects[i])
			{
				Object.Destroy(currentEffects[i]);
			}
		}
		currentEffects.Clear();
		characterAnimationTransform.gameObject.AddComponent<ParticleSystemColor>();
		hideControl.Show();
		spriteSmallCoinsTweenColor.enabled = true;
		labelCoinsTweenColor.enabled = true;
	}

	public void ShowSlot()
	{
		if (isWinnerSlot && (type == RouletteRewardType.Ordinary || type == RouletteRewardType.Premium))
		{
			SetSlotAnimation(slotCharacterAnimation, "idle");
			SetSlotAnimations(slotDoubleCharacterAnimations, "idle");
		}
	}

	public void OpenSlot()
	{
		if (!isWinnerSlot)
		{
			return;
		}
		GameObject systemEffect = null;
		switch (type)
		{
		case RouletteRewardType.Coins1:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinCoinsSmall));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("coins_get_little");
			break;
		case RouletteRewardType.Coins2:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinCoinsMedium));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("coins_get_middle");
			break;
		case RouletteRewardType.Coins3:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinCoinsLarge));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("coins_get_many");
			break;
		case RouletteRewardType.Gems1:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinGemsSmall));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("gems_changing");
			break;
		case RouletteRewardType.Gems2:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinGemsMedium));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("gems_changing");
			break;
		case RouletteRewardType.Gems3:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinGemsLarge));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("gems_changing");
			break;
		case RouletteRewardType.Ordinary:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinOrdinary));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("buy_character_common");
			SetSlotAnimation(slotCharacterAnimation, "win");
			SetSlotAnimations(slotDoubleCharacterAnimations, "win");
			break;
		case RouletteRewardType.Premium:
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteWinPremium));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			AudioManager.PlayAudioWithKey("buy_character_rare");
			SetSlotAnimation(slotCharacterAnimation, "win");
			SetSlotAnimations(slotDoubleCharacterAnimations, "win");
			break;
		}
	}

	public void RefreshMeshFonts()
	{
		tk2dTextMesh[] componentsInChildren = GetComponentsInChildren<tk2dTextMesh>(true);
		List<tk2dFontData> defaultLanguageFonts = CachedManagers.CachedLocalisationManager.DefaultLanguageFonts;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (defaultLanguageFonts == null)
			{
				continue;
			}
			int num = defaultLanguageFonts.IndexOf(componentsInChildren[i].DefaultFontData);
			if (num != -1)
			{
				tk2dFontData tk2dFontData2 = CachedManagers.CachedLocalisationManager.CurrentLanguageFonts[num];
				if (tk2dFontData2 != null)
				{
					componentsInChildren[i].font = tk2dFontData2;
					componentsInChildren[i].ForceBuild();
				}
			}
		}
	}

	public void DoubleReward(int counter = 2)
	{
		GameObject systemEffect = null;
		switch (type)
		{
		case RouletteRewardType.Coins1:
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Coins1 * counter;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Coins2:
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Coins2 * counter;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Coins3:
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Coins3 * counter;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Gems1:
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Gems1 * counter;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Gems2:
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Gems2 * counter;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Gems3:
			slotLabelCoins.text = "+ " + SingletonMonoBehaviour<RouletteManager>.Instance.Gems3 * counter;
			SingletonMonoBehaviour<Scheduler>.Instance.CallMethodWithDelay(this, delegate
			{
				systemEffect = Object.Instantiate(CachedEffectsManager.EffectPrefabByType(EffectType.GUIRouletteCoinsIdle));
				systemEffect.transform.parent = CachedTransform;
				systemEffect.transform.localPosition = Vector3.zero;
				currentEffects.Add(systemEffect);
			}, 0f);
			break;
		case RouletteRewardType.Ordinary:
		case RouletteRewardType.Premium:
			break;
		}
	}

	private void AddAnimations(RouletteRewardInfo winInfo)
	{
		float num = ((!tk2dSystem.IsRetina) ? 0.5f : 1f);
		Vector3 localScale = new Vector3(num, num, 1f);
		Vector3 vector = Vector3.down * 80f;
		RouletteScreen rouletteScreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.RouletteScreen;
		if (!(rouletteScreen != null))
		{
			return;
		}
		guiCharacter = CachedManagers.CachedLevelsManager.ObjectsSpawner.GetGUICharacter(winInfo.archer.Infos[0].characterNameType);
		slotDoubleCharacterAnimations = guiCharacter.AddDoubleCharacterAnimations(rouletteScreen.DefaultAnimation, characterAnimationTransform);
		if (slotDoubleCharacterAnimations.Count != 0)
		{
			characterShadowTweenColor.GetComponentInChildren<tk2dSprite>().scale = new Vector3(slotDoubleCharacterAnimations.Count, 1f, 1f);
			for (int i = 0; i < slotDoubleCharacterAnimations.Count; i++)
			{
				if (slotDoubleCharacterAnimations[i] != null)
				{
					slotDoubleCharacterAnimations[i] = SetSlotDoubleAnimationParameters(slotDoubleCharacterAnimations[i], "default", characterAnimationTweenColor);
					slotDoubleCharacterAnimations[i].transform.localPosition += vector;
					slotDoubleCharacterAnimations[i].transform.localScale = localScale;
				}
			}
		}
		else
		{
			slotCharacterAnimation = guiCharacter.AddCharacterAnimation(rouletteScreen.DefaultAnimation, characterAnimationTransform);
			slotCharacterAnimation = SetSlotAnimationParameters(slotCharacterAnimation, "default", characterAnimationTweenColor);
			slotCharacterAnimation.transform.localPosition = vector;
			slotCharacterAnimation.transform.localScale = localScale;
		}
	}

	private void AddCharacterSprite(RouletteRewardInfo winInfo)
	{
		Vector3 localPosition = Vector3.down * 80f;
		RouletteScreen rouletteScreen = SingletonMonoBehaviour<GUIScreenManager>.Instance.RouletteScreen;
		if (rouletteScreen != null)
		{
			guiCharacter = CachedManagers.CachedLevelsManager.ObjectsSpawner.GetGUICharacter(winInfo.archer.Infos[0].characterNameType);
			slotCharacterSprite = Object.Instantiate(characterSprite);
			slotCharacterSprite.transform.parent = characterAnimationTransform;
			slotCharacterSprite.SetSprite(guiCharacter.RouletteSpriteCollectionsData, guiCharacter.RouletteSpriteName);
			slotCharacterSprite.transform.localPosition = localPosition;
			slotCharacterSprite.gameObject.SetActive(true);
			characterAnimationTweenColor.Target = slotCharacterSprite.gameObject;
		}
	}

	private SkeletonAnimation SetSlotAnimation(SkeletonAnimation slotAnimation, string animationName)
	{
		if (slotAnimation != null)
		{
			slotAnimation.State.SetAnimation(0, animationName, true);
			slotAnimation.LateUpdate();
		}
		return slotAnimation;
	}

	private List<SkeletonAnimation> SetSlotAnimations(List<SkeletonAnimation> slotAnimations, string animationName)
	{
		for (int i = 0; i < slotAnimations.Count; i++)
		{
			if (slotAnimations[i] != null)
			{
				slotAnimations[i].State.SetAnimation(0, animationName, true);
				slotAnimations[i].LateUpdate();
			}
		}
		return slotAnimations;
	}

	private SkeletonAnimation SetSlotAnimationParameters(SkeletonAnimation slotAnimation, string animationName, TweenColor animationTweenColor)
	{
		slotAnimation.gameObject.SetActive(true);
		slotAnimation.ignoreMonoUpdate = true;
		slotAnimation.State.SetAnimation(0, animationName, true);
		slotAnimation.LateUpdate();
		slotAnimation.gameObject.AddComponent<SkeletonColor>();
		animationTweenColor.Target = slotCharacterAnimation.gameObject;
		return slotAnimation;
	}

	private SkeletonAnimation SetSlotDoubleAnimationParameters(SkeletonAnimation slotAnimation, string animationName, TweenColor animationTweenColor)
	{
		slotAnimation.gameObject.SetActive(true);
		slotAnimation.ignoreMonoUpdate = true;
		slotAnimation.State.SetAnimation(0, animationName, true);
		slotAnimation.LateUpdate();
		slotAnimation.gameObject.AddComponent<SkeletonColor>();
		TweenColor tweenColor = animationTweenColor.gameObject.AddComponent<TweenColor>();
		tweenColor = animationTweenColor;
		tweenColor.Target = slotAnimation.gameObject;
		return slotAnimation;
	}
}
