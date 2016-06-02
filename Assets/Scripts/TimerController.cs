using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;


public class TimerController : NetworkBehaviour {

	[SyncVar(hook="OnTotalTimerChanged")]
	private float totalTimer = 30.0f;
	[SyncVar]
	private float timer;
	private Text txt;
	public Slider timeBarSlider;  //reference for slider
	public bool active = true;

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		txt = gameObject.GetComponent<Text>(); 
		gameManager = GameManager.GetInstance();

		if (isServer) {
			totalTimer = gameManager.getTimer (gameManager.getCurrentLevel ());
		}
		timer = totalTimer;
	}

	public float getTimeLeft(){
		return timer;
	}

	private void OnTotalTimerChanged(float value){
		timer = totalTimer;
	}

	[Server]
	public void setTimer(float time){
		timer = time;
	}

	[Server]
	public void addOnTimer(float addOnTime){
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
