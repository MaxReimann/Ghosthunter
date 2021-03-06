﻿using UnityEngine;
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

	[SyncVar(hook="OnHostStartedChanged")]
	public bool hostStarted = false;
	
	[SyncVar(hook="OnMultiplayerStatusChanged")]
	public bool isMultiPlayer = false;

	[SyncVar(hook="OnChangeAutoCreate")]
	public bool autoCreatePlayer = true;

	[SyncVar(hook="OnLevelNameChange")]
	public string levelName;

	[SyncVar(hook="OnScoreChanged")]
	public int score;
	

	private void OnLivesChanged(int lives){
		currentLives = lives;
		GameManager.GetInstance ().setCurrentLives (lives);
	}

	private void OnHostStartedChanged(bool started){
		this.hostStarted = started;
		GameManager.GetInstance ().setHostStarted (started);
	}

	private void OnMultiplayerStatusChanged(bool status){
		this.isMultiPlayer = status;
		GameManager.GetInstance ().setMultiPlayer (status);
	}

	private void OnChangeAutoCreate(bool autocreate){
		autoCreatePlayer = autocreate;
		NetworkManager.singleton.autoCreatePlayer = autocreate;
	}

	private void OnLevelNameChange(string levelName){
		this.levelName = levelName;
		GameManager.GetInstance().setCurrentLevel (levelName);
	}

	private void OnScoreChanged(int score){
		this.score = score;
		GameManager.GetInstance().setCurrentScore (score);
	}


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


}
