using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	
	private static float OFFSET = 0.01f;
	private static float HIT_TIME = 3f;
	private static float TOGGLE_TIME = 0.2f;

	private float nonCollisionTimer = 0.0f;
	private float toggleTimer = 0.0f;

	public bool direction_right = true;
	private bool isHit = false;
	private bool isDead = false;

	private int hitCount;

	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		if (!direction_right) {
			animator.SetTrigger ("zombie_walk_left");
		}
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		gameManager = GameManager.GetInstance ();
	}
	
	// Update is called once per frame
	void Update () {

		if (nonCollisionTimer > 0.0f) {
			nonCollisionTimer -= Time.deltaTime;
		} else {
			isHit = false;
			gameObject.layer = LayerMask.NameToLayer("Zombies");
			spriteRenderer.enabled=true;
		}

		if(!isDead){
			if (!isHit) {
				doWalk ();
			} else {
				toggleVisibility();
			}
		}
	}

	private void toggleVisibility(){		
		if (toggleTimer > 0) {
			toggleTimer -= Time.deltaTime;
			return;
		}
		spriteRenderer.enabled = !spriteRenderer.isVisible;
		toggleTimer = TOGGLE_TIME;
	}

	private void doWalk(){
		Vector3 position = transform.position;
		if (direction_right) {
			position.x += OFFSET;
		} else {
			position.x -= OFFSET;
		}
		transform.position = position;
	}

	private void explode(){
		animator.SetTrigger ("zombie_explode");
		gameObject.layer = LayerMask.NameToLayer("NonCollZombies");
		spriteRenderer.enabled=true;
		Invoke ("doExplode", 1f);
	}

	private void doExplode(){
		Destroy(gameObject);
		gameManager.addScore(5);
	}

	void OnCollisionEnter2D(Collision2D coll){

		if (coll.gameObject.tag == "Spell") {
			isHit = true;
			hitCount++;
			if(hitCount == 3){
				isDead = true;
				explode();
			}else{
				nonCollisionTimer = HIT_TIME;
				gameObject.layer = LayerMask.NameToLayer("NonCollZombies");
			}
			return;
		}

		if (coll.gameObject.tag == "Wizards") {
			isHit = true;
			nonCollisionTimer = HIT_TIME;
			gameObject.layer = LayerMask.NameToLayer("NonCollZombies");
			return;
		}


		if (coll.gameObject.tag == "LeftBorder") {
			animator.SetTrigger ("zombie_walk_right");
			direction_right = true;
			return;
		}

		if (coll.gameObject.tag == "RightBorder") {
			animator.SetTrigger ("zombie_walk_left");
			direction_right = false;
			return;
		}


	}


}
