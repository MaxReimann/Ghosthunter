using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialController : MonoBehaviour {

	private TutorialManager manager = TutorialManager.Instance;
	private Text text;

	private bool isGhostCreated = false;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		manager.wasLeft = false;
		manager.wasRight = false;
		manager.hasShot = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!manager.wasLeft) {
			text.text="Move to the left using the arrow keys";
			return;
		}
		if (!manager.wasRight) {
			text.text="Move to the right using the arrow keys";
			return;
		}
		if (!manager.hasShot) {
			text.text="Create a spell by either tapping the screen or pressing the space bar";
			return;
		}

		if (!isGhostCreated) {
			text.text="Ban all the ghosts with a spell!!";
			Invoke("createGhost",1f);
			isGhostCreated = true;
		}
	}

	void createGhost(){
		Instantiate(Resources.Load("L2Ghost"), new Vector2(-5,2), Quaternion.identity);
	}
}
