using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChangeScreenController : NetworkBehaviour {

	private bool started = false;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Update() {
		GameObject[] wizards = GameObject.FindGameObjectsWithTag ("Wizards");
		if (wizards.Length >= 2 &&!started) {
			print ("now change levels");
			started = true;
			NetworkManager.singleton.GetComponent<NetworkManagerHUD> ().enabled = false;
			Invoke("CmdchangeLevel", 2.0f);
		}
	}
	
	[Command]
	public void CmdchangeLevel(){
			GameManager.GetInstance ().loadLevel1 ();
	}
}
