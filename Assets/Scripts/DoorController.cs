using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public GameObject otherDoor;
	[HideInInspector] public bool wasJustTeleported;
	[HideInInspector] public Vector2 dropOffLocation;

	private DoorController otherController;

	// Use this for initialization
	void Start () {
		wasJustTeleported = false;
		otherController = otherDoor.GetComponent<DoorController> ();
		float wizardYExtents = 0.515295f;//we will assume, that the size will not change during the game
		dropOffLocation = new Vector2(transform.position.x, GetComponent<BoxCollider2D> ().bounds.min.y + wizardYExtents);
	}


	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Wizards") && !wasJustTeleported) {
			WizardController wizardController = coll.gameObject.GetComponent<WizardController>();
			wizardController.setToPosition( otherController.dropOffLocation );
			otherController.wasJustTeleported = true;
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag ("Wizards")) {
			this.wasJustTeleported = false;
		}
	}
}
