﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovieController : MonoBehaviour {
	
	public AudioSource audioSource;
	private MovieTexture movie;

	// Use this for initialization
	void Start () {		

		#if (UNITY_IPHONE || UNITY_ANDROID)
			playOnMobile();
			return;
		#endif

		movie = GetComponent<RawImage>().mainTexture as MovieTexture;
		audioSource.clip = movie.audioClip;
		if (!movie.isPlaying) {
			movie.Play();
			audioSource.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {

		#if (UNITY_IPHONE || UNITY_ANDROID)
			return;
		#endif

		if (!movie.isPlaying) {
			Application.LoadLevel("Menu");
		}
	}

	private void playOnMobile(){
		Handheld.PlayFullScreenMovie ("OpenScene.mov", Color.black, FullScreenMovieControlMode.Hidden,
		                              FullScreenMovieScalingMode.AspectFill);
		Application.LoadLevel ("Menu");
	}
}
