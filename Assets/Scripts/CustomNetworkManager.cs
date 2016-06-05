using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;


public class CustomNetworkManager : NetworkManager {

	int hostStartCount = 0;

	// Use this for initialization
	void Start () {
	}

//
//	public override NetworkClient StartHost(ConnectionConfig config, int maxConnections)
//	{
//		print ("start host with config");
//		//this fixes a extremly strange bug, which wants to start a new host on client scene change
//		// if the host has already been started, just return the actual client
//		if (hostStartCount++ == 0)
//			return base.StartHost (config, maxConnections);
//
//		return this.client;
//	}
//
//	public override NetworkClient StartHost(){
//		print ("start host");
//		if (hostStartCount++ == 0)
//			base.StartHost();
//
//		return this.client;
//	}
//		
//	public override NetworkClient StartHost(MatchInfo info) {
//		print ("start host with match");
//		if (hostStartCount++ == 0)
//			base.StartHost();
//		
//		return this.client;
//
//	}

	public override void OnStartHost ()
	{
		base.OnStartHost ();

		StartScreenController startscreenControl = FindObjectOfType (typeof(StartScreenController))as StartScreenController;
		if (startscreenControl != null && startscreenControl.inMultiplayerMode) {
			startscreenControl.showClientJoinText();
		}

	}

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
