using UnityEngine;

public class NotificationButton : MonoBehaviour
{
	[SerializeField]
	private GameObject bubbleSprite;

	[SerializeField]
	private GameObject buttonCellObj;

	[SerializeField]
	private tk2dTextMesh counterLabel;

	[SerializeField]
	private tk2dUIItem buttonItem;

	[SerializeField]
	private tk2dSlicedSprite backSprite;

	private int unreadMsgCounter;

	private static Vector2 correctDimensions = Vector2.zero;

	private void Awake()
	{
		float num = ((!tk2dSystem.IsRetina) ? 1 : 2);
		GUILayoutCell component = buttonCellObj.GetComponent<GUILayoutCell>();
		bubbleSprite.transform.SetLocalPositionX(component.RecievedRect.width * 0.22f);
		bubbleSprite.transform.SetLocalPositionY(component.RecievedRect.height * 0.47f);
		counterLabel.transform.SetLocalPositionX(-1.8f * num);
		counterLabel.transform.SetLocalPositionY(-1f * num);
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		enable();
		if (correctDimensions == Vector2.zero && backSprite.dimensions != Vector2.zero)
		{
			correctDimensions = backSprite.dimensions;
		}
		show(false);
	}

	private void Start()
	{
		setUnreadMessagesCounter(BowmastersNewsfeed.Instance.NrOfMessagesAvailable);
		show(BowmastersNewsfeed.Instance.MessagesAvailability);
	}

	public void setUnreadMessagesCounter(uint counter)
	{
		counterLabel.text = counter.ToString();
		if (counter == 0)
		{
			bubbleSprite.SetActive(false);
		}
		else
		{
			bubbleSprite.SetActive(true);
		}
	}

	public void disable()
	{
		buttonItem.OnClick -= NotificationButton_OnClick;
	}

	public void enable()
	{
		buttonItem.OnClick += NotificationButton_OnClick;
	}

	public void show(bool show)
	{
		buttonCellObj.SetActive(show);
		if (show)
		{
			backSprite.dimensions = correctDimensions;
		}
	}

	private void NotificationButton_OnClick()
	{
		BowmastersNewsfeed.Instance.ShowBoard();
	}
}
