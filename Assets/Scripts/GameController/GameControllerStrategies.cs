using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameControllerStrategies {
	protected Scores scoresObject;
	public int MaxNumberOfRecords;

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
	public override void Start() {
		Debug.Log("hi");
	}

	public override void Update() {
		Debug.Log("hi");
	}
}