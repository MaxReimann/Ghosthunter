using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{
	
	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

	private int level = 1;                                  //Current level number, expressed in game as "Day 1".
	private int score = 0;
	private int ghosts = 1; // number of ghosts in current level

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

		this.level++;
		Debug.Log (level);
		Application.LoadLevel (this.level);


		this.ghosts = GameObject.FindGameObjectsWithTag("Ghost").Length ;
	}

	public void gameOver(){
		Application.LoadLevel("GameOver");
	}

	public void timeout(){
		Application.LoadLevel("Timeout");
	}
}
