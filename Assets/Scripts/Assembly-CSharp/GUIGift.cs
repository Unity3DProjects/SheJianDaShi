using UnityEngine;

public class GUIGift : MonoBehaviour
{
	private const byte DISABLE_R = 90;

	private const byte DISABLE_G = 0;

	private const byte DISABLE_B = 100;

	private const byte DISABLE_A = byte.MaxValue;

	private const byte ACTIVE_COUNTER_R = byte.MaxValue;

	private const byte ACTIVE_COUNTER_G = 70;

	private const byte ACTIVE_COUNTER_B = 109;

	private const byte ACTIVE_COUNTER_A = byte.MaxValue;

	private const byte INACTIVE_COUNTER_R = 138;

	private const byte INACTIVE_COUNTER_G = 66;

	private const byte INACTIVE_COUNTER_B = 155;

	private const byte INACTIVE_COUNTER_A = byte.MaxValue;

	private const byte INACTIVE_COUNTER_LABEL_R = 202;

	private const byte INACTIVE_COUNTER_LABEL_G = 104;

	private const byte INACTIVE_COUNTER_LABEL_B = 216;

	private const byte INACTIVE_COUNTER_LABEL_A = byte.MaxValue;

	private const float DISABLE_GRAYSCALE = 1f;

	private const string DISABLE_SHADER = "tk2d/BlendVertexColorGrayscaleColorize";

	private const string DEFAULT_SHADER = "tk2d/PremulVertexColor";

	private const string DISABLE_GRAYSCALE_PROPERTY = "_GrayscaleFactor";

	private const string ICON_BG_STANDART = "daily_gift_standart_circle";

	private const string ICON_BG_TODAY = "daily_gift_today_reward";

	private const float ICON_ACTIVE_SCALE = 1.1f;

	[SerializeField]
	private int numberGift;

	[SerializeField]
	private tk2dSprite iconGift;

	[SerializeField]
	private tk2dSprite backgroundCircleSprite;

	[SerializeField]
	private tk2dTextMesh priceLabel;

	[SerializeField]
	private tk2dSlicedSprite priceBackSprite;

	[SerializeField]
	private GameObject rootDay;

	[SerializeField]
	private GameObject rootCheck;

	[SerializeField]
	private GameObject rootIcon;

	[SerializeField]
	private tk2dSlicedSprite backSprite;

	[SerializeField]
	private tk2dTextMesh todayLabel;

	[SerializeField]
	private tk2dSlicedSprite todayBackSprite;

	[SerializeField]
	private GameObject rootToday;

	[SerializeField]
	private GameObject rootPrice;

	private GiftInfo currentGiftInfo;

	public int NumberGift
	{
		get
		{
			return numberGift;
		}
	}

	public GiftInfo Info
	{
		get
		{
			return currentGiftInfo;
		}
	}

	public tk2dSprite IconGift
	{
		get
		{
			return iconGift;
		}
	}

	public void SetGiftByInfo(GiftInfo info, int currentNumberGift)
	{
		currentGiftInfo = info;
		rootPrice.SetActive(info.type == GiftType.Coins || info.type == GiftType.Gems);
		priceLabel.text = info.CoinsValue.ToString();
		iconGift.SetSprite(info.spriteName);
		backSprite.gameObject.SetActive(true);
		todayBackSprite.gameObject.SetActive(false);
		rootToday.SetActive(false);
		rootIcon.transform.localScale = Vector3.one;
		backgroundCircleSprite.SetSprite("daily_gift_standart_circle");
		backgroundCircleSprite.scale = Vector3.one;
		priceBackSprite.color = new Color32(byte.MaxValue, 70, 109, byte.MaxValue);
		priceLabel.color = Color.white;
		if (numberGift < currentNumberGift)
		{
			rootCheck.gameObject.SetActive(true);
			rootDay.gameObject.SetActive(false);
			priceBackSprite.color = new Color32(138, 66, 155, byte.MaxValue);
			priceLabel.color = new Color32(202, 104, 216, byte.MaxValue);
		}
		else if (numberGift == currentNumberGift)
		{
			rootCheck.gameObject.SetActive(false);
			rootDay.gameObject.SetActive(false);
			if (SingletonMonoBehaviour<GiftManager>.Instance.IsGiftAvailable)
			{
				backSprite.gameObject.SetActive(false);
				todayBackSprite.gameObject.SetActive(true);
				backgroundCircleSprite.SetSprite("daily_gift_today_reward");
				rootIcon.transform.localScale *= 1.1f;
				rootToday.SetActive(true);
			}
			else
			{
				rootCheck.gameObject.SetActive(false);
				rootDay.gameObject.SetActive(true);
			}
		}
		else
		{
			rootCheck.gameObject.SetActive(false);
			rootDay.gameObject.SetActive(true);
		}
	}

