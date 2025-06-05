using System;
using UnityEngine;

public class WaveExplosion : MonoBehaviour
{
	[SerializeField]
	private float timeWave = 0.5f;

	[SerializeField]
	[Range(0f, 2f)]
	private float radiusWave = 0.2f;

	[SerializeField]
	[Range(0f, 2f)]
	private float widthWave = 0.1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float convexWave = 0.1f;

	private tk2dCamera cameraWave;

	[SerializeField]
	private bool isIgnoreMonoUpdate;

	[SerializeField]
	private bool isAutoDestroy;

	[SerializeField]
	private Material materialExplosionWave;

	private float currentTime;

	private Vector2 currentCenter;

	private Action<WaveExplosion> currentCallback;

	private bool isActive;

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

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}

	public float RadiusWave
	{
		get
		{
			return radiusWave;
		}
		set
		{
			radiusWave = Mathf.Clamp(value, 0f, 2f);
		}
	}

	public float TimeWave
	{
		get
		{
			return timeWave;
		}
		set
		{
			timeWave = Mathf.Max(value, 0f);
		}
	}

	public float WidthWave
	{
		get
		{
			return widthWave;
		}
		set
		{
			widthWave = Mathf.Clamp(value, 0f, 2f);
		}
	}

	public float ConvexWave
	{
		get
		{
			return convexWave;
		}
		set
		{
			convexWave = Mathf.Clamp(value, 0f, 1f);
		}
	}

	public tk2dCamera CameraWave
	{
		get
		{
			return cameraWave;
		}
		set
		{
			cameraWave = value;
		}
	}

	public bool IsIgnoreMonoUpdate
	{
		get
		{
			return isIgnoreMonoUpdate;
		}
		set
		{
			isIgnoreMonoUpdate = value;
		}
	}

	public bool IsAutoDestroy
	{
		get
		{
			return isAutoDestroy;
		}
		set
		{
			isAutoDestroy = value;
		}
	}

	private void Awake()
	{
		if (materialExplosionWave == null)
		{
			materialExplosionWave = new Material(Shader.Find("Custom/WaveExplosion"));
		}
	}

	private void Update()
	{
		if (isActive && !isIgnoreMonoUpdate)
		{
			if (currentTime < timeWave)
			{
				currentTime += Time.deltaTime;
			}
			else
			{
				StopWaveExplosion(currentCallback);
			}
		}
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (isActive && cameraWave != null)
		{
			Vector2 vector = cameraWave.ScreenCamera.WorldToScreenPoint(currentCenter);
			materialExplosionWave.SetFloat("_CenterX", vector.x / (float)cameraWave.ScreenCamera.pixelWidth);
			materialExplosionWave.SetFloat("_CenterY", vector.y / (float)cameraWave.ScreenCamera.pixelHeight);
			float num = Mathf.Lerp(0f, radiusWave, currentTime / timeWave);
			num *= cameraWave.ZoomFactor;
			materialExplosionWave.SetFloat("_Radius", num);
			Graphics.Blit(src, dest, materialExplosionWave);
		}
		else
		{
			Graphics.Blit(src, dest);
		}
	}

	public static WaveExplosion CreateWaveExplosion(tk2dCamera camera, WaveExplosion prefab = null)
	{
		WaveExplosion waveExplosion = null;
		if (camera != null)
		{
			waveExplosion = camera.gameObject.AddComponent<WaveExplosion>();
			waveExplosion.CameraWave = camera;
			if (prefab != null)
			{
				waveExplosion.CopyWaveExplosion(prefab);
			}
		}
		return waveExplosion;
	}

	public void CopyWaveExplosion(WaveExplosion waveSettings)
	{
		radiusWave = waveSettings.RadiusWave;
		timeWave = waveSettings.TimeWave;
		widthWave = waveSettings.WidthWave;
		convexWave = waveSettings.ConvexWave;
		isIgnoreMonoUpdate = waveSettings.IsIgnoreMonoUpdate;
		IsAutoDestroy = waveSettings.IsAutoDestroy;
	}

	public void StartWaveExplosion(Vector2 center, Action<WaveExplosion> callback = null)
	{
		if (!isActive && cameraWave != null)
		{
			isActive = true;
			currentTime = 0f;
			currentCenter = center;
			currentCallback = callback;
			Vector2 vector = cameraWave.ScreenCamera.WorldToScreenPoint(currentCenter);
			materialExplosionWave.SetFloat("_CenterX", vector.x / (float)cameraWave.ScreenCamera.pixelWidth);
			materialExplosionWave.SetFloat("_CenterY", vector.y / (float)cameraWave.ScreenCamera.pixelHeight);
			materialExplosionWave.SetFloat("_Wavewidth", widthWave);
			materialExplosionWave.SetFloat("_Waveconvex", convexWave);
			materialExplosionWave.SetFloat("_Radius", 0f);
		}
	}

	public void StopWaveExplosion(Action<WaveExplosion> callback = null)
	{
		isActive = false;
		currentTime = 0f;
		if (callback != null)
		{
			callback(this);
		}
		if (isAutoDestroy)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void UpdateWaveExplosion(float deltaTime)
	{
		if (isActive && isIgnoreMonoUpdate)
		{
			if (currentTime < timeWave)
			{
				currentTime += Time.deltaTime;
			}
			else
			{
				StopWaveExplosion(currentCallback);
			}
		}
	}
}
