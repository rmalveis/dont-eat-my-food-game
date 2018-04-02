using System.Linq;
using System.Text;
using UnityEngine;

namespace Prefab_Scripts
{
	public class InputName : ControlSetup {

		public string PlayerNumber = "1";
		public GameObject[] Chars;
		public GameObject InputYourNameObject;

		private UnityEngine.UI.Text _current;
		private int _currentIndex;
		// Use this for initialization

		private void Start() {
			SetFocus ();
			setController (PlayerNumber);
			InputYourNameObject.GetComponent<UnityEngine.UI.Text>().text =
				InputYourNameObject.GetComponent<UnityEngine.UI.Text>().text + " (player " + PlayerNumber + ")";
		}
	
		// Update is called once per frame
		private void Update () {

			if (Input.GetButtonUp (_horizontal)) {
				SetFocus (Input.GetAxis(_horizontal) > 0 );
			} else if (Input.GetButtonUp (_vertical)) {
				SelectNextLetter (Input.GetAxis(_vertical) < 0 );
			} else if (Input.GetButtonUp(_jump)) {
				SaveName();
			}
		}

		private void SetFocus(bool next = false) {
			if (next) {
				_currentIndex = _currentIndex >= Chars.Length - 1 ? _currentIndex : _currentIndex + 1;
			} else {
				_currentIndex = _currentIndex == 0 ? _currentIndex : _currentIndex - 1;
			}

			_current = Chars [_currentIndex].GetComponent<UnityEngine.UI.Text> ();
		}

		private void SelectNextLetter(bool next){
			var asciiBytes = Encoding.ASCII.GetBytes (_current.text);

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
		
			_current.text = Encoding.ASCII.GetString (asciiBytes);
		}

		private void SaveName()
		{
			var name = Chars.Aggregate("", (current, c) => current + c.GetComponent<UnityEngine.UI.Text>().text);
			EventManager.CallOnSave(name);
		}
	}
}
