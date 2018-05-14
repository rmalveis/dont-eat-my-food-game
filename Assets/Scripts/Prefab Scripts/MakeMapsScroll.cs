using UnityEngine;

public class MakeMapsScroll : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D hit)
	{
		Debug.Log("Firing OnEnter HIT");
		EventManager.CallOnHit(hit);
	}

	private void OnTriggerStay2D(Collider2D hit)
	{
		Debug.Log("Firing OnStay HIT");
		EventManager.CallOnHit(hit);
	}
}
