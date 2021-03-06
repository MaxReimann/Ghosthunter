﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ItemController : NetworkBehaviour {

	public enum ItemType {
		Spell,
		Shield,
		AddonTime
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

	public void DestroyItem() {
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[Server]
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Wizards")) {
//			WizardController wizardController;
//			if (!GameManager.GetInstance().isMultiplayer())
			WizardController	wizardController = coll.gameObject.GetComponent<WizardController> ();

			if (itemType == ItemType.Spell){
				wizardController.SetSpellType(this.spellType);
			}

			if (itemType == ItemType.Shield){
				wizardController.ActivateShield(powerDuration);
			}

			if (itemType == ItemType.AddonTime){
				(FindObjectOfType(typeof(TimerController)) as TimerController).addOnTimer(powerDuration);
			}

			Destroy (this.gameObject);
		}
	}

}
