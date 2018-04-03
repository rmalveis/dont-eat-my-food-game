using Players;
using Prefab_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameController
{
    public abstract class GameControllerStrategies : MonoBehaviour
    {
        public GameObject InputPrefab;

        // Use this for initialization
        public abstract void Start();

        // Update is called once per frame
        public abstract void Update();
    }

// GameController with all needed Code for Main Scene
    public class MainGameControllerStrategy : GameControllerStrategies
    {
        public override void Start()
        {
            Debug.Log("hi");
        }

        public override void Update()
        {
            if (!Input.anyKey) return;

            SceneManager.LoadScene("ending");
            var a = Random.Range(10, 1000);
            var b = Random.Range(1, 2);
            Debug.Log("FROM MAIN: result" + a);
            Debug.Log("FROM MAIN: type" + b);

            PlayerPrefs.SetInt("LastResult", a);
            PlayerPrefs.SetInt("LastResultPlayerNumber", b);
            PlayerPrefs.Save();
        }
    }

// GameController with all needed Code for Opening Scene
    public class OpeningGameControllerStrategy : GameControllerStrategies
    {
        public override void Start()
        {
            Scores.DrawScoreTable();
        }

        public override void Update()
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("main");
            }
        }
    }

// GameController with all needed Code for Ending Scene
    public class EndingGameControllerStrategy : GameControllerStrategies
    {
        private bool _saveNewScore;
        private int _winnerPlayerNumber;
        private int _userScore;

        private void OnEnable()
        {
            EventManager.OnSaveName += SaveData;
        }

        private void OnDisable()
        {
            EventManager.OnSaveName -= SaveData;
        }

        public override void Start()
        {
            if (!_saveNewScore)
            {
                SceneManager.LoadScene("opening");
            }
            else
            {
                var input = Instantiate(InputPrefab);
                Debug.Log("trying: " + _winnerPlayerNumber);
                input.GetComponent<InputName>().PlayerNumber = _winnerPlayerNumber.ToString();
            }
        }

        public override void Update()
        {
        }

        private void SaveData(string name)
        {
            Debug.Log("SAVA DATA:" + name);
            if (Scores.SetNewScore(_userScore, name, (PlayerType) _winnerPlayerNumber))
            {
                SceneManager.LoadScene("opening");
            }
        }

        private void Awake()
        {
            _winnerPlayerNumber = PlayerPrefs.GetInt("LastResultPlayerNumber");
            Debug.Log("loaded player number: " + _winnerPlayerNumber);
            _userScore = PlayerPrefs.GetInt("LastResult");
            Debug.Log("LastScore:" + _userScore);

            _saveNewScore = Scores.TestIsInTop10(_userScore);
            Debug.Log("SaveNewScore:" + _saveNewScore);
        }
    }
}