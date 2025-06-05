using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaskedMaterial : MonoBehaviour
{
	protected static string[] MaskNames = new string[3] { "_Mask0", "_Mask1", "_Mask2" };

	protected static string[] TransformMaskNames = new string[3] { "_Mask0_Transform", "_Mask1_Transform", "_Mask2_Transform" };

	protected static string[] RectMaskNames = new string[3] { "_Mask0_Rect", "_Mask1_Rect", "_Mask2_Rect" };

	[SerializeField]
	private List<Mask> masks = new List<Mask>();

	protected Matrix4x4 maskMatrix;

	protected Material material;

	protected SpriteRenderer spriteRenderer;

	protected Texture mainTexture;

	protected int[] maskTransformIDs;

	protected const int MASK_MAX_COUNT = 3;

	public List<Mask> Masks
	{
		get
		{
			return masks;
		}
		set
		{
			masks = value;
			if ((bool)CachedMaterial)
			{
				UpdateMasks();
			}
		}
	}

	protected Material CachedMaterial
	{
		get
		{
			if (!material)
			{
				Renderer component = GetComponent<Renderer>();
				if ((bool)component)
				{
					if (Application.isPlaying)
					{
						material = component.material;
					}
					else
					{
						material = component.sharedMaterial;
					}
				}
			}
			return material;
		}
	}

	protected SpriteRenderer CachedSpriteRenderer
	{
		get
		{
			if (!spriteRenderer)
			{
				SpriteRenderer component = GetComponent<SpriteRenderer>();
				if ((bool)component)
				{
					spriteRenderer = component;
				}
			}
			return spriteRenderer;
		}
	}

	protected Texture MainTexture
	{
		get
		{
			if (!mainTexture)
			{
				if ((bool)CachedSpriteRenderer)
				{
					mainTexture = CachedSpriteRenderer.sprite.texture;
				}
				else
				{
					Renderer component = base.gameObject.GetComponent<Renderer>();
					if ((bool)component)
					{
						mainTexture = component.sharedMaterial.mainTexture;
					}
				}
			}
			return mainTexture;
		}
	}

	private void Awake()
	{
		maskTransformIDs = new int[3]
		{
			Shader.PropertyToID("_Mask0_Transform"),
			Shader.PropertyToID("_Mask1_Transform"),
			Shader.PropertyToID("_Mask2_Transform")
		};
	}

	private void Start()
	{
		if ((bool)CachedMaterial)
		{
			UpdateSprite();
			UpdateMasks();
		}
	}

	private void LateUpdate()
	{
		if (!CachedMaterial)
		{
			return;
		}
		int i = 0;
		for (int count = Masks.Count; i < count && i < 3; i++)
		{
			Mask mask = Masks[i];
			if ((bool)mask && mask.TransformHasChanged)
			{
				maskMatrix = mask.CachedTransform.worldToLocalMatrix;
				CachedMaterial.SetMatrix(maskTransformIDs[i], maskMatrix);
			}
		}
	}

	protected virtual void UpdateSprite()
	{
		Sprite sprite = CachedSpriteRenderer.sprite;
		if ((bool)sprite && (bool)MainTexture)
		{
			Vector4 value = new Vector4(sprite.rect.xMin / (float)MainTexture.width, sprite.rect.yMin / (float)MainTexture.height, sprite.rect.width / (float)MainTexture.width, sprite.rect.height / (float)MainTexture.height);
			CachedMaterial.SetFloat("_PixelsPerUnit", sprite.pixelsPerUnit);
			CachedMaterial.SetVector("_MainTex_Rect", value);
		}
	}

	protected virtual void UpdateMasks()
	{
		int count = Masks.Count;
		for (int i = 0; i < 3; i++)
		{
			if (i < count)
			{
				Mask mask = Masks[i];
				if ((bool)mask && (bool)mask.MainTexture)
				{
					maskMatrix = mask.CachedTransform.worldToLocalMatrix;
					Sprite maskSprite = mask.MaskSprite;
					Vector4 value = new Vector4(maskSprite.rect.xMin / (float)mask.MainTexture.width, maskSprite.rect.yMin / (float)mask.MainTexture.height, maskSprite.rect.width / (float)mask.MainTexture.width, maskSprite.rect.height / (float)mask.MainTexture.height);
					CachedMaterial.SetTexture(MaskNames[i], mask.MainTexture);
					CachedMaterial.SetMatrix(TransformMaskNames[i], maskMatrix);
					CachedMaterial.SetVector(RectMaskNames[i], value);
				}
			}
			else
			{
				CachedMaterial.SetTexture(MaskNames[i], Texture2D.blackTexture);
			}
		}
	}
}
