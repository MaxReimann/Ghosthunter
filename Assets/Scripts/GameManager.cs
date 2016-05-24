using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager{
	
	private static GameManager instance;

	private GameManager(){

	}

	public static GameManager GetInstance(){
		if(instance == null){
			instance = new GameManager();
		}
		return instance;
	}

	
	private int score = 0;
	private int ghosts = 1; // number of ghosts in current level
	private string currentLevel;

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
		this.ghosts = GameObject.FindGameObjectsWithTag("Ghost").Length ;
	}

}
