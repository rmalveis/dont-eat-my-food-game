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
        public GameObject SceneHelperObject3;

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
            EventManager.EventManager.CallEnablePowerUp(playerType);
        }

        private void EndGame(PlayerType playerType)
        {
            if (!_gameIsRunning) return;
            _gameIsRunning = false;


            int pointsToSave;
            int lastResultPlayerNumber;
            switch (playerType)
            {
                case PlayerType.Boss:
                    pointsToSave = _employeePlayer.Points;
                    lastResultPlayerNumber = (int) PlayerType.Employee;
                    break;
                case PlayerType.Employee:
                    pointsToSave = _bossPlayer.Points;
                    lastResultPlayerNumber = (int) PlayerType.Boss;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("playerType", playerType, null);
            }

            PlayerPrefs.SetInt("LastResult", pointsToSave);
            PlayerPrefs.SetInt("LastResultPlayerNumber", lastResultPlayerNumber);
            PlayerPrefs.Save();
            SceneManager.LoadScene("ending");
        }

        private void OnEnable()
        {
            EventManager.EventManager.OnCollect += SumUpCollectedItem;
            EventManager.EventManager.OnDeath += EndGame;
            EventManager.EventManager.OnPowerUp += OnPowerUpFired;
        }

        private void OnDisable()
        {
            EventManager.EventManager.OnCollect -= SumUpCollectedItem;
            EventManager.EventManager.OnDeath -= EndGame;
            EventManager.EventManager.OnPowerUp -= OnPowerUpFired;
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
            const string tmplString = "Jogador: <b>{0}</b>\nPontos: <b>{1}</b>\n{2}: <b>{3}</b>";

            _bossText.text = string.Format(tmplString, "Chefe", _bossPlayer.Points,
                "Chamar para o Cafezinho", _bossPlayer.PowerUpEnabled ? "SIM" : "NÃO");
            _employeeText.text = string.Format(tmplString, "Empregado", _employeePlayer.Points,
                "Passar ligação de Cliente", _employeePlayer.PowerUpEnabled ? "SIM" : "NÃO");
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
            SceneHelperObject.SetActive(Input.GetAxis("AZUL0") != 0f || Input.GetAxis("AZUL1") != 0f);
            SceneHelperObject2.SetActive(!SceneHelperObject.active && !SceneHelperObject3.active);

            if (Input.anyKey && Input.GetAxis("AZUL0") == 0f && Input.GetAxis("AZUL1") == 0f)
            {
                SceneHelperObject.SetActive(false);
                SceneHelperObject3.SetActive(true);
                CancelInvoke();
                Invoke("LoadMain", 5);
            }
        }

        private void LoadMain()
        {
            SceneManager.LoadScene("main");
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
            EventManager.EventManager.OnSaveName += SaveData;
        }

        private void OnDisable()
        {
            EventManager.EventManager.OnSaveName -= SaveData;
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
                input.GetComponent<InputName>().PlayerNumber = _winnerPlayerNumber;
            }
        }

        public override void Update()
        {
        }

        private void SaveData(string name)
        {
            if (Scores.SetNewScore(_userScore, name, (PlayerType) _winnerPlayerNumber))
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