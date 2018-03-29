using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : ControlSetup {

	public string playerNumber;
	Rigidbody2D rb;

	private bool landed = false;

	public void Awake() {
		rb = GetComponent<Rigidbody2D>();
		setController (playerNumber);
	}


	void FixedUpdate () {
		// Fazer a programaão do player
		var horizontalInput = Input.GetAxis(_horizontal);

		if (landed) {
			rb.AddForce (Vector2.right * 10.0f * horizontalInput, ForceMode2D.Force);
			if (Input.GetButtonDown (_jump)) {
				rb.AddForce (Vector2.up * 7f, ForceMode2D.Impulse);
			}
		}



	}

	void OnCollisionEnter2D(Collision2D collider) {
		if (collider.gameObject.tag.Equals ("platform")) {
			landed = true;
		}
	}
}
