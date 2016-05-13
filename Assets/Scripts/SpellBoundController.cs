using UnityEngine;
using System.Collections;

public class SpellBoundController : MonoBehaviour {
	private Vector2 startPosition;
	
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		BoxCollider2D boxCollider = this.GetComponent<BoxCollider2D>();
		float distance = Mathf.Abs (transform.position.y - startPosition.y);
		boxCollider.size = new Vector2 (boxCollider.size.x, distance);
		boxCollider.offset = new Vector2 (0.0f, -distance / 2.0f);
	}
	
	void OnCollisionEnter2D(Collision2D coll){

		Destroy(transform.parent.gameObject);
		
	}
	
	
	void OnDrawGizmos() {
		BoxCollider2D b = GetComponent<BoxCollider2D>();
		Transform t = GetComponent<Transform>();
		
		
		// Draw BoxColliders
		if (b != null) { 
			Vector3 tl = new Vector3(t.position.x -  (b.size.x / 2) + b.offset.x, t.position.y + (b.size.y / 2) + b.offset.y, 0f);
			Vector3 bl = new Vector3(t.position.x - (b.size.x / 2)+ b.offset.x, t.position.y - (b.size.y / 2) + b.offset.y, 0f);
			Vector3 br = new Vector3(t.position.x + (b.size.x / 2) + b.offset.x, t.position.y - (b.size.y / 2) + b.offset.y, 0f);
			Vector3 tr = new Vector3(t.position.x + (b.size.x / 2) + b.offset.x, t.position.y + (b.size.y / 2) + b.offset.y, 0f);
			Gizmos.color = Color.red;
			Gizmos.DrawLine (tl, bl);
			Gizmos.DrawLine (bl, br);
			Gizmos.DrawLine (br, tr);
			Gizmos.DrawLine (tr, tl);
		}
	}
}
