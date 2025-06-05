using UnityEngine;

[RequireComponent(typeof(tk2dSprite))]
public class TweenTk2dSpriteScale : Tweener
{
	private tk2dSprite sprite;

	[SerializeField]
	private Vector3 endScale;

	[SerializeField]
	private Vector3 beginScale;

	public Vector3 EndScale
	{
		get
		{
			return endScale;
		}
		set
		{
			endScale = value;
		}
	}

	public Vector3 BeginScale
	{
		get
		{
			return beginScale;
		}
		set
		{
			beginScale = value;
		}
	}

	private tk2dSprite Sprite
	{
		get
		{
			if (sprite == null)
			{
				sprite = GetComponent<tk2dSprite>();
			}
			return sprite;
		}
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		Sprite.scale = BeginScale + (EndScale - BeginScale) * factor;
	}

	protected override void TweenUpdateEditor(float factor)
	{
		Sprite.scale = BeginScale + (EndScale - BeginScale) * factor;
	}
}
