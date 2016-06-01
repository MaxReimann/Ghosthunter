using UnityEngine;
using System.Collections;

public class MovingPlatformLtoR : MonoBehaviour {

	private static float OFFSET = 0.05f;
	public bool direction_left = true;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		if (direction_left) {
			position.x += OFFSET;
		} else {
			position.x -= OFFSET;
		}
		transform.position = position;
		
		if (transform.position.x <= -1.5f) {
			direction_left = true;
		} 
		if (transform.position.x >= 4.3f) {
			direction_left = false;
		}
		
		
	}
}