	public void SetGiftByInfoAndStyle(GiftInfo info, GiftStateType giftStyle)
	{
		currentGiftInfo = info;
		rootPrice.SetActive(info.type == GiftType.Coins || info.type == GiftType.Gems);
		priceLabel.text = info.CoinsValue.ToString();
		iconGift.SetSprite(info.spriteName);
		backSprite.gameObject.SetActive(true);
		todayBackSprite.gameObject.SetActive(false);
		rootToday.SetActive(false);
		rootIcon.transform.localScale = Vector3.one;
		backgroundCircleSprite.SetSprite("daily_gift_today_reward");
		backgroundCircleSprite.scale = Vector3.one;
		priceBackSprite.color = new Color32(byte.MaxValue, 70, 109, byte.MaxValue);
		priceLabel.color = Color.white;
		switch (giftStyle)
		{
		case GiftStateType.Claimed:
			backgroundCircleSprite.SetSprite("daily_gift_standart_circle");
			rootCheck.gameObject.SetActive(true);
			rootDay.gameObject.SetActive(false);
			priceBackSprite.color = new Color32(138, 66, 155, byte.MaxValue);
			priceLabel.color = new Color32(202, 104, 216, byte.MaxValue);
			break;
		case GiftStateType.Today:
			rootDay.gameObject.SetActive(false);
			rootCheck.gameObject.SetActive(false);
			backSprite.gameObject.SetActive(false);
			todayBackSprite.gameObject.SetActive(true);
			backgroundCircleSprite.SetSprite("daily_gift_today_reward");
			rootIcon.transform.localScale *= 1.1f;
			rootToday.SetActive(true);
			todayLabel.text = "ui.gift.label.today";
			break;
		case GiftStateType.NextDay:
		{
			rootDay.gameObject.SetActive(false);
			rootCheck.gameObject.SetActive(false);
			backSprite.gameObject.SetActive(false);
			todayBackSprite.gameObject.SetActive(true);
			float x = backgroundCircleSprite.GetUntrimmedBounds().size.x;
			float y = backgroundCircleSprite.GetUntrimmedBounds().size.y;
			backgroundCircleSprite.SetSprite("daily_gift_today_reward");
			backgroundCircleSprite.scale = new Vector3(0.47f, 0.45f, 1f);
			rootIcon.transform.localScale *= 1.1f;
			rootToday.SetActive(true);
			todayLabel.text = "ui.gift.label.nextday";
			break;
		}
		case GiftStateType.FutureDay:
			backgroundCircleSprite.SetSprite("daily_gift_standart_circle");
			rootCheck.gameObject.SetActive(false);
			rootDay.gameObject.SetActive(true);
			break;
		}
	}

	public void Fade(bool isFade = true)
	{
		if (isFade)
		{
			iconGift.CachedRenderer.material.shader = Shader.Find("tk2d/BlendVertexColorGrayscaleColorize");
			iconGift.color = new Color32(90, 0, 100, byte.MaxValue);
			iconGift.CachedRenderer.material.SetFloat("_GrayscaleFactor", 1f);
		}
		else
		{
			iconGift.CachedRenderer.material.shader = Shader.Find("tk2d/PremulVertexColor");
			iconGift.color = Color.white;
		}
	}
}
