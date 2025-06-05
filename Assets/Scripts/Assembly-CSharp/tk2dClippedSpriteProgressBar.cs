using UnityEngine;

public class tk2dClippedSpriteProgressBar : MonoBehaviour
{
	public enum ProgressDirection
	{
		Vertical = 0,
		Horizontal = 1
	}

	[SerializeField]
	private tk2dClippedSprite clippedSprite;

	[SerializeField]
	private bool isInversed;

	[SerializeField]
	private ProgressDirection direction = ProgressDirection.Horizontal;

	[SerializeField]
	private float fillAmount;

	public float FillAmount
	{
		get
		{
			return fillAmount;
		}
		set
		{
			fillAmount = Mathf.Clamp01(value);
			UpdateState();
		}
	}

	private void UpdateState()
	{
		if (!(clippedSprite != null))
		{
			return;
		}
		if (direction == ProgressDirection.Horizontal)
		{
			if (isInversed)
			{
				clippedSprite.ClipRect = new Rect(1f, 0f, -1f * fillAmount, 1f);
			}
			else
			{
				clippedSprite.ClipRect = new Rect(0f, 0f, 1f * fillAmount, 1f);
			}
		}
		else if (direction == ProgressDirection.Vertical)
		{
			if (isInversed)
			{
				clippedSprite.ClipRect = new Rect(0f, 1f, 1f, -1f * fillAmount);
			}
			else
			{
				clippedSprite.ClipRect = new Rect(0f, 0f, 1f, 1f * fillAmount);
			}
		}
	}
}
