using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSetup : MonoBehaviour {

	protected string _jump;
	protected string _horizontal;
	protected string _especial;
	protected string _vertical;

	protected void setController(string playerNumber) {
		_jump = "jump-p" + playerNumber;
		_horizontal = "horizontal-p" + playerNumber;
		_especial = "especial-p" + playerNumber;
		_vertical = "vertical-p" + playerNumber;
	}

}
