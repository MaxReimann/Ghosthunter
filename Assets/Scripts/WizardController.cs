using UnityEngine;
using System.Collections;

public class WizardController : MonoBehaviour {

	public float speed = 10;
	private Rigidbody2D myBody;

	// Use this for initialization
	void Start (){
		myBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update (){
		Move (Input.GetAxisRaw ("Horizontal"));
		
		if (Input.GetKeyDown("space")) {
			if(!GameObject.FindGameObjectWithTag("Spell")){
				createSpell();
			}
		}
	}
	
	public void Move(float horizontalInput){
		Vector2 moveVel = myBody.velocity;
		moveVel.x = horizontalInput * speed;
		myBody.velocity = moveVel;
	}
	
	public void createSpell(){
		Vector3 newPos = new Vector3 ();
		newPos.x = transform.position.x+0.9f;
		newPos.y = transform.position.y-0.4f;
		newPos.z = transform.position.z;

		GameObject spell = Instantiate(Resources.Load("Spell"), newPos, Quaternion.identity) as GameObject;
		Rigidbody2D rigidBody = spell.GetComponent<Rigidbody2D>();
		rigidBody.velocity = transform.up * speed;

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		print (coll.gameObject.tag);
		if (coll.gameObject.tag == "Ghost") {
			//TODO decrease life or gameOver
			//Destroy(gameObject);
		}
	}
}
