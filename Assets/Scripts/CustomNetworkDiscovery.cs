using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CustomNetworkDiscovery : NetworkDiscovery
{
	private bool started = false;
	private NetworkManager manager;

	public void Awake() {
		Initialize ();
		manager = NetworkManager.singleton;
		manager.GetComponent<NetworkManagerHUD> ().showGUI = true;;

	}
	

//	public void StartServer() {
//		bool success = this.StartAsServer ();
//		if (!success) {
//			print ("Error: Not able to broadcast");
//			return;
//		}
//
//		if (!manager.IsClientConnected () && !NetworkServer.active && manager.matchMaker == null) {
//			NetworkManager.singleton.StartHost ();
//		}
//
//	}
//
//	public void StartClient() {
//		bool success = this.StartAsClient ();
//		if (!success) {
//			print ("Error: Not able to broadcast");
//			return;
//		}
//	}

	public override void OnReceivedBroadcast (string fromAddress, string data)
	{
//		print (fromAddress);
//		if (isServer) {
//			print ("caught host client");
//			return;
//		}
//
//		if (started)
//			return;
//
//		if (fromAddress == ":::ffff:192.168.56.1:7777")
//			return;
//
//		NetworkManager.singleton.networkAddress = fromAddress;
//		//NetworkManager.singleton.StartClient();
//		started = true;
//		this.StopBroadcast ();
	}

	
}