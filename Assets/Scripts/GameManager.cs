using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 


public class GameManager : MonoBehaviour {
	
	private static GameManager instance;

	private int score = 0;
	private string currentLevel;
	AudioSource source;

	private string playerName = "Anonymus";

	private Dictionary<string, int> levelTimers = new Dictionary<string, int> (){
													{"Level1", 30},
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
	}
	
	public void nextLevel(){
		
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
	
	public void gameOver(){
		finalizeGame ();
		Application.LoadLevel("GameOver");
	}

	private void loadWinScene(){
		source.Stop();
		Application.LoadLevel("Win");
	}
	
	public void loadLevel1(){
		source.Play ();
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
		source.Play ();
		if (currentLevel == null) {
			loadLevel1();
			return;
		}
		loadLevel (currentLevel);
	}
	
	private void loadLevel(string level){
		Debug.Log (level);
		this.currentLevel = level;
		Application.LoadLevel(level);
	}

}
