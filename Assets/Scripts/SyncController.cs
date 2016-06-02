using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;


//This class is basically a workaround to not have a networkidentity on the gamemanager,
// which causes strange issues to happen. All variables of the gamemanager (and other global variables), which
// should be synced, should be put in here
public class SyncController : NetworkBehaviour {

	private static SyncController instance;

	[SyncVar(hook="OnLivesChanged")]
	public int currentLives;

	[SyncVar]
	private string currentLevel = "Menu";

	private bool hostStarted = false;
	
	[SyncVar]
	private bool isMultiPlayer = false;
	

	// Use this for initialization
	public void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}


	public static SyncController GetInstance(){
		if(instance == null){
			//will make new gamesync object, implicitly calls awake
			Instantiate(Resources.Load("GameSync"), new Vector3(0,0,0), Quaternion.identity); 
		}
		return instance;
	}

	private void OnLivesChanged(int lives){
		currentLives = lives;
		GameManager.GetInstance ().setCurrentLives (lives);
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
