using System;
using AOT;
using UnityEngine;

public static class LLTextureManager
{
	private sealed class LLTextureLoader : ITextureLoader
	{
		private static Action<IntPtr, IntPtr> callbackTextureLoad;

		private static Action<IntPtr, IntPtr> callbackTextureFail;

		public void Init(Action<IntPtr, IntPtr> callbackLoad, Action<IntPtr, IntPtr> callbackFail)
		{
			callbackTextureLoad = callbackLoad;
			callbackTextureFail = callbackFail;
		}

		public TextureManager.TextureInfo GetInfo(string imagePath)
		{
			return default(TextureManager.TextureInfo);
		}

		public IntPtr LoadTexture(string imagePath, bool doMipMaps)
		{
			return IntPtr.Zero;
		}

		public IntPtr LoadTextureAsync(string imagePath, bool doMipMaps)
		{
			return IntPtr.Zero;
		}

		public void ReleaseTexture(IntPtr texturePointer)
		{
			if (!(texturePointer != IntPtr.Zero))
			{
			}
		}

		public void Deinit()
		{
			callbackTextureLoad = null;
			callbackTextureFail = null;
		}

		[MonoPInvokeCallback(typeof(Action<IntPtr, IntPtr>))]
		private static void OnTextureLoad(IntPtr textureID, IntPtr newTexturePointer)
		{
			if (callbackTextureLoad != null)
			{
				callbackTextureLoad(textureID, newTexturePointer);
			}
		}

		[MonoPInvokeCallback(typeof(Action<IntPtr, IntPtr>))]
		private static void OnTextureFail(IntPtr textureID, IntPtr newTexturePointer)
		{
			if (callbackTextureFail != null)
			{
				callbackTextureFail(textureID, newTexturePointer);
			}
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void RuntimeInitialize()
	{
	}
}
