using UnityEngine;

public class Mask : MonoBehaviour
{
	private Texture maskTexture;

	private Sprite maskSprite;

	private tk2dSprite tk2dmask;

	private bool transformHasChanged;

	private Transform cachedTransform;

	public Transform CachedTransform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public Texture MainTexture
	{
		get
		{
			if (!maskTexture)
			{
				Renderer component = GetComponent<Renderer>();
				SpriteRenderer spriteRenderer = component as SpriteRenderer;
				if ((bool)spriteRenderer)
				{
					maskTexture = spriteRenderer.sprite.texture;
				}
				else if ((bool)component)
				{
					maskTexture = component.sharedMaterial.mainTexture;
				}
			}
			return maskTexture;
		}
	}

	public Sprite MaskSprite
	{
		get
		{
			if (!maskSprite)
			{
				Renderer component = GetComponent<Renderer>();
				SpriteRenderer spriteRenderer = component as SpriteRenderer;
				if ((bool)spriteRenderer)
				{
					maskSprite = spriteRenderer.sprite;
				}
			}
			return maskSprite;
		}
	}

	public tk2dSprite tk2dMaskSprite
	{
		get
		{
			if (!tk2dmask)
			{
				tk2dmask = GetComponent<tk2dSprite>();
			}
			return tk2dmask;
		}
	}

	public bool TransformHasChanged
	{
		get
		{
			return transformHasChanged;
		}
	}

	private void LateUpdate()
	{
		transformHasChanged = base.transform.hasChanged;
		base.transform.hasChanged = false;
	}
}
