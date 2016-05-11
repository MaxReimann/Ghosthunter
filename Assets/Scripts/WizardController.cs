using UnityEngine;
using UnityEditor;
using System.Collections;

public class WizardController : MonoBehaviour {

	public float speed = 10;
	public Rigidbody2D spell;

	private Rigidbody2D myBody;

	// Use this for initialization
	void Start (){
		myBody = GetComponent<Rigidbody2D>();
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
	}
	
	public void createSpell(){
		Vector3 newPos = new Vector3 ();
		newPos.x = transform.position.x+0.9f;
		newPos.y = transform.position.y;
		newPos.z = transform.position.z;

		Rigidbody2D spellClone = (Rigidbody2D) Instantiate(spell, newPos, transform.rotation);
		spellClone.velocity = transform.up * speed;

//		Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Sprites/Spell.prefab", typeof(GameObject));
//		Rigidbody2D spell = Instantiate(prefab, newPos, transform.rotation) as Rigidbody2D;
//		spell.velocity = transform.up * speed;
	}
}
