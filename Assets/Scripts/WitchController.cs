using UnityEngine;
using System.Collections;

public class WitchController : MonoBehaviour {
	public float speed = 10;
	Transform myTrans;
	Rigidbody2D myBody;
	bool facingLeft = true;
	private Transform firePoint;
	public Rigidbody2D bullet;
	// Use this for initialization
	void Start () 
	{
		myTrans = this.transform;
		myBody = GetComponent<Rigidbody2D>();
		GameObject emitterpoint = this.transform.FindChild("emitterpoint").gameObject;
		firePoint = emitterpoint.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Move (Input.GetAxisRaw ("Horizontal"));

		if (Input.GetKeyDown("space")) {
			FireBullet();
		}

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

	public void FireBullet()
	{
		Rigidbody2D bulletClone = (Rigidbody2D) Instantiate(bullet, firePoint.position, firePoint.rotation);
		bulletClone.velocity = transform.up * speed;
	}

}
