using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class TutorialController : MonoBehaviour {

	private TutorialManager manager = TutorialManager.Instance;
	private Text text;

	private bool isGhostCreated = false;
	private bool spellItemCreated = false;
	private bool spellItemInvoked = false;
	private bool timeItemInvoked = false;
	private bool timeItemCreated = false;
	private bool startItemTut = false;
	private bool shieldItemInvoked = false;
	private bool shieldItemCreated;

	public TimerController timerController;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		manager.wasLeft = false;
		manager.wasRight = false;
		manager.hasShot = false;
		manager.hasTimeItem = false;
		manager.hasPermanentSpellItem = false;
		manager.spellItemCreated = false;
		manager.hasShieldItem = false;
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
			Invoke("createGhost",0.5f);
			isGhostCreated = true;
			return;
		}


		if (!spellItemInvoked && startItemTut) {
			text.text = "Collect the permanent spell item for staying spells";
			Invoke("createSpellItem", 1.0f);
			spellItemInvoked = true;
			return;
		}

		if (spellItemCreated && GameObject.FindGameObjectWithTag ("PermanentSpellItem") == null) {
			manager.hasPermanentSpellItem = true;
		}



		if (manager.hasPermanentSpellItem && !timeItemInvoked) {
			text.text = "Collect the time item for extra time";
			timerController.setTotalTimer(20.0f);
			timerController.setTimer(15.0f);
			timerController.active = true;
			Invoke("createTimeItem", 1.0f);
			timeItemInvoked = true;
			return;
		}

		if (timeItemCreated && GameObject.FindGameObjectWithTag ("AddonTimeItem") == null) {
			manager.hasTimeItem = true;
		}

		if (manager.hasTimeItem && !shieldItemInvoked) {
			text.text = "Collect the shield item for blocking of ghosts";
			timerController.active = false;
			Invoke ("createShieldItem", 1.0f);
			shieldItemInvoked = true;
			return;
		}


	}

	private void startItemTutorial(){
		startItemTut = true;
	}


	void createGhost(){
		GameObject ghost = Instantiate (Resources.Load ("L3Ghost"), new Vector2 (-5, 2), Quaternion.identity) as GameObject;
		Invoke ("startItemTutorial", 3.0f);
		NetworkServer.Spawn (ghost);

	}

	void createTimeItem(){
		Vector3 itemStart = GameObject.Find ("timeItemStart").transform.position;
		GameObject time = Instantiate (Resources.Load ("AddonTimeItem"), itemStart, Quaternion.identity) as GameObject;
		time.GetComponent<ItemController> ().itemLifeTime = 1000;
		timeItemCreated = true;
		NetworkServer.Spawn (time);
	}

	void createSpellItem(){
		Vector3 itemStart = GameObject.Find ("pSpellStart").transform.position;
		GameObject spell = Instantiate (Resources.Load ("permanentSpellItem"), itemStart, Quaternion.identity) as GameObject;
		spell.GetComponent<ItemController> ().itemLifeTime = 1000;
		spellItemCreated = true;
		NetworkServer.Spawn (spell);
	}

	void createShieldItem(){
		Vector3 itemStart = GameObject.Find ("shieldItemStart").transform.position;
		GameObject shield = Instantiate (Resources.Load ("ShieldItem"), itemStart, Quaternion.identity) as GameObject;
		shield.GetComponent<ItemController> ().itemLifeTime = 1000;
		shieldItemCreated = true;
		NetworkServer.Spawn (shield);
	}
}
