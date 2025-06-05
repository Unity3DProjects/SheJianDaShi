using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GifManager : SingletonMonoBehaviour<GifManager>
{
	[SerializeField]
	private Vector2 gifDimensions;

	[SerializeField]
	private int framesCount;

	[SerializeField]
	private Texture2D watermarkTexture;

	[SerializeField]
	private Texture2D watermarkTextureRetina;

	[SerializeField]
	private Vector2 watermarkOffset;

	private List<Texture2D> textureList;

	private RenderTexture cachedRenderTexture;

	private Vector2 cachedDimensions = Vector2.zero;

	private Level cachedLevel;

	private Camera cachedMainCamera;

	private bool isCapturing;

	private int cachedFramesCount;

	private Level CachedLevel
	{
		get
		{
			if (cachedLevel == null)
			{
				cachedLevel = SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel;
			}
			return cachedLevel;
		}
	}

	public Vector2 CachedDimensions
	{
		get
		{
			if (cachedDimensions == Vector2.zero)
			{
				cachedDimensions = gifDimensions;
				cachedDimensions *= ((!tk2dSystem.IsRetina) ? 1f : 2f);
			}
			return cachedDimensions;
		}
	}

	private RenderTexture CachedRenderTexture
	{
		get
		{
			if (cachedRenderTexture == null)
			{
				cachedRenderTexture = new RenderTexture(ScreenDimentions.Width, ScreenDimentions.Height, 24);
			}
			return cachedRenderTexture;
		}
	}

	private Camera CachedMainCamera
	{
		get
		{
			if (cachedMainCamera == null)
			{
				cachedMainCamera = SingletonMonoBehaviour<GameManager>.Instance.CameraController.GetComponent<Camera>();
			}
			return cachedMainCamera;
		}
	}

	public List<Texture2D> CachedTextureList
	{
		get
		{
			return textureList;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		textureList = new List<Texture2D>();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if (!isCapturing)
		{
			return;
		}
		Vector3 position = CachedMainCamera.transform.position;
		Vector3 vector = CachedMainCamera.WorldToScreenPoint(position);
		if (cachedFramesCount > 0)
		{
			cachedFramesCount--;
			CachedMainCamera.targetTexture = CachedRenderTexture;
			RenderTexture.active = CachedRenderTexture;
			CachedMainCamera.Render();
			float x = Mathf.Clamp(vector.x - CachedDimensions.x / 2f, 0f, float.MaxValue);
			float y = Mathf.Clamp(vector.y - CachedDimensions.y / 2f, 0f, float.MaxValue);
			Rect source = new Rect(x, y, CachedDimensions.x, CachedDimensions.y);
			Texture2D texture2D = new Texture2D((int)CachedDimensions.x, (int)CachedDimensions.y, TextureFormat.RGB24, false);
			texture2D.ReadPixels(source, 0, 0);
			if ((cachedFramesCount & 1) == 0)
			{
				texture2D = AddWatermark(texture2D, watermarkTexture, watermarkOffset);
			}
			texture2D.Apply();
			textureList.Add(texture2D);
			RenderTexture.active = null;
			CachedMainCamera.targetTexture = null;
		}
		else
		{
			isCapturing = false;
		}
	}

	public void StartCapturing()
	{
		isCapturing = true;
		cachedFramesCount = framesCount;
	}

	public void AddWatermark()
	{
		for (int i = 0; i < textureList.Count; i++)
		{
			if ((i & 1) == 0)
			{
				Texture2D texture2D = (tk2dSystem.IsRetina ? AddWatermark(textureList[i], watermarkTextureRetina, watermarkOffset * 2f) : AddWatermark(textureList[i], watermarkTexture, watermarkOffset));
				texture2D.Apply();
				textureList[i] = texture2D;
			}
		}
	}

	public void Save()
	{
		GifEncoder gifEncoder = new GifEncoder(0, 5);
		gifEncoder.SetDelay(Mathf.RoundToInt(60f));
		List<GifFrame> list = new List<GifFrame>();
		RenderTexture.active = CachedRenderTexture;
		for (int i = 0; i < textureList.Count; i++)
		{
			if ((i & 1) == 0)
			{
				GifFrame gifFrame = new GifFrame();
				gifFrame.Width = textureList[i].width;
				gifFrame.Height = textureList[i].height;
				gifFrame.Data = textureList[i].GetPixels32();
				GifFrame item = gifFrame;
				list.Add(item);
			}
		}
		RenderTexture.active = null;
		string text = null;
		text = Application.dataPath + "/MyGif.gif";
		Worker worker = new Worker(System.Threading.ThreadPriority.Highest);
		worker.m_Encoder = gifEncoder;
		worker.m_Frames = list;
		worker.m_FilePath = text;
		worker.m_OnFileSaved = OnFileSaved;
		Worker worker2 = worker;
		worker2.Start();
	}

	private Texture2D AddWatermark(Texture2D background, Texture2D watermark, Vector2 offset)
	{
		int num = background.width - (watermark.width + (int)offset.x);
		int num2 = (int)offset.y;
		for (int i = num; i < background.width; i++)
		{
			for (int j = num2; j < background.height; j++)
			{
				Color pixel = background.GetPixel(i, j);
				Color pixel2 = watermark.GetPixel(i - num, j - num2);
				Color color = Color.Lerp(pixel, pixel2, pixel2.a / 1f);
				background.SetPixel(i, j, color);
			}
		}
		return background;
	}

	private void OnFileSaved(int id, string filepath)
	{
		Debug.Log(filepath);
	}

	private void ResultScreen_OnResultScreenHide()
	{
		textureList.Clear();
	}
}
