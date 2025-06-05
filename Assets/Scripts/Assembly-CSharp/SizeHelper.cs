using UnityEngine;

public static class SizeHelper
{
	private static float factor = -1f;

	private static float heightFactor = -1f;

	private static float widthFactor = -1f;

	private static float maxFactor = -1f;

	private static float minFactor = -1f;

	public static float Factor
	{
		get
		{
			if (factor < 0f)
			{
				factor = HeightFactor / WidthFactor;
			}
			return factor;
		}
	}

	public static Vector2 Factors
	{
		get
		{
			return new Vector2(WidthFactor, HeightFactor);
		}
	}

	public static float HeightFactor
	{
		get
		{
			if (heightFactor < 0f)
			{
				heightFactor = (float)ScreenDimentions.Height / (float)ScriptableSingleton<SizeHelperSettings>.Instance.baseHeight;
			}
			return heightFactor;
		}
	}

	public static float WidthFactor
	{
		get
		{
			if (widthFactor < 0f)
			{
				widthFactor = (float)ScreenDimentions.Width / (float)ScriptableSingleton<SizeHelperSettings>.Instance.baseWidth;
			}
			return widthFactor;
		}
	}

	public static float MaxFactor
	{
		get
		{
			if (maxFactor < 0f)
			{
				maxFactor = Mathf.Max(HeightFactor, WidthFactor);
			}
			return maxFactor;
		}
	}

	public static float MinFactor
	{
		get
		{
			if (minFactor < 0f)
			{
				minFactor = Mathf.Min(HeightFactor, WidthFactor);
			}
			return minFactor;
		}
	}

	private static float RoundFloat(float value, RoundFloatEnum roundPreference)
	{
		switch (roundPreference)
		{
		case RoundFloatEnum.RoundToInteger:
		case RoundFloatEnum.RoundTo1DiginAfterPoint:
		case RoundFloatEnum.RoundTo2DiginAfterPoint:
		case RoundFloatEnum.RoundTo3DiginAfterPoint:
		case RoundFloatEnum.RoundTo4DiginAfterPoint:
			value *= Mathf.Pow(10f, (float)roundPreference);
			value = (int)((Mathf.Abs(value) + 0.5f) * Mathf.Sign(value));
			value /= Mathf.Pow(10f, (float)roundPreference);
			break;
		}
		return value;
	}

	private static Vector2 RoundVector2(Vector2 vector, RoundFloatEnum roundPreference)
	{
		switch (roundPreference)
		{
		case RoundFloatEnum.RoundToInteger:
		case RoundFloatEnum.RoundTo1DiginAfterPoint:
		case RoundFloatEnum.RoundTo2DiginAfterPoint:
		case RoundFloatEnum.RoundTo3DiginAfterPoint:
		case RoundFloatEnum.RoundTo4DiginAfterPoint:
		{
			vector.Scale(Vector2.one * Mathf.Pow(10f, (float)roundPreference));
			for (int i = 0; i < 2; i++)
			{
				vector[i] = (int)((Mathf.Abs(vector[i]) + 0.5f) * Mathf.Sign(vector[i]));
			}
			vector.Scale(Vector2.one / Mathf.Pow(10f, (float)roundPreference));
			break;
		}
		}
		return vector;
	}

	private static Vector3 RoundVector3(Vector3 vector, RoundFloatEnum roundPreference)
	{
		switch (roundPreference)
		{
		case RoundFloatEnum.RoundToInteger:
		case RoundFloatEnum.RoundTo1DiginAfterPoint:
		case RoundFloatEnum.RoundTo2DiginAfterPoint:
		case RoundFloatEnum.RoundTo3DiginAfterPoint:
		case RoundFloatEnum.RoundTo4DiginAfterPoint:
		{
			vector.Scale(Vector3.one * Mathf.Pow(10f, (float)roundPreference));
			for (int i = 0; i < 3; i++)
			{
				vector[i] = (int)((Mathf.Abs(vector[i]) + 0.5f) * Mathf.Sign(vector[i]));
			}
			vector.Scale(Vector3.one / Mathf.Pow(10f, (float)roundPreference));
			break;
		}
		}
		return vector;
	}

	private static Vector3 RecalculateByFactor(Vector3 baseValue, SizeFactor type)
	{
		switch (type)
		{
		case SizeFactor.MaxFactor:
			baseValue.x *= MaxFactor;
			baseValue.y *= MaxFactor;
			break;
		case SizeFactor.MinFactor:
			baseValue.x *= MinFactor;
			baseValue.y *= MinFactor;
			break;
		case SizeFactor.TwoFactor:
			baseValue.x *= WidthFactor;
			baseValue.y *= HeightFactor;
			break;
		case SizeFactor.HeightFactor:
			baseValue.x *= HeightFactor;
			baseValue.y *= HeightFactor;
			break;
		case SizeFactor.WidthFactor:
			baseValue.x *= WidthFactor;
			baseValue.y *= WidthFactor;
			break;
		case SizeFactor.MirrorTwoFactor:
			baseValue.x *= HeightFactor;
			baseValue.y *= WidthFactor;
			break;
		case SizeFactor.OnlyHeightFactor:
			baseValue.y *= HeightFactor;
			break;
		case SizeFactor.OnlyWidhtFactor:
			baseValue.x *= WidthFactor;
			break;
		}
		return baseValue;
	}

