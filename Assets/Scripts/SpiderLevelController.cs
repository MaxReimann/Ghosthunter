using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpiderLevelController : NetworkBehaviour {

	private bool lightsAdded = false;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (lightsAdded)
			return;


		GameObject[] wizards = GameObject.FindGameObjectsWithTag ("Wizards");
		if (GameManager.GetInstance ().IsMultiplayer () && wizards.Length < 2)
			return;

		foreach (GameObject wizard in wizards) {
			wizard.GetComponent<WizardController> ().AddSpotLight ();
			lightsAdded = true;
			print ("lightsadded");
		}
	
	}
}
