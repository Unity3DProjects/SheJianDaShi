using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using UnityEngine;

[AddComponentMenu("Miscellaneous/Moments Recorder")]
[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public sealed class Recorder : MonoBehaviour
{
	[SerializeField]
	[Min(8f)]
	private int m_Width = 320;

	[SerializeField]
	[Min(8f)]
	private int m_Height = 200;

	[SerializeField]
	private bool m_AutoAspect = true;

	[SerializeField]
	[Range(1f, 30f)]
	private int m_FramePerSecond = 15;

	[SerializeField]
	[Min(-1f)]
	private int m_Repeat;

	[SerializeField]
	[Range(1f, 100f)]
	private int m_Quality = 15;

	[SerializeField]
	[Min(0.1f)]
	private float m_BufferSize = 3f;

	public System.Threading.ThreadPriority WorkerPriority = System.Threading.ThreadPriority.BelowNormal;

	public Action OnPreProcessingDone;

	public Action<int, float> OnFileSaveProgress;

	public Action<int, string> OnFileSaved;

	private int m_MaxFrameCount;

	private float m_Time;

	private float m_TimePerFrame;

	private Queue<RenderTexture> m_Frames;

	private RenderTexture m_RecycledRenderTexture;

	private ReflectionUtils<Recorder> m_ReflectionUtils;

	public RecorderState State { get; private set; }

	public string SaveFolder { get; set; }

	public float EstimatedMemoryUse
	{
		get
		{
			float num = (float)m_FramePerSecond * m_BufferSize;
			num *= (float)(m_Width * m_Height * 4);
			return num / 1048576f;
		}
	}

	public void Setup(bool autoAspect, int width, int height, int fps, float bufferSize, int repeat, int quality)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(Recorder), "x");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(Recorder), "x");
		ParameterExpression parameterExpression3 = Expression.Parameter(typeof(Recorder), "x");
		ParameterExpression parameterExpression4 = Expression.Parameter(typeof(Recorder), "x");
		ParameterExpression parameterExpression5 = Expression.Parameter(typeof(Recorder), "x");
		ParameterExpression parameterExpression6 = Expression.Parameter(typeof(Recorder), "x");
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to setup the component during the pre-processing step.");
			return;
		}
		FlushMemory();
		m_AutoAspect = autoAspect;
		m_ReflectionUtils.ConstrainMin(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression }), width);
		if (autoAspect)
		{
			m_ReflectionUtils.ConstrainMin(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression2, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression2 }), height);
		}
		m_ReflectionUtils.ConstrainRange(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression3, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression3 }), fps);
		m_ReflectionUtils.ConstrainMin(Expression.Lambda<Func<Recorder, float>>(Expression.Field(parameterExpression4, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression4 }), bufferSize);
		m_ReflectionUtils.ConstrainMin(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression5, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression5 }), repeat);
		m_ReflectionUtils.ConstrainRange(Expression.Lambda<Func<Recorder, int>>(Expression.Field(parameterExpression6, FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/)), new ParameterExpression[1] { parameterExpression6 }), quality);
		Init();
	}

	public void Pause()
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to pause recording during the pre-processing step. The recorder is automatically paused when pre-processing.");
		}
		else
		{
			State = RecorderState.Paused;
		}
	}

	public void Record()
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to resume recording during the pre-processing step.");
		}
		else
		{
			State = RecorderState.Recording;
		}
	}

	public void FlushMemory()
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to flush memory during the pre-processing step.");
			return;
		}
		Init();
		if (m_RecycledRenderTexture != null)
		{
			Flush(m_RecycledRenderTexture);
		}
		if (m_Frames == null)
		{
			return;
		}
		foreach (RenderTexture frame in m_Frames)
		{
			Flush(frame);
		}
		m_Frames.Clear();
	}

	public void Save()
	{
		Save(GenerateFileName());
	}

	public void Save(string filename)
	{
		if (State == RecorderState.PreProcessing)
		{
			Debug.LogWarning("Attempting to save during the pre-processing step.");
			return;
		}
		if (m_Frames.Count == 0)
		{
			Debug.LogWarning("Nothing to save. Maybe you forgot to start the recorder ?");
			return;
		}
		State = RecorderState.PreProcessing;
		if (string.IsNullOrEmpty(filename))
		{
			filename = GenerateFileName();
		}
		StartCoroutine(PreProcess(filename));
	}

	private void Awake()
	{
		m_ReflectionUtils = new ReflectionUtils<Recorder>(this);
		m_Frames = new Queue<RenderTexture>();
		Init();
	}

	private void OnDestroy()
	{
		FlushMemory();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (State != RecorderState.Recording)
		{
			Graphics.Blit(source, destination);
			return;
		}
		m_Time += Time.unscaledDeltaTime;
		if (m_Time >= m_TimePerFrame)
		{
			if (m_Frames.Count >= m_MaxFrameCount)
			{
				m_RecycledRenderTexture = m_Frames.Dequeue();
			}
			m_Time -= m_TimePerFrame;
			RenderTexture renderTexture = m_RecycledRenderTexture;
			m_RecycledRenderTexture = null;
			if (renderTexture == null)
			{
				renderTexture = new RenderTexture(m_Width, m_Height, 0, RenderTextureFormat.ARGB32);
				renderTexture.wrapMode = TextureWrapMode.Clamp;
				renderTexture.filterMode = FilterMode.Bilinear;
				renderTexture.anisoLevel = 0;
			}
			Graphics.Blit(source, renderTexture);
			m_Frames.Enqueue(renderTexture);
		}
		Graphics.Blit(source, destination);
	}

	private void Init()
	{
		State = RecorderState.Paused;
		ComputeHeight();
		m_MaxFrameCount = Mathf.RoundToInt(m_BufferSize * (float)m_FramePerSecond);
		m_TimePerFrame = 1f / (float)m_FramePerSecond;
		m_Time = 0f;
		if (string.IsNullOrEmpty(SaveFolder))
		{
			SaveFolder = Application.persistentDataPath;
		}
	}

	public void ComputeHeight()
	{
		if (m_AutoAspect)
		{
			m_Height = Mathf.RoundToInt((float)m_Width / GetComponent<Camera>().aspect);
		}
	}

	private void Flush(UnityEngine.Object obj)
	{
		UnityEngine.Object.Destroy(obj);
	}

	private string GenerateFileName()
	{
		string text = DateTime.Now.ToString("yyyyMMddHHmmssffff");
		return "GifCapture-" + text;
	}

	private IEnumerator PreProcess(string filename)
	{
		string filepath = SaveFolder + "/" + filename + ".gif";
		List<GifFrame> frames = new List<GifFrame>(m_Frames.Count);
		Texture2D temp = new Texture2D(m_Width, m_Height, TextureFormat.RGB24, false)
		{
			hideFlags = HideFlags.HideAndDontSave,
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Bilinear,
			anisoLevel = 0
		};
		while (m_Frames.Count > 0)
		{
			GifFrame frame = ToGifFrame(m_Frames.Dequeue(), temp);
			frames.Add(frame);
			yield return null;
		}
		Flush(temp);
		State = RecorderState.Paused;
		if (OnPreProcessingDone != null)
		{
			OnPreProcessingDone();
		}
		GifEncoder encoder = new GifEncoder(m_Repeat, m_Quality);
		encoder.SetDelay(Mathf.RoundToInt(m_TimePerFrame * 1000f));
		Worker worker = new Worker(WorkerPriority)
		{
			m_Encoder = encoder,
			m_Frames = frames,
			m_FilePath = filepath
		};
		worker.Start();
	}

	private GifFrame ToGifFrame(RenderTexture source, Texture2D target)
	{
		RenderTexture.active = source;
		target.ReadPixels(new Rect(0f, 0f, source.width, source.height), 0, 0);
		target.Apply();
		RenderTexture.active = null;
		GifFrame gifFrame = new GifFrame();
		gifFrame.Width = target.width;
		gifFrame.Height = target.height;
		gifFrame.Data = target.GetPixels32();
		return gifFrame;
	}
}
