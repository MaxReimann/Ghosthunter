using UnityEngine;
using System.Collections;

public class KinematicRockController : MonoBehaviour, IChildDelegator {

	public GameObject collisionVase;
	public GameObject collisionVaseUpperHalf;
	public GameObject timeItemStartPosition;

	private bool collisionHappened = false;
	private Animator animator;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();


	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.Equals (collisionVase) && !collisionHappened) {
			animator.SetTrigger ("hit_vase");
			GetComponent<Rigidbody2D> ().isKinematic = true; //stop body
			collisionHappened  = true;
			Invoke("doDestroy",1.05f);
			Destroy(collisionVase);
			Rigidbody2D flyingVaseRigidBody = collisionVaseUpperHalf.GetComponent<Rigidbody2D>();
			flyingVaseRigidBody.isKinematic = false;
			flyingVaseRigidBody.WakeUp();
			flyingVaseRigidBody.AddForceAtPosition(	new Vector2(-80.5f, 400.0f), new Vector2(0.1f,-0.01f));
			collisionVaseUpperHalf.GetComponent<SpriteRenderer>().enabled = true;
			Invoke("changeUpperVaseLayer", 0.6f);
			Invoke("createTimeItem", 0.9f);

			
		}
	}

	void changeUpperVaseLayer() {
		//make upper half collidable with other kinematic objects
		//do this after some delay after rock collision, to avoid colliding and stopping rock
		collisionVaseUpperHalf.layer = LayerMask.NameToLayer ("KinematicCollider");
		collisionVaseUpperHalf.GetComponent<PolygonCollider2D> ().enabled = true;
	}

	void createTimeItem(){
		GameObject spell = Instantiate (Resources.Load ("AddonTimeItem"), 
		                                timeItemStartPosition.transform.position,
		                                Quaternion.identity) as GameObject;
	}



	void doDestroy(){
		Destroy (this.gameObject);
	}

	//interface methods
	public void childEnterCollision2D (Collision2D coll){
		if (coll.gameObject.CompareTag("Spell"))
			GetComponent<Rigidbody2D> ().isKinematic = false; //activate rigibody rolling
	}


	public void childEnterTrigger2D (Collider2D coll){
	} 
	

}
