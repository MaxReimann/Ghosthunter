using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class GhostController : NetworkBehaviour {

	public float velX = 1.9f;//horizontal speed of ball
	[SyncVar]
	public bool startOppositeDirection = false;
	[SyncVar]
	[HideInInspector] public Vector2 inVel;//incoming velocity
	[SyncVar]
	public float nonCollisionTimer = 0.0f;
	[SyncVar]
	private bool unreactiveTimerFinished = false;

	[SyncVar]
	[HideInInspector] public bool isHitL1; // L1ghost, hit but not yet destroyed


	private float startY;//max jump height (every time ball hits floor it will calculate force needed to reach this height).
	private Rigidbody2D rigidBody;
	private BoxCollider2D leftBorder;
	private BoxCollider2D rightBorder;
	private Ghost ghostType;
	private GameManager gameManager;
	// additional height gain directly after splitting ghosts
	private float splitYGain = 1.5f;
	// time to be unreactive on collisions with player character after split
	private const float unreactiveTime = 0.3f; 


	private Animator animator;
	

	//last positions of the ghost, used to track and free stuck ghosts
	private FixedSizedQueue<Vector3> lastPositions;
	private const int positionQueueCapacity = 10;

	// Use this for initialization
	void Start () {
		this.rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.velocity = new Vector2( - velX * (startOppositeDirection? 1f : -1f), inVel.y);

		GameObject leftBorderObject = GameObject.FindWithTag ("LeftBorder");
		leftBorder = (BoxCollider2D) leftBorderObject.GetComponent<Collider2D>();

		GameObject rightBorderObject = GameObject.FindWithTag ("RightBorder");
		rightBorder = (BoxCollider2D) rightBorderObject.GetComponent<Collider2D>();  

		//string cleanedName = this.name.Replace ("(Clone)", "");
		int stopIndex = this.name.IndexOf ("(");
		if (stopIndex == -1)
			stopIndex = this.name.Length;
		string cleanedName = this.name.Substring (0, stopIndex);
		cleanedName = cleanedName.Trim ();

 		ghostType = GhostTypes.getType (cleanedName);
		startY = ghostType.bounceHeight;

		gameManager = GameManager.GetInstance();

		animator = GetComponent<Animator> ();
		isHitL1 = false;

		beNonReactive (unreactiveTime);
		//queue with capacity, old elements are pushed out
		lastPositions = new FixedSizedQueue<Vector3> (positionQueueCapacity);
	}
	
	// Update is called once per frame
	void Update () {


		inVel = rigidBody.velocity;

		if (nonCollisionTimer > 0.0f) {
			nonCollisionTimer -= Time.deltaTime;
		} else if (!unreactiveTimerFinished) {
			nonCollisionTimer = 0.0f;
			unreactiveTimerFinished = true;
			//GameObject wizard = GameObject.FindGameObjectWithTag ("Wizards");
			//activate collisions
			this.gameObject.layer = LayerMask.NameToLayer("Ghosts");
		}
		lastPositions.Enqueue (this.transform.position);
		//produces very weird behaviour with network: seems to be  stuck in sky
		if (isServer && stuckCheck ()) {
			float sign = transform.position.x > 0.0f ? -1.0f : 1.0f;
			rigidBody.velocity = new Vector2 (velX * sign, velX * 4);
		}

		checkOutsideBorder ();
	}


	private void checkOutsideBorder() {
		//specifies outside borders
		float leftBorderX = leftBorder.bounds.center.x - leftBorder.bounds.extents.x;
		float rightBorderX = rightBorder.bounds.center.x + leftBorder.bounds.extents.x;


		// reset to inside border if ghost is far outside
		if (this.transform.position.x < leftBorderX)
			this.transform.position = new Vector3 (leftBorder.bounds.center.x + leftBorder.bounds.extents.x + 1,
			                                      transform.position.y, 0);
		if (this.transform.position.x > rightBorderX)
			this.transform.position = new Vector3 (rightBorder.bounds.center.x - leftBorder.bounds.extents.x - 1,
			                                      transform.position.y, 0);
	}

	private void beNonReactive(float time)
	{
		nonCollisionTimer = time;
		//ignore wizard collision until timer is run down
		this.gameObject.layer = LayerMask.NameToLayer("NonCollGhosts");
	}

	public bool nonColliding() {
		return nonCollisionTimer > 0.0f;
	}

	private void spellCollision() {
		//set to unreactiveness until death
		beNonReactive (10.0f);


		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();


		if (ghostType.name == "L1Ghost" || ghostType.name == "L1GhostHigh") {
			//TODO ghost vanish animation
			isHitL1 = true;
			animator.SetTrigger ("ghost_disappear");
			Invoke ("doSpellCollision", 0.5f);
		} else {
			animator.SetTrigger ("ghost_split");
			Invoke ("doSpellCollision", 0.5f);
		}
		//check before destruction, to avoid games getting timed out, although all existing ghosts  hit
		gameManager.checkGameFinished();
	}

	[Command]
	private void CmdDoSpellCollision(){
		gameManager.addScore (2);
		NetworkServer.Destroy (this.gameObject);
	//	Destroy (this.gameObject);
		
		string nextType = ghostType.splitInto; 
		if (nextType != "None") {
			Object resource = Resources.Load (nextType);
			createNewGhosts(resource);
		}
	}

	private void doSpellCollision(){
		CmdDoSpellCollision ();
	}

	private void createNewGhosts(Object originalResource){

		// add offset only if not to close to the walls
		Vector2 start = transform.position;
		float leftOffset = start.x - 1 > leftBorder.bounds.center.x + leftBorder.bounds.extents.x ? -1f : 0f;
		float rightOffset = start.x + 1 < rightBorder.bounds.center.x - leftBorder.bounds.extents.x ? 1f : 0f;

		// create left and right ghost from prefabs
		GameObject leftGhost = Instantiate (originalResource, start + new Vector2 (leftOffset, 0), Quaternion.identity) as GameObject;
		GameObject rightGhost = Instantiate (originalResource, start + new Vector2 (rightOffset, 0), Quaternion.identity) as GameObject;
		GhostController leftGhostController = leftGhost.GetComponent<GhostController> ();
		GhostController rightGhostController = rightGhost.GetComponent<GhostController> ();
		NetworkServer.Spawn (leftGhost); //spawn on clients
		NetworkServer.Spawn (rightGhost);

		// boost temporarly to heigher y position
		float newYVel = Mathf.Sqrt (2 * -splitYGain * Physics2D.gravity.y * rigidBody.gravityScale);
		leftGhostController.inVel = new Vector2 (0, newYVel);
		rightGhostController.inVel = new Vector2 (0, newYVel);
		
		leftGhost.GetComponent<GhostController> ().startOppositeDirection = true;



		PolygonCollider2D leftGhostCollider = leftGhost.GetComponent<PolygonCollider2D> ();
		if (leftGhostCollider.IsTouching (leftBorder)) {
			print ("Warning: Left ghost touching wall)");
			leftGhost.GetComponent<Rigidbody2D> ().MovePosition (start + new Vector2 (1, 0));
		}
		
		PolygonCollider2D rightGhostCollider = rightGhost.GetComponent<PolygonCollider2D> ();
		if (rightGhostCollider.IsTouching (rightBorder)) {
			print ("Warning: Right ghost touching wall)");
			rightGhost.GetComponent<Rigidbody2D> ().MovePosition (start - new Vector2 (1, 0));
		}
	}


	bool stuckCheck()
	{
		if (lastPositions.Count < positionQueueCapacity)
			return false;

		const float epsilon = 0.1f;
		Vector3 first = new Vector3(0,0,0);
		int count = 0;
		foreach (Vector3 position in lastPositions) {
			if (count++ == 0) {
				first = position;
				continue;
			}

			if (Mathf.Abs ((first - position).magnitude) >= epsilon)
				return false;
		}

		//all position delta smaller than epsilon -> stuck
		return true;
	}

	

	//using code from http://answers.unity3d.com/questions/670204/simple-ball-bounce-like-pangbubble-trouble.html
	void OnCollisionEnter2D(Collision2D coll)
	{

		if (coll.gameObject.tag == "Zombie") {
			Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), coll.collider);
			return;
		}

		if (coll.gameObject.tag == "Spell") {
			spellCollision();
			return;
		}

		animator.SetTrigger ("ghost_bounce");

		ContactPoint2D hit = coll.contacts[0]; //(for debug only) the first contact is enough
		//Vector3 outVel = Vector3.Reflect(inVel, hit.normal);
		if (hit.normal.x < 0) {
			rigidBody.velocity = new Vector2 (- velX, inVel.y);
		} else if (hit.normal.x > 0) {
			rigidBody.velocity = new Vector2 (velX, inVel.y);
		}
		else {

			float sign;
			if (Mathf.Abs(inVel.x) > 0.0f)
				sign = inVel.x / Mathf.Abs (inVel.x);
			else 
				 sign = (Mathf.Round( Random.Range(0.0f,1.0f) ) - 0.5f) * 2.0f; 
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
		
	}
}


