using UnityEngine;
using System.Collections;

public class WizardController : MonoBehaviour {

	public float speed = 10;
	public float spellSpeed = 10;
	private Rigidbody2D myBody;
	private Vector3 spellStartPoint; 

	private GameManager gameManager;

	private bool isLeft = false;
	
	private Animator animator;

	// Use this for initialization
	void Start (){
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update (){


		float horizontalInput = Input.GetAxisRaw ("Horizontal");

		Move(horizontalInput);

		if (horizontalInput < 0) {
			isLeft = true;
			animator.SetTrigger("wizard_run_left");
		} 
		if (horizontalInput == 0) {
			animator.SetTrigger("wizard_idle");
		}
		if (horizontalInput > 0) {
			isLeft = false;
			animator.SetTrigger("wizard_run_right");
		}

		if (Input.GetKeyDown ("space")) {
			createSpell ();
		}
	}

	public void createSpell(){
		if(GameObject.FindGameObjectWithTag("Spell")){
			return;
		}
		spellStartPoint = new Vector3 ();
		if (isLeft) {
			spellStartPoint.x = transform.position.x-1;
		} else {
			spellStartPoint.x = transform.position.x+0.5f;
		}
		spellStartPoint.y = transform.position.y-1;
		spellStartPoint.z = transform.position.z;
		animator.SetTrigger("wizard_attack");
		Invoke ("createSpellParticle", 1);
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
