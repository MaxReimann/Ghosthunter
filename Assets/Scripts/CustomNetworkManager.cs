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

	public override void OnServerReady (NetworkConnection conn)
	{
		base.OnServerReady (conn);

		if (IsClientConnected() && ++playerCount == 2 )
			GameManager.GetInstance ().loadLevel1 ();
	}

	public override void OnServerConnect(NetworkConnection conn)
	{



//		if(conn.connectionId != 0 && numPlayers != 0)
//		{
//			Debug.Log("A remote client has connected!");
//			Debug.Log(conn.connectionId.ToString());
//		}else if(conn.connectionId == 0 && numPlayers == 0 && localClientFirstConnect)
//		{
//			Debug.Log("A local client has connected!");
//			Debug.Log(conn.connectionId.ToString());
//			localClientFirstConnect = false;
//
//			return;
//		}else
//		{
//			Debug.Log("Caught the local client's second OnServerConnect call.");
//
//			return;
//		}
//
//
//		GameObject startButton = GameObject.FindGameObjectWithTag ("StartGameAsServer") as GameObject;
//		if (startButton == null)
//			print ("Error: button startgameasserver not in this scene");
//		startButton.GetComponent<Button> ().interactable = true;
//
//		GameObject networkDiscoveryObject = GameObject.FindGameObjectWithTag ("Matchmaking") as GameObject;
//		networkDiscoveryObject.GetComponent<NetworkDiscovery> ().StopBroadcast ();
	}

}
