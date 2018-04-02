using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractPlayer {

	// Use this for initialization
	void Start () {
		this.playerType = PlayerType.Boss;
		Debug.Log ("Type: " + playerType);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
