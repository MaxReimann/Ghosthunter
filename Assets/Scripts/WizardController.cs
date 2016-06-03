using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System.Collections;

public class WizardController : NetworkBehaviour {

	private static float SPELL_DELAY = 0.01f;
	private static float HIT_TIME = 1f;
	private static float TOGGLE_TIME = 0.1f;

	private float nonCollisionTimer = 0.0f;
	private float toggleTimer = 0.0f;
	private float shieldToggleTimer = 0.0f;

	public float speed = 5;
	public float spellSpeed = 8;
	private Rigidbody2D myBody;
	private Vector3 spellStartPoint; 

	private GameManager gameManager;	

	private GameObject shield;

	private SpriteRenderer spriteRenderer;

	[SyncVar(hook="FlipIfNeeded")] //will synchronize only server -> client, calls this function on change
	public bool isLeft = false;


	private float spellExecution;
	private float buttonInput;

	private Animator animator;

	private SpellController.SpellType spellType = SpellController.SpellType.Normal;
	[SyncVar]
	private bool shieldBlinking = false;
	[SyncVar]
	private bool isHit = false;


	// Use this for initialization
	void Start (){
//		DontDestroyOnLoad(this.gameObject);
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		gameManager = GameManager.GetInstance();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		if (gameManager.getCurrentLevel() == "Menu")
			return;

		//register wizard to touch buttons
		if (isLocalPlayer) {
			GameObject leftButtonObject = GameObject.FindGameObjectWithTag ("LeftButton");
			GameObject rightButtonObject = GameObject.FindGameObjectWithTag ("RightButton");

			leftButtonObject.GetComponent<CustomEventTrigger> ().RegisterWizard (this);
			rightButtonObject.GetComponent<CustomEventTrigger> ().RegisterWizard (this);

			GameObject touchFieldObject = GameObject.FindGameObjectWithTag ("TouchField");
			touchFieldObject.GetComponent<Button> ().onClick.AddListener (() => Spell ());
		}else{
			//colour the network player in green
			spriteRenderer.color = Color.green;
		}


		print("playerId:" + GetComponent<NetworkIdentity>().playerControllerId);
	}

	private void toggleVisibility(){		
		if (toggleTimer > 0) {
			toggleTimer -= Time.deltaTime;
			return;
		}
		spriteRenderer.enabled = !spriteRenderer.isVisible;
		toggleTimer = TOGGLE_TIME;
	}

	private void toggleShieldVisibility(){	
		if (shieldToggleTimer > 0) {
			shieldToggleTimer -= Time.deltaTime;
			return;
		}
		SpriteRenderer spriteRenderer = shield.GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = !spriteRenderer.isVisible;
		shieldToggleTimer = TOGGLE_TIME;
	}

	// Update is called once per frame
	void Update ()
	{
	//	print (nonCollisionTimer);
	
		if (nonCollisionTimer > 0.0f) {
			nonCollisionTimer -= Time.deltaTime;
			toggleVisibility();
		} else {
			nonCollisionTimer = 0.0f;
			isHit=false;
			spriteRenderer.enabled=true;
		}

		if(shield != null && shieldBlinking){
			toggleShieldVisibility();
		}

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

	[Server]
	public void ActivateShield(float time){
		shield = Instantiate(Resources.Load("Shield"), this.transform.position, Quaternion.identity) as GameObject; 
		ShieldController shieldController = shield.GetComponent<ShieldController> ();
		shieldController.wizard = this.gameObject;
		shieldController.wizardNetId = this.netId;
		NetworkServer.Spawn (shield);
		Invoke ("blinkShield", time-1);
	}


	private void blinkShield(){
		shieldBlinking = true;
		Invoke ("DeactivateShield", 1f);
	}

	public void DeactivateShield(){
		shieldBlinking = false;
		Destroy (shield);
		shield = null;
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
		GameObject[] foundSpells = GameObject.FindGameObjectsWithTag ("Spell");
		if ((gameManager.IsMultiplayer() && foundSpells.Length >= 2) ||
		    (!gameManager.IsMultiplayer() && foundSpells.Length >= 1))
		    return;





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
		spellStartPoint.y = transform.position.y-0.8f;
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

	[ClientRpc]
	private void RpcsetNonCollisionTimer(float time){
		nonCollisionTimer = time;
	}


	void OnCollisionEnter2D(Collision2D coll){
		if (!isServer)
			return;
		if (shield == null && !isHit) {
			if(coll.gameObject.tag == "Ghost" || coll.gameObject.tag == "LethalItem" || coll.gameObject.tag == "Zombie"){
				isHit = true;
				nonCollisionTimer = HIT_TIME;
				RpcsetNonCollisionTimer(HIT_TIME);
				gameManager.decreaseLive();
			}		
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (!isServer)
			return;

		if (shield == null && !isHit) {
			if(coll.gameObject.tag == "Ghost" || coll.gameObject.tag == "LethalItem" || coll.gameObject.tag == "Zombie"){
				doHit();
			}		
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "Platform") {
			transform.parent = other.transform;
		} else {
			OnTriggerEnter2D (other);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		Debug.Log (other.gameObject.name);
		if (other.gameObject.tag == "Platform") {
			transform.parent = null;
		}
	}

	private void doHit(){
		isHit = true;
		nonCollisionTimer = HIT_TIME;
		RpcsetNonCollisionTimer(HIT_TIME);
		gameManager.decreaseLive();
	}
}
