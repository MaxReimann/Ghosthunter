using UnityEngine;
using System.Collections;

public class SawController : MonoBehaviour {

	private static float OFFSET = 0.05f;
	public bool direction_up = true;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		if (direction_up) {
			position.y += OFFSET;
		} else {
			position.y -= OFFSET;
		}
		transform.position = position;

		if (transform.position.y <= -2f) {
			direction_up = true;
		} 
		if (transform.position.y >= 2.2f) {
			direction_up = false;
		}


	}
}
