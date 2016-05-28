using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

	private float totalTimer;
	private float timer;
	private Text txt;
	public Slider timeBarSlider;  //reference for slider
	
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>(); 
		gameManager = GameManager.GetInstance();
		totalTimer = gameManager.getTimer (gameManager.getCurrentLevel ());
		timer = totalTimer;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		timeBarSlider.value -= Time.deltaTime / totalTimer;
		txt.text = "Time: "+(int)timer;
		if (timer < 1) {
			gameManager.timeout();
		}
	}
}
