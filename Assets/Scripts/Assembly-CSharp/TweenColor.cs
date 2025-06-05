using System;
using UnityEngine;

[AddComponentMenu("Inventain/Tween/Color")]
public class TweenColor : Tweener
{
	public Action OnColorChanged;

	public bool[] useChanelMask = new bool[4] { true, true, true, true };

	public Color beginColor = new Color(1f, 1f, 1f, 0f);

	public Color endColor = new Color(1f, 1f, 1f, 1f);

	[SerializeField]
	private GameObject target;

	private tk2dBaseSprite tk2dSprite;

	private tk2dTextMesh tk2dLabel;

	private ColorQuad quad;

	private DrawableMesh drawableMesh;

	private MeshRenderer meshRenderer;

	private ITweenColor objectColor;

	private bool needHandleMeshRenderer;

	public override string TargetName
	{
		get
		{
			return Target.name;
		}
	}

	public GameObject Target
	{
		get
		{
			if (target == null)
			{
				target = base.gameObject;
			}
			return target;
		}
		set
		{
			target = value;
			if (target == null)
			{
				target = Target;
			}
			InitReference(true);
		}
	}

	public bool IsReferenceSetUp
	{
		get
		{
			if (tk2dSprite != null)
			{
				return true;
			}
			if (tk2dLabel != null)
			{
				return true;
			}
			if (quad != null)
			{
				return true;
			}
			if (drawableMesh != null)
			{
				return true;
			}
			if (meshRenderer != null)
			{
				return true;
			}
			if (objectColor != null)
			{
				return true;
			}
			return false;
		}
	}

	public Color CurrentColor
	{
		get
		{
			InitReference(false);
			if (tk2dSprite != null)
			{
				return tk2dSprite.color;
			}
			if (tk2dLabel != null)
			{
				return tk2dLabel.color;
			}
			if (quad != null)
			{
				return quad.TintColor;
			}
			if (drawableMesh != null)
			{
				return drawableMesh.Color;
			}
			if (meshRenderer != null)
			{
				return meshRenderer.sharedMaterial.color;
			}
			if (objectColor != null)
			{
				return objectColor.Color;
			}
			return Color.black;
		}
		set
		{
			InitReference(false);
			if (tk2dSprite != null)
			{
				tk2dSprite.color = ApplyChanelMask(value, tk2dSprite.color);
				if (needHandleMeshRenderer)
				{
					tk2dSprite.GetComponent<Renderer>().enabled = tk2dSprite.color.a > 0.01f;
				}
			}
			else if (tk2dLabel != null)
			{
				Color color = tk2dLabel.color;
				Color color2 = ApplyChanelMask(value, color);
				if (color != color2)
				{
					tk2dLabel.color = color2;
				}
				if (needHandleMeshRenderer)
				{
					tk2dLabel.GetComponent<Renderer>().enabled = tk2dLabel.color.a > 0.01f;
				}
			}
			else if (quad != null)
			{
				quad.TintColor = ApplyChanelMask(value, quad.TintColor);
			}
			else if (drawableMesh != null)
			{
				drawableMesh.Color = ApplyChanelMask(value, drawableMesh.Color);
			}
			else if (meshRenderer != null)
			{
				meshRenderer.sharedMaterial.color = ApplyChanelMask(value, meshRenderer.sharedMaterial.color);
			}
			else if (objectColor != null)
			{
				objectColor.Color = ApplyChanelMask(value, objectColor.Color);
			}
			if (OnColorChanged != null)
			{
				OnColorChanged();
			}
		}
	}

	public bool IsOnlyAlphaTween
	{
		get
		{
			return !useChanelMask[0] && !useChanelMask[1] && !useChanelMask[2] && useChanelMask[3];
		}
	}

	private void Start()
	{
		needHandleMeshRenderer = GetComponent<tmBatchObject>() == null;
	}

	protected override void Awake()
	{
		base.Awake();
		InitReference(false);
	}

	private void InitReference(bool force)
	{
		if (!force && (!(tk2dSprite == null) || !(tk2dLabel == null) || !(quad == null) || !(drawableMesh == null)))
		{
			return;
		}
		tk2dSprite = Target.GetComponent<tk2dBaseSprite>();
		tk2dLabel = Target.GetComponent<tk2dTextMesh>();
		quad = Target.GetComponent<ColorQuad>();
		drawableMesh = Target.GetComponent<DrawableMesh>();
		objectColor = Target.GetComponent<ITweenColor>();
		if (tk2dSprite == null && tk2dLabel == null && quad == null && drawableMesh == null && objectColor == null)
		{
			meshRenderer = Target.GetComponent<MeshRenderer>();
			if (meshRenderer != null && (meshRenderer.sharedMaterial == null || !meshRenderer.sharedMaterial.HasProperty("_Color")))
			{
				CustomDebug.LogWarning("Wrong material!");
				meshRenderer = null;
			}
		}
	}

	private Color ApplyChanelMask(Color value, Color source)
	{
		if (!useChanelMask[0])
		{
			value.r = source.r;
		}
		if (!useChanelMask[1])
		{
			value.g = source.g;
		}
		if (!useChanelMask[2])
		{
			value.b = source.b;
		}
		if (!useChanelMask[3])
		{
			value.a = source.a;
		}
		return value;
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		CurrentColor = Color.Lerp(beginColor, endColor, factor);
	}

	public static TweenColor SetColor(GameObject go, Color color, float duration = 1f)
	{
		TweenColor tweenColor = Tweener.InitGO<TweenColor>(go, duration);
		tweenColor.InitReference(true);
		tweenColor.beginColor = tweenColor.CurrentColor;
		tweenColor.endColor = color;
		tweenColor.Play(true);
		return tweenColor;
	}
}
