using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class CustomNetworkManager : NetworkManager {

	int playerCount = 0;
	// Use this for initialization
	void Start () {
	}

	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		//base.OnClientSceneChanged (conn);
		
		bool addPlayer = (ClientScene.localPlayers.Count == 0);
		bool foundPlayer = false;
		foreach (var playerController in ClientScene.localPlayers)
		{
			if (playerController.gameObject != null)
			{
				foundPlayer = true;
				break;
			}
		}
		if (!foundPlayer)
		{
			// there are players, but their game objects have all been deleted
			addPlayer = true;
		}
		if (addPlayer)
		{
			ClientScene.AddPlayer(0);
		}
		
	}
	

}
