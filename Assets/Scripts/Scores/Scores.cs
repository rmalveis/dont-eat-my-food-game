using System.Collections.Generic;
using Players;
using UnityEngine;

public static class Scores
{
    public static int MaxScoresSaved = 10;

    private sealed class Score
    {
        public readonly int _score;
        public readonly string _name;
        public readonly PlayerType _playerType;

        public Score(int score, string name, PlayerType type)
        {
            _score = score;
            _name = name;
            _playerType = type;
        }
    }

    private sealed class ScoreTable
    {
        private readonly string _header = "Best Scores \n\n";
        private readonly string _footer = "\n\n Press any key";
        private readonly string _emptyListText = "Be the first to let your mark!";
        private readonly int _lineLength = 60;

        public void DrawTable()
        {
            var lst = new ScoreManagement().getSavedTopScores();
            var aux = _header;

            if (lst.Count > 0)
            {
                foreach (var s in lst)
                {
                    var numberOfDots = s._name.Length + s._score.ToString().Length + " as ".Length +
                                       s._playerType.ToString().Length;
                    numberOfDots = _lineLength - numberOfDots;

                    aux += s._name + " as " + s._playerType + new string('.', numberOfDots) + s._score + "\n";
                }
            }
            else
            {
                aux += _emptyListText;
            }

            aux += _footer;

            var scoreTable = GameObject.Find("scoreTable");
            scoreTable.GetComponent<UnityEngine.UI.Text>().text = aux;
        }
    }

    private sealed class ScoreManagement
    {
        public int IsInTop10(int score)
        {
            var savedTopScores = getSavedTopScores();

            Debug.Log("LIST COUNT: " + savedTopScores.Count);
            // If the list is not fullfilled returns the count
            if (savedTopScores.Count == 0)
            {
                Debug.Log("Returned COUNT " + savedTopScores.Count);
                return savedTopScores.Count;
            }

            foreach (var s in savedTopScores)
            {
                if (s._score < score)
                {
                    return savedTopScores.IndexOf(s);
                }
            }

            return -1;
        }

        public bool SetScore(int score, string name, PlayerType playerType)
        {
            var savedTopScores = getSavedTopScores();
            foreach (var s in savedTopScores)
            {
                if (s._score >= score) continue;
                savedTopScores.Insert(savedTopScores.IndexOf(s), new Score(score, name, playerType));
                break;
            }

            return true;
        }


        /**
     * Get Scores saved on PlayerPrefs
     **/
        public List<Score> getSavedTopScores()
        {
            var lst = new List<Score>();

            var i = 1;
            while (i < MaxScoresSaved && PlayerPrefs.HasKey("Scores[" + i + "].name"))
            {
                var strAux = PlayerPrefs.GetString("Scores[" + i + "].playerType");
                var pTypeAux = strAux == PlayerType.Boss.ToString() ? PlayerType.Boss : PlayerType.Employee;
                var tmp = new Score(
                    PlayerPrefs.GetInt("Scores[" + i + "].score"),
                    PlayerPrefs.GetString("Scores[" + i + "].name"),
                    pTypeAux);
                lst.Add(tmp);
            }

            return lst;
        }
    }

    public static void DrawScoreTable()
    {
        new ScoreTable().DrawTable();
    }

    public static bool SetNewScore(int score, string name, PlayerType playerType)
    {
        return new ScoreManagement().SetScore(score, name, playerType);
    }

    public static bool TestIsInTop10(int score)
    {
        return new ScoreManagement().IsInTop10(score) >= 0;
    }
}