using UnityEngine;

public class ControlSetup : MonoBehaviour {

	protected string _jump;
	protected string _horizontal;
	protected string _special;
	protected string _vertical;

	protected void setController(string playerNumber) {
		_jump = "AZUL" + playerNumber;
		_horizontal = "HORIZONTAL" + playerNumber;
		_special = "AMARELO" + playerNumber;
		_vertical = "VERTICAL" + playerNumber;
	}

}
