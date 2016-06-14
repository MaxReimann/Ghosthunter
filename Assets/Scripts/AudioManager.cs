using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	private static AudioManager instance;

	private AudioSource source;
	private AudioClip gameSound;
	private AudioClip menuSound;
	private AudioClip gameOverSound;
	private AudioClip winSound;


	public static AudioManager GetInstance(){
		if(instance == null){
			//will make new gamemanager object
			Instantiate(Resources.Load("AudioManager"), new Vector3(0,0,0), Quaternion.identity); 
			if (instance == null){
				print("not awake");
				instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
			}
		}
		return instance;
	}
	
	public void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
			source = GetComponent<AudioSource> ();
			gameSound = (AudioClip) Resources.Load ("ghostrag");
			menuSound = (AudioClip) Resources.Load ("menu");
			gameOverSound = (AudioClip) Resources.Load ("gameover");
			winSound = (AudioClip) Resources.Load ("boss");
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public void stop(){
		if (source.isPlaying) {
			source.Stop();
		}
		source.volume = 1f;
		source.loop = false;
	}

	public void playGameMusic(){
		stop ();
		source.clip = gameSound;
		source.loop = true;
		source.Play ();
	}

	public void continueGameMusic(){
		if (source.isPlaying && source.clip.Equals(gameSound))
			return;
		stop ();
		source.clip = gameSound;
		source.loop = true;
		source.Play ();
	}

	public void playMenuMusic(){
		stop ();
		source.clip = menuSound;
		source.volume = 0.25f;
		source.loop = true;
		source.Play ();
	}

	public void playGameOverMusic(){
		stop ();
		source.clip = gameOverSound;
		source.loop = true;
		source.Play ();
	}

	public void playWinMusic(){
		stop ();
		source.clip = winSound;
		source.loop = true;
		source.Play ();
	}
}
