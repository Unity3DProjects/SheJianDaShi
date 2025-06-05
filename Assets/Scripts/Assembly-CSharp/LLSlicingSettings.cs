using UnityEngine;

[CreateAssetMenu(fileName = "LLSlicingSettings")]
public class LLSlicingSettings : ScriptableSingleton<LLSlicingSettings>
{
	public const string ASSET_FOLDER_STREAMING_TEXTURES = "Assets/StreamingAssets/tk2dTextures/";

	public const string XCODE_FOLDER_STREAMING_TEXTURES = "Data/Raw/tk2dTextures/";

	[Header("Auto-Generate fields")]
	public string[] universalTextures;

	public string[] nonretinaOnlyTextures;

	public string[] retinaOnlyTextures;

	public string[] platformTextures;
}
