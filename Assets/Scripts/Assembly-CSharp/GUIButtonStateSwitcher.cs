using UnityEngine;

[RequireComponent(typeof(tk2dUIItem))]
public class GUIButtonStateSwitcher : MonoBehaviour, ILayoutCellHandler
{
	public enum ButtonState
	{
		Released = 0,
		Pressed = 1
	}

	[SerializeField]
	private tk2dBaseSprite buttonSprite;

	[SerializeField]
	private string normalSpriteName;

	[SerializeField]
	private string pressedSpriteName;

	[SerializeField]
	private string blinkSpriteName;

	[SerializeField]
	private float blinkSpeed = 0.5f;

	[SerializeField]
	private Transform contentRoot;

	[SerializeField]
	private float pressedOffset;

	private Vector2 initialDimensions;

	private Vector3 initialLocalPosition = Vector3.zero;

	private float initialY;

	private ButtonState CurrentState;

	private float currentBlinkTime;

	private tk2dUIItem uiitem;

	private tk2dUIItem UIItem
	{
		get
		{
			if (uiitem == null)
			{
				uiitem = GetComponent<tk2dUIItem>();
			}
			return uiitem;
		}
	}

	public string NormalSpriteName
	{
		get
		{
			return normalSpriteName;
		}
		set
		{
			normalSpriteName = value;
			buttonSprite.SetSprite(normalSpriteName);
		}
	}

	public string PressedSpriteName
	{
		get
		{
			return pressedSpriteName;
		}
		set
		{
			pressedSpriteName = value;
		}
	}

	private bool IsSlicedSprite
	{
		get
		{
			return buttonSprite is tk2dSlicedSprite;
		}
	}

	private tk2dSlicedSprite SlicedSprite
	{
		get
		{
			return buttonSprite as tk2dSlicedSprite;
		}
	}

	private void Awake()
	{
		if (tk2dSystem.IsRetina)
		{
			pressedOffset *= 2f;
		}
	}

	private void Update()
	{
		if (currentBlinkTime >= blinkSpeed)
		{
			if (CurrentState != ButtonState.Pressed && !string.IsNullOrEmpty(blinkSpriteName))
			{
				buttonSprite.SetSprite((!buttonSprite.CurrentSprite.name.Equals(normalSpriteName)) ? normalSpriteName : blinkSpriteName);
				currentBlinkTime = 0f;
			}
		}
		else
		{
			currentBlinkTime += Time.deltaTime;
		}
	}

	private void OnEnable()
	{
		UIItem.OnDown += UIItem_OnDown;
		UIItem.OnRelease += UIItem_OnRelease;
		buttonSprite.SetSprite(normalSpriteName);
		if (contentRoot != null)
		{
			initialY = contentRoot.localPosition.y;
		}
	}

	private void OnDisable()
	{
		UIItem.OnDown -= UIItem_OnDown;
		UIItem.OnRelease -= UIItem_OnRelease;
		if (contentRoot != null)
		{
			contentRoot.SetLocalPositionY(initialY);
		}
		if (IsSlicedSprite)
		{
			SlicedSprite.dimensions = initialDimensions;
			SlicedSprite.transform.localPosition = initialLocalPosition;
		}
		CurrentState = ButtonState.Released;
	}

	public void RepositionForCell(LayoutCellInfo info)
	{
		if (IsSlicedSprite)
		{
			initialDimensions = SlicedSprite.dimensions;
		}
	}

	private void UIItem_OnRelease()
	{
		buttonSprite.SetSprite(normalSpriteName);
		CurrentState = ButtonState.Released;
		if (contentRoot != null)
		{
			contentRoot.SetLocalPositionY(initialY);
		}
		if (IsSlicedSprite)
		{
			SlicedSprite.dimensions = initialDimensions;
			SlicedSprite.transform.localPosition = initialLocalPosition;
		}
	}

	private void UIItem_OnDown()
	{
		buttonSprite.SetSprite(pressedSpriteName);
		CurrentState = ButtonState.Pressed;
		if (contentRoot != null)
		{
			initialY = contentRoot.localPosition.y;
			contentRoot.SetLocalPositionY(contentRoot.localPosition.y + ((!IsSlicedSprite) ? pressedOffset : ((float)(Mathf.FloorToInt(pressedOffset / 2f) + Mathf.FloorToInt(pressedOffset / 4f)))));
		}
		if (IsSlicedSprite)
		{
			initialDimensions = SlicedSprite.dimensions;
			SlicedSprite.dimensions = new Vector2(SlicedSprite.dimensions.x, SlicedSprite.dimensions.y + pressedOffset);
			SlicedSprite.transform.SetLocalPositionY(SlicedSprite.transform.localPosition.y + (float)Mathf.FloorToInt(pressedOffset / 2f));
		}
	}
}
