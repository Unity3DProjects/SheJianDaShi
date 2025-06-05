using System;
using Spine;
using UnityEngine;

public class AutoRenewableSubscriptionPopup : GUIPopup
{
	private const string SPARKS_EVENT_NAME = "Sparks";

	private const string SPARKS_END_EVENT_NAME = "MegaSparks";

	[SerializeField]
	private Tweener horizontalShowTweener;

	[SerializeField]
	private Tweener verticalShowTweener;

	[SerializeField]
	private tk2dUIItem[] closeButtons;

	[SerializeField]
	private tk2dUIItem[] trialButtons;

	[SerializeField]
	private tk2dUIItem[] weekButtons;

	[SerializeField]
	private tk2dUIItem[] monthButtons;

	[SerializeField]
	private tk2dUIItem[] yearButtons;

	[SerializeField]
	private GameObject[] weekPriceRoot;

	[SerializeField]
	private tk2dTextMesh[] weekPriceLabel;

	[SerializeField]
	private GameObject[] weekLoadingRoot;

	[SerializeField]
	private GameObject[] monthPriceRoot;

	[SerializeField]
	private tk2dTextMesh[] monthPriceLabel;

	[SerializeField]
	private GameObject[] monthLoadingRoot;

	[SerializeField]
	private GameObject[] yearPriceRoot;

	[SerializeField]
	private tk2dTextMesh[] yearPriceLabel;

	[SerializeField]
	private GameObject[] yearLoadingRoot;

	[SerializeField]
	private tk2dTextMesh[] trialButtonWeeklyPriceLabel;

	[SerializeField]
	private GameObject[] trialButtonWeeklyPriceRoot;

	[SerializeField]
	private GameObject[] trialButtonWeeklyLoadingRoot;

	[SerializeField]
	private tk2dUIItem[] TermsAndConditions;

	[SerializeField]
	private Transform[] popupLabelTransforms;

	[SerializeField]
	[ResourceLink]
	private AssetLink shineIdleEffectPrefab;

	[SerializeField]
	[ResourceLink]
	private AssetLink sparksIdleEffectPrefab;

	[SerializeField]
	private SkeletonAnimation lumberjackAnimationLandscape;

	[SerializeField]
	private Transform lumberjackSparksTransformLandscape;

	[SerializeField]
	private SkeletonAnimation lumberjackAnimationPortrait;

	[SerializeField]
	private Transform lumberjackSparksTransformPortrait;

	[SerializeField]
	[ResourceLink]
	private AssetLink sparksIdleCreedEffectPrefab;

	[SerializeField]
	private SkeletonAnimation phantomCreedAnimationLandscape;

	[SerializeField]
	private SkeletonAnimation phantomCreedAnimationPortrait;

	[SerializeField]
	private Transform phantomCreedSparksTransformLandscape;

	[SerializeField]
	private Transform phantomCreedSparksTransformPortrait;

	[SerializeField]
	private GameObject[] chineseLayouters;

	[SerializeField]
	private GameObject[] englishLayouters;

	[SerializeField]
	private ShopScrollView chinesePortraitScrollView;

	[SerializeField]
	private ShopScrollView englishPortraitScrollView;

	[SerializeField]
	private ShopScrollView chineseLandscapeScrollView;

	[SerializeField]
	private ShopScrollView englishLandscapeScrollView;

	private ShopItem subscriptionWeekItem;

	private ShopItem subscriptionMonthItem;

	private ShopItem subscriptionYearItem;

	private EventData cachedEventDataSparks;

	private EventData cachedEventDataSparksEnd;

	private EventData cachedEventDataSparksCreed;

	private bool isLanguageChinese;

	private SystemLanguage currentLanguage;

	private ShopScrollView scrollView;

	private const int scrollSpeed = 6;

	private EventData CachedEventDataSparks
	{
		get
		{
			if (cachedEventDataSparks == null)
			{
				cachedEventDataSparks = lumberjackAnimationLandscape.Skeleton.Data.FindEvent("Sparks");
			}
			return cachedEventDataSparks;
		}
	}

