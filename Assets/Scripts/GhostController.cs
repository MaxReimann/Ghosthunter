using UnityEngine;
using System.Collections;


public class GhostController : MonoBehaviour {

	public float velX = 1.9f;//horizontal speed of ball
	public bool startOppositeDirection = false;
	[HideInInspector] public Vector2 inVel;//incoming velocity
	private float startY;//max jump height (every time ball hits floor it will calculate force needed to reach this height).
	private Rigidbody2D rigidBody;
	private BoxCollider2D leftBorder;
	private BoxCollider2D rightBorder;
	private Ghost ghostType;
	private GameManager gameManager;
	// additional height gain directly after splitting ghosts
	private float splitYGain = 1.5f;

	// Use this for initialization
	void Start () {
		this.rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.velocity = new Vector2( - velX * (startOppositeDirection? 1f : -1f), inVel.y);

		GameObject leftBorderObject = GameObject.FindWithTag ("LeftBorder");
		leftBorder = (BoxCollider2D) leftBorderObject.GetComponent<Collider2D>();

		GameObject rightBorderObject = GameObject.FindWithTag ("RightBorder");
		rightBorder = (BoxCollider2D) leftBorderObject.GetComponent<Collider2D>();  

		string cleanedName = this.name.Replace ("(Clone)", "");
		ghostType = GhostTypes.getType (cleanedName);
		startY = ghostType.bounceHeight;

		gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		inVel = rigidBody.velocity;
	}

	public void spellCollision() {
		Destroy (this.gameObject);

		// add offset only if not to close to the walls
		Vector2 start = transform.position;
		float leftOffset = start.x - 1 > leftBorder.bounds.center.x + leftBorder.bounds.extents.x ? -1f : 0f;
		float rightOffset = start.x + 1 < rightBorder.bounds.center.x - leftBorder.bounds.extents.x ? 1f : 0f;

		string nextType = ghostType.splitInto;
		if (nextType == "None")
			return;

		// create left and right ghost from prefabs
		GameObject leftGhost = Instantiate(Resources.Load(nextType),start + new Vector2(leftOffset,0), Quaternion.identity) as GameObject;
		GameObject rightGhost = Instantiate(Resources.Load(nextType), start + new Vector2(rightOffset,0), Quaternion.identity) as GameObject;
		GhostController leftGhostController = leftGhost.GetComponent<GhostController> ();
		GhostController rightGhostController = rightGhost.GetComponent<GhostController> ();

		// boost temporarly to heigher y position
		float newYVel = Mathf.Sqrt(2 * -splitYGain * Physics2D.gravity.y * rigidBody.gravityScale);
		leftGhostController.inVel = new Vector2 (0,	newYVel);
		rightGhostController.inVel = new Vector2 (0, newYVel);

		leftGhost.GetComponent<GhostController> ().startOppositeDirection = true;

		CircleCollider2D leftGhostCollider = leftGhost.GetComponent<CircleCollider2D> ();
		if (leftGhostCollider.IsTouching (leftBorder)) {
			print ("Warning: Left ghost touching wall)");
			leftGhost.GetComponent<Rigidbody2D> ().MovePosition (start + new Vector2 (1, 0));
		}

		CircleCollider2D rightGhostCollider = rightGhost.GetComponent<CircleCollider2D> ();
		if (rightGhostCollider.IsTouching (rightBorder)) {
			print ("Warning: Right ghost touching wall)");
			rightGhost.GetComponent<Rigidbody2D> ().MovePosition (start - new Vector2 (1, 0));
		}

		gameManager.addScore (2);
	}
	


	//using code from http://answers.unity3d.com/questions/670204/simple-ball-bounce-like-pangbubble-trouble.html
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Spell") {
			spellCollision();
		}
		
		ContactPoint2D hit = coll.contacts[0]; //(for debug only) the first contact is enough
		Vector3 outVel = Vector3.Reflect(inVel, hit.normal);
		if (hit.normal.x < 0)
			rigidBody.velocity = new Vector2 (- velX, inVel.y);
		else if (hit.normal.x > 0)
			rigidBody.velocity = new Vector2 (velX, inVel.y);
		else {
			float sign = Mathf.Abs(inVel.x) > 0.0f ? inVel.x / Mathf.Abs (inVel.x) : 0.0f;
			rigidBody.velocity = new Vector2 (velX * sign, rigidBody.velocity.y);
		}
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
			if (newYVel == 0) 
				newYVel = 1f;
			rigidBody.velocity = new Vector2( rigidBody.velocity.x, newYVel);
		}
		//save now, because sometimes collision happens before next FixedUpdate tick
//		inVel = this.rigidBody.velocity;
	}
}

