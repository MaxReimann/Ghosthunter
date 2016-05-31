using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

	private static float totalTimer;
	//static as there is only one game timer at once
	private static float timer;
	private Text txt;
	public Slider timeBarSlider;  //reference for slider
	public bool active = true;

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>(); 
		gameManager = GameManager.GetInstance();
		totalTimer = gameManager.getTimer (gameManager.getCurrentLevel ());
		timer = totalTimer;
	}

	public static float getTimeLeft(){
		return timer;
	}

	public static void setTimer(float time){
		timer = time;
	}

	public static void addOnTimer(float addOnTime){
		timer += addOnTime;
		if (timer > totalTimer)
			timer = totalTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			timer -= Time.deltaTime;
			timeBarSlider.value = timer / totalTimer;
			txt.text = "Time: " + (int)timer;
			if (timer < 1) {
				gameManager.timeout ();
			}
		}
	}
}
