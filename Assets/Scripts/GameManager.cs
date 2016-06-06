using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;



using System;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 


public class GameManager : NetworkBehaviour {
	
	private static GameManager instance;
	private AudioManager audioManager;

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

	private string playerName = "Anonymus";

// time for levels depending on platform, as the game on mobile is generally harder than on pc
#if (UNITY_ANDROID || UNITY_IOS)
	private Dictionary<string, int> levelTimers = new Dictionary<string, int> (){
													{"Level1", 30},
													{"Menu" , 300},
													{"Tutorial", 30},
													{"Level2", 40},
													{"Level3", 34},
													{"Level4", 37},
													{"Level5", 50}};
#else
	private Dictionary<string, int> levelTimers = new Dictionary<string, int> (){
		{"Level1", 30},
		{"Menu" , 300},
		{"Tutorial", 30},
		{"Level2", 35},
		{"Level3", 30},
		{"Level4", 37},
		{"Level5", 50}};
#endif
	
	private GameManager(){

	}

	public static GameManager GetInstance(){
		if(instance == null){
			//will make new gamemanager object
			Instantiate(Resources.Load("GameManager"), new Vector3(0,0,0), Quaternion.identity); 
			if (instance == null){
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
			audioManager = AudioManager.GetInstance();
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public void checkGameFinished(){
		GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
		foreach (GameObject ghost in ghosts) {
			if (!ghost.GetComponent<GhostController>().isHitL1)
				return; //any ghost which is found is not already hit returns from function
		}

		GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
		foreach (GameObject zombie in zombies) {
			if (!zombie.GetComponent<ZombieController>().IsDead())
				return; //any zombie which is found is not already hit returns from function
		}

		nextLevel();
	}


	/////// getters & setter /////////////

	public int getTotalLives(){
		return totalLives;
	}

	public int getCurrentLives(){
		return lives;
	}


	public int getTimer(string level) {
		return levelTimers [level];
	}
		
	public void addScore(int score){
		this.score += score;
		setCurrentScore (this.score);
	}

	public int getScore(){
		return score;
	}

	public string getCurrentLevel(){
		return this.currentLevel;
	}

	public bool IsMultiplayer() {
		return this.isMultiPlayer;
	}

	public void decreaseLive(){
		setCurrentLives(lives-1);
		if (lives == 0) {
			if(currentLevel != "Tutorial"){
				gameOver ();
			}
		}
	}


	private void finalizeGame() {
		audioManager.stop ();
		string now = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
		string oldscores = PlayerPrefs.GetString ("highscores");
		string newscores = oldscores + playerName + ";" + now + ";" + this.score.ToString() + "\n";
		PlayerPrefs.SetString ("highscores", newscores);
		PlayerPrefs.Save ();
	}
	



	void OnLevelWasLoaded(int level) {
		this.networkManager = NetworkManager.singleton;

		string currentScene = Application.loadedLevelName;

		if ((!currentScene.StartsWith ("Level") && !currentScene.Equals ("Tutorial")) || currentScene.Equals( "Levels"))
			return;

		print ("on level was loaded...");
		
		if (!isMultiPlayer && !hostStarted) {
			print ("onlevelload: start host");
			//NOTE: this spawns the player and ghosts at the right position
			networkClient = networkManager.StartHost ();
			this.setHostStarted(true);
			OnStartSinglePlayer();

		}
	}

	void OnStartSinglePlayer() {
		NetworkAnimator[] networkAnimators = FindObjectsOfType (typeof(NetworkAnimator)) as NetworkAnimator[];
		foreach (NetworkAnimator anim in networkAnimators) {
			anim.enabled = false;
		}

	}


	//// scene loading ///////
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

	private void gameOver(){
		setAutoCreate (false);
		finalizeGame ();
		networkManager.ServerChangeScene("GameOver");
		audioManager.playGameOverMusic();
	}

	public void timeout(){
		setAutoCreate (false);
		networkManager.ServerChangeScene("Timeout");
		audioManager.playGameOverMusic();
	}

	private void loadWinScene(){
		setAutoCreate (false);
		networkManager.ServerChangeScene("Win");
		audioManager.playWinMusic();
	}

	private void loadTutorialEnd(){
		setAutoCreate (false);
		networkManager.ServerChangeScene("TutorialEnd");
		audioManager.playWinMusic();
	}

	public void loadMainMenu(){
		audioManager.playMenuMusic ();
		setCurrentLives(totalLives);
		setAutoCreate (false);
		setCurrentScore (0);

		if (networkManager != null) {
			networkManager.StopHost ();
		}
		
		Invoke ("_delayedLoadMenu", 0.7f); //wait to stop hosts
	}


	private void _delayedLoadMenu(){
		Destroy (networkManager.gameObject);
		Application.LoadLevel ("Menu");
		hostStarted = false;
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


	
	public void reloadLevel(){
		if (lives == 0) {
			setCurrentLives(totalLives);
			setCurrentScore(0);
			loadLevel1();
			return;
		}
		if (currentLevel == null) {
			loadLevel1();
			return;
		}
		loadLevel (currentLevel);
	}

	[Server]
	private void loadLevel(string level){
		audioManager.playGameMusic();
		setCurrentLevel(level);
		setAutoCreate (true);
		networkManager.ServerChangeScene (level);
	}
	



	////// state syncing setters

	private void setAutoCreate(bool autoCreate){
		networkManager.autoCreatePlayer = autoCreate;
		if (SyncController.GetInstance ().autoCreatePlayer != autoCreate)
			SyncController.GetInstance ().autoCreatePlayer = autoCreate;

	}

	public void setMultiPlayer(bool multiplayerGame) {
		this.isMultiPlayer = multiplayerGame;
		if (this.isMultiPlayer != SyncController.GetInstance ().isMultiPlayer)
			SyncController.GetInstance ().isMultiPlayer = multiplayerGame;
	}


	public void setHostStarted(bool started){
		this.hostStarted = started;
		if (hostStarted != SyncController.GetInstance ().hostStarted)
			SyncController.GetInstance ().hostStarted = started;
	}


	public void setCurrentLives(int lives){
		this.lives = lives;
		if (lives != SyncController.GetInstance ().currentLives)
			SyncController.GetInstance ().currentLives = lives;
	}

	public void setCurrentLevel(string level){
		this.currentLevel = level;
		if (currentLevel != SyncController.GetInstance ().levelName)
			SyncController.GetInstance ().levelName = currentLevel;
	}

	public void setCurrentScore(int score){
		this.score = score;
		if (score != SyncController.GetInstance ().score)
			SyncController.GetInstance ().score = score;
	}
	
}
