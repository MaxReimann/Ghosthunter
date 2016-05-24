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

	public void loadLevel1(){
		this.gameManager.loadLevel1();
	}
	
	public void loadLevel2(){
		this.gameManager.loadLevel2();
	}
	
	public void loadLevel3(){
		this.gameManager.loadLevel3();
	}
	
	public void loadLevel4(){
		this.gameManager.loadLevel4();
	}
	
	public void loadLevel5(){
		this.gameManager.loadLevel5();
	}
	
	public void LoadMainMenu(){
		Application.LoadLevel("Menu");
	}
	
	public void LoadLevelOverview(){
		Application.LoadLevel("Levels");
	}


}
