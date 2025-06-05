using System;

[Serializable]
public struct LocalPlayerSettings
{
	public CharacterNameType characterID;

	public bool isMainPlayer;

	public string countryCode;

	public string nickname;

	public bool isMembershipPlayer;
}
