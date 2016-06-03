using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour {

	private AudioManager audioManager;

	#if !(UNITY_IPHONE || UNITY_ANDROID)
		private MovieTexture movie;
		private AudioSource audioSource;
	#endif
	
	void Start () {		
		audioManager = AudioManager.GetInstance ();
		#if (UNITY_IPHONE || UNITY_ANDROID)
			playOnMobile();
		#else
			audioSource = GetComponent<AudioSource>();
			movie = (MovieTexture) Resources.Load("OpenScene", typeof(MovieTexture));
			if (movie != null){
				GetComponent<RawImage>().texture = movie;
				audioSource.clip = movie.audioClip;
				if (!movie.isPlaying) {
					movie.Play();
					audioSource.Play();
				}
			}
		#endif
	}
	
	void Update () {
		#if !(UNITY_IPHONE || UNITY_ANDROID)
			if (movie == null || !movie.isPlaying) {
				loadMenu();
			}
		#endif
	}

	public void skipVideo(){
		#if !(UNITY_IPHONE || UNITY_ANDROID)
			Debug.Log("skiping video");
			movie.Stop();
		#endif
	}

	private void loadMenu(){
		Application.LoadLevel("Menu");
		audioManager.playMenuMusic ();
	}


	private void playOnMobile(){
		#if (UNITY_IPHONE || UNITY_ANDROID)
		Debug.Log("playing movie on mobile");
		Handheld.PlayFullScreenMovie("OpenScene.mp4", Color.black , FullScreenMovieControlMode.CancelOnInput, 
		                             FullScreenMovieScalingMode.AspectFill);
		loadMenu();
		#endif
	}
}
