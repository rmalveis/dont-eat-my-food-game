using UnityEngine;

public class MakeMapsScroll : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D hit)
	{
		EventManager.CallOnHit(hit);
	}

	private void OnTriggerStay2D(Collider2D hit)
	{
		EventManager.CallOnHit(hit);
	}
}
