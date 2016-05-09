using UnityEngine;
using System.Collections;


public class BallController : MonoBehaviour {
	private float velX = 1.9f;//horizontal speed of ball
	private Vector2 inVel;//incoming velocity
	private float startY;//max jump height (every time ball hits floor it will calculate force needed to reach this height).
	private Rigidbody2D rigidBody;
	// Use this for initialization
	void Start () {
		this.rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		inVel = rigidBody.velocity;
	}

	//using code from http://answers.unity3d.com/questions/670204/simple-ball-bounce-like-pangbubble-trouble.html
	void OnCollisionEnter2D(Collision2D coll)
	{
		print ("enter");
		ContactPoint2D hit = coll.contacts[0]; //(for debug only) the first contact is enough
		Vector3 outVel = Vector3.Reflect(inVel, hit.normal);
		if(hit.normal.x < 0)
			rigidBody.velocity = new Vector2( - velX, inVel.y);
		else if(hit.normal.x > 0)
			rigidBody.velocity = new Vector2( velX, inVel.y);
		else
			rigidBody.velocity = new Vector2( velX*(inVel.x/Mathf.Abs(inVel.x)), rigidBody.velocity.y);
		if(hit.normal.y < 0)
		{
			if (Mathf.Abs(inVel.y) < 1)
				rigidBody.velocity = new Vector2( rigidBody.velocity.x, -1);
			else
				rigidBody.velocity = new Vector2( rigidBody.velocity.x, -Mathf.Abs(inVel.y));
		} else if(hit.normal.y > 0)
		{     //jumping up, calculate how much force is needed to jump to certain height (startY)
			float relPos = transform.position.y - startY;
			if(relPos > 0f)
				relPos = 0f;
			float newYVel = Mathf.Sqrt(2 * relPos * Physics2D.gravity.y * rigidBody.gravityScale);
			if (newYVel == 0) newYVel = 1f;
			rigidBody.velocity = new Vector2( rigidBody.velocity.x, newYVel);
		}
		//save now, because sometimes collision happens before next FixedUpdate tick
		inVel = this.rigidBody.velocity;
	}
}
