using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public enum ItemType {
		Spell,
		Shield
	};


	public ItemType itemType; //set in inspector
	[Tooltip("Only used, if item type is spell")]
	public SpellController.SpellType spellType; //set in inspector

	public float itemLifeTime; //time of apperance in seconds


	// Use this for initialization
	void Start () {
	
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
				wizardController.ActivateShield(6.0f);
			}
			//TODO: add shield type


			Destroy (this.gameObject);
		}
	}

}
