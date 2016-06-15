using UnityEngine;
using System.Collections;

public class TreasureController : MonoBehaviour {

	bool hasTouched = false;

	public GameObject sparcleParticles;

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
			location.x = location.x+1.5f;
			Instantiate(Resources.Load("permanentSpellItem"),location,Quaternion.identity);
			Destroy(sparcleParticles.gameObject);
		}
	}
}
