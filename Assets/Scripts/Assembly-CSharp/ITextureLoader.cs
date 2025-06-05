using System;

public interface ITextureLoader
{
	void Init(Action<IntPtr, IntPtr> callbackLoad, Action<IntPtr, IntPtr> callbackFail);

	TextureManager.TextureInfo GetInfo(string imagePath);

	IntPtr LoadTexture(string imagePath, bool doMipMaps);

	IntPtr LoadTextureAsync(string imagePath, bool doMipMaps);

	void ReleaseTexture(IntPtr texturePointer);

	void Deinit();
}
