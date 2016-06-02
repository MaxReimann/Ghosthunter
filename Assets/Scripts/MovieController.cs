﻿#if !(UNITY_IPHONE || UNITY_ANDROID)
	using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovieController : MonoBehaviour {

	#if !(UNITY_IPHONE || UNITY_ANDROID)
		private MovieTexture movie;
		private AudioSource audioSource;
	#endif

	//private LevelLoader levelLoader;

	// Use this for initialization
	void Start () {		

		//levelLoader = GetComponent<LevelLoader> ();

		#if (UNITY_IPHONE || UNITY_ANDROID)
			playOnMobile();
		#else
			audioSource = GetComponent<AudioSource>();
			//movie = (MovieTexture) Resources.Load("OpenScene", typeof(MovieTexture));
			movie = (MovieTexture) AssetDatabase.LoadAssetAtPath("Assets/Movies/OpenScene.mov", typeof(MovieTexture));

			GetComponent<RawImage>().texture = movie;
			audioSource.clip = movie.audioClip;
			if (!movie.isPlaying) {
				Debug.Log("playing movie on desktop");
				movie.Play();
				audioSource.Play();
			}
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		#if !(UNITY_IPHONE || UNITY_ANDROID)
			if (!movie.isPlaying) {
				//levelLoader.LoadMainMenu();
				Application.LoadLevel("Menu");
			}
		#endif
	}


	private void playOnMobile(){
		Debug.Log("playing movie on mobile");
		Handheld.PlayFullScreenMovie("OpenScene.mp4", Color.black , FullScreenMovieControlMode.CancelOnInput, 
		                             FullScreenMovieScalingMode.AspectFill);
		//levelLoader.LoadMainMenu();
		Application.LoadLevel("Menu");
	}
}
