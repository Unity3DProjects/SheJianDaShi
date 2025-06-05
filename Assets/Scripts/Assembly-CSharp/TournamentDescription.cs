using System;
using System.Collections.Generic;

[Serializable]
public struct TournamentDescription
{
	public TournamentSettings settings;

	public List<object> enemiesID;

	public int currentEnemyNumber;

	public object playerCharacterID;

	public bool canBeContinued;
}
