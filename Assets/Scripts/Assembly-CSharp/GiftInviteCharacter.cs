using System;

[Serializable]
public struct GiftInviteCharacter
{
	public CharacterNameType inviteCharacter;

	public string inviteCharacterSpriteName;

	public int invitesNeededCount;

	public float inviteWeight;

	public bool isForcedShowed;

	public float forcedWeight;
}
