﻿using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public enum ItemType {
		Spell,
		Shield
	};


	public ItemType itemType; //set in inspector
	[Tooltip("Only used, if item type is spell")]
	public SpellController.SpellType spellType; //set in inspector

	public float itemLifeTime = 20.0f; //time of apperance in seconds
	public float powerDuration = 10.0f; // duration of the items power on wizard


	// Use this for initialization
	void Start () {
		Invoke ("DestroyItem", itemLifeTime);	
	}

	void DeleteItem() {
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Wizards")) {
			WizardController wizardController = GameObject.FindGameObjectWithTag ("Wizards").GetComponent<WizardController> ();

			if (itemType == ItemType.Spell){
				wizardController.SetSpellType(this.spellType);
			}

			if (itemType == ItemType.Shield){
				wizardController.ActivateShield(powerDuration);
			}
			//TODO: add shield type


			Destroy (this.gameObject);
		}
	}

}
