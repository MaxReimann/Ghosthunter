using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour {
	
	private static GameManager instance;

	private int score = 0;
	private string currentLevel;


	private Dictionary<string, int> levelTimers;

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
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public int getTimer(string level) {
		if (levelTimers == null) {
			levelTimers = new Dictionary<string, int> (){
				{"Level1", 30},
				{"Level2", 40},
				{"Level3", 30},
				{"Level4", 30},
				{"Level5", 30}
			};
		}

		if (level == null)
			level = "Level1"; //for debug in-editor starts

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
			Application.LoadLevel("Win");
			return;
		}
		
		loadLevel1 ();
	}
	
	public void timeout(){
		Application.LoadLevel("Timeout");
	}
	
	public void gameOver(){
		Application.LoadLevel("GameOver");
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
		Debug.Log (level);
		this.currentLevel = level;
		Application.LoadLevel(level);
	}

}
