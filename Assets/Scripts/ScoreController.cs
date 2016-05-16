using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	Text txt;
	int score = -1;
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>(); 
		gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		txt.text = "Score: " + gameManager.getScore ();
	}
}
