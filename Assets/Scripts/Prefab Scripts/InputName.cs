using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputName : ControlSetup {

	public string playerNumber;
	public GameObject[] Chars;

	private UnityEngine.UI.Text current;
	private int currentIndex = 0;
	// Use this for initialization

	void Start() {
		SetFocus ();
		setController (playerNumber);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonUp (_horizontal)) {
			SetFocus (Input.GetAxis(_horizontal) > 0 );
		} else if (Input.GetButtonUp (_vertical)) {
			SelectNextLetter (Input.GetAxis(_vertical) < 0 );
		} else if (Input.GetButtonUp(_jump)) {
			SaveName();
		}
	}

	void SetFocus(bool next = false) {
		if (next) {
			currentIndex = currentIndex >= Chars.Length - 1 ? currentIndex : currentIndex + 1;
		} else {
			currentIndex = currentIndex == 0 ? currentIndex : currentIndex - 1;
		}

		current = Chars [currentIndex].GetComponent<UnityEngine.UI.Text> ();
	}

	void SelectNextLetter(bool next){
		byte[] asciiBytes = Encoding.ASCII.GetBytes (current.text);

		if (next) {
			if (asciiBytes [0] == 90) {
				asciiBytes [0] = 65;
			} else {
				asciiBytes [0]++;
			}
		} else { 
			if (asciiBytes [0] == 65) {
				asciiBytes [0] = 90;
			} else {
				asciiBytes [0]--;
			}
		}
		
		current.text = Encoding.ASCII.GetString (asciiBytes);
	}

	void SaveName() {
		Debug.Log ("salvo");
	}
}
