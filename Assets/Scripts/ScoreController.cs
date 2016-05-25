using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	Text txt;
	GameManager gameManager;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>(); 
		gameManager = GameManager.GetInstance();
	}
	
	// Update is called once per frame
	void Update () {
		txt.text = "Score: " + gameManager.getScore ();
	}
}
