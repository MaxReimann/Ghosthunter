using UnityEngine;
using System.Collections;

public class witchController : MonoBehaviour {
	public float speed = 10;
	Transform myTrans;
	Rigidbody2D myBody;
	bool facingLeft = true;
	// Use this for initialization
	void Start () 
	{
		myTrans = this.transform;
		myBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Move (Input.GetAxisRaw ("Horizontal"));

	}

	public void Move(float horizontalInput)
	{
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

}
