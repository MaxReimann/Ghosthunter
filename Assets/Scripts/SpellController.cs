using UnityEngine;
using System.Collections;

public class SpellController : MonoBehaviour {

	public enum SpellType {
		Normal,
		Permanent
	};

	private bool isHalted = false;
	private static float haltTime = 5.0f;

	public SpellType spellType; //set in inspector

	// Use this for initialization
	void Start () {
		transform.GetChild(0).GetComponent<Renderer>().sortingLayerName="Spells";
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Destroy()
	{
		Destroy(this.gameObject);
	}

	void HaltSpell() {
		if (isHalted)
			return;

		Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
		rigidBody.velocity = new Vector2 (0, 0);

		Invoke ("Destroy", haltTime);
		
	}

	// called by child script
	public void collision(Collision2D coll) {

		if (coll.gameObject.CompareTag ("Ghost")) {
			Destroy();
			return;
		}
		
		switch (spellType) {
			case SpellType.Normal:
				Destroy();
				break;
			case SpellType.Permanent:
				HaltSpell();
				break;
		}


	}
}
