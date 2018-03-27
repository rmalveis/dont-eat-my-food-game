using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : MonoBehaviour {

	public string playerController;

	void FixedUpdate () {

		// Fazer a programaão do player

		Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce (Vector3.forward * 1.0f, ForceMode.Force);
	}
}
