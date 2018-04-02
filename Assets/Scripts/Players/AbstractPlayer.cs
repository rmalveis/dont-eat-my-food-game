using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : ControlSetup
{

	public string playerNumber;
	Rigidbody2D rb;

	private bool landed = false;

	public void Awake ()
	{
		rb = GetComponent<Rigidbody2D> ();
		setController (playerNumber);
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}


	void FixedUpdate ()
	{
		// Fazer a programaão do player
		var horizontalInput = Input.GetAxis (_horizontal);

		var modifier = landed ? 10.0f : 5.0f;

		rb.AddForce (Vector2.right * modifier * horizontalInput, ForceMode2D.Force);
		if (landed) {
			if (Input.GetButtonDown (_jump)) {
				rb.AddForce (Vector2.up * 7f, ForceMode2D.Impulse);
			}
		}



	}

	void OnCollisionEnter2D (Collision2D collider)
	{
		if (collider.gameObject.tag.Equals ("platform")) {
			
			landed = true;
			Debug.Log ("Landed changed to " + landed);
		}
	}

	void OnCollisionExit2D (Collision2D collider)
	{
		if (collider.gameObject.tag.Equals ("platform")) {
			
			landed = false;
			Debug.Log ("Landed changed to " + landed);
		}
	}
}
