using UnityEngine;

public class tk2dMaskedMaterial : MaskedMaterial
{
	[SerializeField]
	protected Shader shader;

	protected tk2dSprite tk2dSprite;

	private const float PIXEL_PER_UNIT = 1f;

	private Matrix4x4 tk2dflip = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 270f), new Vector3(-1f, 1f, 1f));

	protected tk2dSprite Cachedtk2dSprite
	{
		get
		{
			if (!tk2dSprite)
			{
				tk2dSprite = GetComponent<tk2dSprite>();
			}
			return tk2dSprite;
		}
	}

	protected override void UpdateSprite()
	{
		if ((bool)Cachedtk2dSprite && (bool)base.MainTexture)
		{
			Rect rect = Cachedtk2dSprite.GetRect();
			Vector4 value = new Vector4(rect.xMin / (float)base.MainTexture.width, rect.yMin / (float)base.MainTexture.height, rect.width / (float)base.MainTexture.width, rect.height / (float)base.MainTexture.height);
			base.CachedMaterial.shader = shader;
			base.CachedMaterial.SetFloat("_PixelsPerUnit", 1f);
			base.CachedMaterial.SetVector("_MainTex_Rect", value);
		}
	}

	protected override void UpdateMasks()
	{
		int count = base.Masks.Count;
		for (int i = 0; i < 3; i++)
		{
			if (i < count)
			{
				Mask mask = base.Masks[i];
				if ((bool)mask && (bool)mask.MainTexture)
				{
					maskMatrix = mask.CachedTransform.worldToLocalMatrix;
					tk2dSprite tk2dMaskSprite = mask.tk2dMaskSprite;
					Rect rect = tk2dMaskSprite.GetRect();
					Vector4 value = new Vector4(rect.xMin / (float)mask.MainTexture.width, rect.yMin / (float)mask.MainTexture.height, rect.width / (float)mask.MainTexture.width, rect.height / (float)mask.MainTexture.height);
					Vector3 vector = tk2dMaskSprite.CurrentSprite.boundsData[0] * -1f;
					maskMatrix.m03 += vector.x;
					maskMatrix.m13 += vector.y;
					maskMatrix.m23 += vector.z;
					if (tk2dMaskSprite.CurrentSprite.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
					{
						value = new Vector4(rect.xMin / (float)mask.MainTexture.width, rect.yMin / (float)mask.MainTexture.height, rect.height / (float)mask.MainTexture.width, rect.width / (float)mask.MainTexture.height);
						maskMatrix = tk2dflip * maskMatrix;
					}
					else if (tk2dMaskSprite.CurrentSprite.flipped == tk2dSpriteDefinition.FlipMode.TPackerCW)
					{
						Debug.LogError(" not implemented ");
					}
					base.CachedMaterial.SetTexture(MaskedMaterial.MaskNames[i], mask.MainTexture);
					base.CachedMaterial.SetMatrix(MaskedMaterial.TransformMaskNames[i], maskMatrix);
					base.CachedMaterial.SetVector(MaskedMaterial.RectMaskNames[i], value);
				}
			}
			else
			{
				base.CachedMaterial.SetTexture(MaskedMaterial.MaskNames[i], Texture2D.blackTexture);
			}
		}
	}
}
