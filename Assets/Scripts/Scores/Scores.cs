using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores
{

	public int MaxScoresSaved = 10;

	sealed class Score
	{
		public int _score;
		public string _name;
		public PlayerType _playerType;

		public Score (int score, string name, PlayerType type)
		{
			_score = score;
			_name = name;
			_playerType = type;
		}

	}

	sealed class ScoreTable
	{
		private string header = "Best Scores \n\n";
		private string footer = "\n\n Press any key";
		private string emptyListText = "Be the first to let your mark!";
		private int lineLength = 60;

		public void drawTable ()
		{
			List<Score> lst = new ScoreManagement ().getSavedTopScores ();
			string aux = header; 

			if (lst.Count > 0) {
				int numberOfDots = 0;

				foreach (Score s in lst) {
					numberOfDots = s._name.Length + s._score.ToString ().Length + " as ".Length + s._playerType.ToString ().Length;
					numberOfDots = lineLength - numberOfDots;


					aux += s._name + " as " + s._playerType + new string ('.', numberOfDots) + s._score + "\n";

				}
			} else {
				aux += emptyListText;
			}

			aux += footer;

			var scoreTable = GameObject.Find ("scoreTable");
			scoreTable.GetComponent<UnityEngine.UI.Text> ().text = aux;
		}
	}

	sealed class ScoreManagement : Scores
	{
		public int isInTop10 (int score)
		{
			List<Score> savedTopScores = getSavedTopScores ();

			Debug.Log ("LIST COUNT: " + savedTopScores.Count);
			// If the list is not fullfilled returns the count
			if (savedTopScores.Count == 0) {
				Debug.Log ("Returned COUNT " + savedTopScores.Count);
				return savedTopScores.Count;
			}

			foreach (Score s in savedTopScores) {
				if (s._score < score) {
					return savedTopScores.IndexOf (s);
				}
			}
			return -1;
		}

		public bool setScore (int score, string name, PlayerType playerType)
		{
			List<Score> savedTopScores = getSavedTopScores ();
			foreach (Score s in savedTopScores) {
				if (s._score < score) {
					savedTopScores.Insert (savedTopScores.IndexOf (s), new Score (score, name, playerType));
					break;
				}
			}
			return true;
		}


		/**
		 * Get Scores saved on PlayerPrefs
		 **/
		public List<Score> getSavedTopScores ()
		{
			List<Score> lst = new List<Score> ();

			int i = 1;
			var strAux = "";
			PlayerType pTypeAux;
			while (i < MaxScoresSaved && PlayerPrefs.HasKey ("Scores[" + i + "].name")) {
				strAux = PlayerPrefs.GetString ("Scores[" + i + "].playerType");
				pTypeAux = strAux == PlayerType.Boss.ToString() ? PlayerType.Boss : PlayerType.Employee;
				Score tmp = new Score (
					            PlayerPrefs.GetInt ("Scores[" + i + "].score"),
					            PlayerPrefs.GetString ("Scores[" + i + "].name"),
					            pTypeAux);
				lst.Add (tmp);
			}

			return lst;
		}
	}

	public void drawScoreTable ()
	{
		new ScoreTable ().drawTable ();
	}

	public bool setNewScore (int score, string name, PlayerType playerType)
	{
		return 	new ScoreManagement ().setScore (score, name, playerType);
	}

	public bool testIsInTop10 (int score)
	{
		return new ScoreManagement ().isInTop10 (score) >= 0;
	}
}
