﻿using UnityEngine;
using System.Collections;

public class LethalAreaController : MonoBehaviour {

	public GameObject resetPoint; //set in inspector

	void OnTriggerEnter2D(Collider2D coll){
		this.gameObject.layer = LayerMask.NameToLayer("UI");
	}


	void OnTriggerExit2D(Collider2D other){
		this.gameObject.layer = LayerMask.NameToLayer("LethalItems");
	}

}
