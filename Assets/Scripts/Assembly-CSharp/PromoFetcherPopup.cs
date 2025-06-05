using System;
using UnityEngine;
using UnityEngine.Analytics;

public class PromoFetcherPopup : GUIPopup
{
	private const string HEADER_LIKE_KEY = "ui.popup.promofetcher.like.header";

	private const string TOP_LABEL_LIKE_KEY = "ui.popup.promofetcher.like.top.label";

	private const string BOTTOM_LABEL_LIKE_KEY = "ui.popup.promofetcher.like.bottom.label";

	private const string BUTTON_LIKE_KEY = "ui.popup.promofetcher.like.button";

	private const string SPRITE_LIKE_KEY = "icon_give_like";

	private const string HEADER_COMMENT_KEY = "ui.popup.promofetcher.comment.header";

	private const string TOP_LABEL_COMMENT_KEY = "ui.popup.promofetcher.comment.top.label";

	private const string BOTTOM_LABEL_COMMENT_KEY = "ui.popup.promofetcher.comment.bottom.label";

	private const string BUTTON_COMMENT_KEY = "ui.popup.promofetcher.comment.button";

	private const string SPRITE_COMMENT_KEY = "icon_comment";

	[SerializeField]
	private tk2dTextMesh fetcherHeader;

	[SerializeField]
	private tk2dTextMesh fetcherTopLabel;

	[SerializeField]
	private tk2dTextMesh fetcherBottomLabel;

	[SerializeField]
	private tk2dSprite[] fetcherLabelIcons;

	[SerializeField]
	private tk2dSlicedSprite cloudLabelIcons;

	[SerializeField]
	private GUILayoutCell topLabelCell;

	[SerializeField]
	private Color purpleColor;

	[SerializeField]
	private tk2dTextMesh coinsLabel;

	[SerializeField]
	private tk2dTextMesh gemsLabel;

	[SerializeField]
	private tk2dTextMesh fetcherButtonLabel;

	[SerializeField]
	private tk2dSprite fetcherButtonSprite;

	[SerializeField]
	private tk2dUIItem fetcherButton;

	[SerializeField]
	private GUILayoutCell buttonLabelCell;

	[SerializeField]
	private int coinsReward = 500;

	[SerializeField]
	private int gemsReward = 5;

	private string urlLink;

	private LLPromoType promoType;

	protected override void OnEnable()
	{
		base.OnEnable();
		fetcherButton.OnClick += FetcherButton_OnClick;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		fetcherButton.OnClick -= FetcherButton_OnClick;
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		LLPromoFetcherUnit lLPromoFetcherUnit = LLPromoFetcher.PromoFetcherActiveUnit();
		urlLink = lLPromoFetcherUnit.promoURL;
		promoType = lLPromoFetcherUnit.promoType;
		coinsLabel.text = string.Format("+ {0}", coinsReward);
		gemsLabel.text = string.Format("+ {0}", gemsReward);
		if (string.IsNullOrEmpty(urlLink))
		{
			return;
		}
		switch (promoType)
		{
		case LLPromoType.Comment:
		{
			fetcherHeader.text = "ui.popup.promofetcher.comment.header";
			fetcherTopLabel.text = "ui.popup.promofetcher.comment.top.label";
			fetcherTopLabel.color = purpleColor;
			fetcherBottomLabel.text = "ui.popup.promofetcher.comment.bottom.label";
			fetcherBottomLabel.color = purpleColor;
			fetcherButtonLabel.text = "ui.popup.promofetcher.comment.button";
			fetcherButtonSprite.SetSprite("icon_comment");
			tk2dSprite[] array2 = fetcherLabelIcons;
			foreach (tk2dSprite tk2dSprite3 in array2)
			{
				tk2dSprite3.gameObject.SetActive(false);
			}
			cloudLabelIcons.gameObject.SetActive(true);
			buttonLabelCell.SizeValue = 120f;
			topLabelCell.SizeValue = 214f;
			break;
		}
		case LLPromoType.Like:
		{
			fetcherHeader.text = "ui.popup.promofetcher.like.header";
			fetcherTopLabel.text = "ui.popup.promofetcher.like.top.label";
			fetcherBottomLabel.text = "ui.popup.promofetcher.like.bottom.label";
			fetcherButtonLabel.text = "ui.popup.promofetcher.like.button";
			fetcherButtonSprite.SetSprite("icon_give_like");
			tk2dSprite[] array = fetcherLabelIcons;
			foreach (tk2dSprite tk2dSprite2 in array)
			{
				tk2dSprite2.gameObject.SetActive(true);
			}
			cloudLabelIcons.gameObject.SetActive(false);
			buttonLabelCell.SizeValue = 60f;
			topLabelCell.SizeValue = 160f;
			break;
		}
		}
		base.Show(onFinished, immediately, delay, callInvokes);
	}

	protected override void CloseButton_OnClick()
	{
		LLPromoFetcher.visitActiveUnit(LLPromoVisitType.Skipped);
		string text = ((promoType != LLPromoType.Like) ? "comment_post_decline" : "like_post_decline");
		base.CachedManagers.CachedStatisticsManager.SendEvent(text);
		Analytics.CustomEvent(text);
		base.CloseButton_OnClick();
	}

	private void FetcherButton_OnClick()
	{
		base.CachedManagers.CachedStatisticsManager.AddCoins(coinsReward);
		base.CachedManagers.CachedStatisticsManager.AddGems(gemsReward);
		if (!string.IsNullOrEmpty(urlLink))
		{
			LLPromoFetcher.visitActiveUnit(LLPromoVisitType.Opened);
			string text = ((promoType != LLPromoType.Like) ? "comment_post_get" : "like_post_get");
			base.CachedManagers.CachedStatisticsManager.SendEvent(text);
			Analytics.CustomEvent(text);
			Application.OpenURL(urlLink);
		}
		Hide();
	}
}