	private EventData CachedEventDataSparksEnd
	{
		get
		{
			if (cachedEventDataSparksEnd == null)
			{
				cachedEventDataSparksEnd = lumberjackAnimationPortrait.Skeleton.Data.FindEvent("MegaSparks");
			}
			return cachedEventDataSparksEnd;
		}
	}

	private EventData CachedEventDataSparksCreed
	{
		get
		{
			if (cachedEventDataSparksCreed == null)
			{
				cachedEventDataSparksCreed = phantomCreedAnimationLandscape.Skeleton.Data.FindEvent("Sparks");
			}
			return cachedEventDataSparksCreed;
		}
	}

	private void Awake()
	{
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		tk2dUIItem[] array = closeButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick += CloseButton_OnClick;
		}
		tk2dUIItem[] array2 = trialButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick += WeekButton_OnClick;
		}
		tk2dUIItem[] array3 = weekButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick += WeekButton_OnClick;
		}
		tk2dUIItem[] array4 = monthButtons;
		foreach (tk2dUIItem tk2dUIItem5 in array4)
		{
			tk2dUIItem5.OnClick += MonthButton_OnClick;
		}
		tk2dUIItem[] array5 = yearButtons;
		foreach (tk2dUIItem tk2dUIItem6 in array5)
		{
			tk2dUIItem6.OnClick += YearButton_OnClick;
		}
		tk2dUIItem[] termsAndConditions = TermsAndConditions;
		foreach (tk2dUIItem tk2dUIItem7 in termsAndConditions)
		{
			tk2dUIItem7.OnClick += TermsAndConditions_OnClick;
		}
		subscriptionWeekItem = base.CachedManagers.CachedShopManager.SubscriptionWeek;
		subscriptionMonthItem = base.CachedManagers.CachedShopManager.SubscriptionMonth;
		subscriptionYearItem = base.CachedManagers.CachedShopManager.SubscriptionYear;
		if (subscriptionWeekItem != null)
		{
			subscriptionWeekItem.OnPriceChanged += ShopItem_OnPriceChanged;
		}
		if (subscriptionMonthItem != null)
		{
			subscriptionMonthItem.OnPriceChanged += ShopItem_OnPriceChanged;
		}
		if (subscriptionYearItem != null)
		{
			subscriptionYearItem.OnPriceChanged += ShopItem_OnPriceChanged;
		}
		ShopScrollView shopScrollView = chinesePortraitScrollView;
		shopScrollView.OnContentPositionChange = (Action)Delegate.Combine(shopScrollView.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		ShopScrollView shopScrollView2 = englishPortraitScrollView;
		shopScrollView2.OnContentPositionChange = (Action)Delegate.Combine(shopScrollView2.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		ShopScrollView shopScrollView3 = chineseLandscapeScrollView;
		shopScrollView3.OnContentPositionChange = (Action)Delegate.Combine(shopScrollView3.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		ShopScrollView shopScrollView4 = englishLandscapeScrollView;
		shopScrollView4.OnContentPositionChange = (Action)Delegate.Combine(shopScrollView4.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		lumberjackAnimationLandscape.State.Event += LumberjackLandscape_State_Event;
		lumberjackAnimationPortrait.State.Event += LumberjackPortrait_State_Event;
		phantomCreedAnimationLandscape.State.Event += PhantomCreedLandscape_State_Event;
		phantomCreedAnimationPortrait.State.Event += PhantomCreedPortrait_State_Event;
		currentLanguage = base.CachedManagers.CachedStatisticsManager.CurrentLanguage;
		isLanguageChinese = currentLanguage == SystemLanguage.Chinese || currentLanguage == SystemLanguage.ChineseSimplified || currentLanguage == SystemLanguage.ChineseTraditional;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		tk2dUIItem[] array = closeButtons;
		foreach (tk2dUIItem tk2dUIItem2 in array)
		{
			tk2dUIItem2.OnClick -= CloseButton_OnClick;
		}
		tk2dUIItem[] array2 = trialButtons;
		foreach (tk2dUIItem tk2dUIItem3 in array2)
		{
			tk2dUIItem3.OnClick -= WeekButton_OnClick;
		}
		tk2dUIItem[] array3 = weekButtons;
		foreach (tk2dUIItem tk2dUIItem4 in array3)
		{
			tk2dUIItem4.OnClick -= WeekButton_OnClick;
		}
		tk2dUIItem[] array4 = monthButtons;
		foreach (tk2dUIItem tk2dUIItem5 in array4)
		{
			tk2dUIItem5.OnClick -= MonthButton_OnClick;
		}
		tk2dUIItem[] array5 = yearButtons;
		foreach (tk2dUIItem tk2dUIItem6 in array5)
		{
			tk2dUIItem6.OnClick -= YearButton_OnClick;
		}
		tk2dUIItem[] termsAndConditions = TermsAndConditions;
		foreach (tk2dUIItem tk2dUIItem7 in termsAndConditions)
		{
			tk2dUIItem7.OnClick -= TermsAndConditions_OnClick;
		}
		if (subscriptionWeekItem != null)
		{
			subscriptionWeekItem.OnPriceChanged -= ShopItem_OnPriceChanged;
		}
		if (subscriptionMonthItem != null)
		{
			subscriptionMonthItem.OnPriceChanged -= ShopItem_OnPriceChanged;
		}
		if (subscriptionYearItem != null)
		{
			subscriptionYearItem.OnPriceChanged -= ShopItem_OnPriceChanged;
		}
		ShopScrollView shopScrollView = chinesePortraitScrollView;
		shopScrollView.OnContentPositionChange = (Action)Delegate.Remove(shopScrollView.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		ShopScrollView shopScrollView2 = englishPortraitScrollView;
		shopScrollView2.OnContentPositionChange = (Action)Delegate.Remove(shopScrollView2.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		ShopScrollView shopScrollView3 = chineseLandscapeScrollView;
		shopScrollView3.OnContentPositionChange = (Action)Delegate.Remove(shopScrollView3.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		ShopScrollView shopScrollView4 = englishLandscapeScrollView;
		shopScrollView4.OnContentPositionChange = (Action)Delegate.Remove(shopScrollView4.OnContentPositionChange, new Action(ScrollView_OnContentPositionChange));
		lumberjackAnimationLandscape.State.Event -= LumberjackLandscape_State_Event;
		lumberjackAnimationPortrait.State.Event -= LumberjackPortrait_State_Event;
		phantomCreedAnimationLandscape.State.Event -= PhantomCreedLandscape_State_Event;
		phantomCreedAnimationPortrait.State.Event -= PhantomCreedPortrait_State_Event;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		for (int i = 0; i < chineseLayouters.Length; i++)
		{
			chineseLayouters[i].gameObject.SetActive(isLanguageChinese);
		}
		for (int j = 0; j < englishLayouters.Length; j++)
		{
			englishLayouters[j].gameObject.SetActive(!isLanguageChinese);
		}
		SetPrice();
		for (int k = 0; k < popupLabelTransforms.Length; k++)
		{
			if (shineIdleEffectPrefab != null && !string.IsNullOrEmpty(shineIdleEffectPrefab.assetGUID))
			{
				GameObject gameObject = (GameObject)shineIdleEffectPrefab.GetInstance();
				gameObject.transform.parent = popupLabelTransforms[k];
				gameObject.transform.localPosition = Vector3.zero;
				UpdateOrderInLayer(gameObject);
			}
		}
		base.Show(onFinished, immediately, delay, callInvokes);
		if (verticalShowTweener != null && horizontalShowTweener != null)
		{
			if (base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType == GUILayouterRotationType.Portrait)
			{
				verticalShowTweener.enabled = true;
				horizontalShowTweener.SetEndStateImmediately();
			}
			else
			{
				horizontalShowTweener.enabled = true;
				verticalShowTweener.SetEndStateImmediately();
			}
		}
		ScrollView_OnContentPositionChange();
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		base.Hide(onFinished, immediately, delay, callInvokes);
		if (verticalShowTweener != null && horizontalShowTweener != null)
		{
			verticalShowTweener.IsReversed = true;
			verticalShowTweener.ResetTween();
			verticalShowTweener.enabled = true;
			horizontalShowTweener.IsReversed = true;
			horizontalShowTweener.ResetTween();
			horizontalShowTweener.enabled = true;
		}
	}

	private void SetPrice()
	{
		if (subscriptionWeekItem != null)
		{
			string priceString = subscriptionWeekItem.PriceString;
			bool flag = !string.IsNullOrEmpty(priceString);
			tk2dUIItem[] array = trialButtons;
			foreach (tk2dUIItem tk2dUIItem2 in array)
			{
				tk2dUIItem2.gameObject.SetActive((subscriptionWeekItem as ShopItemSubscription).IsTrial);
			}
			tk2dUIItem[] array2 = weekButtons;
			foreach (tk2dUIItem tk2dUIItem3 in array2)
			{
				tk2dUIItem3.gameObject.SetActive(!(subscriptionWeekItem as ShopItemSubscription).IsTrial);
			}
			if (!(subscriptionWeekItem as ShopItemSubscription).IsTrial)
			{
				GameObject[] array3 = weekPriceRoot;
				foreach (GameObject gameObject in array3)
				{
					gameObject.SetActive(flag);
				}
				GameObject[] array4 = weekLoadingRoot;
				foreach (GameObject gameObject2 in array4)
				{
					gameObject2.SetActive(!flag);
				}
				if (flag)
				{
					tk2dTextMesh[] array5 = weekPriceLabel;
					foreach (tk2dTextMesh tk2dTextMesh2 in array5)
					{
						tk2dTextMesh2.text = priceString;
					}
				}
			}
			GameObject[] array6 = trialButtonWeeklyPriceRoot;
			foreach (GameObject gameObject3 in array6)
			{
				gameObject3.SetActive(flag);
			}
			GameObject[] array7 = trialButtonWeeklyLoadingRoot;
			foreach (GameObject gameObject4 in array7)
			{
				gameObject4.SetActive(!flag);
			}
			if (flag)
			{
				tk2dTextMesh[] array8 = trialButtonWeeklyPriceLabel;
				foreach (tk2dTextMesh tk2dTextMesh3 in array8)
				{
					tk2dTextMesh3.text = priceString;
				}
			}
		}
		if (subscriptionMonthItem != null)
		{
			string priceString2 = subscriptionMonthItem.PriceString;
			bool flag2 = !string.IsNullOrEmpty(priceString2);
			GameObject[] array9 = monthPriceRoot;
			foreach (GameObject gameObject5 in array9)
			{
				gameObject5.SetActive(flag2);
			}
			GameObject[] array10 = monthLoadingRoot;
			foreach (GameObject gameObject6 in array10)
			{
				gameObject6.SetActive(!flag2);
			}
			if (flag2)
			{
				tk2dTextMesh[] array11 = monthPriceLabel;
				foreach (tk2dTextMesh tk2dTextMesh4 in array11)
				{
					tk2dTextMesh4.text = priceString2;
				}
			}
		}
		if (!(subscriptionYearItem != null))
		{
			return;
		}
		string priceString3 = subscriptionYearItem.PriceString;
		bool flag3 = !string.IsNullOrEmpty(priceString3);
		GameObject[] array12 = yearPriceRoot;
		foreach (GameObject gameObject7 in array12)
		{
			gameObject7.SetActive(flag3);
		}
		GameObject[] array13 = yearLoadingRoot;
		foreach (GameObject gameObject8 in array13)
		{
			gameObject8.SetActive(!flag3);
		}
		if (flag3)
		{
			tk2dTextMesh[] array14 = yearPriceLabel;
			foreach (tk2dTextMesh tk2dTextMesh5 in array14)
			{
				tk2dTextMesh5.text = priceString3;
			}
		}
	}

	private void UpdateOrderInLayer(GameObject effect)
	{
		Renderer[] componentsInChildren = effect.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].sortingOrder > base.MaxOrderInLayer)
			{
				base.MaxOrderInLayer = componentsInChildren[i].sortingOrder;
			}
		}
	}

	private void WeekButton_OnClick()
	{
		if (subscriptionWeekItem != null)
		{
			subscriptionWeekItem.OnItemPurchased += ShopItem_OnItemPurchased;
			subscriptionWeekItem.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			tk2dUIManager.Instance.InputEnabled = false;
			if (!subscriptionWeekItem.TryBuy())
			{
				subscriptionWeekItem.OnItemPurchased -= ShopItem_OnItemPurchased;
				subscriptionWeekItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				tk2dUIManager.Instance.InputEnabled = true;
			}
		}
	}

	private void MonthButton_OnClick()
	{
		if (subscriptionMonthItem != null)
		{
			subscriptionMonthItem.OnItemPurchased += ShopItem_OnItemPurchased;
			subscriptionMonthItem.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			tk2dUIManager.Instance.InputEnabled = false;
			if (!subscriptionMonthItem.TryBuy())
			{
				subscriptionMonthItem.OnItemPurchased -= ShopItem_OnItemPurchased;
				subscriptionMonthItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				tk2dUIManager.Instance.InputEnabled = true;
			}
		}
	}

	private void YearButton_OnClick()
	{
		if (subscriptionYearItem != null)
		{
			subscriptionYearItem.OnItemPurchased += ShopItem_OnItemPurchased;
			subscriptionYearItem.OnItemPurchaseFailed += ShopItem_OnItemPurchaseFailed;
			tk2dUIManager.Instance.InputEnabled = false;
			if (!subscriptionYearItem.TryBuy())
			{
				subscriptionYearItem.OnItemPurchased -= ShopItem_OnItemPurchased;
				subscriptionYearItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
				tk2dUIManager.Instance.InputEnabled = true;
			}
		}
	}

	private void ShopItem_OnPriceChanged(ShopItem shopItem)
	{
		SetPrice();
	}

	private void ShopItem_OnItemPurchased(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
		Hide();
	}

	private void ShopItem_OnItemPurchaseFailed(ShopItem shopItem)
	{
		shopItem.OnItemPurchased -= ShopItem_OnItemPurchased;
		shopItem.OnItemPurchaseFailed -= ShopItem_OnItemPurchaseFailed;
		tk2dUIManager.Instance.InputEnabled = true;
	}

	private void LumberjackLandscape_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataSparks && sparksIdleEffectPrefab != null && !string.IsNullOrEmpty(sparksIdleEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)sparksIdleEffectPrefab.GetInstance();
			gameObject.transform.parent = lumberjackSparksTransformLandscape;
			gameObject.transform.localPosition = Vector3.zero;
			UpdateOrderInLayer(gameObject);
			UnityEngine.Object.Destroy(gameObject, 0.5f);
		}
		if (e.Data == CachedEventDataSparksEnd && sparksIdleEffectPrefab != null && !string.IsNullOrEmpty(sparksIdleEffectPrefab.assetGUID))
		{
			GameObject gameObject2 = (GameObject)sparksIdleEffectPrefab.GetInstance();
			gameObject2.transform.parent = lumberjackSparksTransformLandscape;
			gameObject2.transform.localPosition = Vector3.zero;
			UpdateOrderInLayer(gameObject2);
			UnityEngine.Object.Destroy(gameObject2, 1f);
		}
	}

	private void LumberjackPortrait_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataSparks && sparksIdleEffectPrefab != null && !string.IsNullOrEmpty(sparksIdleEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)sparksIdleEffectPrefab.GetInstance();
			gameObject.transform.parent = lumberjackSparksTransformPortrait;
			gameObject.transform.localPosition = Vector3.zero;
			UpdateOrderInLayer(gameObject);
			UnityEngine.Object.Destroy(gameObject, 0.5f);
		}
		if (e.Data == CachedEventDataSparksEnd && sparksIdleEffectPrefab != null && !string.IsNullOrEmpty(sparksIdleEffectPrefab.assetGUID))
		{
			GameObject gameObject2 = (GameObject)sparksIdleEffectPrefab.GetInstance();
			gameObject2.transform.parent = lumberjackSparksTransformPortrait;
			gameObject2.transform.localPosition = Vector3.zero;
			UpdateOrderInLayer(gameObject2);
			UnityEngine.Object.Destroy(gameObject2, 1f);
		}
	}

	private void PhantomCreedLandscape_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataSparksCreed && sparksIdleCreedEffectPrefab != null && !string.IsNullOrEmpty(sparksIdleCreedEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)sparksIdleCreedEffectPrefab.GetInstance();
			gameObject.transform.parent = phantomCreedSparksTransformLandscape;
			gameObject.transform.localPosition = Vector3.zero;
			UpdateOrderInLayer(gameObject);
			UnityEngine.Object.Destroy(gameObject, 0.2f);
		}
	}

	private void PhantomCreedPortrait_State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data == CachedEventDataSparksCreed && sparksIdleCreedEffectPrefab != null && !string.IsNullOrEmpty(sparksIdleCreedEffectPrefab.assetGUID))
		{
			GameObject gameObject = (GameObject)sparksIdleCreedEffectPrefab.GetInstance();
			gameObject.transform.parent = phantomCreedSparksTransformPortrait;
			gameObject.transform.localPosition = Vector3.zero;
			UpdateOrderInLayer(gameObject);
			UnityEngine.Object.Destroy(gameObject, 0.2f);
		}
	}

	private void AutoRenewableSubscriptionButton_OnClick()
	{
		LLApplicationWebViewController.ShowUrlInApplication(base.CachedManagers.CachedSubscriptionManager.SubscriptionLink, LocalisationManager.LocalizedStringOrSource(base.CachedManagers.CachedSubscriptionManager.SubscriptionTitle));
	}

	private void PrivacyPolicyButton_OnClick()
	{
		MCServices.ShowTermsPopupWebView();
	}

	private void TermsAndConditions_OnClick()
	{
		MCServices.ShowTermsPopupWebView();
	}

	private void ScrollView_OnContentPositionChange()
	{
		if (base.CachedManagers.CachedGUIOrientationManager.CurrentRotationType == GUILayouterRotationType.Portrait)
		{
			scrollView = ((!isLanguageChinese) ? englishPortraitScrollView : chinesePortraitScrollView);
		}
		else
		{
			scrollView = ((!isLanguageChinese) ? englishLandscapeScrollView : chineseLandscapeScrollView);
		}
		UpdateScrollViewContent();
	}

	protected override void Update()
	{
		base.Update();
		ScrollView_OnContentPositionChange();
		float y = 6f * Time.deltaTime;
		Transform contentContainer = scrollView.contentContainer;
		Transform child = contentContainer.GetChild(contentContainer.childCount - 1);
		GUILayoutCell component = child.GetComponent<GUILayoutCell>();
		float height = component.RecievedRect.height;
		float num = contentContainer.childCount;
		float num2 = height * num;
		if (Input.touchCount > 0 || child.transform.position.y >= scrollView.transform.position.y)
		{
			return;
		}
		foreach (Transform item in contentContainer)
		{
			item.transform.position += new Vector3(0f, y, 0f);
		}
		UpdateScrollViewContent();
	}

	private void UpdateScrollViewContent()
	{
		float y = scrollView.transform.position.y;
		BoxCollider component = scrollView.transform.Find("ScrollCollider").GetComponent<BoxCollider>();
		float num = component.size.y * 0.44f / GUIScreenManager.GuiScale;
		float num2 = component.size.y * 0.5f / GUIScreenManager.GuiScale;
		Transform contentContainer = scrollView.contentContainer;
		foreach (Transform item in contentContainer)
		{
			tk2dTextMesh[] componentsInChildren = item.GetComponentsInChildren<tk2dTextMesh>();
			tk2dTextMesh[] array = componentsInChildren;
			foreach (tk2dTextMesh tk2dTextMesh2 in array)
			{
				Color color = new Color(tk2dTextMesh2.color.r, tk2dTextMesh2.color.g, tk2dTextMesh2.color.b, 1f);
				Color b = new Color(tk2dTextMesh2.color.r, tk2dTextMesh2.color.g, tk2dTextMesh2.color.b, 0f);
				float num3 = Mathf.Abs(tk2dTextMesh2.transform.position.y - y);
				float t = Mathf.Clamp((num3 - num2) / (num2 - num), 0f, 1f);
				if (num3 > num)
				{
					tk2dTextMesh2.color = Color.Lerp(color, b, t);
				}
				else
				{
					tk2dTextMesh2.color = color;
				}
			}
		}
	}
}
