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

	public NetworkManager networkManager;
	private NetworkClient networkClient;

	private int score = 0;
	[SyncVar]
	private string currentLevel = "Menu";


	private bool hostStarted = false;

	[SyncVar]
	private bool isMultiPlayer = false;



	AudioSource source;

	private string playerName = "Anonymus";

	private Dictionary<string, int> levelTimers = new Dictionary<string, int> (){
													{"Level1", 30},
												{"Menu" , 300},
												{"MultplayerScreen" , 300},
													{"Tutorial", 30},
													{"Level2", 40},
													{"Level3", 30},
													{"Level4", 30},
													{"Level5", 40}};
	
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

	public void checkGameFinished(){
		int ghostCount = GameObject.FindGameObjectsWithTag("Ghost").Length-1;
		int zombieCount = GameObject.FindGameObjectsWithTag("Ghost").Length-1;
		if (ghostCount == 0 && zombieCount == 0) {
			nextLevel();
		}
	}

	public int getTotalLives(){
		return totalLives;
	}

	public int getCurrentLives(){
		return lives;
	}

	public void setCurrentLives(int lives){
		this.lives = lives;
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
		if (this.isMultiPlayer != SyncController.GetInstance ().isMultiPlayer)
			SyncController.GetInstance ().isMultiPlayer = multiplayerGame;
	}

	public bool IsMultiplayer() {
		return this.isMultiPlayer;
	}

	public void setHostStarted(bool started){
		this.hostStarted = started;
		if (hostStarted != SyncController.GetInstance ().hostStarted)
			SyncController.GetInstance ().hostStarted = started;
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
		SyncController.GetInstance ().currentLives = --lives;
		if (lives == 0) {
			if(currentLevel != "Tutorial"){
				gameOver ();
			}
		}
	}

	void OnLevelWasLoaded(int level) {
		currentLevel = Application.loadedLevelName;


		if (!currentLevel.StartsWith ("Level") && currentLevel != "Tutorial")
			return;

		print ("is Multiplayer");
		if (!isMultiPlayer && !hostStarted) {
			print ("onlevelload: start host");
			//NOTE: this spawns the player and ghosts at the right position
			networkClient = networkManager.StartHost ();
			this.setHostStarted(true);
		}


//		GameObject[] wizards = GameObject.FindGameObjectsWithTag("Wizards");
//		foreach (GameObject wizard in wizards) {
//			wizard.GetComponent<WizardController> ().newLevelLoaded ();
//		}
	}

	
	private void gameOver(){
		finalizeGame ();
		//Application.LoadLevel("GameOver");
		//dont auto spawn players on the next screen
		setAutoCreate (false);
		networkManager.ServerChangeScene("GameOver");

	}

	private void loadWinScene(){
		source.Stop();
		//Application.LoadLevel("Win");
		setAutoCreate (false);
		networkManager.ServerChangeScene("Win");
	}

	private void loadTutorialEnd(){
		source.Stop();
		//Application.LoadLevel("TutorialEnd");
		setAutoCreate (false);
		networkManager.ServerChangeScene("TutorialEnd");
	}

	public void loadMultiplayerScreen(){
		setAutoCreate (false);
		this.currentLevel = "MultiplayerScreen";
		networkManager.ServerChangeScene("MultiplayerScreen");
	}

	public void setPlayerName(string name){
		playerName = name;
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

	public void loadMainMenu(){
		lives = totalLives;
		Application.LoadLevel("Menu");
	}
	
	public void reloadLevel(){
		if (lives == 0) {
			lives = totalLives;
			SyncController.GetInstance().currentLives = lives;
			loadLevel1();
			return;
		}
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
		setAutoCreate (true);
		this.currentLevel = level;
		//Application.LoadLevel(level);
		
	networkManager.ServerChangeScene (level);
	}
	
	private void setAutoCreate(bool autoCreate){
		networkManager.autoCreatePlayer = autoCreate;
		SyncController.GetInstance ().autoCreatePlayer = autoCreate;

	}
	
}
