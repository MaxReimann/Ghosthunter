using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameManager.GetInstance();
	}

	//delegating methods as gameManager is a singleton but can NOT be added as a gameObject (detelet by singleton method)

	public void reloadLevel(){
		this.gameManager.reloadLevel ();
	}

	public void loadTutorial(){
		this.gameManager.loadTutorial();
	}

	public void loadLevel1(){
		this.gameManager.loadLevel1();
	}
	
	public void loadLevel2(){
		this.gameManager.loadLevel("Level2");
	}
	
	public void loadLevel3(){
		this.gameManager.loadLevel("Level3");
	}
	
	public void loadLevel4(){
		this.gameManager.loadLevel("Level4");
	}
	
	public void loadLevel5(){
		this.gameManager.loadLevel("Level5");
	}

	public void loadLevel6(){
		this.gameManager.loadLevel("Level6");
	}

	public void loadLevel7(){
		this.gameManager.loadLevel("Level7");
	}
	
	public void LoadMainMenu(){
		this.gameManager.loadMainMenu();
	}

	public void LoadLevelOverview(){
		Application.LoadLevel("Levels");
	}


}
