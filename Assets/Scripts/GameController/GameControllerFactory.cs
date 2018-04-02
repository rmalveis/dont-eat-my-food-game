using System.Collections;
using System.Collections.Generic;
using GameController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerFactory : MonoBehaviour {
	public int MaxNumberOfRecords = 10;
	public GameObject InputPrefab;
	// Use this for initialization
	void Awake ()
	{
		//Decides the strategy based on wich scene is current playing.
		Scene currentScene = SceneManager.GetActiveScene ();

		GameControllerStrategies strategy;
		switch (currentScene.name) {
		case "opening": 
			strategy = gameObject.AddComponent<OpeningGameControllerStrategy> () as OpeningGameControllerStrategy;
			break;
		case "ending":
			strategy =  gameObject.AddComponent<EndingGameControllerStrategy> () as EndingGameControllerStrategy;
			break;
		default:
			strategy = gameObject.AddComponent<MainGameControllerStrategy> () as MainGameControllerStrategy;
			break;
		}
		strategy.MaxNumberOfRecords = MaxNumberOfRecords;
		strategy.InputPrefab = InputPrefab;

	}
}
