using UnityEngine;
using System.Collections;

public class MovingPlatformController : MonoBehaviour {

	private GameObject child;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = gameObject.transform.position;
		pos.y = pos.y - 0.001f;
		gameObject.transform.position = pos;
	}
}
