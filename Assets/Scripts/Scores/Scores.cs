using System;
using System.Collections.Generic;
using System.Linq;
using Players;
using UnityEngine;

public static class Scores
{
    private sealed class ScoreItem
    {
        public readonly int Score;
        public readonly string Name;
        public readonly PlayerType PlayerType;

        public ScoreItem(int score, string name, PlayerType type)
        {
            Score = score;
            Name = name;
            PlayerType = type;
        }
    }

    private sealed class ScoreTable
    {
        private readonly string _header = "Melhores Resultados \n\n";
        private readonly string _footer = "\n\n Aperte e segure <b><color=\"#5B6FF4FF\">\"Azul\"</color></b> para <b>Ajuda</b>. \n Qualquer outra tecla para <b>Jogar</b>.";
        private readonly string _emptyListText = "Nenhum! \n\n Seja o Primeiro a deixar sua marca!";
        private readonly int _lineLength = 30;

        public void DrawTable()
        {
            var lst = new ScoreManagement().GetSavedTopScores();
            var aux = _header;

            if (lst.Count > 0)
            {
                foreach (var s in lst)
                {
                    var numberOfDots = s.Name.Length + s.Score.ToString().Length + " as ".Length +
                                       s.PlayerType.ToString().Length;
                    numberOfDots = _lineLength - numberOfDots;

                    aux += s.Name + " as " + s.PlayerType + new string('.', numberOfDots) + s.Score + "\n";
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
        private readonly int MaxScoresSaved = 10;

        public int IsInTop10(int score)
        {
            var savedTopScores = GetSavedTopScores();

            // If the list is not fullfilled returns the count
            if (savedTopScores.Count < MaxScoresSaved)
            {
                return savedTopScores.Count;
            }

            var i = 0;
            while (i < MaxScoresSaved && i < savedTopScores.Count && savedTopScores.Count == MaxScoresSaved)
            {
                var s = savedTopScores[i];
                if (s.Score < score)
                {
                    return savedTopScores.IndexOf(s);
                }

                i++;
            }

            return -1;
        }

        public bool InsertScoreOnList(int score, string name, PlayerType playerType)
        {
            var savedTopScores = GetSavedTopScores();
            var item = new ScoreItem(score, name, playerType);

            if (savedTopScores.Count < MaxScoresSaved)
            {
                savedTopScores.Add(item);
                savedTopScores = savedTopScores.OrderByDescending(s => s.Score).ToList();
                return WriteTopScores(savedTopScores);
            }

            var i = 0;
            while (i < MaxScoresSaved && savedTopScores.Count < MaxScoresSaved && i < savedTopScores.Count)
            {
                var s = savedTopScores[i];
                if (s.Score < score)
                {
                    break;
                }

                i++;
            }

            savedTopScores.Insert(i, item);
            if (savedTopScores.Count > MaxScoresSaved)
            {
                savedTopScores.RemoveAt(savedTopScores.Count - 1);
            }

            savedTopScores = savedTopScores.OrderByDescending(s => s.Score).ToList();
            return WriteTopScores(savedTopScores);
        }

        private bool WriteTopScores(IList<ScoreItem> toSaveScores)
        {
            try
            {
                var i = 0;
                PlayerPrefs.DeleteAll();

                while (i < MaxScoresSaved && i < toSaveScores.Count)
                {
                    PlayerPrefs.SetString("Scores[" + i + "].name", toSaveScores[i].Name);
                    PlayerPrefs.SetString("Scores[" + i + "].playerType", toSaveScores[i].PlayerType.ToString());
                    PlayerPrefs.SetInt("Scores[" + i + "].score", toSaveScores[i].Score);

                    i++;
                }

                PlayerPrefs.Save();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }


        /**
         * Get Scores saved on PlayerPrefs
         **/
        public List<ScoreItem> GetSavedTopScores()
        {
            var lst = new List<ScoreItem>();

            var i = 0;
            while (i < MaxScoresSaved && PlayerPrefs.HasKey("Scores[" + i + "].name"))
            {
                var strAux = PlayerPrefs.GetString("Scores[" + i + "].playerType");
                var pTypeAux = strAux == PlayerType.Boss.ToString() ? PlayerType.Boss : PlayerType.Employee;
                var tmp = new ScoreItem(
                    PlayerPrefs.GetInt("Scores[" + i + "].score"),
                    PlayerPrefs.GetString("Scores[" + i + "].name"),
                    pTypeAux);
                lst.Add(tmp);
                i++;
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
        return new ScoreManagement().InsertScoreOnList(score, name, playerType);
    }

    public static bool TestIsInTop10(int score)
    {
        return new ScoreManagement().IsInTop10(score) >= 0;
    }
}