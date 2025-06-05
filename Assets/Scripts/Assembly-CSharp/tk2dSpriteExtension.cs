using UnityEngine;

public static class tk2dSpriteExtension
{
	public static Rect GetRect(this tk2dBaseSprite sprite)
	{
		Texture2D texture2D = sprite.CurrentSprite.materialInst.mainTexture as Texture2D;
		return new Rect((float)texture2D.width * sprite.CurrentSprite.uvs[0].x, (float)texture2D.height * sprite.CurrentSprite.uvs[0].y, sprite.CurrentSprite.boundsData[1].x, sprite.CurrentSprite.boundsData[1].y);
	}
}
