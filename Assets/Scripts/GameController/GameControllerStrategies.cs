using System;
using System.Runtime.CompilerServices;
using Players;
using Prefab_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameController
{
    public abstract class GameControllerStrategies : MonoBehaviour
    {
        public GameObject SceneHelperObject;
        public GameObject SceneHelperObject2;

        // Use this for initialization
        public abstract void Start();

        // Update is called once per frame
        public abstract void Update();
    }

// GameController with all needed Code for Main Scene
    public class MainGameControllerStrategy : GameControllerStrategies
    {
        private PlayerGameController _bossPlayer;
        private PlayerGameController _employeePlayer;

        private int _bossLastPowerUpAt = 50;
        private int _employeeLastPowerUpAt = 50;
        private bool _gameIsRunning;
        private Text _bossText;
        private Text _employeeText;

        //RULESET
        private const int SufficientForNextPowerUp = 100;
        private const int CollectiblesPoints = 10;

        private void OnPowerUpFired(PlayerType playerType)
        {
            if (playerType == _bossPlayer.Type)
            {
                _bossPlayer.PowerUpFired();
                return;
            }

            _employeePlayer.PowerUpFired();
        }

        private void SumUpCollectedItem(PlayerType playerType)
        {
            var player = playerType == PlayerType.Boss ? _bossPlayer : _employeePlayer;

            player.Points += CollectiblesPoints;

            if (SufficientForNextPowerUp > player.Points - player.LastPowerUpAt) return;

            player.PowerUpEnabled = true;
            EventManager.CallEnablePowerUp(playerType);
        }

        private void EndGame(PlayerType playerType)
        {
            if (!_gameIsRunning) return;
            _gameIsRunning = false;

            var pointsToSave = playerType == PlayerType.Employee ? _bossPlayer.Points : _employeePlayer.Points;

//            PlayerPrefs.SetInt("LastResult", pointsToSave);
//            PlayerPrefs.SetInt("LastResultPlayerNumber", (int) playerType);
//            PlayerPrefs.Save();
//            SceneManager.LoadScene("ending");
        }

        private void OnEnable()
        {
            EventManager.OnCollect += SumUpCollectedItem;
            EventManager.OnDeath += EndGame;
            EventManager.OnPowerUp += OnPowerUpFired;
        }

        private void OnDisable()
        {
            EventManager.OnCollect -= SumUpCollectedItem;
            EventManager.OnDeath -= EndGame;
            EventManager.OnPowerUp -= OnPowerUpFired;
        }

        public override void Start()
        {
            _bossPlayer = new PlayerGameController(PlayerType.Boss);
            _employeePlayer = new PlayerGameController(PlayerType.Employee);

            _bossText = GameObject.Find("BossText").GetComponent<UnityEngine.UI.Text>();
            _employeeText = GameObject.Find("EmployeeText").GetComponent<UnityEngine.UI.Text>();

            _gameIsRunning = true;
        }

        public override void Update()
        {
            const string tmplString = "Jogador: <b>{0}</b>\nPontos: <b>{1}</b>\nPowerUp Disponível: <b>{2}</b>";

            _bossText.text = string.Format(tmplString, "Chefe", _bossPlayer.Points,
                _bossPlayer.PowerUpEnabled ? "SIM" : "NÃO");
            _employeeText.text = string.Format(tmplString, "Empregado", _employeePlayer.Points,
                _employeePlayer.PowerUpEnabled ? "SIM" : "NÃO");
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
            if (Input.anyKey && Input.GetAxis("AZUL0") == 0f && Input.GetAxis("AZUL1") == 0f)
            {
                SceneManager.LoadScene("main");
            }

            SceneHelperObject.SetActive(Input.GetAxis("AZUL0") != 0f || Input.GetAxis("AZUL1") != 0f);
            SceneHelperObject2.SetActive(Input.GetAxis("AZUL0") == 0f && Input.GetAxis("AZUL1") == 0f);
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
                var input = Instantiate(SceneHelperObject);
                input.GetComponent<InputName>().PlayerNumber = _winnerPlayerNumber.ToString();
            }
        }

        public override void Update()
        {
        }

        private void SaveData(string name)
        {
            if (Scores.SetNewScore(_userScore, name, (PlayerType) _winnerPlayerNumber - 1))
            {
                SceneManager.LoadScene("opening");
            }
        }

        private void Awake()
        {
            _winnerPlayerNumber = PlayerPrefs.GetInt("LastResultPlayerNumber");
            _userScore = PlayerPrefs.GetInt("LastResult");
            _saveNewScore = Scores.TestIsInTop10(_userScore);
        }
    }
}