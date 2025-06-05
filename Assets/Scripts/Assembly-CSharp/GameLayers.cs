using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 1)]
public struct GameLayers
{
	public const string IGNORE_COLLISIONS = "IgnoreCollisions";

	public const string IGNORE_CHARACTER_COLLISIONS = "IgnoreCharacterCollisions";

	public const string CHARACTER_PARTS = "CharacterParts";

	public const string UI = "UI";

	public const string ONLY_GROUND_COLLISION_LAYER_NAME = "CharacterGuts";

	public const string FIRST_SINGLE_COLLISION = "FirstSingleCollision";

	public const string SECOND_SINGLE_COLLISION = "SecondSingleCollision";

	public const string MISTERY_BOX = "MisteryBox";
}
