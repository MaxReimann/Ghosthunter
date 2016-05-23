using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

	private float timer = 40;
	private Text txt;

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>(); 
		gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		txt.text = "Time: "+(int)timer;
		if (timer < 1) {
			gameManager.timeout();
		}
	}
}
