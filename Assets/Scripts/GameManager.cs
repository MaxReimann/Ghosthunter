using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;



using System;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 


public class GameManager : NetworkBehaviour {
	
	private static GameManager instance;


	private static int totalLives = 5;
	[SyncVar]
	private int lives = totalLives;
	private static GameObject[] hearts = new GameObject[totalLives];

	public NetworkManager networkManager;
	private NetworkClient networkClient;

	private int score = 0;
	[SyncVar(hook="setAutoCreate")]
	private string currentLevel = "Menu";

	private bool hostStarted = false;

	private bool isMultiPlayer = false;



	AudioSource source;

	private string playerName = "Anonymus";

	private Dictionary<string, int> levelTimers = new Dictionary<string, int> (){
													{"Level1", 300},
												{"Menu" , 300},
												{"MultplayerScreen" , 300},
													{"Tutorial", 30},
													{"Level2", 40},
													{"Level3", 30},
													{"Level4", 30},
													{"Level5", 30}};
	
	private GameManager(){

	}

	public static GameManager GetInstance(){
		if(instance == null){
			//will make new gamemanager object
			Instantiate(Resources.Load("GameManager"), new Vector3(0,0,0), Quaternion.identity); 
			instance.setCurrentLevel(Application.loadedLevelName);
			if (instance == null)
			{
				print("not awake");
				instance = GameObject.Find("GameManager").GetComponent<GameManager>();
			}
		}
		return instance;
	}

	public void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
			this.setCurrentLevel(Application.loadedLevelName);
			source = gameObject.GetComponent<AudioSource>();
			source.loop = true;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public int getTimer(string level) {
		if (level == null) {
			level = "Level1"; //for debug in-editor starts
		}

		return levelTimers [level];
	}
		
		public void addScore(int score){
		this.score += score;
	}

	public int getScore(){
		return score;
	}

	public void setCurrentLevel(string level){
		this.currentLevel = level;
	}
	
	public string getCurrentLevel(){
		return this.currentLevel;
		//return networkManager.sce
	}

	public void setMultiPlayer(bool multiplayerGame) {
		this.isMultiPlayer = multiplayerGame;
	}
	
	public void nextLevel(){

		if (currentLevel == null) {
			currentLevel = Application.loadedLevelName;
		}
		
		if (currentLevel == "Level1") {
			loadLevel2();
			return;
		}
		if (currentLevel == "Level2") {
			loadLevel3();
			return;
		}
		if (currentLevel == "Level3") {
			loadLevel4();
			return;
		}
		if (currentLevel == "Level4") {
			loadLevel5();
			return;
		}
		if (currentLevel == "Level5") {
			loadWinScene();
			return;
		}

		if (currentLevel == "Tutorial") {
			loadTutorialEnd();
			return;
		}
		
		loadLevel1 ();
	}

	private void finalizeGame() {
		source.Stop();
		string now = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
		string oldscores = PlayerPrefs.GetString ("highscores");
		string newscores = oldscores + playerName + ";" + now + ";" + this.score.ToString() + "\n";
		PlayerPrefs.SetString ("highscores", newscores);
		PlayerPrefs.Save ();
	}
	
	public void timeout(){
		finalizeGame ();
		Application.LoadLevel("Timeout");
	}

	public void decreaseLive(){
		lives--;
		if (lives == 0) {
			gameOver ();
			return;
		}

		redrawHearts ();
	}

	private void redrawHearts() {
		for (int i = totalLives-1; i>lives-1; i--) {
			GameObject heart = hearts [i];
			if (heart != null) { // not triggered by menu
				SpriteRenderer renderer = heart.GetComponent<SpriteRenderer> ();
				Color color = renderer.color;
				color.a = 0.6f;
				renderer.color = color;
			}
		}
	}

	void OnLevelWasLoaded(int level) {
		if (!currentLevel.StartsWith ("Level") && currentLevel != "Tutorial")
			return;


		if (!isMultiPlayer && !hostStarted) {
			//NOTE: this spawns the player and ghosts at the right position
			networkClient = networkManager.StartHost ();
			hostStarted = true;
		}

		createLiveIndicators ();
//		GameObject[] wizards = GameObject.FindGameObjectsWithTag("Wizards");
//		foreach (GameObject wizard in wizards) {
//			wizard.GetComponent<WizardController> ().newLevelLoaded ();
//		}
	}

	
	private void gameOver(){
		finalizeGame ();
		//Application.LoadLevel("GameOver");
		//dont auto spawn players on the next screen
		networkManager.autoCreatePlayer = false; 
		networkManager.ServerChangeScene("GameOver");
	}

	private void loadWinScene(){
		source.Stop();
		//Application.LoadLevel("Win");
		networkManager.autoCreatePlayer = false;
		networkManager.ServerChangeScene("Win");
	}

	private void loadTutorialEnd(){
		source.Stop();
		//Application.LoadLevel("TutorialEnd");
		networkManager.autoCreatePlayer = false;
		networkManager.ServerChangeScene("TutorialEnd");
	}

	public void loadMultiplayerScreen(){
		networkManager.autoCreatePlayer = false;
		this.currentLevel = "MultiplayerScreen";
		networkManager.ServerChangeScene("MultiplayerScreen");
	}



	public void loadTutorial(){
		loadLevel ("Tutorial");
	}

	public void loadLevel1(){
		loadLevel ("Level1");

	}

	public void loadLevel2(){
		loadLevel ("Level2");
	}
	
	public void loadLevel3(){
		loadLevel ("Level3");
	}
	
	public void loadLevel4(){
		loadLevel ("Level4");
	}
	
	public void loadLevel5(){
		loadLevel ("Level5");
	}


	
	public void reloadLevel(){
		if (currentLevel == null) {
			loadLevel1();
			return;
		}
		loadLevel (currentLevel);
	}
	
	private void loadLevel(string level){
		if (!source.isPlaying) {
			source.Play();
		}
		networkManager.autoCreatePlayer = true; //spawn players on startpositions
		this.currentLevel = level;
		//Application.LoadLevel(level);
		
	networkManager.ServerChangeScene (level);
	}
	
	private void setAutoCreate(string unused){
		networkManager.autoCreatePlayer = true;
	}



	private void createLiveIndicators(){
		for(int i=0;i<totalLives;i++){
			float x = 8.2f - i*0.75f;
			GameObject heart = Instantiate(Resources.Load("Heart"), new Vector2(x,4.3f), Quaternion.identity) as GameObject;
			hearts[i] = heart;
		}
	}
	
}
