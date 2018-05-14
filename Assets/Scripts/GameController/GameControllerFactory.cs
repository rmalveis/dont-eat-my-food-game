using GameController;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerFactory : MonoBehaviour {
	public GameObject SceneHelperObject;

	public GameObject SceneHelperObject2;
	// Use this for initialization
	void Awake ()
	{
		//Decides the strategy based on wich scene is current playing.
		Scene currentScene = SceneManager.GetActiveScene ();

		GameControllerStrategies strategy;
		switch (currentScene.name) {
		case "opening": 
			strategy = gameObject.AddComponent<OpeningGameControllerStrategy> ();
			break;
		case "ending":
			strategy =  gameObject.AddComponent<EndingGameControllerStrategy> ();
			break;
		default:
			strategy = gameObject.AddComponent<MainGameControllerStrategy> ();
			break;
		}
		strategy.SceneHelperObject = SceneHelperObject;
		strategy.SceneHelperObject2 = SceneHelperObject2;

	}
}
