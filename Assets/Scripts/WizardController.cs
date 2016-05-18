using UnityEngine;
using System.Collections;

public class WizardController : MonoBehaviour {

	public float speed = 10;
	public float spellSpeed = 10;
	private Rigidbody2D myBody;
	private Transform spellStartPoint; 

	private GameManager gameManager;

	// Use this for initialization
	void Start (){
		myBody = GetComponent<Rigidbody2D>();
		spellStartPoint = transform.Find("spellStartPoint");
		gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update (){
		Move (Input.GetAxisRaw ("Horizontal"));
		
		if (Input.GetKeyDown("space")) {
			createSpell();
		}
	}

	public void createSpell(){
		if(GameObject.FindGameObjectWithTag("Spell")){
			return;
		}
		Vector3 newPos = new Vector3 ();
		newPos = spellStartPoint.position;
		
		GameObject spell = Instantiate(Resources.Load("Spell"), newPos, Quaternion.identity) as GameObject;
		
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
		print (coll.gameObject.tag);
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
