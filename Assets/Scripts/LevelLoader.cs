using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	public void loadLevel1(){
		Debug.Log ("loading first level...");
		Application.LoadLevel("Level1");
	}

}
