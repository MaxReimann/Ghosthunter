using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
	
	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

	private int score = 0;
	private int ghosts = 1; // number of ghosts in current level
	private string currentLevel;

	//Awake is always called before any Start functions
	void Awake()
	{
		//Check if instance already exists
		if (instance == null)
			//if not, set instance to this
			instance = this;
		
		//If instance already exists and it's not this:
		else if (instance != this)
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		

		//Call the InitGame function to initialize the first level 
		InitGame();
	}
	
	//Initializes the game for each level.
	void InitGame()
	{
		//setup the scene
		this.ghosts = GameObject.FindGameObjectsWithTag("Ghost").Length ;
	}
	
	
	
	//Update is called every frame.
	void Update()
	{
		
	}

	public void decreaseGhostCount(){
		this.ghosts--;
		if (this.ghosts <= 0)
			nextLevel ();
	}

	public void increaseGhostCount(){
		this.ghosts++;
	}

	public void addScore(int score){
		this.score += score;
	}

	public int getScore(){
		return score;
	}

	public void nextLevel(){

		if (currentLevel == "Level5") {
			Application.LoadLevel("Win");
			return;
		}

		if (currentLevel == null) {
			loadLevel1();
		}else if (currentLevel == "Level1") {
			loadLevel2();
		}else if (currentLevel == "Level2") {
			loadLevel3();
		}else if (currentLevel == "Level3") {
			loadLevel4();
		}else if (currentLevel == "Level4") {
			loadLevel5();
		}
		this.ghosts = GameObject.FindGameObjectsWithTag("Ghost").Length ;
	}
	
	public void reloadLevel(){
		Application.LoadLevel(currentLevel);
	}
	
	public void loadLevel1(){
		currentLevel = "Level1";
		Application.LoadLevel(currentLevel);
	}
	
	public void loadLevel2(){
		currentLevel = "Level2";
		Application.LoadLevel(currentLevel);
	}
	
	public void loadLevel3(){
		currentLevel = "Level3";
		Application.LoadLevel(currentLevel);
	}
	
	public void loadLevel4(){
		currentLevel = "Level4";
		Application.LoadLevel(currentLevel);
	}
	
	public void loadLevel5(){
		currentLevel = "Level5";
		Application.LoadLevel(currentLevel);
	}
	
	public void LoadMainMenu(){
		Application.LoadLevel("Menu");
	}
	
	public void LoadLevelOverview(){
		Application.LoadLevel("Levels");
	}
	
	public void loadGameOver(){
		Application.LoadLevel("GameOver");
	}
	
	public void loadTimeout(){
		Application.LoadLevel("Timeout");
	}
	
	public void loadWin(){
		Application.LoadLevel("Win");
	}

}
