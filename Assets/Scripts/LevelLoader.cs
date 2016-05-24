using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	private GameManager gameManager;
	private string currentLevel = "Level1";

	public void Start(){
		gameManager = GameManager.instance;
	}

	public void loadNext(){
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
			loadWin();
			return;
		}

		loadLevel1();
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
