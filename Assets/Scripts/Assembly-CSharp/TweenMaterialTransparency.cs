using UnityEngine;

[AddComponentMenu("Inventain/Tween/MaterialTransparency")]
public class TweenMaterialTransparency : Tweener
{
	[SerializeField]
	private float endTransparency = 1f;

	[SerializeField]
	private float beginTransparency;

	[SerializeField]
	private string shaderTransparencyId = "_Transparency";

	private Material targetMaterial;

	private int transparencyId;

	public float EndTransparency
	{
		get
		{
			return endTransparency;
		}
		set
		{
			endTransparency = value;
		}
	}

	public float BeginTransparency
	{
		get
		{
			return beginTransparency;
		}
		set
		{
			beginTransparency = value;
		}
	}

	public Material TargetMaterial
	{
		get
		{
			if (targetMaterial == null)
			{
				targetMaterial = GetComponent<Renderer>().material;
			}
			return targetMaterial;
		}
	}

	public float CurrentTransparency
	{
		get
		{
			return TargetMaterial.GetFloat(transparencyId);
		}
		set
		{
			TargetMaterial.SetFloat(transparencyId, value);
		}
	}

	public string ShaderTransparencyId
	{
		get
		{
			return shaderTransparencyId;
		}
		set
		{
			shaderTransparencyId = value;
		}
	}

	protected override void Awake()
	{
		transparencyId = Shader.PropertyToID(shaderTransparencyId);
		base.Awake();
	}

	public static TweenMaterialTransparency SetTransparency(GameObject go, float transparency, float duration = 1f)
	{
		TweenMaterialTransparency tweenMaterialTransparency = Tweener.InitGO<TweenMaterialTransparency>(go);
		tweenMaterialTransparency.BeginTransparency = tweenMaterialTransparency.CurrentTransparency;
		tweenMaterialTransparency.EndTransparency = transparency;
		tweenMaterialTransparency.duration = duration;
		tweenMaterialTransparency.Play(true);
		return tweenMaterialTransparency;
	}

	protected override void TweenUpdateRuntime(float factor, bool isFinished)
	{
		CurrentTransparency = Mathf.Lerp(BeginTransparency, EndTransparency, factor);
	}

	protected override void TweenUpdateEditor(float factor)
	{
		CurrentTransparency = Mathf.Lerp(BeginTransparency, EndTransparency, factor);
	}
}
