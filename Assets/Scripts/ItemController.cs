using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public enum ItemType {
		Spell,
		Shield
	};


	public ItemType itemType; //set in inspector
	public SpellController.SpellType spellType; //set in inspector
	public float lifeTime; //time of apperance in seconds


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Wizard")) {
			WizardController wizardController = GameObject.FindGameObjectWithTag ("Wizard").GetComponent<WizardController> ();

			if (itemType == ItemType.Spell){
				wizardController.SetSpellType(this.spellType);
			}
			//TODO: add shield type


			Destroy (this.gameObject);
		}
	}

}
