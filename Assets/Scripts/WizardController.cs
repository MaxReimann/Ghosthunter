using UnityEngine;
using System.Collections;

public class WizardController : MonoBehaviour {

	private static float SPELL_DELAY = 0.3f;

	public float speed = 10;
	public float spellSpeed = 10;
	private Rigidbody2D myBody;
	private Vector3 spellStartPoint; 

	private GameManager gameManager;

	private bool isLeft = false;
	private float spellExecution;
	
	private Animator animator;

	// Use this for initialization
	void Start (){
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		gameManager = GameManager.instance;
	}

	// Update is called once per frame
	void Update (){

		if (spellExecution >0 && spellExecution+SPELL_DELAY < Time.time) {
			createSpellParticle();
			spellExecution = 0;
		}

		float horizontalInput = Input.GetAxisRaw ("Horizontal");
		Move(horizontalInput);

		if (Input.GetKeyDown ("space")) {
			createSpell ();
			return;
		}

		if (horizontalInput < 0) {
			isLeft = true;
			animator.SetTrigger("wizard_run_left");
		}else if (horizontalInput == 0) {
			animator.SetTrigger("wizard_idle");
		}else{
			isLeft = false;
			animator.SetTrigger("wizard_run_right");
		}
	}

	public void createSpell(){
		if(GameObject.FindGameObjectWithTag("Spell")){
			return;
		}

		animator.SetTrigger("wizard_attack");

		spellStartPoint = new Vector3 ();
		if (isLeft) {
			spellStartPoint.x = transform.position.x-0.5f;
		} else {
			spellStartPoint.x = transform.position.x+0.5f;
		}
		spellStartPoint.y = transform.position.y-1;
		spellStartPoint.z = transform.position.z;

		spellExecution = Time.time;
	}

	private void createSpellParticle(){

		GameObject spell = Instantiate(Resources.Load("Spell"), spellStartPoint, Quaternion.identity) as GameObject;
		
		Rigidbody2D rigidBody = spell.GetComponent<Rigidbody2D>();
		rigidBody.velocity = transform.up * spellSpeed;
	}
	
	public void Move(float horizontalInput){
		Vector2 moveVel = myBody.velocity;
		moveVel.x = horizontalInput * speed;
		myBody.velocity = moveVel;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Ghost") {
//			if (!coll.gameObject.GetComponent<GhostController>().nonColliding())
	//		{
				//TODO decrease life or gameOver
				//Destroy(gameObject);
				gameManager.gameOver();
		//	}
		}
	}
}
