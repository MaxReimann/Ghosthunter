using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

class HighScore  : IComparable<HighScore>{
	public string name;
	public string date;
	public int score;

	public HighScore(string _name, string _date, int _score)
	{
		this.name = _name;
		this.date = _date;
		this.score = _score;
	}

	public int CompareTo(HighScore score2)
	{
		return this.score.CompareTo(score2.score);
	}

	public bool isSame(HighScore score2){
		return (name == score2.name && 
			date == score2.date &&
			score == score2.score);
	}
			
			
			
}



public class HighscoreController : MonoBehaviour {

	public GameObject nameField;
	public GameObject dateField;
	public GameObject scoreField;

	public int maxCount = 10;

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey ("highscores"))
			return;


		if (nameField == null || scoreField == null || dateField == null)
			Debug.LogError("set the fields of highscore controller with fields");

		UnityEngine.UI.Text nameText = nameField.GetComponent<UnityEngine.UI.Text> ();
		UnityEngine.UI.Text dateText = dateField.GetComponent<UnityEngine.UI.Text> ();
		UnityEngine.UI.Text scoreText = scoreField.GetComponent<UnityEngine.UI.Text> ();

		string highscores = PlayerPrefs.GetString ("highscores");
		highscores = highscores.Trim ();
		string[] lines = highscores.Split('\n');

		List<HighScore> scores = new List<HighScore>(); 

		foreach (string line in lines) {
			string[] split = line.Split(';');
			scores.Add(new HighScore(split[0],split[1], Int32.Parse(split[2])));
		}


		scores.Sort ();
		scores.Reverse (); // starting with biggest

		HighScore mostRecent = mostRecentScore (scores);

		int count = 0;
		foreach (HighScore score in scores) {
			if (++count >= maxCount)
				break;

			if (mostRecent.isSame (score)) {
				nameText.text += "<color=orange>";
				dateText.text += "<color=orange>";
				scoreText.text += "<color=orange>";
			}
				
			nameText.text = nameText.text + score.name + "\n";
			dateText.text = dateText.text + score.date + "\n";
			scoreText.text = scoreText.text + score.score + "\n";

			if (mostRecent.isSame (score)) {
				nameText.text += "</color>";
				dateText.text += "</color>";
				scoreText.text += "</color>";
			}
		}
	}

	private HighScore mostRecentScore(List<HighScore> scores) 
	{
		long mostRecentTime = 0;
		HighScore maxScore = null;
		foreach (HighScore score in scores) { 
			DateTime date = DateTime.ParseExact (score.date, "dd/MM/yyyy HH:mm:ss",
			                                    System.Globalization.CultureInfo.InvariantCulture);
			if (date.Ticks > mostRecentTime) {
				mostRecentTime = date.Ticks;
				maxScore = score;
			}

		}

		return maxScore;
	}
	
				

	
	// Update is called once per frame
	void Update () {
	
	}
}
