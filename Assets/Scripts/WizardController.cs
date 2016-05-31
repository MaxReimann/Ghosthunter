using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class WizardController : NetworkBehaviour {

	private static float SPELL_DELAY = 0.01f;

	public float speed = 5;
	public float spellSpeed = 8;
	private Rigidbody2D myBody;
	private Vector3 spellStartPoint; 

	private GameManager gameManager;	

	[SyncVar(hook="FlipIfNeeded")] //will synchronize only server -> client, calls this function on change
	public bool isLeft = false;


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
	void Update ()
	{
		if (!isLocalPlayer)
			return;
	

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
		Instantiate(Resources.Load("Shield"), this.transform.position, Quaternion.identity); 
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
		if (!isLocalPlayer)
			return;
		animator.SetTrigger("wizard_run");
		Move(-1);
		
		if (isServer)
			FlipIfNeeded (isLeft);
		
		if (!isLeft) {
			if (isClient)
				CmdFlip (); //execute this on the server 
			isLeft = true;
		}
	}


	private void MoveRight(){
		if (!isLocalPlayer)
			return;
		animator.SetTrigger("wizard_run");
		Move(1);

		if (isServer)
			FlipIfNeeded (isLeft);

		if (isLeft) {
			if (isClient)
				CmdFlip (); //execute this on the server 
			isLeft = false;
		}
	}


	
	public void Idle(){
		animator.SetTrigger("wizard_idle");
		Move(0);
	}

	[Command]
	public void CmdFlip()
	{
		isLeft = !isLeft;
		FlipIfNeeded (isLeft);
	}

	public void FlipIfNeeded(bool left) {
		if (left)
			transform.localScale = new Vector3( - Mathf.Abs( transform.localScale.x),
			                                   transform.localScale.y,
			                                   transform.localScale.z); //flip image
		else 
			transform.localScale = new Vector3( Mathf.Abs( transform.localScale.x),
			                                   transform.localScale.y,
			                                   transform.localScale.z); //flip image
		
	}
	
	public void Spell(){
		if(GameObject.FindGameObjectWithTag("Spell")){
			return;
		}
		animator.SetTrigger("wizard_attack");
		Invoke("createSpell", SPELL_DELAY);
	}

	private void createSpell(){
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();

		if (!isLocalPlayer)
			return;
		
		CmdCreateSpellParticle ();
	}

	[Command] //executed on server
	private void CmdCreateSpellParticle()
	{
		string prefab = "NormalSpell";
		switch (this.spellType) {
		case SpellController.SpellType.Normal:
			prefab = "NormalSpell"; break;
		case SpellController.SpellType.Permanent:
			prefab = "PermanentSpell"; break;
		}

		spellStartPoint = new Vector3 ();
		if (isLeft) {
			spellStartPoint.x = transform.position.x-0.5f;
		} else {
			spellStartPoint.x = transform.position.x+0.5f;
		}
		spellStartPoint.y = transform.position.y-1;
		spellStartPoint.z = transform.position.z;
		
		GameObject spell = Instantiate(Resources.Load(prefab), spellStartPoint, Quaternion.identity) as GameObject;
		
		Rigidbody2D rigidBody = spell.GetComponent<Rigidbody2D>();
		rigidBody.velocity = transform.up * spellSpeed;

		NetworkServer.Spawn (spell); //spawn on clients
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

	void OnCollisionEnter2D(Collision2D coll){
		if (!shieldActive) {
			if(coll.gameObject.tag == "Ghost" || coll.gameObject.tag == "LethalItem" || coll.gameObject.tag == "Zombie"){
				gameManager.decreaseLive();
			}		
		}
	}
}
