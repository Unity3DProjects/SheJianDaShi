using UnityEngine;

public class MaskSpineProperty
{
	public const int MAX_MASKED = 2;

	protected const string MASK_TEX = "_MainTex";

	protected const string MASKED_FILL_TEX = "_MaskedFill";

	protected const string MASKED_CUT_TEX = "_MaskedCut";

	protected const string RECT = "_Rect";

	protected const string VERT1 = "_Vert1";

	protected const string VERT2 = "_Vert2";

	protected const string VERT3 = "_Vert3";

	protected const string TRANSFORM = "_Transform";

	public int texture;

	public int rect;

	public int vert1;

	public int vert2;

	public int vert3;

	public int transform;

	private static MaskSpineProperty cachedPropertyMask = null;

	private static MaskSpineProperty[] cachedPropertyMaskedFill = new MaskSpineProperty[2];

	private static MaskSpineProperty[] cachedPropertyMaskedCutt = new MaskSpineProperty[2];

	protected MaskSpineProperty(string texName)
	{
		texture = Shader.PropertyToID(texName);
		rect = Shader.PropertyToID(texName + "_Rect");
		vert1 = Shader.PropertyToID(texName + "_Vert1");
		vert2 = Shader.PropertyToID(texName + "_Vert2");
		vert3 = Shader.PropertyToID(texName + "_Vert3");
		transform = Shader.PropertyToID(texName + "_Transform");
	}

	public static MaskSpineProperty CachePropertyMask()
	{
		if (cachedPropertyMask == null)
		{
			cachedPropertyMask = new MaskSpineProperty("_MainTex");
		}
		return cachedPropertyMask;
	}

	public static MaskSpineProperty CachePropertyMaskedFill(int i)
	{
		if (cachedPropertyMaskedFill[i] == null)
		{
			cachedPropertyMaskedFill[i] = new MaskSpineProperty("_MaskedFill" + i);
		}
		return cachedPropertyMaskedFill[i];
	}

	public static MaskSpineProperty CachePropertyMaskedCut(int i)
	{
		if (cachedPropertyMaskedCutt[i] == null)
		{
			cachedPropertyMaskedCutt[i] = new MaskSpineProperty("_MaskedCut" + i);
		}
		return cachedPropertyMaskedCutt[i];
	}
}
