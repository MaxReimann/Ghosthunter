using UnityEngine;
using System.Collections;

public class WitchController : MonoBehaviour {
	
	public float speed = 10;
	public Rigidbody2D spell;

	private Transform myTrans;
	private Rigidbody2D myBody;
	private bool facingLeft = true;
	private Transform firePoint;

	// Use this for initialization
	void Start (){
		myTrans = this.transform;
		myBody = GetComponent<Rigidbody2D>();
		GameObject emitterpoint = this.transform.FindChild("emitterpoint").gameObject;
		firePoint = emitterpoint.transform;
	}
	
	// Update is called once per frame
	void Update (){
		Move (Input.GetAxisRaw ("Horizontal"));

		if (Input.GetKeyDown("space")) {
			createSpell();
		}

	}

	public void Move(float horizontalInput){
		Vector2 moveVel = myBody.velocity;
		moveVel.x = horizontalInput * speed;
		myBody.velocity = moveVel;

		if (Mathf.Abs(moveVel.x) > 0.0) {
			if (moveVel.x > 0) {
				facingLeft = false;
				myTrans.localRotation = Quaternion.Euler (0, 180, 0);
			} else {
				facingLeft = true;
				myTrans.localRotation = Quaternion.Euler (0, 0, 0);
			}
		}
	}

	public void createSpell(){
		Rigidbody2D spellClone = (Rigidbody2D) Instantiate(spell, firePoint.position, firePoint.rotation);
		spellClone.velocity = transform.up * speed;
	}

}
