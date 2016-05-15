using UnityEngine;
using System.Collections;

public class Ghost {

	public string name;
	public float bounceHeight;
	public string splitInto; //type to split into, or None
	public int score;

	public Ghost(string name, float bounceHeight, string splitInto)
	{
		this.name = name;
		this.bounceHeight = bounceHeight;
		this.splitInto = splitInto;
	}


}

