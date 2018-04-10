using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using System.Collections.Generic;
public class NetworkLobbyHook : LobbyHook {

	public List<int> AvailaleIDs;


	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer> ();
		SetupLocalPlayer localPlayer = gamePlayer.GetComponent<SetupLocalPlayer> ();

		localPlayer.pName = lobby.playerName;
		localPlayer.playerColor = lobby.playerColor;
		localPlayer.starvationCount = 0;
		localPlayer.isAlive = true;
		localPlayer.hasAttackedThisRound = false;
	//	localPlayer.setName (lobby.playerName);

		int playerID = AvailaleIDs [Random.Range (0, AvailaleIDs.Count)];

		for(int i = 0; i < AvailaleIDs.Count; i ++)
		{
			if(AvailaleIDs[i] == playerID)
			{
				AvailaleIDs.RemoveAt (i);
			}
		}

		localPlayer.playerID = playerID;
	//	localPlayer.setID (playerID);

	}
}
