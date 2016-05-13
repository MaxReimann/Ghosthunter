﻿using UnityEngine;
using System.Collections;

public class SpellController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetChild(0).GetComponent<Renderer>().sortingLayerName="Spells";
	}
	
	// Update is called once per frame
	void Update () {
	}

	// called by child script
	public void collision(Collision2D coll) {
		Destroy(this.gameObject);

		if (coll.gameObject.tag == "Ghost") {
			GhostController ghost = coll.gameObject.GetComponent<GhostController>();
			ghost.spellCollision ();
		}


	}
}
