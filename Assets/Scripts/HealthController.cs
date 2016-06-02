using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	private GameManager manager;
	private GameObject[] hearts;

	private int cachedLives = -1;

	// Use this for initialization
	void Start () {
		manager = GameManager.GetInstance();
		hearts = new GameObject[manager.getTotalLives()];
		for(int i=0; i<hearts.Length; i++){
			float x = 8.2f - i*0.75f;
			GameObject heart = Instantiate(Resources.Load("Heart"), new Vector2(x,4.3f), Quaternion.identity) as GameObject;
			hearts[i] = heart;
		};
	}
	
	// Update is called once per frame
	void Update () {
		if (cachedLives != manager.getCurrentLives ()) {
			cachedLives = manager.getCurrentLives();
			redrawHearts();
		}	
	}

	private void redrawHearts() {
		for (int i = hearts.Length-1; i>=cachedLives; i--) {
			GameObject heart = hearts [i];
			SpriteRenderer renderer = heart.GetComponent<SpriteRenderer> ();
			Color color = renderer.color;
			color.a = 0.6f;
			renderer.color = color;
		}
	}
}
