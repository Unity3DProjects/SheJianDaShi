using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class SplashLoader : MonoBehaviour
{
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action m_OnSplashRemoved;

	[SerializeField]
	private tk2dSprite fade;

	[SerializeField]
	private float fadeTime;

	[SerializeField]
	private bool useGUIDimention;

	private tk2dSpriteFromTexture splashSprite;

	private Texture2D loadedTexture;

	public static event Action OnSplashRemoved
	{
		add
		{
			Action action = SplashLoader.m_OnSplashRemoved;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SplashLoader.m_OnSplashRemoved, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = SplashLoader.m_OnSplashRemoved;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref SplashLoader.m_OnSplashRemoved, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Awake()
	{
		base.transform.localPosition = new Vector3(ScreenDimentions.Width / 2, ScreenDimentions.Height / 2, base.transform.localPosition.z);
		splashSprite = GetComponent<tk2dSpriteFromTexture>();
	}

	private void OnEnable()
	{
		string splashPathForWidth = GetSplashPathForWidth(Screen.width);
		if (loadedTexture == null)
		{
			TextureLoaded(TextureHelper.Instance.LoadImageToTexture(splashPathForWidth, false, TextureFormat.RGB24));
		}
	}

	private void OnDisable()
	{
		if (loadedTexture != null)
		{
			TextureHelper.Instance.UnloadTexture(loadedTexture);
			loadedTexture = null;
		}
	}

	private IEnumerator SplashCoroutine()
	{
		yield return new WaitForSeconds(2f);
		TweenColor tc = TweenColor.SetColor(base.gameObject, new Color(1f, 1f, 1f, 0f), fadeTime);
		tc.SetOnFinishedDelegate(delegate
		{
			if (SplashLoader.OnSplashRemoved != null)
			{
				SplashLoader.OnSplashRemoved();
			}
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		});
	}

	private string GetSplashPathForWidth(int width)
	{
		string empty = string.Empty;
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidPluginNames.BOWMASTERS_ACTIVITY);
		empty = androidJavaClass.CallStatic<string>(AndroidPluginNames.BOWMASTERS_ACTIVITY_GET_LOADER_NAME, new object[1] { width });
		string str = "Loader";
		return str.AppendPathComponent(empty).AppendPathExtention("png");
	}

	private void TextureLoaded(Texture2D tex)
	{
		loadedTexture = tex;
		splashSprite.texture = tex;
		splashSprite.spriteCollectionSize.type = tk2dSpriteCollectionSize.Type.PixelsPerMeter;
		splashSprite.spriteCollectionSize.pixelsPerMeter = 1f;
		splashSprite.anchor = tk2dBaseSprite.Anchor.MiddleCenter;
		splashSprite.ForceBuild();
		Vector2 vector = new Vector2((float)ScreenDimentions.Width / (float)tex.width, (float)ScreenDimentions.Height / (float)tex.height);
		float num = Mathf.Max(vector.x, vector.y);
		splashSprite.transform.localScale = new Vector3(num, num, 1f);
		StartCoroutine(SplashCoroutine());
	}
}
