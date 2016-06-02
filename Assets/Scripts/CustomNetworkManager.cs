using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class CustomNetworkManager : NetworkManager {

	int count = 0;
	int playerCount = 0;
	int hostStartCount = 0;
	// Use this for initialization
	void Start () {
	}

	public override NetworkClient StartHost(ConnectionConfig config, int maxConnections)
	{
		//this fixes a extremly strange bug, which wants to start a new host on client scene change
		// if the host has already been started, just return the actual client
		if (hostStartCount++ == 0)
			return base.StartHost (config, maxConnections);

		return this.client;
	}

	public override NetworkClient StartHost(){
		if (hostStartCount++ == 0)
			base.StartHost();

		return this.client;
	}
		
	//public override NetworkClient StartHost(MatchInfo info) {
	//}

//	public override void OnClientSceneChanged (NetworkConnection conn)
//	{
//		if (count++ == 0)
//			base.OnClientSceneChanged (conn);
//		else 
//			return;
//		
//		bool addPlayer = (ClientScene.localPlayers.Count == 0);
//		bool foundPlayer = false;
//		foreach (var playerController in ClientScene.localPlayers)
//		{
//			if (playerController.gameObject != null)
//			{
//				foundPlayer = true;
//				break;
//			}
//		}
//		if (!foundPlayer)
//		{
//			// there are players, but their game objects have all been deleted
//			addPlayer = true;
//		}
//		if (addPlayer)
//		{
//			ClientScene.AddPlayer(0);
//		}
//		
//	}
//	

}
