using UnityEngine;
using System.Collections;

public class TriggerController : MonoBehaviour {

	private TutorialManager manager = TutorialManager.Instance;

	void OnTriggerEnter2D(Collider2D coll){
		if (!manager.wasLeft && gameObject.name == "Trigger Left") {
			manager.wasLeft = true;
			Destroy(gameObject);
			return;
		}
		if (!manager.wasRight && gameObject.name == "Trigger Right") {
			manager.wasRight = true;
			Destroy(gameObject);
			return;
		}
	
		if (!manager.hasShot && gameObject.name == "Trigger Up") {
			manager.hasShot = true;
			Destroy(gameObject);
			return;
		}
	}
}
