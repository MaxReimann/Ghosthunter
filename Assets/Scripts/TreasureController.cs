using UnityEngine;
using System.Collections;

public class TreasureController : MonoBehaviour {

	bool hasTouched = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag=="Wizards" && !hasTouched) {
			hasTouched = true;
			Vector2 location = transform.position;
			location.x = location.x+2;
			Instantiate(Resources.Load("AddonTimeItem"),location,Quaternion.identity);
		}
	}
}
