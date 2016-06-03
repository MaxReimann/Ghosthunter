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

		NetworkManager.singleton.GetComponent<NetworkManagerHUD> ().showGUI = true;

		startScreenWizard.SetActive (false);

		inMultiplayerMode = true;

	}

	
	public void setInMultiplayerMode(){
		inMultiplayerMode = true;
	}

	public void showClientJoinText(){
		clientWaitJoinText.transform.position = new Vector3 (0,
		                                                    clientWaitJoinText.transform.position.y,
		                                                    clientWaitJoinText.transform.position.z);
	}

	public void Update() {
//		if (!isServer)
//			return;
		GameObject[] wizards = GameObject.FindGameObjectsWithTag ("Wizards");
		if (wizards.Length >= 2 &&!started) {
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
