using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameControllerStrategies : MonoBehaviour {
	protected Scores scoresObject;

	public int MaxNumberOfRecords;
	public GameObject InputPrefab;

	// Use this for initialization
	public abstract void  Start () ;
	
	// Update is called once per frame
	public abstract void Update () ;
}

// GameController with all needed Code for Main Scene
public class MainGameControllerStrategy : GameControllerStrategies {
	public override void Start() {
		Debug.Log("hi");
	}

	public override void Update() {
		if (Input.anyKey) {
			SceneManager.LoadScene ("ending");
			PlayerPrefs.SetInt ("LastResult", 1000);
			PlayerPrefs.SetInt ("LastResultPlayerNumber", 1);
		}
	}
}

// GameController with all needed Code for Opening Scene
public class OpeningGameControllerStrategy : GameControllerStrategies {
	public override void Start() {
		scoresObject = new Scores ();
		scoresObject.MaxScoresSaved = MaxNumberOfRecords;
		scoresObject.drawScoreTable ();
	}

	public override void Update() {
		if (Input.anyKey) {
			SceneManager.LoadScene ("main");
		}
			
	}
}

// GameController with all needed Code for Ending Scene
public class EndingGameControllerStrategy : GameControllerStrategies {
	List<Scores> lst;
	bool saveNewScore = false;
	string winnerPlayerNumber;

	public override void Start() {
		if (!saveNewScore) {
			SceneManager.LoadScene ("opening");
		} else {
			var input = Instantiate (InputPrefab);
			Debug.Log ("trying: " + winnerPlayerNumber);
			input.GetComponent<InputName> ().playerNumber = winnerPlayerNumber;
		}
	}

	public override void Update() {
		
	}

	void Awake() {
		scoresObject = new Scores ();
		scoresObject.MaxScoresSaved = MaxNumberOfRecords;
		winnerPlayerNumber = PlayerPrefs.GetInt ("LastResultPlayerNumber").ToString();
		Debug.Log ("loaded player number: " + winnerPlayerNumber);
		var lastScore = PlayerPrefs.GetInt ("LastResult");
		Debug.Log ("LastScore:" + lastScore);

		saveNewScore = scoresObject.testIsInTop10 (lastScore);
		Debug.Log ("SaveNewScore:" + saveNewScore);

	}
}