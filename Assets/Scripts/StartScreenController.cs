using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class StartScreenController : MonoBehaviour {

	private bool started = false;

	public GameObject playButtonObj;
	public GameObject tutorialButtonObj;
	public GameObject levelButtonObj;
	public GameObject multiplayerButtonObj;
	public GameObject playerNameObj;
	public GameObject clientWaitJoinText;

	public GameObject startHostObj;
	public GameObject startClientObj;
	public GameObject backButtonObj;
	public GameObject ipAdressObj;

	public GameObject startMultiButton;

	private Vector3 startHostPosition;
	private Vector3 startClientPosition;
	private Vector3 backButtonPosition;
	private Vector3 ipAdressPosition;
	

	public GameObject startScreenWizard;

	public bool inMultiplayerMode = false;
	

	// Use this for initialization
	void Start () {
		if (playButtonObj == null || tutorialButtonObj == null ||
			levelButtonObj == null || multiplayerButtonObj == null || playerNameObj == null)
			Debug.LogError ("initialize all startscreen buttons in startscreenmanager");

		if (startScreenWizard == null)
			Debug.LogError ("reference the startscreen wizard in the startscreenmanager");
	}

	public void onClickMultiplayer(){
		playButtonObj.SetActive (false);
		tutorialButtonObj.SetActive (false);
		levelButtonObj.SetActive (false);
		multiplayerButtonObj.SetActive (false);
		playerNameObj.SetActive (false);

		GameObject downBorder = GameObject.Find ("Border (bottom)");
		downBorder.GetComponent<SpriteRenderer> ().enabled = true;

		startHostPosition = startHostObj.transform.position;
		startClientPosition = startClientObj.transform.position;
		backButtonPosition = backButtonObj.transform.position;
		ipAdressPosition = ipAdressObj.transform.position;

		NetworkManager.singleton.GetComponent<NetworkManagerHUD> ().showGUI = false;
		enableMultiplayerMenu ();

		startScreenWizard.SetActive (false);

		inMultiplayerMode = true;



	}

	public void enableMultiplayerMenu(){
		float diffTextFieldX = ipAdressObj.transform.position.x - startClientObj.transform.position.x;
		startHostObj.transform.position = playButtonObj.transform.position;
		startClientObj.transform.position = tutorialButtonObj.transform.position;
		backButtonObj.transform.position = levelButtonObj.transform.position;
		ipAdressObj.transform.position = startClientObj.transform.position + new Vector3 (diffTextFieldX, 0.0f, 0.0f);
	}

	//may not be needed..
	public void disableMultiplayerMenu(){
		startHostObj.SetActive (false);
		startClientObj.SetActive (false);
		backButtonObj.SetActive (false);
		ipAdressObj.SetActive(false);
	}

	
	public void setInMultiplayerMode(){
		inMultiplayerMode = true;
	}

	public void showClientJoinText(){
		clientWaitJoinText.transform.position = startHostObj.transform.position;
	}

	public void Update() {
//		if (!isServer)
//			return;
		GameObject[] wizards = GameObject.FindGameObjectsWithTag ("Wizards");
		if (wizards.Length >= 2 &&!started) {
			clientWaitJoinText.SetActive(false);
			started = true;
			GameManager.GetInstance().setMultiPlayer(true);
			NetworkManager.singleton.GetComponent<NetworkManagerHUD> ().enabled = false;
			//Invoke("StartLevel",2.0f);
			if (SyncController.GetInstance().isServer) 
				startMultiButton.transform.position = startHostPosition;
		}
	}


	public void StartHost() {
		disableMultiplayerMenu ();
		showClientJoinText ();
		NetworkManager.singleton.StartHost ();
	}

	public void StartClient() {
		disableMultiplayerMenu ();
		NetworkManager.singleton.StartClient ();
	}

	public void StartLevel(){
			GameManager.GetInstance ().loadLevel1 ();
	}
}
