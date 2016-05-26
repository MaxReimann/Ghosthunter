using UnityEngine;
using System.Collections;

public class WizardController : MonoBehaviour {

	private static float SPELL_DELAY = 0.3f;

	public float speed = 10;
	public float spellSpeed = 10;
	private Rigidbody2D myBody;
	private Vector3 spellStartPoint; 

	private GameManager gameManager;

	private bool isLeft = false;
	private float spellExecution;
	private float buttonInput;

	private Animator animator;

	private SpellController.SpellType spellType = SpellController.SpellType.Normal;
	private bool shieldActive = false;

	// Use this for initialization
	void Start (){
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		gameManager = GameManager.GetInstance();
	}

	// Update is called once per frame
	void Update (){

		if (spellExecution > 0 && spellExecution + SPELL_DELAY < Time.time) {
			createSpellParticle ();
			spellExecution = 0;
		}

		if (Input.GetKeyDown ("space")) {
			Spell ();
			return;
		}

		if (buttonInput != 0) {
			if(buttonInput<0){
				MoveLeft();
			}else{
				MoveRight();
			}
		
		} else {
			//check keyboard input
			float horizontalInput = Input.GetAxisRaw ("Horizontal");
			if (horizontalInput < 0) {
				MoveLeft ();
			} else if (horizontalInput > 0) {
				MoveRight ();
			} else {
				Idle ();
			}
		}

	}

	public void SetSpellType(SpellController.SpellType type){
		this.spellType = type;
	}

	public void ActivateShield(float time){
		GameObject shield = Instantiate(Resources.Load("Shield"), this.transform.position, Quaternion.identity) as GameObject; 
		shieldActive = true;

		Invoke ("DeactivateShield", time);
	}

	public void DeactivateShield(){
		shieldActive = false;
		Destroy (GameObject.FindGameObjectWithTag ("Shield"));
	}


	public void LeftPressed(){
		buttonInput = -1;
	}

	public void RightPressed(){
		buttonInput = 1;

	}

	public void Released(){
		buttonInput = 0;
	}

	private void MoveLeft(){;
		animator.SetTrigger("wizard_run_left");
		Move(-1);
		isLeft = true;
	}

	private void MoveRight(){
		animator.SetTrigger("wizard_run_right");
		Move(1);
		isLeft = false;
	}
	
	public void Idle(){
		animator.SetTrigger("wizard_idle");
		Move(0);
	}
	
	public void Spell(){
		if(GameObject.FindGameObjectWithTag("Spell")){
			return;
		}

		animator.SetTrigger("wizard_attack");

		spellStartPoint = new Vector3 ();
		if (isLeft) {
			spellStartPoint.x = transform.position.x-0.5f;
		} else {
			spellStartPoint.x = transform.position.x+0.5f;
		}
		spellStartPoint.y = transform.position.y-1;
		spellStartPoint.z = transform.position.z;

		spellExecution = Time.time;
	}

	private void createSpellParticle(){
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
		
		string prefab = "NormalSpell";
		switch (this.spellType) {
			case SpellController.SpellType.Normal:
				prefab = "NormalSpell"; break;
			case SpellController.SpellType.Permanent:
				prefab = "PermanentSpell"; break;
		}

		GameObject spell = Instantiate(Resources.Load(prefab), spellStartPoint, Quaternion.identity) as GameObject;

		Rigidbody2D rigidBody = spell.GetComponent<Rigidbody2D>();
		rigidBody.velocity = transform.up * spellSpeed;
	}
	
	private void Move(float horizontalInput){
		Vector2 moveVel = myBody.velocity;
		moveVel.x = horizontalInput * speed;
		myBody.velocity = moveVel;
	}

	public void setToPosition(Vector2 pos)
	{
		this.transform.position = pos;
		Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
		rigidBody.position = pos;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Ghost" && !shieldActive) {
				Destroy(gameObject);
				gameManager.gameOver();
		}
	}
}
