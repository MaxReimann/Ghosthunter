﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovieController : MonoBehaviour {
	
	public AudioSource audioSource;
	private MovieTexture movie;

	// Use this for initialization
	void Start () {		
		movie = GetComponent<RawImage>().mainTexture as MovieTexture;
		audioSource.clip = movie.audioClip;
		if (!movie.isPlaying) {
			movie.Play();
			audioSource.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!movie.isPlaying) {
			Application.LoadLevel("Menu");
		}
	}
}