using UnityEngine;
using System.Collections;

public class MovingPlatformLtoR : MonoBehaviour {

	private static float WAIT_TIME = 1.5f;
	private static float OFFSET = 0.05f;
	public bool direction_up = true;
	
	private float waitTimer = 0.0f;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (waitTimer >= 0) {
			waitTimer -= Time.deltaTime;
		}else{
			Vector3 position = transform.position;
			if (direction_up) {
				position.x += OFFSET;
			} else {
				position.x -= OFFSET;
			}
			transform.position = position;
			
			if (transform.position.x <= -1.5f) {
				direction_up = true;
				waitTimer = WAIT_TIME;
			} 
			if (transform.position.x >= 1.8f) {
				direction_up = false;
				waitTimer = WAIT_TIME;
			}	
		}	
	}
}
