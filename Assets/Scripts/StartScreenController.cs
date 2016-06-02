using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class StartScreenController : NetworkBehaviour {

	private bool started = false;

	public GameObject playButtonObj;
	public GameObject tutorialButtonObj;
	public GameObject levelButtonObj;
	public GameObject multiplayerButtonObj;

	public GameObject startScreenWizard;
	

	// Use this for initialization
	void Start () {
		if (playButtonObj == null || tutorialButtonObj == null ||
			levelButtonObj == null || multiplayerButtonObj == null)
			Debug.LogError ("initialize all startscreen buttons in startscreenmanager");

		if (startScreenWizard == null)
			Debug.LogError ("reference the startscreen wizard in the startscreenmanager");
	}

	public void onClickMultiplayer(){
		playButtonObj.SetActive (false);
		tutorialButtonObj.SetActive (false);
		levelButtonObj.SetActive (false);
		multiplayerButtonObj.SetActive (false);

		GameObject downBorder = GameObject.Find ("Border (bottom)");
		downBorder.GetComponent<SpriteRenderer> ().enabled = true;

		NetworkManager.singleton.GetComponent<NetworkManagerHUD> ().showGUI = true;

		startScreenWizard.SetActive (false);

	}

	public void Update() {
		if (!isServer)
			return;
		GameObject[] wizards = GameObject.FindGameObjectsWithTag ("Wizards");
		if (wizards.Length >= 2 &&!started) {
			print ("now change levels");
			started = true;
			GameManager.GetInstance().setMultiPlayer(true);
			NetworkManager.singleton.GetComponent<NetworkManagerHUD> ().enabled = false;
			Invoke("StartLevel",2.0f);
		}
	}

	public void StartLevel(){
			GameManager.GetInstance ().loadLevel1 ();
	}
}
