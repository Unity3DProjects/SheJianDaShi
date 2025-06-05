using System;
using UnityEngine;

public class WhatsNewPopup : GUIPopup
{
	[Serializable]
	private struct LanguageText
	{
		[TextArea]
		public string header;

		[TextArea]
		public string text;

		public SystemLanguage language;
	}

	private const string ANIMATION_SKELETON_NAME = "Skeleton";

	private const string ANIMATION_SKELETON_BEEFY_NAME = "SkeletonBeefy";

	private const string ANIMATION_SKELETON_PLUMPY_NAME = "SkeletonPlumpy";

	[SerializeField]
	private SkeletonAnimation leftArcherAnimation;

	[SerializeField]
	private SkeletonAnimation rightArcherAnimation;

	[SerializeField]
	private tk2dUIItem buttonRateUs;

	[SerializeField]
	private TweenPosition showTweener;

	[SerializeField]
	private tk2dUIItem closeButtonAdditional;

	[SerializeField]
	private GUILayoutCell descriptionCell;

	[SerializeField]
	private float descriptionCellSize;

	private void Start()
	{
		FadeSkeletons(leftArcherAnimation);
		FadeSkeletons(rightArcherAnimation);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		buttonRateUs.OnClick += ButtonsRateUs_OnClick;
		descriptionCell.FixedSizeValue = descriptionCellSize;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		buttonRateUs.OnClick -= ButtonsRateUs_OnClick;
	}

	protected override void Update()
	{
		base.Update();
		if (showTweener.gameObject.activeInHierarchy && showTweener.enabled)
		{
			showTweener.Update();
		}
	}

	public override void Hide(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		CustomPlayerPrefs.SetString("forced_rate_game", Application.version);
		base.Hide(onFinished, immediately, delay, callInvokes);
	}

	private void FadeSkeletons(SkeletonAnimation currentAnimation)
	{
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

	private void ButtonsRateUs_OnClick()
	{
		SingletonMonoBehaviour<RateMeManager>.Instance.RateAppAction();
		Hide();
	}
}
