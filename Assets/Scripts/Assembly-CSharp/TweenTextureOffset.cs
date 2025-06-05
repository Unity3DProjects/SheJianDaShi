using UnityEngine;

[AddComponentMenu("Inventain/Tween/TweenTextureOffset")]
public class TweenTextureOffset : Tweener
{
	public Vector2 beginOffset = Vector2.zero;

	public Vector2 endOffset = Vector2.one;

	private ColorQuad colorQuad;

	private GradientQuad gadientQuad;

	public Vector2 CurrentOffset
	{
		get
		{
			InitReference(false);
			if (colorQuad != null)
			{
				return colorQuad.Offset;
			}
			if (gadientQuad != null)
			{
				return gadientQuad.Offset;
			}
			return -1f * Vector2.one;
		}
		set
		{
			InitReference(false);
			if (colorQuad != null)
			{
				colorQuad.Offset = value;
			}
			else if (gadientQuad != null)
			{
				gadientQuad.Offset = value;
			}
		}
	}

	private void InitReference(bool force)
	{
		if (force || (colorQuad == null && gadientQuad == null))
		{
			colorQuad = base.gameObject.GetComponent<ColorQuad>();
			gadientQuad = base.gameObject.GetComponent<GradientQuad>();
		}
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		CurrentOffset = Vector2.Lerp(beginOffset, endOffset, factor);
	}

	public static TweenTextureOffset SetOffset(GameObject go, float duration, Vector2 offset)
	{
		TweenTextureOffset tweenTextureOffset = Tweener.InitGO<TweenTextureOffset>(go, duration);
		tweenTextureOffset.beginOffset = tweenTextureOffset.CurrentOffset;
		tweenTextureOffset.endOffset = offset;
		return tweenTextureOffset;
	}
}
