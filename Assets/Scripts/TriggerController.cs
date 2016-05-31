using UnityEngine;
using System.Collections;

public class TriggerController : MonoBehaviour {

	private TutorialManager manager = TutorialManager.Instance;

	void OnTriggerEnter2D(Collider2D coll){

		if (!manager.wasLeft) {
			if(gameObject.name == "Trigger Left") {
				manager.wasLeft = true;
				Destroy(gameObject);
			}
			return;
		}

		if (!manager.wasRight) {
			if(gameObject.name == "Trigger Right") {
				manager.wasRight = true;
				Destroy(gameObject);
			}
			return;
		}

		if (!manager.hasShot) {
			if(gameObject.name == "Trigger Up") {
				manager.hasShot = true;
				Destroy(gameObject);
			}
			return;
		}
	}
}