	private static Vector3 RecalculateByFit(Vector3 baseValue, Fit type, bool half)
	{
		switch (type)
		{
		case Fit.Height:
			baseValue.y = ((!half) ? ((float)ScreenDimentions.Height) : ((float)ScreenDimentions.Height / 2f));
			break;
		case Fit.Width:
			baseValue.x = ((!half) ? ((float)ScreenDimentions.Width) : ((float)ScreenDimentions.Width / 2f));
			break;
		case Fit.Full:
			baseValue.x = ((!half) ? ((float)ScreenDimentions.Width) : ((float)ScreenDimentions.Width / 2f));
			baseValue.y = ((!half) ? ((float)ScreenDimentions.Height) : ((float)ScreenDimentions.Height / 2f));
			break;
		}
		return baseValue;
	}

	public static Vector3 RecalculatePosition(Vector3 position, SizeFactor factorType, RoundFloatEnum roundPreference = RoundFloatEnum.DontRoundFloat)
	{
		return RoundVector3(RecalculateByFactor(position, factorType), roundPreference);
	}

	private static float RecalculateValue(float value, SizeFactor factorType)
	{
		switch (factorType)
		{
		case SizeFactor.MaxFactor:
			value *= MaxFactor;
			break;
		case SizeFactor.MinFactor:
			value *= MinFactor;
			break;
		case SizeFactor.HeightFactor:
			value *= HeightFactor;
			break;
		case SizeFactor.WidthFactor:
			value *= WidthFactor;
			break;
		}
		return value;
	}

	public static void RecalculatePosition(Transform transform, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(transform == null))
		{
			transform.localPosition = RecalculateByFactor(transform.localPosition, factorType);
			transform.localPosition = RoundVector3(transform.localPosition, roundPreference);
		}
	}

	public static void RecalculateScale(Transform transform, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(transform == null))
		{
			transform.localScale = RecalculateByFactor(transform.localScale, factorType);
			transform.localScale = RoundVector3(transform.localScale, roundPreference);
		}
	}

	public static void RecalculateFOV(Camera camera, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(camera == null))
		{
			camera.fieldOfView = RecalculateValue(camera.fieldOfView, factorType);
			camera.fieldOfView = RoundFloat(camera.fieldOfView, roundPreference);
		}
	}

	public static void RecalculateMask(tk2dUIMask mask, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(mask == null))
		{
			mask.size = RecalculateByFactor(mask.size, factorType);
			mask.size = RoundVector2(mask.size, roundPreference);
			mask.Build();
		}
	}

	public static void RecalculateGrid(Grid grid, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(grid == null))
		{
			Vector2 vector = new Vector2(grid.cellWidth, grid.cellHeight);
			vector = RecalculateByFactor(vector, factorType);
			vector = RoundVector2(vector, roundPreference);
			grid.cellWidth = vector.x;
			grid.cellHeight = vector.y;
		}
	}

	public static void RecalculateCollider(BoxCollider collider, SizeFactor factorType, Fit fitType, RoundFloatEnum roundPreference)
	{
		if (!(collider == null))
		{
			collider.size = RecalculateByFactor(collider.size, factorType);
			collider.size = RecalculateByFit(collider.size, fitType, false);
			collider.size = RoundVector3(collider.size, roundPreference);
			collider.center = RecalculateByFactor(collider.center, factorType);
			collider.center = RoundVector3(collider.center, roundPreference);
		}
	}

	public static void RecalculateSizeParticleSystem(ParticleSystem particleSystem, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(particleSystem == null))
		{
			RecalculateScale(particleSystem.transform, factorType, roundPreference);
			ParticleSystem.MainModule main = particleSystem.main;
			main.startSizeMultiplier = RecalculateValue(main.startSizeMultiplier, factorType);
			main.startSizeMultiplier = RoundFloat(main.startSizeMultiplier, roundPreference);
		}
	}

	public static void RecalculateSizeColorQuad(ColorQuad quad, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(quad == null))
		{
			quad.Size = RecalculateByFactor(quad.Size, factorType);
			quad.Size = RoundVector2(quad.Size, roundPreference);
		}
	}

	public static void RecalculateSizeGradientQuad(GradientQuad quad, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(quad == null))
		{
			quad.Size = RecalculateByFactor(quad.Size, factorType);
			quad.Size = RoundVector2(quad.Size, roundPreference);
		}
	}

	public static void RecalculateSizeSprite(tk2dBaseSprite sprite, SizeFactor factorType, Fit fitType, RoundFloatEnum roundPreference)
	{
		if (!(sprite == null))
		{
			sprite.scale = RecalculateByFactor(sprite.scale, factorType);
			sprite.scale = RecalculateByFit(sprite.scale, fitType, true);
			sprite.scale = RoundVector3(sprite.scale, roundPreference);
		}
	}

	public static void RecalculateSizeSlicedSprite(tk2dSlicedSprite sprite, SizeFactor factorType, RoundFloatEnum roundPreference)
	{
		if (!(sprite == null))
		{
			sprite.dimensions = RecalculateByFactor(sprite.dimensions, factorType);
			sprite.dimensions = RoundVector2(sprite.dimensions, roundPreference);
		}
	}

	public static void RecalculateSizeSpriteFromTexture(tk2dSpriteFromTexture spriteFromTexture, SizeFactor factorType, Fit fitType, RoundFloatEnum roundPreference)
	{
		if (!(spriteFromTexture == null))
		{
			spriteFromTexture.ForceBuild();
			RecalculateSizeSprite(spriteFromTexture.GetComponent<tk2dBaseSprite>(), factorType, fitType, roundPreference);
		}
	}

	public static void RecalculateSizeText(tk2dTextMesh label, SizeFactor type, RoundFloatEnum roundPreference)
	{
		if (!(label == null))
		{
			label.scale = RecalculateByFactor(label.scale, type);
			label.scale = RoundVector3(label.scale, roundPreference);
		}
	}
}
