using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private GameControllerStrategies gController;
	public int MaxNumberOfRecords = 0;
	// Use this for initialization
	void Start ()
	{
		gController.Start ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		gController.Update ();
	}

	void Awake ()
	{
		//Decides the strategy based on wich scene is current playing.
		Scene currentScene = SceneManager.GetActiveScene ();

		switch (currentScene.name) {
		case "opening": 
			gController = new OpeningGameControllerStrategy ();
			break;
		case "ending":
			gController = new EndingGameControllerStrategy ();
			break;
		default:
			gController = new MainGameControllerStrategy ();
			break;
		}

		gController.MaxNumberOfRecords = MaxNumberOfRecords;
	}
}
