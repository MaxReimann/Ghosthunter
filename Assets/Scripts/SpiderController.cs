using UnityEngine;
using System.Collections;

public class SpiderController : MonoBehaviour {

	private float SPEED = 0.1f;
	public float DOWNADD = 0.1f;
	public bool direction_up = true;
	public float fromUp = 2.2f;
	public float toBottom = -2f;

	private float upTimer = 0.0f;
	public float stayUpTime = 2.0f;

	private float coolOff = 0.0f;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (upTimer > 0.0f) {
			upTimer -= Time.deltaTime;
			return;
		}


		Vector3 position = transform.position;
		if (direction_up) {
			position.y += SPEED;
		} else {
			position.y -= SPEED + DOWNADD;
		}
		transform.position = position;

		if (transform.position.y <= toBottom) {
			direction_up = true;
		} 
		if (transform.position.y >= fromUp) {
			if (direction_up)
				upTimer = stayUpTime;

			direction_up = false;
		}


	}
}
