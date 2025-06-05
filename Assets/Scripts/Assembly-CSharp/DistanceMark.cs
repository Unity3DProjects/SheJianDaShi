using UnityEngine;

public class DistanceMark : MonoBehaviour
{
	[SerializeField]
	private tk2dTextMesh labelDistance;

	[SerializeField]
	private tk2dTextMesh labelM;

	[SerializeField]
	private tk2dSprite spriteIcon;

	[SerializeField]
	private tk2dSprite spriteBack;

	private int distance;

	private bool isFlipped;

	private bool isVisible;

	public bool IsFlipped
	{
		get
		{
			return isFlipped;
		}
		set
		{
			if (value != isFlipped)
			{
				isFlipped = value;
				spriteBack.transform.localScale *= -1f;
				Vector3 localPosition = spriteBack.transform.localPosition;
				localPosition.x *= -1f;
				localPosition.y *= -1f;
				spriteBack.transform.localPosition = localPosition;
			}
		}
	}

	public bool IsVisible
	{
		get
		{
			return isVisible;
		}
		set
		{
			if (value != isVisible)
			{
				isVisible = value;
				base.gameObject.SetActive(isVisible);
			}
		}
	}

	public int Distance
	{
		get
		{
			return distance;
		}
		set
		{
			if (value != distance)
			{
				distance = value;
				labelDistance.text = distance.ToString();
			}
		}
	}

	public tk2dSprite SpriteIcon
	{
		get
		{
			return spriteIcon;
		}
	}

	private void Awake()
	{
		if (tk2dSystem.IsRetina)
		{
			labelDistance.transform.localPosition *= 2f;
			labelM.transform.localPosition *= 2f;
			spriteIcon.transform.localPosition *= 2f;
			spriteBack.transform.localPosition *= 2f;
		}
		IsVisible = false;
	}
}
