using UnityEngine;
using System.Collections;

public class SpellController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnBecameInvisible() {
		print("invisible");
		Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		print ("collision");
	}
}
