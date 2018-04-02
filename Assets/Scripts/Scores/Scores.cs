using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores {

	public int MaxScoresSaved = 10;

	sealed class Score {
		public int score;
		public string name;

		public Score(int score, string name) {
			this.score = score;
			this.name = name;
		}

	}

	sealed class ScoreTable {
		private string header = "Best Scores \n\n";
		private string footer = "\n\n Press any key";
		private string emptyListText = "Be the first to let your mark!";
		private int lineLength = 60;

		public void drawTable () {
			List<Score> lst = new ScoreManagement ().getSavedTopScores();
			string aux = header; 

			if (lst.Count > 0) {
				int numberOfDots = 0;

				foreach (Score s in lst) {
					numberOfDots = s.name.Length + s.score.ToString ().Length;
					numberOfDots = lineLength - numberOfDots;


					aux += s.name + new string ('.', numberOfDots) + s.score + "\n";

				}
			} else {
				aux += emptyListText;
			}

			aux += footer;

			var scoreTable = GameObject.Find ("scoreTable");
			scoreTable.GetComponent<UnityEngine.UI.Text> ().text = aux;
		}
	}

	sealed class ScoreManagement : Scores {
		public bool setScore (int score, string name)
		{
			List<Score> savedTopScores = getSavedTopScores ();
			foreach (Score s in savedTopScores) {
				if (s.score < score) {
					savedTopScores.Insert(savedTopScores.IndexOf(s), new Score(score, name));
					break;
				}
			}
			return true;
		}


		/**
		 * Get Scores saved on PlayerPrefs
		 **/
		public List<Score> getSavedTopScores() {
			List<Score> lst = new List<Score> ();

			int i = 1;
			while (i < MaxScoresSaved && PlayerPrefs.HasKey ("Scores[" + i + "].name")) {
				Score tmp = new Score (PlayerPrefs.GetInt("Scores[" + i + "].score"), PlayerPrefs.GetString ("Scores[" + i + "].name"));
				lst.Add (tmp);
			}

			return lst;
		}
	}

	public void drawScoreTable() {
		new ScoreTable ().drawTable ();
	}

	public bool setNewScore(int score, string name) {
		return 	new ScoreManagement ().setScore (score, name);
	}
}
